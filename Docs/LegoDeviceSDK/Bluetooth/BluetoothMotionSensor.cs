using System;
using LegoDeviceSDK.Generic;
using LegoDeviceSDK.Generic.Data;
using LegoDeviceSDK.Interfaces;
using LegoDeviceSDK.Interfaces.Services;

namespace LegoDeviceSDK.Bluetooth {


	/// <summary>
	///This service provides readings from an motion sensor (aka. detect sensor).
	///Add a instance of a <see cref="IMotionSensorDelegate"/> using addDelegate: to be notified when a service receives an updated value.
	///</summary>
	public class BluetoothMotionSensor : BluetoothService, IMotionSensor {
		float distance = 0f;
		uint count = 0;

		/// <summary>
		/// The current mode of the motion sensor
		/// </summary>
		public MotionSensorMode MotionSensorMode {
            get { return (MotionSensorMode)InputFormatMode; }
            set { InputFormatMode = (int)value; }
        }

		internal BluetoothMotionSensor(IDevice device, IIo io, ConnectInfo connectInfo)
			: base(device, io, connectInfo) {
			ServiceName = "Motion Sensor";
			AddValidDataFormats();
			DefaultInputFormat = new InputFormat() { ConnectId = ConnectInfo.ConnectId, DeltaInterval = 1, Mode = 0, NumberOfBytes = 1, NotificationsEnabled = true, Unit = InputFormatUnit.InputFormatUnitSi };
		}

		/// <summary>
		/// Adds valid dataformats. Input must be according toone of these
		/// </summary>
		void AddValidDataFormats() {
			AddValidDataFormat(DataFormat.FormatWithModeName("Detect", (int)MotionSensorMode.Detect, InputFormatUnit.InputFormatUnitRaw, 1, 1));
			AddValidDataFormat(DataFormat.FormatWithModeName("Detect", (int)MotionSensorMode.Detect, InputFormatUnit.InputFormatUnitPercentage, 1, 1));
			AddValidDataFormat(DataFormat.FormatWithModeName("Detect", (int)MotionSensorMode.Detect, InputFormatUnit.InputFormatUnitSi, 4, 1));
			AddValidDataFormat(DataFormat.FormatWithModeName("Count", (int)MotionSensorMode.Count, InputFormatUnit.InputFormatUnitRaw, 4, 1));
			AddValidDataFormat(DataFormat.FormatWithModeName("Count", (int)MotionSensorMode.Count, InputFormatUnit.InputFormatUnitPercentage, 1, 1));
			AddValidDataFormat(DataFormat.FormatWithModeName("Count", (int)MotionSensorMode.Count, InputFormatUnit.InputFormatUnitSi, 4, 1));

		}

		/// <summary>
		/// The most recent distance reading from the sensor
		/// </summary>
		public float Distance {
			get {
				if (MotionSensorMode != MotionSensorMode.Detect)
					return 0f;

				return distance;
			}
		}

		/// <summary>
		/// The most recent count reading from the sensor
		/// </summary>
		public uint Count {
			get {
				if (MotionSensorMode != MotionSensorMode.Count)
					return 0;

				return count;
			}
		}

		/// <summary>
		/// Handles new Values from the device. 
		/// </summary>
		/// <param name="data">The new value form the device</param>
		/// <param name="error">Error != null if error occurs</param>
		/// <returns>True if new Value data is valid</returns>
		internal override bool HandleUpdatedValueData(byte[] data, out SdkError error) {
			var oldDistance = distance;
			bool success = base.HandleUpdatedValueData(data, out error);
			if (success) {
				// handle detect mode
				if (MotionSensorMode == MotionSensorMode.Detect) {
					if (data.Length == 4) {
						distance = ValueAsFloat;
					}
					else {
						distance = ValueAsInteger;
					}
				}
				// handle count mode
				else if (MotionSensorMode == MotionSensorMode.Count) {
					if (data.Length == 4) {
						count = (uint) ValueAsInteger;
					}
					else {
						count = (uint) ValueAsInteger;
					}
				}
				// notify watchers
				lock (serviceDelegates) {
					foreach (var serviceDelegate in serviceDelegates) {
						var motionSensorDelegate = serviceDelegate as IMotionSensorDelegate;
						if (motionSensorDelegate != null) {
							if (MotionSensorMode == MotionSensorMode.Detect) {
								motionSensorDelegate.DidUpdateDistance(this, oldDistance, distance);
							}
							else if (MotionSensorMode == MotionSensorMode.Count) {
								motionSensorDelegate.DidUpdateCount(this, count);
							}
						}
					}
				}
			}
			return success;
		}
	}
}
