using System;

namespace LegoDeviceSDK.Interfaces.Services {
    /// <summary>
    /// This service provides voltage (<see cref="MilliVolts"/>) readings for the battery on the device. Add a instance of a <see cref="IVoltageSensorDelegate"/> using addDelegate to be notified when a service receives an updated value.
    /// </summary>
    public interface IVoltageSensor : IService {
        /// <summary>
        /// The battery voltage in milli volts
        /// </summary>
        float MilliVolts { get; }
    }
}
