using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration.Pnp;
using Windows.Storage.Streams;
using Windows.System.Threading;
using Windows.UI.Core;
using LegoDeviceSDK.Bluetooth.Definitions;
using LegoDeviceSDK.Generic;
using LegoDeviceSDK.Helpers;
using LegoDeviceSDK.Interfaces;
using LegoDeviceSDK.Wrapper;
using GattCharacteristic = LegoDeviceSDK.Wrapper.GattCharacteristic;

namespace LegoDeviceSDK.Bluetooth {


	/// <summary>
	/// A device represents the physical device / Hub. The device may have a number of services (inputs, motors, etc).
	/// The <see cref="IDeviceManager"/> can be used to scan for and connect to an <see cref="IDevice"/>.
	/// Implement the <see cref="IDeviceManagerDelegate"/> to be notified about changes to the service attributes, for instance when a sensor has a new reading.
	/// </summary>
	public class BluetoothDevice : IDevice {
		const string Tag = "BluetoothDevice";
		readonly BlockingCollection<Tuple<byte[], IDevice, IIo>> blockingCollection = new BlockingCollection<Tuple<byte[], IDevice, IIo>>();

		readonly BluetoothDeviceManager deviceManager;
		/// <summary>
		/// Battery level of the WeDo
		/// </summary>
		public uint BatteryLevel { get; private set; }
		/// <summary>
		/// Is the button pressed
		/// </summary>
		public bool ButtonPressed { get; private set; }
		/// <summary>
		/// The device ID of the device
		/// </summary>
		public string DeviceId { get; private set; }
		/// <summary>
		/// The <see cref="DeviceInfo"/> of the Device
		/// </summary>
		public DeviceInfo DeviceInfo { get; private set; }

		bool wasAllreadyConnected = false;
		DeviceState connectState = DeviceState.DeviceStateDisconnected;
		/// <summary>
		/// The <see cref="DeviceState"/> of the device
		/// </summary>
		public DeviceState ConnectState {
			get { return connectState; }
			internal set {
				// if nothing changed return
				if (connectState == value)
					return;
				connectState = value;
				SdkLogger.D(Tag, string.Format("Changed connect state to: {0} for {1}", connectState, Name));
				// check state and call appropiate callbacks
				if (connectState == DeviceState.DeviceStateConnecting) {
					deviceManager.WillStartConnectingToDevice(this);
				}
				else if (connectState == DeviceState.DeviceStateDisconnected) {
					ResetOnDisconnect();
					deviceManager.DidDisconnectFromDevice(this);
				}
				else if (connectState == DeviceState.DeviceStateInterrogating) {
					deviceManager.DidStartInterrogatingDevice(this);
				}
				else if (connectState == DeviceState.DeviceStateInterrogationFinished) {
					deviceManager.DidFinishInterrogatingDevice(this);
					wasAllreadyConnected = true;
					connectionTimeoutTimer.Dispose();
					CompleteConnection();
				}
			}
		}
	

		string name = "";
		string oldName = "";
		/// <summary>
		/// The Name of the Device
		/// </summary>
		public string Name {
			get { return name; }
			set {
                if (ConnectState != DeviceState.DeviceStateInterrogationFinished) {
                    SdkLogger.W(Tag, "Ignoring call to set device name - not connected to device");
                    return;
                }

				oldName = name;
				//name = value;
				UpdateName(value);
			}
		}

		/// <summary>
		/// Update the Name of the device
		/// </summary>
		/// <param name="newName">The new Device name</param>
		async void UpdateName(string newName) {
			// clanmp to 2- characters
			if (newName.Length > 20)
				newName = newName.Substring(0, 20);
			var data = System.Text.Encoding.UTF8.GetBytes(newName).ToList();
			// add null values at the end
			if (data.Count < 20) {
				var charactersMissing = 20 - newName.Length;
				for (int i = 0; i < charactersMissing; i++) {
					data.Add(0);
				}
			}
			// Write new name to characteristic
			var status = await BluetoothHelper.WriteCharacteristicAsync(data.ToArray(), nameCharacteristic, GattWriteOption.WriteWithResponse);
			if (status != GattCommunicationStatus.Success) {
				return;
			}
			// Read name again just to be sure
			await ReadNameFromDevice();
			SdkLogger.I(Tag, string.Format("Changed name from: {0} to {1}", oldName, Name));
		}

        /// <summary>
        /// Has a low voltage alert been received from the device indicating that batteries should be changed or charged?
        /// </summary>
        public bool LowVoltage { get; private set; }

		/// <summary>
		/// A list of external <see cref="IService"/> connected to the Device
		/// </summary>
		/// <returns>
		/// A list of external <see cref="IService"/> connected to the Device
		/// </returns>
		public List<IService> ExternalServices { get; private set; }
		/// <summary>
		/// A list of internal <see cref="IService"/> connected to the Device
		/// </summary>
		/// <returns>
		/// A list of internal <see cref="IService"/> connected to the Device
		/// </returns>
		public List<IService> InternalServices { get; private set; }
		/// <summary>
		/// A list of all <see cref="IService"/> (internal and external) connected to the Device
		/// </summary>
		/// <returns>
		/// A list of all <see cref="IService"/> (internal and external) connected to the Device
		/// </returns>
		public List<IService> Services { get; private set; }

		List<IDeviceDelegate> delegates = new List<IDeviceDelegate>();

		/// <summary>
		/// If a delegate is registered it receives callbacks on changes to offered <see cref="IService"/>, as well as properties of the device like <see cref="Name"/> and color. 
		/// </summary>
		/// <param name="deviceDelegate">The delegate to add</param>
		public void AddDelegate(IDeviceDelegate deviceDelegate) {
			SdkLogger.I(Tag, string.Format("Add Delegate: {0} to device: {1}", deviceDelegate.GetType(), Name));
			// lock is used, since the callbacks are called randomly from different threads
			lock (delegates) {
				delegates.Add(deviceDelegate);
			}
		}

		/// <summary>
		/// Remove delegate from this device
		/// </summary>
		/// <param name="deviceDelegate">The delegate to remove</param>
		public void RemoveDelegate(IDeviceDelegate deviceDelegate) {
			SdkLogger.I(Tag, string.Format("Remove Delegate: {0} from device: {1}", deviceDelegate.GetType(), Name));
			lock (delegates) {
				delegates.Remove(deviceDelegate);
			}
		}

		IDeviceInformation standartDevice;
		DeviceServiceDefinition DeviceServiceDefinition { get; set; }
		BluetoothCharacteristicDefinition nameCharacteristic { get; set; }
		BluetoothCharacteristicDefinition ioCharacteristic { get; set; }
		BluetoothCharacteristicDefinition buttonCharacteristic { get; set; }
		BluetoothCharacteristicDefinition disconnectCharacteristic { get; set; }
        BluetoothCharacteristicDefinition lowVoltageCharacteristic { get; set; }

		IoServiceDefinition IoServiceDefinition { get; set; }
		internal BluetoothIo BluetoothIo { get; private set; }

		DeviceInfoServiceDefinition DeviceInfoServiceDefinition { get; set; }
		BluetoothCharacteristicDefinition firmwareCharacteristic { get; set; }
		BluetoothCharacteristicDefinition hardwareCharacteristic { get; set; }
		BluetoothCharacteristicDefinition softwareCharacteristic { get; set; }
		BluetoothCharacteristicDefinition manufacturerCharacteristic { get; set; }

		BluetoothBatteryServiceDefinition BatteryServiceDefinition { get; set; }
		BluetoothCharacteristicDefinition batteryLevelCharacteristic { get; set; }

		private GattCharacteristic characteristic;
		private PnpObjectWatcher watcher;
		bool watcherStarted = false;
		private bool IsServiceInitialized { get; set; }

		// timers for timout and reconnect
		Timer connectionTimeoutTimer;

		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		CancellationToken cancellationToken;

		internal event DeviceConnectionUpdatedHandler DeviceConnectionUpdated;

		internal BluetoothDevice(string deviceId, IDeviceInformation standartDevice, BluetoothDeviceManager deviceManager) {
			// init members
			this.DeviceId = deviceId;
			this.standartDevice = standartDevice;
			this.deviceManager = deviceManager;
			// reset lists
			Services = new List<IService>();
			InternalServices = new List<IService>();
			ExternalServices = new List<IService>();
			ConnectState = DeviceState.DeviceStateDisconnected;

			// take standart device name before real device name is extracted
			if(standartDevice != null)
				name = standartDevice.Name;

			// init service definitions
			DeviceServiceDefinition = BluetoothServiceDefinition.DeviceServiceDefinition;
			IoServiceDefinition = BluetoothServiceDefinition.IoServiceDefinition;
			DeviceInfoServiceDefinition = BluetoothServiceDefinition.DeviceInfoServiceDefinition;
			BatteryServiceDefinition = BluetoothServiceDefinition.BatteryServiceDefinition;
		}

		/// <summary>
		/// Run HandleUpdatedData in background
		/// </summary>
		async void StartHandleUpdatedData() {
			try {
				await Task.Run(() => HandleUpdatedData(), cancellationToken);
			}
			catch {
			}
		}


		/// <summary>
		/// Disconnect disposes of all GattDevice services and resets wasAllreadyConnected so a reconnect works as expected
		/// </summary>
		internal async void Disconnect() {
			await UnsubscribeNotifications();
			if (disconnectCharacteristic != null)
				await BluetoothHelper.WriteCharacteristicAsync(new byte[0], disconnectCharacteristic, GattWriteOption.WriteWithResponse);
			Dispose();
		}

		async Task UnsubscribeNotifications() {
			if (DeviceServiceDefinition != null) {
				await BluetoothHelper.UnsubscribeNotification(buttonCharacteristic);
				await BluetoothHelper.UnsubscribeNotification(ioCharacteristic);
				await BluetoothHelper.UnsubscribeNotification(batteryLevelCharacteristic);
			}
            if (BluetoothIo != null) {
                await BluetoothIo.UnsubscribeNotifications();
            }
		}

		/// <summary>
		/// Disposes all services. Used to be able to disconnect without reconnecting
		/// </summary>
		void Dispose() {
			StopProcessingIo();
			wasAllreadyConnected = false;
			// call Dispose on all GattDeviceServices, so the WeDo will not automatically reconnect
			if (DeviceServiceDefinition != null && DeviceServiceDefinition.GattDeviceService != null) {
				DeviceServiceDefinition.GattDeviceService.Dispose();
				DeviceServiceDefinition = null;
			}
			if (IoServiceDefinition != null && IoServiceDefinition.GattDeviceService != null) {
				IoServiceDefinition.GattDeviceService.Dispose();
				IoServiceDefinition = null;
				BluetoothIo.IoServiceDefinition = null;
			}
			if (DeviceInfoServiceDefinition != null && DeviceInfoServiceDefinition.GattDeviceService != null) {
				DeviceInfoServiceDefinition.GattDeviceService.Dispose();
				DeviceInfoServiceDefinition = null;
			}
			if (BatteryServiceDefinition != null && BatteryServiceDefinition.GattDeviceService != null) {
				BatteryServiceDefinition.GattDeviceService.Dispose();
				BatteryServiceDefinition = null;
			}
		}


		internal async Task Connect() {
			cancellationTokenSource = new CancellationTokenSource();
			cancellationToken = cancellationTokenSource.Token;
			// some error handling
			if (standartDevice == null) {
				// start receiving data for test purposes
				StartHandleUpdatedData();

				return;
			}
			// if allready was connected, do not connect again. Will result in multiple notify subscriptions
			if (wasAllreadyConnected)
				return;

			SdkLogger.I(Tag, string.Format("Calling connect on device: {0}", Name));
			// start timer for connection timeout
			connectionTimeoutTimer = new Timer(state => {
				if (ConnectState != DeviceState.DeviceStateInterrogationFinished) {
					ConnectState = DeviceState.DeviceStateDisconnected;
					Dispose();
					deviceManager.DidFailToConnectToDevice(this);
				}
			}
				, null, deviceManager.ConnectRequestTimeoutInterval, Timeout.InfiniteTimeSpan);
			ConnectState = DeviceState.DeviceStateConnecting;

			// load all definitions
			await BluetoothServiceDefinition.LoadDefinitions(standartDevice);
			// get new BluetoothIo for this specific device
			BluetoothIo = BluetoothIo.GetBluetoothIo(BluetoothServiceDefinition.IoServiceDefinition);

			// get all characteristics from the services
			DeviceServiceDefinition = BluetoothServiceDefinition.DeviceServiceDefinition;
			nameCharacteristic = DeviceServiceDefinition.CharacteristicWithUuid(DeviceServiceDefinition.HubCharacteristicNameUuid);
			ioCharacteristic = DeviceServiceDefinition.CharacteristicWithUuid(DeviceServiceDefinition.HubCharacteristicAttachedIo);
			buttonCharacteristic = DeviceServiceDefinition.CharacteristicWithUuid(DeviceServiceDefinition.HubCharacteristicButtonState);
			disconnectCharacteristic = DeviceServiceDefinition.CharacteristicWithUuid(DeviceServiceDefinition.HubCharacteristicDisconnect);
            lowVoltageCharacteristic = DeviceServiceDefinition.CharacteristicWithUuid(DeviceServiceDefinition.HubCharacteristicLowVoltageAlert);

			IoServiceDefinition = BluetoothServiceDefinition.IoServiceDefinition;

			DeviceInfoServiceDefinition = BluetoothServiceDefinition.DeviceInfoServiceDefinition;
			firmwareCharacteristic = DeviceInfoServiceDefinition.CharacteristicWithUuid(DeviceInfoServiceDefinition.DeviceInfoFirmwareRevisionCharacteristicUuid);
			hardwareCharacteristic = DeviceInfoServiceDefinition.CharacteristicWithUuid(DeviceInfoServiceDefinition.DeviceInfoHardwareRevisionCharacteristicUuid);
			softwareCharacteristic = DeviceInfoServiceDefinition.CharacteristicWithUuid(DeviceInfoServiceDefinition.DeviceInfoSoftwareRevisionCharacteristicUuid);
			manufacturerCharacteristic = DeviceInfoServiceDefinition.CharacteristicWithUuid(DeviceInfoServiceDefinition.DeviceInfoManufacturerNameCharacteristicUuid);

			BatteryServiceDefinition = BluetoothServiceDefinition.BatteryServiceDefinition;
			batteryLevelCharacteristic = BatteryServiceDefinition.CharacteristicWithUuid(BluetoothBatteryServiceDefinition.BatteryLevelCharacteristicUuid);

			// start interogating device
			ConnectState = DeviceState.DeviceStateInterrogating;

			// start background thread that receives the attached IO notifications and handles them
			StartHandleUpdatedData();

			IsServiceInitialized = true;
			// configure all characteristics with notify. used to establish a connection to the device
			
			await ConfigureServiceForNotificationsAsync(buttonCharacteristic, ProcessDataHelper.GetButtonPressedEvent);

			await ConfigureServiceForNotificationsAsync(ioCharacteristic, ProcessDataHelper.GetAttachedIoEvent);

			await ConfigureServiceForNotificationsAsync(batteryLevelCharacteristic, ProcessDataHelper.GetBatteryLevelUpdatedEvent);

            await ConfigureServiceForNotificationsAsync(lowVoltageCharacteristic, ProcessDataHelper.GetLowVoltageEvent);

			await BluetoothIo.ConfigureNotifications();
		}

		/// <summary>
		/// When connected, read all values from the other sensors
		/// </summary>
		async void CompleteConnection() {
			// hack so all values are retrieved correctly before new reads are called
			await Task.Delay(500);

			await ReadNameFromDevice();
			await ReadFromBattery();
			await ReadDeviceInfo();
		}


		/// <summary>
		/// Reset values on disconnect
		/// </summary>
		void ResetOnDisconnect() {
			Services = new List<IService>();
			InternalServices = new List<IService>();
			ExternalServices = new List<IService>();
		}

		/// <summary>
		/// Reads all values that are part of the Deviceinfo instance
		/// </summary>
		/// <returns>Awaitable Task</returns>
		async Task ReadDeviceInfo() {
			var deviceInfo = new DeviceInfo();
			try {
				var gattReadResult = await BluetoothHelper.ReadCharacteristic(firmwareCharacteristic);
				if (gattReadResult != null && gattReadResult.Status == GattCommunicationStatus.Success) {
					deviceInfo.FirmwareRevision = Revision.FromString(gattReadResult.Value.ToArray());
				}
                if (hardwareCharacteristic != null) {
                    gattReadResult = await BluetoothHelper.ReadCharacteristic(hardwareCharacteristic);
                    if (gattReadResult != null && gattReadResult.Status == GattCommunicationStatus.Success)
                    {
                        deviceInfo.HardwareRevision = Revision.FromString(gattReadResult.Value.ToArray());
                    }
                }
				gattReadResult = await BluetoothHelper.ReadCharacteristic(softwareCharacteristic);
				if (gattReadResult != null && gattReadResult.Status == GattCommunicationStatus.Success) {
					deviceInfo.SoftwareRevision = Revision.FromString(gattReadResult.Value.ToArray());
				}
				gattReadResult = await BluetoothHelper.ReadCharacteristic(manufacturerCharacteristic);
				if (gattReadResult != null && gattReadResult.Status == GattCommunicationStatus.Success) {
					var nameArray = gattReadResult.Value.ToArray();
					deviceInfo.ManufacturerName = System.Text.Encoding.UTF8.GetString(nameArray, 0, nameArray.Length);
				}
				DeviceInfoUpdateNotification(deviceInfo);
			}
			catch { }
		}

		/// <summary>
		/// Reads battery value
		/// </summary>
		/// <returns>Awaitable Task</returns>
		async Task ReadFromBattery() {
			try {
				var batterylevelResult = await BluetoothHelper.ReadCharacteristic(batteryLevelCharacteristic);
				if (batterylevelResult != null && batterylevelResult.Status == GattCommunicationStatus.Success)
					ProcessDataHelper.GetBatteryLevelUpdatedEvent(batterylevelResult.Value.ToArray(), this);
			}
			catch (Exception) {}
		}

		/// <summary>
		/// Reads name from device
		/// </summary>
		/// <returns>Awaitable Task</returns>
		async Task ReadNameFromDevice() {
			try {
				var gattReadResult = await BluetoothHelper.ReadCharacteristic(nameCharacteristic);
				if (gattReadResult != null && gattReadResult.Status == GattCommunicationStatus.Success) {
					var nameArray = gattReadResult.Value.ToArray();
					name = System.Text.Encoding.UTF8.GetString(nameArray, 0, nameArray.Length).TrimEnd(' ');
				}
				if (oldName != Name) {
					lock (delegates) {
						foreach (var deviceDelegate in delegates) {
							deviceDelegate.DidChangeName(this, oldName, name);
						}
					}
				}
			}
			catch (Exception) {}
		}


		/// <summary>
		/// Returns YES if this device is equal to otherDevice
		/// </summary>
		/// <param name="other">The device to be compared to the receiver</param>
		/// <returns>True if devices are equal</returns>
		public bool Equals(IDevice other) {
			return DeviceId.Equals(other.DeviceId) &&
			       DeviceInfo == other.DeviceInfo &&
			       ConnectState == other.ConnectState &&
			       Name == other.Name;
		}


		/// <summary>
		/// Register to be notified when a connection is established to the Bluetooth device
		/// </summary>
		private void StartDeviceConnectionWatcher() {
			watcher = PnpObject.CreateWatcher(PnpObjectType.DeviceContainer,
					new string[] { "System.Devices.Connected" });

			watcher.Updated += DeviceConnection_Updated;
			watcher.Start();
			ConnectState = DeviceState.DeviceStateConnecting;
		}

		/// <summary>
		/// Invoked when a connection is established to the Bluetooth device
		/// </summary>
		/// <param name="sender">The watcher object that sent the notification</param>
		/// <param name="args">The updated device object properties</param>
		private void DeviceConnection_Updated(PnpObjectWatcher sender, PnpObjectUpdate args) {
			var connectedProperty = args.Properties["System.Devices.Connected"];
			var argId = args.Id.Replace("{", "").Replace("}", "");
			var containerId = standartDevice.Properties["System.Devices.ContainerId"].ToString();
			// check if this device is the device, that got the connection updated event
			if (!argId.Equals(containerId))
				return;


			bool isConnected = false;
			Boolean.TryParse(connectedProperty.ToString(), out isConnected);

			SdkLogger.I(Tag, string.Format("Device connection updated to {0} for device: {1}", isConnected ? "Connected" : "not Connected", Name));
			// set connect state to the correct one
			if (isConnected) 
				ConnectState = DeviceState.DeviceStateInterrogationFinished;
			else {
				ConnectState = DeviceState.DeviceStateDisconnected;
				Dispose();
			}
			// Notifying subscribers of connection state updates
			if (DeviceConnectionUpdated != null) {
				DeviceConnectionUpdated(isConnected);
			}
		}

		/// <summary>
		/// Configure the Bluetooth device to send notifications whenever the Characteristic value changes
		/// </summary>
		private async Task ConfigureServiceForNotificationsAsync(BluetoothCharacteristicDefinition characteristicDefinition, ValueChangeCompletedHandler valueChangeCompletedHandler) {
			if (!watcherStarted) {
				watcherStarted = true;
				// Register a PnpObjectWatcher to detect when a connection to the device is established,
				// such that the application can retry device configuration.
				StartDeviceConnectionWatcher();
			}

			try {
				// Obtain the characteristic for which notifications are to be received
				//characteristic = service.GetCharacteristics(CHARACTERISTIC_UUID)[CHARACTERISTIC_INDEX];
				var foo = characteristicDefinition.ServiceDefinition.GattDeviceService.GetCharacteristics(characteristicDefinition.Uuid);
				characteristic = foo[0];
				characteristic.RegisterValueChangedCallback();
				// While encryption is not required by all devices, if encryption is supported by the device,
				// it can be enabled by setting the ProtectionLevel property of the Characteristic object.
				// All subsequent operations on the characteristic will work over an encrypted link.
				//characteristic.ProtectionLevel = GattProtectionLevel.Plain;

				// Register the event handler for receiving notifications
				characteristic.ValueChanged += (sender, args) => {
					var data = new byte[args.CharacteristicValue.Length];

					DataReader.FromBuffer(args.CharacteristicValue).ReadBytes(data);
					if (valueChangeCompletedHandler != null) {
						valueChangeCompletedHandler(data, this);
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
						ConnectState = DeviceState.DeviceStateDisconnected;
						deviceManager.DidFailToConnectToDevice(this);
					}
					else {
						ConnectState = DeviceState.DeviceStateInterrogationFinished;
					}
				}
			}
			catch {
			}
		}

		/// <summary>
		/// Set button pressed and notify watchers
		/// </summary>
		/// <param name="buttonPressed">True if the button is pressed</param>
		internal void ButtonPressedUpdatedNotification(bool buttonPressed) {
			SdkLogger.I(Tag, string.Format("Button updated to: {0} on device: {1}", buttonPressed, Name));
			ButtonPressed = buttonPressed;
			lock (delegates) {
				foreach (var deviceDelegate in delegates) {
					deviceDelegate.DidChangeButtonState(this, buttonPressed);
				}
			}
		}

		/// <summary>
		/// Update with the new Battery level
		/// </summary>
		/// <param name="newLevel">The new battery level</param>
		internal void BatteryUpdatedNotification(uint newLevel) {
			SdkLogger.I(Tag, string.Format("Battery level updated to: {0} on device: {1}", newLevel, Name));
			BatteryLevel = newLevel;
			lock (delegates) {
				foreach (var deviceDelegate in delegates) {
					deviceDelegate.DidUpdateBatteryLevel(this, newLevel);
				}
			}
		}

        /// <summary>
        /// Set low voltage alert and notify watchers
        /// </summary>
        /// <param name="lowVoltage">True if the device sent a low voltage alert</param>
        internal void LowVoltageAlertUpdatedNotification(bool lowVoltage) {
            SdkLogger.I(Tag, string.Format("Low voltage alert updated to: {0} on device: {1}", lowVoltage, Name));
            LowVoltage = lowVoltage;
            lock (delegates) {
                foreach (var deviceDelegate in delegates) {
                    deviceDelegate.DidChangeLowVoltageState(this, lowVoltage);
                }
            }
        }

		/// <summary>
		/// Update with a new DeviceInfo
		/// </summary>
		/// <param name="info">The new DeviceInfo</param>
		internal void DeviceInfoUpdateNotification(DeviceInfo info) {
			DeviceInfo = info;
			lock (delegates) {
				foreach (var deviceDelegate in delegates) {
					deviceDelegate.DidUpdateDeviceInfo(this, info, null);
				}
			}
		}

		/// <summary>
		/// Puts new IO into a blocking collection for threadsafe access to new data
		/// </summary>
		/// <param name="data">The new IO as byte array</param>
		/// <param name="device">The device that received the new IO</param>
		/// <param name="io">The IO that received the new IO</param>
		internal async void HandleNewIo(byte[] data, IDevice device, IIo io) {
			try {
				// put into BlockingCollection
				var newIo = new Tuple<byte[], IDevice, IIo>(data, device, io);
				while (!blockingCollection.TryAdd(newIo, 1000, cancellationToken)) {
					await Task.Delay(10, cancellationToken);
				}
			}
			catch {
			}
		}

		/// <summary>
		/// Method to stop Processing Io for Unit test. should not be called otherwise
		/// </summary>
		internal void StopProcessingIo() {
			cancellationTokenSource.Cancel();
		}

		private void HandleUpdatedData() {
			try {
				// read from BlockingCollection and process input
				foreach (var data in blockingCollection.GetConsumingEnumerable(cancellationToken)) {
					ProcessDataHelper.ProcessAttachedIoEvent(data.Item1, data.Item2, data.Item3);
				}
			}
			catch {
			}
		}

		/// <summary>
		/// A new service is attached
		/// </summary>
		/// <param name="service">The new attached service</param>
		internal void AttachedIoNotification(IService service) {
			// find all services with the same connectId and remove those.
			var sameContactId = new List<IService>();
			lock (Services) {
				sameContactId = new List<IService>(Services.Where(service1 => service1.ConnectInfo.ConnectId == service.ConnectInfo.ConnectId));
			}
			foreach (var service1 in sameContactId) {
					DetachedIoNotification(service1);
			}

			SdkLogger.I(Tag, string.Format("Added service: {0} at: {1} on device: {2}", service.ConnectInfo.TypeString, service.ConnectInfo.ConnectId, Name));
			// add to lists
			lock (Services) {
				Services.Add(service);

				if (service.IsInternalService)
					InternalServices.Add(service);
				else
					ExternalServices.Add(service);
			}
			// notify watchers
			lock (delegates) {
				foreach (var deviceDelegate in delegates) {
					deviceDelegate.DidAddService(this, service);
				}
			}
			OrderServices();
		}

		/// <summary>
		/// A service was detached.
		/// </summary>
		/// <param name="service">The detached service</param>
		internal void DetachedIoNotification(IService service) {
			SdkLogger.I(Tag, string.Format("Removed service: {0} at: {1} on device: {2}", service.ConnectInfo.TypeString, service.ConnectInfo.ConnectId, Name));
			// find all services with the correct connectid
			var servicesToBeRemoved = new List<IService>();
			lock (Services) {
				servicesToBeRemoved = new List<IService>(Services.Where(service1 => service1.ConnectInfo.ConnectId == service.ConnectInfo.ConnectId));
				if (!servicesToBeRemoved.Any())
					return;
			}
			// remove each of those services
			foreach (var serviceToBeRemoved in servicesToBeRemoved) {
				lock (Services) {
					Services.Remove(serviceToBeRemoved);
					InternalServices.Remove(serviceToBeRemoved);
					ExternalServices.Remove(serviceToBeRemoved);
				}
				// notify watchers
				lock (delegates) {
					foreach (var deviceDelegate in delegates) {
						deviceDelegate.DidRemoveService(this, serviceToBeRemoved);
					}
				}
				// unsubscribe from BluetoothIo
				var bluetoothService = serviceToBeRemoved as BluetoothService;
				if (bluetoothService != null)
					bluetoothService.UnsubscribeFromBluetoothIo();
			}
		}

		/// <summary>
		/// Sorts Services by connectId
		/// </summary>
		void OrderServices() {
			lock (Services) {
				Services = Services.OrderBy(service => service.ConnectInfo.ConnectId).ToList();
				InternalServices = InternalServices.OrderBy(service => service.ConnectInfo.ConnectId).ToList();
				ExternalServices = ExternalServices.OrderBy(service => service.ConnectInfo.ConnectId).ToList();
			}
		}
	
	
	}
}
