using System;
using System.Windows.Forms;
using System.Collections.Generic;

using wclCommon;
using wclCommunication;
using wclBluetooth;

namespace WeDoScan
{
    public partial class fmMain : Form
    {
        private wclGattClient FClient;
        private wclBluetoothManager FManager;
        private wclBluetoothLeBeaconWatcher FWatcher;

        #region Helper functions
        private void Trace(String Msg)
        {
            lbLog.Items.Add(Msg);
        }

        private void Trace(String Msg, Int32 Error)
        {
            Trace(Msg + ": 0x" + Error.ToString("X8"));
        }

        private wclBluetoothRadio FindRadio()
        {
            wclBluetoothRadio Radio = null;

            if (FManager.Count == 0)
                Trace("Bluetooth hardware was not found.");
            else
            {
                for (Int32 i = 0; i < FManager.Count; i++)
                {
                    if (FManager[i].Available)
                    {
                        Radio = FManager[i];
                        break;
                    }
                }

                if (Radio == null)
                    Trace("Bluetooth hardware was found but is not available");
                else
                    Trace("Found " + Radio.ApiName + " Bluetooth radio.");
            }

            return Radio;
        }
        #endregion

        #region Main Form events
        private void FmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            FClient = null;
            FManager = null;
            FWatcher = null;
        }

        private void FmMain_Load(object sender, EventArgs e)
        {
            FClient = new wclGattClient();
            FClient.OnConnect += FClient_OnConnect;
            FClient.OnDisconnect += FClient_OnDisconnect;

            FManager = new wclBluetoothManager();
            FManager.AfterOpen += FManager_AfterOpen;
            FManager.BeforeClose += FManager_BeforeClose;

            FWatcher = new wclBluetoothLeBeaconWatcher();
            FWatcher.OnStarted += FWatcher_OnStarted;
            FWatcher.OnStopped += FWatcher_OnStopped;
            FWatcher.OnAdvertisementRawFrame += FWatcher_OnAdvertisementRawFrame;
            FWatcher.OnAdvertisementUuidFrame += FWatcher_OnAdvertisementUuidFrame;
        }
        #endregion

        #region GATT Client events
        private void FClient_OnConnect(object Sender, int Error)
        {
            if (Error != wclErrors.WCL_E_SUCCESS)
                Trace("GATT Client connection failed", Error);
            else
            {
                Trace("Connected to device");
                Trace("Read services");
                wclGattService[] Services;
                Int32 Res = FClient.ReadServices(wclGattOperationFlag.goNone, out Services);
                if (Res != wclErrors.WCL_E_SUCCESS)
                    Trace("Read services failed", Res);
                else
                {
                    if (Services == null || Services.Length == 0)
                        Trace("No primary services were found");
                    else
                    {
                        foreach (wclGattService Service in Services)
                        {
                            if (Service.Uuid.IsShortUuid)
                                Trace("  Service: " + Service.Uuid.ShortUuid.ToString("X4"));
                            else
                                Trace("  Service: " + Service.Uuid.LongUuid.ToString());

                            Trace("  Read characteristics");
                            wclGattCharacteristic[] Chars;
                            Res = FClient.ReadCharacteristics(Service, wclGattOperationFlag.goNone, out Chars);
                            if (Res != wclErrors.WCL_E_SUCCESS)
                                Trace("  Read characteristics failed", Res);
                            else
                            {
                                if (Chars != null && Chars.Length > 0)
                                {
                                    foreach (wclGattCharacteristic Char in Chars)
                                    {
                                        if (Char.Uuid.IsShortUuid)
                                            Trace("    Characteristic: " + Char.Uuid.ShortUuid.ToString("X4"));
                                        else
                                            Trace("    Characteristic: " + Char.Uuid.LongUuid.ToString());
                                        if (Char.IsBroadcastable)
                                            Trace("      IsBroadcastable");
                                        if (Char.IsIndicatable)
                                            Trace("      IsIndicatable");
                                        if (Char.IsNotifiable)
                                            Trace("      IsNotifiable");
                                        if (Char.IsReadable)
                                            Trace("      IsReadable");
                                        if (Char.IsSignedWritable)
                                            Trace("      IsSignedWritable");
                                        if (Char.IsWritable)
                                            Trace("      IsWritable");
                                        if (Char.IsWritableWithoutResponse)
                                            Trace("      IsWritableWithoutResponse");
                                    }
                                }
                            }
                        }
                    }
                }

                Trace("Disconnecting");
                Res = FClient.Disconnect();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    Trace("Disconnect failed", Res);
            }

            FManager.Close();
        }

        private void FClient_OnDisconnect(object Sender, int Reason)
        {
            Trace("GATT Client disconnected with reason", Reason);
        }
        #endregion

        #region Bluetooth Manager events
        private void FManager_AfterOpen(object sender, EventArgs e)
        {
            Trace("Bluetooth MNanager has been opened");

            wclBluetoothRadio Radio = FindRadio();
            if (Radio != null)
            {
                Int32 Res = FWatcher.Start(Radio);
                Radio = null;
                if (Res != wclErrors.WCL_E_SUCCESS)
                {
                    Trace("Start Beacon Watcher failed", Res);
                    FManager.Close();
                }
            }
        }

        private void FManager_BeforeClose(object sender, EventArgs e)
        {
            Trace("Bluetooth Manager is closing");

            if (FClient.State != wclClientState.csDisconnected && FClient.State != wclClientState.csDisconnecting)
            {
                Trace("Disconnecting from device");
                Int32 Res = FClient.Disconnect();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    Trace("Disconnect failed", Res);
            }

            if (FWatcher.Monitoring)
            {
                Trace("Stopping Beacon Wacther");

                Int32 Res = FWatcher.Stop();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    Trace("Beacon Watcher Stop failed", Res);
            }

            laStatus.Text = "Stopped";
        }
        #endregion

        #region Beacon Watcher events
        private void FWatcher_OnStarted(object sender, EventArgs e)
        {
            Trace("Beacon Watcher has been started.");
            laStatus.Text = "Running";
        }

        private void FWatcher_OnStopped(object sender, EventArgs e)
        {
            Trace("Beacon Watcher has been stopped");
        }

        private void FWatcher_OnAdvertisementRawFrame(object Sender, long Address, long Timestamp, sbyte Rssi, byte DataType, byte[] Data)
        {
            /*Trace("Advertisement frame received");
            Trace("  Address: " + Address.ToString("X12"));
            Trace("  DataType: " + DataType.ToString("X2"));
            if (Data == null || Data.Length == 0)
                Trace("  Data: N/A");
            else
            {
                Trace("  Data (" + Data.Length.ToString() + "):");
                String s = "";
                foreach (Byte b in Data)
                    s = s + b.ToString("X2");
                Trace("    " + s);
            }*/
        }

        private void FWatcher_OnAdvertisementUuidFrame(object Sender, long Address, long Timestamp, sbyte Rssi, Guid Uuid)
        {
            Trace("Advertisement UUID frame received");
            Trace("  Address: " + Address.ToString("X12"));
            Trace("  UUID: " + Uuid.ToString());

            //Guid PrimaryUUID = new Guid("{0000FFF1-0000-1000-8000-00805F9B34FB}");
            Guid PrimaryService1 = new Guid("{00001523-1212-efde-1523-785feabcd123}");
            Guid PrimaryService2 = new Guid("{00004F0E-1212-efde-1523-785feabcd123}");

            if (Uuid == PrimaryService1 || Uuid == PrimaryService2)
            {
                Trace("Primary service found!");
                if (Uuid == PrimaryService1)
                    Trace("  Found service 1");
                else
                    Trace("  Found service 2");

                wclBluetoothRadio Radio = FWatcher.Radio;

                Trace("Stop Beacon Watcher");
                Int32 Res = FWatcher.Stop();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    Trace("Stop Beacon Wacther failed", Res);
                else
                {
                    Trace("Try to connect");
                    FClient.Address = Address;
                    Res = FClient.Connect(Radio);
                    if (Res != wclErrors.WCL_E_SUCCESS)
                        Trace("Connect to device failed", Res);
                }

                if (Res != wclErrors.WCL_E_SUCCESS)
                    FManager.Close();

                Radio = null;
            }
        }
        #endregion

        #region Buttons
        private void BtStart_Click(object sender, EventArgs e)
        {
            if (FManager.Active)
                MessageBox.Show("Alsready running");
            else
            {
                lbLog.Items.Clear();
                Int32 Res = FManager.Open();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    Trace("Open Bluetooth Manager failed", Res);
            }
        }

        private void BtStop_Click(object sender, EventArgs e)
        {
            if (!FManager.Active)
                MessageBox.Show("Not running");
            else
            {
                Int32 Res = FManager.Close();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    Trace("Close Bluetooth Manager failed", Res);
            }
        }
        #endregion

        public fmMain()
        {
            InitializeComponent();
        }
    }
}
