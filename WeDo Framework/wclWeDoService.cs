using System;
using System.Text;

using wclCommon;
using wclCommunication;
using wclBluetooth;

namespace wclWeDoFramework
{
    /// <summary> The base class for all WeDo services. </summary>
    public abstract class wclWeDoService
    {
        private wclGattCharacteristic[] FCharacteristics;
        private wclGattClient FClient;
        private Boolean FConnected;
        private wclWeDoHub FHub;
        private wclGattService[] FServices;

        private Int32 ReadCharacteristics(wclGattService Service)
        {
            // Did we already read the characteristics for given service?
            if (FCharacteristics != null)
                return wclErrors.WCL_E_SUCCESS;

            Int32 Res = FClient.ReadCharacteristics(Service, wclGattOperationFlag.goNone, out FCharacteristics);
            if (Res != wclErrors.WCL_E_SUCCESS)
                return Res;
            if (FCharacteristics == null)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;

            return wclErrors.WCL_E_SUCCESS;
        }

        /// <summary> Converts <see cref="wclGattUuid"/> type to standard system <see cref="Guid"/>. </summary>
        /// <param name="Uuid"> The <see cref="wclGattUuid"/> that should be converted. </param>
        /// <returns> The <see cref="Guid"/> composed from the <c>Uuid</c>. </returns>
        /// <seealso cref="Guid"/>
        /// <seealso cref="wclGattUuid"/>
        protected Guid ToGuid(wclGattUuid Uuid)
        {
            if (!Uuid.IsShortUuid)
                return Uuid.LongUuid;

            Byte[] Bytes = wclUUIDs.Bluetooth_Base_UUID.ToByteArray();
            Int16 Data2 = BitConverter.ToInt16(Bytes, 4);
            Int16 Data3 = BitConverter.ToInt16(Bytes, 6);
            Byte[] Data4 = new Byte[8];
            Buffer.BlockCopy(Bytes, 8, Data4, 0, 8);
            return new Guid(Uuid.ShortUuid, Data2, Data3, Data4);
        }

        /// <summary> Compares the attribute's <see cref="wclGattUuid"/> with given standard
        ///   system <see cref="Guid"/>. </summary>
        /// <param name="GattUuid"> The attribute's <see cref="wclGattUuid"/>. </param>
        /// <param name="Uuid">The system <see cref="Guid"/>. </param>
        /// <returns> Returns <c>true</c> if the attribute's UUID is equals to the GUID.
        ///   Returns <c>false</c> otherwise. </returns>
        /// <seealso cref="Guid"/>
        /// <seealso cref="wclGattUuid"/>
        protected Boolean CompareGuid(wclGattUuid GattUuid, Guid Uuid)
        {
            return (ToGuid(GattUuid) == Uuid);
        }

        /// <summary> Finds the service with given UUID. </summary>
        /// <param name="Uuid"> The service's UUID. </param>
        /// <param name="Service"> If the method completed with success the parameter
        ///   contains found service. Otherwise the parameter is <c>null</c>. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seealso cref="Guid"/>
        /// <seealso cref="wclGattService"/>
        protected Int32 FindService(Guid Uuid, out wclGattService? Service)
        {
            Service = null;

            if (FServices == null)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;

            foreach (wclGattService svc in FServices)
            {
                if (CompareGuid(svc.Uuid, Uuid))
                {
                    Service = svc;
                    return wclErrors.WCL_E_SUCCESS;
                }
            }

            return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;
        }

        /// <summary> Finds the characteristic with given UUID. </summary>
        /// <param name="Uuid"> The characteristic's UUID. </param>
        /// <param name="Service"> The GATT service that should contain the required
        ///   characteristic. </param>
        /// <param name="Characteristic"> If the method completed with success the
        ///   parameter contains the found characteristic. Otherwise the parameter is <c>null</c>. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seealso cref="Guid"/>
        /// <seealso cref="wclGattService"/>
        /// <seealso cref="wclGattCharacteristic"/>
        protected Int32 FindCharactersitc(Guid Uuid, wclGattService? Service,
            out wclGattCharacteristic? Characteristic)
        {
            Characteristic = null;

            if (Service == null)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;

            Int32 Res = ReadCharacteristics(Service.Value);
            if (Res != wclErrors.WCL_E_SUCCESS)
                return Res;

            foreach (wclGattCharacteristic chr in FCharacteristics)
            {
                if (CompareGuid(chr.Uuid, Uuid))
                {
                    Characteristic = chr;
                    return wclErrors.WCL_E_SUCCESS;
                }
            }

            return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;
        }

        /// <summary> Subscribes to the changes notifications of the given characteristic. </summary>
        /// <param name="Characteristic"> The characteristic. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seealso cref="wclGattCharacteristic"/>
        protected Int32 SubscribeForNotifications(wclGattCharacteristic? Characteristic)
        {
            if (Characteristic == null)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;

            // Windows does not support dual indicatable and notifiable chars so select one.
            wclGattCharacteristic chr = Characteristic.Value;
            if (chr.IsIndicatable && chr.IsNotifiable)
                chr.IsIndicatable = false;
            Int32 Res = FClient.Subscribe(chr);
            if (Res == wclErrors.WCL_E_SUCCESS)
            {
                Res = FClient.WriteClientConfiguration(chr, true, wclGattOperationFlag.goNone);
                if (Res != wclErrors.WCL_E_SUCCESS)
                    FClient.Unsubscribe(chr);
            }
            return Res;
        }

        /// <summary> Unsubscribes from the changes notifications of the given characteristic. </summary>
        /// <param name="Characteristic"> The characteristic to unsubsribe. </param>
        /// <seealso cref="wclGattCharacteristic"/>
        protected void UnsubscribeFromNotifications(wclGattCharacteristic? Characteristic)
        {
            if (FClient.State == wclClientState.csConnected && Characteristic != null)
            {
                wclGattCharacteristic chr = Characteristic.Value;
                if (chr.IsIndicatable && chr.IsNotifiable)
                    chr.IsIndicatable = false;
                FClient.WriteClientConfiguration(chr, false, wclGattOperationFlag.goNone);
                FClient.Unsubscribe(chr);
            }
        }

        /// <summary> Reads string value from the given characteristic. </summary>
        /// <param name="Characteristic"> The GATT characteristic. </param>
        /// <param name="Value"> If the method completed with success contains the read value. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        protected Int32 ReadStringValue(wclGattCharacteristic? Characteristic, out String Value)
        {
            Value = "";

            if (!FConnected)
                return wclConnectionErrors.WCL_E_CONNECTION_NOT_ACTIVE;
            if (Characteristic == null)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;

            Byte[] CharValue;
            Int32 Res = FClient.ReadCharacteristicValue(Characteristic.Value, wclGattOperationFlag.goNone,
                out CharValue);
            if (Res == wclErrors.WCL_E_SUCCESS)
            {
                if (CharValue != null && CharValue.Length > 0)
                    Value = Encoding.UTF8.GetString(CharValue);
            }
            return Res;
        }

        /// <summary> Reads byte value from the given characteristic. </summary>
        /// <param name="Characteristic"> The GATT characteristic. </param>
        /// <param name="Value"> If the method completed with success contains the read value. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        protected Int32 ReadByteValue(wclGattCharacteristic? Characteristic, out Byte Value)
        {
            Value = 0;

            if (!FConnected)
                return wclConnectionErrors.WCL_E_CONNECTION_NOT_ACTIVE;
            if (Characteristic == null)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;

            Byte[] CharValue;
            Int32 Res = FClient.ReadCharacteristicValue(Characteristic.Value, wclGattOperationFlag.goNone,
                out CharValue);
            if (Res == wclErrors.WCL_E_SUCCESS)
            {
                if (CharValue != null && CharValue.Length > 0)
                    Value = CharValue[0];
            }
            return Res;
        }

        /// <summary> Initializes the WeDo service. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <remarks> A derived clases must override this method to initialize all required
        ///   parameters to work with WeDo service. </remarks>
        protected abstract Int32 Initialize();

        /// <summary> Uninitializes the WeDo service. </summary>
        /// <remarks> A derived clases must override this method to cleanup allocated resources. </remarks>
        protected abstract void Uninitialize();

        /// <summary> Gets the GATT client object. </summary>
        /// <value> The GATT client object. </value>
        /// <seealso cref="wclGattClient"/>
        protected internal wclGattClient Client { get { return FClient; } }

        /// <summary> This method called internally by the <see cref="wclWeDoHub"/>
        ///   to notify about characteristic changes. A derived class may override this method
        ///   to check for required characteristic changes. </summary>
        /// <param name="Handle"> The characteristic handle. </param>
        /// <param name="Value"> The new characteristic value. </param>
        internal virtual void CharacteristicChanged(UInt16 Handle, Byte[] Value)
        {
            // Do nothing in default implementation.
        }

        internal Int32 Connect(wclGattService[] Services)
        {
            if (FConnected)
                return wclConnectionErrors.WCL_E_CONNECTION_ACTIVE;
            if (Services == null)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;

            FServices = Services;
            Int32 Res = Initialize();
            if (Res == wclErrors.WCL_E_SUCCESS)
                FConnected = true;
            return Res;
        }

        internal Int32 Disconnect()
        {
            if (!FConnected)
                return wclConnectionErrors.WCL_E_CONNECTION_NOT_ACTIVE;

            Uninitialize();

            FCharacteristics = null;
            FConnected = false;
            FServices = null;

            return wclErrors.WCL_E_SUCCESS;
        }

        internal Boolean Connected { get { return FConnected; } }

        /// <summary> Creates new WeDo Service Client object. </summary>
        /// <param name="Client"> The <see cref="wclGattClient"/> object that handles the connection
        ///   to a WeDo device. </param>
        /// <param name="Hub"> The <see cref="wclWeDoHub"/> object that owns the service. </param>
        /// <exception cref="wclEInvalidArgument"> The exception raises if the <c>Client</c> or <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoService(wclGattClient Client, wclWeDoHub Hub)
        {
            if (Client == null || Hub == null)
                throw new wclEInvalidArgument("Client parameter can not be null.");

            FCharacteristics = null;
            FClient = Client;
            FConnected = false;
            FHub = Hub;
            FServices = null;
        }

        /// <summary> Gets the <see cref="wclWeDoHub"/> object that owns the service. </summary>
        /// <value> The <see cref="wclWeDoHub"/> object. </value>
        /// <seealso cref="wclWeDoHub"/>
        public wclWeDoHub Hub { get { return FHub; } }
    };
}
