using System;

using wclCommon;

namespace wclWeDoFramework
{
    /// <summary> The class represents a WeDo Hub current sensor. </summary>
    /// <seealso cref="wclWeDoIo"/>
    public class wclWeDoCurrentSensor : wclWeDoIo
    {
        private float GetCurrent()
        {
            if (InputFormat == null || InputFormat.Mode != 0 || InputFormat.Unit != wclWeDoSensorDataUnit.suSi)
                return 0f;

            return AsFloat;
        }

        /// <summary> The method called when data value has been changed. </summary>
        protected override void ValueChanged()
        {
            DoCurrentChanged();
        }

        /// <summary> Fires the <c>OnCurrentChanged</c> event. </summary>
        protected virtual void DoCurrentChanged()
        {
            if (OnCurrentChanged != null)
                OnCurrentChanged(this, EventArgs.Empty);
        }

        /// <summary> Creates new current sensor device object. </summary>
        /// <param name="Hub"> The Hub object that owns the device. If this parameter is <c>null</c>
        ///   the <seealso cref="wclEInvalidArgument"/> exception raises. </param>
        /// <param name="ConnectionId"> The device's Connection ID. </param>
        /// <seealso cref="wclWeDoHub"/>
        /// <exception cref="wclEInvalidArgument"> The exception raises when the <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoCurrentSensor(wclWeDoHub Hub, Byte ConnectionId)
            : base(Hub, ConnectionId)
        {
            DefaultInputFormat = new wclWeDoInputFormat(ConnectionId, wclWeDoIoDeviceType.iodCurrentSensor, 0, 30,
                wclWeDoSensorDataUnit.suSi, true, 0, 1);

            OnCurrentChanged = null;
        }

        /// <summary> Gets the battery current in mA. </summary>
        /// <value> The current in milli ampers. </value>
        public float Current {  get { return GetCurrent(); } }

        /// <summary> The event fires when current has been changed. </summary>
        /// <seealso cref="EventHandler"/>
        public event EventHandler OnCurrentChanged;
    };
}
