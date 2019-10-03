using System;

namespace LegoDeviceSDK.Generic.Data {
	/// <summary>
	/// Tilt sensor mode
	/// </summary>
	public enum TiltSensorMode {
		/// <summary>
		/// Angle
		/// </summary>
		TiltSensorModeAngle = 0,
		/// <summary>
		/// Tilt
		/// </summary>
		TiltSensorModeTilt = 1,
		/// <summary>
		/// Crash
		/// </summary>
		LeTiltSensorModeCrash = 2,
		/// <summary>
		/// Unkown
		/// </summary>
		TiltSensorModeUnknown,

	}
	/// <summary>
	/// Direction of tilt sensor
	/// </summary>
	public enum TiltSensorDirection {
		/// <summary>
		/// Neutral
		/// </summary>
		TiltSensorDirectionNeutral = 0,
		/// <summary>
		/// Backward
		/// </summary>
		TiltSensorDirectionBackward = 3,
        /// <summary>
        /// Right
        /// </summary>
        TiltSensorDirectionRight = 5,
        /// <summary>
        /// Left
        /// </summary>
        TiltSensorDirectionLeft = 7,
		/// <summary>
		/// Forward
		/// </summary>
		TiltSensorDirectionForward = 9,
		/// <summary>
		/// Unknown
		/// </summary>
		TiltSensorDirectionUnknown,
	
	}

	/// <summary>
	/// Representation of the tilt sensor angle values
	/// </summary>
	public class TiltSensorAngle {
		/// <summary>
		/// X value of tilt angle
		/// </summary>
		public float X { get; set; }
		/// <summary>
		/// Y value of tilt angle
		/// </summary>
		public float Y { get; set; }

		/// <summary>
		/// Compares two Angles
		/// </summary>
		/// <param name="other">Angle to compare to</param>
		/// <returns>True if angles are equal</returns>
		public bool Equals(TiltSensorAngle other) {
			return (Math.Abs(X - other.X) < float.Epsilon) && (Math.Abs(Y - other.Y) < float.Epsilon);
		}
	}

	/// <summary>
	/// Representation of the tilt sensor crash values
	/// </summary>
	public class TiltSensorCrash {
		/// <summary>
		/// X value of crash
		/// </summary>
		public float X { get; set; }
		/// <summary>
		/// Y value of crash
		/// </summary>
		public float Y { get; set; }
		/// <summary>
		/// Z value of crash
		/// </summary>
		public float Z { get; set; }
		/// <summary>
		/// Compares two sensor crash values
		/// </summary>
		/// <param name="other">crash value to compare to</param>
		/// <returns>True if crash values are equal</returns>
		public bool Equals(TiltSensorCrash other) {
			return (Math.Abs(X - other.X) < float.Epsilon) && (Math.Abs(Y - other.Y) < float.Epsilon) && (Math.Abs(Z - other.Z) < float.Epsilon);
		}
	}

}
