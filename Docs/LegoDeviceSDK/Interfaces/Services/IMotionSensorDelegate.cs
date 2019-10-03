using LegoDeviceSDK.Bluetooth;

namespace LegoDeviceSDK.Interfaces.Services {
	/// <summary>
	/// Implement this protocol to be notified when the <see cref="BluetoothMotionSensor"/> updates its value
	/// </summary>
	public interface IMotionSensorDelegate : IServiceDelegate {

		/// <summary>
		/// Invoked when the motion sensor has an updated distance value
		/// </summary>
		/// <param name="motionSensor">The sensor that has a new value</param>
		/// <param name="oldDistance">The previous distance reading</param>
		/// <param name="newDistance">The new distance reading</param>
		void DidUpdateDistance(IMotionSensor motionSensor, float oldDistance, float newDistance);

		/// <summary>
		/// Invoked when the motion sensor has an updated count value
		/// </summary>
		/// <param name="motionSensor">The sensor that has a new value</param>
		/// <param name="newCount">The new value</param>
        void DidUpdateCount(IMotionSensor motionSensor, uint newCount);
	}
}
