using System;
using System.Collections.Generic;

namespace LegoDeviceSDK.Bluetooth {
	internal class BluetoothOutputCommand {

		const int HeaderSize = 3;
		const int WriteMotorPowerCommandId = 1;
		const int WritePlayPiezoToneCommandId = 2;
		const int WriteStopPiezoToneCommandId = 3;
		const int WriteRgbCommandId = 4;
		const int WriteDirectId = 5;

		byte[] payload;
		public byte[] Data { get { return payload; } }

		public BluetoothOutputCommand(int connectId, int commandId, byte[] data) {
			payload = new byte[HeaderSize + data.Length];
			payload[0] = (byte)connectId;
			payload[1] = (byte)commandId;
			payload[2] = (byte)data.Length;
			for (int i = 0; i < data.Length; i++) {
				payload[3 + i] = data[i];
			}

		}

		public static BluetoothOutputCommand WriteMotorPowerCommand(int motorPower, int connectId) {
			return new BluetoothOutputCommand(connectId, WriteMotorPowerCommandId, new byte[] {(byte) motorPower});
		}

		public static BluetoothOutputCommand WritePiezoToneFrequency(int frequency, int milliseconds, int connectId) {
			var data = new List<byte>(4);
			var frequencyBytes = BitConverter.GetBytes(frequency);
			data.Add(frequencyBytes[0]);
			data.Add(frequencyBytes[1]);
			var millisecondBytes= BitConverter.GetBytes(milliseconds);
			data.Add(millisecondBytes[0]);
			data.Add(millisecondBytes[1]);
			return new BluetoothOutputCommand(connectId, WritePlayPiezoToneCommandId, data.ToArray());
		}

		public static BluetoothOutputCommand WriteRgbLight(int red, int green, int blue, int connectId) {
			var data = new List<byte>(3);
			data.Add((byte)red);
			data.Add((byte)green);
			data.Add((byte)blue);
			return new BluetoothOutputCommand(connectId, WriteRgbCommandId, data.ToArray());
		}

        public static BluetoothOutputCommand WriteRgbLightIndex(int index, int connectId) {
            var data = new List<byte>(1);
            data.Add((byte)index);
            return new BluetoothOutputCommand(connectId, WriteRgbCommandId, data.ToArray());
        }

		public static BluetoothOutputCommand CommandWithDirectWriteThroughData(byte[] data, int connectId) {
			return new BluetoothOutputCommand(connectId, WriteDirectId, data);
		}

		public static BluetoothOutputCommand WritePiezoToneStop(int connectId) {
			return new BluetoothOutputCommand(connectId, WriteStopPiezoToneCommandId, new byte[0]);
		}

		public static BluetoothOutputCommand CommandWithConnectId(int connectId, int commandId, byte[] data) {
			return new BluetoothOutputCommand(connectId, commandId, data);
		}

	}
}
