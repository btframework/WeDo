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
        /// <summary> Powered Up Medium Motor. </summary>
        iodMediumMotor,
        /// <summary> Powered Up Train Motor. </summary>
        iodTrainMotor,
        /// <summary> Powered Up Lights. </summary>
        iodLights,
        /// <summary> Powered Up Hub battery voltage. </summary>
        iodVoltageSensor,
        /// <summary> Powered Up Hub battery current. </summary>
        iodCurrentSensor,
        /// <summary> Powered Up Hub piezo tone (WeDo 2.0 only). </summary>
        iodWeDo20Piezo,
        /// <summary> Powered Up Hub indicator light. </summary>
        iodRgb,
        /// <summary> EV3 Color Sensor. </summary>
        iodEv3ColorSensor,
        /// <summary> EV3 Ultrasonic Sensor. </summary>
        iodEv3UltrasonicSensor,
        /// <summary> EV3 Gyro Sensor. </summary>
        iodEv3GyroSensor,
        /// <summary> EV3 Infrared Sensor. </summary>
        iodEv3InfraredSensor,
        /// <summary> WeDo 2.0 Tilt Sensor. </summary>
        iodWeDo20TiltSensor,
        /// <summary> WeDo 2.0 Motion Sensor. </summary>
        iodWeDo20MotionSensor,
        /// <summary> WeDo 2.0 generic device. </summary>
        iodWeDo20Generic,
        /// <summary> BOOST Color and Distance Sensor. </summary>
        iodBoostColorSensor,
        /// <summary> BOOST Interactive Motor. </summary>
        iodBoostInteractiveMotor,
        /// <summary> BOOST Move Hub motor. </summary>
        iodBoostHubMotor,
        /// <summary> BOOST Move Hub Tilt sensor. </summary>
        iodBoostHubTiltSensor,
        /// <summary> DUPLO Train hub motor. </summary>
        iodDuploHubMotor,
        /// <summary> DUPLO Train hub beeper. </summary>
        iodDuploHubBeeper,
        /// <summary> DUPLO Train hub color sensor. </summary>
        iodDuploHubColorSensor,
        /// <summary> DUPLO Train hub speed. </summary>
        iodDuploHubSpeedSensor,
        /// <summary> Technic Control+ Large Motor. </summary>
        iodTechnicLargeMotor,
        /// <summary> Technic Control+ XL Motor. </summary>
        iodTechnicXlMotor,
        /// <summary> SPIKE Prime Medium Motor. </summary>
        iodSpikeMediumMotor,
        /// <summary> SPIKE Prime Large Motor. </summary>
        iodSpikeLargeMotor,
        /// <summary> Powered Up hub IMU gesture. </summary>
        iodImuGesture,
        /// <summary> Powered Up Handset Buttons. </summary>
        iodHandsetButtons,
        /// <summary> Powered Up Handset Light. </summary>
        iodHandsetLight,
        /// <summary> Powered Up hub IMU accelerometer. </summary>
        iodImuAccel,
        /// <summary> Powered Up hub IMU gyro. </summary>
        iodImuGyro,
        /// <summary> Powered Up hub IMU position. </summary>
        iodImuPosition,
        /// <summary> Powered Up hub IMU temperature. </summary>
        iodImuTemp,
        /// <summary> SPIKE Prime Color Sensor. </summary>
        iodSpikeColorSensor,
        /// <summary> SPIKE Prime Ultrasonic Sensor. </summary>
        iodSpikeUltrasonicSensor,
        /// <summary> SPIKE Prime Force Sensor. </summary>
        iodSpikeUForceSensor,
        /// <summary> A type is unknown. </summary>
        iodUnknown
    };
}
