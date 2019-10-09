using System;

using wclCommon;
using wclCommunication;
using wclBluetooth;

namespace wclWeDoFramework
{
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
}
