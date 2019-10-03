using LegoDeviceSDK.Bluetooth;

namespace LegoDeviceSDK.Generic {
	/// <summary>
	/// Represent the type of an attached IO (motor, sensor, etc).
	/// </summary>
	public enum IoType {
		/// <summary>
		/// A Motor - use the <see cref="BluetoothMotor"/> to communicate with this type of IO
		/// </summary>
		IoTypeMotor = 1,
		/// <summary>
		/// A Voltage Sensor - use the <see cref="BluetoothVoltageSensor"/> to communicate with this type of IO
		/// </summary>
		IoTypeVoltage = 20,
		/// <summary>
		/// A Current Sensor - use the <see cref="BluetoothCurrentSensor"/> to communicate with this type of IO
		/// </summary>
		IoTypeCurrent = 21,
		/// <summary>
		/// A Piezo Tone player - use the <see cref="BluetoothPiezoTonePlayer"/> to communicate with this type of IO
		/// </summary>
		IoTypePiezoTone = 22,
		/// <summary>
		/// An RGB light - use the <see cref="BluetoothRgbLight"/> to communicate with this type of IO
		/// </summary>
		IoTypeRgbLight = 23,
		/// <summary>
		/// A Tilt Sensor - use the <see cref="BluetoothTiltSensor"/> to communicate with this type of IO
		/// </summary>
		IoTypeTiltSensor = 34,
		/// <summary>
		/// A Motion Sensor (aka. Detect Sensor) - use the <see cref="BluetoothMotionSensor"/> to communicate with this type of IO
		/// </summary>
		IoTypeMotionSensor = 35,
		/// <summary>
		/// A type unknown to the SDK - use the <see cref="BluetoothGenericService"/> to communicate with this type of IO.
		/// </summary>
		IoTypeGeneric,
	}
}
