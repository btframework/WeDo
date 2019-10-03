using LegoDeviceSDK.Interfaces;

namespace LegoDeviceSDK.Generic {
	/// <summary>
	/// The format to be compared to the receiver.
	/// </summary>
	public class DeviceInfo {

		/// <summary>
		/// The firmware revision of a <see cref="IDevice"/>
		/// </summary>
		public Revision FirmwareRevision { get; internal set; }
		/// <summary>
		/// The hardware revision of a <see cref="IDevice"/>
		/// </summary>
		public Revision HardwareRevision { get; internal set; }
		/// <summary>
		/// The software  revision of a <see cref="IDevice"/>
		/// </summary>
		public Revision SoftwareRevision { get; internal set; }

		/// <summary>
		/// The manufacturer name of the <see cref="IDevice"/>
		/// </summary>
		public string ManufacturerName { get; internal set; }

		/// <summary>
		/// Return YES if this device info is equal to info
		/// </summary>
		/// <param name="other">The device info to check for equality with</param>
		/// <returns>Return YES if this device info is equal to info</returns>
		public bool Equals(DeviceInfo other) {
			return Equals(FirmwareRevision, other.FirmwareRevision) && Equals(HardwareRevision, other.HardwareRevision) && Equals(SoftwareRevision, other.SoftwareRevision) && string.Equals(ManufacturerName, other.ManufacturerName);
		}

	}
}
