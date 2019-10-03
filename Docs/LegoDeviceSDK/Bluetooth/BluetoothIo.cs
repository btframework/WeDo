using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using LegoDeviceSDK.Bluetooth.Definitions;
using LegoDeviceSDK.Generic;
using LegoDeviceSDK.Helpers;

namespace LegoDeviceSDK.Bluetooth {
	internal class BluetoothIo : Io {
		const string Tag = "BluetoothIo";
		// BlockingCollection is the Queue like datastructure for thread safe access to new received data
		readonly BlockingCollection<Tuple<byte[], BluetoothCharacteristicDefinition>> blockingCollection = new BlockingCollection<Tuple<byte[], BluetoothCharacteristicDefinition>>();

		internal IoServiceDefinition IoServiceDefinition { get; set; }
		BluetoothCharacteristicDefinition InputValueCharacteristic { get; set; }
		BluetoothCharacteristicDefinition InputFormatCharacteristic { get; set; }
		BluetoothCharacteristicDefinition InputCommandCharacteristic { get; set; }
		BluetoothCharacteristicDefinition OutputCommandCharacteristic { get; set; }

		Dictionary<int, InputFormat> inputFormats = new Dictionary<int, InputFormat>();


		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		CancellationToken cancellationToken;

		// when a input format is missing (value received does not have a valid input format) this dictionary sets it's value to true to signal that a request for a new input format was received
		Dictionary<int, bool> missingInputFormatDictionary = new Dictionary<int, bool>(); 

		internal BluetoothIo() {
			cancellationToken = cancellationTokenSource.Token;
		}

		internal void Dispose() {
			cancellationTokenSource.Cancel();
		}

		internal static BluetoothIo GetBluetoothIo(IoServiceDefinition ioServiceDefinition) {
			var bluetoothIo = new BluetoothIo {
				IoServiceDefinition = ioServiceDefinition,
				InputValueCharacteristic = ioServiceDefinition.CharacteristicWithUuid(IoServiceDefinition.CharacteristicInputValueUuid),
				InputFormatCharacteristic = ioServiceDefinition.CharacteristicWithUuid(IoServiceDefinition.CharacteristicInputFormatUuid),
				InputCommandCharacteristic = ioServiceDefinition.CharacteristicWithUuid(IoServiceDefinition.CharacteristicInputCommandUuid),
				OutputCommandCharacteristic = ioServiceDefinition.CharacteristicWithUuid(IoServiceDefinition.CharacteristicOutputCommandUuid)
			};
			if (bluetoothIo.InputValueCharacteristic == null || bluetoothIo.InputValueCharacteristic == null || bluetoothIo.InputValueCharacteristic == null || bluetoothIo.InputValueCharacteristic == null) {
				SdkLogger.E(Tag, "IOService missing mandatory characteristic");
				return null;
			}
			return bluetoothIo;
		}

		/// <summary>
		/// Subscribes to all Notify characteristics
		/// </summary>
		/// <returns>Awaitable Task</returns>
		internal async Task ConfigureNotifications() {
			StartHandleUpdatedData();
			await BluetoothHelper.ConfigureServiceForNotificationsAsync(InputValueCharacteristic, HandleUpdatedInputServiceCharacteristic);
			await BluetoothHelper.ConfigureServiceForNotificationsAsync(InputFormatCharacteristic, HandleUpdatedInputServiceCharacteristic);
			
		}

		internal async Task UnsubscribeNotifications() {
			if (IoServiceDefinition != null) {
				await BluetoothHelper.UnsubscribeNotification(InputValueCharacteristic);
				await BluetoothHelper.UnsubscribeNotification(InputFormatCharacteristic);
			}
		}

		async void StartHandleUpdatedData() {
			await Task.Run(() => HandleUpdatedData(), cancellationToken);
			
		}
        
		#region AccessInputs
		public override void ReadValueForConnectId(int connectId) {
			WriteInputCommand(BluetoothInputCommand.ReadValueForConnectId(connectId));
		}

		public override void ResetStateForConnectId(int connectId) {
			//Byte sequence sent to sensor to reset any state (for instance, crash-count for tilt sensor)
			var resetBytes = new byte[] { (byte)68, (byte)17, (byte)170 };
			WriteData(resetBytes, connectId);
		}

		public override void WriteInputFormat(InputFormat inputFormat, int connectId) {
			WriteInputCommand(BluetoothInputCommand.WriteInputFormat(inputFormat, connectId));
		}

		public override void ReadInputFormatForConnectId(int connectId) {
			WriteInputCommand(BluetoothInputCommand.ReadInputFormatForConnectId(connectId));
		}

		public async void WriteInputCommand(BluetoothInputCommand command) {
			SdkLogger.I(Tag, string.Format("Writing Input Command: {0}", BitConverter.ToString(command.Data)));
			var status = await BluetoothHelper.WriteCharacteristicAsync(command.Data, InputCommandCharacteristic);
			if (status != GattCommunicationStatus.Success)
				SdkLogger.E(Tag, "Error writing input command");
		}
		#endregion

		#region AccessOutputs
		public override void WriteMotorPower(int power, int connectId) {
            WriteMotorPower(power, 0, connectId);
		}

        public override void WriteMotorPower(int power, int offset, int connectId)
        {
            bool isPositive = power >= 0;
            power = Math.Abs(power);

            float actualPower = ((100.0f - offset) / 100.0f) * power + offset;
            int actualResultInt = (int)Math.Round(actualPower);

            if (!isPositive) {
                actualResultInt = -actualResultInt;
            }

            var command = BluetoothOutputCommand.WriteMotorPowerCommand(actualResultInt, connectId);
            WriteOutputCommand(command);
            SdkLogger.I(Tag, string.Format("Writing Motor Power: {0}", BitConverter.ToString(command.Data)));
        }

		public override void WritePiezoToneFrequency(int frequency, int milliseconds, int connectId) {
			var command = BluetoothOutputCommand.WritePiezoToneFrequency(frequency, milliseconds, connectId);
			WriteOutputCommand(command);
			SdkLogger.I(Tag, string.Format("Writing Piezo Tone Frequency: {0}", BitConverter.ToString(command.Data)));
		}

		public override void WritePiezoToneStop(int connectId) {
			var command = BluetoothOutputCommand.WritePiezoToneStop(connectId);
			WriteOutputCommand(command);
			SdkLogger.I(Tag, string.Format("Writing Piezo Tone stop: {0}", BitConverter.ToString(command.Data)));
		}

		public override void WriteColor(int red, int green, int blue, int connectId) {
			var command = BluetoothOutputCommand.WriteRgbLight(red, green, blue, connectId);
			WriteOutputCommand(command);
			SdkLogger.I(Tag, string.Format("Writing color: {0}", BitConverter.ToString(command.Data)));
		}

        public override void WriteColorIndex(int index, int connectId) {
            var command = BluetoothOutputCommand.WriteRgbLightIndex(index, connectId);
            WriteOutputCommand(command);
            SdkLogger.I(Tag, string.Format("Writing color index: {0}", BitConverter.ToString(command.Data)));
        }

		public override void WriteData(byte[] data, int connectId) {
			var command = BluetoothOutputCommand.CommandWithDirectWriteThroughData(data, connectId);
			WriteOutputCommand(command);
			SdkLogger.I(Tag, string.Format("Writing Data: {0}", BitConverter.ToString(command.Data)));
		}

		public virtual async void WriteOutputCommand(BluetoothOutputCommand outputCommand) {
			await BluetoothHelper.WriteCharacteristicAsync(outputCommand.Data, OutputCommandCharacteristic);
		}

		public void HandleUpdatedInputServiceCharacteristic(byte[] data, BluetoothCharacteristicDefinition characteristic) {
			//SdkLogger.V("Get Notifications", "Notification received: " + BitConverter.ToString(data));
			// put into BlockingCollection
			//while (!blockingCollection.TryAdd(new Tuple<byte[], BluetoothCharacteristicDefinition>(data, characteristic), 1000, cancellationToken)) {
			//	await Task.Delay(10, cancellationToken);
			//}
			lock (blockingCollection) {
				blockingCollection.Add(new Tuple<byte[], BluetoothCharacteristicDefinition>(data, characteristic), cancellationToken);
			}
		}

		/// <summary>
		/// Handles new data from the Bluetooth Device. handles both normal values and new Input Formats
		/// </summary>
		private void HandleUpdatedData() {
			try {
				// read from BlockingCollection and process input
				foreach (var data in blockingCollection.GetConsumingEnumerable(cancellationToken)) {
					if (IoServiceDefinition.InputFormat.MatchesCharacteristic(data.Item2)) {
						HandleUpdatedInputFormatData(data.Item1);
					}
					else if (IoServiceDefinition.InputValue.MatchesCharacteristic(data.Item2)) {
						HandleUpdatedInputValue(data.Item1);
					}
				}
			}
			catch {
			}
		}

		/// <summary>
		/// Handles new input format
		/// </summary>
		/// <param name="data">The new Input Format as byte array</param>
		void HandleUpdatedInputFormatData(byte[] data) {
			var format = InputFormat.InputFormatFromByteArray(data);
			if (format != null) {
				SdkLogger.I(Tag, string.Format("Did receive Input Format: {0}", format));
				//If we already have input format with an earlier revision, delete all those
        //as all known formats must have the same version
				lock (inputFormats) {
					InputFormat anyFormat = inputFormats.Values.FirstOrDefault();
					// clear if revisions are not equal
					if (anyFormat != null && anyFormat.Revision != format.Revision) {
						inputFormats.Clear();
					}
					// update input formats in local list
					if (inputFormats.ContainsKey(format.ConnectId))
						inputFormats[format.ConnectId] = format;
					else {
						inputFormats.Add(format.ConnectId, format);
					}
					if (missingInputFormatDictionary.ContainsKey(format.ConnectId)) {
						missingInputFormatDictionary[format.ConnectId] = false;
					}
					else {
						missingInputFormatDictionary.Add(format.ConnectId, false);
					}
				}
				// notify all delegates that are involved about the new Input Format
				lock (Delegates) {
					foreach (var ioDelegate in Delegates) {
						if(ioDelegate.ConnectInfo.ConnectId == format.ConnectId)
							ioDelegate.DidReceiveInputFormat(this, format);
					}
				}
			}
		}

		/// <summary>
		/// Handles the new Input value
		/// </summary>
		/// <param name="data">The new Value as a byte array</param>
		void HandleUpdatedInputValue(byte[] data) {
			int valueFormatRevision = data[0];
			bool hasMoreValues = true;
			int byteIndex = 1;
			var idToValue = new Dictionary<int, byte[]>();
			var bytesList = data.ToList();
			// iterate over values in byte array until byte array is empty
			while (hasMoreValues) {
				int valueConnectId = bytesList[byteIndex];
				InputFormat inputFormat;
				// if value has connectId that is not known by the system ignore
				if (!inputFormats.TryGetValue(valueConnectId, out inputFormat)) {
					HandleMissingInputFormat(valueConnectId);
					SdkLogger.E(Tag, string.Format("Cannot parse value - has not yet received any Input Format from device with connectId: {0}", valueConnectId));
					return;
				}
				// if no input format is available for this connectId ignore
				if (inputFormat == null) {
					SdkLogger.E(Tag, string.Format("No known Input Format for input with Connect ID {0}", valueConnectId));
					return;
				}
				// if the revision from the input value is different than the revision from the input format ignore
				if (inputFormat.Revision != valueFormatRevision) {
					SdkLogger.E(Tag, string.Format("Format revision {0} in received value does not match last recieved Input Format revision {1}", valueFormatRevision, inputFormat.Revision));
					return;
				}
				byteIndex++;
				// read value
				var value = bytesList.GetRange(byteIndex, inputFormat.NumberOfBytes).ToArray();
				byteIndex += inputFormat.NumberOfBytes;
				idToValue.Add(valueConnectId, value);

				// check for more values
				if (byteIndex >= data.Length)
					hasMoreValues = false;
			}
			// notify delegates
			lock (Delegates) {
				foreach (var ioDelegate in Delegates) {
					var info = ioDelegate.DidRequestConnectInfo(this);
					byte[] value;
					if (idToValue.TryGetValue(info.ConnectId, out value)) {
						ioDelegate.DidReceiveValueData(this, value);
					}
				}
			}
		}

		/// <summary>
		/// When a Input format is missing, this method requests another. uses dictionary so the device is not spammed with input format requests.
		/// </summary>
		/// <param name="valueConnectId"></param>
		void HandleMissingInputFormat(int valueConnectId) {
			if (!missingInputFormatDictionary.ContainsKey(valueConnectId)) {
				missingInputFormatDictionary.Add(valueConnectId, false);
			}
			var inputFormatRequested = missingInputFormatDictionary[valueConnectId];
			if (!inputFormatRequested) {
				ReadInputFormatForConnectId(valueConnectId);
				inputFormatRequested = true;
			}
			missingInputFormatDictionary[valueConnectId] = inputFormatRequested;
		}
		#endregion


	}
}
