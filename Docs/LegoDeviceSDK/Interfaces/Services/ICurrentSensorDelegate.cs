using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegoDeviceSDK.Bluetooth;

namespace LegoDeviceSDK.Interfaces.Services {
	/// <summary>
	/// Implement this protocol to be notified when the <see cref="BluetoothCurrentSensor"/> updates its value
	/// </summary>
	public interface ICurrentSensorDelegate : IServiceDelegate {

		/// <summary>
        /// Invoked when the <see cref="ICurrentSensor"/> receives an updated value
		/// </summary>
		/// <param name="service">The sensor that has a new value</param>
		/// <param name="milliAmps">The new current value in milli amp</param>
		void DidUpdateMilliAmp(ICurrentSensor service, float milliAmps);

	}
}
