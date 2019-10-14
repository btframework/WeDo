using System;
using System.Collections.Generic;

using wclCommon;
using wclCommunication;
using wclBluetooth;

namespace wclWeDoFramework
{
    /// <summary> The class represents a WeDo Hub hardware. </summary>
    public class wclWeDoHub
    {
        private wclGattClient FClient;
        private Boolean FConnected;
        private List<wclWeDoIo> FDevices;
        private Boolean FHubConnected;

        // Hub GATT services.
        private wclWeDoDeviceInformationService FDeviceInformation;
        private wclWeDoBatteryLevelService FBatteryLevel;
        private wclWeDoIoService FIo;
        private wclWeDoHubService FHub;

        // Detaches all devices when Hub disconnected.
        private void DetachDevices()
        {
            foreach (wclWeDoIo Device in FDevices)
            {
                Device.Detach();
                DoDeviceDetached(Device);
            }
            FDevices.Clear();
        }

        // Disconnect from all WeDo services.
        private void DisconnectServices()
        {
            if (FDeviceInformation.Connected)
                FDeviceInformation.Disconnect();
            if (FBatteryLevel.Connected)
                FBatteryLevel.Disconnect();
            if (FIo.Connected)
                FIo.Disconnect();
            if (FHub.Connected)
                FHub.Disconnect();
        }

        private void DisconnectHub()
        {
            if (FHubConnected)
            {
                // Detach all attached devices.
                DetachDevices();

                // We have to release all services.
                DisconnectServices();

                FHubConnected = false;
            }
        }

        // GATT client connect event handler.
        private void ClientConnect(object Sender, int Error)
        {
            // If connection was not established simple fire the event with error code.
            if (Error != wclErrors.WCL_E_SUCCESS)
                DoConnected(Error);
            else
            {
                FHubConnected = true;

                // Read services. We do it only once to save connection time.
                wclGattService[] Services;
                Int32 Res = FClient.ReadServices(wclGattOperationFlag.goNone, out Services);
                if (Res == wclErrors.WCL_E_SUCCESS)
                {
                    if (Services == null)
                        Res = wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;
                    else
                    {
                        // Try to connect to WeDo services.
                        Res = FDeviceInformation.Connect(Services);
                        if (Res == wclErrors.WCL_E_SUCCESS)
                            Res = FBatteryLevel.Connect(Services);
                        if (Res == wclErrors.WCL_E_SUCCESS)
                            Res = FIo.Connect(Services);
                        if (Res == wclErrors.WCL_E_SUCCESS)
                            Res = FHub.Connect(Services);

                        Services = null;
                    }
                }

                // If all the operations were success (services found, characteristics are found too)
                // we have to set connection flag.
                if (Res == wclErrors.WCL_E_SUCCESS)
                    FConnected = true;
                else
                    // Otherwise we have to disconnect!
                    FClient.Disconnect();

                // Now we can fire the OnConnected event.
                DoConnected(Res);
            }
        }

        // GATT client disconnect event handler.
        private void ClientDisconnect(object Sender, int Reason)
        {
            DisconnectHub();

            // We have to fire the event only if connection was really established and
            // all the services and characteristics were read.
            if (FConnected)
            {
                // Do not forget to reset connection state.
                FConnected = false;
                DoDisconnected(Reason);
            }
        }

        // GATT client characteristi changed event handler.
        private void ClientCharacteristicChanged(Object sender, UInt16 Handle, Byte[] Value)
        {
            // Notify all services about characteristic changes. So each one can select owm updated data.
            if (FDeviceInformation.Connected)
                FDeviceInformation.CharacteristicChanged(Handle, Value);
            if (FBatteryLevel.Connected)
                FBatteryLevel.CharacteristicChanged(Handle, Value);
            if (FIo.Connected)
                FIo.CharacteristicChanged(Handle, Value);
            if (FHub.Connected)
                FHub.CharacteristicChanged(Handle, Value);
        }

        private void HubButtonStateChanged(Object Sender, Boolean Pressed)
        {
            DoButtonStateChanged(Pressed);
        }

        private void HubLowVoltageAlert(Object Sender, Boolean Alert)
        {
            DoLowVoltageAlert(Alert);
        }

        private void HubHighCurrentAlert(object Sender, bool Alert)
        {
            DoHighCurrentAlert(Alert);
        }

        private void HubDeviceAttached(Object Semder, wclWeDoIo Device)
        {
            // Make sure device is not attached yet.
            foreach (wclWeDoIo Io in FDevices)
            {
                if (Io.ConnectionId == Device.ConnectionId)
                    break;
            }

            FDevices.Add(Device);
            DoDeviceAttached(Device);
        }

        private void HubDeviceDetached(Object Sender, Byte ConnectionId)
        {
            wclWeDoIo Io = null;
            // Make sure device was attached.
            for (Int32 i = 0; i < FDevices.Count; i++)
            {
                if (FDevices[i].ConnectionId == ConnectionId)
                {
                    Io = FDevices[i];
                    break;
                }
            }

            if (Io != null)
            {
                Io.Detach();
                DoDeviceDetached(Io);
                FDevices.Remove(Io);
            }
        }

        internal wclWeDoIoService Io { get { return FIo; } }

        /// <summary> Fires the <c>OnConnected</c> event. </summary>
        /// <param name="Error"> If the connection has been established the parameter is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If connection has not been established the
        ///   parameter value is one of the Bluetooth error codes. </param>
        protected virtual void DoConnected(Int32 Error)
        {
            if (OnConnected != null)
                OnConnected(this, Error);
        }

        /// <summary> Fires the <c>OnDisconnected</c> event. </summary>
        /// <param name="Reason"> The disconnection reason code. </param>
        protected virtual void DoDisconnected(Int32 Reason)
        {
            if (OnDisconnected != null)
                OnDisconnected(this, Reason);
        }

        /// <summary> Fires the <c>OnButtonStateChanged</c> event. </summary>
        /// <param name="Pressed"> <c>True</c> if the button has been pressed. <c>False</c> if the
        ///   button has been released. </param>
        protected virtual void DoButtonStateChanged(Boolean Pressed)
        {
            if (OnButtonStateChanged != null)
                OnButtonStateChanged(this, Pressed);
        }

        /// <summary> Fires the <c>OnLowVoltageAlert</c> event. </summary>
        /// <param name="Alert"> <c>True</c> if device runs on low battery. <c>False</c> otherwise. </param>
        protected virtual void DoLowVoltageAlert(Boolean Alert)
        {
            if (OnLowVoltageAlert != null)
                OnLowVoltageAlert(this, Alert);
        }

        /// <summary> Fires the <c>OnHighCurrentAlert</c> event. </summary>
        /// <param name="Alert"> <c>True</c> if device runs on high current. <c>False</c> otherwise. </param>
        protected virtual void DoHighCurrentAlert(Boolean Alert)
        {
            if (OnHighCurrentAlert != null)
                OnHighCurrentAlert(this, Alert);
        }

        /// <summary> Fires the <c>OnDeviceAttached</c> event. </summary>
        /// /// <param name="Device"> The Input/Output device object. </param>
        /// <seealso cref="wclWeDoIo"/>
        protected virtual void DoDeviceAttached(wclWeDoIo Device)
        {
            if (OnDeviceAttached != null)
                OnDeviceAttached(this, Device);
        }

        /// <summary> Fires the <c>OnDeviceDetached</c> event. </summary>
        /// /// <param name="Device"> The Input/Output device object. </param>
        /// <seealso cref="wclWeDoIo"/>
        protected virtual void DoDeviceDetached(wclWeDoIo Device)
        {
            if (OnDeviceDetached != null)
                OnDeviceDetached(this, Device);
        }

        /// <summary> Creates new WeDo Client. </summary>
        public wclWeDoHub()
        {
            FConnected = false;
            FHubConnected = false;

            FClient = new wclGattClient();
            FClient.OnCharacteristicChanged += ClientCharacteristicChanged;
            FClient.OnConnect += ClientConnect;
            FClient.OnDisconnect += ClientDisconnect;

            FDeviceInformation = new wclWeDoDeviceInformationService(FClient, this);
            FBatteryLevel = new wclWeDoBatteryLevelService(FClient, this);
            FIo = new wclWeDoIoService(FClient, this);

            // This service precessed by special way because we need to delegate all methods and events
            // to the Hub object.
            FHub = new wclWeDoHubService(FClient, this);
            FHub.OnButtonStateChanged += HubButtonStateChanged;
            FHub.OnDeviceAttached += HubDeviceAttached;
            FHub.OnDeviceDetached += HubDeviceDetached;
            FHub.OnLowVoltageAlert += HubLowVoltageAlert;
            FHub.OnHighCurrentAlert += HubHighCurrentAlert;

            // Create attached devices list.
            FDevices = new List<wclWeDoIo>();

            OnConnected = null;
            OnDisconnected = null;
            OnButtonStateChanged = null;
            OnLowVoltageAlert = null;
            OnDeviceAttached = null;
            OnDeviceDetached = null;
            OnHighCurrentAlert = null;
        }

        /// <summary> Connects to a selected WeDo Hub. </summary>
        /// <param name="Radio"> The <see cref="wclBluetoothRadio" /> object that should be used
        ///   for executing Bluetooth LE connection. </param>
        /// <param name="Address"> The WeDo Hub MAC address. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seealso cref="wclBluetoothRadio" />
        public Int32 Connect(wclBluetoothRadio Radio, Int64 Address)
        {
            if (Radio == null || Address == 0)
                return wclErrors.WCL_E_INVALID_ARGUMENT;

            // This check prevent exception raising when you try to change
            // connection MAC address on already connected client.
            if (FClient.State != wclClientState.csDisconnected)
                return wclConnectionErrors.WCL_E_CONNECTION_ACTIVE;

            FClient.Address = Address;
            return FClient.Connect(Radio);
        }

        /// <summary> Disconnects from WeDo Hub. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 Disconnect()
        {
            DisconnectHub();

            return FClient.Disconnect();
        }

        /// <summary> Reads the current device name. </summary>
        /// <param name="Name"> If the method completed with success the parameter contains the
        ///   current device name. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 ReadDeviceName(out String Name)
        {
            return FHub.ReadDeviceName(out Name);
        }

        /// <summary> Writes new device name. </summary>
        /// <param name="Name"> The new device name. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 WriteDeviceName(String Name)
        {
            return FHub.WriteDeviceName(Name);
        }

        /// <summary> Gets the Hub device information service object. </summary>
        /// <value> The Hub device information service object. </value>
        /// <seealso cref="wclWeDoDeviceInformationService"/>
        public wclWeDoDeviceInformationService DeviceInformation { get { return FDeviceInformation; } }
        /// <summary> Gets the battery level service object. </summary>
        /// <value> The battery level service object. </value>
        /// <seealso cref="wclWeDoBatteryLevelService"/>
        public wclWeDoBatteryLevelService BatteryLevel { get { return FBatteryLevel; } }
        
        /// <summary> Gets the connected WeDo Hub Address. </summary>
        /// <value> The Hub MAC address. </value>
        public Int64 Address {  get { return FClient.Address; } }
        /// <summary> Gets connected status. </summary>
        /// <value> <c>true</c> if connected to WeDo Hub. </value>
        public Boolean Connected { get { return FConnected; } }
        /// <summary> Gets internal GATT client state. </summary>
        /// <value> The internal GATT client state. </value>
        /// <seealso cref="wclClientState" />
        public wclClientState ClientState { get { return FClient.State; } }
        /// <summary> Gets the list of the attached IO devices. </summary>
        /// <value> The list of the attached IO devices. </value>
        /// <seealso cref="wclWeDoIo"/>
        public List<wclWeDoIo> IoDevices {  get { return FDevices; } }

        /// <summary> The event fires when connection to a WeDo Hub
        ///   has been established. </summary>
        /// <seealso cref="wclClientConnectionConnectEvent" />
        public event wclClientConnectionConnectEvent OnConnected;
        /// <summary> The event fires when WeDo Hub has been disconnected. </summary>
        /// <seealso cref="wclClientConnectionDisconnectEvent" />
        public event wclClientConnectionDisconnectEvent OnDisconnected;
        /// <summary> The event fires when button state has been changed. </summary>
        /// <seealso cref="wclWeDoHubButtonStateChangedEvent"/>
        public event wclWeDoHubButtonStateChangedEvent OnButtonStateChanged;
        /// <summary> The event fires when new IO device has been attached. </summary>
        /// <seealso cref="wclWeDoDeviceStateChangedEvent"/>
        public event wclWeDoDeviceStateChangedEvent OnDeviceAttached;
        /// <summary> The event fires when an existing IO device has been detached. </summary>
        /// <seealso cref="wclWeDoDeviceStateChangedEvent"/>
        public event wclWeDoDeviceStateChangedEvent OnDeviceDetached;
        /// <summary> The event fires when device runs on low battery. </summary>
        /// <seealso cref="wclWeDoHubAlertEvent"/>
        public event wclWeDoHubAlertEvent OnLowVoltageAlert;
        /// <summary> The event fires when device runs on high current. </summary>
        /// <seealso cref="wclWeDoHubAlertEvent"/>
        public event wclWeDoHubAlertEvent OnHighCurrentAlert;
    };
}
