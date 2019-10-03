using System;
using System.Text;
using LegoDeviceSDK.Bluetooth;
using LegoDeviceSDK.Interfaces;

namespace LegoDeviceSDK.Generic {
	/// <summary>
	///This class represent a configuration of a Input (sensor). At any time a sensor can be in just one mode, and the details of this mode is captured by this class.
	///For senors types recognized by the SDK (like the <see cref="BluetoothTiltSensor"/> and <see cref="BluetoothMotionSensor"/>) you will not need to know about the details of this class as the implementation of these services in the SDK handles this for you.
	///If you need to access an Input that is not recognized by the SDK you will need to create and send an input format for the corresponding service. See the <see cref="BluetoothGenericService"/> documentation for an example.
	///</summary>
	public class InputFormat {
		const string Tag = "InputFormat";
		const int InputFormatPackageSize = 11;
		/// <summary>
		/// The connectID of the corresponding service, see <see cref="IService.ConnectInfo"/>
		/// </summary>
		/// <returns>The connect id</returns>
		public int ConnectId { get; internal set; }
		/// <summary>
		/// The delta interval. When notifications are enabled the service will only receive updates if the value has change with ‘delta interval’ or more since last reading
		/// </summary>
		/// <returns>The delta interval</returns>
		public uint DeltaInterval { get; internal set; }
		/// <summary>
		/// The mode of the Input
		/// </summary>
		/// <returns>The mode of the Input</returns>
		public int Mode { get; internal set; }
		/// <summary>
		/// YES if new values are send whenever the value of the Input changes beyond delta interval
		/// </summary>
		/// <returns>True if notifications are enabled</returns>
		public bool NotificationsEnabled { get; internal set; }
		/// <summary>
		/// The number of bytes to be expected in the Input data payload (set by the Device)
		/// </summary>
		/// <returns>The number of bytes</returns>
		public int NumberOfBytes { get; internal set; }
		/// <summary>
		/// The revision of the <see cref="InputFormat"/> (set by the <see cref="IDevice"/>). When a new <see cref="InputFormat"/> is set for a service the Device will send the updated <see cref="InputFormat"/> through the <see cref="IServiceDelegate.DidUpdateInputFormat"/>. The Device will assign a revision number to the new <see cref="InputFormat"/>. The revision number is matched against the revision format when receiving values for the corresponding service.
		/// </summary>
		/// <returns>The revision of the <see cref="InputFormat"/></returns>
		public int Revision { get; internal set; }
		/// <summary>
		/// The typeID of the corresponding service, see <see cref="IService.ConnectInfo"/>
		/// </summary>
		/// <returns>The typeID of the corresponding service</returns>
		public int TypeId { get; internal set; }
		/// <summary>
		/// The unit the values are delivered in (as raw values, or as SI values)
		/// </summary>
		/// <returns>The unit of the values</returns>
		public InputFormatUnit Unit { get; internal set; }

		internal static InputFormat InputFormatFromByteArray(byte[] data) {
			if (data == null) {
				SdkLogger.E(Tag, "Cannot instantiate InputFormat with null data");
				return null;
			}
			if (data.Length != InputFormatPackageSize) {
				SdkLogger.E(Tag, string.Format("Cannot create InputFormat from package with size {0}, expected size to be {1}", data.Length, InputFormatPackageSize));
				return null;
			}
			var format = new InputFormat() {
				Revision = data[0],
				ConnectId = data[1],
				TypeId = data[2],
				Mode = data[3],
				DeltaInterval = BitConverter.ToUInt32(data, 4),
				Unit = (InputFormatUnit) data[8],
				NotificationsEnabled = data[9] == 1,
				NumberOfBytes = data[10],
			};
			return format;
		}

		internal static byte[] InputFormatToByteArray(InputFormat inputFormat) {
			var deltaInterval = BitConverter.GetBytes(inputFormat.DeltaInterval);
			var data = new byte[] {
				(byte)inputFormat.TypeId,
				(byte)inputFormat.Mode,
				deltaInterval[0],
				deltaInterval[1],
				deltaInterval[2],
				deltaInterval[3],
				(byte)inputFormat.Unit,
				inputFormat.NotificationsEnabled ? (byte)1 : (byte)0,
			};
			return data;
		}

		/// <summary>
		/// Create a new instance of an <see cref="InputFormat"/>.
		/// </summary>
		/// <param name="connectId">The connectID of the service, see <see cref="IService.ConnectInfo"/>.</param>
		/// <param name="typeId">The type of the IO, see <see cref="IService.ConnectInfo"/>.</param>
		/// <param name="mode">The mode of the IO (Inputs/Senors may support a number of different modes)</param>
		/// <param name="deltaInterval">The delta interval</param>
		/// <param name="unit">The unit the sensor should return values in</param>
		/// <param name="notificationsEnabled">YES if the device should send updates when the value changes.</param>
		/// <returns>New input format configured with the given parameters</returns>
		public static InputFormat InputFormatWithConnectId(int connectId, int typeId, int mode, uint deltaInterval, InputFormatUnit unit, bool notificationsEnabled) {
			return new InputFormat() {
				ConnectId = connectId, DeltaInterval = deltaInterval, Mode = mode, NotificationsEnabled = notificationsEnabled, TypeId = typeId, Unit = unit,
			};
		}

		/// <summary>
		/// Creates a copy of this input format with a new delta interval
		/// </summary>
		/// <param name="interval">The new delta interval</param>
		/// <returns>new Input format with new delta interval</returns>
		public InputFormat InputFormatBySettingDeltaInterval(uint interval) {
			return new InputFormat() {
				ConnectId = ConnectId,
				DeltaInterval = interval,
				Mode = Mode,
				NotificationsEnabled = NotificationsEnabled,
				TypeId = TypeId,
				Unit = Unit,
			};
		}
		/// <summary>
		/// Creates a copy of this input format with a new <paramref name="mode"/>
		/// </summary>
		/// <param name="mode">The new mode</param>
		/// <returns>new Input format with new mode</returns>
		public InputFormat InputFormatBySettingMode(int mode) {
			return new InputFormat() {
				ConnectId = ConnectId,
				DeltaInterval = DeltaInterval,
				Mode = mode,
				NotificationsEnabled = NotificationsEnabled,
				TypeId = TypeId,
				Unit = Unit,
			};
		}

		/// <summary>
		/// Creates a copy of this input format with a new <paramref name="mode"/> and <paramref name="unit"/>
		/// </summary>
		/// <param name="mode">The new mode</param>
		/// <param name="unit">The new unit</param>
		/// <returns>new Input format with new mode and unit</returns>
		public InputFormat InputFormatBySettingMode(int mode, InputFormatUnit unit) {
			return new InputFormat() {
				ConnectId = ConnectId,
				DeltaInterval = DeltaInterval,
				Mode = mode,
				NotificationsEnabled = NotificationsEnabled,
				TypeId = TypeId,
				Unit = unit,
			};
		}

		/// <summary>
		/// Creates a copy of this input format with a new value for notifications enabled
		/// </summary>
		/// <param name="notificationsEnabled">YES if the sensor should send updates when the value changes</param>
		/// <returns>new Input format with new notifications enabled</returns>
		public InputFormat InputFormatBySettingNotificationsEnabled(bool notificationsEnabled) {
			return new InputFormat() {
				ConnectId = ConnectId,
				DeltaInterval = DeltaInterval,
				Mode = Mode,
				NotificationsEnabled = notificationsEnabled,
				TypeId = TypeId,
				Unit = Unit,
			};
		}

		/// <summary>
		/// String formatting of the <see cref="InputFormat"/>
		/// </summary>
		/// <returns>A formatted string of the <see cref="InputFormat"/></returns>
		public override string ToString() {
			var sb = new StringBuilder();
			sb.Append("<InputFormat:");
			sb.Append(string.Format(", revision: {0}", Revision));
			sb.Append(string.Format(", ConnectId: {0}", ConnectId));
			sb.Append(string.Format(", TypeId: {0}", TypeId));
			sb.Append(string.Format(", Mode: {0}", Mode));
			sb.Append(string.Format(", DeltaInterval: {0}", DeltaInterval));
			sb.Append(string.Format(", Unit: {0}", Unit));
			sb.Append(string.Format(", NotificationsEnabled: {0}", NotificationsEnabled));
			sb.Append(">");

			return sb.ToString();
		}

		/// <summary>
		/// A Short description of the <see cref="InputFormat"/>.
		/// </summary>
		public string ShortDescription {
			get {
				var sb = new StringBuilder();
				sb.Append(string.Format("Unit: {0}", UnitString()));
				sb.Append(string.Format(", DeltaInterval: {0}", DeltaInterval));

				return sb.ToString();
			}
		}

		private string UnitString() {
			switch (Unit) {
				case InputFormatUnit.InputFormatUnitRaw:
					return "Raw";
				case InputFormatUnit.InputFormatUnitPercentage:
					return "Percentage";
				case InputFormatUnit.InputFormatUnitSi:
					return "SI";
				case InputFormatUnit.InputFormatUnitUnknown:
					return "Unknown";
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// Returns YES if this input format is equal to otherFormat
		/// </summary>
		/// <param name="otherFormat">The input format to be compared to the receiver.</param>
		/// <returns>True if Input formats are equal</returns>
		public bool Equals(InputFormat otherFormat) {
			return ConnectId == otherFormat.ConnectId &&
						 DeltaInterval == otherFormat.DeltaInterval &&
						 Mode == otherFormat.Mode &&
						 NotificationsEnabled == otherFormat.NotificationsEnabled &&
						 NumberOfBytes == otherFormat.NumberOfBytes &&
						 Revision == otherFormat.Revision &&
						 TypeId == otherFormat.TypeId &&
						 Unit == otherFormat.Unit;
		}



	}
}
