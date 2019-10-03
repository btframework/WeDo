using LegoDeviceSDK.Bluetooth;
using LegoDeviceSDK.Generic;

namespace LegoDeviceSDK.Interfaces {
	/// <summary>
	/// An <see cref="IService"/> represent an IO of some kind, for example a motor or sensor. It could also be an internal IO, such as Voltage sensor build into the device.
	/// The <see cref="IService"/> has a number of sub-classes for known IO types. This includes <see cref="BluetoothMotor"/>, <see cref="BluetoothTiltSensor"/>and <see cref="BluetoothMotionSensor"/> just to mention a few.
	/// Add a instance of a <see cref="IServiceDelegate"/> using addDelegate to be notified when a service receives an updated value.
	/// </summary>
	public interface IServiceDelegate {
		/// <summary>
		/// Invoked when a service receives an updated <see cref="InputFormat"/>
		/// </summary>
		/// <param name="service">The service that received an updated value</param>
		/// <param name="oldFormat">The previous input format</param>
		/// <param name="newFormat">The new input format</param>
		void DidUpdateInputFormat(IService service, InputFormat oldFormat, InputFormat newFormat);

		/// <summary>
		/// Invoked when a service receives an updated value. You can use of the convenience methods <see cref="IService.ValueData"/>, <see cref="IService.ValueAsInteger"/> or <see cref="IService.ValueAsFloat"/> to retrieve the value as a number.
		/// </summary>
		/// <param name="service">The service that received an updated value</param>
		/// <param name="oldData">The previous value</param>
		/// <param name="newData">The new value</param>
		void DidUpdateValueData(IService service, byte[] oldData, byte[] newData);
	}
}
