using System;
using System.Collections.Generic;
using System.Linq;

using wclCommon;
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
            while (Index < Value.Length) {
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
