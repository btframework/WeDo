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
using System.Collections.Generic;

using wclCommon;
using wclCommunication;
using wclBluetooth;

namespace wclWeDoFramework
{
    /// <summary> The <c>OnHubConnected</c> event handler prototype. </summary>
    /// <param name="Sender"> The object that fired the event. </param>
    /// <param name="Hub"> The WeDo HUB object that has been connected. </param>
    /// <seealso cref="wclWeDoHub"/>
    public delegate void wclWeDoRobotHubConnectedEvent(Object Sender, wclWeDoHub Hub);
    /// <summary> The <c>OnHubDisconnected</c> event handler prototype. </summary>
    /// <param name="Sender"> The object that fired the event. </param>
    /// <param name="Hub"> The WeDo HUB object that has been disconnected. </param>
    /// <param name="Reason"> The disconnection reason code. </param>
    /// <seealso cref="wclWeDoHub"/>
    public delegate void wclWeDoRobotHubDisconnectedEvent(Object Sender, wclWeDoHub Hub,
        Int32 Reason);

    /// <summary> The class represents a WeDo Robot.  </summary>
    /// <remarks> The WeDo Robot is the class that combines all the WeDo Framework features into
    ///   a single place. It allows to work with more thean single Hub and hides all the
    ///   Bluetooth Framework prepartion steps required todiscover WeDo Hubs and to work with
    ///   them.</remarks>
    public sealed class wclWeDoRobot
    {
        private List<Int64> FAddresses;
        private Boolean FDisposed;
        private Dictionary<Int64, wclWeDoHub> FHubs;
        private wclBluetoothManager FManager;
        private wclBluetoothRadio FRadio;
        private wclWeDoWatcher FWatcher;

        private void WatcherHubFound(Object Sender, Int64 Address, String Name)
        {
            // First, check that we are not connected to the HUB.
            if (!FHubs.ContainsKey(Address))
            {
                // Now make sure we are interested in this HUB.
                if (FAddresses.Count == 0 || FAddresses.Contains(Address))
                {
                    // Prepare HUB object.
                    wclWeDoHub Hub = new wclWeDoHub();
                    // Setup HUB events.
                    Hub.OnConnected += HubConnected;
                    Hub.OnDisconnected += HubDisconnected;

                    // Try to connect to the given HUB.
                    if (Hub.Connect(FRadio, Address) == wclErrors.WCL_E_SUCCESS)
                        // If operation started with success add HUB to the list to prevent
                        // From adding it one more time.
                        FHubs.Add(Address, Hub);
                }
            }
        }

        private void HubDisconnected(Object Sender, Int32 Reason)
        {
            // Simple remove HUb from the list and fire the event.
            Int64 Address = ((wclWeDoHub)Sender).Address;
            if (FHubs.ContainsKey(Address))
            {
                FHubs.Remove(Address);
                if (OnHubDisconnected != null)
                    OnHubDisconnected(this, (wclWeDoHub)Sender, Reason);
            }
        }

        private void HubConnected(Object Sender, Int32 Error)
        {
            // If connection was success we can add HUB into the list and fire the event.
            if (Error == wclErrors.WCL_E_SUCCESS)
            {
                FHubs.Add(((wclWeDoHub)Sender).Address, (wclWeDoHub)Sender);
                if (OnHubConnected != null)
                    OnHubConnected(this, (wclWeDoHub)Sender);
            }
        }

        /// <summary> Creates new instance of the WeDo Robot class. </summary>
        public wclWeDoRobot()
        {
            FAddresses = new List<Int64>();
            FDisposed = false;
            FHubs = new Dictionary<Int64, wclWeDoHub>();
            FManager = new wclBluetoothManager();
            FRadio = null;
            FWatcher = new wclWeDoWatcher();

            // WeDoWatcher events
            FWatcher.OnHubFound += WatcherHubFound;

            OnHubConnected = null;
            OnHubDisconnected = null;
        }

        /// <summary> Connects to specified HUBs. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 Connect()
        {
            if (FDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (FRadio != null)
                return wclConnectionErrors.WCL_E_CONNECTION_ACTIVE;

            // Now try to open Bluetooth Manager.
            Int32 Res = FManager.Open();
            if (Res == wclErrors.WCL_E_SUCCESS)
            {
                if (FManager.Count == 0)
                    Res = wclBluetoothErrors.WCL_E_BLUETOOTH_API_NOT_FOUND;
                else
                {
                    // Look for first available radio.
                    for (int i = 0; i < FManager.Count; i++)
                    {
                        if (FManager[i].Available)
                        {
                            FRadio = FManager[i];
                            break;
                        }
                    }

                    if (FRadio == null)
                        Res = wclBluetoothErrors.WCL_E_BLUETOOTH_RADIO_UNAVAILABLE;
                    else
                    {
                        // Try to start watching for HUBs.
                        Res = FWatcher.Start(FRadio);

                        // If something went wrong we must clear the working radio objecy.
                        if (Res != wclErrors.WCL_E_SUCCESS)
                            FRadio = null;
                    }
                }

                // If something went wrong we must close Bluetooth Manager
                if (Res != wclErrors.WCL_E_SUCCESS)
                    FManager.Close();
            }
            return Res;
        }

        /// <summary> Disconnects from  all connected HUBs. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 Disconnect()
        {
            if (FDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (FRadio == null)
                return wclConnectionErrors.WCL_E_CONNECTION_NOT_ACTIVE;

            // First, stop watcher to prevent from connecting other devices.
            FWatcher.Stop();
            // Disconnect all HUBs.
            while (FHubs.Count > 0)
                FHubs[0].Disconnect();
            // Close Bluetooth Manager.
            FManager.Close();
            // And cleanup radio.
            FRadio = null;

            return wclErrors.WCL_E_SUCCESS;
        }

        /// <summary> Gets the list of required addresses. </summary>
        /// <value> The list of HUBs MACs. </value>
        public List<Int64> Addresses {  get { return FAddresses; } }
        /// <summary> Gets the class state. </summary>
        /// <value> <c>True</c> if connection is running. <c>False</c> otherwise. </value>
        public Boolean Connected { get { return FRadio != null; } }
        /// <summary> Gets the list of connected HUBs. </summary>
        /// <value> The list of connected HUBs. </value>
        public Dictionary<Int64, wclWeDoHub> Hubs { get { return FHubs; } }

        /// <summary> The event fires when new found WeDo HUB has just been connected. </summary>
        /// <seealso cref="wclWeDoRobotHubConnectedEvent"/>
        public event wclWeDoRobotHubConnectedEvent OnHubConnected;
        /// <summary> The event fires when HUB has been disconnected. </summary>
        /// <seealso cref="wclWeDoRobotHubDisconnectedEvent"/>
        public event wclWeDoRobotHubDisconnectedEvent OnHubDisconnected;
    };
}
