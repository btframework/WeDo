using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LegoDeviceSDK.Interfaces {

	/// <summary>
	/// This class is the main entry point for connecting and communicating with a LEGO Device. You must implement the <see cref="IDeviceManagerDelegate"/> protocol and set the delegate property before scanning for and connecting to devices.
	/// </summary>
	public interface IDeviceManager {

		/// <summary>
		/// Returns a list with all known devices regardless of their current connect state.
		/// </summary>
		List<IDevice> AllDevices { get; }
		
		/// <summary>
		/// If enabled, the <see cref="IDeviceManager"/> will attempt to reconnect in case of a connection loss, but only if the connection was not closed by the user, the default value is NO.
		/// </summary>
		bool AutomaticReconnectOnConnectionLostEnabled { get; set; }
		
		/// <summary>
		/// If a connect request is not successful within this time interval the connection attempt is cancelled and the <see cref="IDeviceManagerDelegate.DidFailToConnectToDevice"/> is invoked. The default value is 10 seconds.
		/// </summary>
		TimeSpan ConnectRequestTimeoutInterval { get; set; }

		/// <summary>
		/// Add a delegate to receive device discovery and connection events
		/// </summary>
		/// <param name="deviceManagerDelegate">Add a delegate to receive device discovery and connection events</param>
		void AddDelegate(IDeviceManagerDelegate deviceManagerDelegate);
		
		/// <summary>
		/// Remove a delegate
		/// </summary>
		/// <param name="deviceManagerDelegate">The delegate to remove</param>
		void RemoveDelegate(IDeviceManagerDelegate deviceManagerDelegate);

		/// <summary>
		/// Disconnect from a LEGO Device
		/// </summary>
		/// <param name="device">The device to disconnect from</param>
		void CancelDeviceConnection(IDevice device);
		
		/// <summary>
		/// Connect to a LEGO LEDevice. If a connection is not established within the connectRequestTimeoutInterval the connection attempt is cancelled and the <see cref="IDeviceManagerDelegate.DidFailToConnectToDevice"/> is invoked
		/// </summary>
		/// <param name="device">The device to establish a connection to</param>
		/// <returns>Task that can be awaited</returns>
		Task ConnectToDeviceAsync(IDevice device);

		/// <summary>
		/// Start scanning for LEGO devices
		/// </summary>
		/// <returns>Task that can be awaited</returns>
		Task ScanAsync();
		
		///// <summary>
		///// Stop scanning for LEGO devices
		///// </summary>
		//void StopScanning();


	}
}
