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

using wclCommon;
using wclBluetooth;

namespace wclWeDoFramework
{
    /// <summary> The class represents a WeDo Hub current sensor. </summary>
    /// <seealso cref="wclWeDoIo"/>
    public class wclWeDoCurrentSensor : wclWeDoIo
    {
        private float GetCurrent()
        {
            if (InputFormat == null || InputFormat.Mode != 0 || InputFormat.Unit != wclWeDoSensorDataUnit.suSi)
                return 0f;

            return AsFloat;
        }

        /// <summary> The method called when data value has been changed. </summary>
        protected override void ValueChanged()
        {
            DoCurrentChanged();
        }

        /// <summary> Fires the <c>OnCurrentChanged</c> event. </summary>
        protected virtual void DoCurrentChanged()
        {
            if (OnCurrentChanged != null)
                OnCurrentChanged(this, EventArgs.Empty);
        }

        /// <summary> Creates new current sensor device object. </summary>
        /// <param name="Hub"> The Hub object that owns the device. If this parameter is <c>null</c>
        ///   the <seealso cref="wclEInvalidArgument"/> exception raises. </param>
        /// <param name="ConnectionId"> The device's Connection ID. </param>
        /// <seealso cref="wclWeDoHub"/>
        /// <exception cref="wclEInvalidArgument"> The exception raises when the <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoCurrentSensor(wclWeDoHub Hub, Byte ConnectionId)
            : base(Hub, ConnectionId)
        {
            DefaultInputFormat = new wclWeDoInputFormat(ConnectionId, wclWeDoIoDeviceType.iodCurrentSensor, 0, 30,
                wclWeDoSensorDataUnit.suSi, true, 0, 1);

            OnCurrentChanged = null;
        }

        /// <summary> Gets the battery current in mA. </summary>
        /// <value> The current in milli ampers. </value>
        public float Current { get { return GetCurrent(); } }

        /// <summary> The event fires when current has been changed. </summary>
        /// <seealso cref="EventHandler"/>
        public event EventHandler OnCurrentChanged;
    };

    /// <summary> The class represents a WeDo Hub Voltage sensor. </summary>
    /// <seealso cref="wclWeDoIo"/>
    public class wclWeDoVoltageSensor : wclWeDoIo
    {
        private float GetVoltage()
        {
            if (InputFormat == null || InputFormat.Mode != 0 || InputFormat.Unit != wclWeDoSensorDataUnit.suSi)
                return 0f;

            return AsFloat;
        }

        /// <summary> The method called when data value has been changed. </summary>
        protected override void ValueChanged()
        {
            DoVoltageChanged();
        }

        /// <summary> Fires the <c>OnVoltageChanged</c> event. </summary>
        protected virtual void DoVoltageChanged()
        {
            if (OnVoltageChanged != null)
                OnVoltageChanged(this, EventArgs.Empty);
        }

        /// <summary> Creates new voltage sensor device object. </summary>
        /// <param name="Hub"> The Hub object that owns the device. If this parameter is <c>null</c>
        ///   the <seealso cref="wclEInvalidArgument"/> exception raises. </param>
        /// <param name="ConnectionId"> The device's Connection ID. </param>
        /// <seealso cref="wclWeDoHub"/>
        /// <exception cref="wclEInvalidArgument"> The exception raises when the <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoVoltageSensor(wclWeDoHub Hub, Byte ConnectionId)
            : base(Hub, ConnectionId)
        {
            DefaultInputFormat = new wclWeDoInputFormat(ConnectionId, wclWeDoIoDeviceType.iodVoltageSensor, 0, 30,
                wclWeDoSensorDataUnit.suSi, true, 0, 1);

            OnVoltageChanged = null;
        }

        /// <summary> Gets the current battery voltage in milli volts. </summary>
        /// <value> The battery voltage in milli volts. </value>
		public float Voltage { get { return GetVoltage(); } }

        /// <summary> The event fires when voltage has been changed. </summary>
        /// <seealso cref="EventHandler"/>
        public event EventHandler OnVoltageChanged;
    };

    /// <summary> The base class for WeDo sensors that can be reset. </summary>
    /// <seealso cref="wclWeDoIo"/>
    public abstract class wclWeDoResetableSensor : wclWeDoIo
    {
        /// <summary> Creates new device object. </summary>
        /// <param name="Hub"> The Hub object that owns the device. If this parameter is <c>null</c>
        ///   the <seealso cref="wclEInvalidArgument"/> exception raises. </param>
        /// <param name="ConnectionId"> The device's Connection ID. </param>
        /// <seealso cref="wclWeDoHub"/>
        /// <exception cref="wclEInvalidArgument"> The exception raises when the <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoResetableSensor(wclWeDoHub Hub, Byte ConnectionId)
            : base(Hub, ConnectionId)
        {
        }

        /// <summary> Resets the sensor. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 Reset()
        {
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            return base.ResetSensor();
        }
    };

    /// <summary> Supported modes for the motion sensor. </summary>
	public enum wclWeDoMotionSensorMode
    {
        /// <summary> Detect mode - produces value that reflect the relative distance from the sensor
        ///   to objects in front of it. </summary>
        mmDetect = 0,
        /// <summary> Count mode - produces values that reflect how many times the sensor has been
        ///   activated. </summary>
        mmCount = 1,
        /// <summary> Unknown (unsupported) mode. </summary>
        mmUnknown = 255
    };

    /// <summary> The class represents a WeDo Motion Sensor. </summary>
    /// <seealso cref="wclWeDoResetableSensor"/>
    public class wclWeDoMotionSensor : wclWeDoResetableSensor
    {
        private UInt32 FCount;
        private float FDistance;

        private UInt32 GetCount()
        {
            if (Mode != wclWeDoMotionSensorMode.mmCount)
                return 0;

            return FCount;
        }

        private float GetDistance()
        {
            if (Mode != wclWeDoMotionSensorMode.mmDetect)
                return 0f;

            return FDistance;
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

        /// <summary> Fires the <c>OnVoltageChanged</c> event. </summary>
        protected override void ValueChanged()
        {
            if (Mode == wclWeDoMotionSensorMode.mmDetect)
            {
                if (Value.Length == 4)
                    FDistance = AsFloat;
                else
                    FDistance = AsInteger;
                DoDistanceChanged();
            }
            else
            {
                if (Mode == wclWeDoMotionSensorMode.mmCount)
                {
                    if (Value.Length == 4)
                        FCount = (UInt32)AsInteger;
                    else
                        FCount = (UInt32)AsInteger;
                    DoCountChanged();
                }
            }
        }

        /// <summary> Fires the <c>OnCountChanged</c> event. </summary>
        protected virtual void DoCountChanged()
        {
            if (OnCountChanged != null)
                OnCountChanged(this, EventArgs.Empty);
        }

        /// <summary> Fires the <c>OnDistanceChanged</c> event. </summary>
        protected virtual void DoDistanceChanged()
        {
            if (OnDistanceChanged != null)
                OnDistanceChanged(this, EventArgs.Empty);
        }

        /// <summary> Fires the <c>OnModeChanged</c> event. </summary>
        protected virtual void DoModeChanged()
        {
            if (OnModeChanged != null)
                OnModeChanged(this, EventArgs.Empty);
        }

        /// <summary> Creates new motion sensor device object. </summary>
        /// <param name="Hub"> The Hub object that owns the device. If this parameter is <c>null</c>
        ///   the <seealso cref="wclEInvalidArgument"/> exception raises. </param>
        /// <param name="ConnectionId"> The device's Connection ID. </param>
        /// <seealso cref="wclWeDoHub"/>
        /// <exception cref="wclEInvalidArgument"> The exception raises when the <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoMotionSensor(wclWeDoHub Hub, Byte ConnectionId)
            : base(Hub, ConnectionId)
        {
            FCount = 0;
            FDistance = 0;

            //FormatWithModeName(string modeName, uint modeValue, InputFormatUnit unit, uint sizeOfDataSet, uint dataSetCount)
            AddValidDataFormat(new wclWeDoDataFormat(1, 1, (Byte)wclWeDoMotionSensorMode.mmDetect, wclWeDoSensorDataUnit.suRaw));
            AddValidDataFormat(new wclWeDoDataFormat(1, 1, (Byte)wclWeDoMotionSensorMode.mmDetect, wclWeDoSensorDataUnit.suPercentage));
            AddValidDataFormat(new wclWeDoDataFormat(1, 4, (Byte)wclWeDoMotionSensorMode.mmDetect, wclWeDoSensorDataUnit.suSi));
            AddValidDataFormat(new wclWeDoDataFormat(1, 4, (Byte)wclWeDoMotionSensorMode.mmCount, wclWeDoSensorDataUnit.suRaw));
            AddValidDataFormat(new wclWeDoDataFormat(1, 1, (Byte)wclWeDoMotionSensorMode.mmCount, wclWeDoSensorDataUnit.suPercentage));
            AddValidDataFormat(new wclWeDoDataFormat(1, 4, (Byte)wclWeDoMotionSensorMode.mmCount, wclWeDoSensorDataUnit.suSi));

            DefaultInputFormat = new wclWeDoInputFormat(ConnectionId, wclWeDoIoDeviceType.iodMotionSensor, 0, 1,
                wclWeDoSensorDataUnit.suRaw, true, 0, 4);

            OnCountChanged = null;
            OnDistanceChanged = null;
            OnModeChanged = null;
        }

        /// <summary> Sets the motion sensor mode. </summary>
        /// <param name="Mode"> The motion sensor mode. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seealso cref="wclWeDoMotionSensorMode"/>
        public Int32 SetMode(wclWeDoMotionSensorMode Mode)
        {
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            return SetInputFormatMode((Byte)Mode);
        }

        /// <summary> Gets the most recent count reading from the sensor. </summary>
        /// <value> The detections count. </value>
		public uint Count { get { return GetCount(); } }
        /// <summary> Gets the most recent distance reading from the sensor. </summary>
        /// <value> The distance. </value>
		public float Distance { get { return GetDistance(); } }
        /// <summary> Gets the current mode of the motion sensor. </summary>
        /// <value> The sensor mode. </value>
        /// <seealso cref="wclWeDoMotionSensorMode"/>
		public wclWeDoMotionSensorMode Mode { get { return (wclWeDoMotionSensorMode)InputFormatMode; } }

        /// <summary> The event fires when the counter has been changed. </summary>
        /// <seealso cref="EventHandler"/>
        public event EventHandler OnCountChanged;
        /// <summary> The event fires when distance has been changed. </summary>
        /// <seealso cref="EventHandler"/>
        public event EventHandler OnDistanceChanged;
        /// <summary> The event fires when the mode has been changed. </summary>
        public event EventHandler OnModeChanged;
    };

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
                (Byte)wclWeDoTiltSensorMode.tmTilt, 1, wclWeDoSensorDataUnit.suRaw, true, 0, 1);

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
