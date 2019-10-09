using System;

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

        // Hub GATT services.
        private wclWeDoDeviceInformationService FDeviceInformation;
        private wclWeDoBatteryLevelService FBatteryLevel;
        private wclWeDoIoService FIo;
        private wclWeDoHubService FHub;

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

        // GATT client connect event handler.
        private void ClientConnect(object Sender, int Error)
        {
            // If connection was not established simple fire the event with error code.
            if (Error != wclErrors.WCL_E_SUCCESS)
                DoConnected(Error);
            else
            {
                // try to connect to WeDo services.
                Int32 Res = FDeviceInformation.Connect();
                if (Res == wclErrors.WCL_E_SUCCESS)
                    Res = FBatteryLevel.Connect();
                if (Res == wclErrors.WCL_E_SUCCESS)
                    Res = FIo.Connect();
                if (Res == wclErrors.WCL_E_SUCCESS)
                    Res = FHub.Connect();

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
            // We have to release all services.
            DisconnectServices();

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

        /// <summary> Creates new WeDo Client. </summary>
        public wclWeDoHub()
        {
            FConnected = false;

            FClient = new wclGattClient();
            FClient.OnCharacteristicChanged += ClientCharacteristicChanged;
            FClient.OnConnect += ClientConnect;
            FClient.OnDisconnect += ClientDisconnect;

            FDeviceInformation = new wclWeDoDeviceInformationService(FClient, this);
            FBatteryLevel = new wclWeDoBatteryLevelService(FClient, this);
            FIo = new wclWeDoIoService(FClient, this);
            FHub = new wclWeDoHubService(FClient, this);

            OnConnected = null;
            OnDisconnected = null;
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
            return FClient.Disconnect();
        }

        /// <summary> Gets the Hub device information service object. </summary>
        /// <value> The Hub device information service object. </value>
        /// <seealso cref="wclWeDoDeviceInformationService"/>
        public wclWeDoDeviceInformationService DeviceInformation { get { return FDeviceInformation; } }
        /// <summary> Gets the battery level service object. </summary>
        /// <value> The battery level service object. </value>
        /// <seealso cref="wclWeDoBatteryLevelService"/>
        public wclWeDoBatteryLevelService BatteryLevel { get { return FBatteryLevel; } }
        /// <summary> Gets the IO service object. </summary>
        /// <value> The IO service object. </value>
        /// <seealso cref="wclWeDoIoService"/>
        public wclWeDoIoService Io { get { return FIo; } }
        /// <summary> Gets the Hub service object. </summary>
        /// <value> The Hub service object. </value>
        /// <seealso cref="wclWeDoHubService"/>
        public wclWeDoHubService Hub { get { return FHub; } }

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

        /// <summary> The event fires when connection to a WeDo Hub
        ///   has been established. </summary>
        /// <seealso cref="wclClientConnectionConnectEvent" />
        public event wclClientConnectionConnectEvent OnConnected;
        /// <summary> The event fires when WeDo Hub has been disconnected. </summary>
        /// <seealso cref="wclClientConnectionDisconnectEvent" />
        public event wclClientConnectionDisconnectEvent OnDisconnected;
    };
}
