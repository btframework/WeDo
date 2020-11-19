using System;
using System.Windows.Forms;

using wclCommon;
using wclBluetooth;
using wclWeDoFramework;

namespace WeDoTiltSensor
{
    public partial class fmMain : Form
    {
        private wclBluetoothManager FManager;
        private wclWeDoWatcher FWatcher;
        private wclWeDoHub FHub;
        private wclWeDoTiltSensor FTilt;

        public fmMain()
        {
            InitializeComponent();
        }

        private void FmMain_Load(Object Sender, EventArgs e)
        {
            cbMode.SelectedIndex = 0;

            FManager = new wclBluetoothManager();

            FWatcher = new wclWeDoWatcher();
            FWatcher.OnHubFound += FWatcher_OnHubFound;

            FHub = new wclWeDoHub();
            FHub.OnConnected += FHub_OnConnected;
            FHub.OnDisconnected += FHub_OnDisconnected;
            FHub.OnDeviceAttached += FHub_OnDeviceAttached;
            FHub.OnDeviceDetached += FHub_OnDeviceDetached;

            FTilt = null;
        }

        private void EnableControl(Boolean Attached)
        {
            if (Attached)
                laIoState.Text = "Attached";
            else
            {
                laIoState.Text = "Dectahed";

                laDirection.Text = "Unknown";
                laX.Text = "0";
                laY.Text = "0";
                laZ.Text = "0";
            }

            laMode.Enabled = Attached;
            cbMode.Enabled = Attached;
            btChange.Enabled = Attached;
            laXTitle.Enabled = Attached;
            laX.Enabled = Attached;
            laYTitle.Enabled = Attached;
            laY.Enabled = Attached;
            laZTitle.Enabled = Attached;
            laZ.Enabled = Attached;
            laDirectionTitle.Enabled = Attached;
            laDirection.Enabled = Attached;
            btReset.Enabled = Attached;
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
            if (Device.DeviceType == wclWeDoIoDeviceType.iodTiltSensor && FTilt != null && Device.ConnectionId == FTilt.ConnectionId)
            {
                FTilt = null;
                EnableControl(false);
            }
        }

        private void FHub_OnDeviceAttached(Object Sender, wclWeDoIo Device)
        {
            if (FTilt == null)
            {
                if (Device.DeviceType == wclWeDoIoDeviceType.iodTiltSensor)
                {
                    FTilt = (wclWeDoTiltSensor)Device;
                    FTilt.OnAngleChanged += FTilt_OnAngleChanged;
                    FTilt.OnCrashChanged += FTilt_OnCrashChanged;
                    FTilt.OnDirectionChanged += FTilt_OnDirectionChanged;
                    FTilt.OnModeChanged += FTilt_OnModeChanged;
                    EnableControl(true);
                }
            }
        }

        private void FTilt_OnModeChanged(Object Sender, EventArgs e)
        {
            switch (FTilt.Mode)
            {
                case wclWeDoTiltSensorMode.tmAngle:
                    cbMode.SelectedIndex = 0;
                    break;
                case wclWeDoTiltSensorMode.tmTilt:
                    cbMode.SelectedIndex = 1;
                    break;
                case wclWeDoTiltSensorMode.tmCrash:
                    cbMode.SelectedIndex = 2;
                    break;
                default:
                    cbMode.SelectedIndex = -1;
                    break;
            }
        }

        private void FTilt_OnDirectionChanged(Object Sender, EventArgs e)
        {
            switch (FTilt.Direction)
            {
                case wclWeDoTiltSensorDirection.tdBackward:
                    laDirection.Text = "Backward";
                    break;
                case wclWeDoTiltSensorDirection.tdForward:
                    laDirection.Text = "Forward";
                    break;
                case wclWeDoTiltSensorDirection.tdLeft:
                    laDirection.Text = "Left";
                    break;
                case wclWeDoTiltSensorDirection.tdNeutral:
                    laDirection.Text = "Neutral";
                    break;
                case wclWeDoTiltSensorDirection.tdRight:
                    laDirection.Text = "Right";
                    break;
                default:
                    laDirection.Text = "Unknown";
                    break;
            }
            laX.Text = "0";
            laY.Text = "0";
            laZ.Text = "0";
        }

        private void FTilt_OnCrashChanged(Object Sender, EventArgs e)
        {
            wclWeDoTiltSensorCrash Crash = FTilt.Crash;
            laX.Text = Crash.X.ToString();
            laY.Text = Crash.Y.ToString();
            laZ.Text = Crash.Z.ToString();
            laDirection.Text = "Unknown";
        }

        private void FTilt_OnAngleChanged(Object Sender, EventArgs e)
        {
            wclWeDoTiltSensorAngle Angle = FTilt.Angle;
            laX.Text = Angle.X.ToString();
            laY.Text = Angle.Y.ToString();
            laZ.Text = "0";
            laDirection.Text = "Unknown";
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

                // Check that at least one Bluetooth Radio exists (or at least Bluetooth drivers installed).
                if (FManager.Count == 0)
                    // No one, even drivers?
                    MessageBox.Show("No Bluetooth Hardware installed");
                else
                {
                    // Ok, at least one Bluetooth Radio module should be available.
                    for (Int32 i = 0; i < FManager.Count; i++)
                    {
                        // Check if current Radio module is available (plugged in and turned ON).
                        if (FManager[i].Available)
                        {
                            // Looks like we have Bluetooth on this PC!
                            Radio = FManager[i];
                            // Terminate the loop.
                            break;
                        }
                    }

                    // Check that we found the Bluetooth Radio module.
                    if (Radio == null)
                        // If not, let user know that he has no Bluetooth.
                        MessageBox.Show("No available Bluetooth Radio found");
                    else
                    {
                        // If found, try to start discovering.
                        Res = FWatcher.Start(Radio);
                        if (Res != wclErrors.WCL_E_SUCCESS)
                        {
                            // It is something wrong with discovering starting. Notify user about the error.
                            MessageBox.Show("Unable to start discovering: 0x" + Res.ToString("X8"));
                            // Also clean up found Radio variable so we can check it later.
                            Radio = null;
                        }
                        else
                        {
                            btConnect.Enabled = false;
                            btDisconnect.Enabled = true;
                            laStatus.Text = "Searching...";
                        }
                    }
                }

                // Again, check the found Radio.
                if (Radio == null)
                    // And if it is null (not found or discovering was not started
                    // close the Bluetooth Manager to release all the allocated resources.
                    FManager.Close();
            }
        }

        private void BtChange_Click(Object Sender, EventArgs e)
        {
            if (FTilt == null)
                MessageBox.Show("Device is not attached");
            else
            {
                wclWeDoTiltSensorMode Mode;
                switch (cbMode.SelectedIndex)
                {
                    case 0:
                        Mode = wclWeDoTiltSensorMode.tmAngle;
                        break;
                    case 1:
                        Mode = wclWeDoTiltSensorMode.tmTilt;
                        break;
                    case 2:
                        Mode = wclWeDoTiltSensorMode.tmCrash;
                        break;
                    default:
                        Mode = wclWeDoTiltSensorMode.tmUnknown;
                        break;
                }
                if (Mode == wclWeDoTiltSensorMode.tmUnknown)
                    MessageBox.Show("Invalid mode.");
                else
                {
                    Int32 Res = FTilt.SetMode(Mode);
                    if (Res != wclErrors.WCL_E_SUCCESS)
                        MessageBox.Show("Mode change failed: 0x" + Res.ToString("X8"));
                }
            }
        }

        private void BtReset_Click(Object Sender, EventArgs e)
        {
            if (FTilt == null)
                MessageBox.Show("Device is not attached");
            else
            {
                Int32 Res = FTilt.Reset();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Reset failed; 0x" + Res.ToString("X8"));
            }
        }

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void laMode_Click(object sender, EventArgs e)
        {

        }
    }
}
