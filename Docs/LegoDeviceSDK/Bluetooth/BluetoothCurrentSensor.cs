using LegoDeviceSDK.Generic;
using LegoDeviceSDK.Generic.Data;
using LegoDeviceSDK.Interfaces;
using LegoDeviceSDK.Interfaces.Services;

namespace LegoDeviceSDK.Bluetooth {
	/// <summary>
	/// This service provides current (<see cref="MilliAmp"/>) readings for the battery on the device. Add a instance of a LECurrentSensorDelegate using addDelegate: to be notified when a service receives an updated value.
	/// </summary>
    public class BluetoothCurrentSensor : BluetoothService, ICurrentSensor {
		const string Tag = "BluetoothCurrentSensor";
		internal BluetoothCurrentSensor(IDevice device, IIo io, ConnectInfo connectInfo) : base(device, io, connectInfo) {
			ServiceName = "Current Sensor";
			DefaultInputFormat = new InputFormat() { ConnectId = ConnectInfo.ConnectId, DeltaInterval = 30, Mode = 0, NumberOfBytes = 1, NotificationsEnabled = true, Unit = InputFormatUnit.InputFormatUnitSi };
		}


		/// <summary>
		/// The battery current in milli amps
		/// </summary>
		public float MilliAmp {
			get {
                if (InputFormat == null || InputFormat.Mode != 0 || InputFormat.Unit != InputFormatUnit.InputFormatUnitSi) {
					SdkLogger.W(Tag, "Can only retrieve milli amps from Current Sensor when sensor is in mode 0 and uses SI units");
					return 0f;
				}

				return ValueAsFloat;
			}
		}

		internal override bool HandleUpdatedValueData(byte[] data, out SdkError error) {
			var success = base.HandleUpdatedValueData(data, out error);
			if (success) {
				lock (serviceDelegates) {
					foreach (var serviceDelegate in serviceDelegates) {
						var currentSensorDelegate = serviceDelegate as ICurrentSensorDelegate;
						if (currentSensorDelegate != null) {
							currentSensorDelegate.DidUpdateMilliAmp(this, MilliAmp);
						}
					}
				}
			}
			return success;
		}


	}

}
