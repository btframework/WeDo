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

        private const Byte CMD_HDR_SIZE = 3;
        private const Byte CMD_ID_MOTOR_POWER_CONTROL = 1;
        private const Byte CMD_ID_PIEZO_PLAY_TONE = 2;
        private const Byte CMD_ID_PIEZO_STOP = 3;
        private const Byte CMD_ID_RGB_CONTROL = 4;
        private const Byte CMD_ID_DIRECT_WRITE = 5;

        private wclGattCharacteristic? FSensorValueChar;
        private wclGattCharacteristic? FSensorValueFormatChar;
        private wclGattCharacteristic? FInputCommandChar;
        private wclGattCharacteristic? FOutputCommandChar;

        private Byte[] ComposeWriteCommand(Byte CommandId, Byte PortId, Byte[] Data)
        {
            if (Data != null && Data.Length > 255)
                return null;

            Int32 CmdLen = CMD_HDR_SIZE;
            if (Data != null && Data.Length > 0)
                CmdLen = CmdLen + Data.Length;
            Byte[] Cmd = new byte[CmdLen];
            Cmd[0] = PortId;
            Cmd[1] = CommandId;
            if (Data == null || Data.Length == 0)
                Cmd[2] = 0;
            else
            {
                Cmd[2] = (Byte)Data.Length;
                for (Int32 i = 0; i < Data.Length; i++)
                    Cmd[3 + i] = Data[i];
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

        internal Int32 PiezoPlayTone(UInt16 Frequency, UInt16 Duration, Byte PortId)
        {
            Byte[] FreqencyBytes = BitConverter.GetBytes(Frequency);
            Byte[] DurationBytes = BitConverter.GetBytes(Duration);
            Byte[] Data = new Byte[4];
            Data[0] = FreqencyBytes[0];
            Data[1] = FreqencyBytes[1];
            Data[2] = DurationBytes[0];
            Data[3] = DurationBytes[1];
            Byte[] Cmd = ComposeWriteCommand(CMD_ID_PIEZO_PLAY_TONE, PortId, Data);
            return WriteOutputCommand(Cmd);
        }

        internal Int32 PiezoStopPlaying(Byte PortId)
        {
            Byte[] Cmd = ComposeWriteCommand(CMD_ID_PIEZO_STOP, PortId, null);
            return WriteOutputCommand(Cmd);
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
    };
}
