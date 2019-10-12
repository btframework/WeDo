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

    /// <summary> The class represets an attached Input/Outpout device. </summary>
    public abstract class wclWeDoIo
    {
        private Boolean FAttached;
        private Byte FConnectionId;
        private List<wclWeDoDataFormat> FDataFormats;
        private wclWeDoInputFormat FDefaultInputFormat;
        private wclWeDoIoDeviceType FDeviceType;
        private String FFirmwareVersion;
        private String FHardwareVersion;
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

            wclWeDoIo Io = null;
            Byte ConnectionId = RawInfo[0];
            switch (RawInfo[3])
            {
                /*case 1:
                    connectInfo.TypeEnum = IoType.IoTypeMotor;*/
                /*case 20:
                    connectInfo.TypeEnum = IoType.IoTypeVoltage;*/
                /*case 21:
                connectInfo.TypeEnum = IoType.IoTypeCurrent;*/
                case 22:
                    Io = new wclWeDoPieazo(Hub, ConnectionId);
                    Io.FDeviceType = wclWeDoIoDeviceType.iodPiezo;
                    break;
                case 23:
                    Io = new wclWeDoRgbLight(Hub, ConnectionId);
                    Io.FDeviceType = wclWeDoIoDeviceType.iodRgb;
                    break;
                    /*case 34:
                    connectInfo.TypeEnum = IoType.IoTypeTiltSensor;*/
                    /*case 35:
                    connectInfo.TypeEnum = IoType.IoTypeMotionSensor;*/
                    /*default:
                    connectInfo.TypeEnum = IoType.IoTypeGeneric;*/
            }

            if (Io != null)
            {
                Io.FFirmwareVersion = RawInfo[8].ToString() + "." + RawInfo[9].ToString() + "." +
                    RawInfo[10].ToString() + "." + RawInfo[11].ToString();
                Io.FHardwareVersion = RawInfo[4].ToString() + "." + RawInfo[5].ToString() + "." + 
                    RawInfo[6].ToString() + "." + RawInfo[7].ToString();
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
		protected Int32 Reset()
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
        protected abstract void InputFormatChanged(wclWeDoInputFormat OldFormat);

        /// <summary> The method called when data value has been changed. </summary>
        /// <remarks> A derived class must override this method to get notifications about
        ///   value changes. </remarks>
        protected abstract void ValueChanged();

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
        public Byte InputFormatMode { get { return GetInputFormatMode(); } }
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
            FFirmwareVersion = "";
            FHardwareVersion = "";
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
        /// <sumnmary> Gets the device represented by this object. </sumnmary>
        /// <value> The IO device type. </value>
        /// <seealso cref="wclWeDoIoDeviceType"/>
        public wclWeDoIoDeviceType DeviceType { get { return FDeviceType; } }
        /// <summary> Gets the IO device firmware version. </summary>
        /// <value> The IO device firmware version. </value>
		public String FirmwareVersion { get { return FFirmwareVersion; } }
        /// <summary> Gets the IO device hardware version. </summary>
        /// <value> The IO device hardware version. </value>
        public String HardwareVersion { get { return FHardwareVersion; } }
        /// <summary> Gets the IO type represented by this object. </summary>
        /// <value> <c>True</c> if the IO device is internal. <c>False</c> if the IO device is external. </value>
        public Boolean Internal { get { return FInternal; } }
        /// <summary> Gets the WeDo Hub object that owns the IO device. </summary>
        /// <value> The WeDo Hub object. </value>
        /// <seealso cref="wclWeDoHub"/>
        public wclWeDoHub Hub { get { return FHub; } }
        /// <summary> The index of the port on the Hub the IO is attached to.  </summary>
		public Byte PortId { get { return FPortId; } }
    };
}
