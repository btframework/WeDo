using LegoDeviceSDK.Generic;
using LegoDeviceSDK.Interfaces;
using LegoDeviceSDK.Interfaces.Services;

namespace LegoDeviceSDK.Bluetooth {
	/// <summary>
	/// This service provides voltage (<see cref="MilliVolts"/>) readings for the battery on the device. Add a instance of a <see cref="IVoltageSensorDelegate"/> using addDelegate to be notified when a service receives an updated value.
	/// </summary>
	public class BluetoothVoltageSensor : BluetoothService, IVoltageSensor {
		const string Tag = "BluetoothVoltageSensor";
		internal BluetoothVoltageSensor(IDevice device, IIo io, ConnectInfo connectInfo)
			: base(device, io, connectInfo) {
			ServiceName = "Voltage Sensor";
			DefaultInputFormat = new InputFormat() { ConnectId = ConnectInfo.ConnectId, DeltaInterval = 30, Mode = 0, NumberOfBytes = 1, NotificationsEnabled = true, Unit = InputFormatUnit.InputFormatUnitSi };
		}

		/// <summary>
		/// The battery voltage in milli volts
		/// </summary>
		public float MilliVolts {
			get {
				if (InputFormat == null || InputFormat.Mode != 0 || InputFormat.Unit != InputFormatUnit.InputFormatUnitSi) {
					SdkLogger.W(Tag, "Can only retrieve milli volts from Voltage Sensor when sensor is in mode 0 and uses SI units");
					return 0f;
				}
				return ValueAsFloat;
			} }

		internal override bool HandleUpdatedValueData(byte[] data, out SdkError error) {
			var success = base.HandleUpdatedValueData(data, out error);
			if (success) {
				lock (serviceDelegates) {
					foreach (var serviceDelegate in serviceDelegates) {
						var voltageSensorDelegate = serviceDelegate as IVoltageSensorDelegate;
						if (voltageSensorDelegate != null) {
							voltageSensorDelegate.DidUpdateMilliVolts(this, MilliVolts);
						}
					}
				}
			}
			return success;
		}
	}
}
