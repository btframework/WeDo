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
        private wclGattClient FClient;
        private Boolean FConnected;

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

            // First we have to read services from the connected device.
            // It is a bit stupid imp,ementation but this makes code clear.
            // Any way, once we use goNone flag the connection to the device wilkl be executed only once
            // and each next call to this method will use cached services list. So it should not be too
            // slow.
            wclGattService[] Services;
            Int32 Res = FClient.ReadServices(wclGattOperationFlag.goNone, out Services);
            if (Res != wclErrors.WCL_E_SUCCESS)
                return Res;

            foreach (wclGattService svc in Services)
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

            // Read characteristics from device. Once we use goNone flag the characteristics
            // will be cached at first read so it should not be too slow in case we will call
            // this method few times for the same service.
            wclGattCharacteristic[] Characteristics;
            Int32 Res = FClient.ReadCharacteristics(Service.Value, wclGattOperationFlag.goNone,
                out Characteristics);
            if (Res != wclErrors.WCL_E_SUCCESS)
                return Res;

            foreach (wclGattCharacteristic chr in Characteristics)
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

        internal Int32 Connect()
        {
            if (FConnected)
                return wclConnectionErrors.WCL_E_CONNECTION_ACTIVE;

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

            FConnected = false;
            return wclErrors.WCL_E_SUCCESS;
        }

        internal Boolean Connected { get { return FConnected; } }

        /// <summary> Creates new WeDo Service Client object. </summary>
        /// <param name="Client"> The <see cref="wclGattClient"/> object that handles the connection
        ///   to a WeDo device. </param>
        /// <exception cref="wclEInvalidArgument"> The exception raises if the <c>Client</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoService(wclGattClient Client)
        {
            if (Client == null)
                throw new wclEInvalidArgument("Client parameter can not be null.");

            FClient = Client;
            FConnected = false;
        }
    };
}
