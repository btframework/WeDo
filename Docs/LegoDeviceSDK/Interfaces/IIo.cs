using LegoDeviceSDK.Bluetooth;
using LegoDeviceSDK.Generic;

namespace LegoDeviceSDK.Interfaces {
	internal interface IIo {

		/// <summary>
		/// Forces a read value for a specific connect ID
		/// </summary>
		/// <param name="connectId">The connect id to read a value from</param>
		void ReadValueForConnectId(int connectId);

		/// <summary>
		/// Resets a state for a connect ID. is used to reset crash on a motion or tilt sensor
		/// </summary>
		/// <param name="connectId">The connect id to reset a value from</param>
		void ResetStateForConnectId(int connectId);

		/// <summary>
		/// Writes an input format to a service with the connectId specified
		/// </summary>
		/// <param name="inputFormat">The input format to write</param>
		/// <param name="connectId">The connect Id to write to</param>
		void WriteInputFormat(InputFormat inputFormat, int connectId);

		/// <summary>
		/// Read the input format from a specific connect Id. Forces a notify from the device
		/// </summary>
		/// <param name="connectId">The connect Id to read the input format from</param>
		void ReadInputFormatForConnectId(int connectId);

	
		/// <summary>
		/// Write Motor power to <see cref="BluetoothMotor"/>
		/// </summary>
		/// <param name="power">Amount of power</param>
		/// <param name="connectId">The connect Id of the motor</param>
		void WriteMotorPower(int power, int connectId);

        /// <summary>
        /// Write Motor power to <see cref="BluetoothMotor"/>
        /// </summary>
        /// <param name="power">Amount of power</param>
        /// <param name="offset">Amount of power offset</param>
        /// <param name="connectId">The connect Id of the motor</param>
        void WriteMotorPower(int power, int offset, int connectId);

		/// <summary>
		/// Write Piezo tone frequency and duration
		/// </summary>
		/// <param name="frequency">The frequency to play</param>
		/// <param name="milliseconds">The duration to play the tone</param>
		/// <param name="connectId">The connect id to write a value from</param>
		void WritePiezoToneFrequency(int frequency, int milliseconds, int connectId);

		/// <summary>
		/// Stop playing a sound.
		/// </summary>
		/// <param name="connectId">The connect id to write a value from</param>
		void WritePiezoToneStop(int connectId);

		/// <summary>
		/// Write a color to the <see cref="BluetoothRgbLight"/>
		/// </summary>
		/// <param name="red">Amount of red</param>
		/// <param name="green">Amount of green</param>
		/// <param name="blue">Amount of blue</param>
		/// <param name="connectId">The connect id to write a value from</param>
		void WriteColor(int red, int green, int blue, int connectId);

        /// <summary>
        /// Write a color index to the <see cref="BluetoothRgbLight"/>
        /// </summary>
        /// <param name="index">Index of the color</param>
        /// <param name="connectId">The connect id to write a value from</param>
        void WriteColorIndex(int index, int connectId);

		/// <summary>
		/// Writes raw data to the service
		/// </summary>
		/// <param name="data">The data to write</param>
		/// <param name="connectId">The connectId of the </param>
		void WriteData(byte[] data, int connectId);

		/// <summary>
		/// Add delegate
		/// </summary>
		/// <param name="ioDelegate">The delegate to add</param>
		void AddDelegate(BluetoothService ioDelegate);
		/// <summary>
		/// Remove delegate
		/// </summary>
		/// <param name="ioDelegate">The delegate to remove</param>
		void RemoveDelegate(BluetoothService ioDelegate);

	}
}
