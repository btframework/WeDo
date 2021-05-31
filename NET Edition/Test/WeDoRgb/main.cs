using System;
using System.Windows.Forms;
using System.Windows.Media;

using wclCommon;
using wclBluetooth;
using wclWeDoFramework;

namespace WeDoRgb
{
    public partial class fmMain : Form
    {
        private wclBluetoothManager FManager;
        private wclWeDoWatcher FWatcher;
        private wclWeDoHub FHub;
        private wclWeDoRgbLight FRgb;

        public fmMain()
        {
            InitializeComponent();
        }

        private void FmMain_Load(Object Sender, EventArgs e)
        {
            cbColorMode.SelectedIndex = -1;

            FManager = new wclBluetoothManager();

            FWatcher = new wclWeDoWatcher();
            FWatcher.OnHubFound += FWatcher_OnHubFound;

            FHub = new wclWeDoHub();
            FHub.OnConnected += FHub_OnConnected;
            FHub.OnDisconnected += FHub_OnDisconnected;
            FHub.OnDeviceAttached += FHub_OnDeviceAttached;
            FHub.OnDeviceDetached += FHub_OnDeviceDetached;

            FRgb = null;
        }

        private void UpdateMode()
        {
            if (FRgb != null)
            {
                switch (FRgb.Mode)
                {
                    case wclWeDoRgbLightMode.lmDiscrete:
                        cbColorMode.SelectedIndex = 0;
                        break;
                    case wclWeDoRgbLightMode.lmAbsolute:
                        cbColorMode.SelectedIndex = 1;
                        break;
                    default:
                        cbColorMode.SelectedIndex = -1;
                        break;
                }
            }
        }

        private void UpdateRgb()
        {
            Color c = FRgb.Color;
            edR.Text = c.R.ToString();
            edG.Text = c.G.ToString();
            edB.Text = c.B.ToString();
        }

        private void UpdateIndex()
        {
            cbColorIndex.SelectedIndex = (Int32)FRgb.ColorIndex;
        }

        private void UpdateValues()
        {
            UpdateRgb();
            UpdateIndex();
            UpdateMode();
        }

        private void EnableSetColors(Boolean Attached)
        {
            if (Attached)
                laIoState.Text = "Attached";
            else
            {
                laIoState.Text = "Dectahed";

                edR.Text = "";
                edG.Text = "";
                edB.Text = "";

                cbColorIndex.SelectedIndex = -1;
                cbColorMode.SelectedIndex = -1;
            }

            cbColorMode.Enabled = Attached;
            laColorMode.Enabled = Attached;

            laR.Enabled = Attached;
            laG.Enabled = Attached;
            laB.Enabled = Attached;
            edR.Enabled = Attached;
            edG.Enabled = Attached;
            edB.Enabled = Attached;
            btSetRgb.Enabled = Attached;

            laColorIndex.Enabled = Attached;
            cbColorIndex.Enabled = Attached;
            btSetIndex.Enabled = Attached;

            btSetDefault.Enabled = Attached;
            btTurnOff.Enabled = Attached;
            btSetMode.Enabled = Attached;

            if (Attached)
                UpdateValues();
        }

        private void EnableConnect(Boolean Connected)
        {
            if (Connected)
            {
                btConnect.Enabled = false;
                btDisconnect.Enabled = true;
                laStatus.Text = "Connected";
            }
            else
            {
                btConnect.Enabled = true;
                btDisconnect.Enabled = false;
                laStatus.Text = "Disconnected";
            }
        }

        private void FHub_OnDeviceDetached(Object Sender, wclWeDoIo Device)
        {
            if (Device.DeviceType == wclWeDoIoDeviceType.iodRgb)
            {
                FRgb = null;
                EnableSetColors(false);
            }
        }

        private void FHub_OnDeviceAttached(Object Sender, wclWeDoIo Device)
        {
            if (Device.DeviceType == wclWeDoIoDeviceType.iodRgb)
            {
                FRgb = (wclWeDoRgbLight)Device;
                FRgb.OnColorChanged += FRgb_OnColorChanged;
                FRgb.OnModeChanged += FRgb_OnModeChanged;

                EnableSetColors(true);
                EnableColorControls();
            }
        }

        private void FRgb_OnColorChanged(Object Sender, EventArgs e)
        {
            UpdateRgb();
            UpdateIndex();
        }

        private void FRgb_OnModeChanged(Object Sender, EventArgs e)
        {
            UpdateMode();
        }

        private void FHub_OnDisconnected(Object Sender, Int32 Reason)
        {
            EnableConnect(false);
            FManager.Close();
        }

        private void FHub_OnConnected(Object Sender, Int32 Error)
        {
            if (Error != wclErrors.WCL_E_SUCCESS)
            {
                MessageBox.Show("Connect failed: 0x" + Error.ToString("X8"));
                EnableConnect(false);
                FManager.Close();
            }
            else
                EnableConnect(true);
        }

        private void FWatcher_OnHubFound(Object Sender, Int64 Address, String Name)
        {
            wclBluetoothRadio Radio = FWatcher.Radio;
            FWatcher.Stop();
            Int32 Res = FHub.Connect(Radio, Address);
            if (Res != wclErrors.WCL_E_SUCCESS)
            {
                MessageBox.Show("Connect failed: 0x" + Res.ToString("X8"));
                EnableConnect(false);
            }
            else
                laStatus.Text = "Connecting";
        }

        private void Disconnect()
        {
            FWatcher.Stop();
            FHub.Disconnect();
            FManager.Close();

            btConnect.Enabled = true;
            btDisconnect.Enabled = false;
        }

        private void BtDisconnect_Click(Object Sender, EventArgs e)
        {
            Disconnect();
        }

        private void FmMain_FormClosed(Object Sender, FormClosedEventArgs e)
        {
            Disconnect();
        }

        private void BtConnect_Click(Object Sender, EventArgs e)
        {
            // The very first thing we have to do is to open Bluetooth Manager.
            // That initializes the underlying drivers and allows us to work with Bluetooth.

            // Always check result!
            Int32 Res = FManager.Open();
            if (Res != wclErrors.WCL_E_SUCCESS)
                // It should never happen but if it does notify user.
                MessageBox.Show("Unable to open Bluetooth Manager: 0x" + Res.ToString("X8"));
            else
            {
                // Assume that no one Bluetooth Radio available.
                wclBluetoothRadio Radio = null;
                Res = FManager.GetLeRadio(out Radio);
                if (Res != wclErrors.WCL_E_SUCCESS)
                    // If not, let user know that he has no Bluetooth.
                    MessageBox.Show("No available Bluetooth Radio found");
                else
                {
                    // If found, try to start discovering.
                    Res = FWatcher.Start(Radio);
                    if (Res != wclErrors.WCL_E_SUCCESS)
                        // It is something wrong with discovering starting. Notify user about the error.
                        MessageBox.Show("Unable to start discovering: 0x" + Res.ToString("X8"));
                    else
                    {
                        btConnect.Enabled = false;
                        btDisconnect.Enabled = true;
                        laStatus.Text = "Searching...";
                    }
                }

                // Again, check the found Radio.
                if (Res != wclErrors.WCL_E_SUCCESS)
                {
                    // And if it is null (not found or discovering was not started
                    // close the Bluetooth Manager to release all the allocated resources.
                    FManager.Close();
                    // Also clean up found Radio variable so we can check it later.
                    Radio = null;
                }
            }
        }

        private void BtSetRgb_Click(Object Sender, EventArgs e)
        {
            if (FRgb == null)
                MessageBox.Show("Device is not attached");
            else
            {
                Color c = Color.FromRgb(Convert.ToByte(edR.Text), Convert.ToByte(edG.Text),
                    Convert.ToByte(edB.Text));
                Int32 Res = FRgb.SetColor(c);
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Unable to set color: 0x" + Res.ToString("X8"));
            }
        }

        private void BtSetDefault_Click(Object Sender, EventArgs e)
        {
            if (FRgb == null)
                MessageBox.Show("Device is not attached");
            else
            {
                Int32 Res = FRgb.SwitchToDefaultColor();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Unable to set default color: 0x" + Res.ToString("X8"));
            }
        }

        private void BtTurnOff_Click(Object Sender, EventArgs e)
        {
            if (FRgb == null)
                MessageBox.Show("Device is not attached");
            else
            {
                Int32 Res = FRgb.SwitchOff();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Unable to switch LED off: 0x" + Res.ToString("X8"));
            }
        }

        private void BtSetIndex_Click(Object Sender, EventArgs e)
        {
            if (FRgb == null)
                MessageBox.Show("Device is not attached");
            else
            {
                Int32 Res = FRgb.SetColorIndex((wclWeDoColor)cbColorIndex.SelectedIndex);
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Unable to set color index: 0x" + Res.ToString("X8"));
            }
        }
        
        private void EnableColorControls()
        {
            Boolean RgbEnabled = (cbColorMode.SelectedIndex == 1);
            Boolean IndexEnabled = (cbColorMode.SelectedIndex == 0);

            laR.Enabled = RgbEnabled;
            laG.Enabled = RgbEnabled;
            laB.Enabled = RgbEnabled;
            edR.Enabled = RgbEnabled;
            edG.Enabled = RgbEnabled;
            edB.Enabled = RgbEnabled;
            btSetRgb.Enabled = RgbEnabled;

            laColorIndex.Enabled = IndexEnabled;
            cbColorIndex.Enabled = IndexEnabled;
            btSetIndex.Enabled = IndexEnabled;
        }

        private void BtSetMode_Click(object sender, EventArgs e)
        {
            if (FRgb == null)
                MessageBox.Show("Device is not attached");
            else
            {
                Boolean RgbEnabled = (cbColorMode.SelectedIndex == 1);
                Boolean IndexEnabled = (cbColorMode.SelectedIndex == 0);

                Int32 Res = wclErrors.WCL_E_SUCCESS;
                if (RgbEnabled)
                    Res = FRgb.SetMode(wclWeDoRgbLightMode.lmAbsolute);
                else
                {
                    if (IndexEnabled)
                        Res = FRgb.SetMode(wclWeDoRgbLightMode.lmDiscrete);
                }

                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Unable to change color mode: 0x" + Res.ToString("X8"));
                else
                {
                    EnableColorControls();

                    if (RgbEnabled)
                        UpdateRgb();
                    else
                    {
                        if (IndexEnabled)
                            UpdateIndex();
                    }
                }
            }
        }
    }
}
