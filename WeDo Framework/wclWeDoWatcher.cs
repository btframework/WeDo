using System;
using System.Collections.Generic;

using wclCommon;
using wclBluetooth;

namespace wclWeDo
{
    /// <summary> The <c>OnDeviceFound</c> event handler prototype. </summary>
    /// <param name="Sender"> The object that fired the event. </param>
    /// <param name="Address"> The WeDo device's MAC address. </param>
    public delegate void wclWeDoDeviceFoundEvent(Object Sender, Int64 Address);

    /// <summary> The class used to search WeDo devices. </summary>
    public class wclWeDoWatcher
    {
        // WeDo advertises this service UUID so we look for it to identify WeDo device.
        private static Guid WEDO_ADVERTISING_SERVICE = new Guid("00001523-1212-efde-1523-785feabcd123");

        private wclBluetoothLeBeaconWatcher FWatcher;
        private List<Int64> FDevices;

        private void WatcherAdvertisementUuidFrame(object Sender, long Address, long Timestamp, sbyte Rssi, Guid Uuid)
        {
            if (Uuid == WEDO_ADVERTISING_SERVICE)
            {
                if (!FDevices.Contains(Address))
                {
                    DoDeviceFound(Address);
                    FDevices.Add(Address);
                }
            }
        }

        private void WatcherStarted(object sender, EventArgs e)
        {
            DoStarted();
        }

        private void WatcherStopped(object sender, EventArgs e)
        {
            DoStopped();
        }

        /// <summary> Fires the <c>OnDeviceFound</c> event. </summary>
        /// <param name="Address"> The device's MAC. </param>
        protected virtual void DoDeviceFound(Int64 Address)
        {
            if (OnDeviceFound != null)
                OnDeviceFound(this, Address);
        }

        /// <summary> Fires the <c>OnStarte</c> event. </summary>
        protected virtual void DoStarted()
        {
            if (OnStarted != null)
                OnStarted(this, EventArgs.Empty);
        }

        /// <summary> Fires the <c>OnStopped</c> event. </summary>
        protected virtual void DoStopped()
        {
            if (OnStopped != null)
                OnStopped(this, EventArgs.Empty);
        }

        /// <summary> Creates new WeDo Watcher object. </summary>
        public wclWeDoWatcher()
        {
            FDevices = new List<Int64>();

            FWatcher = new wclBluetoothLeBeaconWatcher();
            FWatcher.OnAdvertisementUuidFrame += WatcherAdvertisementUuidFrame;
            FWatcher.OnStarted += WatcherStarted;
            FWatcher.OnStopped += WatcherStopped;

            OnDeviceFound = null;
            OnStarted = null;
            OnStopped = null;
        }

        /// <summary> Starts watching (discovering) for WeDo devices. </summary>
        /// <param name="Radio"> The <see cref="wclBluetoothRadio" /> object that should be used
        ///   for executing Bluetooth LE discovering. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seealso cref="wclBluetoothRadio" />
        public Int32 Start(wclBluetoothRadio Radio)
        {
            return FWatcher.Start(Radio);
        }

        /// <summary> Stops discovering WeDo devices. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 Stop()
        {
            return FWatcher.Stop();
        }

        /// <summary> The event fires when new WeDo device has been found. </summary>
        /// <seealso cref="wclWeDoDeviceFoundEvent" />
        public event wclWeDoDeviceFoundEvent OnDeviceFound;
        /// <summary> The event fires when discovering has been started. </summary>
        /// <seealso cref="EventHandler" />
        public event EventHandler OnStarted;
        /// <summary> The event fires when discovering has been stopped. </summary>
        /// <seealso cref="EventHandler" />
        public event EventHandler OnStopped;
    };
}
