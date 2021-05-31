using System;
using System.Linq;
using System.Windows.Forms;

using wclCommon;
using wclBluetooth;
using wclWeDoFramework;

namespace WeDoColorSensor
{
    public partial class fmMain : Form
    {
        private wclBluetoothManager FManager;
        private wclWeDoWatcher FWatcher;
        private wclWeDoHub FHub;
        private wclWeDoColorSensor FColor;
        private wclWeDoRgbLight FRgb;

        public fmMain()
        {
            InitializeComponent();
        }

        private void btConnect_Click(object sender, EventArgs e)
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

        private void Disconnect()
        {
            FWatcher.Stop();
            FHub.Disconnect();
            FManager.Close();

            btConnect.Enabled = true;
            btDisconnect.Enabled = false;
        }

        private void fmMain_Load(object sender, EventArgs e)
        {
            FManager = new wclBluetoothManager();

            FWatcher = new wclWeDoWatcher();
            FWatcher.OnHubFound += FWatcher_OnHubFound;

            FHub = new wclWeDoHub();
            FHub.OnConnected += FHub_OnConnected;
            FHub.OnDisconnected += FHub_OnDisconnected;
            FHub.OnDeviceAttached += FHub_OnDeviceAttached;
            FHub.OnDeviceDetached += FHub_OnDeviceDetached;

            FColor = null;
            FRgb = null;
        }

        private void FWatcher_OnHubFound(object Sender, long Address, string Name)
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

        private void FHub_OnConnected(object Sender, int Error)
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

        private void FHub_OnDisconnected(object Sender, int Reason)
        {
            EnableConnect(false);
            FManager.Close();
        }

        private void FHub_OnDeviceAttached(object Sender, wclWeDoIo Device)
        {
            if (FColor == null)
            {
                if (Device.DeviceType == wclWeDoIoDeviceType.iodColorSensor)
                {
                    FColor = (wclWeDoColorSensor)Device;
                    FColor.OnColorDetected += FColor_OnColorDetected;
                    EnableControl(true);
                }
            }

            if (Device.DeviceType == wclWeDoIoDeviceType.iodRgb)
                FRgb = (wclWeDoRgbLight)Device;
        }

        private void FColor_OnColorDetected(object Sender, wclWeDoColorSensorColor Color)
        {
            if (FRgb != null)
            {
                switch (Color)
                {
                    case wclWeDoColorSensorColor.ccBlue:
                        FRgb.SetColorIndex(wclWeDoColor.clBlue);
                        break;
                    case wclWeDoColorSensorColor.ccGreen:
                        FRgb.SetColorIndex(wclWeDoColor.clGreen);
                        break;
                    case wclWeDoColorSensorColor.ccRed:
                        FRgb.SetColorIndex(wclWeDoColor.clRed);
                        break;
                    case wclWeDoColorSensorColor.ccWhite:
                        FRgb.SetColorIndex(wclWeDoColor.clWhite);
                        break;
                    case wclWeDoColorSensorColor.ccYellow:
                        FRgb.SetColorIndex(wclWeDoColor.clYellow);
                        break;
                    default:
                        FRgb.SetColorIndex(wclWeDoColor.clBlack);
                        break;
                }
            }
        }

        private void FHub_OnDeviceDetached(object Sender, wclWeDoIo Device)
        {
            if (Device.DeviceType == wclWeDoIoDeviceType.iodColorSensor && FColor != null && Device.ConnectionId == FColor.ConnectionId)
            {
                FColor = null;
                EnableControl(false);
            }

            if (Device.DeviceType == wclWeDoIoDeviceType.iodRgb)
                FRgb = null;
        }

        private void EnableControl(Boolean Attached)
        {
            if (Attached)
                laIoState.Text = "Attached";
            else
                laIoState.Text = "Dectahed";
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

        private void btDisconnect_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void fmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Disconnect();
        }
    }
}
