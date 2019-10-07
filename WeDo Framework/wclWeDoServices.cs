using System;
using System.Text;

using wclCommon;
using wclCommunication;
using wclBluetooth;

namespace wclWeDo
{
    /// <summary> The base class for all WeDo services. </summary>
    public abstract class wclWeDoService
    {
        private wclGattClient FClient;
        private Boolean FConnected;

        /// <summary> Converts <see cref="wclGattUuid"/> type to standard system <see cref="Guid"/>. </summary>
        /// <param name="Uuid"> The <see cref="wclGattUuid"/> that should be converted. </param>
        /// <returns> The <see cref="Guid"/> composed from the <c>Uuid</c>. </returns>
        /// <seealso cref="Guid"/>
        /// <seealso cref="wclGattUuid"/>
        protected Guid ToGuid(wclGattUuid Uuid)
        {
            if (!Uuid.IsShortUuid)
                return Uuid.LongUuid;

            Byte[] Bytes = wclUUIDs.Bluetooth_Base_UUID.ToByteArray();
            Int16 Data2 = BitConverter.ToInt16(Bytes, 4);
            Int16 Data3 = BitConverter.ToInt16(Bytes, 6);
            Byte[] Data4 = new Byte[8];
            Buffer.BlockCopy(Bytes, 8, Data4, 0, 8);
            return new Guid(Uuid.ShortUuid, Data2, Data3, Data4);
        }

        /// <summary> Compares the attribute's <see cref="wclGattUuid"/> with given standard
        ///   system <see cref="Guid"/>. </summary>
        /// <param name="GattUuid"> The attribute's <see cref="wclGattUuid"/>. </param>
        /// <param name="Uuid">The system <see cref="Guid"/>. </param>
        /// <returns> Returns <c>true</c> if the attribute's UUID is equals to the GUID.
        ///   Returns <c>false</c> otherwise. </returns>
        /// <seealso cref="Guid"/>
        /// <seealso cref="wclGattUuid"/>
        protected Boolean CompareGuid(wclGattUuid GattUuid, Guid Uuid)
        {
            return (ToGuid(GattUuid) == Uuid);
        }

        /// <summary> Finds the service with given UUID. </summary>
        /// <param name="Uuid"> The service's UUID. </param>
        /// <param name="Service"> If the method completed with success the parameter
        ///   contains found service. Otherwise the parameter is <c>null</c>. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seealso cref="Guid"/>
        /// <seealso cref="wclGattService"/>
        protected Int32 FindService(Guid Uuid, out wclGattService? Service)
        {
            Service = null;

            // First we have to read services from the connected device.
            // It is a bit stupid imp,ementation but this makes code clear.
            // Any way, once we use goNone flag the connection to the device wilkl be executed only once
            // and each next call to this method will use cached services list. So it should not be too
            // slow.
            wclGattService[] Services;
            Int32 Res = FClient.ReadServices(wclGattOperationFlag.goNone, out Services);
            if (Res != wclErrors.WCL_E_SUCCESS)
                return Res;

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

        /// <summary> Finds the characteristic with given UUID. </summary>
        /// <param name="Uuid"> The characteristic's UUID. </param>
        /// <param name="Service"> The GATT service that should contain the required
        ///   characteristic. </param>
        /// <param name="Characteristic"> If the method completed with success the
        ///   parameter contains the found characteristic. Otherwise the parameter is <c>null</c>. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seealso cref="Guid"/>
        /// <seealso cref="wclGattService"/>
        /// <seealso cref="wclGattCharacteristic"/>
        protected Int32 FindCharactersitc(Guid Uuid, wclGattService? Service,
            out wclGattCharacteristic? Characteristic)
        {
            Characteristic = null;

            if (Service == null)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;

            // Read characteristics from device. Once we use goNone flag the characteristics
            // will be cached at first read so it should not be too slow in case we will call
            // this method few times for the same service.
            wclGattCharacteristic[] Characteristics;
            Int32 Res = FClient.ReadCharacteristics(Service.Value, wclGattOperationFlag.goNone,
                out Characteristics);
            if (Res != wclErrors.WCL_E_SUCCESS)
                return Res;

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

        /// <summary> Subscribes to the changes notifications of the given characteristic. </summary>
        /// <param name="Characteristic"> The characteristic. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seealso cref="wclGattCharacteristic"/>
        protected Int32 SubscribeForNotifications(wclGattCharacteristic? Characteristic)
        {
            if (Characteristic == null)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;

            // Windows does not support dual indicatable and notifiable chars so select one.
            wclGattCharacteristic chr = Characteristic.Value;
            if (chr.IsIndicatable && chr.IsNotifiable)
                chr.IsIndicatable = false;
            Int32 Res = FClient.Subscribe(chr);
            if (Res == wclErrors.WCL_E_SUCCESS)
            {
                Res = FClient.WriteClientConfiguration(chr, true, wclGattOperationFlag.goNone);
                if (Res != wclErrors.WCL_E_SUCCESS)
                    FClient.Unsubscribe(chr);
            }
            return Res;
        }

        /// <summary> Unsubscribes from the changes notifications of the given characteristic. </summary>
        /// <seealso cref="wclGattCharacteristic"/>
        protected void UnsubscribeFromNotifications(wclGattCharacteristic? Characteristic)
        {
            if (FClient.State == wclClientState.csConnected && Characteristic != null)
            {
                wclGattCharacteristic chr = Characteristic.Value;
                if (chr.IsIndicatable && chr.IsNotifiable)
                    chr.IsIndicatable = false;
                FClient.WriteClientConfiguration(chr, false, wclGattOperationFlag.goNone);
                FClient.Unsubscribe(chr);
            }
        }

        /// <summary> Reads string value from the given characteristic. </summary>
        /// <param name="Characteristic"> The GATT characteristic. </param>
        /// <param name="Value"> If the method completed with success contains the read value. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        protected Int32 ReadStringValue(wclGattCharacteristic? Characteristic, out String Value)
        {
            Value = "";

            if (!FConnected)
                return wclConnectionErrors.WCL_E_CONNECTION_NOT_ACTIVE;
            if (Characteristic == null)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;

            Byte[] CharValue;
            Int32 Res = FClient.ReadCharacteristicValue(Characteristic.Value, wclGattOperationFlag.goNone,
                out CharValue);
            if (Res == wclErrors.WCL_E_SUCCESS)
            {
                if (CharValue != null && CharValue.Length > 0)
                    Value = Encoding.UTF8.GetString(CharValue);
            }
            return Res;
        }

        /// <summary> Reads byte value from the given characteristic. </summary>
        /// <param name="Characteristic"> The GATT characteristic. </param>
        /// <param name="Value"> If the method completed with success contains the read value. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        protected Int32 ReadByteValue(wclGattCharacteristic? Characteristic, out Byte Value)
        {
            Value = 0;

            if (!FConnected)
                return wclConnectionErrors.WCL_E_CONNECTION_NOT_ACTIVE;
            if (Characteristic == null)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;

            Byte[] CharValue;
            Int32 Res = FClient.ReadCharacteristicValue(Characteristic.Value, wclGattOperationFlag.goNone,
                out CharValue);
            if (Res == wclErrors.WCL_E_SUCCESS)
            {
                if (CharValue != null && CharValue.Length > 0)
                    Value = CharValue[0];
            }
            return Res;
        }

        /// <summary> Initializes the WeDo service. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <remarks> A derived clases must override this method to initialize all required
        ///   parameters to work with WeDo service. </remarks>
        protected abstract Int32 Initialize();

        /// <summary> Uninitializes the WeDo service. </summary>
        /// <remarks> A derived clases must override this method to cleanup allocated resources. </remarks>
        protected abstract void Uninitialize();

        /// <summary> Gets the GATT client object. </summary>
        /// <value> The GATT client object. </value>
        /// <seealso cref="wclGattClient"/>
        protected internal wclGattClient Client { get { return FClient; } }

        /// <summary> This method called internally by the <see cref="wclWeDoControl"/>
        ///   to notify about characteristic changes. A derived class may override this method
        ///   to check for required characteristic changes. </summary>
        /// <param name="Handle"> The characteristic handle. </param>
        /// <param name="Value"> The new characteristic value. </param>
        internal virtual void CharacteristicChanged(UInt16 Handle, Byte[] Value)
        {
            // Do nothing in default implementation.
        }

        /// <summary> Creates new WeDo Service Client object. </summary>
        /// <param name="Client"> The <see cref="wclGattClient"/> object that handles the connection
        ///   to a WeDo device. </param>
        /// <exception cref="wclEInvalidArgument"> The exception raises if the <c>Client</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoService(wclGattClient Client)
        {
            if (Client == null)
                throw new wclEInvalidArgument("Client parameter can not be null.");

            FClient = Client;
            FConnected = false;
        }

        /// <summary> Connects to a WeDo service. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <remarks> This method used internally and anapplication must not call this method
        ///   direcly. </remarks>
        public Int32 Connect()
        {
            if (FConnected)
                return wclConnectionErrors.WCL_E_CONNECTION_ACTIVE;

            Int32 Res = Initialize();
            if (Res == wclErrors.WCL_E_SUCCESS)
                FConnected = true;
            return Res;
        }

        /// <summary> Disconnects from the service that is handled by the object.  </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <remarks> This method used internally and anapplication must not call this method
        ///   direcly. </remarks>
        public Int32 Disconnect()
        {
            if (!FConnected)
                return wclConnectionErrors.WCL_E_CONNECTION_NOT_ACTIVE;

            Uninitialize();

            FConnected = false;
            return wclErrors.WCL_E_SUCCESS;
        }

        /// <summary> Gets the connected state of the service. </summary>
        /// <value> <c>true</c> of the service has been connected. </value>
        public Boolean Connected { get { return FConnected; } }
    };

    /// <summary> The class represents the WeDo Device Information service. </summary>
    /// <seealso cref="wclWeDoService"/>
    public class wclWeDoDeviceInformationService : wclWeDoService
    {
        // Standard Bluetooth LE Device Information Service.
        private static Guid WEDO_SERVICE_DEVICE_INFORMATION = new Guid("0000180a-0000-1000-8000-00805f9b34fb");

        // Firmware Revision characteristic. [Optional] [Readable]
        private static Guid WEDO_CHARACTERISTIC_FIRMWARE_REVISION = new Guid("00002a26-0000-1000-8000-00805f9b34fb");
        // Firmware Revision characteristic. [Optional] [Readable]
        private static Guid WEDO_CHARACTERISTIC_HARDWARE_REVISION = new Guid("00002a27-0000-1000-8000-00805f9b34fb");
        // Software Revision characteristic. [Optional] [Readable]
        private static Guid WEDO_CHARACTERISTIC_SOFTWARE_REVISION = new Guid("00002a28-0000-1000-8000-00805f9b34fb");
        // Manufacturer Name characteristic. [Optional] [Readable]
        private static Guid WEDO_CHARACTERISTIC_MANUFACTURER_NAME = new Guid("00002a29-0000-1000-8000-00805f9b34fb");

        private wclGattCharacteristic? FFirmwareVersionChar;
        private wclGattCharacteristic? FHardwareVersionChar;
        private wclGattCharacteristic? FSoftwareVersionChar;
        private wclGattCharacteristic? FManufacturerNameChar;

        /// <summary> Initializes the WeDo service. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        protected override Int32 Initialize()
        {
            // Find Device Information service and its characteristics.
            wclGattService? Service;
            Int32 Res = FindService(WEDO_SERVICE_DEVICE_INFORMATION, out Service);
            if (Res == wclErrors.WCL_E_SUCCESS)
            {
                // These characteristics are not important so we can ignore errors if some was not found.
                FindCharactersitc(WEDO_CHARACTERISTIC_FIRMWARE_REVISION, Service, out FFirmwareVersionChar);
                FindCharactersitc(WEDO_CHARACTERISTIC_HARDWARE_REVISION, Service, out FHardwareVersionChar);
                FindCharactersitc(WEDO_CHARACTERISTIC_SOFTWARE_REVISION, Service, out FSoftwareVersionChar);
                FindCharactersitc(WEDO_CHARACTERISTIC_MANUFACTURER_NAME, Service, out FManufacturerNameChar);
            }
            return Res;
        }

        /// <summary> Uninitializes the WeDo service. </summary>
        protected override void Uninitialize()
        {
            FFirmwareVersionChar = null;
            FHardwareVersionChar = null;
            FSoftwareVersionChar = null;
            FManufacturerNameChar = null;
        }

        /// <summary> Creates new Device Information service client. </summary>
        /// <param name="Client"> The <see cref="wclGattClient"/> object that handles the connection
        ///   to a WeDo device. </param>
        /// <exception cref="wclEInvalidArgument"> The exception raises if the <c>Client</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoDeviceInformationService(wclGattClient Client)
            : base(Client)
        {
            Uninitialize();
        }

        /// <summary> Reads the firmware version. </summary>
        /// <param name="Version"> If the method completed with success the parameter contains the
        ///   current device's firmware version. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 ReadFirmwareVersion(out String Version)
        {
            return ReadStringValue(FFirmwareVersionChar, out Version);
        }

        /// <summary> Reads the hardware version. </summary>
        /// <param name="Version"> If the method completed with success the parameter contains the
        ///   current device's hardware version. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 ReadHardwareVersion(out String Version)
        {
            return ReadStringValue(FHardwareVersionChar, out Version);
        }

        /// <summary> Reads the software version. </summary>
        /// <param name="Version"> If the method completed with success the parameter contains the
        ///   current device's software version. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 ReadSoftwareVersion(out String Version)
        {
            return ReadStringValue(FSoftwareVersionChar, out Version);
        }

        /// <summary> Reads the device's manufacturer name. </summary>
        /// <param name="Name"> If the method completed with success the parameter contains the
        ///   current device's manufacturer name. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 ReadManufacturerName(out String Name)
        {
            return ReadStringValue(FManufacturerNameChar, out Name);
        }
    };

    /// <summary> The <c>OnBatteryLevelChanged</c> event handler prototype. </summary>
    /// <param name="Sender"> The object that fired the event. </param>
    /// <param name="Level"> The current battery level in percents in range 0-100. </param>
    public delegate void wclBatteryLevelChangedEvent(Object Sender, Byte Level);

    /// <summary> The class represents the WeDo Battery Level service. </summary>
    /// <seealso cref="wclWeDoService"/>
    public class wclWeDoBatteryLevelService : wclWeDoService
    {
        // Standard Bluetooth LE Battery Level Service.
        private static Guid WEDO_SERVICE_BATTERY_LEVEL = new Guid("0000180f-0000-1000-8000-00805f9b34fb");

        // Battery level characteristic. [Mandatory] [Readable, Notifiable]
        private static Guid WEDO_CHARACTERISTIC_BATTERY_LEVEL = new Guid("00002a19-0000-1000-8000-00805f9b34fb");

        private wclGattCharacteristic? FBatteryLevelChar;

        /// <summary> Fires the <c>OnBatteryLevelChanged</c> event. </summary>
        /// <param name="Level"> The current battery level in percents in range 0-100. </param>
        protected virtual void DoBatteryLevelChanged(Byte Level)
        {
            if (OnBatteryLevelChanged != null)
                OnBatteryLevelChanged(this, Level);
        }

        /// <summary> Initializes the WeDo service. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        protected override Int32 Initialize()
        {
            // Find Battery Level service and its characteristics.
            wclGattService? Service;
            Int32 Res = FindService(WEDO_SERVICE_BATTERY_LEVEL, out Service);
            if (Res == wclErrors.WCL_E_SUCCESS)
            {
                Res = FindCharactersitc(WEDO_CHARACTERISTIC_BATTERY_LEVEL, Service, out FBatteryLevelChar);
                if (Res == wclErrors.WCL_E_SUCCESS)
                {
                    Res = SubscribeForNotifications(FBatteryLevelChar);
                    if (Res != wclErrors.WCL_E_SUCCESS)
                        Uninitialize();
                }
            }
            return Res;
        }

        /// <summary> Uninitializes the WeDo service. </summary>
        protected override void Uninitialize()
        {
            if (Connected)
                UnsubscribeFromNotifications(FBatteryLevelChar);

            FBatteryLevelChar = null;
        }

        /// <summary> This method called internally by the <see cref="wclWeDoControl"/>
        ///   to notify about characteristic changes. A derived class may override this method
        ///   to check for required characteristic changes. </summary>
        /// <param name="Handle"> The characteristic handle. </param>
        /// <param name="Value"> The new characteristic value. </param>
        internal override void CharacteristicChanged(UInt16 Handle, Byte[] Value)
        {
            // We have to process battery level changes notifications here.
            if (Handle == FBatteryLevelChar.Value.Handle)
                DoBatteryLevelChanged(Value[0]);
        }

        /// <summary> Creates new Battery Level service client. </summary>
        /// <param name="Client"> The <see cref="wclGattClient"/> object that handles the connection
        ///   to a WeDo device. </param>
        /// <exception cref="wclEInvalidArgument"> The exception raises if the <c>Client</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoBatteryLevelService(wclGattClient Client)
            : base(Client)
        {
            Uninitialize();

            OnBatteryLevelChanged = null;
        }

        /// <summary> Reads the device's battery level. </summary>
        /// <param name="Level"> the current battery level in percents. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 ReadBatteryLevel(out Byte Level)
        {
            return ReadByteValue(FBatteryLevelChar, out Level);
        }

        /// <summary> The event fires when the battery level has been changed. </summary>
        /// <seealso cref="wclBatteryLevelChangedEvent" />
        public event wclBatteryLevelChangedEvent OnBatteryLevelChanged;
    };

    /// <summary> The class represents the WeDo IO service. </summary>
    /// <seealso cref="wclWeDoService"/>
    public class wclWeDoIoService : wclWeDoService
    {
        // WeDo IO Service
        private static Guid WEDO_SERVICE_IO = new Guid("{00004f0e-1212-efde-1523-785feabcd123}");

        // Sensor Value characteristic. [Mandatory] [Readable, Notifiable]
        private static Guid WEDO_CHARACTERISTIC_SENSOR_VALUE = new Guid("00001560-1212-efde-1523-785feabcd123");
        // Sensor Value Format characteristic. [Mandatory] [Notifiable]
        private static Guid WEDO_CHARACTERISTIC_SENSOR_VALUE_FORMAT = new Guid("00001561-1212-efde-1523-785feabcd123");
        // Input command characteristic. [Mandatory] [Writable, Writable Without Response]
        private static Guid WEDO_CHARACTERISTIC_INPUT_COMMAND = new Guid("00001563-1212-efde-1523-785feabcd123");
        // Output command characteristic. [Mandatory] [Writable, Writable Without Response]
        private static Guid WEDO_CHARACTERISTIC_OUTPUT_COMMAND = new Guid("00001565-1212-efde-1523-785feabcd123");

        private wclGattCharacteristic? FSensorValueChar;
        private wclGattCharacteristic? FSensorValueFormatChar;
        private wclGattCharacteristic? FInputCommandChar;
        private wclGattCharacteristic? FOutputCommandChar;

        /// <summary> Initializes the WeDo service. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        protected override Int32 Initialize()
        {
            // Find Battery Level service and its characteristics.
            wclGattService? Service;
            Int32 Res = FindService(WEDO_SERVICE_IO, out Service);
            if (Res == wclErrors.WCL_E_SUCCESS)
            {
                // All the IO characteristics are important!
                Res = FindCharactersitc(WEDO_CHARACTERISTIC_SENSOR_VALUE, Service, out FSensorValueChar);
                if (Res == wclErrors.WCL_E_SUCCESS)
                    Res = FindCharactersitc(WEDO_CHARACTERISTIC_SENSOR_VALUE_FORMAT, Service, out FSensorValueFormatChar);
                if (Res == wclErrors.WCL_E_SUCCESS)
                    Res = FindCharactersitc(WEDO_CHARACTERISTIC_INPUT_COMMAND, Service, out FInputCommandChar);
                if (Res == wclErrors.WCL_E_SUCCESS)
                    Res = FindCharactersitc(WEDO_CHARACTERISTIC_OUTPUT_COMMAND, Service, out FOutputCommandChar);
                if (Res == wclErrors.WCL_E_SUCCESS)
                {
                    Res = SubscribeForNotifications(FSensorValueChar);
                    if (Res == wclErrors.WCL_E_SUCCESS)
                    {
                        Res = SubscribeForNotifications(FSensorValueFormatChar);
                        if (Res != wclErrors.WCL_E_SUCCESS)
                            UnsubscribeFromNotifications(FSensorValueChar);
                    }
                }

                if (Res != wclErrors.WCL_E_SUCCESS)
                    Uninitialize();
            }
            return Res;
        }

        /// <summary> Uninitializes the WeDo service. </summary>
        protected override void Uninitialize()
        {
            if (Connected)
            {
                UnsubscribeFromNotifications(FSensorValueChar);
                UnsubscribeFromNotifications(FSensorValueFormatChar);
            }

            FSensorValueChar = null;
            FSensorValueFormatChar = null;
            FInputCommandChar = null;
            FOutputCommandChar = null;
        }

        /// <summary> This method called internally by the <see cref="wclWeDoControl"/>
        ///   to notify about characteristic changes. A derived class may override this method
        ///   to check for required characteristic changes. </summary>
        /// <param name="Handle"> The characteristic handle. </param>
        /// <param name="Value"> The new characteristic value. </param>
        internal override void CharacteristicChanged(UInt16 Handle, Byte[] Value)
        {
            
        }

        /// <summary> Creates new IO service client. </summary>
        /// <param name="Client"> The <see cref="wclGattClient"/> object that handles the connection
        ///   to a WeDo device. </param>
        /// <exception cref="wclEInvalidArgument"> The exception raises if the <c>Client</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoIoService(wclGattClient Client)
            : base(Client)
        {
            Uninitialize();
        }
    };

    /// <summary> The class represents the WeDo Hub service. </summary>
    /// <seealso cref="wclWeDoService"/>
    public class wclWeDoHubService : wclWeDoService
    {
        // WeDo HUB service.
        private static Guid WEDO_SERVICE_HUB = new Guid("00001523-1212-efde-1523-785feabcd123");

        // Device name characteristic. [Mandatory] [Readable, Writable]
        private static Guid WEDO_CHARACTERISTIC_DEVICE_NAME = new Guid("00001524-1212-efde-1523-785feabcd123");
        // Buttons state characteristic. [Mandatory] [Readable, Notifiable]
        private static Guid WEDO_CHARACTERISTIC_BUTTON_STATE = new Guid("00001526-1212-efde-1523-785feabcd123");
        // IO attached charactrisitc. [Mandatory] [Notifiable]
        private static Guid WEDO_CHARACTERISTIC_IO_ATTACHED = new Guid("00001527-1212-efde-1523-785feabcd123");
        // Low Voltrage Alert characteristic. [Mandatory] [Readable, Notifiable]
        private static Guid WEDO_CHARACTERISTIC_LOW_VOLTAGE_ALERT = new Guid("00001528-1212-efde-1523-785feabcd123");
        // High Current Aleart characteristic. [Optional] [Readable, Notifiable]
        private static Guid WEDO_CHARACTERISTIC_HIGH_CURRENT_ALERT = new Guid("00001529-1212-efde-1523-785feabcd123");
        // Low Signal Aleart characteristic. [Optional] [Readable, Notifiable]
        private static Guid WEDO_CHARACTERISTIC_LOW_SIGNAL_ALERT = new Guid("0000152a-1212-efde-1523-785feabcd123");
        // Turn Off command characteristic. [Mandatory] [Writable]
        private static Guid WEDO_CHARACTERISTIC_TURN_OFF = new Guid("0000152b-1212-efde-1523-785feabcd123");
        // VCC Port Control characteristic. [Optional] [Readable, Writable]
        private static Guid WEDO_CHARACTERISTIC_VCC_PORT_CONTROL = new Guid("0000152c-1212-efde-1523-785feabcd123");
        // Battery Type characteristic. [Optional] [Readable]
        private static Guid WEDO_CHARACTERISTIC_BATTERY_TYPE = new Guid("0000152d-1212-efde-1523-785feabcd123");
        // Device Disconnect Cpmmand characteristic. [Optional] [Writable]
        private static Guid WEDO_CHARACTERISTIC_DEVICE_DISCONNECT = new Guid("0000152e-1212-efde-1523-785feabcd123");

        private wclGattCharacteristic? FDeviceNameChar;
        private wclGattCharacteristic? FButtonStateChar;
        private wclGattCharacteristic? FIoAttachedChar;
        private wclGattCharacteristic? FLowVoltageAlertChar;
        private wclGattCharacteristic? FHighCurrentAleartChar;
        private wclGattCharacteristic? FLowSignalChar;
        private wclGattCharacteristic? FTurnOffChar;
        private wclGattCharacteristic? FVccPortChar;
        private wclGattCharacteristic? FBatteryTypeChar;
        private wclGattCharacteristic? FDeviceDisconnectChar;

        /// <summary> Initializes the WeDo service. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        protected override Int32 Initialize()
        {
            // Find Battery Level service and its characteristics.
            wclGattService? Service;
            Int32 Res = FindService(WEDO_SERVICE_HUB, out Service);
            if (Res == wclErrors.WCL_E_SUCCESS)
            {
                // The following characteristics are important so we have to check if they are present.
                Res = FindCharactersitc(WEDO_CHARACTERISTIC_DEVICE_NAME, Service, out FDeviceNameChar);
                if (Res == wclErrors.WCL_E_SUCCESS)
                    Res = FindCharactersitc(WEDO_CHARACTERISTIC_BUTTON_STATE, Service, out FButtonStateChar);
                if (Res == wclErrors.WCL_E_SUCCESS)
                    Res = FindCharactersitc(WEDO_CHARACTERISTIC_IO_ATTACHED, Service, out FIoAttachedChar);
                if (Res == wclErrors.WCL_E_SUCCESS)
                    Res = FindCharactersitc(WEDO_CHARACTERISTIC_LOW_VOLTAGE_ALERT, Service, out FLowVoltageAlertChar);
                if (Res == wclErrors.WCL_E_SUCCESS)
                    Res = FindCharactersitc(WEDO_CHARACTERISTIC_TURN_OFF, Service, out FTurnOffChar);

                // Subscribe for mandatory chars.
                if (Res == wclErrors.WCL_E_SUCCESS)
                {
                    Res = SubscribeForNotifications(FButtonStateChar);
                    if (Res == wclErrors.WCL_E_SUCCESS)
                    {
                        Res = SubscribeForNotifications(FIoAttachedChar);
                        if (Res == wclErrors.WCL_E_SUCCESS)
                        {
                            Res = SubscribeForNotifications(FLowVoltageAlertChar);
                            if (Res != wclErrors.WCL_E_SUCCESS)
                                UnsubscribeFromNotifications(FIoAttachedChar);
                        }
                        if (Res != wclErrors.WCL_E_SUCCESS)
                            UnsubscribeFromNotifications(FButtonStateChar);
                    }
                }

                // The following characterisitcs may not be available so we ignore any errors. However we must check
                // that previous characteristics (which are important) have been found.
                if (Res == wclErrors.WCL_E_SUCCESS)
                {
                    FindCharactersitc(WEDO_CHARACTERISTIC_HIGH_CURRENT_ALERT, Service, out FHighCurrentAleartChar);
                    FindCharactersitc(WEDO_CHARACTERISTIC_LOW_SIGNAL_ALERT, Service, out FLowSignalChar);
                    FindCharactersitc(WEDO_CHARACTERISTIC_VCC_PORT_CONTROL, Service, out FVccPortChar);
                    FindCharactersitc(WEDO_CHARACTERISTIC_BATTERY_TYPE, Service, out FBatteryTypeChar);
                    FindCharactersitc(WEDO_CHARACTERISTIC_DEVICE_DISCONNECT, Service, out FDeviceDisconnectChar);

                    // Subscribe for optional chars.
                    if (FHighCurrentAleartChar != null)
                        Res = SubscribeForNotifications(FHighCurrentAleartChar);
                    if (Res == wclErrors.WCL_E_SUCCESS && FLowSignalChar != null)
                    {
                        Res = SubscribeForNotifications(FLowSignalChar);
                        if (Res != wclErrors.WCL_E_SUCCESS && FHighCurrentAleartChar != null)
                            UnsubscribeFromNotifications(FHighCurrentAleartChar);
                    }
                }

                if (Res != wclErrors.WCL_E_SUCCESS)
                {
                    UnsubscribeFromNotifications(FButtonStateChar);
                    UnsubscribeFromNotifications(FIoAttachedChar);
                    UnsubscribeFromNotifications(FLowVoltageAlertChar);

                    Uninitialize();
                }
            }
            return Res;
        }

        /// <summary> Uninitializes the WeDo service. </summary>
        protected override void Uninitialize()
        {
            if (Connected)
            {
                // Mandatory characteristics.
                UnsubscribeFromNotifications(FButtonStateChar);
                UnsubscribeFromNotifications(FIoAttachedChar);
                UnsubscribeFromNotifications(FLowVoltageAlertChar);

                // Optional characteristics.
                if (FHighCurrentAleartChar != null)
                    UnsubscribeFromNotifications(FHighCurrentAleartChar);
                if (FLowSignalChar != null)
                    UnsubscribeFromNotifications(FLowSignalChar);
            }

            FDeviceNameChar = null;
            FButtonStateChar = null;
            FIoAttachedChar = null;
            FLowVoltageAlertChar = null;
            FHighCurrentAleartChar = null;
            FLowSignalChar = null;
            FTurnOffChar = null;
            FVccPortChar = null;
            FBatteryTypeChar = null;
            FDeviceDisconnectChar = null;
        }

        /// <summary> This method called internally by the <see cref="wclWeDoControl"/>
        ///   to notify about characteristic changes. A derived class may override this method
        ///   to check for required characteristic changes. </summary>
        /// <param name="Handle"> The characteristic handle. </param>
        /// <param name="Value"> The new characteristic value. </param>
        internal override void CharacteristicChanged(UInt16 Handle, Byte[] Value)
        {

        }

        /// <summary> Creates new IO service client. </summary>
        /// <param name="Client"> The <see cref="wclGattClient"/> object that handles the connection
        ///   to a WeDo device. </param>
        /// <exception cref="wclEInvalidArgument"> The exception raises if the <c>Client</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoHubService(wclGattClient Client)
            : base(Client)
        {
            Uninitialize();
        }

        /// <summary> Reads the current device name. </summary>
        /// <param name="Name"> If the method completed with success the parameter contains the
        ///   current device name. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 ReadDeviceName(out String Name)
        {
            return ReadStringValue(FDeviceNameChar, out Name);
        }

        /// <summary> Writes new device name. </summary>
        /// <param name="Name"> The new device name. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 WriteDeviceName(String Name)
        {
            if (Name == null || Name == "")
                return wclErrors.WCL_E_INVALID_ARGUMENT;

            if (!Connected)
                return wclConnectionErrors.WCL_E_CONNECTION_NOT_ACTIVE;

            if (Name.Length > 20)
                Name = Name.Substring(0, 20);
            Byte[] Bytes = Encoding.UTF8.GetBytes(Name);
            Byte[] CharVal;
            if (Bytes.Length < 20)
            {
                CharVal = new Byte[20];
                for (Int32 i = 0; i < Bytes.Length; i++)
                    CharVal[i] = Bytes[i];
                for (Int32 i = Bytes.Length; i < 20; i++)
                    CharVal[i] = 0;
            }
            else
                CharVal = Bytes;

            return Client.WriteCharacteristicValue(FDeviceNameChar.Value, CharVal);
        }
    };
}
