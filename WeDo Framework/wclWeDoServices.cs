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
using System.Text;
using System.Linq;
using System.Collections.Generic;

using wclCommon;
using wclCommunication;
using wclBluetooth;

namespace wclWeDoFramework
{
    /// <summary> The base class for all WeDo services. </summary>
    public abstract class wclWeDoService
    {
        private wclGattCharacteristic[] FCharacteristics;
        private wclGattClient FClient;
        private Boolean FConnected;
        private wclWeDoHub FHub;
        private wclGattService[] FServices;

        private Int32 ReadCharacteristics(wclGattService Service)
        {
            // Did we already read the characteristics for given service?
            if (FCharacteristics != null)
                return wclErrors.WCL_E_SUCCESS;

            Int32 Res = FClient.ReadCharacteristics(Service, wclGattOperationFlag.goNone, out FCharacteristics);
            if (Res != wclErrors.WCL_E_SUCCESS)
                return Res;
            if (FCharacteristics == null)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;

            return wclErrors.WCL_E_SUCCESS;
        }

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

            if (FServices == null)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;

            foreach (wclGattService svc in FServices)
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

            Int32 Res = ReadCharacteristics(Service.Value);
            if (Res != wclErrors.WCL_E_SUCCESS)
                return Res;

            foreach (wclGattCharacteristic chr in FCharacteristics)
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
        /// <param name="Characteristic"> The characteristic to unsubsribe. </param>
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

        /// <summary> This method called internally by the <see cref="wclWeDoHub"/>
        ///   to notify about characteristic changes. A derived class may override this method
        ///   to check for required characteristic changes. </summary>
        /// <param name="Handle"> The characteristic handle. </param>
        /// <param name="Value"> The new characteristic value. </param>
        internal virtual void CharacteristicChanged(UInt16 Handle, Byte[] Value)
        {
            // Do nothing in default implementation.
        }

        internal Int32 Connect(wclGattService[] Services)
        {
            if (FConnected)
                return wclConnectionErrors.WCL_E_CONNECTION_ACTIVE;
            if (Services == null)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;

            FServices = Services;
            Int32 Res = Initialize();
            if (Res == wclErrors.WCL_E_SUCCESS)
                FConnected = true;
            return Res;
        }

        internal Int32 Disconnect()
        {
            if (!FConnected)
                return wclConnectionErrors.WCL_E_CONNECTION_NOT_ACTIVE;

            Uninitialize();

            FCharacteristics = null;
            FConnected = false;
            FServices = null;

            return wclErrors.WCL_E_SUCCESS;
        }

        internal Boolean Connected { get { return FConnected; } }

        /// <summary> Creates new WeDo Service Client object. </summary>
        /// <param name="Client"> The <see cref="wclGattClient"/> object that handles the connection
        ///   to a WeDo device. </param>
        /// <param name="Hub"> The <see cref="wclWeDoHub"/> object that owns the service. </param>
        /// <exception cref="wclEInvalidArgument"> The exception raises if the <c>Client</c> or <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoService(wclGattClient Client, wclWeDoHub Hub)
        {
            if (Client == null || Hub == null)
                throw new wclEInvalidArgument("Client parameter can not be null.");

            FCharacteristics = null;
            FClient = Client;
            FConnected = false;
            FHub = Hub;
            FServices = null;
        }

        /// <summary> Gets the <see cref="wclWeDoHub"/> object that owns the service. </summary>
        /// <value> The <see cref="wclWeDoHub"/> object. </value>
        /// <seealso cref="wclWeDoHub"/>
        public wclWeDoHub Hub { get { return FHub; } }
    };

    /// <summary> The <c>OnButtonStateChanged</c> event handler prototype. </summary>
    /// <param name="Sender"> The object that fires the event. </param>
    /// <param name="Pressed"> The button's state. <c>True</c> if button has been pressed.
    ///   <c>False</c> if button has been released. </param>
    public delegate void wclWeDoHubButtonStateChangedEvent(Object Sender, Boolean Pressed);
    /// <summary> The event handler prototype for alert events. </summary>
    /// <param name="Sender"> The object that fires the event. </param>
    /// <param name="Alert"> <c>True</c> if the alert is active. <c>False</c> otherwise. </param>
    public delegate void wclWeDoHubAlertEvent(Object Sender, Boolean Alert);
    /// <summary> The <c>OnDeviceAttached</c> and <c>OnDeviceDetached</c> events handler prototype. </summary>
    /// <param name="Sender"> The object that fires the event. </param>
    /// <param name="Device"> The Input/Output device object. </param>
    /// <seealso cref="wclWeDoIo"/>
    public delegate void wclWeDoDeviceStateChangedEvent(Object Sender, wclWeDoIo Device);

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
        /// <param name="Hub"> The <see cref="wclWeDoHub"/> object that owns the service. </param>
        /// <exception cref="wclEInvalidArgument"> The exception raises if the <c>Client</c> or <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoDeviceInformationService(wclGattClient Client, wclWeDoHub Hub)
            : base(Client, Hub)
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

        /// <summary> This method called internally by the <see cref="wclWeDoHub"/>
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
        /// <param name="Hub"> The <see cref="wclWeDoHub"/> object that owns the service. </param>
        /// <exception cref="wclEInvalidArgument"> The exception raises if the <c>Client</c> or <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoBatteryLevelService(wclGattClient Client, wclWeDoHub Hub)
            : base(Client, Hub)
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
        // Turn Off command characteristic. [Mandatory] [Writable]
        private static Guid WEDO_CHARACTERISTIC_TURN_OFF = new Guid("0000152b-1212-efde-1523-785feabcd123");

        // High Current Aleart characteristic. [Optional] [Readable, Notifiable]
        private static Guid WEDO_CHARACTERISTIC_HIGH_CURRENT_ALERT = new Guid("00001529-1212-efde-1523-785feabcd123");
        // Low Signal Aleart characteristic. [Optional] [Readable, Notifiable]
        private static Guid WEDO_CHARACTERISTIC_LOW_SIGNAL_ALERT = new Guid("0000152a-1212-efde-1523-785feabcd123");
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
        private wclGattCharacteristic? FTurnOffChar;

        private wclGattCharacteristic? FHighCurrentAleartChar;
        private wclGattCharacteristic? FLowSignalChar;
        private wclGattCharacteristic? FVccPortChar;
        private wclGattCharacteristic? FBatteryTypeChar;
        private wclGattCharacteristic? FDeviceDisconnectChar;

        internal delegate void wclWeDoHubDeviceDetachedEvent(Object Sender, Byte ConnectionId);

        internal Int32 ReadDeviceName(out String Name)
        {
            return ReadStringValue(FDeviceNameChar, out Name);
        }

        internal Int32 WriteDeviceName(String Name)
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

        internal Int32 TurnOff()
        {
            if (!Connected)
                return wclConnectionErrors.WCL_E_CONNECTION_NOT_ACTIVE;

            Byte[] Value = new Byte[1];
            Value[0] = 0x01;
            return Client.WriteCharacteristicValue(FTurnOffChar.Value, Value);
        }

        internal event wclWeDoHubButtonStateChangedEvent OnButtonStateChanged;
        internal event wclWeDoDeviceStateChangedEvent OnDeviceAttached;
        internal event wclWeDoHubDeviceDetachedEvent OnDeviceDetached;
        internal event wclWeDoHubAlertEvent OnHighCurrentAlert;
        internal event wclWeDoHubAlertEvent OnLowVoltageAlert;
        internal event wclWeDoHubAlertEvent OnLowSignalAlert;

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

        /// <summary> This method called internally by the <see cref="wclWeDoHub"/>
        ///   to notify about characteristic changes. A derived class may override this method
        ///   to check for required characteristic changes. </summary>
        /// <param name="Handle"> The characteristic handle. </param>
        /// <param name="Value"> The new characteristic value. </param>
        internal override void CharacteristicChanged(UInt16 Handle, Byte[] Value)
        {
            // Process data only if it presents.
            if (Value != null && Value.Length > 0)
            {
                // Button pressed?
                if (FButtonStateChar != null && Handle == FButtonStateChar.Value.Handle)
                    DoButtonStateChanged(Value[0] == 1);
                // Low voltage?
                if (FLowVoltageAlertChar != null && Handle == FLowVoltageAlertChar.Value.Handle)
                    DoLowVoltageAlert(Value[0] == 1);
                // High current!
                if (FHighCurrentAleartChar != null && Handle == FHighCurrentAleartChar.Value.Handle)
                    DoHightCurrentAlert(Value[0] == 1);
                // Low signal
                if (FLowSignalChar != null && Handle == FLowSignalChar.Value.Handle)
                    DoLowSignalAlert(Value[0] == 1);
                // IO attached/detached
                if (FIoAttachedChar != null && Handle == FIoAttachedChar.Value.Handle)
                {
                    if (Value.Length >= 2)
                    {
                        if (Value[1] == 1)
                        {
                            // Attached
                            wclWeDoIo Io = wclWeDoIo.Attach(Hub, Value);
                            if (Io != null)
                                DoDeviceAttached(Io);
                        }
                        else
                        {
                            // Detached.
                            DoDeviceDetached(Value[0]);
                        }
                    }
                }
            }
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

        /// <summary> Fires the <c>OnDeviceAttached</c> event. </summary>
        /// <param name="Device"> The IO device object. </param>
        /// <seealso cref="wclWeDoIo"/>
        protected virtual void DoDeviceAttached(wclWeDoIo Device)
        {
            if (OnDeviceAttached != null)
                OnDeviceAttached(this, Device);
        }

        /// <summary> Fires the <c>OnDeviceDetached</c> event. </summary>
        /// <param name="ConnectionId"> The device connection ID. </param>
        protected virtual void DoDeviceDetached(Byte ConnectionId)
        {
            if (OnDeviceDetached != null)
                OnDeviceDetached(this, ConnectionId);
        }

        /// <summary> Fires then <c>OnHighCurrentAlert</c> event.</summary>
        /// <param name="Alert"> <c>True</c> if device runs on high current. <c>False</c> otherwise. </param>
        protected virtual void DoHightCurrentAlert(Boolean Alert)
        {
            if (OnHighCurrentAlert != null)
                OnHighCurrentAlert(this, Alert);
        }

        /// <summary> Fires then <c>OnLowSignalAlert</c> event.</summary>
        /// <param name="Alert"> <c>True</c> if the signal from radio has low RSSI. <c>False</c> otherwise. </param>
        protected virtual void DoLowSignalAlert(Boolean Alert)
        {
            if (OnLowSignalAlert != null)
                OnLowSignalAlert(this, Alert);
        }

        /// <summary> Creates new IO service client. </summary>
        /// <param name="Client"> The <see cref="wclGattClient"/> object that handles the connection
        ///   to a WeDo device. </param>
        /// <param name="Hub"> The <see cref="wclWeDoHub"/> object that owns the service. </param>
        /// <exception cref="wclEInvalidArgument"> The exception raises if the <c>Client</c> or <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoHubService(wclGattClient Client, wclWeDoHub Hub)
            : base(Client, Hub)
        {
            OnButtonStateChanged = null;
            OnLowVoltageAlert = null;
            OnDeviceAttached = null;
            OnDeviceDetached = null;
            OnHighCurrentAlert = null;
            OnLowSignalAlert = null;

            Uninitialize();
        }
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

        private const Byte OUT_CMD_HDR_SIZE = 3;
        private const Byte OUT_CMD_ID_MOTOR_POWER_CONTROL = 1;
        private const Byte OUT_CMD_ID_PIEZO_PLAY_TONE = 2;
        private const Byte OUT_CMD_ID_PIEZO_STOP = 3;
        private const Byte OUT_CMD_ID_RGB_CONTROL = 4;
        private const Byte OUT_CMD_ID_DIRECT_WRITE = 5;

        private const Byte IN_CMD_HDR_SIZE = 3;
        private const Byte IN_CMD_TYPE_READ = 1;
        private const Byte IN_CMD_TYPE_WRITE = 2;
        private const Byte IN_CMD_ID_INPUT_VALUE = 0;
        private const Byte IN_CMD_ID_INPUT_FORMAT = 1;

        // Local list of ALL input formats of the WeDo HUB.
        private Dictionary<Byte, wclWeDoInputFormat> FInputFormats;
        // When an input format is missing (value received does not have a valid input format)
        // this dictionary sets it's value to true to signal that a request for a new input
        // format was received.
        private Dictionary<Byte, Boolean> FMissingInputFormats;

        private wclGattCharacteristic? FSensorValueChar;
        private wclGattCharacteristic? FSensorValueFormatChar;
        private wclGattCharacteristic? FInputCommandChar;
        private wclGattCharacteristic? FOutputCommandChar;

        private Byte[] ComposeOutputCommand(Byte CommandId, Byte ConnectionId, Byte[] Data)
        {
            if (Data != null && Data.Length > 255)
                return null;

            Int32 CmdLen = OUT_CMD_HDR_SIZE;
            if (Data != null && Data.Length > 0)
                CmdLen += Data.Length;
            Byte[] Cmd = new byte[CmdLen];
            Cmd[0] = ConnectionId;
            Cmd[1] = CommandId;
            if (Data == null || Data.Length == 0)
                Cmd[2] = 0;
            else
            {
                Cmd[2] = (Byte)Data.Length;
                for (Int32 i = 0; i < Data.Length; i++)
                    Cmd[OUT_CMD_HDR_SIZE + i] = Data[i];
            }
            return Cmd;
        }

        private Byte[] ComposeInputCommand(Byte CommandId, Byte CommandType, Byte ConnectionId, Byte[] Data)
        {
            if (Data != null && Data.Length > 255)
                return null;

            Int32 CmdLen = IN_CMD_HDR_SIZE;
            if (Data != null && Data.Length > 0)
                CmdLen += Data.Length;
            Byte[] Cmd = new byte[CmdLen];
            Cmd[0] = CommandId;
            Cmd[1] = CommandType;
            Cmd[2] = ConnectionId;
            if (Data != null && Data.Length > 0)
            {
                for (Int32 i = 0; i < Data.Length; i++)
                    Cmd[IN_CMD_HDR_SIZE + i] = Data[i];
            }
            return Cmd;
        }

        private Int32 WriteOutputCommand(Byte[] Command)
        {
            // Writes command to the IO service.
            if (FOutputCommandChar == null)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;

            return Client.WriteCharacteristicValue(FOutputCommandChar.Value, Command);
        }

        private Int32 WriteInputCommand(Byte[] Command)
        {
            if (FInputCommandChar == null)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;

            return Client.WriteCharacteristicValue(FInputCommandChar.Value, Command);
        }

        private void RequestMissingInputFormat(Byte ConnectionId)
        {
            if (!FMissingInputFormats.ContainsKey(ConnectionId))
                FMissingInputFormats.Add(ConnectionId, false);
            // Have we already requested for missing Input Format?
            Boolean InputFormatRequested = FMissingInputFormats[ConnectionId];
            if (!InputFormatRequested)
            {
                // If no - do it right now.
                if (ReadInputFormat(ConnectionId) == wclErrors.WCL_E_SUCCESS)
                    InputFormatRequested = true;
            }
            // Change Input Format request flag.
            FMissingInputFormats[ConnectionId] = InputFormatRequested;
        }

        private void InputValueChanged(Byte[] Value)
        {
            if (Value == null || Value.Length == 0)
                return;

            Byte Revision = Value[0];
            Int32 Index = 1;
            Dictionary<Byte, Byte[]> IdToValue = new Dictionary<Byte, Byte[]>();
            List<Byte> List = Value.ToList();

            // Iterate over values in byte array until byte array is empty.
            while (Index < Value.Length)
            {
                Byte ConnectionId = List[Index];

                wclWeDoInputFormat Format;
                // If value has Connection ID that is not known by the system - ignore.
                if (!FInputFormats.TryGetValue(ConnectionId, out Format))
                {
                    RequestMissingInputFormat(ConnectionId);
                    return;
                }

                // If no input format is available for this Connection ID - ignore.
                if (Format == null)
                    return;

                // If the revision from the input value is different than the revision from the input format - ignore.
                if (Format.Revision != Revision)
                    return;

                // Read data value.
                Index++;
                Byte[] Data = List.GetRange(Index, Format.NumberOfBytes).ToArray();
                Index += Format.NumberOfBytes;
                IdToValue.Add(ConnectionId, Data);
            }

            // Update value son devices.
            foreach (wclWeDoIo Io in Hub.IoDevices)
            {
                Byte[] Data;
                if (IdToValue.TryGetValue(Io.ConnectionId, out Data))
                    Io.UpdateValue(Data);
            }
        }

        private void InputFormatChanged(Byte[] Data)
        {
            wclWeDoInputFormat Format = wclWeDoInputFormat.FromBytesArray(Data);
            if (Format == null)
                return;

            // If we already have input format with an earlier revision, delete all those
            // as all known formats must have the same version.
            wclWeDoInputFormat AnyFormat = FInputFormats.Values.FirstOrDefault();
            // Clear if revisions are not equal.
            if (AnyFormat != null && AnyFormat.Revision != Format.Revision)
                FInputFormats.Clear();

            // Update input formats in local list.
            if (FInputFormats.ContainsKey(Format.ConnectionId))
                FInputFormats[Format.ConnectionId] = Format;
            else
                FInputFormats.Add(Format.ConnectionId, Format);

            // Check for missing Input Formats.
            if (FMissingInputFormats.ContainsKey(Format.ConnectionId))
                FMissingInputFormats[Format.ConnectionId] = false;
            else
                FMissingInputFormats.Add(Format.ConnectionId, false);

            // Update Input format for IOs.
            foreach (wclWeDoIo Io in Hub.IoDevices)
            {
                if (Io.ConnectionId == Format.ConnectionId)
                    Io.UpdateInputFormat(Format);
            }
        }

        internal Int32 PiezoPlayTone(UInt16 Frequency, UInt16 Duration, Byte ConnectionId)
        {
            Byte[] FreqencyBytes = BitConverter.GetBytes(Frequency);
            Byte[] DurationBytes = BitConverter.GetBytes(Duration);
            Byte[] Data = new Byte[4];
            Data[0] = FreqencyBytes[0];
            Data[1] = FreqencyBytes[1];
            Data[2] = DurationBytes[0];
            Data[3] = DurationBytes[1];
            Byte[] Cmd = ComposeOutputCommand(OUT_CMD_ID_PIEZO_PLAY_TONE, ConnectionId, Data);
            return WriteOutputCommand(Cmd);
        }

        internal Int32 PiezoStopPlaying(Byte ConnectionId)
        {
            Byte[] Cmd = ComposeOutputCommand(OUT_CMD_ID_PIEZO_STOP, ConnectionId, null);
            return WriteOutputCommand(Cmd);
        }

        internal Int32 WriteData(Byte[] Data, Byte ConnectionId)
        {
            Byte[] Cmd = ComposeOutputCommand(OUT_CMD_ID_DIRECT_WRITE, ConnectionId, Data);
            return WriteOutputCommand(Cmd);
        }

        internal Int32 WriteMotorPower(SByte Power, Byte Offset, Byte ConnectionId)
        {
            Boolean Positive = (Power >= 0);
            Power = Math.Abs(Power);

            float ActualPower = ((100.0f - Offset) / 100.0f) * Power + Offset;
            Byte Value = (Byte)Math.Round(ActualPower);

            if (!Positive)
                Value = (Byte)((0xFF - Value) + 1);

            Byte[] Data = new Byte[1];
            Data[0] = Value;
            Byte[] Cmd = ComposeOutputCommand(OUT_CMD_ID_MOTOR_POWER_CONTROL, ConnectionId, Data);
            return WriteOutputCommand(Cmd);
        }

        internal Int32 WriteMotorPower(SByte Power, Byte ConnectionId)
        {
            return WriteMotorPower(Power, 0, ConnectionId);
        }

        internal Int32 WriteColor(Byte Red, Byte Green, Byte Blue, Byte ConnectionId)
        {
            Byte[] Data = new Byte[3];
            Data[0] = Red;
            Data[1] = Green;
            Data[2] = Blue;
            Byte[] Cmd = ComposeOutputCommand(OUT_CMD_ID_RGB_CONTROL, ConnectionId, Data);
            return WriteOutputCommand(Cmd);
        }

        internal Int32 WriteColorIndex(Byte Index, Byte ConnectionId)
        {
            if (Index > 10)
                return wclErrors.WCL_E_INVALID_ARGUMENT;

            Byte[] Data = new Byte[1];
            Data[0] = Index;
            Byte[] Cmd = ComposeOutputCommand(OUT_CMD_ID_RGB_CONTROL, ConnectionId, Data);
            return WriteOutputCommand(Cmd);
        }

        internal Int32 ReadValue(Byte ConnectionId)
        {
            Byte[] Cmd = ComposeInputCommand(IN_CMD_ID_INPUT_VALUE, IN_CMD_TYPE_READ, ConnectionId, null);
            return WriteInputCommand(Cmd);
        }

        internal Int32 WriteInputFormat(wclWeDoInputFormat Format, Byte ConnectionId)
        {
            Byte[] Cmd = ComposeInputCommand(IN_CMD_ID_INPUT_FORMAT, IN_CMD_TYPE_WRITE, ConnectionId,
                Format.ToBytesArray());
            return WriteInputCommand(Cmd);
        }

        internal Int32 ReadInputFormat(Byte ConnectionId)
        {
            Byte[] Cmd = ComposeInputCommand(IN_CMD_ID_INPUT_FORMAT, IN_CMD_TYPE_READ, ConnectionId, null);
            return WriteInputCommand(Cmd);
        }

        internal Int32 ResetIo(Byte ConnectionId)
        {
            Byte[] Cmd = new Byte[3];
            Cmd[0] = 68;
            Cmd[1] = 17;
            Cmd[2] = 170;
            return WriteData(Cmd, ConnectionId);
        }

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
                // Unsubscribe from all characteristics.
                UnsubscribeFromNotifications(FSensorValueChar);
                UnsubscribeFromNotifications(FSensorValueFormatChar);
            }

            // Clear characteristics.
            FSensorValueChar = null;
            FSensorValueFormatChar = null;
            FInputCommandChar = null;
            FOutputCommandChar = null;

            // Clear input format lists.
            FInputFormats.Clear();
            FMissingInputFormats.Clear();
        }

        /// <summary> This method called internally by the <see cref="wclWeDoHub"/>
        ///   to notify about characteristic changes. A derived class may override this method
        ///   to check for required characteristic changes. </summary>
        /// <param name="Handle"> The characteristic handle. </param>
        /// <param name="Value"> The new characteristic value. </param>
        internal override void CharacteristicChanged(UInt16 Handle, Byte[] Value)
        {
            if (FSensorValueChar != null && FSensorValueChar.Value.Handle == Handle)
                InputValueChanged(Value);
            if (FSensorValueFormatChar != null && FSensorValueFormatChar.Value.Handle == Handle)
                InputFormatChanged(Value);
        }

        /// <summary> Creates new IO service client. </summary>
        /// <param name="Client"> The <see cref="wclGattClient"/> object that handles the connection
        ///   to a WeDo device. </param>
        /// <param name="Hub"> The <see cref="wclWeDoHub"/> object that owns the service. </param>
        /// <exception cref="wclEInvalidArgument"> The exception raises if the <c>Client</c> or <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoIoService(wclGattClient Client, wclWeDoHub Hub)
            : base(Client, Hub)
        {
            // Create input formats lists first.
            FInputFormats = new Dictionary<Byte, wclWeDoInputFormat>();
            FMissingInputFormats = new Dictionary<Byte, Boolean>();

            Uninitialize();
        }
    };
}
