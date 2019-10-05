using System;
using System.Windows.Forms;

using wclCommon;
using wclBluetooth;
using wclWeDo;

namespace WeDoControl
{
    public partial class fmMain : Form
    {
        private wclWeDoWatcher FWatcher;
        private wclWeDoClient FClient;
        private wclBluetoothRadio FRadio;
        private wclBluetoothManager FManager;

        private void Cleanup()
        {
            laState.Text = "Disconnected";

            btDisconnect.Enabled = false;
            btConnect.Enabled = true;
            btDevInfo.Enabled = false;

            pbBatt.Value = 0;
            laBattPercent.Text = "0%";

            FRadio = null;

            FManager.Close();
        }

        private void UpdateBatteryLevel(Byte Level)
        {
            pbBatt.Value = Level;
            laBattPercent.Text = Level.ToString() + "%";
        }

        private void ReadBattLevel()
        {
            Byte Level;
            Int32 Res = FClient.BatteryLevel.ReadBatteryLevel(out Level);
            if (Res == wclErrors.WCL_E_SUCCESS)
                UpdateBatteryLevel(Level);
        }

        private void WatcherDeviceFound(object Sender, long Address)
        {
            // Once device found we have to stop watcher and try to connect to the just found device.
            FWatcher.Stop();
            // Do not forget to stop the timer.
            Timer.Stop();
            // Now try to connect to just found device.
            Int32 Res = FClient.Connect(FRadio, Address);
            if (Res != wclErrors.WCL_E_SUCCESS)
            {
                Cleanup();
                MessageBox.Show("Connect failed with error: 0x" + Res.ToString("X8"));
            }
            else
                laState.Text = "Connecting...";
        }

        private void ClientConnected(object Sender, int Error)
        {
            if (Error != wclErrors.WCL_E_SUCCESS)
            {
                Cleanup();
                MessageBox.Show("Connect failed with error: 0x" + Error.ToString("X8"));
            }
            else
            {
                btDisconnect.Enabled = true;
                btDevInfo.Enabled = true;

                laState.Text = "Connected";
                ReadBattLevel();
            }
        }

        private void ClientDisconnected(object Sender, int Reason)
        {
            Cleanup();
        }

        private void BatteryLevelChanged(object Sender, byte Level)
        {
            UpdateBatteryLevel(Level);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Looks like device was not found. So terminate searching and release resources.
            Timer.Stop();
            FWatcher.Stop();
            Cleanup();

            MessageBox.Show("No WeDo device was found");
        }

        private void FmMain_Load(object sender, EventArgs e)
        {
            FWatcher = new wclWeDoWatcher();
            FWatcher.OnDeviceFound += WatcherDeviceFound;

            FClient = new wclWeDoClient();
            FClient.OnConnected += ClientConnected;
            FClient.OnDisconnected += ClientDisconnected;
            FClient.BatteryLevel.OnBatteryLevelChanged += BatteryLevelChanged;

            FManager = new wclBluetoothManager();

            FRadio = null;
        }

        private void FmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            FWatcher.Stop();
            FWatcher = null;

            FClient.Disconnect();
            FClient = null;

            FManager.Close();
            FManager = null;
        }

        private void BtConnect_Click(object sender, EventArgs e)
        {
            Int32 Res = FManager.Open();
            if (Res != wclErrors.WCL_E_SUCCESS)
                MessageBox.Show("Open BluetoothManager failed with error: 0x" + Res.ToString("X8"));
            else
            {
                if (FManager.Count == 0)
                    MessageBox.Show("No Bluetooth Hardware was found");
                else
                {
                    for (Int32 i = 0; i < FManager.Count; i++)
                    {
                        if (FManager[i].Available)
                        {
                            FRadio = FManager[i];
                            break;
                        }
                    }

                    if (FRadio == null)
                        MessageBox.Show("No available Bluetooth Radio found.");
                    else
                    {
                        Res = FWatcher.Start(FRadio);
                        if (Res != wclErrors.WCL_E_SUCCESS)
                        {
                            MessageBox.Show("Failed to scan WeDo devices: 0x" + Res.ToString("X8"));
                            FRadio = null;
                        }
                        else
                        {
                            laState.Text = "Searching...";
                            btConnect.Enabled = false;
                            Timer.Start();
                        }
                    }
                }

                if (FRadio == null)
                    FManager.Close();
            }
        }

        private void BtDisconnect_Click(object sender, EventArgs e)
        {
            FClient.Disconnect();
        }

        private void BtDevInfo_Click(object sender, EventArgs e)
        {
            fmDevInfo DevInfo = new fmDevInfo(FClient);
            DevInfo.ShowDialog(this);
            DevInfo.Dispose();
        }

        public fmMain()
        {
            InitializeComponent();
        }
    }
}
