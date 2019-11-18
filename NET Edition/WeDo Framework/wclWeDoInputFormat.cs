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

namespace wclWeDoFramework
{
    /// <summary> This class describes a configuration of an Input (sensor). At any
    ///   time a sensor can be in just one mode, and the details of this mode is
    ///   captured by this structure. </summary>
    public sealed class wclWeDoInputFormat
    {
        private const Byte INPUT_FORMAT_PACKET_SIZE = 11;
        
        private const Byte WEDO_DATA_UNIT_RAW = 0;
        private const Byte WEDO_DATA_UNIT_PERCENTAGE = 1;
        private const Byte WEDO_DATA_UNIT_SI = 2;
        private const Byte WEDO_DATA_UNIT_UNKNOWN = 255;

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
                case wclWeDoIo.WEDO_DEVICE_MOTOR:
                    DeviceType = wclWeDoIoDeviceType.iodMotor;
                    break;
                case wclWeDoIo.WEDO_DEVICE_VOLTAGE_SENSOR:
                    DeviceType = wclWeDoIoDeviceType.iodVoltageSensor;
                    break;
                case wclWeDoIo.WEDO_DEVICE_CURRENT_SENSOR:
                    DeviceType = wclWeDoIoDeviceType.iodCurrentSensor;
                    break;
                case wclWeDoIo.WEDO_DEVICE_PIEZO:
                    DeviceType = wclWeDoIoDeviceType.iodPiezo;
                    break;
                case wclWeDoIo.WEDO_DEVICE_RGB:
                    DeviceType = wclWeDoIoDeviceType.iodRgb;
                    break;
                case wclWeDoIo.WEDO_DEVICE_TILT_SENSOR:
                    DeviceType = wclWeDoIoDeviceType.iodTiltSensor;
                    break;
                case wclWeDoIo.WEDO_DEVICE_MOTION_SENSOR:
                    DeviceType = wclWeDoIoDeviceType.iodMotionSensor;
                    break;
                default:
                    DeviceType = wclWeDoIoDeviceType.iodUnknown;
                    break;
            }

            wclWeDoSensorDataUnit Unit;
            switch (Data[8])
            {
                case WEDO_DATA_UNIT_RAW:
                    Unit = wclWeDoSensorDataUnit.suRaw;
                    break;
                case WEDO_DATA_UNIT_PERCENTAGE:
                    Unit = wclWeDoSensorDataUnit.suPercentage;
                    break;
                case WEDO_DATA_UNIT_SI:
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
                    DeviceType = wclWeDoIo.WEDO_DEVICE_MOTOR;
                    break;
                case wclWeDoIoDeviceType.iodVoltageSensor:
                    DeviceType = wclWeDoIo.WEDO_DEVICE_VOLTAGE_SENSOR;
                    break;
                case wclWeDoIoDeviceType.iodCurrentSensor:
                    DeviceType = wclWeDoIo.WEDO_DEVICE_CURRENT_SENSOR;
                    break;
                case wclWeDoIoDeviceType.iodPiezo:
                    DeviceType = wclWeDoIo.WEDO_DEVICE_PIEZO;
                    break;
                case wclWeDoIoDeviceType.iodRgb:
                    DeviceType = wclWeDoIo.WEDO_DEVICE_RGB;
                    break;
                case wclWeDoIoDeviceType.iodTiltSensor:
                    DeviceType = wclWeDoIo.WEDO_DEVICE_TILT_SENSOR;
                    break;
                case wclWeDoIoDeviceType.iodMotionSensor:
                    DeviceType = wclWeDoIo.WEDO_DEVICE_MOTION_SENSOR;
                    break;
                default:
                    DeviceType = wclWeDoIo.WEDO_DEVICE_UNKNOWN;
                    break;
            }

            Byte Unit;
            switch (FUnit)
            {
                case wclWeDoSensorDataUnit.suRaw:
                    Unit = WEDO_DATA_UNIT_RAW;
                    break;
                case wclWeDoSensorDataUnit.suPercentage:
                    Unit = WEDO_DATA_UNIT_PERCENTAGE;
                    break;
                case wclWeDoSensorDataUnit.suSi:
                    Unit = WEDO_DATA_UNIT_SI;
                    break;
                default:
                    Unit = WEDO_DATA_UNIT_UNKNOWN;
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
        /// <param name="obj"> The other object to be compared with current. </param>
        /// <returns> <c>True</c> if this input format is equal to <c>obj</c>.
        ///   <c>False</c> otherwise. </returns>
        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;

            wclWeDoInputFormat Format = (wclWeDoInputFormat)obj;
            return (FConnectionId == Format.ConnectionId && FInterval == Format.Interval &&
                FMode == Format.Mode && FNotificationsEnabled == Format.NotificationsEnabled &&
                FNumberOfBytes == Format.NumberOfBytes && FRevision == Format.Revision &&
                FDeviceType == Format.DeviceType && FUnit == Format.Unit);
        }

        /// <summary> Gets the hash code for the current format. </summary>
        /// <returns> The hash code. </returns>
        public override Int32 GetHashCode()
        {
            Int32 Hash = FConnectionId.GetHashCode();
            Hash = (Hash * 397) ^ FDeviceType.GetHashCode();
            Hash = (Hash * 397) ^ FInterval.GetHashCode();
            Hash = (Hash * 397) ^ FMode.GetHashCode();
            Hash = (Hash * 397) ^ FNotificationsEnabled.GetHashCode();
            Hash = (Hash * 397) ^ FNumberOfBytes.GetHashCode();
            Hash = (Hash * 397) ^ FRevision.GetHashCode();
            Hash = (Hash * 397) ^ FUnit.GetHashCode();
            return Hash;
        }

        /// <summary> Override th <c>==</c> operator. </summary>
        /// <param name="a"> The first argument for comparison. </param>
        /// <param name="b"> The second argument for comparison. </param>
        /// <returns> <c>True</c> if <c>a==b</c>. <c>False</c> otherwise. </returns>
        /// <seealso cref="wclWeDoInputFormat"/>
        public static Boolean operator ==(wclWeDoInputFormat a, wclWeDoInputFormat b)
        {
            return Equals(a, b);
        }

        /// <summary> Override th <c>!=</c> operator. </summary>
        /// <param name="a"> The first argument for comparison. </param>
        /// <param name="b"> The second argument for comparison. </param>
        /// <returns> <c>True</c> if <c>a!=b</c>. <c>False</c> otherwise. </returns>
        /// <seealso cref="wclWeDoInputFormat"/>
        public static Boolean operator !=(wclWeDoInputFormat a, wclWeDoInputFormat b)
        {
            return (!(a == b));
        }

        /// <summary> The Connect ID of the corresponding device. </summary>
        /// <value> The connect ID. </value>
        public Byte ConnectionId { get { return FConnectionId; } }
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
    };
}
