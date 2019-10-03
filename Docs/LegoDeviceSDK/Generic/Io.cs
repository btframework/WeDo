using System.Collections.Generic;
using LegoDeviceSDK.Bluetooth;
using LegoDeviceSDK.Interfaces;

namespace LegoDeviceSDK.Generic {
	/// <summary>
	/// Abstract class that implements Add and RemoveDelegate
	/// </summary>
	internal abstract class Io : IIo {

		public abstract void ReadValueForConnectId(int connectId);

		public abstract void ResetStateForConnectId(int connectId);

		public abstract void WriteInputFormat(InputFormat inputFormat, int connectId);

		public abstract void ReadInputFormatForConnectId(int connectId);

		public abstract void WriteMotorPower(int power, int connectId);

        public abstract void WriteMotorPower(int power, int offset, int connectId);

		public abstract void WritePiezoToneFrequency(int frequency, int milliseconds, int connectId);

		public abstract void WritePiezoToneStop(int connectId);

		public abstract void WriteColor(int red, int green, int blue, int connectId);

        public abstract void WriteColorIndex(int index, int connectId);

		public abstract void WriteData(byte[] data, int connectId);

		protected List<BluetoothService> Delegates = new List<BluetoothService>();

		/// <summary>
		/// Add delegate to list
		/// </summary>
		/// <param name="ioDelegate">The delegate to add</param>
		public void AddDelegate(BluetoothService ioDelegate) {
			lock (Delegates) {
				Delegates.Add(ioDelegate);
			}
		}
		/// <summary>
		/// Remove delegate from list
		/// </summary>
		/// <param name="ioDelegate">The delegate to remove</param>
		public void RemoveDelegate(BluetoothService ioDelegate) {
			lock (Delegates) {
				Delegates.Remove(ioDelegate);
			}
		}
	
	}
}
