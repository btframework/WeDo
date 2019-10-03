using LegoDeviceSDK.Generic.Data;
using System;

namespace LegoDeviceSDK.Interfaces.Services {
    /// <summary>
    /// This service allows for controlling a simple motor
    /// </summary>
    public interface IMotor : IService {
        /// <summary>
        /// The current running direction of the motor
        /// </summary>
        MotorDirection Direction { get; set; }

        /// <summary>
        /// The power the motor is currently running with (0 if braking or drifting).
        /// </summary>
        int Power { get; }

        /// <summary>
        /// YES if the motor is currently braking (not running)
        /// </summary>
        bool isBraking { get; }

        /// <summary>
        /// YES if the motor is currently drifting / floating. When floating the motor axis can be turned without resistance.
        /// </summary>
        bool isDrifting { get; }

        /// <summary>
        /// Send a command to stop (brake) the motor
        /// </summary>
        void Brake();

        /// <summary>
        /// Send a command to stop (drift/float) the motor
        /// </summary>
        void Drift();

        /// <summary>
        /// Send a command to run the motor at a given <paramref name="power"/> in a given <paramref name="direction"/>. The minimum speed is 0 and the maximum speed is 100.
        /// </summary>
        /// <param name="direction">The direction to run the motor</param>
        /// <param name="power">The power to run the motor with.</param>
        void RunInDirection(LegoDeviceSDK.Generic.Data.MotorDirection direction, int power);
    }
}
