using LegoDeviceSDK.Generic.Data;
using System;

namespace LegoDeviceSDK.Interfaces.Services {
    /// <summary>
    /// This service provides readings from a tilt sensor.
    ///Add a instance of a <see cref="ITiltSensorDelegate"/> using addDelegate to be notified when a service receives an updated value.
    /// </summary>
    public interface ITiltSensor : IService {
        /// <summary>
        /// The current mode of the tilt sensor
        /// </summary>
        TiltSensorMode TiltSensorMode { get; set; }

        /// <summary>
        /// The most recent angle reading from the sensor. The angle represents the angle the sensor is tilted in the x, y and z-<see cref="Direction"/>.
        /// </summary>
        // <remarks>If no angle reading has been received, of if the sensor is not in mode <see cref="LeTiltSensorMode.LeTiltSensorModeAngle"/> the value of this property will be LETiltSensorAngleZero</remarks>
        TiltSensorAngle Angle { get; }

        /// <summary>
        /// The most recent crash reading from the sensor. The value represents the number of times the sensor has been ‘bumped’ in the x, y, and z-<see cref="Direction"/>. The value can be reset by sending the <see cref="IService.SendResetStateRequest"/>.
        /// </summary>
        // <remarks>If no crash reading has been received, of if the sensor is not in mode <see cref="TiltSensorMode.TiltSensorModeCrash"/> the value of this property will be LETiltSensorCrashZero</remarks>
        TiltSensorCrash Crash { get; }

        /// <summary>
        /// The most recent direction reading from the sensor.
        /// </summary>
        /// <remarks>If no direction reading has been received, of if the sensor is not in mode <see cref="Generic.Data.TiltSensorMode.TiltSensorModeTilt"/> the value of this property will be <see cref="TiltSensorDirection.TiltSensorDirectionUnknown"/>.</remarks>
        TiltSensorDirection Direction { get; }
    }
}
