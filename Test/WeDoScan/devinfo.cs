using System;
using System.Windows.Forms;

using wclCommon;
using wclBluetooth;

using wclWeDoFramework;

namespace WeDoScan
{
    public partial class fmDevInfo : Form
    {
        // The radio object used to connect to WeDo Hub.
        private wclBluetoothRadio FRadio;
        // The Hub MAC address.
        private Int64 FAddress;
        // WeDo Hub instance.
        private wclWeDoHub FHub;

        private void DisplayDeviceInforValue(Label label, String Value, Int32 Res)
        {
            // Helper function to show information value.
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
            // Read all possible information from Hub.
            String Value;
            Int32 Res = FHub.DeviceInformation.ReadFirmwareVersion(out Value);
            DisplayDeviceInforValue(laFirmwareVersion, Value, Res);
            Res = FHub.DeviceInformation.ReadHardwareVersion(out Value);
            DisplayDeviceInforValue(laHardwareVersion, Value, Res);
            Res = FHub.DeviceInformation.ReadSoftwareVersion(out Value);
            DisplayDeviceInforValue(laSoftwareVersion, Value, Res);
            Res = FHub.DeviceInformation.ReadManufacturerName(out Value);
            DisplayDeviceInforValue(laManufacturerName, Value, Res);
        }

        private void ReadDeviceName()
        {
            // Read Hub name.
            String Name;
            Int32 Res = FHub.ReadDeviceName(out Name);
            if (Res == wclErrors.WCL_E_SUCCESS)
                edDeviceName.Text = Name;
            else
            {
                if (Res == wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND)
                    edDeviceName.Text = "<unsupported>";
                else
                    edDeviceName.Text = "<error>";
            }
        }

        private void UpdateBattLevel(Byte Level)
        {
            pbBattLevel.Value = Level;
            laBattLevel.Text = Level.ToString() + " %";
        }

        private void ReadBatteryLevel()
        {
            // Helper function reads battery level.
            Byte Level;
            Int32 Res = FHub.BatteryLevel.ReadBatteryLevel(out Level);
            if (Res == wclErrors.WCL_E_SUCCESS)
                UpdateBattLevel(Level);
        }

        private void BtClose_Click(object sender, EventArgs e)
        {
            // Disconnect from Hub.
            FHub.Disconnect();
        }

        private void BtSetDeviceName_Click(object sender, EventArgs e)
        {
            // Try to change Hub name.
            Int32 Res = FHub.WriteDeviceName(edDeviceName.Text);
            if (Res != wclErrors.WCL_E_SUCCESS)
                MessageBox.Show("Write hub name failed with error: 0x" + Res.ToString("X8"));
        }

        private void FmDevInfo_Load(object sender, EventArgs e)
        {
            // Create WeDo Hub instance.
            FHub = new wclWeDoHub();
            // Setup its event handlers. We are interested in batt level only for now.
            FHub.BatteryLevel.OnBatteryLevelChanged += BatteryLevel_OnBatteryLevelChanged;
            FHub.OnLowVoltageAlert += Hub_OnLowVoltageAlert;
            FHub.OnConnected += FHub_OnConnected;
            FHub.OnDisconnected += FHub_OnDisconnected;
            FHub.OnDeviceAttached += FHub_OnDeviceAttached;
            FHub.OnDeviceDetached += FHub_OnDeviceDetached;
            FHub.OnLowSignalAlert += FHub_OnLowSignalAlert;
            // Try to connect. We will use the same Bluetooth Radio object that is used by
            // the WeDo Watcher.
            Int32 Res = FHub.Connect(FRadio, FAddress);
            if (Res != wclErrors.WCL_E_SUCCESS)
            {
                // If something went wrong show message and close the form.
                MessageBox.Show("Connect to Hub failed: 0x" + Res.ToString("X8"));
                Close();
            }
        }

        private void FHub_OnLowSignalAlert(object Sender, bool Alert)
        {
            laLowSignal.Visible = Alert;
        }

        private void FHub_OnDeviceDetached(object Sender, wclWeDoIo Device)
        {
            foreach (ListViewItem Item in lvAttachedDevices.Items)
            {
                if (Item.Text == Device.ConnectionId.ToString())
                {
                    lvAttachedDevices.Items.Remove(Item);
                    break;
                }
            }
        }

        private void FHub_OnDeviceAttached(object Sender, wclWeDoIo Device)
        {
            ListViewItem Item = lvAttachedDevices.Items.Add(Device.ConnectionId.ToString());
            Item.SubItems.Add(Device.DeviceType.ToString());
            Item.SubItems.Add(Device.FirmwareVersion.ToString());
            Item.SubItems.Add(Device.HardwareVersion.ToString());
            Item.SubItems.Add(Device.Internal.ToString());
            Item.SubItems.Add(Device.PortId.ToString());
        }

        private void Hub_OnLowVoltageAlert(object Sender, bool Alert)
        {
            // Show Low Voltage Warning when alert received.
            laLowVoltage.Visible = Alert;
        }

        private void FHub_OnDisconnected(object Sender, int Reason)
        {
            // When disconnected from Hub close the form.
            Close();
        }

        private void FHub_OnConnected(object Sender, int Error)
        {
            if (Error == wclErrors.WCL_E_SUCCESS)
            {
                // If connection was success we can read Hub information.
                ReadDeviceInformation();
                ReadDeviceName();
                ReadBatteryLevel();
            }
            else
            {
                // Show error message to user and close the form.
                MessageBox.Show("Connection error: 0x" + Error.ToString("X8"));
                Close();
            }
        }

        private void BatteryLevel_OnBatteryLevelChanged(object Sender, byte Level)
        {
            // Update battery level.
            UpdateBattLevel(Level);
        }

        public fmDevInfo(wclBluetoothRadio Radio, Int64 Address)
            : base()
        {
            FRadio = Radio;
            FAddress = Address;

            InitializeComponent();
        }
    }
}
