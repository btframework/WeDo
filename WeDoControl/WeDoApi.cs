using System;
using System.Collections.Generic;
using System.Text;

using wclCommon;
using wclCommunication;
using wclBluetooth;

namespace WeDo
{
    /// <summary> The <c>OnDeviceFound</c> event handler prototype. </summary>
    /// <param name="Sender"> The object that fired the event. </param>
    /// <param name="Address"> The WeDo device's MAC address. </param>
    public delegate void WeDoDeviceFoundEvent(Object Sender, Int64 Address);

    /// <summary> This class searchs for WeDo devices. </summary>
    /// <remarks> An application must always dispose the class when it is not needed. </remarks>
    public class WeDoWatcher : IDisposable
    {
        private static Guid WEDO_ADVERTISING_SERVICE = new Guid("00001523-1212-efde-1523-785feabcd123");

        private List<Int64> FDevices;
        private Boolean FDisposed;
        private wclBluetoothLeBeaconWatcher FWatcher;

        #region Beacon Watcher events
        private void WatcherAdvertisementUuidFrame(object Sender, long Address, long Timestamp, sbyte Rssi, Guid Uuid)
        {
            if (Uuid == WEDO_ADVERTISING_SERVICE)
            {
                if (!FDevices.Contains(Address))
                {
                    DoDeviceFound(Address);
                    FDevices.Add(Address);
                }
            }
        }

        private void WatcherStarted(object sender, EventArgs e)
        {
            DoStarted();
        }

        private void WatcherStopped(object sender, EventArgs e)
        {
            DoStopped();
        }
        #endregion

        #region Event rountines
        /// <summary> Fires the <c>OnDeviceFound</c> event. </summary>
        /// <param name="Address"> The device's MAC. </param>
        protected virtual void DoDeviceFound(Int64 Address)
        {
            if (OnDeviceFound != null)
                OnDeviceFound(this, Address);
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
        #endregion

        #region IDisposable
        /// <summary> Disposes the object.  </summary>
        /// <param name="disposing"> <para> If the parameter equals true, the method has been called directly
        ///   or indirectly by a user's code. Managed and unmanaged resources can be disposed. </para>
        ///   <para> If disposing equals false, the method has been called by the runtime from inside the
        ///   finalizer and you should not reference other objects. Only unmanaged resources can be
        ///   disposed. </para> </param>
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!FDisposed)
            {
                if (disposing)
                {
                    FWatcher.Stop();
                    FWatcher = null;

                    FDevices.Clear();
                    FDevices = null;
                }

                FDisposed = true;
            }
        }
        #endregion

        #region Constructor and destructor(s)
        /// <summary> Creates new WeDo Watcher object. </summary>
        public WeDoWatcher()
        {
            FDevices = new List<Int64>();
            FDisposed = false;

            FWatcher = new wclBluetoothLeBeaconWatcher();
            FWatcher.OnAdvertisementUuidFrame += WatcherAdvertisementUuidFrame;
            FWatcher.OnStarted += WatcherStarted;
            FWatcher.OnStopped += WatcherStopped;

            OnDeviceFound = null;
            OnStarted = null;
            OnStopped = null;
        }

        /// <summary> Finalizer. </summary>
        ~WeDoWatcher()
        {
            Dispose(false);
        }

        /// <summary> Disposes the object. </summary>
        public void Dispose()
        {
            if (FDisposed)
                throw new ObjectDisposedException("WeDoWatcher");

            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Discovering methods
        /// <summary> Starts watching (discovering) for WeDo devices. </summary>
        /// <param name="Radio"> The <see cref="wclBluetoothRadio" /> object that should be used
        ///   for executing Bluetooth LE discovering. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seealso cref="wclBluetoothRadio" />
        public Int32 Start(wclBluetoothRadio Radio)
        {
            if (FDisposed)
                throw new ObjectDisposedException("WeDoWatcher");

            return FWatcher.Start(Radio);
        }

        /// <summary> Stops discovering WeDo devices. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 Stop()
        {
            if (FDisposed)
                throw new ObjectDisposedException("WeDoWatcher");

            return FWatcher.Stop();
        }
        #endregion

        #region Events.
        /// <summary> The event fires when new WeDo device has been found. </summary>
        /// <seealso cref="WeDoDeviceFoundEvent" />
        public event WeDoDeviceFoundEvent OnDeviceFound;
        /// <summary> The event fires when discovering has been started. </summary>
        /// <seealso cref="EventHandler" />
        public event EventHandler OnStarted;
        /// <summary> The event fires when discovering has been stopped. </summary>
        /// <seealso cref="EventHandler" />
        public event EventHandler OnStopped;
        #endregion
    };

    /// <summary> The main class that implements all the WeDo controlling features. </summary>
    /// <remarks> An application must always dispoe the class when it is not needed. </remarks>
    public class WeDoController : IDisposable
    {
        #region WeDo services and characteristics
        #region Device Information Service
        /// <summary> Standard Bluetooth LE Device Information Service. </summary>
        private static Guid WEDO_SERVICE_DEVICE_INFORMATION = new Guid("0000180a-0000-1000-8000-00805f9b34fb");
        #region Characteristics
        /// <summary> Firmware Revision characteristic. </summary>
        /// <remarks> Readable characteristic. </remarks>
        private static Guid WEDO_CHARACTERISTIC_FIRMWARE_REVISION = new Guid("00002a26-0000-1000-8000-00805f9b34fb");
        /// <summary> Firmware Revision characteristic. </summary>
        /// <remarks> Readable characteristic. </remarks>
        private static Guid WEDO_CHARACTERISTIC_HARDWARE_REVISION = new Guid("00002a27-0000-1000-8000-00805f9b34fb");
        /// <summary> Software Revision characteristic. </summary>
        /// <remarks> Readable characteristic. </remarks>
        private static Guid WEDO_CHARACTERISTIC_SOFTWARE_REVISION = new Guid("00002a28-0000-1000-8000-00805f9b34fb");
        /// <summary> Manufacturer Name characteristic. </summary>
        /// <remarks> Readable characteristic. </remarks>
        private static Guid WEDO_CHARACTERISTIC_MANUFACTURER_NAME = new Guid("00002a29-0000-1000-8000-00805f9b34fb");
        #endregion
        #endregion

        #region Battery Level Service
        /// <summary> Standard Bluetooth LE Battery Level Service. </summary>
        private static Guid WEDO_SERVICE_BATTERY_LEVEL = new Guid("0000180f-0000-1000-8000-00805f9b34fb");
        #region Characteristics
        /// <summary> Battery level characteristic. </summary>
        /// <remarks> Readable and Notifiable characteristic. </remarks>
        private static Guid WEDO_CHARACTERISTIC_BATTERY_LEVEL = new Guid("00002a19-0000-1000-8000-00805f9b34fb");
        #endregion
        #endregion

        #region WeDo IO Service
        private static Guid WEDO_SERVICE_IO = new Guid("{00004f0e-1212-efde-1523-785feabcd123}");
        #region Characteristics
        /// <summary> Sensor Value characteristic. </summary>
        /// <remarks> Readable and Notifiable characterisitc. </remarks>
        private static Guid WEDO_CHARACTERISTIC_SENSOR_VALUE = new Guid("00001560-1212-efde-1523-785feabcd123");
        /// <summary> Sensor Value Format characteristic . </summary>
        /// <remarks> Notifiable characteristic. </remarks>
        private static Guid WEDO_CHARACTERISTIC_SENSOR_VALUE_FORMAT = new Guid("00001561-1212-efde-1523-785feabcd123");
        /// <summary> Input command characteristic. </summary>
        /// <remarks> Writable and Writable Without Response characteristic. </remarks>
        private static Guid WEDO_CHARACTERISTIC_INPUT_COMMAND = new Guid("00001563-1212-efde-1523-785feabcd123");
        /// <summary> Output command characteristic. </summary>
        /// <remarks> Writable and Writable Without Response characteristic. </remarks>
        private static Guid WEDO_CHARACTERISTIC_OUTPUT_COMMAND = new Guid("00001565-1212-efde-1523-785feabcd123");
        #endregion
        #endregion

        #region WeDo HUB Service
        /// <summary> WeDo HUB service. </summary>
        private static Guid WEDO_SERVICE_HUB = new Guid("00001523-1212-efde-1523-785feabcd123");
        #region Characteristics
        /// <summary> Device name characteristic. </summary>
        /// <remarks> Readable and Writable characteristic. </remarks>
        private static Guid WEDO_CHARACTERISTIC_DEVICE_NAME = new Guid("00001524-1212-efde-1523-785feabcd123");
        /// <summary> Buttons state characteristic. </summary>
        /// <remarks> Readable and Notifiable characteristic. </remarks>
        private static Guid WEDO_CHARACTERISTIC_BUTTON_STATE = new Guid("00001526-1212-efde-1523-785feabcd123");
        /// <summary> IO attached charactrisitc.</summary>
        /// <remarks> Notifiable characteristic. </remarks>
        private static Guid WEDO_CHARACTERISTIC_IO_ATTACHED = new Guid("00001527-1212-efde-1523-785feabcd123");
        /// <summary> Low Voltrage Alert characteristic. </summary>
        /// <remarks> Readable and Notifiable characteristic. </remarks>
        private static Guid WEDO_CHARACTERISTIC_LOW_VOLTAGE_ALERT = new Guid("00001528-1212-efde-1523-785feabcd123");
        /// <summary> High Current Aleart characteristic. </summary>
        /// <remarks> Readable and Notifiable characteristic. </remarks>
        private static Guid WEDO_CHARACTERISTIC_HIGH_CURRENT_ALERT = new Guid("00001529-1212-efde-1523-785feabcd123");
        /// <summary> Low Signal Aleart characteristic. </summary>
        /// <remarks> Readable and Notifiable characteristic. </remarks>
        private static Guid WEDO_CHARACTERISTIC_LOW_SIGNAL_ALERT = new Guid("0000152a-1212-efde-1523-785feabcd123");
        /// <summary> Turn Off command characteristic. </summary>
        /// <remarks> Writable characteristic. </remarks>
        private static Guid WEDO_CHARACTERISTIC_TURN_OFF = new Guid("0000152b-1212-efde-1523-785feabcd123");
        /// <summary> VCC Port Control characteristic. </summary>
        /// <remarks> Readable and Writable characteristic. </remarks>
        private static Guid WEDO_CHARACTERISTIC_VCC_PORT_CONTROL = new Guid("0000152c-1212-efde-1523-785feabcd123");
        /// <summary> Battery Type characteristic. </summary>
        /// <remarks> Readable characteristic. </remarks>
        private static Guid WEDO_CHARACTERISTIC_BATTERY_TYPE = new Guid("0000152d-1212-efde-1523-785feabcd123");
        /// <summaery> Device Disconnect Cpmmand characteristic. </summaery>
        /// <remarks> Writable characteristic. </remarks>
        private static Guid WEDO_CHARACTERISTIC_DEVICE_DISCONNECT = new Guid("0000152e-1212-efde-1523-785feabcd123");
        #endregion
        #endregion
        #endregion

        private wclGattClient FClient;
        private Boolean FConnected;
        private Boolean FDisposed;

        #region Characteristics
        #region Device Information
        private wclGattCharacteristic? FirmwareVersionChar;
        private wclGattCharacteristic? HardwareVersionChar;
        private wclGattCharacteristic? SoftwareVersionChar;
        private wclGattCharacteristic? ManufacturerNameChar;
        #endregion

        #region Battery Level
        private wclGattCharacteristic? BatteryLevelChar;
        #endregion

        #region WeDo IO
        private wclGattCharacteristic? SensorValueChar;
        private wclGattCharacteristic? SensorValueFormatChar;
        private wclGattCharacteristic? InputCommandChar;
        private wclGattCharacteristic? OutputCommandChar;
        #endregion

        #region WeDo Hub
        private wclGattCharacteristic? DeviceNameChar;
        private wclGattCharacteristic? ButtonStateChar;
        private wclGattCharacteristic? IoAttachedChar;
        private wclGattCharacteristic? LowVoltageAlertChar;
        private wclGattCharacteristic? HighCurrentAleartChar;
        private wclGattCharacteristic? LowSignalChar;
        private wclGattCharacteristic? TurnOffChar;
        private wclGattCharacteristic? VccPortChar;
        private wclGattCharacteristic? BatteryTypeChar;
        private wclGattCharacteristic? DeviceDisconnectChar;
        #endregion
        #endregion

        #region Helper functions
        private Guid ToGuid(wclGattUuid Uuid)
        {
            if (Uuid.IsShortUuid)
            {
                Byte[] Bytes = wclUUIDs.Bluetooth_Base_UUID.ToByteArray();
                Int16 Data2 = BitConverter.ToInt16(Bytes, 4);
                Int16 Data3 = BitConverter.ToInt16(Bytes, 6);
                Byte[] Data4 = new Byte[8];
                Buffer.BlockCopy(Bytes, 8, Data4, 0, 8);
                return new Guid(Uuid.ShortUuid, Data2, Data3, Data4);
            }
            else
                return Uuid.LongUuid;
        }

        private Boolean CompareGuid(wclGattUuid GattUuid, Guid Uuid)
        {
            return (ToGuid(GattUuid) == Uuid);
        }

        private Int32 FindService(Guid Uuid, ref wclGattService[] Services, out wclGattService? Service)
        {
            Service = null;

            foreach (wclGattService svc in Services)
            {
                if (CompareGuid(svc.Uuid, Uuid))
                {
                    Service = svc;
                    return wclErrors.WCL_E_SUCCESS;
                }
            }

            return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;
        }

        private Int32 FindCharactersitc(Guid Uuid, ref wclGattCharacteristic[] Characteristics,
            out wclGattCharacteristic? Characteristic)
        {
            Characteristic = null;

            foreach (wclGattCharacteristic chr in Characteristics)
            {
                if (CompareGuid(chr.Uuid, Uuid))
                {
                    Characteristic = chr;
                    return wclErrors.WCL_E_SUCCESS;
                }
            }

            return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;
        }

        private void ResetCharacteristics()
        {
            FirmwareVersionChar = null;
            HardwareVersionChar = null;
            SoftwareVersionChar = null;
            ManufacturerNameChar = null;

            BatteryLevelChar = null;

            SensorValueChar = null;
            SensorValueFormatChar = null;
            InputCommandChar = null;
            OutputCommandChar = null;

            DeviceNameChar = null;
            ButtonStateChar = null;
            IoAttachedChar = null;
            LowVoltageAlertChar = null;
            HighCurrentAleartChar = null;
            LowSignalChar = null;
            TurnOffChar = null;
            VccPortChar = null;
            BatteryTypeChar = null;
            DeviceDisconnectChar = null;
        }

        private Int32 ReadStringValue(wclGattCharacteristic? Characterisitc, out String Value)
        {
            Value = "";

            if (FDisposed)
                throw new ObjectDisposedException("WeDoController");

            if (!FConnected)
                return wclConnectionErrors.WCL_E_CONNECTION_NOT_ACTIVE;

            if (Characterisitc == null)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;

            Byte[] CharValue;
            Int32 Res = FClient.ReadCharacteristicValue(Characterisitc.Value, wclGattOperationFlag.goNone,
                out CharValue);
            if (Res == wclErrors.WCL_E_SUCCESS)
            {
                if (CharValue != null && CharValue.Length > 0)
                    Value = Encoding.UTF8.GetString(CharValue);
            }

            return Res;
        }
        #endregion

        #region Characteristics detection
        private Int32 FindDeviceInfoChars(ref wclGattService[] Services)
        {
            wclGattService? Service;
            Int32 Res = FindService(WEDO_SERVICE_DEVICE_INFORMATION, ref Services, out Service);
            if (Res == wclErrors.WCL_E_SUCCESS)
            {
                wclGattCharacteristic[] Characteristics;
                Res = FClient.ReadCharacteristics(Service.Value, wclGattOperationFlag.goNone, out Characteristics);
                if (Res == wclErrors.WCL_E_SUCCESS)
                {
                    // These characteristics are not important so we can ignore errors if some was not found.
                    FindCharactersitc(WEDO_CHARACTERISTIC_FIRMWARE_REVISION, ref Characteristics, out FirmwareVersionChar);
                    FindCharactersitc(WEDO_CHARACTERISTIC_HARDWARE_REVISION, ref Characteristics, out HardwareVersionChar);
                    FindCharactersitc(WEDO_CHARACTERISTIC_SOFTWARE_REVISION, ref Characteristics, out SoftwareVersionChar);
                    FindCharactersitc(WEDO_CHARACTERISTIC_MANUFACTURER_NAME, ref Characteristics, out ManufacturerNameChar);

                    Characteristics = null;
                }
            }
            return Res;
        }

        private Int32 FindBatteryLevelChars(ref wclGattService[] Services)
        {
            wclGattService? Service;
            Int32 Res = FindService(WEDO_SERVICE_BATTERY_LEVEL, ref Services, out Service);
            if (Res == wclErrors.WCL_E_SUCCESS)
            {
                wclGattCharacteristic[] Characteristics;
                Res = FClient.ReadCharacteristics(Service.Value, wclGattOperationFlag.goNone, out Characteristics);
                if (Res == wclErrors.WCL_E_SUCCESS)
                {
                    // Battery level service and characteristic is important!
                    Res = FindCharactersitc(WEDO_CHARACTERISTIC_BATTERY_LEVEL, ref Characteristics, out BatteryLevelChar);

                    Characteristics = null;
                }
            }
            return Res;
        }

        private Int32 FindWeDoIoChars(ref wclGattService[] Services)
        {
            wclGattService? Service;
            Int32 Res = FindService(WEDO_SERVICE_IO, ref Services, out Service);
            if (Res == wclErrors.WCL_E_SUCCESS)
            {
                wclGattCharacteristic[] Characteristics;
                Res = FClient.ReadCharacteristics(Service.Value, wclGattOperationFlag.goNone, out Characteristics);
                if (Res == wclErrors.WCL_E_SUCCESS)
                {
                    // All the IO characteristics are important!
                    Res = FindCharactersitc(WEDO_CHARACTERISTIC_SENSOR_VALUE, ref Characteristics, out SensorValueChar);
                    if (Res == wclErrors.WCL_E_SUCCESS)
                        Res = FindCharactersitc(WEDO_CHARACTERISTIC_SENSOR_VALUE_FORMAT, ref Characteristics, out SensorValueFormatChar);
                    if (Res == wclErrors.WCL_E_SUCCESS)
                        Res = FindCharactersitc(WEDO_CHARACTERISTIC_INPUT_COMMAND, ref Characteristics, out InputCommandChar);
                    if (Res == wclErrors.WCL_E_SUCCESS)
                        Res = FindCharactersitc(WEDO_CHARACTERISTIC_OUTPUT_COMMAND, ref Characteristics, out OutputCommandChar);

                    Characteristics = null;
                }
            }
            return Res;
        }

        private Int32 FindWeDoHubChars(ref wclGattService[] Services)
        {
            wclGattService? Service;
            Int32 Res = FindService(WEDO_SERVICE_HUB, ref Services, out Service);
            if (Res == wclErrors.WCL_E_SUCCESS)
            {
                wclGattCharacteristic[] Characteristics;
                Res = FClient.ReadCharacteristics(Service.Value, wclGattOperationFlag.goNone, out Characteristics);
                if (Res == wclErrors.WCL_E_SUCCESS)
                {
                    // The following characteristics are important so we have to check if they are present.
                    Res = FindCharactersitc(WEDO_CHARACTERISTIC_DEVICE_NAME, ref Characteristics, out DeviceNameChar);
                    if (Res == wclErrors.WCL_E_SUCCESS)
                        Res = FindCharactersitc(WEDO_CHARACTERISTIC_BUTTON_STATE, ref Characteristics, out ButtonStateChar);
                    if (Res == wclErrors.WCL_E_SUCCESS)
                        Res = FindCharactersitc(WEDO_CHARACTERISTIC_IO_ATTACHED, ref Characteristics, out IoAttachedChar);
                    if (Res == wclErrors.WCL_E_SUCCESS)
                        Res = FindCharactersitc(WEDO_CHARACTERISTIC_LOW_VOLTAGE_ALERT, ref Characteristics, out LowVoltageAlertChar);
                    if (Res == wclErrors.WCL_E_SUCCESS)
                        Res = FindCharactersitc(WEDO_CHARACTERISTIC_TURN_OFF, ref Characteristics, out TurnOffChar);

                    // The following characterisitcs may not be available so we ignore any errors. However we must check
                    // that previous characteristics (which are important) have been found.
                    if (Res == wclErrors.WCL_E_SUCCESS)
                    {
                        FindCharactersitc(WEDO_CHARACTERISTIC_HIGH_CURRENT_ALERT, ref Characteristics, out HighCurrentAleartChar);
                        FindCharactersitc(WEDO_CHARACTERISTIC_LOW_SIGNAL_ALERT, ref Characteristics, out LowSignalChar);
                        FindCharactersitc(WEDO_CHARACTERISTIC_VCC_PORT_CONTROL, ref Characteristics, out VccPortChar);
                        FindCharactersitc(WEDO_CHARACTERISTIC_BATTERY_TYPE, ref Characteristics, out BatteryTypeChar);
                        FindCharactersitc(WEDO_CHARACTERISTIC_DEVICE_DISCONNECT, ref Characteristics, out DeviceDisconnectChar);
                    }

                    Characteristics = null;
                }
            }
            return Res;
        }
        #endregion

        #region GATT client events.
        private void ClientCharacteristicChanged(object Sender, ushort Handle, byte[] Value)
        {
            throw new NotImplementedException();
        }

        private void ClientConnect(object Sender, int Error)
        {
            // If connection was not established simple fire the event with error code.
            if (Error != wclErrors.WCL_E_SUCCESS)
                DoConnected(Error);
            else
            {
                // If connection has been established we have to check services and rad all the required
                // characteristics.

                // First, read services.
                wclGattService[] Services;
                Int32 Res = FClient.ReadServices(wclGattOperationFlag.goNone, out Services);
                if (Res == wclErrors.WCL_E_SUCCESS)
                {
                    if (Services == null || Services.Length == 0)
                        Res = wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;
                    else
                    {
                        Res = FindDeviceInfoChars(ref Services);
                        if (Res == wclErrors.WCL_E_SUCCESS)
                        { 
                            Res = FindBatteryLevelChars(ref Services);
                            if (Res == wclErrors.WCL_E_SUCCESS)
                            {
                                Res = FindWeDoIoChars(ref Services);
                                if (Res == wclErrors.WCL_E_SUCCESS)
                                    Res = FindWeDoHubChars(ref Services);
                            }
                        }

                        // Do not forget to clean up services list.
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

        private void ClientDisconnect(object Sender, int Reason)
        {
            ResetCharacteristics();

            // We have to fire the event only if connection was really established and
            // all the services and characteristics were read.
            if (FConnected)
            {
                // Do not forget to reset connection state.
                FConnected = false;
                DoDisconnected(Reason);
            }
        }
        #endregion

        #region IDisposable
        /// <summary> Disposes the object.  </summary>
        /// <param name="disposing"> <para> If the parameter equals true, the method has been called directly
        ///   or indirectly by a user's code. Managed and unmanaged resources can be disposed. </para>
        ///   <para> If disposing equals false, the method has been called by the runtime from inside the
        ///   finalizer and you should not reference other objects. Only unmanaged resources can be
        ///   disposed. </para> </param>
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!FDisposed)
            {
                if (disposing)
                {
                    Disconnect();
                    FClient = null;
                }

                FDisposed = true;
            }
        }
        #endregion

        #region Event routines.
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
        #endregion

        #region Constructor and destructor(s)
        /// <summary> Creates new controller object. </summary>
        public WeDoController()
        {
            FConnected = false;
            FDisposed = false;

            FClient = new wclGattClient();
            FClient.OnCharacteristicChanged += ClientCharacteristicChanged;
            FClient.OnConnect += ClientConnect;            
            FClient.OnDisconnect += ClientDisconnect;

            ResetCharacteristics();

            OnConnected = null;
            OnDisconnected = null;
        }

        /// <summary> Finalizer. </summary>
        ~WeDoController()
        {
            Dispose(false);
        }

        /// <summary> Disposes the object. </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Connection management
        /// <summary> Connects to a selected WeDo device. </summary>
        /// <param name="Radio"> The <see cref="wclBluetoothRadio" /> object that should be used
        ///   for executing Bluetooth LE connection. </param>
        /// <param name="Address"> The WeDo device's address. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seealso cref="wclBluetoothRadio" />
        /// <exception cref="ObjectDisposedException"> The exception raises if an application calls the method
        ///   after object has been dispoised. </exception>
        public Int32 Connect(wclBluetoothRadio Radio, Int64 Address)
        {
            if (FDisposed)
                throw new ObjectDisposedException("WeDoController");

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
        /// <exception cref="ObjectDisposedException"> The exception raises if an application calls the method
        ///   after object has been dispoised. </exception>
        public Int32 Disconnect()
        {
            if (FDisposed)
                throw new ObjectDisposedException("WeDoController");

            return FClient.Disconnect();
        }
        #endregion

        #region Device Information properties
        /// <summary> Reads the firmware version. </summary>
        /// <param name="Version"> If the method completed with success the parameter contains the
        ///   current device's firmware version. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <exception cref="ObjectDisposedException"> The exception raises if an application calls the method
        ///   after object has been dispoised. </exception>
        public Int32 ReadFirmwareVersion(out String Version)
        {
            return ReadStringValue(FirmwareVersionChar, out Version);
        }

        /// <summary> Reads the hardware version. </summary>
        /// <param name="Version"> If the method completed with success the parameter contains the
        ///   current device's hardware version. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <exception cref="ObjectDisposedException"> The exception raises if an application calls the method
        ///   after object has been dispoised. </exception>
        public Int32 ReadHardwareVersion(out String Version)
        {
            return ReadStringValue(HardwareVersionChar, out Version);
        }

        /// <summary> Reads the software version. </summary>
        /// <param name="Version"> If the method completed with success the parameter contains the
        ///   current device's software version. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <exception cref="ObjectDisposedException"> The exception raises if an application calls the method
        ///   after object has been dispoised. </exception>
        public Int32 ReadSoftwareVersion(out String Version)
        {
            return ReadStringValue(SoftwareVersionChar, out Version);
        }

        /// <summary> Reads the device's manufacturer name. </summary>
        /// <param name="Name"> If the method completed with success the parameter contains the
        ///   current device's manufacturer name. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <exception cref="ObjectDisposedException"> The exception raises if an application calls the method
        ///   after object has been dispoised. </exception>
        public Int32 ReadManufacturerName(out String Name)
        {
            return ReadStringValue(ManufacturerNameChar, out Name);
        }
        #endregion

        #region Properties
        /// <summary> Gets connected status. </summary>
        /// <value> <c>true</c> if connected to WeDo device. </value>
        /// <exception cref="ObjectDisposedException"> The exception raises if an application calls the method
        ///   after object has been dispoised. </exception>
        public Boolean Connected
        {
            get
            {
                if (FDisposed)
                    throw new ObjectDisposedException("WeDoController");

                return FConnected;
            }
        }

        /// <summary> Gets internal GATT client state. </summary>
        /// <value> The internal GATT client state. </value>
        /// <seealso cref="wclClientState" />
        /// <exception cref="ObjectDisposedException"> The exception raises if an application calls the method
        ///   after object has been dispoised. </exception>
        public wclClientState ClientState
        {
            get
            {
                if (FDisposed)
                    throw new ObjectDisposedException("WeDoController");

                return FClient.State;
            }
        }
        #endregion

        #region Events
        /// <summary> The event fires when connection to a WeDo device
        ///   has been established. </summary>
        /// <seealso cref="wclClientConnectionConnectEvent" />
        public event wclClientConnectionConnectEvent OnConnected;
        /// <summary> The event fires when WeDo has been disconnected. </summary>
        /// <seealso cref="wclClientConnectionDisconnectEvent" />
        public event wclClientConnectionDisconnectEvent OnDisconnected;
        #endregion
    };
}
