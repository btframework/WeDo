﻿using System;
using System.Windows.Forms;

using wclCommon;
using wclBluetooth;
using wclWeDoFramework;

namespace WeDoMotionSensor
{
    public partial class fmMain : Form
    {
        private wclBluetoothManager FManager;
        private wclWeDoWatcher FWatcher;
        private wclWeDoHub FHub;
        private wclWeDoMotionSensor FMotion;

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

            FMotion = null;
        }

        private void EnableControl(Boolean Attached)
        {
            if (Attached)
                laIoState.Text = "Attached";
            else
            {
                laIoState.Text = "Dectahed";

                laCount.Text = "0";
                laDistance.Text = "0";
            }

            laMode.Enabled = Attached;
            cbMode.Enabled = Attached;
            btChange.Enabled = Attached;
            laCountTitle.Enabled = Attached;
            laCount.Enabled = Attached;
            laDistanceTitle.Enabled = Attached;
            laDistance.Enabled = Attached;
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
            if (Device.DeviceType == wclWeDoIoDeviceType.iodMotionSensor && FMotion != null && Device.ConnectionId == FMotion.ConnectionId)
            {
                FMotion = null;
                EnableControl(false);
            }
        }

        private void FHub_OnDeviceAttached(Object Sender, wclWeDoIo Device)
        {
            if (FMotion == null)
            {
                if (Device.DeviceType == wclWeDoIoDeviceType.iodMotionSensor)
                {
                    FMotion = (wclWeDoMotionSensor)Device;
                    FMotion.OnCountChanged += FMotion_OnCountChanged;
                    FMotion.OnDistanceChanged += FMotion_OnDistanceChanged;
                    FMotion.OnModeChanged += FMotion_OnModeChanged;
                    EnableControl(true);
                }
            }
        }

        private void FMotion_OnModeChanged(Object Sender, EventArgs e)
        {
            switch (FMotion.Mode)
            {
                case wclWeDoMotionSensorMode.mmDetect:
                    cbMode.SelectedIndex = 0;
                    break;
                case wclWeDoMotionSensorMode.mmCount:
                    cbMode.SelectedIndex = 1;
                    break;
                default:
                    cbMode.SelectedIndex = -1;
                    break;
            }
        }

        private void FMotion_OnDistanceChanged(Object Sender, EventArgs e)
        {
            laDistance.Text = FMotion.Distance.ToString();
        }

        private void FMotion_OnCountChanged(Object Sender, EventArgs e)
        {
            laCount.Text = FMotion.Count.ToString();
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

        private void BtDisconnect_Click(Object sender, EventArgs e)
        {
            Disconnect();
        }

        private void FmMain_FormClosed(Object sender, FormClosedEventArgs e)
        {
            Disconnect();
        }

        private void BtConnect_Click(Object sender, EventArgs e)
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

        private void BtChange_Click(Object sender, EventArgs e)
        {
            if (FMotion == null)
                MessageBox.Show("Device is not attached");
            else
            {
                wclWeDoMotionSensorMode Mode;
                switch (cbMode.SelectedIndex)
                {
                    case 0:
                        Mode = wclWeDoMotionSensorMode.mmDetect;
                        break;
                    case 1:
                        Mode = wclWeDoMotionSensorMode.mmCount;
                        break;
                    default:
                        Mode = wclWeDoMotionSensorMode.mmUnknown;
                        break;
                }
                if (Mode == wclWeDoMotionSensorMode.mmUnknown)
                    MessageBox.Show("Invalid mode.");
                else
                {
                    Int32 Res = FMotion.SetMode(Mode);
                    if (Res != wclErrors.WCL_E_SUCCESS)
                        MessageBox.Show("Mode change failed: 0x" + Res.ToString("X8"));
                }
            }
        }

        private void BtReset_Click(Object sender, EventArgs e)
        {
            if (FMotion == null)
                MessageBox.Show("Device is not attached");
            else
            {
                Int32 Res = FMotion.Reset();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Reset failed; 0x" + Res.ToString("X8"));
            }
        }
    }
}
