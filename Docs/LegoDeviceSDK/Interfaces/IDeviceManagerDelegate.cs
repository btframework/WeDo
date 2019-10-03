using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegoDeviceSDK.Interfaces {
	/// <summary>
	/// Implement this protocol to be notified when a new device starts or stops advertising, and when a connection to a device is established or closed.
	/// </summary>
	public interface IDeviceManagerDelegate {

		/// <summary>
		/// Invoked when a device advertising a LEGO Device service UUID is discovered
		/// </summary>
		/// <param name="deviceManager">The <see cref="IDeviceManager"/></param>
		/// <param name="device">The <see cref="IDevice"/></param>
		void DeviceDidAppear(IDeviceManager deviceManager, IDevice device);
		/// <summary>
		/// Invoked when a device stops advertising a LEGO Device service. The <see cref="IDeviceManager"/> will check at small refresh-intervals if an advertising packet was recevived during the refresh-interval. If not, this method is invoked.
		/// </summary>
		/// <param name="deviceManager">The <see cref="IDeviceManager"/></param>
		/// <param name="device">The <see cref="IDevice"/></param>
		void DeviceDidDisappear(IDeviceManager deviceManager, IDevice device);

		/// <summary>
		/// Invoked when a device is disconnected
		/// </summary>
		/// <param name="deviceManager">The <see cref="IDeviceManager"/></param>
		/// <param name="device">The <see cref="IDevice"/></param>
		/// <param name="autoReconnect">YES if an automatic reconnect will be attempted, see <see cref="IDeviceManager.AutomaticReconnectOnConnectionLostEnabled"/>.</param>
		void DidDisconnectFromDevice(IDeviceManager deviceManager, IDevice device, bool autoReconnect);
		/// <summary>
		/// Invoked when a device fails to connect, of if a connection request times out.
		/// </summary>
		/// <param name="deviceManager">The <see cref="IDeviceManager"/></param>
		/// <param name="device">The <see cref="IDevice"/></param>
		/// <param name="autoReconnect">YES if an automatic reconnect will be attempted, see <see cref="IDeviceManager.AutomaticReconnectOnConnectionLostEnabled"/>.</param>
		void DidFailToConnectToDevice(IDeviceManager deviceManager, IDevice device, bool autoReconnect);

		/// <summary>
		/// Invoked when a connection to a device is established and all required services has been discovered. At this point the device is ready for use.
		/// </summary>
		/// <param name="deviceManager">The <see cref="IDeviceManager"/></param>
		/// <param name="device">The <see cref="IDevice"/></param>
		void DidFinishInterrogatingDevice(IDeviceManager deviceManager, IDevice device);
		/// <summary>
		/// Invoked when a connection to a device is established, and the interrogation of the devices for required services begins. A connection is established at this point but the device is yet not ready to be used.
		/// </summary>
		/// <param name="deviceManager">The <see cref="IDeviceManager"/></param>
		/// <param name="device">The <see cref="IDevice"/></param>
		void DidStartInterrogatingDevice(IDeviceManager deviceManager, IDevice device);
		/// <summary>
		/// Invoked when starting a device connect attempt. Normally, this will happen right after calling <see cref="IDeviceManager.ConnectToDeviceAsync"/>. However, it may also happen in relation to an automatic reconnect attempt.
		/// </summary>
		/// <param name="deviceManager">The <see cref="IDeviceManager"/></param>
		/// <param name="device">The <see cref="IDevice"/></param>
		void WillStartConnectingToDevice(IDeviceManager deviceManager, IDevice device);

	}
}
