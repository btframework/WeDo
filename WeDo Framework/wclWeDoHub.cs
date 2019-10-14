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
using System.Linq;

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

        private void HubHighCurrentAlert(object Sender, Boolean Alert)
        {
            DoHighCurrentAlert(Alert);
        }

        private void HubLowSignalAlert(object Sender, Boolean Alert)
        {
            DoLowSignalAlert(Alert);
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

        /// <summary> Fires the <c>OnLowSignalAlert</c> event. </summary>
        /// <param name="Alert"> <c>True</c> if the RSSI value is low. <c>False</c> otherwise. </param>
        protected virtual void DoLowSignalAlert(Boolean Alert)
        {
            if (OnLowSignalAlert != null)
                OnLowSignalAlert(this, Alert);
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
            FHub.OnLowSignalAlert += HubLowSignalAlert;

            // Create attached devices list.
            FDevices = new List<wclWeDoIo>();

            OnConnected = null;
            OnDisconnected = null;
            OnButtonStateChanged = null;
            OnLowVoltageAlert = null;
            OnDeviceAttached = null;
            OnDeviceDetached = null;
            OnHighCurrentAlert = null;
            OnLowSignalAlert = null;
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

        /// <summary> Turns the Hub off. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <remarks> The method sends the Turn Off command to the connected Hub. </remarks>
        public Int32 TurnOff()
        {
            return FHub.TurnOff();
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
        /// <summary> The event fires when low RSSI value received from the device. </summary>
        /// <seealso cref="wclWeDoHubAlertEvent"/>
        public event wclWeDoHubAlertEvent OnLowSignalAlert;
    };

    /// <summary> The class represets an attached Input/Outpout device. </summary>
    public abstract class wclWeDoIo
    {
        private const Byte WEDO_DEVICE_MOTOR = 1;
        private const Byte WEDO_DEVICE_VOLTAGE_SENSOR = 20;
        private const Byte WEDO_DEVICE_CURRENT_SENSOR = 21;
        private const Byte WEDO_DEVICE_PIEZO = 22;
        private const Byte WEDO_DEVICE_RGB = 23;
        private const Byte WEDO_DEVICE_TILT_SENSOR = 34;
        private const Byte WEDO_DEVICE_MOTION_SENSOR = 35;

        private Boolean FAttached;
        private Byte FConnectionId;
        private List<wclWeDoDataFormat> FDataFormats;
        private wclWeDoInputFormat FDefaultInputFormat;
        private wclWeDoIoDeviceType FDeviceType;
        private wclWeDoVersion FFirmwareVersion;
        private wclWeDoVersion FHardwareVersion;
        private wclWeDoHub FHub;
        private wclWeDoInputFormat FInputFormat;
        private Boolean FInternal;
        private List<Byte[]> FNumbersFromValueData;
        private Byte FPortId;
        private List<wclWeDoDataFormat> FValidDataFormats;
        private Byte[] FValue;

        private void SetValue(Byte[] Value)
        {
            FValue = Value;
            ValueChanged();
        }

        private Int32 SetDefaultInputFormat(wclWeDoInputFormat Format)
        {
            FDefaultInputFormat = Format;
            return SendInputFormat(Format);
        }

        private float GetAsFloat()
        {
            if (FValue == null || FValue.Length == 0)
                return 0f;
            if (FValue.Length == 4)
                return BitConverter.ToSingle(FValue, 0);
            return 0f;
        }

        private Int32 GetAsInteger()
        {
            if (FValue == null || FValue.Length == 0)
                return 0;
            if (FValue.Length == 1)
            {
                Int32 Signed = (Int32)FValue[0];
                if (Signed > 127)
                    Signed = 0 - (256 - Signed);
                return Signed;
            }
            if (FValue.Length == 2)
                return BitConverter.ToInt16(FValue, 0);
            if (FValue.Length == 4)
                return BitConverter.ToInt32(FValue, 0);
            return 0;
        }

        private Byte GetInputFormatMode()
        {
            if (FInputFormat != null)
                return FInputFormat.Mode;
            if (FDefaultInputFormat != null)
                return FDefaultInputFormat.Mode;
            return 0;
        }

        private wclWeDoDataFormat DataFormatForInputFormat(wclWeDoInputFormat InputFormat)
        {
            foreach (wclWeDoDataFormat DataFormat in FValidDataFormats)
            {
                if (DataFormat.Mode == InputFormat.Mode && DataFormat.Unit == InputFormat.Unit)
                {
                    if ((DataFormat.DataSetCount * DataFormat.DataSetSize) != InputFormat.NumberOfBytes)
                        return null;
                    return DataFormat;
                }
            }
            return null;
        }

        private Boolean VerifyValue(Byte[] Value)
        {
            if (Value == null || Value.Length == 0)
                return true;
            if (FValidDataFormats.Count == 0)
                return true;

            // If one or more InputDataFormats are defined, we look at the latest received InputFormat from the device
            // for a received value to be accepted, there:
            //   1. Must exists an DataFormat that matches the latest received InputFormat from device.
            //   2. The received valueData length must match this DataFormat.
            wclWeDoDataFormat DataFormat = DataFormatForInputFormat(FInputFormat);
            if (DataFormat == null)
                return false;

            Boolean ValueCorrect = (Value.Length == (DataFormat.DataSetSize * DataFormat.DataSetCount));
            if (!ValueCorrect)
            {
                FNumbersFromValueData.Clear();
                return false;
            }

            // If the Data Format has a value fill the NumbersFromValueData array with all received numbers
            if (DataFormat.DataSetCount > 0)
            {
                List<Byte> DataList = Value.ToList();
                FNumbersFromValueData.Clear();
                for (Int32 i = 0; i < DataList.Count; i += DataFormat.DataSetSize)
                    FNumbersFromValueData.Add(DataList.GetRange(i, DataFormat.DataSetSize).ToArray());
            }

            return ValueCorrect;
        }

        // The method called by HUB when new device found (attached).
        internal static wclWeDoIo Attach(wclWeDoHub Hub, Byte[] RawInfo)
        {
            if (RawInfo == null || RawInfo.Length < 2)
                return null;
            if ((RawInfo[1] == 1 && RawInfo.Length != 12) || (RawInfo[1] == 0 && RawInfo.Length != 2))
                return null;
            if (RawInfo[1] == 0)
                // Detached???
                return null;

            wclWeDoIo Io;
            Byte ConnectionId = RawInfo[0];
            switch (RawInfo[3])
            {
                case WEDO_DEVICE_MOTOR:
                    Io = new wclWeDoMotor(Hub, ConnectionId);
                    Io.FDeviceType = wclWeDoIoDeviceType.iodMotor;
                    break;
                case WEDO_DEVICE_VOLTAGE_SENSOR:
                    Io = new wclWeDoVoltageSensor(Hub, ConnectionId);
                    Io.FDeviceType = wclWeDoIoDeviceType.iodVoltageSensor;
                    break;
                case WEDO_DEVICE_CURRENT_SENSOR:
                    Io = new wclWeDoCurrentSensor(Hub, ConnectionId);
                    Io.FDeviceType = wclWeDoIoDeviceType.iodCurrentSensor;
                    break;
                case WEDO_DEVICE_PIEZO:
                    Io = new wclWeDoPieazo(Hub, ConnectionId);
                    Io.FDeviceType = wclWeDoIoDeviceType.iodPiezo;
                    break;
                case WEDO_DEVICE_RGB:
                    Io = new wclWeDoRgbLight(Hub, ConnectionId);
                    Io.FDeviceType = wclWeDoIoDeviceType.iodRgb;
                    break;
                case WEDO_DEVICE_TILT_SENSOR:
                    Io = new wclWeDoTiltSensor(Hub, ConnectionId);
                    Io.FDeviceType = wclWeDoIoDeviceType.iodTiltSensor;
                    break;
                case WEDO_DEVICE_MOTION_SENSOR:
                    Io = new wclWeDoMotionSensor(Hub, ConnectionId);
                    Io.FDeviceType = wclWeDoIoDeviceType.iodMotionSensor;
                    break;
                default:
                    Io = null;
                    break;
            }

            if (Io != null)
            {

                Byte[] Tmp = new Byte[4];
                Array.Copy(RawInfo, 8, Tmp, 0, 4);
                Io.FFirmwareVersion = wclWeDoVersion.FromByteArray(Tmp);
                Array.Copy(RawInfo, 4, Tmp, 0, 4);
                Io.FHardwareVersion = wclWeDoVersion.FromByteArray(Tmp);
                Io.FPortId = RawInfo[2];
                Io.FInternal = (Io.PortId > 50);
            }

            return Io;
        }

        // The method called by HUB when device has been detached.
        internal void Detach()
        {
            FAttached = false;
            FNumbersFromValueData.Clear();
        }

        // The method called by IO Service when Input Format has been updated.
        internal void UpdateInputFormat(wclWeDoInputFormat Format)
        {
            if (FInputFormat == null || (Format != FInputFormat && FConnectionId == Format.ConnectionId))
            {
                wclWeDoInputFormat OldFormat = FInputFormat;
                FInputFormat = Format;
                InputFormatChanged(OldFormat);
                SendReadValueRequest();
            }
        }

        // The method called by the IO Service when new value received.
        internal void UpdateValue(Byte[] Value)
        {
            if (FValue != null && FValue.Equals(Value))
                return;

            if (VerifyValue(Value))
            {
                FValue = Value;
                ValueChanged();
            }
        }

        /// <summary> Sends data to the IO service. </summary>
		/// <param name="Data"> The data to write. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
		protected Int32 WriteData(Byte[] Data)
        {
            return FHub.Io.WriteData(Data, FConnectionId);
        }

        /// <summary> If the notifications is disabled for the service in the Input Format you will have to use
        ///   this method to request an updated value for the service. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
		protected Int32 SendReadValueRequest()
        {
            return FHub.Io.ReadValue(FConnectionId);
        }

        /// <summary> Sends a reset command to the Device. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
		protected Int32 ResetSensor()
        {
            return FHub.Io.ResetIo(FConnectionId);
        }

        /// <summary> Adds a new valid data format. </summary>
		/// <param name="Format"> The data format to add. </param>
        /// <seealso cref="wclWeDoDataFormat"/>
		protected void AddValidDataFormat(wclWeDoDataFormat Format)
        {
            FValidDataFormats.Add(Format);
        }

        /// <summary> Removes a valid data format. </summary>
		/// <param name="Format"> The data format to remove. </param>
        /// <seealso cref="wclWeDoDataFormat"/>
		protected void RemoveValidDataFormat(wclWeDoDataFormat Format)
        {
            FValidDataFormats.Remove(Format);
        }

        /// <summary> Send an updated input format for this service to the device. </summary>
		/// <param name="Format"> New input format. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seelso cref="wclWeDoInputFormat"/>
		protected Int32 SendInputFormat(wclWeDoInputFormat Format)
        {
            return FHub.Io.WriteInputFormat(Format, FConnectionId);
        }

        /// <summary> Changes mode of the Input Format. </summary>
        /// <param name="Mode"> The Input Format mode. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        protected Int32 SetInputFormatMode(Byte Mode)
        {
            if (FInputFormat != null)
                return SendInputFormat(FInputFormat.InputFormatBySettingMode(Mode));
            if (FDefaultInputFormat != null)
                return SendInputFormat(FDefaultInputFormat.InputFormatBySettingMode(Mode));
            return wclErrors.WCL_E_INVALID_ARGUMENT;
        }

        /// <summary> The method called when Input Format has been changed. </summary>
        /// <param name="OldFormat"> The old Input Format. </param>
        /// <remarks> A derived class must override this method to get notifications about
        ///   format changes. </remarks>
        protected virtual void InputFormatChanged(wclWeDoInputFormat OldFormat)
        {
            // Do nothing
        }

        /// <summary> The method called when data value has been changed. </summary>
        /// <remarks> A derived class must override this method to get notifications about
        ///   value changes. </remarks>
        protected virtual void ValueChanged()
        {
            // Do nothing.
        }

        /// <summary> Gets current sensor's value as <c>Float</c> number. </summary>
        /// <value> The float sensors value. </value>
        protected float AsFloat { get { return GetAsFloat(); } }
        /// <summary> Gets the current sensor's value as <c>Integer</c> number. </summary>
        /// <value> The integer sensor's value. </value>
        protected Int32 AsInteger { get { return GetAsInteger(); } }
        /// <summary> Gets the list of supported Data Formats. </summary>
        /// <value> The list of supported Data Formats. </value>
        /// <seealso cref="wclWeDoDataFormat"/>
        protected List<wclWeDoDataFormat> DataFormats { get { return FDataFormats; } }
        /// <summary> Gets and sets the default input format. </summary>
        /// <value> The default input format. </value>
        /// <seealso cref="wclWeDoInputFormat"/>
        protected wclWeDoInputFormat DefaultInputFormat { get { return FDefaultInputFormat; } set { SetDefaultInputFormat(value); } }
        /// <summary> Gets the sensor Input Format. </summary>
        /// <value> The Input Format. </value>
        /// <seealso cref="wclWeDoInputFormat"/>
        protected wclWeDoInputFormat InputFormat { get { return FInputFormat; } }
        /// <summary> Gets the Input Format mode. </summary>
        /// <value> The Input Format Mode. </value>
        protected Byte InputFormatMode { get { return GetInputFormatMode(); } }
        /// <summary> Gets alist with one byte[] per number received. </summary>
        /// <value> The list of bytes array. </value>
		protected List<Byte[]> NumbersFromValueData { get { return FNumbersFromValueData; } }
        /// <summary> Gets the current sensors value. </summary>
        /// <value> The sensors value as raw bytes array. </value>
        protected Byte[] Value { get { return FValue; } private set { SetValue(value); } }

        /// <summary> Creates new IO device object. </summary>
        /// <param name="Hub"> The Hub object that owns the device. If this parameter is <c>null</c>
        ///   the <seealso cref="wclEInvalidArgument"/> exception raises. </param>
        /// <param name="ConnectionId"> The device's Connection ID. </param>
        /// <seealso cref="wclWeDoHub"/>
        /// <exception cref="wclEInvalidArgument"> The exception raises when the <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoIo(wclWeDoHub Hub, Byte ConnectionId)
        {
            if (Hub == null)
                throw new wclEInvalidArgument("Hub parameter can not be null.");

            FAttached = true; // It is always attached on creation!
            FConnectionId = ConnectionId;
            FDataFormats = new List<wclWeDoDataFormat>();
            FDefaultInputFormat = null;
            FDeviceType = wclWeDoIoDeviceType.iodUnknown;
            FFirmwareVersion = new wclWeDoVersion();
            FHardwareVersion = new wclWeDoVersion();
            FHub = Hub;
            FInputFormat = null;
            FInternal = true;
            FPortId = 0;
            FNumbersFromValueData = new List<Byte[]>();
            FValidDataFormats = new List<wclWeDoDataFormat>();
            FValue = null;
        }

        /// <summary> Gets the IO device state. </summary>
        /// <value> <c>True</c> if the device is attached. <c>False</c> if the device is detached. </value>
        public Boolean Attached { get { return FAttached; } }
        /// <summary> Gets the IO connection ID. </summary>
        /// <value> The IO connection ID. </value>
        /// <remarks> It is guarateed that the connection ID is unique. </remarks>
        public Byte ConnectionId { get { return FConnectionId; } }
        /// <summary> Gets the device represented by this object. </summary>
        /// <value> The IO device type. </value>
        /// <seealso cref="wclWeDoIoDeviceType"/>
        public wclWeDoIoDeviceType DeviceType { get { return FDeviceType; } }
        /// <summary> Gets the IO device firmware version. </summary>
        /// <value> The IO device firmware version. </value>
        /// <seealso cref="wclWeDoVersion"/>
		public wclWeDoVersion FirmwareVersion { get { return FFirmwareVersion; } }
        /// <summary> Gets the IO device hardware version. </summary>
        /// <value> The IO device hardware version. </value>
        /// <seealso cref="wclWeDoVersion"/>
        public wclWeDoVersion HardwareVersion { get { return FHardwareVersion; } }
        /// <summary> Gets the IO type represented by this object. </summary>
        /// <value> <c>True</c> if the IO device is internal. <c>False</c> if the IO device is external. </value>
        public Boolean Internal { get { return FInternal; } }
        /// <summary> Gets the WeDo Hub object that owns the IO device. </summary>
        /// <value> The WeDo Hub object. </value>
        /// <seealso cref="wclWeDoHub"/>
        public wclWeDoHub Hub { get { return FHub; } }
        /// <summary> The index of the port on the Hub the IO is attached to.  </summary>
        /// <value> The port ID. </value>
		public Byte PortId { get { return FPortId; } }
    };
}
