using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;
using Windows.System.Threading;
using LegoDeviceSDK.Bluetooth.Definitions;
using LegoDeviceSDK.Generic;
using LegoDeviceSDK.Wrapper;

namespace LegoDeviceSDK.Helpers {
	internal class BluetoothHelper {
		const string Tag = "BluetoothHelper";
		public enum Permissions {
			PERMISSION_READ = 1,                    // 1   - Characteristic read permission
			PERMISSION_READ_ENCRYPTED = 2,          // 2   - Allow encrypted read operations
			PERMISSION_READ_ENCRYPTED_MITM = 4,     // 4   - Allow reading with man-in-the-middle protection
			UNUSED = 8,                             // 8   - Unused
			PERMISSION_WRITE = 16,                  // 16  - Write permission
			PERMISSION_WRITE_ENCRYPTED = 32,        // 32  - Allow encrypted writes
			PERMISSION_WRITE_ENCRYPTED_MITM = 64,   // 64  - Allow encrypted writes with man-in-the-middle protection
			PERMISSION_WRITE_SIGNED = 128,          // 128 - Allow signed write operations
			PERMISSION_WRITE_SIGNED_MITM = 256,     // 256 - Allow signed write operations with man-in-the-middle protection
		}

		public enum Properties {
			PROPERTY_BROADCAST = 1,                 // 1   - Characteristic is broadcastable.
			PROPERTY_READ = 2,                      // 2   - Characteristic is readable.
			PROPERTY_WRITE_NO_RESPONSE = 4,         // 4   - Characteristic can be written without response.
			PROPERTY_WRITE = 8,                     // 8   - Characteristic can be written.
			PROPERTY_NOTIFY = 16,                   // 16  - Characteristic supports notification
			PROPERTY_INDICATE = 32,                 // 32  - Characteristic supports indication
			PROPERTY_SIGNED_WRITE = 64,             // 64  - Characteristic supports write with signature
			PROPERTY_EXTENDED_PROPS = 128,          // 128 - Characteristic has extended properties
		}

		internal delegate void ValueChangedHandler(byte[] data, BluetoothCharacteristicDefinition characteristic);

		private static readonly SemaphoreSlim MySemaphoreSlim = new SemaphoreSlim(1);

		internal static async Task UnsubscribeNotification(BluetoothCharacteristicDefinition characteristicDefinition) {
			var characteristic = characteristicDefinition.ServiceDefinition.GattDeviceService.GetCharacteristics(characteristicDefinition.Uuid)[0];
			var status = GattCommunicationStatus.Unreachable;
			try {
				status =
						await characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
						GattClientCharacteristicConfigurationDescriptorValue.None);

			}
			catch {
			}

		}

		/// <summary>
		/// Configure the Bluetooth device to send notifications whenever the Characteristic value changes
		/// </summary>
		internal static async Task ConfigureServiceForNotificationsAsync(BluetoothCharacteristicDefinition characteristicDefinition, ValueChangedHandler valueChangedHandler) {
			//await MySemaphoreSlim.WaitAsync();
			try
			{

			    await UnsubscribeNotification(characteristicDefinition);
				// Obtain the characteristic for which notifications are to be received
				var characteristic = characteristicDefinition.ServiceDefinition.GattDeviceService.GetCharacteristics(characteristicDefinition.Uuid)[0];
				characteristic.RegisterValueChangedCallback();
				// While encryption is not required by all devices, if encryption is supported by the device,
				// it can be enabled by setting the ProtectionLevel property of the Characteristic object.
				// All subsequent operations on the characteristic will work over an encrypted link.
				//characteristic.ProtectionLevel = GattProtectionLevel.EncryptionRequired;

				// Register the event handler for receiving notifications
				characteristic.ValueChanged += (sender, args) => {
					var data = new byte[args.CharacteristicValue.Length];
					args.CharacteristicValue.CopyTo(data);

					if (valueChangedHandler != null) {
						valueChangedHandler(data, characteristicDefinition);
					}

				};

				var status = GattCommunicationStatus.Unreachable;
				//try {
				//	status =
				//			await characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
				//			GattClientCharacteristicConfigurationDescriptorValue.Notify);

				//}
				//catch {
				//}
				//if (status != GattCommunicationStatus.Success) {

				// In order to avoid unnecessary communication with the device, determine if the device is already 
				// correctly configured to send notifications.
				// By default ReadClientCharacteristicConfigurationDescriptorAsync will attempt to get the current
				// value from the system cache and communication with the device is not typically required.
				var currentDescriptorValue = await characteristic.ReadClientCharacteristicConfigurationDescriptorAsync();

				if (currentDescriptorValue.Status != GattCommunicationStatus.Success || currentDescriptorValue.ClientCharacteristicConfigurationDescriptor != GattClientCharacteristicConfigurationDescriptorValue.Notify) {
					// Set the Client Characteristic Configuration Descriptor to enable the device to send notifications
					// when the Characteristic value changes
					status =
						await characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
						                                                                           GattClientCharacteristicConfigurationDescriptorValue.Notify);

					if (status == GattCommunicationStatus.Unreachable) {
						SdkLogger.E(Tag, string.Format("Subscribing to Notify property {0} failed with status Unreachable", characteristicDefinition.Name));
					}
				}
			}
			catch (Exception) {
				// handle exception
			}
		}

		/// <summary>
		/// Read a Value from a characteristic
		/// </summary>
		/// <param name="characteristicDefinition">The characteristic to be read</param>
		/// <returns>The read value from the characteristic</returns>
		internal static async Task<GattReadResult> ReadCharacteristic(BluetoothCharacteristicDefinition characteristicDefinition) {
			var characteristics = characteristicDefinition.ServiceDefinition.GattDeviceService.GetCharacteristics(characteristicDefinition.Uuid);
			if (characteristics.Count < 1)
				return null;
			var characteristic = characteristics[0];
			if (characteristic != null)
				return await characteristic.ReadValueAsync();

			return null;
		}

		/// <summary>
		/// Write a value to a characteristic
		/// </summary>
		/// <param name="data">The value to be wrtten</param>
		/// <param name="characteristicDefinition">The characteristic to be written to</param>
		/// <param name="writeOption">The write option (with or without response)</param>
		/// <returns>If the Write is successful or not</returns>
		internal static async Task<GattCommunicationStatus> WriteCharacteristicAsync(byte[] data, BluetoothCharacteristicDefinition characteristicDefinition, GattWriteOption writeOption = GattWriteOption.WriteWithoutResponse) {
			var characteristics = characteristicDefinition.ServiceDefinition.GattDeviceService.GetCharacteristics(characteristicDefinition.Uuid);
			if(characteristics.Count < 1)
				return GattCommunicationStatus.Unreachable;
			var characteristic = characteristics[0];

			var writer = new DataWriter();
			writer.WriteBytes(data);
			if (characteristic != null)
				return await characteristic.WriteValueAsync(writer.DetachBuffer(), writeOption);

			return GattCommunicationStatus.Unreachable;
		}


	}
}
