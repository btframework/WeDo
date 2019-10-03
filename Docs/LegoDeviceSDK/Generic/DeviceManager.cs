using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using LegoDeviceSDK.Bluetooth;
using LegoDeviceSDK.Interfaces;

namespace LegoDeviceSDK.Generic {
	/// <summary>
	/// This class is the main entry point for connecting and communicating with a LEGO Device. You must implement the <see cref="IDeviceManagerDelegate"/> protocol and set the delegate property before scanning for and connecting to devices.
	/// </summary>
	public class DeviceManager : IDeviceManager {
		readonly BluetoothDeviceManager bluetoothDeviceManager;
		private static volatile DeviceManager instance;
		private static object syncRoot = new Object();

		/// <summary>
		/// The shared LEDeviceManager
		/// </summary>
		/// <returns>The shared <see cref="BluetoothDeviceManager"/></returns>
		public static DeviceManager SharedDeviceManager {
			get {
				if (instance == null) {
					lock (syncRoot) {
						if (instance == null)
							instance = new DeviceManager();
					}
				}
				return instance;
			}
		}

		/// <summary>
		/// Show the Version of the SDK
		/// </summary>
		public string Version {
			get {
				string name = typeof(DeviceManager).AssemblyQualifiedName;
				var asmName = new AssemblyName(name);
				return asmName.Version.Major + "." + asmName.Version.Minor + "." + asmName.Version.Build;
			}
		}

		/// <summary>
		/// Public constructor for IOC
		/// </summary>
		public DeviceManager() {
			instance = this;
			bluetoothDeviceManager = BluetoothDeviceManager.SharedDeviceManager;
			AutomaticReconnectOnConnectionLostEnabled = automaticReconnectOnConnectionLostEnabled;
			ConnectRequestTimeoutInterval = connectRequestTimeoutInterval;
		}

		/// <summary>
		/// Returns a list with all known devices regardless of their current connect state.
		/// </summary>
		public List<IDevice> AllDevices {
			get {
				var allDevices = new List<IDevice>();
				if(bluetoothDeviceManager != null)
					allDevices.AddRange(bluetoothDeviceManager.AllDevices);

				return allDevices;
			}
		}

		/// <summary>
		/// If enabled, the <see cref="IDeviceManager"/> will attempt to reconnect in case of a connection loss, but only if the connection was not closed by the user, the default value is NO.
		/// </summary>
		public bool AutomaticReconnectOnConnectionLostEnabled {
			get { return automaticReconnectOnConnectionLostEnabled; }
			set {
				automaticReconnectOnConnectionLostEnabled = value;
				// forward value to BluetoothDeviceManager
				bluetoothDeviceManager.AutomaticReconnectOnConnectionLostEnabled = value;
			}
		}
		bool automaticReconnectOnConnectionLostEnabled = true;

		/// <summary>
		/// If a connect request is not successful within this time interval the connection attempt is cancelled and the <see cref="IDeviceManagerDelegate.DidFailToConnectToDevice"/> is invoked. The default value is 10 seconds.
		/// </summary>
		public TimeSpan ConnectRequestTimeoutInterval {
			get { return connectRequestTimeoutInterval; }
			set {
				connectRequestTimeoutInterval = value;
				// forward value to BluetoothDeviceManager
				bluetoothDeviceManager.ConnectRequestTimeoutInterval = value;
			} }
		TimeSpan connectRequestTimeoutInterval = new TimeSpan(0,0,10);

		/// <summary>
		/// Add a delegate to receive device discovery and connection events
		/// </summary>
		/// <param name="deviceManagerDelegate">Add a delegate to receive device discovery and connection events</param>
		public void AddDelegate(IDeviceManagerDelegate deviceManagerDelegate) {
			if (bluetoothDeviceManager != null)
				bluetoothDeviceManager.AddDelegate(deviceManagerDelegate);
		}

		/// <summary>
		/// Remove a delegate
		/// </summary>
		/// <param name="deviceManagerDelegate">The delegate to remove</param>
		public void RemoveDelegate(IDeviceManagerDelegate deviceManagerDelegate) {
			if (bluetoothDeviceManager != null)
				bluetoothDeviceManager.RemoveDelegate(deviceManagerDelegate);
		}

		/// <summary>
		/// Disconnect from a LEGO Device
		/// </summary>
		/// <param name="device">The device to disconnect from</param>
		public void CancelDeviceConnection(IDevice device) {
			if (bluetoothDeviceManager != null)
				bluetoothDeviceManager.CancelDeviceConnection(device);

		}

		/// <summary>
		/// Connect to a LEGO LEDevice. If a connection is not established within the connectRequestTimeoutInterval the connection attempt is cancelled and the <see cref="IDeviceManagerDelegate.DidFailToConnectToDevice"/> is invoked
		/// </summary>
		/// <param name="device">The device to establish a connection to</param>
		/// <returns>Task that can be awaited</returns>
		public async Task ConnectToDeviceAsync(IDevice device) {
			if (bluetoothDeviceManager != null)
				await bluetoothDeviceManager.ConnectToDeviceAsync(device);

		}

		/// <summary>
		/// Start scanning for LEGO devices
		/// </summary>
		/// <returns>Task that can be awaited</returns>
		public async Task ScanAsync() {
			if (bluetoothDeviceManager != null)
				await bluetoothDeviceManager.ScanAsync();

		}

	}
}
