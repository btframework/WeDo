using LegoDeviceSDK.Bluetooth;
using LegoDeviceSDK.Interfaces;

namespace LegoDeviceSDK.Generic {
	/// <summary>
	/// This class contains info detailing how the data received for a given service (typically a sensor of some kind) should be interpreted.
	///For senors types recognized by the SDK (like the <see cref="BluetoothTiltSensor"/> and <see cref="BluetoothMotionSensor"/>) you will not need to know about the details of this class as the implementation of these services in the SDK handles this for you.
	///If you need to access an Input that is not recognized by the SDK you should create and set one or more valid input formats for a <see cref="IService"/>. See the <see cref="BluetoothGenericService"/> documentation for an example.
	/// </summary>
	public class DataFormat {
		/// <summary>
		/// The data set count (fx. size 3 for a sensor that produce a value in x, y and z-direction)
		/// </summary>
		public uint DataSetCount { get; internal set; }
		/// <summary>
		/// The data set size - fx. 4 if the data is a four byte float
		/// </summary>
		public uint DataSetSize { get; internal set; }
		/// <summary>
		/// The sensor mode
		/// </summary>
		public uint Mode { get; internal set; }
		/// <summary>
		/// The name sensor mode (fx. crash, tilt or angle for a tilt sensor)
		/// </summary>
		public string ModeName { get; internal set; }
		/// <summary>
		/// The sensor unit
		/// </summary>
		public InputFormatUnit Unit { get; internal set; }
		/// <summary>
		/// The sensor unit name (Raw or SI)
		/// </summary>
		public string UnitName { get; internal set; }

		/// <summary>
		/// Create and initialize a new instance of an LEDataFormat.
		/// </summary>
		/// <param name="modeName">The name of the <see cref="Mode"/></param>
		/// <param name="modeValue">The sensor <see cref="Mode"/></param>
		/// <param name="unit">The sensor <see cref="Unit"/></param>
		/// <param name="sizeOfDataSet">The number of bytes in a data set</param>
		/// <param name="dataSetCount">The number of data sets</param>
		/// <returns>The new <see cref="DataFormat"/></returns>
		/// <remarks>
		/// Create and initialize a new instance of an <see cref="DataFormat"/>.
		///Example: When a <see cref="BluetoothTiltSensor"/> is in mode ‘angle’ it will create readings of in the x, y and z-dimension. If the mode is set to SI each angle will be a float representing with a value between 0 and 90 degrees. To create a data set that tells the SDK how to interpret values for til tilt sensor in this mode you would write.
		/// </remarks>
		public static DataFormat FormatWithModeName(string modeName, uint modeValue, InputFormatUnit unit, uint sizeOfDataSet, uint dataSetCount) {
			return new DataFormat() {ModeName = modeName, Mode = modeValue, Unit = unit, DataSetSize = sizeOfDataSet, DataSetCount = dataSetCount, };
		}

		/// <summary>
		/// Returns YES if this data format is equal to otherFormat
		/// </summary>
		/// <param name="otherFormat">The format to be compared to the receiver.</param>
		/// <returns>Returns YES if this data format is equal to otherFormat</returns>
		public bool IsEqualToFormat(DataFormat otherFormat) {
			return DataSetCount == otherFormat.DataSetCount &&
			       DataSetSize == otherFormat.DataSetSize &&
			       Mode == otherFormat.Mode &&
			       ModeName == otherFormat.ModeName &&
			       Unit == otherFormat.Unit &&
			       UnitName == otherFormat.UnitName;

		}
	}
}
