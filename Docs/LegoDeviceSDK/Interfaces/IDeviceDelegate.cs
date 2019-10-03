using System;
using LegoDeviceSDK.Generic;

namespace LegoDeviceSDK.Interfaces {

	/// <summary>
	/// Implement this protocol to be notified about changes to the attributes of the device. The delegate will notify you when an IO such as a motor or sensor is attached or detached from the Device. The delegate will also notify about changes to the device name, battery level and the connect-button state (pressed/released).
	/// </summary>
	public interface IDeviceDelegate {
		/// <summary>
		/// Invoked when a new <see cref="DeviceInfo"/> with info about the software and firmware revision is received from the <see cref="IDevice"/>
		/// </summary>
		/// <param name="device">The device</param>
		/// <param name="deviceInfo">The changed <see cref="DeviceInfo"/> for the <see cref="IDevice"/></param>
		/// <param name="error">If an error occurred, the cause of the failure.</param>
		void DidUpdateDeviceInfo(IDevice device, DeviceInfo deviceInfo, Exception error);

		/// <summary>
		/// Invoked when the user press or release the connect-button on the device
		/// </summary>
		/// <param name="device">The device</param>
		/// <param name="buttonState">YES if the button is pressed, NO otherwise.</param>
		void DidChangeButtonState(IDevice device, bool buttonState);

		/// <summary>
		/// Invoked if the device sends an updated device name
		/// </summary>
		/// <param name="device">The device</param>
		/// <param name="fromName">The previous name of the device</param>
		/// <param name="toName">The new name of the device</param>
		void DidChangeName(IDevice device, string fromName, string toName);

		/// <summary>
		/// Invoked when a new device sends an updated battery level
		/// </summary>
		/// <param name="device">The device</param>
		/// <param name="newLevel">The new battery level as a number between 0 and 100.</param>
		void DidUpdateBatteryLevel(IDevice device, uint newLevel);

        /// <summary>
        /// Invoked when the device sends a low voltage notification
        /// </summary>
        /// <param name="device">The device</param>
        /// <param name="lowVoltage">true if the battery has 'low voltage', false otherwise.</param>
        void DidChangeLowVoltageState(IDevice device, bool lowVoltage);


		/// <summary>
		/// Invoked when the a new motor, sensor or other service is attached to the device (Hub).
		/// </summary>
		/// <param name="device">The device</param>
		/// <param name="service">The attached service</param>
		void DidAddService(IDevice device, IService service);

		/// <summary>
		/// Invoked when a new motor, sensor or other service is detached from the device (Hub).
		/// </summary>
		/// <param name="device">The device</param>
		/// <param name="service">The detached service</param>
		void DidRemoveService(IDevice device, IService service);

		/// <summary>
		/// Invoked when an update from the device about attached services (sensor, motors, etc) could not be understood
		/// </summary>
		/// <param name="device">The device</param>
		/// <param name="error">The cause of the failure</param>
		void DidFailToAddServiceWithError(IDevice device, Exception error);
	}
}
