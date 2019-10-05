using System;

using wclCommon;
using wclCommunication;
using wclBluetooth;

namespace wclWeDo
{
    /// <summary> The main class that implements all the WeDo controlling features. </summary>
    public class wclWeDoClient
    {
        private wclGattClient FClient;
        private Boolean FConnected;

        private wclWeDoDeviceInformationService FDeviceInformation;
        private wclWeDoBatteryLevelService FBatteryLevel;
        private wclWeDoIoService FIo;
        private wclWeDoHubService FHub;

        private void DisconnectClients()
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

        private void ClientDisconnect(object Sender, int Reason)
        {
            DisconnectClients();

            // We have to fire the event only if connection was really established and
            // all the services and characteristics were read.
            if (FConnected)
            {
                // Do not forget to reset connection state.
                FConnected = false;
                DoDisconnected(Reason);
            }
        }

        private void ClientCharacteristicChanged(Object sender, UInt16 Handle, Byte[] Value)
        {
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
        ///   <seecref="wclErrors.WCL_E_SUCCESS" />. If connection has not been established the
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
        public wclWeDoClient()
        {
            FConnected = false;

            FClient = new wclGattClient();
            FClient.OnCharacteristicChanged += ClientCharacteristicChanged;
            FClient.OnConnect += ClientConnect;
            FClient.OnDisconnect += ClientDisconnect;

            FDeviceInformation = new wclWeDoDeviceInformationService(FClient);
            FBatteryLevel = new wclWeDoBatteryLevelService(FClient);
            FIo = new wclWeDoIoService(FClient);
            FHub = new wclWeDoHubService(FClient);

            OnConnected = null;
            OnDisconnected = null;
        }

        /// <summary> Connects to a selected WeDo device. </summary>
        /// <param name="Radio"> The <see cref="wclBluetoothRadio" /> object that should be used
        ///   for executing Bluetooth LE connection. </param>
        /// <param name="Address"> The WeDo device's address. </param>
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

        /// <summary> Disconnects from WeDo device. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 Disconnect()
        {
            return FClient.Disconnect();
        }

        /// <summary> Gets the device information service client object. </summary>
        /// <value> The device information client object. </value>
        /// <seealso cref="wclWeDoDeviceInformationService"/>
        public wclWeDoDeviceInformationService DeviceInformation { get { return FDeviceInformation; } }
        /// <summary> Gets the battery level service client object. </summary>
        /// <value> The battery level service client. </Value>
        /// <seealso cref="wclWeDoBatteryLevelService"/>
        public wclWeDoBatteryLevelService BatteryLevel { get { return FBatteryLevel; } }
        /// <summary> Gets the IO service client object. </summary>
        /// <value> The IO service client object. </value>
        /// <seealso cref="wclWeDoIoService"/>
        public wclWeDoIoService Io { get { return FIo; } }
        /// <summary> Gets the Hub service client object. </summary>
        /// <value> The Hub service client object. </value>
        /// <seealso cref="wclWeDoHubService"/>
        public wclWeDoHubService Hub { get { return FHub; } }

        /// <summary> Gets connected status. </summary>
        /// <value> <c>true</c> if connected to WeDo device. </value>
        public Boolean Connected { get { return FConnected; } }
        /// <summary> Gets internal GATT client state. </summary>
        /// <value> The internal GATT client state. </value>
        /// <seealso cref="wclClientState" />
        public wclClientState ClientState { get { return FClient.State; } }

        /// <summary> The event fires when connection to a WeDo device
        ///   has been established. </summary>
        /// <seealso cref="wclClientConnectionConnectEvent" />
        public event wclClientConnectionConnectEvent OnConnected;
        /// <summary> The event fires when WeDo has been disconnected. </summary>
        /// <seealso cref="wclClientConnectionDisconnectEvent" />
        public event wclClientConnectionDisconnectEvent OnDisconnected;
    };
}
