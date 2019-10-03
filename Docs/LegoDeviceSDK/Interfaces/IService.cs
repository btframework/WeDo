using System.Collections.Generic;
using LegoDeviceSDK.Bluetooth;
using LegoDeviceSDK.Generic;

namespace LegoDeviceSDK.Interfaces {
	/// <summary>
	/// An IService represent an IO of some kind, for example a motor or sensor. It could also be an internal IO, such as Voltage sensor build into the device.
	/// The IService has a number of sub-classes for known IO types. This includes <see cref="BluetoothMotor"/>, <see cref="BluetoothTiltSensor"/> and <see cref="BluetoothMotionSensor"/>just to mention a few.
	/// Add a instance of a <see cref="IServiceDelegate"/> using addDelegate to be notified when a service receives an updated value.
	/// </summary>
	public interface IService {
		/// <summary>
		/// General info about the connected service
		/// </summary>
		ConnectInfo ConnectInfo { get; }
		/// <summary>
		/// The default input format that will be uploaded to the device for this service upon discovery of the service. Only the known service types (<see cref="BluetoothMotor"/>, <see cref="BluetoothTiltSensor"/>, etc.) has a default input format.
		/// </summary>
		InputFormat DefaultInputFormat { get; }
		/// <summary>
		/// The <see cref="IDevice"/> this service is related to
		/// </summary>
		IDevice Device { get; }
		/// <summary>
		/// The current <see cref="InputFormat"/> for this service.
		/// </summary>
		InputFormat InputFormat { get; }
		/// <summary>
		/// Is YES if this service represents an internal IO, such as an <see cref="BluetoothVoltageSensor"/>
		/// </summary>
		bool IsInternalService { get; }
		/// <summary>
		/// The name of the service
		/// </summary>
		string ServiceName { get; }
		/// <summary>
		/// The data formats that this service may use to parse received data.
		/// </summary>
		/// <remarks>
		/// The data formats that this service may use to parse received data.
		/// When a new value for a service is received from the device, the SDK will look for a <see cref="DataFormat"/> among the validDataFormats that matches the <see cref="Generic.InputFormat.Unit"/> and <see cref="Generic.InputFormat.Mode"/>. If a match is found the <see cref="DataFormat"/> is used to parse the received data correctly (as a float, integer).
		/// The known service types such as <see cref="BluetoothTiltSensor"/> and <see cref="BluetoothMotionSensor"/> etc. comes with a set of predefined validDataFormat and you do not need to add valid data formats yourself. However, if you wish to use the LEService with a service type unknown to the SDK you must add the valid data formats using the addValidDataFormat: method.
		/// </remarks>
		List<DataFormat> ValidDataFormats { get; }
		/// <summary>
		/// The latest received value from the service as an integer. If no valid data format is found to parse the data zero is returned, see <see cref="ValidDataFormats"/>.
		/// </summary>
		float ValueAsFloat { get; }
		/// <summary>
		/// The latest received value from the service as an integer. If no valid data format is found to parse the data zero is returned, see <see cref="ValidDataFormats"/>.
		/// </summary>
		int ValueAsInteger { get; }
		/// <summary>
		/// The latest received value from the service as raw data
		/// </summary>
		byte[] ValueData { get; }

		/// <summary>
		/// Add a delegate to receive service updates.
		/// </summary>
		/// <param name="serviceDelegate">The delegate to add</param>
		void AddDelegate(IServiceDelegate serviceDelegate);
		/// <summary>
		/// Remove a delegate
		/// </summary>
		/// <param name="serviceDelegate">The delegate to remove</param>
		void RemoveDelegate(IServiceDelegate serviceDelegate);

		/// <summary>
		/// Add a new valid data format.
		/// </summary>
		/// <param name="dataFormat">The data format to add.</param>
		void AddValidDataFormat(DataFormat dataFormat);
		/// <summary>
		/// Remove a valid data format
		/// </summary>
		/// <param name="dataFormat">The data format to remove</param>
		void RemoveValidDataFormat(DataFormat dataFormat);

		/// <summary>
		/// If the notifications is disabled for the service in the inputFormat through <see cref="Generic.InputFormat.NotificationsEnabled"/> you will have to use this method to request an updated value for the service. The updated value will be delivered through the delegate method <see cref="IServiceDelegate.DidUpdateInputFormat"/>.
		/// </summary>
		void SendReadValueRequest();
		/// <summary>
		/// This will send a reset command to the Device for this service. Some services has state, such as a bump-count for a tilt sensor that you may wish to reset.
		/// </summary>
		void SendResetStateRequest();

		/// <summary>
		/// Send an updated input format for this service to the device. If successful this will trigger an invocation of the delegate callback method <see cref="IServiceDelegate.DidUpdateInputFormat"/>.
		/// </summary>
		/// <param name="inputFormat"></param>
		void UpdateInputFormat(InputFormat inputFormat);

		/// <summary>
		/// Will send data to the IO backed by this service. Useful to write data to an output unknown to the SDK.
		/// </summary>
		/// <param name="data">The data to write.</param>
		void WriteData(byte[] data);

		/// <summary>
		/// Returns YES if this service is equal to otherService - two services are considered equal if their <see cref="ConnectInfo"/> are equal
		/// </summary>
		/// <param name="otherService">The service to be compared to the receiver.</param>
		/// <returns>True if equal</returns>
		bool Equals(IService otherService);

        /// <summary>
        /// Returns the passed data intepreted as a float. If no valid data format is found to parse the data zero is returned, see <see cref="ValidDataFormats"/>.
        /// </summary>
        /// <param name="data">The data to parse.</param>
        /// <returns>Data interpreted as a float</returns>
        float FloatFromData(byte[] data);
        /// <summary>
        /// Returns the passed data intepreted as an integer. If no valid data format is found to parse the data zero is returned, see <see cref="ValidDataFormats"/>.
        /// </summary>
        /// <param name="data">The data to parse.</param>
        /// <returns>Data interpreted as an integer</returns>
        int IntegerFromData(byte[] data);
	}
}
