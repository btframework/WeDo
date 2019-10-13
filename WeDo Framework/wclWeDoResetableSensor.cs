using System;

using wclCommon;
using wclBluetooth;

namespace wclWeDoFramework
{
    /// <summary> The base class for WeDo sensors that can be reset. </summary>
    /// <seealso cref="wclWeDoIo"/>
    public abstract class wclWeDoResetableSensor : wclWeDoIo
    {
        /// <summary> Creates new device object. </summary>
        /// <param name="Hub"> The Hub object that owns the device. If this parameter is <c>null</c>
        ///   the <seealso cref="wclEInvalidArgument"/> exception raises. </param>
        /// <param name="ConnectionId"> The device's Connection ID. </param>
        /// <seealso cref="wclWeDoHub"/>
        /// <exception cref="wclEInvalidArgument"> The exception raises when the <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoResetableSensor(wclWeDoHub Hub, Byte ConnectionId)
            : base(Hub, ConnectionId)
        {
        }

        /// <summary> Resets the sensor. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 Reset()
        {
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            return base.ResetSensor();
        }
    };
}
