using System;
using System.Windows.Forms;

using wclCommon;
using wclBluetooth;
using wclWeDoFramework;

namespace WeDoScan
{
    public partial class fmMain : Form
    {
        // The Bluetooth Manager object (from Bluetooth Framework) allows
        // to control Bluetooth hardware. It is required to work with Bluetooth.
        private wclBluetoothManager FManager;
        // The WeDo Watcher allows to scan for available WeDo Hubs.
        private wclWeDoWatcher FWatcher;

        public fmMain()
        {
            InitializeComponent();
        }

        private void FmMain_Load(Object sender, EventArgs e)
        {
            // Create Bluetooth Manager. We do not need any events from it so simple
            // create the object.
            FManager = new wclBluetoothManager();

            // Now create WeDo Watcher. We need some events from it so also assign event
            // handler.
            FWatcher = new wclWeDoWatcher();
            // The event fires when we started scanning.
            FWatcher.OnStarted += FWatcher_OnStarted;
            // The event fires when we stopped scanning.
            FWatcher.OnStopped += FWatcher_OnStopped;
            // The event fires when new WeDo Hub found.
            FWatcher.OnHubFound += FWatcher_OnHubFound;
            // The event fires when WeDo hub lost.
            FWatcher.OnHubLost += FWatcher_OnHubLost;
            // The event fires when WeDo Hub name changed.
            FWatcher.OnHubNameChanged += FWatcher_OnHubNameChanged;
        }

        private void FWatcher_OnHubNameChanged(Object Sender, Int64 Address, String OldName,
            String NewName)
        {
            // Update Hub name in the list.
            foreach (ListViewItem Item in lvHubs.Items)
            {
                if (Item.Text == Address.ToString("X12"))
                {
                    Item.SubItems[1].Text = NewName;
                    break;
                }
            }
        }

        private void FWatcher_OnHubLost(Object Sender, Int64 Address, String Name)
        {
            // Remove Hub from the list.
            foreach (ListViewItem Item in lvHubs.Items)
            {
                if (Item.Text == Address.ToString("X12"))
                {
                    lvHubs.Items.Remove(Item);
                    break;
                }
            }
        }

        private void FWatcher_OnHubFound(Object Sender, Int64 Address, String Name)
        {
            // One new Hub found simple add it to the list.
            // In Bluetooth world the device's address is always shown as hexadecimal.
            ListViewItem Item = lvHubs.Items.Add(Address.ToString("X12"));
            // Add name.
            Item.SubItems.Add(Name);
        }

        private void EnableButtons(Boolean Started)
        {
            // Started flag indicates how we should update buttons.
            if (Started)
            {
                // If Started is true then we have to disable Start button and enable Stop button.
                btStart.Enabled = false;
                btStop.Enabled = true;
            }
            else
            {
                // If Started is false disable Stop button and enable Start button.
                btStart.Enabled = true;
                btStop.Enabled = false;
                btInfo.Enabled = false;
            }
            // Also clear found devices (hubs) list.
            lvHubs.Items.Clear();
        }

        private void FWatcher_OnStopped(Object Sender, EventArgs e)
        {
            // Here is nothing important to do. Just disable/enable buttons to update UI.
            // Call separate function. This prevents us from writting the same code few times.
            EnableButtons(false);
        }

        private void FWatcher_OnStarted(Object Sender, EventArgs e)
        {
            // Here is nothing important to do. Just disable/enable buttons to update UI.
            // Call separate function. This prevents us from writting the same code few times.
            EnableButtons(true);
        }

        private void Stop()
        {
            // Stop discovering.
            FWatcher.Stop();
            // Close Bluetooth Manager.
            FManager.Close();
        }

        private void BtStop_Click(Object Sender, EventArgs e)
        {
            // We need to execute stop operatrion in few places: when Stop button clicked
            // and when application closed. So use separate function to preven us from code duplication
            // (copy/patse is very bad practice).
            Stop();
        }

        private void FmMain_FormClosed(Object Sender, FormClosedEventArgs e)
        {
            // Stop discovering.
            Stop();
        }

        private void BtStart_Click(Object Sender, EventArgs e)
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
                    }
                }

                // Again, check the found Radio.
                if (Radio == null)
                    // And if it is null (not found or discovering was not started
                    // close the Bluetooth Manager to release all the allocated resources.
                    FManager.Close();
            }
        }

        private void LvHubs_SelectedIndexChanged(Object Sender, EventArgs e)
        {
            // We should enable Get Information button only when device is selected
            // in the list.
            btInfo.Enabled = (lvHubs.SelectedItems.Count != 0);
        }

        private void BtInfo_Click(Object Sender, EventArgs e)
        {
            // We use separate form to show selected Hub information.
            fmDevInfo DevInfo = new fmDevInfo(FWatcher.Radio, 
                Convert.ToInt64(lvHubs.SelectedItems[0].Text, 16));
            DevInfo.ShowDialog(this);
            DevInfo.Dispose();
        }
    }
}
