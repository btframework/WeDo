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
        /// <exception cref="wclEInvalidArgument"> The exception raises if the <c>Client</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoIoService(wclGattClient Client)
            : base(Client)
        {
            Uninitialize();
        }
    };
}
