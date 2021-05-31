using System;
using System.Collections.Generic;
using System.Windows.Forms;

using wclCommon;
using wclBluetooth;
using wclWeDoFramework;

namespace WeDoHubButton
{
    public partial class fmMain : Form
    {
        private wclBluetoothManager FManager;
        private wclWeDoWatcher FWatcher;
        private List<wclWeDoHub> FHubs;

        public fmMain()
        {
            InitializeComponent();
        }

        private void UpdateButtons(Boolean Started)
        {
            if (Started)
            {
                btStart.Enabled = false;
                btStop.Enabled = true;
            }
            else
            {
                btStart.Enabled = true;
                btStop.Enabled = false;

                while (FHubs.Count > 0)
                    FHubs[0].Disconnect();
            }
            lvHubs.Items.Clear();
        }

        private void FmMain_Load(Object Sender, EventArgs e)
        {
            FManager = new wclBluetoothManager();

            FWatcher = new wclWeDoWatcher();
            FWatcher.OnHubFound += FWatcher_OnHubFound;
            FWatcher.OnHubNameChanged += FWatcher_OnHubNameChanged;
            FWatcher.OnStarted += FWatcher_OnStarted;
            FWatcher.OnStopped += FWatcher_OnStopped;

            FHubs = new List<wclWeDoHub>();
        }

        private void FWatcher_OnStopped(Object Sender, EventArgs e)
        {
            UpdateButtons(false);
        }

        private void FWatcher_OnStarted(Object Sender, EventArgs e)
        {
            UpdateButtons(true);
        }

        private void FWatcher_OnHubNameChanged(Object Sender, Int64 Address, String OldName,
            String NewName)
        {
            foreach (ListViewItem Item in lvHubs.Items)
            {
                if (Item.Text == Address.ToString("X12"))
                    Item.SubItems[1].Text = Name;
            }
        }

        private void FWatcher_OnHubFound(Object Sender, Int64 Address, String Name)
        {
            wclWeDoHub Hub = new wclWeDoHub();
            Hub.OnConnected += Hub_OnConnected;
            Hub.OnDisconnected += Hub_OnDisconnected;
            Hub.OnButtonStateChanged += Hub_OnButtonStateChanged;
            Hub.OnLowVoltageAlert += Hub_OnLowVoltageAlert;
            Hub.BatteryLevel.OnBatteryLevelChanged += BatteryLevel_OnBatteryLevelChanged;

            Int32 Res = Hub.Connect(FWatcher.Radio, Address);
            if (Res == wclErrors.WCL_E_SUCCESS)
            {
                ListViewItem Item = lvHubs.Items.Add(Address.ToString("X12"));
                Item.SubItems.Add(Name);
                Item.SubItems.Add("Connecting");
                Item.SubItems.Add("");
                Item.SubItems.Add("");
                Item.SubItems.Add("");
                FHubs.Add(Hub);
            }
        }

        private void Hub_OnLowVoltageAlert(Object Sender, Boolean Alert)
        {
            Int64 Address = ((wclWeDoHub)Sender).Address;
            foreach (ListViewItem Item in lvHubs.Items)
            {
                if (Item.Text == Address.ToString("X12"))
                {
                    if (Alert)
                        Item.SubItems[4].Text = "ALERT";
                    else
                        Item.SubItems[4].Text = "";
                }
            }
        }

        private void Hub_OnButtonStateChanged(Object Sender, Boolean Pressed)
        {
            Int64 Address = ((wclWeDoHub)Sender).Address;
            foreach (ListViewItem Item in lvHubs.Items)
            {
                if (Item.Text == Address.ToString("X12"))
                {
                    if (Pressed)
                        Item.SubItems[5].Text = "PRESSED";
                    else
                        Item.SubItems[5].Text = "";
                }
            }
        }

        private void BatteryLevel_OnBatteryLevelChanged(Object Sender, Byte Level)
        {
            Int64 Address = ((wclWeDoBatteryLevelService)Sender).Hub.Address;
            foreach (ListViewItem Item in lvHubs.Items)
            {
                if (Item.Text == Address.ToString("X12"))
                    Item.SubItems[3].Text = Level.ToString();
            }
        }

        private void Hub_OnDisconnected(Object Sender, Int32 Reason)
        {
            wclWeDoHub Hub = (wclWeDoHub)Sender;
            foreach (ListViewItem Item in lvHubs.Items)
            {
                if (Item.Text == Hub.Address.ToString("X12"))
                {
                    lvHubs.Items.Remove(Item);
                    FHubs.Remove(Hub);
                    break;
                }
            }
        }

        private void Hub_OnConnected(Object Sender, Int32 Error)
        {
            wclWeDoHub Hub = (wclWeDoHub)Sender;
            foreach (ListViewItem Item in lvHubs.Items)
            {
                if (Item.Text == Hub.Address.ToString("X12"))
                {
                    if (Error != wclErrors.WCL_E_SUCCESS)
                    {
                        lvHubs.Items.Remove(Item);
                        FHubs.Remove(Hub);
                        break;
                    }
                    else
                    {
                        Item.SubItems[2].Text = "Connected";
                        Byte Level;
                        Int32 Res = Hub.BatteryLevel.ReadBatteryLevel(out Level);
                        if (Res == wclErrors.WCL_E_SUCCESS)
                            Item.SubItems[3].Text = Level.ToString();
                    }
                }
            }
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
    }
}
