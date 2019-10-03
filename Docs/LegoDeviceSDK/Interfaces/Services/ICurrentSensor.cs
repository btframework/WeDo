using LegoDeviceSDK.Interfaces;
using System;

namespace LegoDeviceSDK.Interfaces.Services {
    /// <summary>
    /// This service provides current (<see cref="MilliAmp"/>) readings for the battery on the device. Add a instance of a LECurrentSensorDelegate using addDelegate: to be notified when a service receives an updated value.
    /// </summary>
    public interface ICurrentSensor : IService {
        /// <summary>
        /// The battery current in milli amps
        /// </summary>
        float MilliAmp { get; }
    }
}
