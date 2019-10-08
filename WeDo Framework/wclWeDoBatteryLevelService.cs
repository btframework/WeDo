using System;

using wclCommon;
using wclCommunication;
using wclBluetooth;

namespace wclWeDoFramework
{
    /// <summary> The <c>OnBatteryLevelChanged</c> event handler prototype. </summary>
    /// <param name="Sender"> The object that fired the event. </param>
    /// <param name="Level"> The current battery level in percents in range 0-100. </param>
    public delegate void wclBatteryLevelChangedEvent(Object Sender, Byte Level);

    /// <summary> The class represents the WeDo Battery Level service. </summary>
    /// <seealso cref="wclWeDoService"/>
    public class wclWeDoBatteryLevelService : wclWeDoService
    {
        // Standard Bluetooth LE Battery Level Service.
        private static Guid WEDO_SERVICE_BATTERY_LEVEL = new Guid("0000180f-0000-1000-8000-00805f9b34fb");

        // Battery level characteristic. [Mandatory] [Readable, Notifiable]
        private static Guid WEDO_CHARACTERISTIC_BATTERY_LEVEL = new Guid("00002a19-0000-1000-8000-00805f9b34fb");

        private wclGattCharacteristic? FBatteryLevelChar;

        /// <summary> Fires the <c>OnBatteryLevelChanged</c> event. </summary>
        /// <param name="Level"> The current battery level in percents in range 0-100. </param>
        protected virtual void DoBatteryLevelChanged(Byte Level)
        {
            if (OnBatteryLevelChanged != null)
                OnBatteryLevelChanged(this, Level);
        }

        /// <summary> Initializes the WeDo service. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        protected override Int32 Initialize()
        {
            // Find Battery Level service and its characteristics.
            wclGattService? Service;
            Int32 Res = FindService(WEDO_SERVICE_BATTERY_LEVEL, out Service);
            if (Res == wclErrors.WCL_E_SUCCESS)
            {
                Res = FindCharactersitc(WEDO_CHARACTERISTIC_BATTERY_LEVEL, Service, out FBatteryLevelChar);
                if (Res == wclErrors.WCL_E_SUCCESS)
                {
                    Res = SubscribeForNotifications(FBatteryLevelChar);
                    if (Res != wclErrors.WCL_E_SUCCESS)
                        Uninitialize();
                }
            }
            return Res;
        }

        /// <summary> Uninitializes the WeDo service. </summary>
        protected override void Uninitialize()
        {
            if (Connected)
                UnsubscribeFromNotifications(FBatteryLevelChar);

            FBatteryLevelChar = null;
        }

        /// <summary> This method called internally by the <see cref="wclWeDoHub"/>
        ///   to notify about characteristic changes. A derived class may override this method
        ///   to check for required characteristic changes. </summary>
        /// <param name="Handle"> The characteristic handle. </param>
        /// <param name="Value"> The new characteristic value. </param>
        internal override void CharacteristicChanged(UInt16 Handle, Byte[] Value)
        {
            // We have to process battery level changes notifications here.
            if (Handle == FBatteryLevelChar.Value.Handle)
                DoBatteryLevelChanged(Value[0]);
        }

        /// <summary> Creates new Battery Level service client. </summary>
        /// <param name="Client"> The <see cref="wclGattClient"/> object that handles the connection
        ///   to a WeDo device. </param>
        /// <exception cref="wclEInvalidArgument"> The exception raises if the <c>Client</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoBatteryLevelService(wclGattClient Client)
            : base(Client)
        {
            Uninitialize();

            OnBatteryLevelChanged = null;
        }

        /// <summary> Reads the device's battery level. </summary>
        /// <param name="Level"> the current battery level in percents. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 ReadBatteryLevel(out Byte Level)
        {
            return ReadByteValue(FBatteryLevelChar, out Level);
        }

        /// <summary> The event fires when the battery level has been changed. </summary>
        /// <seealso cref="wclBatteryLevelChangedEvent" />
        public event wclBatteryLevelChangedEvent OnBatteryLevelChanged;
    };
}
