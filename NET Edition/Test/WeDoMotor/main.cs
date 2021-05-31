using System;
using System.Windows.Forms;

using wclCommon;
using wclBluetooth;
using wclWeDoFramework;

namespace WeDoMotor
{
    public partial class fmMain : Form
    {
        private wclBluetoothManager FManager;
        private wclWeDoWatcher FWatcher;
        private wclWeDoHub FHub;
        private wclWeDoMotor FMotor1;
        private wclWeDoMotor FMotor2;
        private wclWeDoCurrentSensor FCurrent;
        private wclWeDoVoltageSensor FVoltage;

        public fmMain()
        {
            InitializeComponent();
        }

        private void FmMain_Load(Object sender, EventArgs e)
        {
            cbDirection1.SelectedIndex = 0;
            cbDirection2.SelectedIndex = 0;

            FManager = new wclBluetoothManager();

            FWatcher = new wclWeDoWatcher();
            FWatcher.OnHubFound += FWatcher_OnHubFound;

            FHub = new wclWeDoHub();
            FHub.OnConnected += FHub_OnConnected;
            FHub.OnDisconnected += FHub_OnDisconnected;
            FHub.OnDeviceAttached += FHub_OnDeviceAttached;
            FHub.OnDeviceDetached += FHub_OnDeviceDetached;
            FHub.OnHighCurrentAlert += FHub_OnHighCurrentAlert;
            FHub.OnLowVoltageAlert += FHub_OnLowVoltageAlert;

            FMotor1 = null;
            FMotor2 = null;
            FCurrent = null;
            FVoltage = null;
        }

        private void FHub_OnLowVoltageAlert(Object Sender, Boolean Alert)
        {
            laLowVoltage.Visible = Alert;
        }

        private void FHub_OnHighCurrentAlert(Object Sender, Boolean Alert)
        {
            laHighCurrent.Visible = Alert;
        }

        private void EnablePlay()
        {
            Boolean Attached = (FMotor1 != null || FMotor2 != null);

            laCurrentTitle.Enabled = Attached;
            laCurrent.Enabled = Attached;
            laMA.Enabled = Attached;

            laVoltageTitle.Enabled = Attached;
            laVoltage.Enabled = Attached;
            laMV.Enabled = Attached;

            if (!Attached)
            {
                laCurrent.Text = "0";
                laVoltage.Text = "0";

                laHighCurrent.Visible = false;
                laLowVoltage.Visible = false;
            }
        }

        private void EnablePlay1(Boolean Attached)
        {
            if (Attached)
                laIoState1.Text = "Attached";
            else
                laIoState1.Text = "Detached";

            laDirection1.Enabled = Attached;
            cbDirection1.Enabled = Attached;
            laPower1.Enabled = Attached;
            edPower1.Enabled = Attached;

            btStart1.Enabled = Attached;
            btBrake1.Enabled = Attached;
            btDrift1.Enabled = Attached;

            EnablePlay();
        }

        private void EnablePlay2(Boolean Attached)
        {
            if (Attached)
                laIoState2.Text = "Attached";
            else
                laIoState2.Text = "Detached";

            laDirection2.Enabled = Attached;
            cbDirection2.Enabled = Attached;
            laPower2.Enabled = Attached;
            edPower2.Enabled = Attached;

            btStart2.Enabled = Attached;
            btBrake2.Enabled = Attached;
            btDrift2.Enabled = Attached;

            EnablePlay();
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
            if (Device.DeviceType == wclWeDoIoDeviceType.iodMotor)
            {
                if (FMotor1 != null && FMotor1.ConnectionId == Device.ConnectionId)
                {
                    FMotor1 = null;
                    EnablePlay1(false);
                }
                if (FMotor2 != null && FMotor2.ConnectionId == Device.ConnectionId)
                {
                    FMotor2 = null;
                    EnablePlay2(false);
                }
            }
            if (Device.DeviceType == wclWeDoIoDeviceType.iodCurrentSensor)
                FCurrent = null;
            if (Device.DeviceType == wclWeDoIoDeviceType.iodVoltageSensor)
                FVoltage = null;
        }

        private void FHub_OnDeviceAttached(Object Sender, wclWeDoIo Device)
        {
            if (Device.DeviceType == wclWeDoIoDeviceType.iodMotor)
            {
                if (FMotor1 == null)
                {

                    {
                        FMotor1 = (wclWeDoMotor)Device;
                        EnablePlay1(true);
                    }
                }
                else
                {
                    if (FMotor2 == null)
                    {

                        {
                            FMotor2 = (wclWeDoMotor)Device;
                            EnablePlay2(true);
                        }
                    }
                }
            }

            if (FCurrent == null)
            {
                if (Device.DeviceType == wclWeDoIoDeviceType.iodCurrentSensor)
                {
                    FCurrent = (wclWeDoCurrentSensor)Device;
                    FCurrent.OnCurrentChanged += FCurrent_OnCurrentChanged;
                }
            }

            if (FVoltage == null)
            {
                if (Device.DeviceType == wclWeDoIoDeviceType.iodVoltageSensor)
                {
                    FVoltage = (wclWeDoVoltageSensor)Device;
                    FVoltage.OnVoltageChanged += FVoltage_OnVoltageChanged;
                }
            }
        }

        private void FVoltage_OnVoltageChanged(Object Sender, EventArgs e)
        {
            if (FVoltage != null)
                laVoltage.Text = FVoltage.Voltage.ToString();
        }

        private void FCurrent_OnCurrentChanged(Object Sender, EventArgs e)
        {
            if (FCurrent != null)
                laCurrent.Text = FCurrent.Current.ToString();
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

        private void btStart1_Click(Object Sender, EventArgs e)
        {
            if (FMotor1 == null)
                MessageBox.Show("Device is not attached");
            else
            {
                wclWeDoMotorDirection Dir;
                switch (cbDirection1.SelectedIndex)
                {
                    case 0:
                        Dir = wclWeDoMotorDirection.mdRight;
                        break;
                    case 1:
                        Dir = wclWeDoMotorDirection.mdLeft;
                        break;
                    default:
                        Dir = wclWeDoMotorDirection.mdUnknown;
                        break;
                }
                Int32 Res = FMotor1.Run(Dir, Convert.ToByte(edPower1.Text));
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Start motor failed: 0x" + Res.ToString("X8"));
            }
        }

        private void btBrake1_Click(Object Sender, EventArgs e)
        {
            if (FMotor1 == null)
                MessageBox.Show("Device is not attached");
            else
            {
                Int32 Res = FMotor1.Brake();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Brake failed; 0x" + Res.ToString("X8"));
            }
        }

        private void btDrift1_Click(Object Sender, EventArgs e)
        {
            if (FMotor1 == null)
                MessageBox.Show("Device is not attached");
            else
            {
                Int32 Res = FMotor1.Drift();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Drift failed; 0x" + Res.ToString("X8"));
            }
        }

        private void btStart2_Click(object sender, EventArgs e)
        {
            if (FMotor2 == null)
                MessageBox.Show("Device is not attached");
            else
            {
                wclWeDoMotorDirection Dir;
                switch (cbDirection2.SelectedIndex)
                {
                    case 0:
                        Dir = wclWeDoMotorDirection.mdRight;
                        break;
                    case 1:
                        Dir = wclWeDoMotorDirection.mdLeft;
                        break;
                    default:
                        Dir = wclWeDoMotorDirection.mdUnknown;
                        break;
                }
                Int32 Res = FMotor2.Run(Dir, Convert.ToByte(edPower2.Text));
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Start motor failed: 0x" + Res.ToString("X8"));
            }
        }

        private void btBrake2_Click(object sender, EventArgs e)
        {
            if (FMotor2 == null)
                MessageBox.Show("Device is not attached");
            else
            {
                Int32 Res = FMotor2.Brake();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Brake failed; 0x" + Res.ToString("X8"));
            }
        }

        private void btDrift2_Click(object sender, EventArgs e)
        {
            if (FMotor2 == null)
                MessageBox.Show("Device is not attached");
            else
            {
                Int32 Res = FMotor2.Drift();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Drift failed; 0x" + Res.ToString("X8"));
            }
        }
    }
}
