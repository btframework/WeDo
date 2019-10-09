using System;

using wclCommon;
using wclCommunication;
using wclBluetooth;

namespace wclWeDoFramework
{
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

        // Piezo constants.
        private const UInt16 PIEZO_MAX_FREQUENCY = 1500;
        private const UInt16 PIEZO_MAX_DURATION = 65535;

        private wclGattCharacteristic? FSensorValueChar;
        private wclGattCharacteristic? FSensorValueFormatChar;
        private wclGattCharacteristic? FInputCommandChar;
        private wclGattCharacteristic? FOutputCommandChar;

        private Int32 WriteOutputCommand(Byte[] Command)
        {
            // Writes command to the IO service.
            if (FOutputCommandChar == null)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;

            return Client.WriteCharacteristicValue(FOutputCommandChar.Value, Command);
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
                UnsubscribeFromNotifications(FSensorValueChar);
                UnsubscribeFromNotifications(FSensorValueFormatChar);
            }

            FSensorValueChar = null;
            FSensorValueFormatChar = null;
            FInputCommandChar = null;
            FOutputCommandChar = null;
        }

        /// <summary> This method called internally by the <see cref="wclWeDoHub"/>
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
        /// <param name="Hub"> The <see cref="wclWeDoHub"/> object that owns the service. </param>
        /// <exception cref="wclEInvalidArgument"> The exception raises if the <c>Client</c> or <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoIoService(wclGattClient Client, wclWeDoHub Hub)
            : base(Client, Hub)
        {
            Uninitialize();
        }

        /// <summary> Plays a tone with a given frequency for the given duration in ms. </summary>
		/// <param name="Frequency"> The frequency to play (max allowed frequency is 1500). </param>
		/// <param name="Duration"> The duration to play (max supported is 65535 milli seconds). </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 PlayTone(UInt16 Frequency, UInt16 Duration)
        {
            if (Frequency > PIEZO_MAX_FREQUENCY || Duration > PIEZO_MAX_DURATION)
                return wclErrors.WCL_E_INVALID_ARGUMENT;

            return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_FEATURE_NOT_SUPPORTED;
        }
    };
}
