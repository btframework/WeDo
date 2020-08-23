using System;
using System.Windows.Forms;

using wclCommon;
using wclBluetooth;
using wclWeDoFramework;

namespace WeDoPiezo
{
    public partial class fmMain : Form
    {
        private wclBluetoothManager FManager;
        private wclWeDoWatcher FWatcher;
        private wclWeDoHub FHub;
        private wclWeDoPiezo FPiezo;

        public fmMain()
        {
            InitializeComponent();
        }

        private void FmMain_Load(Object Sender, EventArgs e)
        {
            cbNote.SelectedIndex = 0;
            cbOctave.SelectedIndex = 0;

            FManager = new wclBluetoothManager();

            FWatcher = new wclWeDoWatcher();
            FWatcher.OnHubFound += FWatcher_OnHubFound;

            FHub = new wclWeDoHub();
            FHub.OnConnected += FHub_OnConnected;
            FHub.OnDisconnected += FHub_OnDisconnected;
            FHub.OnDeviceAttached += FHub_OnDeviceAttached;
            FHub.OnDeviceDetached += FHub_OnDeviceDetached;

            FPiezo = null;
        }

        private void EnablePlay(Boolean Attached)
        {
            if (Attached)
                laIoState.Text = "Attached";
            else
                laIoState.Text = "Dectahed";
            btPlay.Enabled = Attached;
            btStop.Enabled = Attached;
            cbNote.Enabled = Attached;
            cbOctave.Enabled = Attached;
            edDuration.Enabled = Attached;
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
            if (Device.DeviceType == wclWeDoIoDeviceType.iodPiezo)
            {
                FPiezo = null;
                EnablePlay(false);
            }
        }

        private void FHub_OnDeviceAttached(Object Sender, wclWeDoIo Device)
        {
            if (Device.DeviceType == wclWeDoIoDeviceType.iodPiezo)
            {
                FPiezo = (wclWeDoPiezo)Device;
                EnablePlay(true);
            }
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

        private void BtStop_Click(Object Sender, EventArgs e)
        {
            if (FPiezo == null)
                MessageBox.Show("Device is not attached");
            else
            {
                Int32 Res = FPiezo.StopPlaying();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Stop failed: 0x" + Res.ToString("X8"));
            }
        }

        private void BtPlay_Click(Object Sender, EventArgs e)
        {
            if (FPiezo == null)
                MessageBox.Show("Device is not attached");
            else
            {
                wclWeDoPiezoNote[] Notes = new wclWeDoPiezoNote[]
                {
                    wclWeDoPiezoNote.pnA,
                    wclWeDoPiezoNote.pnAis,
                    wclWeDoPiezoNote.pnB,
                    wclWeDoPiezoNote.pnC,
                    wclWeDoPiezoNote.pnCis,
                    wclWeDoPiezoNote.pnD,
                    wclWeDoPiezoNote.pnDis,
                    wclWeDoPiezoNote.pnE,
                    wclWeDoPiezoNote.pnF,
                    wclWeDoPiezoNote.pnFis,
                    wclWeDoPiezoNote.pnG,
                    wclWeDoPiezoNote.pnGis
                };
                wclWeDoPiezoNote Note = Notes[cbNote.SelectedIndex];
                Int32 Res = FPiezo.PlayNote(Note, (Byte)(cbOctave.SelectedIndex + 1), Convert.ToUInt16(edDuration.Text));
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Play failed: 0x" + Res.ToString("X8"));
            }
        }
    }
}
