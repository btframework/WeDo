using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LegoDeviceSDK.Bluetooth.Definitions;
using LegoDeviceSDK.Generic;
using LegoDeviceSDK.Helpers;
using LegoDeviceSDK.Interfaces;
using LegoDeviceSDK.Wrapper;

namespace LegoDeviceSDK.Bluetooth {

	/// <summary>
	/// This class is the main entry point for connecting and communicating with a LEGO Device. You must implement the <see cref="IDeviceManagerDelegate"/> protocol and set the delegate property before scanning for and connecting to devices.
	/// </summary>
	public class BluetoothDeviceManager : IDeviceManager {
		const string Tag = "BluetoothDeviceManager";
		private static volatile BluetoothDeviceManager instance;
		private static object syncRoot = new Object();
		/// <summary>
		/// The shared LEDeviceManager
		/// </summary>
		/// <returns>The shared <see cref="BluetoothDeviceManager"/></returns>
		public static BluetoothDeviceManager SharedDeviceManager {
			get {
				if (instance == null) {
					lock (syncRoot) {
						if (instance == null)
							instance = new BluetoothDeviceManager();
					}
				}
				return instance;
			}
		}

		/// <summary>
		/// Returns a list with all known devices regardless of their current connect state.
		/// </summary>
		public List<IDevice> AllDevices { get; private set; }
		/// <summary>
		/// If enabled, the <see cref="IDeviceManager"/> will attempt to reconnect in case of a connection loss, but only if the connection was not closed by the user, the default value is NO.
		/// </summary>
		public bool AutomaticReconnectOnConnectionLostEnabled { get; set; }
		/// <summary>
		/// If a connect request is not successful within this time interval the connection attempt is cancelled and the <see cref="IDeviceManagerDelegate.DidFailToConnectToDevice"/> is invoked. The default value is 10 seconds.
		/// </summary>
		public TimeSpan ConnectRequestTimeoutInterval { get; set; }


		private readonly List<IDeviceManagerDelegate> devicemanagerDelegates = new List<IDeviceManagerDelegate>(); 

		DeviceServiceDefinition DeviceServiceDefinition { get; set; }
		IoServiceDefinition IoServiceDefinition { get; set; }

		internal BluetoothDeviceManager() {
			AllDevices = new List<IDevice>();
			DeviceServiceDefinition = BluetoothServiceDefinition.DeviceServiceDefinition;
			IoServiceDefinition = BluetoothServiceDefinition.IoServiceDefinition;
		}

		/// <summary>
		/// Start scanning for LEGO devices
		/// </summary>
		/// <returns>Task that can be awaited</returns>
		public async Task ScanAsync() {
			// get all devices
			var deviceSelector = GattConfiguration.GattDeviceServiceWrapper.GetDeviceSelectorFromUuid(GuidHelper.GetFullGuid(DeviceServiceDefinition.HubService16BitUuid));
			var devices = await GattConfiguration.DeviceInformationWrapper.FindAllAsync(
					deviceSelector,
					new string[] { "System.Devices.ContainerId" });
			// merge device list and notify for new devices discovered
			foreach (var device in devices) {
				// extract device Id
				var deviceId = ExtractRealDeviceId(device.Id);
				// check if device is already present. If not add and notify watchers
				if (!string.IsNullOrEmpty(deviceId)) {
					if (AllDevices.Find(device1 => device1.DeviceId == deviceId) == null) {
						var newDevice = new BluetoothDevice(deviceId, device, this);
						AllDevices.Add(newDevice);
						SdkLogger.I(Tag, string.Format("Adding new Prepherial: {0}", newDevice.Name));
						lock (devicemanagerDelegates) {
							foreach (var devicemanagerDelegate in devicemanagerDelegates) {
								devicemanagerDelegate.DeviceDidAppear(this, newDevice);
							}
						}
					}
				}
			}
			// remove unused devices
			var toDelete = new List<IDevice>();
			foreach (var allDevice in AllDevices) {
				if (devices.Find(information => ExtractRealDeviceId(information.Id) == allDevice.DeviceId) == null) {
					toDelete.Add(allDevice);
				}
			}
			foreach (var leDevice in toDelete) {
				AllDevices.Remove(leDevice);
				SdkLogger.I(Tag, string.Format("Removing Prepherial: {0}", leDevice.Name));
				lock (devicemanagerDelegates) {
					foreach (var devicemanagerDelegate in devicemanagerDelegates) {
						devicemanagerDelegate.DeviceDidDisappear(this, leDevice);
					}
				}
			}
		}

		private string ExtractRealDeviceId(string longDeviceId) {
			var tokens = longDeviceId.Split(new string[]{"#"}, StringSplitOptions.None);
			if (tokens.Length != 4)
				return "";
			var tokens2 = tokens[1].Split(new string[] {"_"}, StringSplitOptions.None);
			if (tokens2.Length == 2)
				return tokens2[1];

			return "";
		}

		/// <summary>
		/// Connect to a LEGO LEDevice. If a connection is not established within the connectRequestTimeoutInterval the connection attempt is cancelled and the <see cref="IDeviceManagerDelegate.DidFailToConnectToDevice"/> is invoked
		/// </summary>
		/// <param name="device">The device to establish a connection to</param>
		/// <returns>Task that can be awaited</returns>
		public async Task ConnectToDeviceAsync(IDevice device) {
			if(device is BluetoothDevice)
				await (device as BluetoothDevice).Connect();
		}

		/// <summary>
		/// Add a delegate to receive device discovery and connection events
		/// </summary>
		/// <param name="deviceManagerDelegate">Add a delegate to receive device discovery and connection events</param>
		public void AddDelegate(IDeviceManagerDelegate deviceManagerDelegate) {
			lock (devicemanagerDelegates) {
				devicemanagerDelegates.Add(deviceManagerDelegate);
			}
		}

		/// <summary>
		/// Remove a delegate
		/// </summary>
		/// <param name="deviceManagerDelegate">The delegate to remove</param>
		public void RemoveDelegate(IDeviceManagerDelegate deviceManagerDelegate) {
			lock (devicemanagerDelegates) {
				devicemanagerDelegates.Remove(deviceManagerDelegate);
			}
		}

		/// <summary>
		/// Disconnect from a LEGO Device
		/// </summary>
		/// <param name="device">The device to disconnect from</param>
		public void CancelDeviceConnection(IDevice device) {
			if(device is BluetoothDevice)
				(device as BluetoothDevice).Disconnect();
		}

		internal void DidDisconnectFromDevice(IDevice device) {
			lock (devicemanagerDelegates) {
				foreach (var devicemanagerDelegate in devicemanagerDelegates) {
					devicemanagerDelegate.DidDisconnectFromDevice(this, device, AutomaticReconnectOnConnectionLostEnabled);
				}
			}
		}

		internal void DidFailToConnectToDevice(IDevice device) {
			lock (devicemanagerDelegates) {
				foreach (var devicemanagerDelegate in devicemanagerDelegates) {
					devicemanagerDelegate.DidFailToConnectToDevice(this, device, AutomaticReconnectOnConnectionLostEnabled);
				}
			}
		}

		internal void DidFinishInterrogatingDevice(IDevice device) {
			lock (devicemanagerDelegates) {
				foreach (var devicemanagerDelegate in devicemanagerDelegates) {
					devicemanagerDelegate.DidFinishInterrogatingDevice(this, device);
				}
			}
		}

		internal void DidStartInterrogatingDevice(IDevice device) {
			lock (devicemanagerDelegates) {
				foreach (var devicemanagerDelegate in devicemanagerDelegates) {
					devicemanagerDelegate.DidStartInterrogatingDevice(this, device);
				}
			}
		}

		internal void WillStartConnectingToDevice(IDevice device) {
			lock (devicemanagerDelegates) {
				foreach (var devicemanagerDelegate in devicemanagerDelegates) {
					devicemanagerDelegate.WillStartConnectingToDevice(this, device);
				}
			}
		}

	}
}
