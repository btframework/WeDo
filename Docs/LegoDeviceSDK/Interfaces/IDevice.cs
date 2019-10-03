using System.Collections.Generic;
using LegoDeviceSDK.Generic;

namespace LegoDeviceSDK.Interfaces {

	internal delegate void ValueChangeCompletedHandler(byte[] bytes, IDevice device);

	internal delegate void DeviceConnectionUpdatedHandler(bool isConnected);

	/// <summary>
	/// A device represents the physical device / Hub. The device may have a number of services (inputs, motors, etc).
	/// The <see cref="IDeviceManager"/> can be used to scan for and connect to an <see cref="IDevice"/>.
	/// Implement the <see cref="IDeviceManagerDelegate"/> to be notified about changes to the service attributes, for instance when a sensor has a new reading.
	/// </summary>
	public interface IDevice {
		/// <summary>
		/// Battery level of the WeDo
		/// </summary>
		uint BatteryLevel { get; }
		/// <summary>
		/// Is the button pressed
		/// </summary>
		bool ButtonPressed { get; }
		/// <summary>
		/// The <see cref="DeviceState"/> of the device
		/// </summary>
		DeviceState ConnectState { get; }
		/// <summary>
		/// The device ID of the device
		/// </summary>
		string DeviceId { get; }
		/// <summary>
		/// The <see cref="DeviceInfo"/> of the Device
		/// </summary>
		DeviceInfo DeviceInfo { get; }
		/// <summary>
		/// The Name of the Device
		/// </summary>
		string Name { get; set; }
        /// <summary>
        /// Has a low voltage alert been received from the device indicating that batteries should be changed or charged?
        /// </summary>
        bool LowVoltage { get; }

		/// <summary>
		/// A list of external <see cref="IService"/> connected to the Device
		/// </summary>
		List<IService> ExternalServices { get; }
		/// <summary>
		/// A list of internal <see cref="IService"/> connected to the Device
		/// </summary>
		List<IService> InternalServices { get; }
		/// <summary>
		/// A list of all <see cref="IService"/> (internal and external) connected to the Device
		/// </summary>
		List<IService> Services { get; }

		/// <summary>
		/// If a delegate is registered it receives callbacks on changes to offered <see cref="IService"/>, as well as properties of the device like <see cref="Name"/> and color. 
		/// </summary>
		/// <param name="deviceDelegate">The delegate to add</param>
		void AddDelegate(IDeviceDelegate deviceDelegate);
		/// <summary>
		/// Remove delegate from this device
		/// </summary>
		/// <param name="deviceDelegate">The delegate to remove</param>
		void RemoveDelegate(IDeviceDelegate deviceDelegate);

		/// <summary>
		/// Returns YES if this device is equal to otherDevice
		/// </summary>
		/// <param name="other">The device to be compared to the receiver</param>
		/// <returns>True if devices are equal</returns>
		bool Equals(IDevice other);

	}
}
