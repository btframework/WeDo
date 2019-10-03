using System.Linq;

namespace LegoDeviceSDK.Generic {
	/// <summary>
	/// The Connect Info represent generic info about an IO (service) attached to a device.
	/// </summary>
	public class ConnectInfo {
		/// <summary>
		/// An identifier used to uniquely identify and address the service. The device is guaranteed not to have two services with the same connectID at the same time
		/// </summary>
		public int ConnectId { get; set; }
		/// <summary>
		/// The firmware revision of the attached IO as received from the device
		/// </summary>
		public Revision FirmwareVersion { get; set; }
		/// <summary>
		/// The hardware revision of the attached IO as received from the device
		/// </summary>
		public Revision HardwareVersion { get; set; }
		/// <summary>
		/// The index of the port on the Hub the IO is attached to. If the index is higher than or equal to 50 the service is an internal service
		/// </summary>
		public int HubIndex { get; set; }
		/// <summary>
		/// The type of the IO. This is the raw type number as it received from the device
		/// </summary>
		public int Type { get; set; }
		/// <summary>
		/// The type of the IO. Use the type property to get the raw type number as it received from the device
		/// </summary>
		public IoType TypeEnum { get; set; }
		/// <summary>
		/// The type of the IO as a string - useful for printing in debug statements
		/// </summary>
		public string TypeString { get { return StringFromInputType(TypeEnum); } }

		/// <summary>
		/// Format an <see cref="IoType"/> as a string - useful for printing in debug statements
		/// </summary>
		/// <param name="ioType"></param>
		/// <returns>Returns a string for the input type</returns>
		public static string StringFromInputType(IoType ioType) {
			switch (ioType) {
				case IoType.IoTypeMotor:
					return "Motor";
				case IoType.IoTypeVoltage:
					return "Voltage";
				case IoType.IoTypeCurrent:
					return "Current";
				case IoType.IoTypePiezoTone:
					return "Piezo tone";
				case IoType.IoTypeRgbLight:
					return "RGB light";
				case IoType.IoTypeTiltSensor:
					return "Tilt sensor";
				case IoType.IoTypeMotionSensor:
					return "Motion sensor";
				case IoType.IoTypeGeneric:
					return "Generic";
				default:
					return "Generic";
			}
		}

		/// <summary>
		/// Return YES if this connect info is equal to info.
		/// </summary>
		/// <param name="otherConnectInfo">The connect info to check for equality with</param>
		/// <returns>True is equal</returns>
		public bool IsEqualToConnectInfo(ConnectInfo otherConnectInfo) {
			return ConnectId == otherConnectInfo.ConnectId &&
			       FirmwareVersion == otherConnectInfo.FirmwareVersion &&
			       HardwareVersion == otherConnectInfo.HardwareVersion &&
			       HubIndex == otherConnectInfo.HubIndex &&
			       TypeEnum == otherConnectInfo.TypeEnum &&
			       TypeString == otherConnectInfo.TypeString;

		}

		internal static ConnectInfo FromByteArray(byte[] data) {
			if (data.Length < 2)
				return null;
			if ((data[1] == 1 && data.Length != 12) || (data[1] == 0 && data.Length != 2))
				return null;
			var connectInfo = new ConnectInfo {ConnectId = data[0]};
			if (data[1] == 1) {
				connectInfo.HubIndex = data[2];
				var typeId = data[3];
				connectInfo.Type = typeId;
				if (typeId == 1)
					connectInfo.TypeEnum = IoType.IoTypeMotor;
				else if (typeId == 20)
					connectInfo.TypeEnum = IoType.IoTypeVoltage;
				else if (typeId == 21)
					connectInfo.TypeEnum = IoType.IoTypeCurrent;
				else if (typeId == 22)
					connectInfo.TypeEnum = IoType.IoTypePiezoTone;
				else if (typeId == 23)
					connectInfo.TypeEnum = IoType.IoTypeRgbLight;
				else if (typeId == 34)
					connectInfo.TypeEnum = IoType.IoTypeTiltSensor;
				else if (typeId == 35)
					connectInfo.TypeEnum = IoType.IoTypeMotionSensor;
				else
					connectInfo.TypeEnum = IoType.IoTypeGeneric;

				connectInfo.HardwareVersion = Revision.FromByteArray(data.ToList().GetRange(4, 4).ToArray());
				connectInfo.FirmwareVersion = Revision.FromByteArray(data.ToList().GetRange(8, 4).ToArray());

			}
			return connectInfo;
		}

	}
}
