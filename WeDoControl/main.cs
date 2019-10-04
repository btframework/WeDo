using System;
using System.Windows.Forms;

using wclCommon;
using wclBluetooth;
using WeDo;

namespace WeDoControl
{
    public partial class fmMain : Form
    {
        private WeDoWatcher FWatcher;
        private WeDoController FClient;
        private wclBluetoothRadio FRadio;
        private wclBluetoothManager FManager;

        #region Helper functions
        private void Cleanup()
        {
            laState.Text = "Disconnected";

            laFirmwareVersion.Text = "<empty>";
            laHardwareVersion.Text = "<empty>";
            laSoftwareVersion.Text = "<empty>";
            laManufacturerName.Text = "<empty>";

            btDisconnect.Enabled = false;
            btConnect.Enabled = true;

            btSetDeviceName.Enabled = false;
            btGetDeviceName.Enabled = false;
            edDeviceName.Text = "";

            pbBatt.Value = 0;

            FRadio = null;

            FManager.Close();
        }
        #endregion

        #region Device information reading
        private void DisplayDeviceInforValue(Label label, String Value, Int32 Res)
        {
            if (Res == wclErrors.WCL_E_SUCCESS)
                label.Text = Value;
            else
            {
                if (Res == wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND)
                    label.Text = "<unspecified>";
                else
                    label.Text = "Read error: 0x" + Res.ToString("X8");
            }
        }

        private void ReadDeviceInformation()
        {
            String Value;
            Int32 Res = FClient.ReadFirmwareVersion(out Value);
            DisplayDeviceInforValue(laFirmwareVersion, Value, Res);
            Res = FClient.ReadHardwareVersion(out Value);
            DisplayDeviceInforValue(laHardwareVersion, Value, Res);
            Res = FClient.ReadSoftwareVersion(out Value);
            DisplayDeviceInforValue(laSoftwareVersion, Value, Res);
            Res = FClient.ReadManufacturerName(out Value);
            DisplayDeviceInforValue(laManufacturerName, Value, Res);
        }
        #endregion

        #region Battery Level
        private void ReadBattLevel()
        {
            Byte Level;
            Int32 Res = FClient.ReadBatteryLevel(out Level);
            if (Res == wclErrors.WCL_E_SUCCESS)
                pbBatt.Value = Level;
        }
        #endregion

        #region HUB
        private Int32 ReadDeviceName()
        {
            String Name;
            Int32 Res = FClient.ReadDeviceName(out Name);
            if (Res == wclErrors.WCL_E_SUCCESS)
                edDeviceName.Text = Name;
            else
            {
                if (Res == wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND)
                    edDeviceName.Text = "<unsupported>";
                else
                    edDeviceName.Text = "<error>";
            }
            return Res;
        }
        #endregion

        #region Device wahtcher events
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
        #endregion

        #region WeDo Controller (Client) events
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
                btGetDeviceName.Enabled = true;
                btSetDeviceName.Enabled = true;

                ReadDeviceInformation();
                ReadBattLevel();
                ReadDeviceName();
            }
        }

        private void ClientDisconnected(object Sender, int Reason)
        {
            Cleanup();
        }

        private void ClientBatteryLevelChanged(object Sender, byte Level)
        {
            pbBatt.Value = Level;
        }
        #endregion

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
            FWatcher = new WeDoWatcher();
            FWatcher.OnDeviceFound += WatcherDeviceFound;

            FClient = new WeDoController();
            FClient.OnConnected += ClientConnected;
            FClient.OnDisconnected += ClientDisconnected;
            FClient.OnBatteryLevelChanged += ClientBatteryLevelChanged;

            FManager = new wclBluetoothManager();

            FRadio = null;
        }

        private void FmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            FWatcher.Stop();
            FWatcher.Dispose(); // We must dispose the object!
            FWatcher = null;

            FClient.Disconnect();
            FClient.Dispose(); // We must dispose the obejct!
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

        public fmMain()
        {
            InitializeComponent();
        }

        private void BtGetDeviceName_Click(object sender, EventArgs e)
        {
            Int32 Res = ReadDeviceName();
            if (Res != wclErrors.WCL_E_SUCCESS)
                MessageBox.Show("Read device name failed with error: 0x" + Res.ToString("X8"));
        }

        private void BtSetDeviceName_Click(object sender, EventArgs e)
        {
            Int32 Res = FClient.WriteDeviceName(edDeviceName.Text);
            if (Res != wclErrors.WCL_E_SUCCESS)
                MessageBox.Show("Write device name failed with error: 0x" + Res.ToString("X8"));
        }
    }
}
