using System;
using System.Windows.Forms;

using wclCommon;
using wclBluetooth;

using wclWeDo;

namespace WeDoControl
{
    public partial class fmDevInfo : Form
    {
        private wclWeDoClient FClient;

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
            Int32 Res = FClient.DeviceInformation.ReadFirmwareVersion(out Value);
            DisplayDeviceInforValue(laFirmwareVersion, Value, Res);
            Res = FClient.DeviceInformation.ReadHardwareVersion(out Value);
            DisplayDeviceInforValue(laHardwareVersion, Value, Res);
            Res = FClient.DeviceInformation.ReadSoftwareVersion(out Value);
            DisplayDeviceInforValue(laSoftwareVersion, Value, Res);
            Res = FClient.DeviceInformation.ReadManufacturerName(out Value);
            DisplayDeviceInforValue(laManufacturerName, Value, Res);
        }

        private void ReadDeviceName()
        {
            String Name;
            Int32 Res = FClient.Hub.ReadDeviceName(out Name);
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

        private void BtClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtSetDeviceName_Click(object sender, EventArgs e)
        {
            Int32 Res = FClient.Hub.WriteDeviceName(edDeviceName.Text);
            if (Res != wclErrors.WCL_E_SUCCESS)
                MessageBox.Show("Write device name failed with error: 0x" + Res.ToString("X8"));
        }

        private void FmDevInfo_Load(object sender, EventArgs e)
        {
            ReadDeviceInformation();
            ReadDeviceName();
        }

        public fmDevInfo(wclWeDoClient Client)
            : base()
        {
            FClient = Client;

            InitializeComponent();
        }
    }
}
