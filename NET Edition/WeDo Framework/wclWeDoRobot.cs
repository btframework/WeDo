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
using System.Linq;
using System.Collections.Specialized;

namespace wclWeDoFramework
{
    /// <summary> The <c>OnHubConnected</c> event handler prototype. </summary>
    /// <param name="Sender"> The object that fired the event. </param>
    /// <param name="Hub"> The WeDo HUB object that has been connected. </param>
    /// <param name="Error"> The connection result. If the connection completed with success the
    ///    <c>Error</c> value is <c>WCL_E_SUCCESS</c>. </param>
    /// <seealso cref="wclWeDoHub"/>
    public delegate void wclWeDoRobotHubConnectedEvent(Object Sender, wclWeDoHub Hub, Int32 Error);
    /// <summary> The <c>OnHubDisconnected</c> event handler prototype. </summary>
    /// <param name="Sender"> The object that fired the event. </param>
    /// <param name="Hub"> The WeDo HUB object that has been disconnected. </param>
    /// <param name="Reason"> The disconnection reason code. </param>
    /// <seealso cref="wclWeDoHub"/>
    public delegate void wclWeDoRobotHubDisconnectedEvent(Object Sender, wclWeDoHub Hub,
        Int32 Reason);
    /// <summary> The <c>OnHubFound</c> event handler prototype. </summary>
    /// <param name="Sender"> The object that fired the event. </param>
    /// <param name="Address"> The HUD address. </param>
    /// <param name="Name"> The HUB name. </param>
    /// <param name="Connect"> An application sets this boolean value to <c>true</c> to
    ///   accept connection to this HUB. Set this parameter to <c>false</c> to ignore HUB. </param>
    public delegate void wclWeDoRobotHubFoundEvent(Object Sender, Int64 Address, String Name,
        out Boolean Connect);

    /// <summary> The class represents a WeDo Robot.  </summary>
    /// <remarks> The WeDo Robot is the class that combines all the WeDo Framework features into
    ///   a single place. It allows to work with more then single Hub and hides all the
    ///   Bluetooth Framework preparation steps required to discover WeDo Hubs and to work with
    ///   them.</remarks>
    public class wclWeDoRobot
    {
        private Boolean FDisposed;
        private Dictionary<Int64, wclWeDoHub> FHubs;
        private wclBluetoothManager FManager;
        private wclBluetoothRadio FRadio;
        private wclWeDoWatcher FWatcher;

        private void HubConnected(Object Sender, Int32 Error)
        {
            wclWeDoHub Hub = (wclWeDoHub)Sender;
            DoHubConnected(Hub, Error);
            if (Error != wclErrors.WCL_E_SUCCESS)
                FHubs.Remove(Hub.Address);
        }

        private void HubDisconnected(Object Sender, Int32 Reason)
        {
            wclWeDoHub Hub = (wclWeDoHub)Sender;
            if (FHubs.ContainsKey(Hub.Address))
            {
                FHubs.Remove(Hub.Address);
                DoHubDisconnected(Hub, Reason);
            }
        }

        private void WatcherHubFound(Object Sender, Int64 Address, String Name)
        {
            // First, check that we are not connected to the HUB.
            if (!FHubs.ContainsKey(Address))
            {
                // Query application about interest of this HUB.
                Boolean Connect;
                DoHubFound(Address, Name, out Connect);
                if (Connect)
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

        private void WatcherStarted(Object sender, EventArgs e)
        {
            DoStarted();
        }

        /// <summary> Fires the <c>OnHubConnected</c> event. </summary>
        /// <param name="Hub"> The WeDo HUB object that has been connected. </param>
        /// <param name="Error"> The connection result code. If connection completed with success the
        ///   value is<c>WCL_E_SUCCESS</c>. </param>
        /// <seealso cref="wclWeDoHub"/>
        protected virtual void DoHubConnected(wclWeDoHub Hub, Int32 Error)
        {
            if (OnHubConnected != null)
                OnHubConnected(this, Hub, Error);
        }

        /// <summary> Fires the <c>OnHubDisconnected</c> event. </summary>
        /// <param name="Hub"> The WeDo Hub that just disconnected. </param>
        /// <param name="Reason"> The disconnection reason code. </param>
        protected virtual void DoHubDisconnected(wclWeDoHub Hub, Int32 Reason)
        {
            if (OnHubDisconnected != null)
                OnHubDisconnected(this, Hub, Reason);
        }

        /// <summary> Fires the <c>OnHubFound</c> event </summary>
        /// <param name="Address"> The HUD address. </param>
        /// <param name="Name"> The HUB name. </param>
        /// <param name="Connect"> An application sets this boolean value to <c>true</c> to
        ///   accept connection to this HUB. Set this parameter to <c>false</c> to ignore HUB. </param>
        protected virtual void DoHubFound(Int64 Address, String Name, out Boolean Connect)
        {
            Connect = false;
            if (OnHubFound != null)
                OnHubFound(this, Address, Name, out Connect);
        }

        /// <summary> Fires the <c>OnStarted</c> event. </summary>
        protected virtual void DoStarted()
        {
            if (OnStarted != null)
                OnStarted(this, EventArgs.Empty);
        }

        private void WatcherStopped(object sender, EventArgs e)
        {
            DoStopped();
        }

        /// <summary> Fires the <c>OnStopped</c> event. </summary>
        protected virtual void DoStopped()
        {
            if (OnStopped != null)
                OnStopped(this, EventArgs.Empty);
        }

        /// <summary> Creates new instance of the WeDo Robot class. </summary>
        public wclWeDoRobot()
        {
            FDisposed = false;
            FHubs = new Dictionary<Int64, wclWeDoHub>();
            FManager = new wclBluetoothManager();
            FRadio = null;
            FWatcher = new wclWeDoWatcher();

            FWatcher.OnHubFound += WatcherHubFound;
            FWatcher.OnStarted += WatcherStarted;
            FWatcher.OnStopped += WatcherStopped;

            OnHubFound = null;

            OnHubConnected = null;
            OnHubDisconnected = null;
            OnHubFound = null;
            OnStarted = null;
            OnStopped = null;
        }

        /// <summary> Starts connection to the WeDo Hubs. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <remarks> The method starts searching for WeDo Hubs and to connect to each found. Once the Hub found
        ///   the <c>OnHubFound</c> event fires. An application may accept connection to this Hub by setting
        ///   the <c>Connect</c> parameter to <c>true</c>. </remarks>
        public Int32 Start()
        {
            if (FDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (FRadio != null)
                return wclConnectionErrors.WCL_E_CONNECTION_ACTIVE;

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

        /// <summary> Stops the WeDo Robot. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 Stop()
        {
            if (FDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (FRadio == null)
                return wclConnectionErrors.WCL_E_CONNECTION_NOT_ACTIVE;

            FWatcher.Stop();
            while (FHubs.Count > 0)
                FHubs.Values.ElementAt(0).Disconnect();
            FManager.Close();
            FRadio = null;

            return wclErrors.WCL_E_SUCCESS;
        }

        /// <summary> Gets the class state. </summary>
        /// <value> <c>True</c> if connection is running. <c>False</c> otherwise. </value>
        public Boolean Active
        {
            get
            {
                if (FDisposed)
                    throw new ObjectDisposedException(GetType().FullName);

                return FRadio != null;
            }
        }

        /// <summary> Gets list of the connected WeDo Hubs. </summary>
        /// <value> The list of the WeDo Hubs. </value>
        /// <seealso cref="wclWeDoHub"/>
        public List<wclWeDoHub> Hubs
        {
            get
            {
                if (FDisposed)
                    throw new ObjectDisposedException(GetType().FullName);

                return FHubs.Values.ToList<wclWeDoHub>();
            }
        }

        /// <summary> The event fires when connection operation has been completed. </summary>
        /// <seealso cref="wclWeDoRobotHubConnectedEvent"/>
        public event wclWeDoRobotHubConnectedEvent OnHubConnected;
        /// <summary> The event fires when the WeDo Hub has been disconnected. </summary>
        /// <seealso cref="wclWeDoRobotHubDisconnectedEvent"/>
        public event wclWeDoRobotHubDisconnectedEvent OnHubDisconnected;
        /// <summary> The event fires when new WeDo HUB found. </summary>
        /// <seealso cref="wclWeDoRobotHubFoundEvent"/>
        public event wclWeDoRobotHubFoundEvent OnHubFound;
        /// <summary> The event fires when search and connect to found WeDo Hubs has been started. </summary>
        public event EventHandler OnStarted;
        /// <summary> The event firs when search and connect to the WeDo Hubs stopped. </summary>
        public event EventHandler OnStopped;
    };
}
