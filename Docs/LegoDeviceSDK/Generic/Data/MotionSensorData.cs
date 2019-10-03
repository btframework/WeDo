namespace LegoDeviceSDK.Generic.Data {
	/// <summary>
	/// Supported modes for the motion sensor
	/// </summary>
	public enum MotionSensorMode {
		/// <summary>
		/// Detect mode - produces value that reflect the relative distance from the sensor to objects in front of it
		/// </summary>
		Detect = 0,
		/// <summary>
		/// Count mode - produces values that reflect how many times the sensor has been activated
		/// </summary>
		Count = 1,
		/// <summary>
		/// Unknown (unsupported) mode
		/// </summary>
		Unknown,
	}
}
