////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//   Wireless Communication Library 7                                         //
//                                                                            //
//   Copyright (C) 2006-2019 Mike Petrichenko                                 //
//                           Soft Service Company                             //
//                           All Rights Reserved                              //
//                                                                            //
//   http://www.btframework.com                                               //
//                                                                            //
//   support@btframework.com                                                  //
//   shop@btframework.com                                                     //
//                                                                            //
// -------------------------------------------------------------------------- //
//                                                                            //
//   WCL Bluetooth Framework: Lego WeDo 2.0 Education Extension.              //
//                                                                            //
//     https://github.com/btframework/WeDo                                    //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Timers;
using System.Collections.Generic;

using wclCommon;
using wclBluetooth;

namespace wclWeDoFramework
{
    /// <summary> The <c>OnHubFound</c> and <c>OnHubLost</c> events handler
    ///   prototype. </summary>
    /// <param name="Sender"> The object that fired the event. </param>
    /// <param name="Address"> The WeDo Hub MAC address. </param>
    /// <param name="Name"> The WeDo Hub name. </param>
    public delegate void wclWeDoHubAppearanceEvent(Object Sender, Int64 Address, String Name);
    /// <summary> The <c>OnHunNameChanged</c> event handler prototype. </summary>
    /// <param name="Sender"> The object that fired the event. </param>
    /// <param name="Address"> The WeDo device's MAC address. </param>
    /// <param name="OldName"> The old name of WeDo Hub. </param>
    /// <param name="NewName"> The new name of WeDo Hub. </param>
    public delegate void wclWeDoHubNameChangedEvent(Object Sender, Int64 Address,
        String OldName, String NewName);

    /// <summary> The class used to search WeDo devices. </summary>
    public class wclWeDoWatcher
    {
        // Structure used internally to monitor WeDo Hubs.
        private struct WeDoHub
        {
            // Devices name.
            public String Name;
            // Last seen.
            public DateTime Timestamp;
        };

        private const Byte WEDO_LOST_TIMER_MESSAGE_ID = 1;

        private class WeDoLostTimerMessage : wclMessage
        {
            public WeDoLostTimerMessage()
                : base(WEDO_LOST_TIMER_MESSAGE_ID, wclMessage.WCL_MSG_CATEGORY_USER)
            {
            }
        };

        // WeDo advertises this service UUID so we look for it to identify WeDo device.
        private static Guid WEDO_ADVERTISING_SERVICE = new Guid("00001523-1212-efde-1523-785feabcd123");

        private Dictionary<Int64, WeDoHub?> FHubs;
        private wclMessageReceiver FReceiver;
        private Timer FTimer;
        private wclBluetoothLeBeaconWatcher FWatcher;

        private void WatcherAdvertisementUuidFrame(Object Sender, Int64 Address, Int64 Timestamp,
            SByte Rssi, Guid Uuid)
        {
            // WeDo advertises its service's UUID so we can check that this advertisement from WeDo HUB.
            if (Uuid == WEDO_ADVERTISING_SERVICE)
            {
                // Make sure it was not found early.
                if (!FHubs.ContainsKey(Address))
                    // If not - add new HUB to the list. But do not fire event yest because
                    // we do not know device's name.
                    FHubs.Add(Address, null);
            }
        }

        private void WatcherAdvertisementFrameInformation(Object Sender, Int64 Address, Int64 Timestamp, SByte Rssi,
            String Name, wclBluetoothLeAdvertisementType PacketType, wclBluetoothLeAdvertisementFlag Flags)
        {
            // If we get device's name in this advertisement.
            if (Name != null && Name != "")
            {
                // Check that this is one of previously found WeDo HUB.
                if (FHubs.ContainsKey(Address))
                {
                    if (FHubs[Address] == null)
                        // If the device was just found, fire the OnFoudn event.
                        DoHubFound(Address, Name);
                    else
                    {
                        // Have the name been changed?
                        if (FHubs[Address].Value.Name != Name)
                            DoHubNameChanged(Address, FHubs[Address].Value.Name, Name);
                    }

                    // Now update its timestamp., Cause we use Distionary we have to update value with
                    // new record!
                    WeDoHub WeDo = new WeDoHub();
                    WeDo.Name = Name;
                    WeDo.Timestamp = DateTime.Now;
                    FHubs[Address] = WeDo;
                }
            }
        }

        private void WatcherStarted(object sender, EventArgs e)
        {
            // Fire the started event.
            DoStarted();
            // And start timer.
            FTimer.Start();
        }

        private void WatcherStopped(object sender, EventArgs e)
        {
            // Stop timer.
            FTimer.Stop();
            // Do not forget to clear the found devices list!
            FHubs.Clear();
            // And now fire the event.
            DoStopped();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Cause this event fires in separate thread we have to use
            // WCL messaging to notify main thread about timer event.
            WeDoLostTimerMessage Message = new WeDoLostTimerMessage();
            try
            {
                FReceiver.Post(Message);
            }
            finally
            {
                Message.Release();
            }
        }

        private void ReceiverMessage(wclMessage Message)
        {
            // If timer is still active.
            if (FTimer.Enabled)
            {
                // All we have to do here is check each device in list that it is
                // still available.
                DateTime Now = DateTime.Now;
                List<Int64> ToRemove = new List<Int64>();
                foreach (KeyValuePair<Int64, WeDoHub?> Hub in FHubs)
                {
                    if (Hub.Value != null)
                    {
                        if (Hub.Value.Value.Timestamp.AddSeconds(3) < Now)
                            ToRemove.Add(Hub.Key);
                    }
                }

                // Remove disappered devices.
                foreach (Int64 Address in ToRemove)
                {
                    String Name = FHubs[Address].Value.Name;
                    FHubs.Remove(Address);
                    DoHubLost(Address, Name);
                }
            }
        }

        /// <summary> Fires the <c>OnHubFound</c> event. </summary>
        /// <param name="Address"> The Hub's MAC. </param>
        /// <param name="Name"> The WeDo Hub name. </param>
        protected virtual void DoHubFound(Int64 Address, String Name)
        {
            if (OnHubFound != null)
                OnHubFound(this, Address, Name);
        }

        /// <summary> Fires the <c>OnHubLost</c> event. </summary>
        /// <param name="Address"> The Hub's MAC. </param>
        /// <param name="Name"> The WeDo Hub name. </param>
        protected virtual void DoHubLost(Int64 Address, String Name)
        {
            if (OnHubLost != null)
                OnHubLost(this, Address, Name);
        }

        /// <summary> Fires the <c>OnNameChanged</c> event. </summary>
        /// <param name="Address"> The WeDo device's MAC address. </param>
        /// <param name="OldName"> The old name of WeDo Hub. </param>
        /// <param name="NewName"> The new name of WeDo Hub. </param>
        protected virtual void DoHubNameChanged(Int64 Address, String OldName, String NewName)
        {
            if (OnHubNameChanged != null)
                OnHubNameChanged(this, Address, OldName, NewName);
        }

        /// <summary> Fires the <c>OnStarte</c> event. </summary>
        protected virtual void DoStarted()
        {
            if (OnStarted != null)
                OnStarted(this, EventArgs.Empty);
        }

        /// <summary> Fires the <c>OnStopped</c> event. </summary>
        protected virtual void DoStopped()
        {
            if (OnStopped != null)
                OnStopped(this, EventArgs.Empty);
        }

        /// <summary> Creates new WeDo Watcher object. </summary>
        public wclWeDoWatcher()
        {
            // Create Disctionary that will be used to store found devices.
            FHubs = new Dictionary<Int64, WeDoHub?>();

            // Create beacon watcher to monitor WeDo Hubs.
            FWatcher = new wclBluetoothLeBeaconWatcher();
            FWatcher.OnAdvertisementUuidFrame += WatcherAdvertisementUuidFrame;
            FWatcher.OnAdvertisementFrameInformation += WatcherAdvertisementFrameInformation;
            FWatcher.OnStarted += WatcherStarted;
            FWatcher.OnStopped += WatcherStopped;

            // We also need timer that allows to checkif device is still available.
            FTimer = new Timer();
            // Check that devide is available during 3 seconds. If it did not update
            // information - it is disappeared.
            FTimer.Interval = 3000;
            FTimer.AutoReset = true;
            FTimer.Elapsed += TimerElapsed;

            // We need message receiver to process messages from timer.
            FReceiver = new wclMessageReceiver();
            FReceiver.OnMessage += ReceiverMessage;

            OnHubFound = null;
            OnHubLost = null;
            OnHubNameChanged = null;
            OnStarted = null;
            OnStopped = null;
        }

        /// <summary> Starts watching (discovering) for WeDo devices. </summary>
        /// <param name="Radio"> The <see cref="wclBluetoothRadio" /> object that should be used
        ///   for executing Bluetooth LE discovering. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seealso cref="wclBluetoothRadio" />
        public Int32 Start(wclBluetoothRadio Radio)
        {
            // First, try to open message receiver.
            Int32 Res = FReceiver.Open();
            if (Res == wclErrors.WCL_E_SUCCESS)
            {
                // Now try to start watcher.
                Res = FWatcher.Start(Radio);
                //if failed we must close message receiver.
                if (Res != wclErrors.WCL_E_SUCCESS)
                    FReceiver.Close();
            }
            return Res;
        }

        /// <summary> Stops discovering WeDo devices. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 Stop()
        {
            // Close message receiver.
            FReceiver.Close();
            // And stop watcher.
            return FWatcher.Stop();
        }

        /// <summary> Gets the WeDo Hub Watcher state. </summary>
        /// <value> Returns <c>true</c> if Watcher is running (searching for WeDo Hubs).
        ///   Returns <c>false</c> otherwise. </value>
        public Boolean Active { get { return FWatcher.Monitoring; } }

        /// <summary> Gets the <see cref="wclBluetoothRadio"/> object that is used for searching WeDo hubs. </summary>
        /// <value> If Watcher is searching returns the <see cref="wclBluetoothRadio"/> object used for
        ///   searching. If the Watcher is not active returns <c>nil</c>. </value>
        /// <seealso cref="wclBluetoothRadio"/>
        public wclBluetoothRadio Radio
        {
            get
            {
                if (FWatcher.Monitoring)
                    return FWatcher.Radio;
                else
                    return null;
            }
        }

        /// <summary> The event fires when new WeDo Hub has been found. </summary>
        /// <seealso cref="wclWeDoHubAppearanceEvent" />
        public event wclWeDoHubAppearanceEvent OnHubFound;
        /// <summary> The event fires when new WeDo Hub has been lost. </summary>
        /// <seealso cref="wclWeDoHubAppearanceEvent" />
        public event wclWeDoHubAppearanceEvent OnHubLost;
        /// <summary> The event fires when name of a WeDo Hub has been changed. </summary>
        /// <seealso cref="wclWeDoHubNameChangedEvent"/>
        public event wclWeDoHubNameChangedEvent OnHubNameChanged;
        /// <summary> The event fires when discovering has been started. </summary>
        /// <seealso cref="EventHandler" />
        public event EventHandler OnStarted;
        /// <summary> The event fires when discovering has been stopped. </summary>
        /// <seealso cref="EventHandler" />
        public event EventHandler OnStopped;
    };
}
