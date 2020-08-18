using System;
using System.Windows.Forms;
using System.Windows.Media;

using wclCommon;
using wclWeDoFramework;

namespace WeDoRobot
{
    public partial class fmMain : Form
    {
        private wclWeDoRobot FRobot;

        public fmMain()
        {
            InitializeComponent();
        }

        private void FmMain_Load(object sender, EventArgs e)
        {
            FRobot = new wclWeDoRobot();
            FRobot.OnHubFound += FRobot_OnHubFound;
            FRobot.OnHubConnected += FRobot_OnHubConnected;
            FRobot.OnHubDisconnected += FRobot_OnHubDisconnected;
            FRobot.OnStarted += FRobot_OnStarted;
            FRobot.OnStopped += FRobot_OnStopped;
        }

        private void FRobot_OnStopped(object sender, EventArgs e)
        {
            lbLog.Items.Add("Stopped");
        }

        private void FRobot_OnStarted(object sender, EventArgs e)
        {
            lbLog.Items.Add("Started");
        }

        private void FRobot_OnHubDisconnected(object Sender, wclWeDoHub Hub, int Reason)
        {
            String Address = Hub.Address.ToString("X12");
            foreach (ListViewItem Item in lvHubs.Items)
            {
                if (Item.Text == Address)
                {
                    lvHubs.Items.Remove(Item);
                    lbLog.Items.Add("Hub " + Address + " disconnected with reason: 0x" + Reason.ToString("X8"));
                    break;
                }
            }
        }

        private void FRobot_OnHubConnected(object Sender, wclWeDoHub Hub, int Error)
        {
            if (Error == wclErrors.WCL_E_SUCCESS)
            {
                lbLog.Items.Add("Hub " + Hub.Address.ToString("X12") + " connected");
                ListViewItem Item = lvHubs.Items.Add(Hub.Address.ToString("X12"));
                Item.Tag = Hub;
                Hub.OnButtonStateChanged += Hub_OnButtonStateChanged;
            }
            else
                lbLog.Items.Add("Hub " + Hub.Address.ToString("X12") + " connect failed: 0x" + Error.ToString("X8"));
        }

        private void Hub_OnButtonStateChanged(object Sender, bool Pressed)
        {
            if (Pressed)
                lbLog.Items.Add("Button pressed");
            else
                lbLog.Items.Add("Button released");
        }

        private void FRobot_OnHubFound(object Sender, long Address, string Name, out bool Connect)
        {
            lbLog.Items.Add("Hub found: " + Address.ToString("X12") + ", " + Name);
            // Accept any hub!
            Connect = true;
        }

        private void FmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            FRobot.Stop();
            FRobot = null;
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            Int32 Res = FRobot.Start();
            if (Res != wclErrors.WCL_E_SUCCESS)
                MessageBox.Show("Start failed: 0x" + Res.ToString("X8"));
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            Int32 Res = FRobot.Stop();
            if (Res != wclErrors.WCL_E_SUCCESS)
                MessageBox.Show("Stop failed: 0x" + Res.ToString("X8"));
        }

        private wclWeDoHub GetHub()
        {
            if (lvHubs.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select HUB");
                return null;
            }

            Int64 Address = Convert.ToInt64(lvHubs.SelectedItems[0].Text, 16);
            foreach (wclWeDoHub Hub in FRobot.Hubs)
            {
                if (Hub.Address == Address)
                    return Hub;
            }

            return null;
        }

        private void btDisconnect_Click(object sender, EventArgs e)
        {
            wclWeDoHub Hub = GetHub();
            if (Hub != null)
                Hub.TurnOff();
        }

        private wclWeDoRgbLight GetRgb(wclWeDoHub Hub)
        {
            foreach (wclWeDoIo Device in Hub.IoDevices)
            {
                if (Device.DeviceType == wclWeDoIoDeviceType.iodRgb)
                    return Device as wclWeDoRgbLight;
            }

            MessageBox.Show("RGB not found");
            return null;
        }

        private void TurnRgb(Boolean state)
        {
            wclWeDoHub Hub = GetHub();
            if (Hub != null)
            {
                wclWeDoRgbLight Rgb = GetRgb(Hub);
                if (Rgb != null)
                {
                    Rgb.SetMode(wclWeDoRgbLightMode.lmAbsolute);
                    if (state)
                        Rgb.SetColor(Color.FromRgb(255, 255, 255));
                    else
                        Rgb.SetColor(Color.FromRgb(0, 0, 0));
                }
            }
        }

        private void btLedOn_Click(object sender, EventArgs e)
        {
            TurnRgb(true);
        }

        private void btLedOff_Click(object sender, EventArgs e)
        {
            TurnRgb(false);
        }
    }
}
