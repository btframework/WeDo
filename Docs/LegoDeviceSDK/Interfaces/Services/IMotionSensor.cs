using LegoDeviceSDK.Generic.Data;
using LegoDeviceSDK.Interfaces;
using System;

namespace LegoDeviceSDK.Interfaces.Services {
    /// <summary>
    ///This service provides readings from an motion sensor (aka. detect sensor).
    ///Add a instance of a <see cref="IMotionSensorDelegate"/> using addDelegate: to be notified when a service receives an updated value.
    ///</summary>
    public interface IMotionSensor : IService {
        /// <summary>
        /// The most recent count reading from the sensor
        /// </summary>
        uint Count { get; }

        /// <summary>
        /// The most recent distance reading from the sensor
        /// </summary>
        float Distance { get; }

        /// <summary>
        /// The current mode of the motion sensor
        /// </summary>
        MotionSensorMode MotionSensorMode { get; set; }
    }
}
