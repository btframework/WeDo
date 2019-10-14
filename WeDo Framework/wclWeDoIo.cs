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
using System.Linq;
using System.Collections.Generic;

using wclCommon;

namespace wclWeDoFramework
{
    /// <summary> Represents a type of an attached IO (motor, sensor, etc). </summary>
	public enum wclWeDoIoDeviceType
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

    /// <summary> The structure describes the device version number. </summary>
    public struct wclWeDoVersion
    {
        internal static wclWeDoVersion FromByteArray(Byte[] Data)
        {
            wclWeDoVersion Version = new wclWeDoVersion();
            Version.MajorVersion = Data[0];
            Version.MinorVersion = Data[1];
            Version.BugFixVersion = Data[2];
            Version.BuildNumber = Data[3];
            return Version;
        }

        /// <summary> The bug fix version number. </summary>
		public Byte BugFixVersion;
        /// <summary> The build number. </summary>
		public Byte BuildNumber;
        /// <summary> The build number. </summary>
		public Byte MajorVersion;
        /// <summary> The major version number. </summary>
		public int MinorVersion;

        /// <summary> A formatted string representation of the version. </summary>
		/// <returns> A formatted string representation of the version </returns>
		public override String ToString()
        {
            return String.Format("{0}.{1}.{2}.{3}", MajorVersion, MinorVersion, BugFixVersion, BuildNumber);
        }

        /// <summary> Compares versions. </summary>
		/// <param name="obj"> The object to compare to. </param>
		/// <returns> <c>True</c> if objects are equal. </returns>
		public override Boolean Equals(Object obj)
        {
            if (object.ReferenceEquals(obj, null))
                return false;
            if (GetType() != obj.GetType())
                return false;

            wclWeDoVersion Version = (wclWeDoVersion)obj;
            return (BugFixVersion == Version.BugFixVersion &&
                BuildNumber == Version.BuildNumber && MajorVersion == Version.MajorVersion &&
                MinorVersion == Version.MinorVersion);
        }

        /// <summary> Gets the object hash. </summary>
        /// <returns> The objects hash. </returns>
        public override Int32 GetHashCode()
        {
            return ((MajorVersion << 24) | (MinorVersion << 16) | (BugFixVersion << 8) | BuildNumber).GetHashCode();
        }

        /// <summary> Override the <c>==</c> operator. </summary>
        /// <param name="a"> First argument. </param>
        /// <param name="b"> Second argument. </param>
        /// <returns> <c>True</c> if a == b. <c>False</c> otherwise. </returns>
        public static Boolean operator ==(wclWeDoVersion a, wclWeDoVersion b)
        {
            return a.Equals(b);
        }

        /// <summary> Override the <c>!==</c> operator. </summary>
        /// <param name="a"> First argument. </param>
        /// <param name="b"> Second argument. </param>
        /// <returns> <c>True</c> if a != b. <c>False</c> otherwise. </returns>
        public static Boolean operator !=(wclWeDoVersion a, wclWeDoVersion b)
        {
            return !a.Equals(b);
        }
    };

    /// <summary> The class represets an attached Input/Outpout device. </summary>
    public abstract class wclWeDoIo
    {
        private const Byte WEDO_DEVICE_MOTOR = 1;
        private const Byte WEDO_DEVICE_VOLTAGE_SENSOR = 20;
        private const Byte WEDO_DEVICE_CURRENT_SENSOR = 21;
        private const Byte WEDO_DEVICE_PIEZO = 22;
        private const Byte WEDO_DEVICE_RGB = 23;
        private const Byte WEDO_DEVICE_TILT_SENSOR = 34;
        private const Byte WEDO_DEVICE_MOTION_SENSOR = 35;

        private Boolean FAttached;
        private Byte FConnectionId;
        private List<wclWeDoDataFormat> FDataFormats;
        private wclWeDoInputFormat FDefaultInputFormat;
        private wclWeDoIoDeviceType FDeviceType;
        private wclWeDoVersion FFirmwareVersion;
        private wclWeDoVersion FHardwareVersion;
        private wclWeDoHub FHub;
        private wclWeDoInputFormat FInputFormat;
        private Boolean FInternal;
        private List<Byte[]> FNumbersFromValueData;
        private Byte FPortId;
        private List<wclWeDoDataFormat> FValidDataFormats;
        private Byte[] FValue;

        private void SetValue(Byte[] Value)
        {
            FValue = Value;
            ValueChanged();
        }

        private Int32 SetDefaultInputFormat(wclWeDoInputFormat Format)
        {
            FDefaultInputFormat = Format;
            return SendInputFormat(Format);
        }

        private float GetAsFloat()
        {
            if (FValue == null || FValue.Length == 0)
                return 0f;
            if (FValue.Length == 4)
                return BitConverter.ToSingle(FValue, 0);
            return 0f;
        }

        private Int32 GetAsInteger()
        {
            if (FValue == null || FValue.Length == 0)
                return 0;
            if (FValue.Length == 1)
            {
                Int32 Signed = (Int32)FValue[0];
                if (Signed > 127)
                    Signed = 0 - (256 - Signed);
                return Signed;
            }
            if (FValue.Length == 2)
                return BitConverter.ToInt16(FValue, 0);
            if (FValue.Length == 4)
                return BitConverter.ToInt32(FValue, 0);
            return 0;
        }

        private Byte GetInputFormatMode()
        {
            if (FInputFormat != null)
                return FInputFormat.Mode;
            if (FDefaultInputFormat != null)
                return FDefaultInputFormat.Mode;
            return 0;
        }

        private wclWeDoDataFormat DataFormatForInputFormat(wclWeDoInputFormat InputFormat)
        {
            foreach (wclWeDoDataFormat DataFormat in FValidDataFormats)
            {
                if (DataFormat.Mode == InputFormat.Mode && DataFormat.Unit == InputFormat.Unit)
                {
                    if ((DataFormat.DataSetCount * DataFormat.DataSetSize) != InputFormat.NumberOfBytes)
                        return null;
                    return DataFormat;
                }
            }
            return null;
        }

        private Boolean VerifyValue(Byte[] Value)
        {
            if (Value == null || Value.Length == 0)
                return true;
            if (FValidDataFormats.Count == 0)
                return true;

            // If one or more InputDataFormats are defined, we look at the latest received InputFormat from the device
            // for a received value to be accepted, there:
            //   1. Must exists an DataFormat that matches the latest received InputFormat from device.
            //   2. The received valueData length must match this DataFormat.
            wclWeDoDataFormat DataFormat = DataFormatForInputFormat(FInputFormat);
            if (DataFormat == null)
                return false;

            Boolean ValueCorrect = (Value.Length == (DataFormat.DataSetSize * DataFormat.DataSetCount));
			if (!ValueCorrect)
            {
                FNumbersFromValueData.Clear();
				return false;
			}

            // If the Data Format has a value fill the NumbersFromValueData array with all received numbers
            if (DataFormat.DataSetCount > 0)
            {
                List<Byte> DataList = Value.ToList();
                FNumbersFromValueData.Clear();
                for (Int32 i = 0; i < DataList.Count; i += DataFormat.DataSetSize)
                    FNumbersFromValueData.Add(DataList.GetRange(i, DataFormat.DataSetSize).ToArray());
            }

			return ValueCorrect;
        }

        // The method called by HUB when new device found (attached).
        internal static wclWeDoIo Attach(wclWeDoHub Hub, Byte[] RawInfo)
        {
            if (RawInfo == null || RawInfo.Length < 2)
                return null;
            if ((RawInfo[1] == 1 && RawInfo.Length != 12) || (RawInfo[1] == 0 && RawInfo.Length != 2))
                return null;
            if (RawInfo[1] == 0)
                // Detached???
                return null;

            wclWeDoIo Io;
            Byte ConnectionId = RawInfo[0];
            switch (RawInfo[3])
            {
                case WEDO_DEVICE_MOTOR:
                    Io = new wclWeDoMotor(Hub, ConnectionId);
                    Io.FDeviceType = wclWeDoIoDeviceType.iodMotor;
                    break;
                case WEDO_DEVICE_VOLTAGE_SENSOR:
                    Io = new wclWeDoVoltageSensor(Hub, ConnectionId);
                    Io.FDeviceType = wclWeDoIoDeviceType.iodVoltageSensor;
                    break;
                case WEDO_DEVICE_CURRENT_SENSOR:
                    Io = new wclWeDoCurrentSensor(Hub, ConnectionId);
                    Io.FDeviceType = wclWeDoIoDeviceType.iodCurrentSensor;
                    break;
                case WEDO_DEVICE_PIEZO:
                    Io = new wclWeDoPieazo(Hub, ConnectionId);
                    Io.FDeviceType = wclWeDoIoDeviceType.iodPiezo;
                    break;
                case WEDO_DEVICE_RGB:
                    Io = new wclWeDoRgbLight(Hub, ConnectionId);
                    Io.FDeviceType = wclWeDoIoDeviceType.iodRgb;
                    break;
                case WEDO_DEVICE_TILT_SENSOR:
                    Io = new wclWeDoTiltSensor(Hub, ConnectionId);
                    Io.FDeviceType = wclWeDoIoDeviceType.iodTiltSensor;
                    break;
                case WEDO_DEVICE_MOTION_SENSOR:
                    Io = new wclWeDoMotionSensor(Hub, ConnectionId);
                    Io.FDeviceType = wclWeDoIoDeviceType.iodMotionSensor;
                    break;
                default:
                    Io = null;
                    break;
            }

            if (Io != null)
            {

                Byte[] Tmp = new Byte[4];
                Array.Copy(RawInfo, 8, Tmp, 0, 4);
                Io.FFirmwareVersion = wclWeDoVersion.FromByteArray(Tmp);
                Array.Copy(RawInfo, 4, Tmp, 0, 4);
                Io.FHardwareVersion = wclWeDoVersion.FromByteArray(Tmp);
                Io.FPortId = RawInfo[2];
                Io.FInternal = (Io.PortId > 50);
            }

            return Io;
        }

        // The method called by HUB when device has been detached.
        internal void Detach()
        {
            FAttached = false;
            FNumbersFromValueData.Clear();
        }

        // The method called by IO Service when Input Format has been updated.
        internal void UpdateInputFormat(wclWeDoInputFormat Format)
        {
            if (FInputFormat == null || (Format != FInputFormat && FConnectionId == Format.ConnectionId))
            {
                wclWeDoInputFormat OldFormat = FInputFormat;
                FInputFormat = Format;
                InputFormatChanged(OldFormat);
                SendReadValueRequest();
            }
        }

        // The method called by the IO Service when new value received.
        internal void UpdateValue(Byte[] Value)
        {
            if (FValue != null && FValue.Equals(Value))
                return;

            if (VerifyValue(Value))
            {
                FValue = Value;
                ValueChanged();
            }
        }

        /// <summary> Sends data to the IO service. </summary>
		/// <param name="Data"> The data to write. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
		protected Int32 WriteData(Byte[] Data)
        {
            return FHub.Io.WriteData(Data, FConnectionId);
        }

        /// <summary> If the notifications is disabled for the service in the Input Format you will have to use
        ///   this method to request an updated value for the service. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
		protected Int32 SendReadValueRequest()
        {
            return FHub.Io.ReadValue(FConnectionId);
        }

        /// <summary> Sends a reset command to the Device. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
		protected Int32 ResetSensor()
        {
            return FHub.Io.ResetIo(FConnectionId);
        }

        /// <summary> Adds a new valid data format. </summary>
		/// <param name="Format"> The data format to add. </param>
        /// <seealso cref="wclWeDoDataFormat"/>
		protected void AddValidDataFormat(wclWeDoDataFormat Format)
        {
            FValidDataFormats.Add(Format);
        }

        /// <summary> Removes a valid data format. </summary>
		/// <param name="Format"> The data format to remove. </param>
        /// <seealso cref="wclWeDoDataFormat"/>
		protected void RemoveValidDataFormat(wclWeDoDataFormat Format)
        {
            FValidDataFormats.Remove(Format);
        }

        /// <summary> Send an updated input format for this service to the device. </summary>
		/// <param name="Format"> New input format. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seelso cref="wclWeDoInputFormat"/>
		protected Int32 SendInputFormat(wclWeDoInputFormat Format)
        {
            return FHub.Io.WriteInputFormat(Format, FConnectionId);
        }

        /// <summary> Changes mode of the Input Format. </summary>
        /// <param name="Mode"> The Input Format mode. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        protected Int32 SetInputFormatMode(Byte Mode)
        {
            if (FInputFormat != null)
                return SendInputFormat(FInputFormat.InputFormatBySettingMode(Mode));
            if (FDefaultInputFormat != null)
                return SendInputFormat(FDefaultInputFormat.InputFormatBySettingMode(Mode));
            return wclErrors.WCL_E_INVALID_ARGUMENT;
        }

        /// <summary> The method called when Input Format has been changed. </summary>
        /// <param name="OldFormat"> The old Input Format. </param>
        /// <remarks> A derived class must override this method to get notifications about
        ///   format changes. </remarks>
        protected virtual void InputFormatChanged(wclWeDoInputFormat OldFormat)
        {
            // Do nothing
        }

        /// <summary> The method called when data value has been changed. </summary>
        /// <remarks> A derived class must override this method to get notifications about
        ///   value changes. </remarks>
        protected virtual void ValueChanged()
        {
            // Do nothing.
        }

        /// <summary> Gets current sensor's value as <c>Float</c> number. </summary>
        /// <value> The float sensors value. </value>
        protected float AsFloat { get { return GetAsFloat(); } }
        /// <summary> Gets the current sensor's value as <c>Integer</c> number. </summary>
        /// <value> The integer sensor's value. </value>
        protected Int32 AsInteger { get { return GetAsInteger(); } }
        /// <summary> Gets the list of supported Data Formats. </summary>
        /// <value> The list of supported Data Formats. </value>
        /// <seealso cref="wclWeDoDataFormat"/>
        protected List<wclWeDoDataFormat> DataFormats { get { return FDataFormats; } }
        /// <summary> Gets and sets the default input format. </summary>
        /// <value> The default input format. </value>
        /// <seealso cref="wclWeDoInputFormat"/>
        protected wclWeDoInputFormat DefaultInputFormat { get { return FDefaultInputFormat; } set { SetDefaultInputFormat(value); } }
        /// <summary> Gets the sensor Input Format. </summary>
        /// <value> The Input Format. </value>
        /// <seealso cref="wclWeDoInputFormat"/>
        protected wclWeDoInputFormat InputFormat { get { return FInputFormat; } }
        /// <summary> Gets the Input Format mode. </summary>
        /// <value> The Input Format Mode. </value>
        protected Byte InputFormatMode { get { return GetInputFormatMode(); } }
        /// <summary> Gets alist with one byte[] per number received. </summary>
        /// <value> The list of bytes array. </value>
		protected List<Byte[]> NumbersFromValueData { get { return FNumbersFromValueData; } }
        /// <summary> Gets the current sensors value. </summary>
        /// <value> The sensors value as raw bytes array. </value>
        protected Byte[] Value { get { return FValue; } private set { SetValue(value); } }

        /// <summary> Creates new IO device object. </summary>
        /// <param name="Hub"> The Hub object that owns the device. If this parameter is <c>null</c>
        ///   the <seealso cref="wclEInvalidArgument"/> exception raises. </param>
        /// <param name="ConnectionId"> The device's Connection ID. </param>
        /// <seealso cref="wclWeDoHub"/>
        /// <exception cref="wclEInvalidArgument"> The exception raises when the <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoIo(wclWeDoHub Hub, Byte ConnectionId)
        {
            if (Hub == null)
                throw new wclEInvalidArgument("Hub parameter can not be null.");

            FAttached = true; // It is always attached on creation!
            FConnectionId = ConnectionId;
            FDataFormats = new List<wclWeDoDataFormat>();
            FDefaultInputFormat = null;
            FDeviceType = wclWeDoIoDeviceType.iodUnknown;
            FFirmwareVersion = new wclWeDoVersion();
            FHardwareVersion = new wclWeDoVersion();
            FHub = Hub;
            FInputFormat = null;
            FInternal = true;
            FPortId = 0;
            FNumbersFromValueData = new List<Byte[]>();
            FValidDataFormats = new List<wclWeDoDataFormat>();
            FValue = null;
        }

        /// <summary> Gets the IO device state. </summary>
        /// <value> <c>True</c> if the device is attached. <c>False</c> if the device is detached. </value>
        public Boolean Attached { get { return FAttached; } }
        /// <summary> Gets the IO connection ID. </summary>
        /// <value> The IO connection ID. </value>
        /// <remarks> It is guarateed that the connection ID is unique. </remarks>
        public Byte ConnectionId { get { return FConnectionId; } }
        /// <summary> Gets the device represented by this object. </summary>
        /// <value> The IO device type. </value>
        /// <seealso cref="wclWeDoIoDeviceType"/>
        public wclWeDoIoDeviceType DeviceType { get { return FDeviceType; } }
        /// <summary> Gets the IO device firmware version. </summary>
        /// <value> The IO device firmware version. </value>
        /// <seealso cref="wclWeDoVersion"/>
		public wclWeDoVersion FirmwareVersion { get { return FFirmwareVersion; } }
        /// <summary> Gets the IO device hardware version. </summary>
        /// <value> The IO device hardware version. </value>
        /// <seealso cref="wclWeDoVersion"/>
        public wclWeDoVersion HardwareVersion { get { return FHardwareVersion; } }
        /// <summary> Gets the IO type represented by this object. </summary>
        /// <value> <c>True</c> if the IO device is internal. <c>False</c> if the IO device is external. </value>
        public Boolean Internal { get { return FInternal; } }
        /// <summary> Gets the WeDo Hub object that owns the IO device. </summary>
        /// <value> The WeDo Hub object. </value>
        /// <seealso cref="wclWeDoHub"/>
        public wclWeDoHub Hub { get { return FHub; } }
        /// <summary> The index of the port on the Hub the IO is attached to.  </summary>
        /// <value> The port ID. </value>
		public Byte PortId { get { return FPortId; } }
    };
}
