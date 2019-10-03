using System;
using System.Diagnostics;
using LegoDeviceSDK.Bluetooth;
using LegoDeviceSDK.Generic;
using LegoDeviceSDK.Interfaces;

namespace LegoDeviceSDK.Helpers {
	internal static class ProcessDataHelper {

		/// <summary>
		/// Handles a button pressed event
		/// </summary>
		/// <param name="data">The data from a button pressed notification</param>
		/// <param name="device">The Device the message came from</param>
		public static void GetButtonPressedEvent(byte[] data, IDevice device) {
			if (data.Length != 1)
				throw new Exception("Invalid Input");
			var pressed = data[0] == 1;
			if(device is BluetoothDevice)
				(device as BluetoothDevice).ButtonPressedUpdatedNotification(pressed);
		}

		/// <summary>
		/// Handles a Battery level updated notification
		/// </summary>
		/// <param name="data">The data from a Battery level updated notification</param>
		/// <param name="device">The Device the message came from</param>
		public static void GetBatteryLevelUpdatedEvent(byte[] data, IDevice device) {
			if (data.Length != 1)
				throw new Exception("Invalid Input");
			var batterylevel = (uint)data[0];
			if (device is BluetoothDevice)
				(device as BluetoothDevice).BatteryUpdatedNotification(batterylevel);
		}

		/// <summary>
		/// Handles new IO
		/// </summary>
		/// <param name="data">Data for new IO notification</param>
		/// <param name="device">The Device the message came from</param>
		public static void GetAttachedIoEvent(byte[] data, IDevice device) {
			GetAttachedIoEvent(data, device, null);
		}

		/// <summary>
		/// Handles new IO
		/// </summary>
		/// <param name="data">Data for new IO notification</param>
		/// <param name="device">The Device the message came from</param>
		/// <param name="io">The <see cref="IIo"/> that is used to get the Io notifications</param>
		public static void GetAttachedIoEvent(byte[] data, IDevice device, IIo io) {
			var bluetoothDevice = device as BluetoothDevice;
			if (bluetoothDevice != null) {
				bluetoothDevice.HandleNewIo(data, device, io);
			}
		}

		/// <summary>
		/// Process data from an IO notification
		/// </summary>
		/// <param name="data">The data to be processed</param>
		/// <param name="device">The device the notification came from</param>
		/// <param name="io">The <see cref="IIo"/> that is used to get the Io notifications</param>
		public static void ProcessAttachedIoEvent(byte[] data, IDevice device, IIo io) {
			bool isConnected = data[1] == 1;
			var connectInfo = ConnectInfo.FromByteArray(data);
			if (isConnected) {
				if (device is BluetoothDevice) {
					var service = BluetoothService.GetServiceFromConnectInfo(device, connectInfo, io);
					if(service != null)
						(device as BluetoothDevice).AttachedIoNotification(service);
				}
			}
			else {
				if (device is BluetoothDevice) {
					var service = device.Services.Find(service1 => service1.ConnectInfo.ConnectId == connectInfo.ConnectId);
					if (service != null)
						(device as BluetoothDevice).DetachedIoNotification(service);
				}
			}
		}

        /// <summary>
        /// Handles a low voltage alert event
        /// </summary>
        /// <param name="data">The data from a low voltage notification</param>
        /// <param name="device">The Device the message came from</param>
        public static void GetLowVoltageEvent(byte[] data, IDevice device) {
            if (data.Length != 1)
                throw new Exception("Invalid Input");
            var lowVoltage = data[0] == 1;
            if (device is BluetoothDevice)
                (device as BluetoothDevice).LowVoltageAlertUpdatedNotification(lowVoltage);
        }

	}
}
