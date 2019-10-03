using LegoDeviceSDK.Generic;
using LegoDeviceSDK.Interfaces;

namespace LegoDeviceSDK.Bluetooth {
	/// <summary>
	/// The SDK will create instances of this class for IOs with an unknown LEIOType.
	/// The <see cref="BluetoothGenericService"/> is a ‘tagging’ interface and does not offer any extra methods or properties other than those available from its parent class <see cref="IService"/>.
	/// As opposed to the known service types (tilt, motion, etc) a generic service does not have a predefined<see cref="IService.DefaultInputFormat"/>. Therefore, when the SDK discovers a service with a type it does not recognize it does not automatically send an <see cref="InputFormat"/> to the <see cref="IDevice"/>. Without a configured input format the Device will not send any updates for the SDK when the value readings of the sensor value changes for that service.
	/// <code lang="C#">
	/// var temperaturSensor = service as BluetoothGenericService;
	/// if(temperaturSensor == null || service.ConnectInfo.Type != temperaturSensorTypeNumber)
	///		return;
	/// //The temperature sensor is yet unknown to the SDK (there is no
	/// //LETemperatureSensor class) so we need to configure
	/// //the service ourselves.
	///
	/// //As a generic sensor does not how a defaultInputFormat defined
	/// //we must create one and send it to the device.
	/// var inputFormat = InputFormat.InputFormatWithConnectId(
	///		connectId: temperaturSensor.ConnectInfo.ConnectId,
	///		typeId: temperaturSensor.ConnectInfo.Type,
	///		mode: 0, //See the documentation for the sensor for supported modes
	///		deltaInterval: 1, //Receive updates when the value changes with delta 1
	///		unit: InputFormatUnit.InputFormatUnitSi,
	///		notificationsEnabled: true);
	///
	/// //Tell the device to configure the sensor with the input format
	/// temperaturSensor.UpdateInputFormat(inputFormat);
	///
	/// //We know from the documentation that the temperature sensor produces readings in
	/// //Kelvin as 4 byte floats when in mode 0 with unit set to SI.
	/// var dataFormat = DataFormat.FormatWithModeName(
	///		modeName: "Kelvin",
	///		modeValue: 0, //must match the mode for the inputFormat
	///		unit: InputFormatUnit.InputFormatUnitSi,  //must match the unit for the inputFormat
	///		sizeOfDataSet: 4, //a 4 byte float
	///		dataSetCount: 1); //only one value in the data set (the temperature)
	///
	/// temperaturSensor.AddValidDataFormat(dataFormat);
	///
	/// //Now, add a delegate to receive notifications when the service has a new
	/// //temperature reading
	/// temperaturSensor.AddDelegate(this);
	/// </code>
	/// To receive IO value reading updates you must create and send an <see cref="InputFormat"/> to the device to be used for the service. You may look for inspiration on how to do this in one of the concrete service classes, like the <see cref="BluetoothMotionSensor"/> or <see cref="BluetoothVoltageSensor"/>. Below an example is given on how you could configure and use a new Temperature Sensor yet unknown to the SDK.
	/// <code>
	/// public void DidUpdateValueData(IService service, byte[] oldData, byte[] newData) {
	/// 	//As we have defined a valid data format stating that the received value can be parsed as a 4 byte
	/// 	//float we can use the convenience method to retrieve the value as a float.
	/// 	var temperatureReading = service.ValueAsFloat;
	/// }
	/// </code>
	/// Now, when the service receives an updated value from the temperature sensor you will receive a notification through the delegate.
	/// It is not required to add a valid data format to the <see cref="BluetoothGenericService"/>, but it is recommended to do so as this will also help SDK validate all received data according to the defined valid data formats and write any inconsistencies to the <see cref="SdkLogger"/>.
	/// </summary>
	public class BluetoothGenericService : BluetoothService {
		internal BluetoothGenericService(IDevice device, IIo io, ConnectInfo connectInfo) : base(device, io, connectInfo) {
			ServiceName = "Generic Service";
		}

	}
}
