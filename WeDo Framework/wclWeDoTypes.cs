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

namespace wclWeDoFramework
{
    /// <summary> The sensor's data unit. </summary>
	public enum wclWeDoSensorDataUnit
    {
        /// <summary> Raw. </summary>
        suRaw,
        /// <summary> Percentage. </summary>
        suPercentage,
        /// <summary> SI. </summary>
        suSi,
        /// <summary> Unknown. </summary>
        suUnknown
    };

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
    };
}
