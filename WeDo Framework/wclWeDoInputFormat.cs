using System;

namespace wclWeDoFramework
{
    /// <summary> This class describes a configuration of an Input (sensor). At any time a sensor can be in just one mode,
    ///   and the details of this mode is captured by this structure. </summary>
    public sealed class wclWeDoInputFormat
    {
        private const Byte INPUT_FORMAT_PACKET_SIZE = 11;

        private Byte FConnectionId;
        private wclWeDoIoDeviceType FDeviceType;
        private UInt32 FInterval;
        private Byte FMode;
        private Boolean FNotificationsEnabled;
        private Byte FNumberOfBytes;
        private Byte FRevision;
        private wclWeDoSensorDataUnit FUnit;

        internal static wclWeDoInputFormat FromBytesArray(Byte[] Data)
        {
            if (Data == null || Data.Length != INPUT_FORMAT_PACKET_SIZE)
                return null;

            Byte Revision = Data[0];
            Byte ConnectionId = Data[1];
            Byte Mode = Data[3];
            UInt32 Interval = BitConverter.ToUInt32(Data, 4);
            Boolean NotificationsEnabled = (Data[9] == 1);
            Byte NumberOfBytes = Data[10];

            wclWeDoIoDeviceType DeviceType;
            switch (Data[2])
            {
                case 1:
                    DeviceType = wclWeDoIoDeviceType.iodMotor;
                    break;
                case 20:
                    DeviceType = wclWeDoIoDeviceType.iodVoltageSensor;
                    break;
                case 21:
                    DeviceType = wclWeDoIoDeviceType.iodCurrentSensor;
                    break;
                case 22:
                    DeviceType = wclWeDoIoDeviceType.iodPiezo;
                    break;
                case 23:
                    DeviceType = wclWeDoIoDeviceType.iodRgb;
                    break;
                case 34:
                    DeviceType = wclWeDoIoDeviceType.iodTiltSensor;
                    break;
                case 35:
                    DeviceType = wclWeDoIoDeviceType.iodMotionSensor;
                    break;
                default:
                    DeviceType = wclWeDoIoDeviceType.iodUnknown;
                    break;
            }

            wclWeDoSensorDataUnit Unit;
            switch (Data[8])
            {
                case 0:
                    Unit = wclWeDoSensorDataUnit.suRaw;
                    break;
                case 1:
                    Unit = wclWeDoSensorDataUnit.suPercentage;
                    break;
                case 2:
                    Unit = wclWeDoSensorDataUnit.suSi;
                    break;
                default:
                    Unit = wclWeDoSensorDataUnit.suUnknown;
                    break;
            }

            return new wclWeDoInputFormat(ConnectionId, DeviceType, Mode, Interval, Unit,
                NotificationsEnabled, Revision, NumberOfBytes);
        }

        internal Byte[] ToBytesArray()
        {
            Byte[] Interval = BitConverter.GetBytes(FInterval);

            Byte DeviceType;
            switch (FDeviceType)
            {
                case wclWeDoIoDeviceType.iodMotor:
                    DeviceType = 1; ;
                    break;
                case wclWeDoIoDeviceType.iodVoltageSensor:
                    DeviceType = 20;
                    break;
                case wclWeDoIoDeviceType.iodCurrentSensor:
                    DeviceType = 21;
                    break;
                case wclWeDoIoDeviceType.iodPiezo:
                    DeviceType = 22;
                    break;
                case wclWeDoIoDeviceType.iodRgb:
                    DeviceType = 23;
                    break;
                case wclWeDoIoDeviceType.iodTiltSensor:
                    DeviceType = 34;
                    break;
                case wclWeDoIoDeviceType.iodMotionSensor:
                    DeviceType = 35;
                    break;
                default:
                    DeviceType = 255;
                    break;
            }

            Byte Unit;
            switch (FUnit)
            {
                case wclWeDoSensorDataUnit.suRaw:
                    Unit = 0;
                    break;
                case wclWeDoSensorDataUnit.suPercentage:
                    Unit = 1;
                    break;
                case wclWeDoSensorDataUnit.suSi:
                    Unit = 2;
                    break;
                default:
                    Unit = 255;
                    break;
            }

            Byte NotificationsEnabled = FNotificationsEnabled ? (Byte)1 : (Byte)0;
            Byte[] Data = new Byte[] {
                DeviceType,
                FMode,
                Interval[0],
                Interval[1],
                Interval[2],
                Interval[3],
                Unit,
                NotificationsEnabled };

            return Data;
        }

        internal wclWeDoInputFormat InputFormatBySettingMode(Byte Mode)
        {
            return new wclWeDoInputFormat(FConnectionId, FDeviceType, Mode, FInterval, FUnit,
                FNotificationsEnabled, FRevision, FNumberOfBytes);
        }

        /// <summary> Create a new instance of <c>wclWeDoInputFormat</c> class. </summary>
        /// <param name="ConnectionId"> The connection ID of the service.</param>
        /// <param name="DeviceType"> The type of the device. </param>
        /// <param name="Mode"> The mode of the device. </param>
        /// <param name="Interval"> The notifications interval. </param>
        /// <param name="Unit"> The unit the sensor should return values in. </param>
        /// <param name="NotificationsEnabled"> <c>True</c> if the device should send updates when the value changes. </param>
        /// <param name="Revision"> The Input Format revision. </param>
        /// <param name="NumberOfBytes"> The number of bytes in device's data packet. </param>
        /// <seealso cref="wclWeDoIoDeviceType"/>
        /// <seealso cref="wclWeDoSensorDataUnit"/>
        public wclWeDoInputFormat(Byte ConnectionId, wclWeDoIoDeviceType DeviceType, Byte Mode,
            UInt32 Interval, wclWeDoSensorDataUnit Unit, Boolean NotificationsEnabled, Byte Revision,
            Byte NumberOfBytes)
        {
            FConnectionId = ConnectionId;
            FInterval = Interval;
            FMode = Mode;
            FNotificationsEnabled = NotificationsEnabled;
            FDeviceType = DeviceType;
            FUnit = Unit;
            FRevision = Revision;
            FNumberOfBytes = NumberOfBytes;
        }

        /// <summary> Compares two Input Formats. </summary>
        /// <param name="obj"> The object to be compare with current one. </param>
        /// <returns> <c>True</c> if the Input Formats are equals. <c>False</c> otherwise. </returns>
        public override Boolean Equals(Object obj)
        {
            if (object.ReferenceEquals(obj, null))
                return false;
            if (GetType() != obj.GetType())
                return false;

            wclWeDoInputFormat Format = (wclWeDoInputFormat)obj;
            return (FConnectionId == Format.ConnectionId && FInterval == Format.Interval &&
                FMode == Format.Mode && FNotificationsEnabled == Format.NotificationsEnabled &&
                FNumberOfBytes == Format.NumberOfBytes && FRevision == Format.Revision &&
                FDeviceType == Format.DeviceType && FUnit == Format.Unit);
        }

        /// <summary> Gets the object hash. </summary>
        /// <returns> The objects hash. </returns>
        public override Int32 GetHashCode()
        {
            return FConnectionId.GetHashCode();
        }

        /// <summary> Override the <c>==</c> operator. </summary>
        public static Boolean operator ==(wclWeDoInputFormat a, wclWeDoInputFormat b)
        {
            if (Object.ReferenceEquals(a, null))
                return Object.ReferenceEquals(b, null);
            return a.Equals(b);
        }

        /// <summary> Override the <c>==</c> operator. </summary>
        public static Boolean operator !=(wclWeDoInputFormat a, wclWeDoInputFormat b)
        {
            return !(a == b);
        }

        /// <summary> The Connect ID of the corresponding device. </summary>
        /// <value> The connect ID. </value>
        public Byte ConnectionId {  get { return FConnectionId; } }
        /// <summary> Gets the device type of the Input Format. </summary>
        /// <value> The device type of the corresponding service. </value>
        /// <seealso cref="wclWeDoIoDeviceType"/>
        public wclWeDoIoDeviceType DeviceType { get { return FDeviceType; } }
        /// <summary> Gets The notifications interval. </summary>
        /// <value> The notifications interval. </value>
        /// <remarks> When notifications are enabled the device sends notifications if the value has change.
        ///   The interval indicates how fast/often updates will be send </remarks>
        public UInt32 Interval { get { return FInterval; } }
        /// <summary> Gets the Input mode. </summary>
        /// <value> The mode of the Input. </value>
        public Byte Mode { get { return FMode; } }
        /// <summary> Gets the notifications state. </summary>
        /// <value> <c>True</c> if new values are send whenever the value of the Input changes beyond
        ///   delta interval. </value>
        public Boolean NotificationsEnabled { get { return FNotificationsEnabled; } }
        /// <summary> Gets the number of bytes to be expected in the Input data payload (set by the Device). </summary>
        /// <value> The number of bytes. </value>
        public Byte NumberOfBytes { get { return FNumberOfBytes; } }
        /// <summary> Gets the Input Format revision. </summary>
        /// <value> The revision of the Input Format. </value>
        public Byte Revision { get { return FRevision; } }
        /// <summary> Gets the value unit. </summary>
        /// <value> The unit of the values. </value>
        /// <seealso cref="wclWeDoSensorDataUnit"/>
        public wclWeDoSensorDataUnit Unit { get { return FUnit; } }

        /*
        /// <summary>
        /// Creates a copy of this input format with a new delta interval
        /// </summary>
        /// <param name="interval">The new delta interval</param>
        /// <returns>new Input format with new delta interval</returns>
        public InputFormat InputFormatBySettingDeltaInterval(uint interval)
        {
            return new InputFormat()
            {
                ConnectId = ConnectId,
                DeltaInterval = interval,
                Mode = Mode,
                NotificationsEnabled = NotificationsEnabled,
                TypeId = TypeId,
                Unit = Unit,
            };
        }

        /// <summary>
        /// Creates a copy of this input format with a new <paramref name="mode"/> and <paramref name="unit"/>
        /// </summary>
        /// <param name="mode">The new mode</param>
        /// <param name="unit">The new unit</param>
        /// <returns>new Input format with new mode and unit</returns>
        public InputFormat InputFormatBySettingMode(int mode, InputFormatUnit unit)
        {
            return new InputFormat()
            {
                ConnectId = ConnectId,
                DeltaInterval = DeltaInterval,
                Mode = mode,
                NotificationsEnabled = NotificationsEnabled,
                TypeId = TypeId,
                Unit = unit,
            };
        }

        /// <summary>
        /// Creates a copy of this input format with a new value for notifications enabled
        /// </summary>
        /// <param name="notificationsEnabled">YES if the sensor should send updates when the value changes</param>
        /// <returns>new Input format with new notifications enabled</returns>
        public InputFormat InputFormatBySettingNotificationsEnabled(bool notificationsEnabled)
        {
            return new InputFormat()
            {
                ConnectId = ConnectId,
                DeltaInterval = DeltaInterval,
                Mode = Mode,
                NotificationsEnabled = notificationsEnabled,
                TypeId = TypeId,
                Unit = Unit,
            };
        }

        */
    };
}
