using System;
using System.Windows.Forms;
using System.Windows.Media;
using System.Collections.Generic;

using wclCommon;
using wclBluetooth;
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

        private wclWeDoHub GetHub()
        {
            if (lvHubs.SelectedItems.Count == 0)
                return null;

            Int64 Address = Convert.ToInt64(lvHubs.SelectedItems[0].Text, 16);
            return FRobot[Address];
        }

        private wclWeDoHub GetSelectedHub()
        {
            wclWeDoHub Hub = GetHub();
            if (Hub == null)
                MessageBox.Show("Select Hub");
            return Hub;
        }

        private void RemoveTabs()
        {
            foreach (TabPage Tab in pcHub.TabPages)
                pcHub.TabPages.Remove(Tab);
        }

        private void DisplayDeviceInforValue(Label label, String Value, Int32 Res)
        {
            // Helper function to show information value.
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

        private void UpdateBattLevel(Byte Level)
        {
            laBattLevel.Text = Level.ToString() + " %";
        }

        private void UpdateHubInfo(wclWeDoHub Hub)
        {
            String Value;
            Int32 Res = Hub.DeviceInformation.ReadFirmwareVersion(out Value);
            DisplayDeviceInforValue(laFirmwareVersion, Value, Res);
            Res = Hub.DeviceInformation.ReadHardwareVersion(out Value);
            DisplayDeviceInforValue(laHardwareVersion, Value, Res);
            Res = Hub.DeviceInformation.ReadSoftwareVersion(out Value);
            DisplayDeviceInforValue(laSoftwareVersion, Value, Res);
            Res = Hub.DeviceInformation.ReadManufacturerName(out Value);
            DisplayDeviceInforValue(laManufacturerName, Value, Res);

            switch (Hub.BatteryType)
            {
                case wclWeDoBatteryType.btRechargeable:
                    laBatteryType.Text = "Rechargeable";
                    break;
                case wclWeDoBatteryType.btStandard:
                    laBatteryType.Text = "Standard";
                    break;
                case wclWeDoBatteryType.btUnknown:
                    laBatteryType.Text = "Unknown";
                    break;
                default:
                    laBatteryType.Text = "Undefined";
                    break;
            }

            String Name;
            Res = Hub.ReadDeviceName(out Name);
            if (Res == wclErrors.WCL_E_SUCCESS)
                laDeviceName.Text = Name;
            else
            {
                if (Res == wclBluetoothErrors.WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND)
                    laDeviceName.Text = "<unsupported>";
                else
                    laDeviceName.Text = "<error>";
            }

            Byte Level;
            Res = Hub.BatteryLevel.ReadBatteryLevel(out Level);
            if (Res == wclErrors.WCL_E_SUCCESS)
                UpdateBattLevel(Level);

            Boolean Pressed;
            Res = Hub.ReadButtonState(out Pressed);
            if (Res == wclErrors.WCL_E_SUCCESS)
                laButtonPressed.Visible = Pressed;

            laLowSignal.Visible = false;
            laLowVoltage.Visible = false;
            laHighCurrent.Visible = false;
        }

        private void UpdateDeviceInfo(Label Version, Label Internal, Label ConnectionId, wclWeDoIo Device)
        {
            Version.Text = "Firmware: " +
                Device.FirmwareVersion.MajorVersion.ToString() + "." +
                Device.FirmwareVersion.MinorVersion.ToString() + "." +
                Device.FirmwareVersion.BuildNumber.ToString() + "." +
                Device.FirmwareVersion.BugFixVersion.ToString() +
                "  Hardware: " +
                Device.HardwareVersion.MajorVersion.ToString() + "." +
                Device.HardwareVersion.MinorVersion.ToString() + "." +
                Device.HardwareVersion.BuildNumber.ToString() + "." +
                Device.HardwareVersion.BugFixVersion.ToString();
            if (Device.Internal)
                Internal.Text = "Internal";
            else
                Internal.Text = "External";
            ConnectionId.Text = "Connection ID: " + Device.ConnectionId.ToString() +
                "  Port ID: " + Device.PortId.ToString();
        }

        private void UpdateCurrent(wclWeDoCurrentSensor Sensor)
        {
            UpdateDeviceInfo(laCurrentVersion, laCurrentDeviceType, laCurrentConnectionId, Sensor);
            laCurrent.Text = Sensor.Current.ToString() + " mA";
        }

        private void UpdateVoltage(wclWeDoVoltageSensor Sensor)
        {
            UpdateDeviceInfo(laVoltageVersion, laVoltageDeviceType, laVoltageConnectionId, Sensor);
            laVoltage.Text = Sensor.Voltage.ToString() + " mV";
        }

        private void UpdateRgbMode(wclWeDoRgbLight Rgb)
        {
            switch (Rgb.Mode)
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

        private void UpdateRgbColors(wclWeDoRgbLight Rgb)
        {
            Color c = Rgb.Color;
            edR.Text = c.R.ToString();
            edG.Text = c.G.ToString();
            edB.Text = c.B.ToString();
        }

        private void UpdateRgbIndex(wclWeDoRgbLight Rgb)
        {
            cbColorIndex.SelectedIndex = (Int32)Rgb.ColorIndex;
        }

        private void UpdateRgb(wclWeDoRgbLight Sensor)
        {
            UpdateDeviceInfo(laRgbVersion, laRgbDeviceType, laRgbConnectionId, Sensor);

            UpdateRgbColors(Sensor);
            UpdateRgbIndex(Sensor);
            UpdateRgbMode(Sensor);
        }

        private void UpdatePiezo(wclWeDoPiezo Sensor)
        {
            UpdateDeviceInfo(laPiezoVersion, laPiezoDeviceType, laPiezoConnectionId, Sensor);
        }

        private void UpdateMotion(wclWeDoMotionSensor Sensor)
        {
            if (Sensor.PortId == 0)
            {
                UpdateDeviceInfo(laMotionVersion1, laMotionDeviceType1, laMotionConnectionId1, Sensor);
                laCount1.Text = Sensor.Count.ToString();
                laDistance1.Text = Sensor.Distance.ToString();
            }
            else
            {
                UpdateDeviceInfo(laMotionVersion2, laMotionDeviceType2, laMotionConnectionId2, Sensor);
                laCount2.Text = Sensor.Count.ToString();
                laDistance2.Text = Sensor.Distance.ToString();
            }
        }

        private void UpdateTilt(wclWeDoTiltSensor Sensor)
        {
            if (Sensor.PortId == 0)
                UpdateDeviceInfo(laTiltVersion1, laTiltDeviceType1, laTiltConnectionId1, Sensor);
            else
                UpdateDeviceInfo(laTiltVersion2, laTiltDeviceType2, laTiltConnectionId2, Sensor);
            UpdateTiltMode(Sensor);
            UpdateDirection(Sensor);
            UpdateCrash(Sensor);
            UpdateAngle(Sensor);
        }

        private void UpdateMotor(wclWeDoMotor Sensor)
        {
            if (Sensor.PortId == 0)
                UpdateDeviceInfo(laMotorVersion1, laMotorDeviceType1, laMotorConnectionId1, Sensor);
            else
                UpdateDeviceInfo(laMotorVersion2, laMotorDeviceType2, laMotorConnectionId2, Sensor);
        }

        private void UpdateTabs()
        {
            wclWeDoHub Hub = GetHub();
            if (Hub == null)
                RemoveTabs();
            else
            {
                AddPage(tsHubInfo);
                UpdateHubInfo(Hub);

                wclWeDoCurrentSensor Current = FRobot.GetCurrentSensor(Hub);
                if (Current != null && Current.Attached)
                {
                    AddPage(tsCurrent);
                    UpdateCurrent(Current);
                }

                wclWeDoVoltageSensor Voltage = FRobot.GetVoltageSensor(Hub);
                if (Voltage != null && Voltage.Attached)
                {
                    AddPage(tsVoltage);
                    UpdateVoltage(Voltage);
                }

                wclWeDoRgbLight Rgb = FRobot.GetRgbDevice(Hub);
                if (Rgb != null && Rgb.Attached)
                {
                    AddPage(tsRgb);
                    UpdateRgb(Rgb);
                }

                wclWeDoPiezo Piezo = FRobot.GetPiezoDevice(Hub);
                if (Piezo != null && Piezo.Attached)
                {
                    AddPage(tsPiezo);
                    UpdatePiezo(Piezo);
                }

                wclWeDoMotionSensor Motion = GetMotionSensor(0);
                if (Motion != null && Motion.Attached)
                {
                    AddPage(tsMotion1);
                    UpdateMotion(Motion);
                }

                Motion = GetMotionSensor(1);
                if (Motion != null && Motion.Attached)
                {
                    AddPage(tsMotion2);
                    UpdateMotion(Motion);
                }

                wclWeDoTiltSensor Tilt = GetTiltSensor(0);
                if (Tilt != null && Tilt.Attached)
                {
                    AddPage(tsTilt1);
                    UpdateTilt(Tilt);
                }

                Tilt = GetTiltSensor(1);
                if (Tilt != null && Tilt.Attached)
                {
                    AddPage(tsTilt2);
                    UpdateTilt(Tilt);
                }

                wclWeDoMotor Motor = GetMotor(0);
                if (Motor != null && Motor.Attached)
                {
                    AddPage(tsMotor1);
                    UpdateMotor(Motor);
                }

                Motor = GetMotor(1);
                if (Motor != null && Motor.Attached)
                {
                    AddPage(tsMotor2);
                    UpdateMotor(Motor);
                }
            }
        }

        private void FmMain_Load(object sender, EventArgs e)
        {
            FRobot = new wclWeDoRobot();
            FRobot.OnStarted += FRobot_OnStarted;
            FRobot.OnStopped += FRobot_OnStopped;
            FRobot.OnHubFound += FRobot_OnHubFound;
            FRobot.OnHubConnected += FRobot_OnHubConnected;
            FRobot.OnHubDisconnected += FRobot_OnHubDisconnected;

            RemoveTabs();

            cbNote.SelectedIndex = 0;
            cbOctave.SelectedIndex = 0;
            cbMode1.SelectedIndex = 0;
            cbMode2.SelectedIndex = 0;
            cbTiltMode1.SelectedIndex = 0;
            cbTiltMode2.SelectedIndex = 0;
            cbMotorDirection1.SelectedIndex = 0;
            cbMotorDirection2.SelectedIndex = 0;
        }

        private void FRobot_OnHubConnected(object Sender, wclWeDoHub Hub, int Error)
        {
            String Address = Hub.Address.ToString("X12");
            if (Error == wclErrors.WCL_E_SUCCESS)
            {
                lbLog.Items.Add("Hub " + Address + " connected.");
                lvHubs.Items.Add(Address);

                Hub.BatteryLevel.OnBatteryLevelChanged += BatteryLevel_OnBatteryLevelChanged;
                Hub.OnLowSignalAlert += Hub_OnLowSignalAlert;
                Hub.OnHighCurrentAlert += Hub_OnHighCurrentAlert;
                Hub.OnLowVoltageAlert += Hub_OnLowVoltageAlert;
                Hub.OnButtonStateChanged += Hub_OnButtonStateChanged;
                Hub.OnDeviceAttached += Hub_OnDeviceAttached;
                Hub.OnDeviceDetached += Hub_OnDeviceDetached;
            }
            else
                lbLog.Items.Add("Hub " + Address + " connect error: 0x" + Error.ToString("X8"));
        }

        private void RemovePage(TabPage Page)
        {
            if (pcHub.TabPages.Contains(Page))
                pcHub.TabPages.Remove(Page);
        }

        private void AddPage(TabPage Page)
        {
            if (!pcHub.TabPages.Contains(Page))
                pcHub.TabPages.Add(Page);
        }

        private void Hub_OnDeviceDetached(object Sender, wclWeDoIo Device)
        {
            wclWeDoHub Hub = GetHub();
            if (Hub != null && Hub.Address == Device.Hub.Address)
            {
                switch (Device.DeviceType)
                {
                    case wclWeDoIoDeviceType.iodCurrentSensor:
                        RemovePage(tsCurrent);
                        break;

                    case wclWeDoIoDeviceType.iodVoltageSensor:
                        RemovePage(tsVoltage);
                        break;

                    case wclWeDoIoDeviceType.iodRgb:
                        RemovePage(tsRgb);
                        break;

                    case wclWeDoIoDeviceType.iodWeDo20Piezo:
                        RemovePage(tsPiezo);
                        break;

                    case wclWeDoIoDeviceType.iodWeDo20MotionSensor:
                        if (Device.PortId == 0)
                            RemovePage(tsMotion1);
                        else
                            RemovePage(tsMotion2);
                        break;

                    case wclWeDoIoDeviceType.iodWeDo20TiltSensor:
                        if (Device.PortId == 0)
                            RemovePage(tsTilt1);
                        else
                            RemovePage(tsTilt2);
                        break;

                    case wclWeDoIoDeviceType.iodMediumMotor:
                        if (Device.PortId == 0)
                            RemovePage(tsMotor1);
                        else
                            RemovePage(tsMotor2);
                        break;
                }
            }
        }

        private void ConnectEvents(wclWeDoIo Device)
        {
            switch (Device.DeviceType)
            {
                case wclWeDoIoDeviceType.iodCurrentSensor:
                    (Device as wclWeDoCurrentSensor).OnCurrentChanged += FmMain_OnCurrentChanged;
                    break;

                case wclWeDoIoDeviceType.iodVoltageSensor:
                    (Device as wclWeDoVoltageSensor).OnVoltageChanged += FmMain_OnVoltageChanged;
                    break;

                case wclWeDoIoDeviceType.iodRgb:
                    wclWeDoRgbLight Rgb = (Device as wclWeDoRgbLight);
                    Rgb.OnColorChanged += FmMain_OnColorChanged;
                    Rgb.OnModeChanged += FmMain_OnModeChanged;
                    break;

                case wclWeDoIoDeviceType.iodWeDo20MotionSensor:
                    wclWeDoMotionSensor Motion = (Device as wclWeDoMotionSensor);
                    Motion.OnCountChanged += FmMain_OnCountChanged;
                    Motion.OnDistanceChanged += FmMain_OnDistanceChanged;
                    Motion.OnModeChanged += FmMain_OnMotionModeChanged;
                    break;

                case wclWeDoIoDeviceType.iodWeDo20TiltSensor:
                    wclWeDoTiltSensor Tilt = (Device as wclWeDoTiltSensor);
                    Tilt.OnModeChanged += Tilt_OnModeChanged;
                    Tilt.OnDirectionChanged += Tilt_OnDirectionChanged;
                    Tilt.OnCrashChanged += Tilt_OnCrashChanged;
                    Tilt.OnAngleChanged += Tilt_OnAngleChanged;
                    break;
            }
        }

        private void UpdateAngle(wclWeDoTiltSensor Sensor)
        {
            wclWeDoTiltSensorAngle Angle = Sensor.Angle;
            if (Sensor.PortId == 0)
            {
                laX1.Text = Angle.X.ToString();
                laY1.Text = Angle.Y.ToString();
                laZ1.Text = "0";
                laDirection1.Text = "Unknown";
            }
            else
            {
                laX2.Text = Angle.X.ToString();
                laY2.Text = Angle.Y.ToString();
                laZ2.Text = "0";
                laDirection2.Text = "Unknown";
            }
        }

        private void UpdateCrash(wclWeDoTiltSensor Sensor)
        {
            wclWeDoTiltSensorCrash Crash = Sensor.Crash;
            if (Sensor.PortId == 0)
            {
                laX1.Text = Crash.X.ToString();
                laY1.Text = Crash.Y.ToString();
                laZ1.Text = Crash.Z.ToString();
                laDirection1.Text = "Unknown";
            }
            else
            {
                laX2.Text = Crash.X.ToString();
                laY2.Text = Crash.Y.ToString();
                laZ2.Text = Crash.Z.ToString();
                laDirection2.Text = "Unknown";
            }
        }

        private void UpdateDirection(wclWeDoTiltSensor Sensor)
        {
            if (Sensor.PortId == 0)
            {
                switch (Sensor.Direction)
                {
                    case wclWeDoTiltSensorDirection.tdBackward:
                        laDirection1.Text = "Backward";
                        break;
                    case wclWeDoTiltSensorDirection.tdForward:
                        laDirection1.Text = "Forward";
                        break;
                    case wclWeDoTiltSensorDirection.tdLeft:
                        laDirection1.Text = "Left";
                        break;
                    case wclWeDoTiltSensorDirection.tdNeutral:
                        laDirection1.Text = "Neutral";
                        break;
                    case wclWeDoTiltSensorDirection.tdRight:
                        laDirection1.Text = "Right";
                        break;
                    default:
                        laDirection1.Text = "Unknown";
                        break;
                }
                laX1.Text = "0";
                laY1.Text = "0";
                laZ1.Text = "0";
            }
            else
            {
                switch (Sensor.Direction)
                {
                    case wclWeDoTiltSensorDirection.tdBackward:
                        laDirection2.Text = "Backward";
                        break;
                    case wclWeDoTiltSensorDirection.tdForward:
                        laDirection2.Text = "Forward";
                        break;
                    case wclWeDoTiltSensorDirection.tdLeft:
                        laDirection2.Text = "Left";
                        break;
                    case wclWeDoTiltSensorDirection.tdNeutral:
                        laDirection2.Text = "Neutral";
                        break;
                    case wclWeDoTiltSensorDirection.tdRight:
                        laDirection2.Text = "Right";
                        break;
                    default:
                        laDirection2.Text = "Unknown";
                        break;
                }
                laX2.Text = "0";
                laY2.Text = "0";
                laZ2.Text = "0";
            }
        }

        private void UpdateTiltMode(wclWeDoTiltSensor Sensor)
        {
            switch (Sensor.Mode)
            {
                case wclWeDoTiltSensorMode.tmAngle:
                    if (Sensor.PortId == 0)
                        cbTiltMode1.SelectedIndex = 0;
                    else
                        cbTiltMode2.SelectedIndex = 0;
                    break;
                case wclWeDoTiltSensorMode.tmTilt:
                    if (Sensor.PortId == 0)
                        cbTiltMode1.SelectedIndex = 1;
                    else
                        cbTiltMode2.SelectedIndex = 1;
                    break;
                case wclWeDoTiltSensorMode.tmCrash:
                    if (Sensor.PortId == 0)
                        cbTiltMode1.SelectedIndex = 2;
                    else
                        cbTiltMode2.SelectedIndex = 2;
                    break;
                default:
                    if (Sensor.PortId == 0)
                        cbTiltMode1.SelectedIndex = -1;
                    else
                        cbTiltMode2.SelectedIndex = -1;
                    break;
            }
        }

        private void Tilt_OnAngleChanged(object sender, EventArgs e)
        {
            wclWeDoHub Hub = GetHub();
            wclWeDoTiltSensor Sensor = (sender as wclWeDoTiltSensor);
            if (Hub != null && Hub.Address == Sensor.Hub.Address)
                UpdateAngle(Sensor);
        }

        private void Tilt_OnCrashChanged(object sender, EventArgs e)
        {
            wclWeDoHub Hub = GetHub();
            wclWeDoTiltSensor Sensor = (sender as wclWeDoTiltSensor);
            if (Hub != null && Hub.Address == Sensor.Hub.Address)
                UpdateCrash(Sensor);
        }

        private void Tilt_OnDirectionChanged(object sender, EventArgs e)
        {
            wclWeDoHub Hub = GetHub();
            wclWeDoTiltSensor Sensor = (sender as wclWeDoTiltSensor);
            if (Hub != null && Hub.Address == Sensor.Hub.Address)
                UpdateDirection(Sensor);
        }

        private void Tilt_OnModeChanged(object sender, EventArgs e)
        {
            wclWeDoHub Hub = GetHub();
            wclWeDoTiltSensor Sensor = (sender as wclWeDoTiltSensor);
            if (Hub != null && Hub.Address == Sensor.Hub.Address)
                UpdateTiltMode(Sensor);
        }

        private void Hub_OnDeviceAttached(object Sender, wclWeDoIo Device)
        {
            ConnectEvents(Device);

            wclWeDoHub Hub = GetHub();
            if (Hub != null && Hub.Address == Device.Hub.Address)
            {
                switch (Device.DeviceType)
                {
                    case wclWeDoIoDeviceType.iodCurrentSensor:
                        AddPage(tsCurrent);
                        UpdateCurrent(Device as wclWeDoCurrentSensor);
                        break;

                    case wclWeDoIoDeviceType.iodVoltageSensor:
                        AddPage(tsVoltage);
                        UpdateVoltage(Device as wclWeDoVoltageSensor);
                        break;

                    case wclWeDoIoDeviceType.iodRgb:
                        AddPage(tsRgb);
                        UpdateRgb(Device as wclWeDoRgbLight);
                        break;

                    case wclWeDoIoDeviceType.iodWeDo20Piezo:
                        AddPage(tsPiezo);
                        UpdatePiezo(Device as wclWeDoPiezo);
                        break;

                    case wclWeDoIoDeviceType.iodWeDo20MotionSensor:
                        if (Device.PortId == 0)
                            AddPage(tsMotion1);
                        else
                            AddPage(tsMotion2);
                        UpdateMotion(Device as wclWeDoMotionSensor);
                        break;

                    case wclWeDoIoDeviceType.iodWeDo20TiltSensor:
                        if (Device.PortId == 0)
                            AddPage(tsTilt1);
                        else
                            AddPage(tsTilt2);
                        UpdateTilt(Device as wclWeDoTiltSensor);
                        break;

                    case wclWeDoIoDeviceType.iodMediumMotor:
                        if (Device.PortId == 0)
                            AddPage(tsMotor1);
                        else
                            AddPage(tsMotor2);
                        UpdateMotor(Device as wclWeDoMotor);
                        break;
                }
            }
        }

        private void FmMain_OnMotionModeChanged(object sender, EventArgs e)
        {
            wclWeDoHub Hub = GetHub();
            wclWeDoMotionSensor Sensor = (sender as wclWeDoMotionSensor);
            if (Hub != null && Hub.Address == Sensor.Hub.Address)
            {
                switch (Sensor.Mode)
                {
                    case wclWeDoMotionSensorMode.mmDetect:
                        if (Sensor.PortId == 0)
                            cbMode1.SelectedIndex = 0;
                        else
                            cbMode2.SelectedIndex = 0;
                        break;
                    case wclWeDoMotionSensorMode.mmCount:
                        if (Sensor.PortId == 0)
                            cbMode1.SelectedIndex = 1;
                        else
                            cbMode2.SelectedIndex = 1;
                        break;
                    default:
                        if (Sensor.PortId == 0)
                            cbMode1.SelectedIndex = -1;
                        else
                            cbMode2.SelectedIndex = -1;
                        break;
                }
            }
        }

        private void FmMain_OnDistanceChanged(object sender, EventArgs e)
        {
            wclWeDoHub Hub = GetHub();
            wclWeDoMotionSensor Sensor = (sender as wclWeDoMotionSensor);
            if (Hub != null && Hub.Address == Sensor.Hub.Address)
            {
                if (Sensor.PortId == 0)
                    laDistance1.Text = Sensor.Distance.ToString();
                else
                    laDistance2.Text = Sensor.Distance.ToString();
            }
        }

        private void FmMain_OnCountChanged(object sender, EventArgs e)
        {
            wclWeDoHub Hub = GetHub();
            wclWeDoMotionSensor Sensor = (sender as wclWeDoMotionSensor);
            if (Hub != null && Hub.Address == Sensor.Hub.Address)
            {
                if (Sensor.PortId == 0)
                    laCount1.Text = Sensor.Count.ToString();
                else
                    laCount2.Text = Sensor.Count.ToString();
            }
        }

        private void FmMain_OnModeChanged(object sender, EventArgs e)
        {
            wclWeDoHub Hub = GetHub();
            if (Hub != null && Hub.Address == (sender as wclWeDoRgbLight).Hub.Address)
                UpdateRgbMode(sender as wclWeDoRgbLight);
        }

        private void FmMain_OnColorChanged(object sender, EventArgs e)
        {
            wclWeDoHub Hub = GetHub();
            if (Hub != null && Hub.Address == (sender as wclWeDoRgbLight).Hub.Address)
            {
                UpdateRgbColors(sender as wclWeDoRgbLight);
                UpdateRgbIndex(sender as wclWeDoRgbLight);
            }
        }

        private void FmMain_OnVoltageChanged(object sender, EventArgs e)
        {
            wclWeDoHub Hub = GetHub();
            if (Hub != null && Hub.Address == (sender as wclWeDoIo).Hub.Address)
                UpdateVoltage(sender as wclWeDoVoltageSensor);
        }

        private void FmMain_OnCurrentChanged(object sender, EventArgs e)
        {
            wclWeDoHub Hub = GetHub();
            if (Hub != null && Hub.Address == (sender as wclWeDoIo).Hub.Address)
                UpdateCurrent(sender as wclWeDoCurrentSensor);
        }

        private void Hub_OnButtonStateChanged(object Sender, bool Pressed)
        {
            wclWeDoHub Hub = GetHub();
            if (Hub != null && (Sender as wclWeDoHub).Address == Hub.Address)
                laButtonPressed.Visible = Pressed;
        }

        private void Hub_OnLowVoltageAlert(object Sender, bool Alert)
        {
            wclWeDoHub Hub = GetHub();
            if (Hub != null && (Sender as wclWeDoHub).Address == Hub.Address)
                laLowVoltage.Visible = Alert;
        }

        private void Hub_OnHighCurrentAlert(object Sender, bool Alert)
        {
            wclWeDoHub Hub = GetHub();
            if (Hub != null && (Sender as wclWeDoHub).Address == Hub.Address)
                laHighCurrent.Visible = Alert;
        }

        private void Hub_OnLowSignalAlert(object Sender, bool Alert)
        {
            wclWeDoHub Hub = GetHub();
            if (Hub != null && (Sender as wclWeDoHub).Address == Hub.Address)
                laLowSignal.Visible = Alert;
        }

        private void BatteryLevel_OnBatteryLevelChanged(object Sender, byte Level)
        {
            wclWeDoHub Hub = GetHub();
            if (Hub != null && Hub.Address == (Sender as wclWeDoBatteryLevelService).Hub.Address)
                UpdateBattLevel(Level);
        }

        private void FRobot_OnHubDisconnected(object Sender, wclWeDoHub Hub, int Reason)
        {
            String Address = Hub.Address.ToString("X12");
            lbLog.Items.Add("Hub " + Address + " disconnected by reason: 0x" + Reason.ToString("X8"));
            foreach (ListViewItem Item in lvHubs.Items)
            {
                if (Item.Text == Address)
                {
                    lvHubs.Items.Remove(Item);
                    break;
                }
            }
        }

        private void FRobot_OnHubFound(object Sender, long Address, string Name, out bool Connect)
        {
            lbLog.Items.Add("Hub " + Address.ToString("X12") + " (" + Name + ") found");
            Connect = true;
        }

        private void FRobot_OnStopped(object sender, EventArgs e)
        {
            lbLog.Items.Add("Robot stopped");
            btStop.Enabled = false;
            btStart.Enabled = true;
        }

        private void FRobot_OnStarted(object sender, EventArgs e)
        {
            lbLog.Items.Add("Robot started");
            btStop.Enabled = true;
            btStart.Enabled = false;
        }

        private void FmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            FRobot.Stop();
            FRobot = null;
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            lbLog.Items.Clear();
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            Int32 Res = FRobot.Start();
            if (Res != wclErrors.WCL_E_SUCCESS)
                MessageBox.Show("Start failed: 0x" + Res.ToString("X8"));
            else
                btStart.Enabled = false;
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            Int32 Res = FRobot.Stop();
            if (Res != wclErrors.WCL_E_SUCCESS)
                MessageBox.Show("Stop failed: 0x" + Res.ToString("X8"));
            else
                btStop.Enabled = false;
        }

        private void lvHubs_SelectedIndexChanged(object sender, EventArgs e)
        {
            btDisconnect.Enabled = (lvHubs.SelectedItems.Count > 0);
            UpdateTabs();
        }

        private void btDisconnect_Click(object sender, EventArgs e)
        {
            wclWeDoHub Hub = GetSelectedHub();
            if (Hub != null)
            {
                Int32 Res = Hub.TurnOff();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Disconnect failed: 0x" + Res.ToString("X8"));
            }
        }

        private void btSetDeviceName_Click(object sender, EventArgs e)
        {
            wclWeDoHub Hub = GetHub();
            if (Hub != null)
            {
                Int32 Res = Hub.WriteDeviceName(edDeviceName.Text);
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Write hub name failed with error: 0x" + Res.ToString("X8"));
            }
        }

        private void btSetRgb_Click(object sender, EventArgs e)
        {
            wclWeDoHub Hub = GetHub();
            wclWeDoRgbLight Rgb = FRobot.GetRgbDevice(Hub);
            if (Rgb != null)
            {
                Color c = Color.FromRgb(Convert.ToByte(edR.Text), Convert.ToByte(edG.Text),
                    Convert.ToByte(edB.Text));
                Int32 Res = Rgb.SetColor(c);
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Unable to set color: 0x" + Res.ToString("X8"));
            }
        }

        private void btSetDefault_Click(object sender, EventArgs e)
        {
            wclWeDoHub Hub = GetHub();
            wclWeDoRgbLight Rgb = FRobot.GetRgbDevice(Hub);
            if (Rgb != null)
            {
                Int32 Res = Rgb.SwitchToDefaultColor();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Unable to set default color: 0x" + Res.ToString("X8"));
            }
        }

        private void btTurnOff_Click(object sender, EventArgs e)
        {
            wclWeDoHub Hub = GetHub();
            wclWeDoRgbLight Rgb = FRobot.GetRgbDevice(Hub);
            if (Rgb != null)
            {
                Int32 Res = Rgb.SwitchOff();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Unable to switch LED off: 0x" + Res.ToString("X8"));
            }
        }

        private void btSetIndex_Click(object sender, EventArgs e)
        {
            wclWeDoHub Hub = GetHub();
            wclWeDoRgbLight Rgb = FRobot.GetRgbDevice(Hub);
            if (Rgb != null)
            {
                Int32 Res = Rgb.SetColorIndex((wclWeDoColor)cbColorIndex.SelectedIndex);
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Unable to set color index: 0x" + Res.ToString("X8"));
            }
        }

        private void btSetMode_Click(object sender, EventArgs e)
        {
            wclWeDoHub Hub = GetHub();
            wclWeDoRgbLight Rgb = FRobot.GetRgbDevice(Hub);
            if (Rgb != null)
            {
                Boolean RgbEnabled = (cbColorMode.SelectedIndex == 1);
                Boolean IndexEnabled = (cbColorMode.SelectedIndex == 0);

                Int32 Res = wclErrors.WCL_E_SUCCESS;
                if (RgbEnabled)
                    Res = Rgb.SetMode(wclWeDoRgbLightMode.lmAbsolute);
                else
                {
                    if (IndexEnabled)
                        Res = Rgb.SetMode(wclWeDoRgbLightMode.lmDiscrete);
                }

                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Unable to change color mode: 0x" + Res.ToString("X8"));
                else
                {
                    if (RgbEnabled)
                        UpdateRgbColors(Rgb);
                    else
                    {
                        if (IndexEnabled)
                            UpdateRgbIndex(Rgb);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            wclWeDoHub Hub = GetHub();
            wclWeDoPiezo Piezo = FRobot.GetPiezoDevice(Hub);
            if (Piezo != null)
            {
                Int32 Res = Piezo.StopPlaying();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Stop failed: 0x" + Res.ToString("X8"));
            }
        }

        private void btPlay_Click(object sender, EventArgs e)
        {
            wclWeDoHub Hub = GetHub();
            wclWeDoPiezo Piezo = FRobot.GetPiezoDevice(Hub);
            if (Piezo != null)
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
                Int32 Res = Piezo.PlayNote(Note, (Byte)(cbOctave.SelectedIndex + 1), Convert.ToUInt16(edDuration.Text));
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Play failed: 0x" + Res.ToString("X8"));
            }
        }

        private wclWeDoMotionSensor GetMotionSensor(Byte Port)
        {
            wclWeDoHub Hub = GetHub();
            List<wclWeDoMotionSensor> Sensors = FRobot.GetMotionSensors(Hub);
            foreach (wclWeDoMotionSensor Sensor in Sensors)
            {
                if (Sensor.PortId == Port)
                    return Sensor;
            }
            return null;
        }

        private wclWeDoTiltSensor GetTiltSensor(Byte Port)
        {
            wclWeDoHub Hub = GetHub();
            List<wclWeDoTiltSensor> Sensors = FRobot.GetTiltSensors(Hub);
            foreach (wclWeDoTiltSensor Sensor in Sensors)
            {
                if (Sensor.PortId == Port)
                    return Sensor;
            }
            return null;
        }

        private wclWeDoMotor GetMotor(Byte Port)
        {
            wclWeDoHub Hub = GetHub();
            List<wclWeDoMotor> Sensors = FRobot.GetMotors(Hub);
            foreach (wclWeDoMotor Sensor in Sensors)
            {
                if (Sensor.PortId == Port)
                    return Sensor;
            }
            return null;
        }

        private void ChangeMotionMode(Byte Port)
        {
            wclWeDoMotionSensor Motion = GetMotionSensor(Port);
            if (Motion != null)
            {
                wclWeDoMotionSensorMode Mode;
                Int32 Index;
                if (Port == 0)
                    Index = cbMode1.SelectedIndex;
                else
                    Index = cbMode2.SelectedIndex;
                switch (Index)
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
                    Int32 Res = Motion.SetMode(Mode);
                    if (Res != wclErrors.WCL_E_SUCCESS)
                        MessageBox.Show("Mode change failed: 0x" + Res.ToString("X8"));
                }
            }
        }

        private void ResetMotion(Byte Port)
        {
            wclWeDoMotionSensor Motion = GetMotionSensor(Port);
            if (Motion != null)
            {
                Int32 Res = Motion.Reset();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Reset failed; 0x" + Res.ToString("X8"));
            }
        }

        private void btChange1_Click(object sender, EventArgs e)
        {
            ChangeMotionMode(0);
        }

        private void btReset1_Click(object sender, EventArgs e)
        {
            ResetMotion(0);
        }

        private void btChange2_Click(object sender, EventArgs e)
        {
            ChangeMotionMode(1);
        }

        private void btReset2_Click(object sender, EventArgs e)
        {
            ResetMotion(1);
        }

        private void ChangeTiltMode(Byte Port)
        {
            wclWeDoTiltSensor Tilt = GetTiltSensor(Port);
            if (Tilt != null)
            {
                wclWeDoTiltSensorMode Mode;
                Int32 Index;
                if (Port == 0)
                    Index = cbTiltMode1.SelectedIndex;
                else
                    Index = cbTiltMode2.SelectedIndex;
                switch (Index)
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
                    Int32 Res = Tilt.SetMode(Mode);
                    if (Res != wclErrors.WCL_E_SUCCESS)
                        MessageBox.Show("Mode change failed: 0x" + Res.ToString("X8"));
                }
            }
        }

        private void ResetTilt(Byte Port)
        {
            wclWeDoTiltSensor Tilt = GetTiltSensor(Port);
            if (Tilt != null)
            {
                Int32 Res = Tilt.Reset();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Reset failed; 0x" + Res.ToString("X8"));
            }
        }

        private void btChangeTilt1_Click(object sender, EventArgs e)
        {
            ChangeTiltMode(0);
        }

        private void btChangeTilt2_Click(object sender, EventArgs e)
        {
            ChangeTiltMode(1);
        }

        private void btResetTilt1_Click(object sender, EventArgs e)
        {
            ResetTilt(0);
        }

        private void btResetTilt2_Click(object sender, EventArgs e)
        {
            ResetTilt(1);
        }

        private void StartMotor(Byte Port, ComboBox Direction, TextBox Power)
        {
            wclWeDoMotor Motor = GetMotor(Port);
            if (Motor != null)
            {
                wclWeDoMotorDirection Dir;
                switch (Direction.SelectedIndex)
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
                Int32 Res = Motor.Run(Dir, Convert.ToByte(Power.Text));
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Start motor failed: 0x" + Res.ToString("X8"));
            }
        }

        private void btStart1_Click(object sender, EventArgs e)
        {
            StartMotor(0, cbMotorDirection1, edPower1);
        }

        private void btStart2_Click(object sender, EventArgs e)
        {
            StartMotor(1, cbMotorDirection2, edPower2);
        }

        private void BreakMotor(Byte Port)
        {
            wclWeDoMotor Motor = GetMotor(Port);
            if (Motor != null)
            {
                Int32 Res = Motor.Brake();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Brake failed; 0x" + Res.ToString("X8"));
            }
        }

        private void btBrake1_Click(object sender, EventArgs e)
        {
            BreakMotor(0);
        }

        private void btBrake2_Click(object sender, EventArgs e)
        {
            BreakMotor(1);
        }

        private void DriftMotor(Byte Port)
        {
            wclWeDoMotor Motor = GetMotor(Port);
            if (Motor != null)
            {
                Int32 Res = Motor.Drift();
                if (Res != wclErrors.WCL_E_SUCCESS)
                    MessageBox.Show("Drift failed; 0x" + Res.ToString("X8"));
            }
        }

        private void btDrift1_Click(object sender, EventArgs e)
        {
            DriftMotor(0);
        }

        private void btDrift2_Click(object sender, EventArgs e)
        {
            DriftMotor(1);
        }
    }
}