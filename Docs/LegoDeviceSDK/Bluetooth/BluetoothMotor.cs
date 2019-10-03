using System;
using LegoDeviceSDK.Generic;
using LegoDeviceSDK.Generic.Data;
using LegoDeviceSDK.Interfaces;
using LegoDeviceSDK.Interfaces.Services;

namespace LegoDeviceSDK.Bluetooth {

	/// <summary>
	/// This service allows for controlling a simple motor
	/// </summary>
	public class BluetoothMotor : BluetoothService, IMotor {
		const string Tag = "BluetoothMotor";
		const int MotorPowerDrift = 0;
		const int MotorPowerBrake = 127;

		const int MotorMinSpeed = 1;
		const int MotorMaxSpeed = 100;

        // Only send values in the range 35-100.
        // An offset is needed as values below 35 is not enough power to actually make the motor turn.
        const int MotorPowerOffset = 35;

		/// <summary>
		/// The power the motor is currently running with (0 if braking or drifting).
		/// </summary>
		public int Power {
			get {
				if (Direction == MotorDirection.MotorDirectionBraking || Direction == MotorDirection.MotorDirectionDrifting)
					return 0;
				return Math.Abs(MostRecentSendPower);
			}
		}

		/// <summary>
		/// The current running direction of the motor
		/// </summary>
		public MotorDirection Direction { get; set; }

		/// <summary>
		/// YES if the motor is currently braking (not running)
		/// </summary>
		public bool isBraking {
			get { return MostRecentSendPower == MotorPowerBrake; }
		}

		/// <summary>
		/// YES if the motor is currently drifting / floating. When floating the motor axis can be turned without resistance.
		/// </summary>
		public bool isDrifting {
			get { return MostRecentSendPower == MotorPowerDrift; }
		}

		int MostRecentSendPower { get; set; }

		internal BluetoothMotor(IDevice device, IIo io, ConnectInfo connectInfo)
			: base(device, io, connectInfo) {
			ServiceName = "Motor";
		}

		/// <summary>
		/// Send a command to run the motor at a given <paramref name="power"/> in a given <paramref name="direction"/>. The minimum speed is 0 and the maximum speed is 100.
		/// </summary>
		/// <param name="direction">The direction to run the motor</param>
		/// <param name="power">The power to run the motor with.</param>
		public void RunInDirection(MotorDirection direction, int power) {
			if (direction == MotorDirection.MotorDirectionDrifting || power == 0) {
				Drift();
			}
			else if (direction == MotorDirection.MotorDirectionBraking) {
				Brake();
			}
			else {
				SendPower(ConvertUnsignedMotorPowerToSigned(direction, power));
				Direction = direction;
			}
		}

		/// <summary>
		/// Send a command to stop (brake) the motor
		/// </summary>
		public void Brake() {
			SendPower(MotorPowerBrake);
			Direction = MotorDirection.MotorDirectionBraking;
		}

		/// <summary>
		/// Send a command to stop (drift/float) the motor
		/// </summary>
		public void Drift() {
			SendPower(MotorPowerDrift);
			Direction = MotorDirection.MotorDirectionDrifting;
		}

		private void SendPower(int power) {
			SdkLogger.D(Tag, string.Format("Setting motor power {0} for connectID {1}", power, ConnectInfo.ConnectId));

            if (power == MotorPowerBrake || power == MotorPowerDrift) {
                // Brake and Float should not be affected by the offset
                Io.WriteMotorPower(power, ConnectInfo.ConnectId);
            }
            else {
                int offset = MotorPowerOffset;
                if (Device.DeviceInfo == null ||
                    Device.DeviceInfo.FirmwareRevision == null ||
                    Device.DeviceInfo.FirmwareRevision.MajorVersion == 0) {
                    // On version 0.x of the firmware, PVM offset is handled in the firmware
                    offset = 0;
                }
                Io.WriteMotorPower(power, offset, ConnectInfo.ConnectId);
            }
			MostRecentSendPower = power;

			SdkError error = null;
			HandleUpdatedValueData(new byte[] {(byte) power}, out error);
			if(error != null)
				SdkLogger.E(Tag, string.Format("Sending power to motor failed: {0}", error.GetErrorMessage()));
		}

		private int ConvertUnsignedMotorPowerToSigned(MotorDirection direction, int power) {
			var resultPower = Math.Min(Math.Max(power, MotorMinSpeed), MotorMaxSpeed);
			if (direction == MotorDirection.MotorDirectionLeft)
				resultPower = -resultPower;

			return resultPower;
		}


	}
}
