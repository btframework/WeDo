using LegoDeviceSDK.Bluetooth;
using LegoDeviceSDK.Generic.Data;

namespace LegoDeviceSDK.Interfaces.Services {
	/// <summary>
    /// Implement this protocol to be notified when the <see cref="ITiltSensor"/> updates its value
	/// </summary>
	public interface ITiltSensorDelegate : IServiceDelegate {


		/// <summary>
		/// Invoked when the tilt sensor has an updated value for angle.
		/// </summary>
		/// <param name="tiltSensor">The tilt sensor</param>
		/// <param name="oldAngle">The old angle</param>
		/// <param name="newAngle">The new angle</param>
		void DidUpdateAngle(ITiltSensor tiltSensor, TiltSensorAngle oldAngle, TiltSensorAngle newAngle);

		/// <summary>
		/// Invoked when the tilt sensor has an updated value for crash readings.
		/// </summary>
		/// <param name="tiltSensor">The tilt sensor</param>
		/// <param name="oldCrashValue">The previous crash value</param>
		/// <param name="newCrashValue">The new crash value</param>
        void DidUpdateCrash(ITiltSensor tiltSensor, TiltSensorCrash oldCrashValue, TiltSensorCrash newCrashValue);

		/// <summary>
		/// Invoked when the tilt sensor has an updated value for direction.
		/// </summary>
		/// <param name="tiltSensor">The tilt sensor</param>
		/// <param name="oldDirection">The previous direction</param>
		/// <param name="newDirection">The new direction</param>
        void DidUpdateDirection(ITiltSensor tiltSensor, TiltSensorDirection oldDirection, TiltSensorDirection newDirection);

	}
}
