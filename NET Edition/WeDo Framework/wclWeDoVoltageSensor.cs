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

namespace wclWeDoFramework
{
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
}
