using System;

using wclCommon;
using wclBluetooth;

namespace wclWeDoFramework
{
    /// <summary> The motor's direction. </summary>
    public enum wclWeDoMotorDirection
    {
        /// <summary> Drifting (Floating). </summary>
        mdDrifting = 0,
        /// <summary> Running left. </summary>
        mdLeft = 1,
        /// <summary> Running right. </summary>
        mdRight = 2,
        /// <summary> Brake. </summary>
        mdBraking = 3,
        /// <summary> Unknwon. </summary>
        mdUnknown = 255
    };

    /// <summary> The class represents a WeDo motor. </summary>
    /// <seealso cref="wclWeDoIo"/>
    public class wclWeDoMotor : wclWeDoIo
    {
        private const SByte MOTOR_POWER_DRIFT = 0;
        private const SByte MOTOR_POWER_BRAKE = 127;

        private const Byte MOTOR_MIN_SPEED = 1;
        private const Byte MOTOR_MAX_SPEED = 100;

        // Only send values in the range 35-100.
        // An offset is needed as values below 35 is not enough power to actually
        // make the motor turn.
        private const Byte MOTOR_POWER_OFFSET = 35;

        private wclWeDoMotorDirection FDirection;
        private SByte FPower;

        private Byte GetPower()
        {
            if (FDirection == wclWeDoMotorDirection.mdBraking || FDirection == wclWeDoMotorDirection.mdDrifting)
                return 0;
            return (Byte)Math.Abs(FPower);
        }

        private Int32 SendPower(SByte Power)
        {
            Int32 Res;
            if (Power == MOTOR_POWER_BRAKE || Power == MOTOR_POWER_DRIFT)
                Res = Hub.Io.WriteMotorPower(Power, ConnectionId);
            else
            {
                Byte Offset = MOTOR_POWER_OFFSET;
                if (FirmwareVersion == null || FirmwareVersion.MajorVersion == 0)
                    // On version 0.x of the firmware, PVM offset is handled in the firmware
                    Offset = 0;
                Res = Hub.Io.WriteMotorPower(Power, Offset, ConnectionId);
            }
            if (Res == wclErrors.WCL_E_SUCCESS)
                FPower = Power;

            return Res;
        }

        private SByte ConvertUnsignedMotorPowerToSigned(wclWeDoMotorDirection Direction, Byte Power)
        {
            SByte Res = (SByte)Math.Min(Math.Max(Power, MOTOR_MIN_SPEED), MOTOR_MAX_SPEED);
            if (Direction == wclWeDoMotorDirection.mdLeft)
                Res = (SByte)(-Res);
            return Res;
        }

        /// <summary> Creates new motor class object. </summary>
        /// <param name="Hub"> The Hub object that owns the device. If this parameter is <c>null</c>
        ///   the <seealso cref="wclEInvalidArgument"/> exception raises. </param>
        /// <param name="ConnectionId"> The device's Connection ID. </param>
        /// <seealso cref="wclWeDoHub"/>
        /// <exception cref="wclEInvalidArgument"> The exception raises when the <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoMotor(wclWeDoHub Hub, Byte ConnectionId)
            : base(Hub, ConnectionId)
        {
            FDirection = wclWeDoMotorDirection.mdBraking;
            FPower = 0;
        }

        /// <summary> Sends a command to stop (brake) the motor. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 Brake()
        {
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            Int32 Res = SendPower(MOTOR_POWER_BRAKE);
            if (Res == wclErrors.WCL_E_SUCCESS)
                FDirection = wclWeDoMotorDirection.mdBraking;
            return Res;
        }

        /// <summary> Sends a command to stop (drift/float) the motor. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 Drift()
        {
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            Int32 Res = SendPower(MOTOR_POWER_DRIFT);
            if (Res == wclErrors.WCL_E_SUCCESS)
                FDirection = wclWeDoMotorDirection.mdDrifting;
            return Res;
        }

        /// <summary> Sends a command to run the motor at a given <c>Power</c> in a given <c>Direction</c>.
        ///   The minimum speed is 0 and the maximum speed is 100. </summary>
        /// <param name="Direction"> The direction to run the motor. </param>
        /// <param name="Power"> The power to run the motor with. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seealso cref="wclWeDoMotorDirection"/>
        public Int32 Run(wclWeDoMotorDirection Direction, Byte Power)
        {
            if (Direction == wclWeDoMotorDirection.mdUnknown || Power > 100)
                return wclErrors.WCL_E_SUCCESS;

            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            if (Direction == wclWeDoMotorDirection.mdDrifting || Power == 0)
                return Drift();
            if (Direction == wclWeDoMotorDirection.mdBraking)
                return Brake();

            Int32 Res = SendPower(ConvertUnsignedMotorPowerToSigned(Direction, Power));
            if (Res == wclErrors.WCL_E_SUCCESS)
                FDirection = Direction;
            return Res;
        }

        /// <summary> Gets the current running direction of the motor. </summary>
        /// <value> Teh motor direction. </value>
        /// <seealso cref="wclWeDoMotorDirection"/>
        public wclWeDoMotorDirection Direction { get { return FDirection; } }
        /// <summary> Gets the motor's braking state. </summary>
        /// <value> <c>True</c> if the motor is in brake state. <c>False</c> otheerwise. </value>
        public Boolean IsBraking { get { return FPower == MOTOR_POWER_BRAKE; } }
        /// <summary> Gets the motor's drifting state. </summary>
        /// <value> <c>True</c> if the motor is currently drifting or floating.
        ///   When floating the motor axis can be turned without resistance. </value>
        public Boolean IsDrifting { get { return FPower == MOTOR_POWER_DRIFT; } }
        /// <summary> Gets the power the motor is currently running with (0 if braking or drifting). </summary>
        /// <value> Teh current motor power. </value>
        public Byte Power { get { return GetPower(); } }
    };
}
