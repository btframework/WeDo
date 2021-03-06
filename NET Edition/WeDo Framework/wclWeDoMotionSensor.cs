﻿////////////////////////////////////////////////////////////////////////////////
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
}
