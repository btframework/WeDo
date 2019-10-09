using System;
using System.Text;

using wclCommon;
using wclCommunication;
using wclBluetooth;

namespace wclWeDoFramework
{
    /// <summary> The <c>OnButtonStateChanged</c> event handler prototype. </summary>
    /// <param name="Sender"> The object that fires the event. </param>
    /// <param name="Pressed"> The button's state. <c>True</c> if button has been pressed.
    ///   <c>False</c> if button has been released. </param>
    public delegate void wclWeDoHubButtonStateChangedEvent(Object Sender, Boolean Pressed);
    /// <summary> The <c>OnLowVoltageAlert</c> event handler prototype. </summary>
    /// <param name="Sender"> The object that fires the event. </param>
    /// <param name="Alert"> <c>True</c> if device runs on low battery. <c>False</c> otherwise. </param>
    public delegate void wclWeDoHubLowVolatgeAlertEvent(Object Sender, Boolean Alert);

    /// <summary> The class represents the WeDo Hub service. </summary>
    /// <seealso cref="wclWeDoService"/>
    public class wclWeDoHubService : wclWeDoService
    {
        // WeDo HUB service.
        private static Guid WEDO_SERVICE_HUB = new Guid("00001523-1212-efde-1523-785feabcd123");

        // Device name characteristic. [Mandatory] [Readable, Writable]
        private static Guid WEDO_CHARACTERISTIC_DEVICE_NAME = new Guid("00001524-1212-efde-1523-785feabcd123");
        // Buttons state characteristic. [Mandatory] [Readable, Notifiable]
        private static Guid WEDO_CHARACTERISTIC_BUTTON_STATE = new Guid("00001526-1212-efde-1523-785feabcd123");
        // IO attached charactrisitc. [Mandatory] [Notifiable]
        private static Guid WEDO_CHARACTERISTIC_IO_ATTACHED = new Guid("00001527-1212-efde-1523-785feabcd123");
        // Low Voltrage Alert characteristic. [Mandatory] [Readable, Notifiable]
        private static Guid WEDO_CHARACTERISTIC_LOW_VOLTAGE_ALERT = new Guid("00001528-1212-efde-1523-785feabcd123");
        // High Current Aleart characteristic. [Optional] [Readable, Notifiable]
        private static Guid WEDO_CHARACTERISTIC_HIGH_CURRENT_ALERT = new Guid("00001529-1212-efde-1523-785feabcd123");
        // Low Signal Aleart characteristic. [Optional] [Readable, Notifiable]
        private static Guid WEDO_CHARACTERISTIC_LOW_SIGNAL_ALERT = new Guid("0000152a-1212-efde-1523-785feabcd123");
        // Turn Off command characteristic. [Mandatory] [Writable]
        private static Guid WEDO_CHARACTERISTIC_TURN_OFF = new Guid("0000152b-1212-efde-1523-785feabcd123");
        // VCC Port Control characteristic. [Optional] [Readable, Writable]
        private static Guid WEDO_CHARACTERISTIC_VCC_PORT_CONTROL = new Guid("0000152c-1212-efde-1523-785feabcd123");
        // Battery Type characteristic. [Optional] [Readable]
        private static Guid WEDO_CHARACTERISTIC_BATTERY_TYPE = new Guid("0000152d-1212-efde-1523-785feabcd123");
        // Device Disconnect Cpmmand characteristic. [Optional] [Writable]
        private static Guid WEDO_CHARACTERISTIC_DEVICE_DISCONNECT = new Guid("0000152e-1212-efde-1523-785feabcd123");

        private wclGattCharacteristic? FDeviceNameChar;
        private wclGattCharacteristic? FButtonStateChar;
        private wclGattCharacteristic? FIoAttachedChar;
        private wclGattCharacteristic? FLowVoltageAlertChar;
        private wclGattCharacteristic? FHighCurrentAleartChar;
        private wclGattCharacteristic? FLowSignalChar;
        private wclGattCharacteristic? FTurnOffChar;
        private wclGattCharacteristic? FVccPortChar;
        private wclGattCharacteristic? FBatteryTypeChar;
        private wclGattCharacteristic? FDeviceDisconnectChar;

        internal Int32 ReadDeviceName(out String Name)
        {
            return ReadStringValue(FDeviceNameChar, out Name);
        }

        internal Int32 WriteDeviceName(String Name)
        {
            if (Name == null || Name == "")
                return wclErrors.WCL_E_INVALID_ARGUMENT;

            if (!Connected)
                return wclConnectionErrors.WCL_E_CONNECTION_NOT_ACTIVE;

            if (Name.Length > 20)
                Name = Name.Substring(0, 20);
            Byte[] Bytes = Encoding.UTF8.GetBytes(Name);
            Byte[] CharVal;
            if (Bytes.Length < 20)
            {
                CharVal = new Byte[20];
                for (Int32 i = 0; i < Bytes.Length; i++)
                    CharVal[i] = Bytes[i];
                for (Int32 i = Bytes.Length; i < 20; i++)
                    CharVal[i] = 0;
            }
            else
                CharVal = Bytes;

            return Client.WriteCharacteristicValue(FDeviceNameChar.Value, CharVal);
        }

        internal event wclWeDoHubButtonStateChangedEvent OnButtonStateChanged;
        internal event wclWeDoHubLowVolatgeAlertEvent OnLowVoltageAlert;

        /// <summary> Initializes the WeDo service. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        protected override Int32 Initialize()
        {
            // Find Battery Level service and its characteristics.
            wclGattService? Service;
            Int32 Res = FindService(WEDO_SERVICE_HUB, out Service);
            if (Res == wclErrors.WCL_E_SUCCESS)
            {
                // The following characteristics are important so we have to check if they are present.
                Res = FindCharactersitc(WEDO_CHARACTERISTIC_DEVICE_NAME, Service, out FDeviceNameChar);
                if (Res == wclErrors.WCL_E_SUCCESS)
                    Res = FindCharactersitc(WEDO_CHARACTERISTIC_BUTTON_STATE, Service, out FButtonStateChar);
                if (Res == wclErrors.WCL_E_SUCCESS)
                    Res = FindCharactersitc(WEDO_CHARACTERISTIC_IO_ATTACHED, Service, out FIoAttachedChar);
                if (Res == wclErrors.WCL_E_SUCCESS)
                    Res = FindCharactersitc(WEDO_CHARACTERISTIC_LOW_VOLTAGE_ALERT, Service, out FLowVoltageAlertChar);
                if (Res == wclErrors.WCL_E_SUCCESS)
                    Res = FindCharactersitc(WEDO_CHARACTERISTIC_TURN_OFF, Service, out FTurnOffChar);

                // Subscribe for mandatory chars.
                if (Res == wclErrors.WCL_E_SUCCESS)
                {
                    Res = SubscribeForNotifications(FButtonStateChar);
                    if (Res == wclErrors.WCL_E_SUCCESS)
                    {
                        Res = SubscribeForNotifications(FIoAttachedChar);
                        if (Res == wclErrors.WCL_E_SUCCESS)
                        {
                            Res = SubscribeForNotifications(FLowVoltageAlertChar);
                            if (Res != wclErrors.WCL_E_SUCCESS)
                                UnsubscribeFromNotifications(FIoAttachedChar);
                        }
                        if (Res != wclErrors.WCL_E_SUCCESS)
                            UnsubscribeFromNotifications(FButtonStateChar);
                    }
                }

                // The following characterisitcs may not be available so we ignore any errors. However we must check
                // that previous characteristics (which are important) have been found.
                if (Res == wclErrors.WCL_E_SUCCESS)
                {
                    FindCharactersitc(WEDO_CHARACTERISTIC_HIGH_CURRENT_ALERT, Service, out FHighCurrentAleartChar);
                    FindCharactersitc(WEDO_CHARACTERISTIC_LOW_SIGNAL_ALERT, Service, out FLowSignalChar);
                    FindCharactersitc(WEDO_CHARACTERISTIC_VCC_PORT_CONTROL, Service, out FVccPortChar);
                    FindCharactersitc(WEDO_CHARACTERISTIC_BATTERY_TYPE, Service, out FBatteryTypeChar);
                    FindCharactersitc(WEDO_CHARACTERISTIC_DEVICE_DISCONNECT, Service, out FDeviceDisconnectChar);

                    // Subscribe for optional chars.
                    if (FHighCurrentAleartChar != null)
                        Res = SubscribeForNotifications(FHighCurrentAleartChar);
                    if (Res == wclErrors.WCL_E_SUCCESS && FLowSignalChar != null)
                    {
                        Res = SubscribeForNotifications(FLowSignalChar);
                        if (Res != wclErrors.WCL_E_SUCCESS && FHighCurrentAleartChar != null)
                            UnsubscribeFromNotifications(FHighCurrentAleartChar);
                    }
                }

                if (Res != wclErrors.WCL_E_SUCCESS)
                {
                    UnsubscribeFromNotifications(FButtonStateChar);
                    UnsubscribeFromNotifications(FIoAttachedChar);
                    UnsubscribeFromNotifications(FLowVoltageAlertChar);

                    Uninitialize();
                }
            }
            return Res;
        }

        /// <summary> Uninitializes the WeDo service. </summary>
        protected override void Uninitialize()
        {
            if (Connected)
            {
                // Mandatory characteristics.
                UnsubscribeFromNotifications(FButtonStateChar);
                UnsubscribeFromNotifications(FIoAttachedChar);
                UnsubscribeFromNotifications(FLowVoltageAlertChar);

                // Optional characteristics.
                if (FHighCurrentAleartChar != null)
                    UnsubscribeFromNotifications(FHighCurrentAleartChar);
                if (FLowSignalChar != null)
                    UnsubscribeFromNotifications(FLowSignalChar);
            }

            FDeviceNameChar = null;
            FButtonStateChar = null;
            FIoAttachedChar = null;
            FLowVoltageAlertChar = null;
            FHighCurrentAleartChar = null;
            FLowSignalChar = null;
            FTurnOffChar = null;
            FVccPortChar = null;
            FBatteryTypeChar = null;
            FDeviceDisconnectChar = null;
        }

        /// <summary> This method called internally by the <see cref="wclWeDoHub"/>
        ///   to notify about characteristic changes. A derived class may override this method
        ///   to check for required characteristic changes. </summary>
        /// <param name="Handle"> The characteristic handle. </param>
        /// <param name="Value"> The new characteristic value. </param>
        internal override void CharacteristicChanged(UInt16 Handle, Byte[] Value)
        {
            // Process data only if it presents.
            if (Value != null && Value.Length > 0)
            {
                // Button pressed?
                if (FButtonStateChar != null && Handle == FButtonStateChar.Value.Handle)
                    DoButtonStateChanged(Value[0] == 1);
                // Low voltage?
                if (FLowVoltageAlertChar != null && Handle == FLowVoltageAlertChar.Value.Handle)
                    DoLowVoltageAlert(Value[0] == 1);
            }
        }

        /// <summary> Fires the <c>OnButtonStateChanged</c> event. </summary>
        /// <param name="Pressed"> <c>True</c> if the button has been pressed. <c>False</c> if the
        ///   button has been released. </param>
        protected virtual void DoButtonStateChanged(Boolean Pressed)
        {
            if (OnButtonStateChanged != null)
                OnButtonStateChanged(this, Pressed);
        }

        /// <summary> Fires the <c>OnLowVoltageAlert</c> event. </summary>
        /// <param name="Alert"> <c>True</c> if device runs on low battery. <c>False</c> otherwise. </param>
        protected virtual void DoLowVoltageAlert(Boolean Alert)
        {
            if (OnLowVoltageAlert != null)
                OnLowVoltageAlert(this, Alert);
        }

        /// <summary> Creates new IO service client. </summary>
        /// <param name="Client"> The <see cref="wclGattClient"/> object that handles the connection
        ///   to a WeDo device. </param>
        /// <param name="Hub"> The <see cref="wclWeDoHub"/> object that owns the service. </param>
        /// <exception cref="wclEInvalidArgument"> The exception raises if the <c>Client</c> or <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoHubService(wclGattClient Client, wclWeDoHub Hub)
            : base(Client, Hub)
        {
            OnButtonStateChanged = null;

            Uninitialize();
        }
    };
}
