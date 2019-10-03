using LegoDeviceSDK.Generic;

namespace LegoDeviceSDK.Bluetooth {
	internal class BluetoothInputCommand {

		byte[] payload;
		public byte[] Data { get { return payload; } }

		private BluetoothInputCommand(int commandId, int commandType, int connectId, byte[] data) {
			payload = new byte[3 + data.Length];
			payload[0] = (byte)commandId;
			payload[1] = (byte)commandType;
			payload[2] = (byte)connectId;
			for (int i = 0; i < data.Length; i++) {
				payload[3 + i] = data[i];
			}
		}

		public static BluetoothInputCommand WriteInputFormat(InputFormat inputFormat, int connectId) {
			return new BluetoothInputCommand(1, 2, connectId, InputFormat.InputFormatToByteArray(inputFormat));
		}

		public static BluetoothInputCommand ReadInputFormatForConnectId(int connectId) {
			return new BluetoothInputCommand(1, 1, connectId, new byte[0]);
		}

		public static BluetoothInputCommand ReadValueForConnectId(int connectId) {
			return new BluetoothInputCommand(0, 1, connectId, new byte[0]);
		}

	}
}
