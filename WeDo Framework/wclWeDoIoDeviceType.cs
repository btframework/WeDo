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
}
