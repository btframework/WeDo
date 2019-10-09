using System;

using wclCommon;

namespace wclWeDoFramework
{
    /// <summary> The enumeration describes the WeDo Input/Output device types. </summary>
    public enum wclWeDoIoType
    {
        /// <summary> The device is internal. </summary>
        ioInternal,
        /// <summary> The device is external. </summary>
        ioExternal
    };

    /// <summary> Represents the type of an attached IO (motor, sensor, etc). </summary>
	public enum wclWeDoIoDevice
    {
        /// <summary> A Motor. </summary>
        iodMotor,
        /// <summary> A Voltage Sensor. </summary>
        iodVoltageSensor,
        /// <summary> A Current Sensor. </summary>
        iodCurrentSensor,
        /// <summary> A Piezo Tone player. </summary>
        iodPiezo,
        /// <summary> An RGB light. </summary>
        iodRgb,
        /// <summary> A Tilt Sensor. </summary>
        iodTiltSensor,
        /// <summary> A Motion Sensor (aka. Detect Sensor). </summary>
        iodMotionSensor,
        /// <summary> A type is unknown. </summary>
        iodUnknown
    };

    /// <summary> The class represets an attached Input/Outpout device. </summary>
    public abstract class wclWeDoIo
    {
        private Boolean FAttached;
        private Byte FConnectionId;
        private wclWeDoIoDevice FDevice;
        private String FFirmwareVersion;
        private String FHardwareVersion;
        private wclWeDoHub FHub;
        private Byte FPortId;
        private wclWeDoIoType FType;

        internal static wclWeDoIo Attach(wclWeDoHub Hub, Byte[] RawInfo)
        {
            if (RawInfo == null || RawInfo.Length < 2)
                return null;
            if ((RawInfo[1] == 1 && RawInfo.Length != 12) || (RawInfo[1] == 0 && RawInfo.Length != 2))
                return null;
            if (RawInfo[1] == 0)
                // Detached???
                return null;

            wclWeDoIo Io = null;
            switch (RawInfo[3])
            {
                /*case 1:
                    connectInfo.TypeEnum = IoType.IoTypeMotor;*/
                /*case 20:
                    connectInfo.TypeEnum = IoType.IoTypeVoltage;*/
                /*case 21:
                connectInfo.TypeEnum = IoType.IoTypeCurrent;*/
                case 22:
                    Io = new wclWeDoPieazo(Hub);
                    Io.FDevice = wclWeDoIoDevice.iodPiezo;
                    Io.FType = wclWeDoIoType.ioInternal;
                    break;

                    /*case 23:
                    connectInfo.TypeEnum = IoType.IoTypeRgbLight;*/
                    /*case 34:
                    connectInfo.TypeEnum = IoType.IoTypeTiltSensor;*/
                    /*case 35:
                    connectInfo.TypeEnum = IoType.IoTypeMotionSensor;*/
                    /*default:
                    connectInfo.TypeEnum = IoType.IoTypeGeneric;*/
            }

            if (Io != null)
            {
                Io.FConnectionId = RawInfo[0];
                Io.FPortId = RawInfo[2];
                Io.FHardwareVersion = RawInfo[4].ToString() + RawInfo[5].ToString() + RawInfo[6].ToString() + RawInfo[7].ToString();
                Io.FFirmwareVersion = RawInfo[8].ToString() + RawInfo[9].ToString() + RawInfo[10].ToString() + RawInfo[11].ToString();
            }

            return Io;
        }


        internal void Detach()
        {
            FAttached = false;
        }

        /// <summary> Creates new IO device object. </summary>
        /// <param name="Hub"> The Hub object that owns the device. If this parameter is <c>null</c>
        ///   the <seealso cref="wclEInvalidArgument"/> exception raises. </param>
        /// <exception cref="wclEInvalidArgument"> The exception raises when the <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoIo(wclWeDoHub Hub)
        {
            if (Hub == null)
                throw new wclEInvalidArgument("Hub parameter can not be null.");

            FAttached = true; // It is always attached on creation!
            FHub = Hub;
        }

        /// <summary> Gets the IO device state. </summary>
        /// <value> <c>True</c> if the device is attached. <c>False</c> if the device is detached. </value>
        public Boolean Attached { get { return FAttached; } }
        /// <summary> Gets the IO connection ID. </summary>
        /// <value> The IO connection ID. </value>
        /// <remarks> It is guarateed that the connection ID is unique. </remarks>
        public Byte ConnectionId { get { return FConnectionId; } }
        /// <sumnmary> Gets the device represented by this object. </sumnmary>
        /// <value> The IO device type. </value>
        /// <seealso cref="wclWeDoIoDevice"/>
        public wclWeDoIoDevice Device { get { return FDevice; } }
        /// <summary> Gets the IO device firmware version. </summary>
        /// <value> The IO device firmware version. </value>
		public String FirmwareVersion { get { return FFirmwareVersion; } }
        /// <summary> Gets the IO device hardware version. </summary>
        /// <value> The IO device hardware version. </value>
        public String HardwareVersion { get { return FHardwareVersion; } }
        /// <summary> Gets the WeDo Hub object that owns the IO device. </summary>
        /// <value> The WeDo Hub object. </value>
        /// <seealso cref="wclWeDoHub"/>
        public wclWeDoHub Hub { get { return FHub; } }
        /// <summary> The index of the port on the Hub the IO is attached to.  </summary>
		public Byte PortId { get { return FPortId; } }
        /// <summary> Gets the IO type represented by this object. </summary>
        /// <value> The type of the device. </value>
        /// <seealso cref="wclWeDoIoType"/>
        public wclWeDoIoType Type { get { return FType; } }
    };
}
