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
using System.Windows.Media;

using wclCommon;
using wclBluetooth;

namespace wclWeDoFramework
{
    /// <summary> Tones that can be played using the <see cref="wclWeDoPieazo"/> </summary>
	public enum wclWeDoPiezoNote
    {
        /// <summary> C </summary>
        pnC = 1,
        /// <summary> C# </summary>
        pnCis = 2,
        /// <summary> D </summary>
        pnD = 3,
        /// <summary> D# </summary>
        pnDis = 4,
        /// <summary> E </summary>
        pnE = 5,
        /// <summary> F </summary>
        pnF = 6,
        /// <summary> F# </summary>
        pnFis = 7,
        /// <summary> G </summary>
        pnG = 8,
        /// <summary> G# </summary>
        pnGis = 9,
        /// <summary> A </summary>
        pnA = 10,
        /// <summary> A# </summary>
        pnAis = 11,
        /// <summary> B </summary>
        pnB = 12
    };

    /// <summary> The class represents a Piezo tone player device. </summary>
    /// <seealso cref="wclWeDoIo"/>
    public class wclWeDoPieazo : wclWeDoIo
    {
        private const UInt16 PIEZO_MAX_FREQUENCY = 1500;
        private const UInt16 PIEZO_MAX_DURATION = 65535;

        /// <summary> Creates new Piezo device object. </summary>
        /// <param name="Hub"> The Hub object that owns the device. If this parameter is <c>null</c>
        ///   the <seealso cref="wclEInvalidArgument"/> exception raises. </param>
        /// <param name="ConnectionId"> The device's Connection ID. </param>
        /// <seealso cref="wclWeDoHub"/>
        /// <exception cref="wclEInvalidArgument"> The exception raises when the <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoPieazo(wclWeDoHub Hub, Byte ConnectionId)
            : base(Hub, ConnectionId)
        {
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
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            return Hub.Io.PiezoPlayTone(Frequency, Duration, ConnectionId);
        }

        /// <summary> Plays a note. The highest supported node is F# in 6th octave. </summary>
        /// <param name="Note"> The note to play. </param>
        /// <param name="Octave"> The octave in which to play the node. </param>
        /// <param name="Duration"> The duration to play (max supported is 65535 milli seconds). </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seealso cref="wclWeDoPiezoNote"/>
        public Int32 PlayNote(wclWeDoPiezoNote Note, Byte Octave, UInt16 Duration)
        {
            if (Octave == 0 || Octave > 6 || (Octave == 6 && Note > wclWeDoPiezoNote.pnFis))
                return wclErrors.WCL_E_INVALID_ARGUMENT;
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            // The basic formula for the frequencies of the notes of the equal tempered scale is given by
            // fn = f0 * (a)n
            // where
            //   f0 - the frequency of one fixed note which must be defined.
            //        A common choice is setting the A above middle C (A4) at f0 = 440 Hz.
            //   n  - the number of half steps away from the fixed note you are. If you are at a higher note,
            //        n is positive. If you are on a lower note, n is negative.
            //   fn - the frequency of the note n half steps away.
            //   a  - (2)1/12 = the twelfth root of 2 = the number which when multiplied by itself 12 times
            //        equals 2 = 1.059463094359...
            Double BaseTone = 440.0;
            Int32 OctavesAboveMiddle = Octave - 4;
            float HalfStepsAwayFromBase = (float)Note - (float)wclWeDoPiezoNote.pnA + (OctavesAboveMiddle * 12);
            Double Frequency = BaseTone * Math.Pow(Math.Pow(2.0, 1.0 / 12), HalfStepsAwayFromBase);

            return Hub.Io.PiezoPlayTone((UInt16)Math.Round(Frequency), Duration, ConnectionId);
        }

        /// <summary> Stop playing any currently playing tone. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 StopPlaying()
        {
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            return Hub.Io.PiezoStopPlaying(ConnectionId);
        }
    };

    /// <summary> Mode of the RGB light device. </summary>
    public enum wclWeDoRgbLightMode
    {
        /// <summary> Discrete mode allows selecting a color index from a set of predefined colors. </summary>
        lmDiscrete = 0,
        /// <summary> Absolute mode allows selecting any color by specifying its RGB component values. </summary>
        lmAbsolute = 1,
        /// <summary> Unknown (unsupported) mode. </summary>
        lmUnknown = 255
    };

    /// <summary> A Lego WeDo color indexes. </summary>
    /// <remarks> This enumration is used in <c>Absolute</c> color mode. </remarks>
    /// <seealso cref="wclWeDoRgbLightMode"/>
    public enum wclWeDoColor
    {
        /// <summary> Black (none) color. </summary>
        clBlack = 0,
        /// <summary> Pink color. </summary>
        clPink = 1,
        /// <summary> Purple color. </summary>
        clPurple = 2,
        /// <summary> Blue color. </summary>
        clBlue = 3,
        /// <summary> Sky blue color. </summary>
        clSkyBlue = 4,
        /// <summary> Teal color. </summary>
        clTeal = 5,
        /// <summary> Green color. </summary>
        clGreen = 6,
        /// <summary> Yellow color. </summary>
        clYellow = 7,
        /// <summary> Orange color. </summary>
        clOrange = 8,
        /// <summary> Red color. </summary>
        clRed = 9,
        /// <summary> White color. </summary>
        clWhite = 10,
        /// <summary> Unknwon color index. </summary>
        clUnknown = 255
    };

    /// <summary> The class represents a HUB RGB light. </summary>
    /// <seealso cref="wclWeDoIo"/>
    public class wclWeDoRgbLight : wclWeDoIo
    {
        private Color FColor;
        private wclWeDoColor FColorIndex;

        private Boolean GetColorFromByteArray(Byte[] Data, out Color Color)
        {
            Color = Colors.Black;
            if (Data == null || Data.Length != 3)
                return false;

            Color = Color.FromRgb(Data[0], Data[1], Data[2]);
            return true;
        }

        /// <summary> The method called when Input Format has been changed. </summary>
        /// <param name="OldFormat"> The old Input Format. </param>
        protected override void InputFormatChanged(wclWeDoInputFormat OldFormat)
        {
            if (InputFormat != null)
            {

                if (OldFormat == null)
                    DoModeChanged();
                else
                {
                    if (InputFormat.Mode != OldFormat.Mode)
                        DoModeChanged();
                }
            }
        }

        /// <summary> The method called when data value has been changed. </summary>
        protected override void ValueChanged()
        {
            if (Mode == wclWeDoRgbLightMode.lmAbsolute)
            {
                Color NewColor;
                if (!GetColorFromByteArray(Value, out NewColor))
                    return;
                if (NewColor.Equals(FColor))
                    return;
                FColor = NewColor;
                DoColorChanged();
            }
            else
            {
                if (Mode == wclWeDoRgbLightMode.lmDiscrete)
                {
                    wclWeDoColor NewColorIndex = (wclWeDoColor)AsInteger;
                    if (NewColorIndex != FColorIndex)
                    {
                        FColorIndex = NewColorIndex;
                        DoColorChanged();
                    }
                }
            }
        }

        /// <summary> Fires the <c>OnColorChanged</c> event. </summary>
        protected virtual void DoColorChanged()
        {
            if (OnColorChanged != null)
                OnColorChanged(this, EventArgs.Empty);
        }

        /// <summary> Fires the <c>OnModeChanged</c> event. </summary>
        protected virtual void DoModeChanged()
        {
            if (OnModeChanged != null)
                OnModeChanged(this, EventArgs.Empty);
        }

        /// <summary> Creates new RGB light device object. </summary>
        /// <param name="Hub"> The Hub object that owns the device. If this parameter is <c>null</c>
        ///   the <seealso cref="wclEInvalidArgument"/> exception raises. </param>
        /// <param name="ConnectionId"> The device's Connection ID. </param>
        /// <seealso cref="wclWeDoHub"/>
        /// <exception cref="wclEInvalidArgument"> The exception raises when the <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoRgbLight(wclWeDoHub Hub, Byte ConnectionId)
            : base(Hub, ConnectionId)
        {
            AddValidDataFormat(new wclWeDoDataFormat(1, 1, (Byte)wclWeDoRgbLightMode.lmDiscrete, wclWeDoSensorDataUnit.suRaw));
            AddValidDataFormat(new wclWeDoDataFormat(1, 1, (Byte)wclWeDoRgbLightMode.lmDiscrete, wclWeDoSensorDataUnit.suPercentage));
            AddValidDataFormat(new wclWeDoDataFormat(1, 4, (Byte)wclWeDoRgbLightMode.lmDiscrete, wclWeDoSensorDataUnit.suSi));

            AddValidDataFormat(new wclWeDoDataFormat(3, 1, (Byte)wclWeDoRgbLightMode.lmAbsolute, wclWeDoSensorDataUnit.suRaw));
            AddValidDataFormat(new wclWeDoDataFormat(3, 1, (Byte)wclWeDoRgbLightMode.lmAbsolute, wclWeDoSensorDataUnit.suPercentage));
            AddValidDataFormat(new wclWeDoDataFormat(3, 4, (Byte)wclWeDoRgbLightMode.lmAbsolute, wclWeDoSensorDataUnit.suSi));

            DefaultInputFormat = new wclWeDoInputFormat(ConnectionId, wclWeDoIoDeviceType.iodRgb, (Byte)wclWeDoRgbLightMode.lmDiscrete,
                1, wclWeDoSensorDataUnit.suRaw, true, 0, 1);

            FColor = DefaultColor;
            FColorIndex = DefaultColorIndex;

            OnColorChanged = null;
            OnModeChanged = null;
        }

        /// <summary> Switch off the RGB light on the device. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
		public Int32 SwitchOff()
        {
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            if (Mode == wclWeDoRgbLightMode.lmAbsolute)
                return SetColor(Colors.Black);
            if (Mode == wclWeDoRgbLightMode.lmDiscrete)
                return SetColorIndex(wclWeDoColor.clBlack);
            return wclErrors.WCL_E_INVALID_ARGUMENT;
        }

        /// <summary> Switches to the default Color (i.e. the same color as the device has right after a successful connection has
        ///   been established). </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 SwitchToDefaultColor()
        {
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            if (Mode == wclWeDoRgbLightMode.lmAbsolute)
                return SetColor(DefaultColor);
            if (Mode == wclWeDoRgbLightMode.lmDiscrete)
                return SetColorIndex(DefaultColorIndex);
            return wclErrors.WCL_E_INVALID_ARGUMENT;
        }

        /// <summary> Sets the RGB color. </summary>
        /// <param name="Rgb"> The RGB color. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seealso cref="System.Windows.Media.Color"/>
        public Int32 SetColor(Color Rgb)
        {
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            if (Mode != wclWeDoRgbLightMode.lmAbsolute)
                return wclErrors.WCL_E_INVALID_ARGUMENT;

            FColor = Rgb;
            return Hub.Io.WriteColor(Rgb.R, Rgb.G, Rgb.B, ConnectionId);
        }

        /// <summary> Sets the index of the currently selected color (discrete mode). </summary>
        /// <param name="Index"> The color index. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 SetColorIndex(wclWeDoColor Index)
        {
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            if (Mode != wclWeDoRgbLightMode.lmDiscrete)
                return wclErrors.WCL_E_INVALID_ARGUMENT;

            FColorIndex = Index;
            return Hub.Io.WriteColorIndex((Byte)Index, ConnectionId);
        }

        /// <summary> Sets the mode of the RGB light. </summary>
        /// <param name="Mode"> The RGB lite mode. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seealso cref="wclWeDoRgbLightMode"/>
        public Int32 SetMode(wclWeDoRgbLightMode Mode)
        {
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            return SetInputFormatMode((Byte)Mode);
        }

        /// <summary> Gets the color of the RGB light on the device (absolute mode). </summary>
        /// <value> The RGB color. </value>
        /// <seealso cref="System.Windows.Media.Color"/>
        public Color Color { get { return FColor; } }
        /// <summary> Gets the index of the currently selected color (discrete mode). </summary>
        /// <value> The color index. </value>
        /// <seealso cref="wclWeDoColor"/>
        public wclWeDoColor ColorIndex { get { return FColorIndex; } }
        /// <summary> Gets the default color of the RGB light (absolute mode). </summary>
        /// <value> The default color. </value>
        /// <seealso cref="System.Windows.Media.Color"/>
        public Color DefaultColor { get { return Color.FromArgb(0xFF, 0x00, 0x00, 0xFF); } }
        /// <summary> Gets the default color index of the RGB, when in the discrete mode. </summary>
        /// <value> The default color index. </value>
        /// <seealso cref="wclWeDoColor"/>
        public wclWeDoColor DefaultColorIndex { get { return wclWeDoColor.clBlue; } }
        /// <summary> Gets the mode of the RGB light. </summary>
        /// <value> The RGB light device mode. </value>
        /// <seealso cref="wclWeDoRgbLightMode"/>
        public wclWeDoRgbLightMode Mode { get { return (wclWeDoRgbLightMode)InputFormatMode; } }
        /// <summary> The event fires when color has been changed. </summary>
        /// <seealso cref="EventHandler"/>
        public event EventHandler OnColorChanged;

        /// <summary> The event fired when the RGB LED mode has been changed. </summary>
        /// <seealso cref="EventHandler"/>
        public event EventHandler OnModeChanged;
    };

    /// <summary> The motor's direction. </summary>
    public enum wclWeDoMotorDirection
    {
        /// <summary> Drifting (Floating). </summary>
        mdDrifting = 0,
        /// <summary> Running left. </summary>
        mdLeft = 1,
        /// <summary> Running right. </summary>
        mdRight = 2,
        /// <summary> Brake. </summary>
        mdBraking = 3,
        /// <summary> Unknwon. </summary>
        mdUnknown = 255
    };

    /// <summary> The class represents a WeDo motor. </summary>
    /// <seealso cref="wclWeDoIo"/>
    public class wclWeDoMotor : wclWeDoIo
    {
        private const SByte MOTOR_POWER_DRIFT = 0;
        private const SByte MOTOR_POWER_BRAKE = 127;

        private const Byte MOTOR_MIN_SPEED = 1;
        private const Byte MOTOR_MAX_SPEED = 100;

        // Only send values in the range 35-100.
        // An offset is needed as values below 35 is not enough power to actually
        // make the motor turn.
        private const Byte MOTOR_POWER_OFFSET = 35;

        private wclWeDoMotorDirection FDirection;
        private SByte FPower;

        private Byte GetPower()
        {
            if (FDirection == wclWeDoMotorDirection.mdBraking || FDirection == wclWeDoMotorDirection.mdDrifting)
                return 0;
            return (Byte)Math.Abs(FPower);
        }

        private Int32 SendPower(SByte Power)
        {
            Int32 Res;
            if (Power == MOTOR_POWER_BRAKE || Power == MOTOR_POWER_DRIFT)
                Res = Hub.Io.WriteMotorPower(Power, ConnectionId);
            else
            {
                Byte Offset = MOTOR_POWER_OFFSET;
                if (FirmwareVersion.MajorVersion == 0)
                    // On version 0.x of the firmware, PVM offset is handled in the firmware
                    Offset = 0;
                Res = Hub.Io.WriteMotorPower(Power, Offset, ConnectionId);
            }
            if (Res == wclErrors.WCL_E_SUCCESS)
                FPower = Power;

            return Res;
        }

        private SByte ConvertUnsignedMotorPowerToSigned(wclWeDoMotorDirection Direction, Byte Power)
        {
            SByte Res = (SByte)Math.Min(Math.Max(Power, MOTOR_MIN_SPEED), MOTOR_MAX_SPEED);
            if (Direction == wclWeDoMotorDirection.mdLeft)
                Res = (SByte)(-Res);
            return Res;
        }

        /// <summary> Creates new motor class object. </summary>
        /// <param name="Hub"> The Hub object that owns the device. If this parameter is <c>null</c>
        ///   the <seealso cref="wclEInvalidArgument"/> exception raises. </param>
        /// <param name="ConnectionId"> The device's Connection ID. </param>
        /// <seealso cref="wclWeDoHub"/>
        /// <exception cref="wclEInvalidArgument"> The exception raises when the <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoMotor(wclWeDoHub Hub, Byte ConnectionId)
            : base(Hub, ConnectionId)
        {
            FDirection = wclWeDoMotorDirection.mdBraking;
            FPower = 0;
        }

        /// <summary> Sends a command to stop (brake) the motor. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 Brake()
        {
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            Int32 Res = SendPower(MOTOR_POWER_BRAKE);
            if (Res == wclErrors.WCL_E_SUCCESS)
                FDirection = wclWeDoMotorDirection.mdBraking;
            return Res;
        }

        /// <summary> Sends a command to stop (drift/float) the motor. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 Drift()
        {
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            Int32 Res = SendPower(MOTOR_POWER_DRIFT);
            if (Res == wclErrors.WCL_E_SUCCESS)
                FDirection = wclWeDoMotorDirection.mdDrifting;
            return Res;
        }

        /// <summary> Sends a command to run the motor at a given <c>Power</c> in a given <c>Direction</c>.
        ///   The minimum speed is 0 and the maximum speed is 100. </summary>
        /// <param name="Direction"> The direction to run the motor. </param>
        /// <param name="Power"> The power to run the motor with. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seealso cref="wclWeDoMotorDirection"/>
        public Int32 Run(wclWeDoMotorDirection Direction, Byte Power)
        {
            if (Direction == wclWeDoMotorDirection.mdUnknown || Power > 100)
                return wclErrors.WCL_E_INVALID_ARGUMENT;

            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            if (Direction == wclWeDoMotorDirection.mdDrifting || Power == 0)
                return Drift();
            if (Direction == wclWeDoMotorDirection.mdBraking)
                return Brake();

            Int32 Res = SendPower(ConvertUnsignedMotorPowerToSigned(Direction, Power));
            if (Res == wclErrors.WCL_E_SUCCESS)
                FDirection = Direction;
            return Res;
        }

        /// <summary> Gets the current running direction of the motor. </summary>
        /// <value> Teh motor direction. </value>
        /// <seealso cref="wclWeDoMotorDirection"/>
        public wclWeDoMotorDirection Direction { get { return FDirection; } }
        /// <summary> Gets the motor's braking state. </summary>
        /// <value> <c>True</c> if the motor is in brake state. <c>False</c> otheerwise. </value>
        public Boolean IsBraking { get { return FPower == MOTOR_POWER_BRAKE; } }
        /// <summary> Gets the motor's drifting state. </summary>
        /// <value> <c>True</c> if the motor is currently drifting or floating.
        ///   When floating the motor axis can be turned without resistance. </value>
        public Boolean IsDrifting { get { return FPower == MOTOR_POWER_DRIFT; } }
        /// <summary> Gets the power the motor is currently running with (0 if braking or drifting). </summary>
        /// <value> Teh current motor power. </value>
        public Byte Power { get { return GetPower(); } }
    };
}
