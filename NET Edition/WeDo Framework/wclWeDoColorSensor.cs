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
    /// <summary> Color sensor colors. </summary>
    public enum wclWeDoColorSensorColor
    {
        /// <summary> Black color detected. </summary>
        ccBlack,
        /// <summary> Blue color detected. </summary>
        ccBlue,
        /// <summary> Green color detected. </summary>
        ccGreen,
        /// <summary> Yellow color detected. </summary>
        ccYellow,
        /// <summary> Red color detected. </summary>
        ccRed,
        /// <summary> White color detected. </summary>
        ccWhite,
        /// <summary> No object or unknown color detected. </summary>
        ccNoObject
    };

    /// <summary> The <c>OnColorDetected</c> event handler prototype. </summary>
    /// <param name="Sender"> The object that fired the event. </param>
    /// <param name="Color"> The detected color. </param>
    /// <seealso cref="wclWeDoColorSensorColor"/>
    public delegate void wclWeDoColorDetectedEbent(Object Sender, wclWeDoColorSensorColor Color);

    /// <summary> The class represents a WeDo Color Sensor device. </summary>
    /// <seealso cref="wclWeDoIo"/>
    public class wclWeDoColorSensor : wclWeDoIo
    {
        /// <summary> Fires the <c>OnVoltageChanged</c> event. </summary>
        protected override void ValueChanged()
        {
            if (Value != null && Value.Length > 0)
            {
                switch (Value[0])
                {
                    case 0x00:
                        DoColorDetected(wclWeDoColorSensorColor.ccBlack);
                        break;
                    case 0x03:
                        DoColorDetected(wclWeDoColorSensorColor.ccBlue);
                        break;
                    case 0x05:
                        DoColorDetected(wclWeDoColorSensorColor.ccGreen);
                        break;
                    case 0x07:
                        DoColorDetected(wclWeDoColorSensorColor.ccYellow);
                        break;
                    case 0x09:
                        DoColorDetected(wclWeDoColorSensorColor.ccRed);
                        break;
                    case 0x0A:
                        DoColorDetected(wclWeDoColorSensorColor.ccWhite);
                        break;
                    case 0xFF:
                        DoColorDetected(wclWeDoColorSensorColor.ccNoObject);
                        break;
                }
            }
        }

        /// <summary> Fires the <c>OnColorDetected</c> event. </summary>
        /// <param name="Color"> The color. </param>
        /// <seealso cref="wclWeDoColorSensorColor"/>
        protected virtual void DoColorDetected(wclWeDoColorSensorColor Color)
        {
            if (OnColorDetected != null)
                OnColorDetected(this, Color);
        }

        /// <summary> Creates new color sensor device object. </summary>
        /// <param name="Hub"> The Hub object that owns the device. If this parameter is <c>null</c>
        ///   the <seealso cref="wclEInvalidArgument"/> exception raises. </param>
        /// <param name="ConnectionId"> The device's Connection ID. </param>
        /// <seealso cref="wclWeDoHub"/>
        /// <exception cref="wclEInvalidArgument"> The exception raises when the <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoColorSensor(wclWeDoHub Hub, Byte ConnectionId)
            : base(Hub, ConnectionId)
        {
            AddValidDataFormat(new wclWeDoDataFormat(1, 1, 0, wclWeDoSensorDataUnit.suRaw));

            DefaultInputFormat = new wclWeDoInputFormat(ConnectionId, wclWeDoIoDeviceType.iodColorSensor,
                0, 1, wclWeDoSensorDataUnit.suRaw, true, 0, 1);

            OnColorDetected = null;
        }

        /// <summary> The event fires when the sensor detected a color. </summary>
        /// <seealso cref="wclWeDoColorDetectedEbent"/>
        public event wclWeDoColorDetectedEbent OnColorDetected;
    };
}
