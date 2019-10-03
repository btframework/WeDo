using LegoDeviceSDK.Bluetooth;

namespace LegoDeviceSDK.Interfaces.Services {
	/// <summary>
    /// Implement this protocol to be notified when the <see cref="IVoltageSensor"/> updates its value
	/// </summary>
	public interface IVoltageSensorDelegate : IServiceDelegate {

		/// <summary>
        /// Invoked when the <see cref="IVoltageSensor"/> receives an updated value
		/// </summary>
		/// <param name="voltageSensor">The sensor that has a new value</param>
		/// <param name="milliVolts">The new voltage value in milli volts</param>
        void DidUpdateMilliVolts(IVoltageSensor voltageSensor, float milliVolts);
	}
}
