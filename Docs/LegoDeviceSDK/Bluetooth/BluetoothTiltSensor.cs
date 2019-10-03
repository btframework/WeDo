using System;
using LegoDeviceSDK.Generic;
using LegoDeviceSDK.Generic.Data;
using LegoDeviceSDK.Helpers;
using LegoDeviceSDK.Interfaces;
using LegoDeviceSDK.Interfaces.Services;

namespace LegoDeviceSDK.Bluetooth {

	/// <summary>
	/// This service provides readings from a tilt sensor.
	///Add a instance of a <see cref="ITiltSensorDelegate"/> using addDelegate to be notified when a service receives an updated value.
	/// </summary>
	public class BluetoothTiltSensor : BluetoothService, ITiltSensor {
		const string Tag = "BluetoothTiltSensor";
		/// <summary>
		/// The most recent angle reading from the sensor. The angle represents the angle the sensor is tilted in the x, y and z-<see cref="Direction"/>.
		/// </summary>
		// <remarks>If no angle reading has been received, of if the sensor is not in mode <see cref="LeTiltSensorMode.LeTiltSensorModeAngle"/> the value of this property will be LETiltSensorAngleZero</remarks>
		public TiltSensorAngle Angle {
			get {
				if (TiltSensorMode != TiltSensorMode.TiltSensorModeAngle || NumbersFromValueData == null || NumbersFromValueData.Count != 2) {
					SdkLogger.W(Tag, "To read the Angle value, the TiltSensorMode must be in Angle mode and the last received value must be 2 numbers");
					return new TiltSensorAngle() {X = 0, Y = 0};
				}

				if (InputFormat.Unit == InputFormatUnit.InputFormatUnitSi && NumbersFromValueData[0].Length == 4)
					return new TiltSensorAngle() { X = BitConverter.ToSingle(NumbersFromValueData[0], 0), Y = BitConverter.ToSingle(NumbersFromValueData[1], 0) };
				else
					return new TiltSensorAngle() { X = ValueHelper.ConvertToSigned(NumbersFromValueData[0][0]), Y = ValueHelper.ConvertToSigned(NumbersFromValueData[1][0]) };
			}
		}

		/// <summary>
		/// The most recent crash reading from the sensor. The value represents the number of times the sensor has been ‘bumped’ in the x, y, and z-<see cref="Direction"/>. The value can be reset by sending the <see cref="IService.SendResetStateRequest"/>.
		/// </summary>
		// <remarks>If no crash reading has been received, of if the sensor is not in mode <see cref="TiltSensorMode.TiltSensorModeCrash"/> the value of this property will be LETiltSensorCrashZero</remarks>
		public TiltSensorCrash Crash {
			get {
				if (TiltSensorMode != TiltSensorMode.LeTiltSensorModeCrash || NumbersFromValueData == null || NumbersFromValueData.Count != 3) {
					SdkLogger.W(Tag, "To read the Crash value, the TiltSensorMode must be in Crash mode and the last received value must be 3 numbers");
					return new TiltSensorCrash() { X = 0, Y = 0, Z = 0 };
				}

				if (InputFormat.Unit == InputFormatUnit.InputFormatUnitSi && NumbersFromValueData[0].Length == 4)
					return new TiltSensorCrash() { X = BitConverter.ToSingle(NumbersFromValueData[0], 0), Y = BitConverter.ToSingle(NumbersFromValueData[1], 0), Z = BitConverter.ToSingle(NumbersFromValueData[2], 0)};
				else
					return new TiltSensorCrash() { X = ValueHelper.ConvertToUnsigned(NumbersFromValueData[0][0]), Y = ValueHelper.ConvertToUnsigned(NumbersFromValueData[1][0]), Z = ValueHelper.ConvertToUnsigned(NumbersFromValueData[2][0]) };
			}
		}


		/// <summary>
		/// The most recent direction reading from the sensor.
		/// </summary>
		/// <remarks>If no direction reading has been received, of if the sensor is not in mode <see cref="Generic.Data.TiltSensorMode.TiltSensorModeTilt"/> the value of this property will be <see cref="TiltSensorDirection.TiltSensorDirectionUnknown"/>.</remarks>
		public TiltSensorDirection Direction {
			get {
				if (TiltSensorMode != TiltSensorMode.TiltSensorModeTilt) {
					SdkLogger.W(Tag, "To read the Direction value, the TiltSensorMode must be in Direction mode");
					return TiltSensorDirection.TiltSensorDirectionUnknown;
				}

				return (TiltSensorDirection)ValueAsInteger;
			}
		}

		/// <summary>
		/// The current mode of the tilt sensor
		/// </summary>
		public TiltSensorMode TiltSensorMode {
            get { return (TiltSensorMode)InputFormatMode; }
            set { InputFormatMode = (int)value; }    
        }

		internal BluetoothTiltSensor(IDevice device, IIo io, ConnectInfo connectInfo)
			: base(device, io, connectInfo) {
			ServiceName = "Tilt Sensor";
			AddValidDataFormats();
			DefaultInputFormat = new InputFormat() { ConnectId = connectInfo.ConnectId, TypeId = (int)connectInfo.TypeEnum, Mode = (int)TiltSensorMode.TiltSensorModeTilt, DeltaInterval = 1, Unit = InputFormatUnit.InputFormatUnitSi, NotificationsEnabled = true };
		}

		/// <summary>
		/// Add valid data formats for the TiltSensor
		/// </summary>
		void AddValidDataFormats() {
			AddValidDataFormat(DataFormat.FormatWithModeName("Angle", (int)TiltSensorMode.TiltSensorModeAngle, InputFormatUnit.InputFormatUnitRaw, 1, 2));
			AddValidDataFormat(DataFormat.FormatWithModeName("Angle", (int)TiltSensorMode.TiltSensorModeAngle, InputFormatUnit.InputFormatUnitPercentage, 1, 2));
			AddValidDataFormat(DataFormat.FormatWithModeName("Angle", (int)TiltSensorMode.TiltSensorModeAngle, InputFormatUnit.InputFormatUnitSi, 4, 2));

			AddValidDataFormat(DataFormat.FormatWithModeName("Count", (int)TiltSensorMode.TiltSensorModeTilt, InputFormatUnit.InputFormatUnitRaw, 1, 1));
			AddValidDataFormat(DataFormat.FormatWithModeName("Count", (int)TiltSensorMode.TiltSensorModeTilt, InputFormatUnit.InputFormatUnitPercentage, 1, 1));
			AddValidDataFormat(DataFormat.FormatWithModeName("Count", (int)TiltSensorMode.TiltSensorModeTilt, InputFormatUnit.InputFormatUnitSi, 4, 1));

			AddValidDataFormat(DataFormat.FormatWithModeName("Crash", (int)TiltSensorMode.LeTiltSensorModeCrash, InputFormatUnit.InputFormatUnitRaw, 1, 3));
			AddValidDataFormat(DataFormat.FormatWithModeName("Crash", (int)TiltSensorMode.LeTiltSensorModeCrash, InputFormatUnit.InputFormatUnitPercentage, 1, 3));
			AddValidDataFormat(DataFormat.FormatWithModeName("Crash", (int)TiltSensorMode.LeTiltSensorModeCrash, InputFormatUnit.InputFormatUnitSi, 4, 3));
		}

		internal override bool HandleUpdatedValueData(byte[] data, out SdkError error) {

			var oldAngle = Angle;
			var oldCrash = Crash;
			var oldDirection = Direction;

			bool success = base.HandleUpdatedValueData(data, out error);
			if (success) {
				lock (serviceDelegates) {
					foreach (var serviceDelegate in serviceDelegates) {
						var tiltSensorDelegate = serviceDelegate as ITiltSensorDelegate;
						if (tiltSensorDelegate != null) {
							if (TiltSensorMode == TiltSensorMode.TiltSensorModeAngle) {
								tiltSensorDelegate.DidUpdateAngle(this, oldAngle, Angle);
							}
							else if (TiltSensorMode == TiltSensorMode.LeTiltSensorModeCrash) {
								tiltSensorDelegate.DidUpdateCrash(this, oldCrash, Crash);
							}
							else if (TiltSensorMode == TiltSensorMode.TiltSensorModeTilt) {
								tiltSensorDelegate.DidUpdateDirection(this, oldDirection, Direction);
							}
						}
					}
				}
			}
			return success;
		}


	}
}
