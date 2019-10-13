using System;

using wclCommon;
using wclBluetooth;

namespace wclWeDoFramework
{
    /// <summary> The enumeration describes the Tilt sensor modes. </summary>
	public enum wclWeDoTiltSensorMode
    {
        /// <summary> Angle. </summary>
        tmAngle = 0,
        /// <summary> Tilt. </summary>
        tmTilt = 1,
        /// <summary> Crash. </summary>
        tmCrash = 2,
        /// <summary> tmUnkown. </summary>
        tmUnknown = 255
    };

    /// <summary> The enumeration describes th Tilt sensor directions. </summary>
	public enum wclWeDoTiltSensorDirection
    {
        /// <summary> Neutral. </summary>
        tdNeutral = 0,
        /// <summary> Backward. </summary>
        tdBackward = 3,
        /// <summary> Right. </summary>
        tdRight = 5,
        /// <summary> Left. </summary>
        tdLeft = 7,
        /// <summary> Forward. </summary>
        tdForward = 9,
        /// <summary> Unknown. </summary>
        tdUnknown = 255
    };

    /// <summary> The record represents the tilt sensor angle values. </summary>
	public struct wclWeDoTiltSensorAngle
    {
        /// <summary>  The X value of tilt angle. </summary>
        public float X;
        /// <summary> The Y value of tilt angle. </summary>
        public float Y;

        /// <summary> Compares angles. </summary>
		/// <param name="obj"> The object to compare to. </param>
		/// <returns> <c>True</c> if objects are equal. </returns>
		public override Boolean Equals(Object obj)
        {
            if (object.ReferenceEquals(obj, null))
                return false;
            if (GetType() != obj.GetType())
                return false;

            wclWeDoTiltSensorAngle Angle = (wclWeDoTiltSensorAngle)obj;
            return ((Math.Abs(X - Angle.X) < float.Epsilon) && (Math.Abs(Y - Angle.Y) < float.Epsilon));
        }

        /// <summary> Gets the object hash. </summary>
        /// <returns> The objects hash. </returns>
        public override Int32 GetHashCode()
        {
            return (X + Y).GetHashCode();
        }

        /// <summary> Override the <c>==</c> operator. </summary>
        /// <param name="a"> First argument. </param>
        /// <param name="b"> Second argument. </param>
        /// <returns> <c>True</c> if a == b. <c>False</c> otherwise. </returns>
        public static Boolean operator ==(wclWeDoTiltSensorAngle a, wclWeDoTiltSensorAngle b)
        {
            return a.Equals(b);
        }

        /// <summary> Override the <c>!==</c> operator. </summary>
        /// <param name="a"> First argument. </param>
        /// <param name="b"> Second argument. </param>
        /// <returns> <c>True</c> if a != b. <c>False</c> otherwise. </returns>
        public static Boolean operator !=(wclWeDoTiltSensorAngle a, wclWeDoTiltSensorAngle b)
        {
            return !a.Equals(b);
        }
    };

    /// <summary> The structure represents the Tilt sensor crash values. </summary>
	public struct wclWeDoTiltSensorCrash
    {
        /// <summary> The X value of crash. </summary>
        public float X;
        /// <summary> The Y value of crash. </summary>
        public float Y;
        /// <summary> The Z value of crash. </summary>
        public float Z;

        /// <summary> Compares crashes. </summary>
		/// <param name="obj"> The object to compare to. </param>
		/// <returns> <c>True</c> if objects are equal. </returns>
		public override Boolean Equals(Object obj)
        {
            if (object.ReferenceEquals(obj, null))
                return false;
            if (GetType() != obj.GetType())
                return false;

            wclWeDoTiltSensorCrash Crash = (wclWeDoTiltSensorCrash)obj;
            return ((Math.Abs(X - Crash.X) < float.Epsilon) && (Math.Abs(Y - Crash.Y) < float.Epsilon) &&
                (Math.Abs(Z - Crash.Z) < float.Epsilon));
        }

        /// <summary> Gets the object hash. </summary>
        /// <returns> The objects hash. </returns>
        public override Int32 GetHashCode()
        {
            return (X + Y + Z).GetHashCode();
        }

        /// <summary> Override the <c>==</c> operator. </summary>
        /// <param name="a"> First argument. </param>
        /// <param name="b"> Second argument. </param>
        /// <returns> <c>True</c> if a == b. <c>False</c> otherwise. </returns>
        public static Boolean operator ==(wclWeDoTiltSensorCrash a, wclWeDoTiltSensorCrash b)
        {
            return a.Equals(b);
        }

        /// <summary> Override the <c>!==</c> operator. </summary>
        /// <param name="a"> First argument. </param>
        /// <param name="b"> Second argument. </param>
        /// <returns> <c>True</c> if a != b. <c>False</c> otherwise. </returns>
        public static Boolean operator !=(wclWeDoTiltSensorCrash a, wclWeDoTiltSensorCrash b)
        {
            return !a.Equals(b);
        }
    };

    /// <summary> The class represents a WeDo Titl Sensor device. </summary>
    /// <seealso cref="wclWeDoResetableSensor"/>
    public class wclWeDoTiltSensor : wclWeDoResetableSensor
    {
        private Int32 ConvertToSigned(Byte b)
        {
            Int32 Signed = (Int32)b;
            if (Signed > 127)
                Signed = 0 - (256 - Signed);
            return Signed;
        }

        private Int32 ConvertToUnsigned(Byte b)
        {
            return BitConverter.ToInt16(new Byte[] { b, 0 }, 0);
        }

        private wclWeDoTiltSensorAngle GetAngle()
        {
            if (Mode != wclWeDoTiltSensorMode.tmAngle || NumbersFromValueData == null || NumbersFromValueData.Count != 2)
            {
                return new wclWeDoTiltSensorAngle()
                {
                    X = 0,
                    Y = 0
                };
            }

            if (InputFormat.Unit == wclWeDoSensorDataUnit.suSi && NumbersFromValueData[0].Length == 4)
            {
                return new wclWeDoTiltSensorAngle()
                {
                    X = BitConverter.ToSingle(NumbersFromValueData[0], 0),
                    Y = BitConverter.ToSingle(NumbersFromValueData[1], 0)
                };
            }

            return new wclWeDoTiltSensorAngle()
            {
                X = ConvertToSigned(NumbersFromValueData[0][0]),
                Y = ConvertToSigned(NumbersFromValueData[1][0])
            };
        }

        private wclWeDoTiltSensorCrash GetCrash()
        {
            if (Mode != wclWeDoTiltSensorMode.tmCrash || NumbersFromValueData == null || NumbersFromValueData.Count != 3)
            {
                return new wclWeDoTiltSensorCrash()
                {
                    X = 0,
                    Y = 0,
                    Z = 0
                };
            }

            if (InputFormat.Unit == wclWeDoSensorDataUnit.suSi && NumbersFromValueData[0].Length == 4)
            {
                return new wclWeDoTiltSensorCrash()
                {
                    X = BitConverter.ToSingle(NumbersFromValueData[0], 0),
                    Y = BitConverter.ToSingle(NumbersFromValueData[1], 0),
                    Z = BitConverter.ToSingle(NumbersFromValueData[2], 0)
                };
            }

            return new wclWeDoTiltSensorCrash()
            {
                X = ConvertToUnsigned(NumbersFromValueData[0][0]),
                Y = ConvertToUnsigned(NumbersFromValueData[1][0]),
                Z = ConvertToUnsigned(NumbersFromValueData[2][0])
            };
        }

        private wclWeDoTiltSensorDirection GetDirection()
        {
            if (Mode != wclWeDoTiltSensorMode.tmTilt)
                return wclWeDoTiltSensorDirection.tdUnknown;

            return (wclWeDoTiltSensorDirection)AsInteger;
        }

        /// <summary> The method called when Input Format has been changed. </summary>
        /// <param name="OldFormat"> The old Input Format. </param>
        protected override void InputFormatChanged(wclWeDoInputFormat OldFormat)
        {
            if (OldFormat != null)
            {

                if (InputFormat == null)
                    DoModeChanged();
                else
                {
                    if (InputFormat.Mode != OldFormat.Mode)
                        DoModeChanged();
                }
            }
        }

        /// <summary> Fires the <c>OnVoltageChanged</c> event. </summary>
        protected override void ValueChanged()
        {
            if (Mode == wclWeDoTiltSensorMode.tmAngle)
                DoAngleChanged();
            else
            {
                if (Mode == wclWeDoTiltSensorMode.tmCrash)
                    DoCrashChanged();
                else
                {
                    if (Mode == wclWeDoTiltSensorMode.tmTilt)
                        DoDirectionChanged();
                }
            }
        }

        /// <summary> Fires the <c>OnAngleChanged</c> event. </summary>
        protected virtual void DoAngleChanged()
        {
            if (OnAngleChanged != null)
                OnAngleChanged(this, EventArgs.Empty);
        }

        /// <summary> Fires the <c>OnCrashChanged</c> event. </summary>
        protected virtual void DoCrashChanged()
        {
            if (OnCrashChanged != null)
                OnCrashChanged(this, EventArgs.Empty);
        }

        /// <summary> Fires the <c>OnDirectionChanged</c> event. </summary>
        protected virtual void DoDirectionChanged()
        {
            if (OnDirectionChanged != null)
                OnDirectionChanged(this, EventArgs.Empty);
        }

        /// <summary> Fires the <c>OnModeChanged</c> event. </summary>
        protected virtual void DoModeChanged()
        {
            if (OnModeChanged != null)
                OnModeChanged(this, EventArgs.Empty);
        }

        /// <summary> Creates new tilt sensor device object. </summary>
        /// <param name="Hub"> The Hub object that owns the device. If this parameter is <c>null</c>
        ///   the <seealso cref="wclEInvalidArgument"/> exception raises. </param>
        /// <param name="ConnectionId"> The device's Connection ID. </param>
        /// <seealso cref="wclWeDoHub"/>
        /// <exception cref="wclEInvalidArgument"> The exception raises when the <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoTiltSensor(wclWeDoHub Hub, Byte ConnectionId)
            : base(Hub, ConnectionId)
        {
            AddValidDataFormat(new wclWeDoDataFormat(2, 1, (Byte)wclWeDoTiltSensorMode.tmAngle, wclWeDoSensorDataUnit.suRaw));
            AddValidDataFormat(new wclWeDoDataFormat(2, 1, (Byte)wclWeDoTiltSensorMode.tmAngle, wclWeDoSensorDataUnit.suPercentage));
            AddValidDataFormat(new wclWeDoDataFormat(2, 4, (Byte)wclWeDoTiltSensorMode.tmAngle, wclWeDoSensorDataUnit.suSi));

            AddValidDataFormat(new wclWeDoDataFormat(1, 1, (Byte)wclWeDoTiltSensorMode.tmTilt, wclWeDoSensorDataUnit.suRaw));
            AddValidDataFormat(new wclWeDoDataFormat(1, 1, (Byte)wclWeDoTiltSensorMode.tmTilt, wclWeDoSensorDataUnit.suPercentage));
            AddValidDataFormat(new wclWeDoDataFormat(1, 4, (Byte)wclWeDoTiltSensorMode.tmTilt, wclWeDoSensorDataUnit.suSi));

            AddValidDataFormat(new wclWeDoDataFormat(3, 1, (Byte)wclWeDoTiltSensorMode.tmCrash, wclWeDoSensorDataUnit.suRaw));
            AddValidDataFormat(new wclWeDoDataFormat(3, 1, (Byte)wclWeDoTiltSensorMode.tmCrash, wclWeDoSensorDataUnit.suPercentage));
            AddValidDataFormat(new wclWeDoDataFormat(3, 4, (Byte)wclWeDoTiltSensorMode.tmCrash, wclWeDoSensorDataUnit.suSi));

            DefaultInputFormat = new wclWeDoInputFormat(ConnectionId, wclWeDoIoDeviceType.iodTiltSensor,
                (Byte)wclWeDoTiltSensorMode.tmTilt, 1, wclWeDoSensorDataUnit.suSi, true, 0, 4);

            OnAngleChanged = null;
            OnCrashChanged = null;
            OnDirectionChanged = null;
            OnModeChanged = null;
        }

        /// <summary> Sets the tilt sensor mode. </summary>
        /// <param name="Mode"> The tils sensor mode. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seealso cref="wclWeDoTiltSensorMode"/>
        public Int32 SetMode(wclWeDoTiltSensorMode Mode)
        {
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            return SetInputFormatMode((Byte)Mode);
        }

        /// <summary> Gets the most recent angle reading from the sensor. The angle represents the
        ///   angle the sensor is tilted in the x and y. </summary>
        /// <value> The Tilt sensor angle. </value>
        /// <seealso cref="wclWeDoTiltSensorAngle"/>
        public wclWeDoTiltSensorAngle Angle { get { return GetAngle(); } }
        /// <summary> Gets the most recent crash reading from the sensor.
        ///   The value represents the number of times the sensor has been ‘bumped’ in the x, y, and z.</summary>
        /// <value> The Tilt sensor crash. </value>
        /// <seealso cref="wclWeDoTiltSensorCrash"/>
        public wclWeDoTiltSensorCrash Crash { get { return GetCrash(); } }
        /// <summary> Gets the most recent direction reading from the sensor. </summary>
        /// <value> The Tilt sensor direction. </value>
        /// <seealso cref="wclWeDoTiltSensorDirection"/>
        public wclWeDoTiltSensorDirection Direction { get { return GetDirection(); } }
        /// <summary> Gets the current mode of the tilt sensor. </summary>
        /// <value> The Tilt sensor mode. </value>
        /// <seealso cref="wclWeDoTiltSensorMode"/>
        public wclWeDoTiltSensorMode Mode { get { return (wclWeDoTiltSensorMode)InputFormatMode; } }

        /// <summary> The event fires when angle has been changed. </summary>
        /// <seealso cref="EventHandler"/>
        public event EventHandler OnAngleChanged;
        /// <summary> The event fires when crash changed. </summary>
        /// <seealso cref="EventHandler"/>
        public event EventHandler OnCrashChanged;
        /// <summary> The event fires when direction has been changed. </summary>
        /// <seealso cref="EventHandler"/>
        public event EventHandler OnDirectionChanged;
        /// <summary> The event fires when Mode has been changed. </summary>
        /// /// <seealso cref="EventHandler"/>
        public event EventHandler OnModeChanged;
    };
}
