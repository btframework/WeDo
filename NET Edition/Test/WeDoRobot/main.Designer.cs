namespace WeDoRobot
{
    partial class fmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btStart = new System.Windows.Forms.Button();
            this.btStop = new System.Windows.Forms.Button();
            this.lbLog = new System.Windows.Forms.ListBox();
            this.btClear = new System.Windows.Forms.Button();
            this.lvHubs = new System.Windows.Forms.ListView();
            this.chHubAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btDisconnect = new System.Windows.Forms.Button();
            this.pcHub = new System.Windows.Forms.TabControl();
            this.tsHubInfo = new System.Windows.Forms.TabPage();
            this.laButtonPressed = new System.Windows.Forms.Label();
            this.laHighCurrent = new System.Windows.Forms.Label();
            this.laLowSignal = new System.Windows.Forms.Label();
            this.laLowVoltage = new System.Windows.Forms.Label();
            this.laNewName = new System.Windows.Forms.Label();
            this.btSetDeviceName = new System.Windows.Forms.Button();
            this.edDeviceName = new System.Windows.Forms.TextBox();
            this.laBattLevel = new System.Windows.Forms.Label();
            this.laBattLevelTitle = new System.Windows.Forms.Label();
            this.laDeviceName = new System.Windows.Forms.Label();
            this.laDeviceNameCaption = new System.Windows.Forms.Label();
            this.laBatteryType = new System.Windows.Forms.Label();
            this.laBattTypeTitle = new System.Windows.Forms.Label();
            this.laManufacturerName = new System.Windows.Forms.Label();
            this.laSoftwareVersion = new System.Windows.Forms.Label();
            this.laHardwareVersion = new System.Windows.Forms.Label();
            this.laFirmwareVersion = new System.Windows.Forms.Label();
            this.laManufacturerNameTitle = new System.Windows.Forms.Label();
            this.laSoftwareVersionTitle = new System.Windows.Forms.Label();
            this.laHardwareVersionTitle = new System.Windows.Forms.Label();
            this.laFirmwareVersionTitle = new System.Windows.Forms.Label();
            this.laDeviceInformationTitle = new System.Windows.Forms.Label();
            this.tsCurrent = new System.Windows.Forms.TabPage();
            this.laCurrent = new System.Windows.Forms.Label();
            this.laCurrentCaption = new System.Windows.Forms.Label();
            this.laCurrentConnectionId = new System.Windows.Forms.Label();
            this.laCurrentConnectionIdCaption = new System.Windows.Forms.Label();
            this.laCurrentDeviceType = new System.Windows.Forms.Label();
            this.laCurrentDeviceTypeCaption = new System.Windows.Forms.Label();
            this.laCurrentVersion = new System.Windows.Forms.Label();
            this.laCurrentVersionCaption = new System.Windows.Forms.Label();
            this.laCurrentDeviceInfo = new System.Windows.Forms.Label();
            this.tsVoltage = new System.Windows.Forms.TabPage();
            this.laVoltage = new System.Windows.Forms.Label();
            this.laVoltageCaption = new System.Windows.Forms.Label();
            this.laVoltageConnectionId = new System.Windows.Forms.Label();
            this.laVoltageConnectionIdCaption = new System.Windows.Forms.Label();
            this.laVoltageDeviceType = new System.Windows.Forms.Label();
            this.laVoltageDeviceTypeCaption = new System.Windows.Forms.Label();
            this.laVoltageVersion = new System.Windows.Forms.Label();
            this.laVoltageVersionCaption = new System.Windows.Forms.Label();
            this.laVotageDeviceInformation = new System.Windows.Forms.Label();
            this.tsPiezo = new System.Windows.Forms.TabPage();
            this.btStopSound = new System.Windows.Forms.Button();
            this.btPlay = new System.Windows.Forms.Button();
            this.edDuration = new System.Windows.Forms.TextBox();
            this.laDuration = new System.Windows.Forms.Label();
            this.cbOctave = new System.Windows.Forms.ComboBox();
            this.laOctave = new System.Windows.Forms.Label();
            this.cbNote = new System.Windows.Forms.ComboBox();
            this.laNote = new System.Windows.Forms.Label();
            this.laPiezoConnectionId = new System.Windows.Forms.Label();
            this.laPiezoConnectionIdCaption = new System.Windows.Forms.Label();
            this.laPiezoDeviceType = new System.Windows.Forms.Label();
            this.laPiezoDeviceTypeCaption = new System.Windows.Forms.Label();
            this.laPiezoVersion = new System.Windows.Forms.Label();
            this.laPiezoVersionCaption = new System.Windows.Forms.Label();
            this.laPiezoDeviceInformation = new System.Windows.Forms.Label();
            this.tsRgb = new System.Windows.Forms.TabPage();
            this.btTurnOff = new System.Windows.Forms.Button();
            this.btSetDefault = new System.Windows.Forms.Button();
            this.cbColorIndex = new System.Windows.Forms.ComboBox();
            this.btSetIndex = new System.Windows.Forms.Button();
            this.laColorIndex = new System.Windows.Forms.Label();
            this.btSetRgb = new System.Windows.Forms.Button();
            this.edB = new System.Windows.Forms.TextBox();
            this.edG = new System.Windows.Forms.TextBox();
            this.edR = new System.Windows.Forms.TextBox();
            this.laB = new System.Windows.Forms.Label();
            this.laG = new System.Windows.Forms.Label();
            this.laR = new System.Windows.Forms.Label();
            this.btSetMode = new System.Windows.Forms.Button();
            this.cbColorMode = new System.Windows.Forms.ComboBox();
            this.laColorMode = new System.Windows.Forms.Label();
            this.laRgbConnectionId = new System.Windows.Forms.Label();
            this.laRgbConnectionIdCaption = new System.Windows.Forms.Label();
            this.laRgbDeviceType = new System.Windows.Forms.Label();
            this.laRgbDeviceTypeCaption = new System.Windows.Forms.Label();
            this.laRgbVersion = new System.Windows.Forms.Label();
            this.laRgbVersionCaption = new System.Windows.Forms.Label();
            this.laRgbDeviceInformation = new System.Windows.Forms.Label();
            this.tsMotion1 = new System.Windows.Forms.TabPage();
            this.btReset1 = new System.Windows.Forms.Button();
            this.laDistance1 = new System.Windows.Forms.Label();
            this.laDistanceTitle1 = new System.Windows.Forms.Label();
            this.laCount1 = new System.Windows.Forms.Label();
            this.laCountTitle1 = new System.Windows.Forms.Label();
            this.btChange1 = new System.Windows.Forms.Button();
            this.cbMode1 = new System.Windows.Forms.ComboBox();
            this.laMode1 = new System.Windows.Forms.Label();
            this.laMotionConnectionId1 = new System.Windows.Forms.Label();
            this.laMotionConnectionIdCaption1 = new System.Windows.Forms.Label();
            this.laMotionDeviceType1 = new System.Windows.Forms.Label();
            this.laMotionDeviceTypeCaption1 = new System.Windows.Forms.Label();
            this.laMotionVersion1 = new System.Windows.Forms.Label();
            this.laMotionVersionCaption1 = new System.Windows.Forms.Label();
            this.laMotionDeviceInformation1 = new System.Windows.Forms.Label();
            this.tsMotion2 = new System.Windows.Forms.TabPage();
            this.btReset2 = new System.Windows.Forms.Button();
            this.laDistance2 = new System.Windows.Forms.Label();
            this.laDistanceTitle2 = new System.Windows.Forms.Label();
            this.laCount2 = new System.Windows.Forms.Label();
            this.laCountTitle2 = new System.Windows.Forms.Label();
            this.btChange2 = new System.Windows.Forms.Button();
            this.cbMode2 = new System.Windows.Forms.ComboBox();
            this.laMode2 = new System.Windows.Forms.Label();
            this.laMotionConnectionId2 = new System.Windows.Forms.Label();
            this.laMotionConnectionIdCaption2 = new System.Windows.Forms.Label();
            this.laMotionDeviceType2 = new System.Windows.Forms.Label();
            this.laMotionDeviceTypeCaption2 = new System.Windows.Forms.Label();
            this.laMotionVersion2 = new System.Windows.Forms.Label();
            this.laMotionVersionCaption2 = new System.Windows.Forms.Label();
            this.laMotionDeviceInformation2 = new System.Windows.Forms.Label();
            this.tsTilt1 = new System.Windows.Forms.TabPage();
            this.laZ1 = new System.Windows.Forms.Label();
            this.laZTitle1 = new System.Windows.Forms.Label();
            this.laY1 = new System.Windows.Forms.Label();
            this.laYTitle1 = new System.Windows.Forms.Label();
            this.laX1 = new System.Windows.Forms.Label();
            this.laXTitle1 = new System.Windows.Forms.Label();
            this.laDirection1 = new System.Windows.Forms.Label();
            this.laDirectionTitle1 = new System.Windows.Forms.Label();
            this.btResetTilt1 = new System.Windows.Forms.Button();
            this.btChangeTilt1 = new System.Windows.Forms.Button();
            this.cbTiltMode1 = new System.Windows.Forms.ComboBox();
            this.laTiltMode1 = new System.Windows.Forms.Label();
            this.laTiltConnectionId1 = new System.Windows.Forms.Label();
            this.laTiltConnectionCaption1 = new System.Windows.Forms.Label();
            this.laTiltDeviceType1 = new System.Windows.Forms.Label();
            this.laTiltDeviceTypeCaption1 = new System.Windows.Forms.Label();
            this.laTiltVersion1 = new System.Windows.Forms.Label();
            this.laTiltVersionCaption1 = new System.Windows.Forms.Label();
            this.laTiltDeviceInformation1 = new System.Windows.Forms.Label();
            this.tsTilt2 = new System.Windows.Forms.TabPage();
            this.laZ2 = new System.Windows.Forms.Label();
            this.laZTitle2 = new System.Windows.Forms.Label();
            this.laY2 = new System.Windows.Forms.Label();
            this.laYTitle2 = new System.Windows.Forms.Label();
            this.laX2 = new System.Windows.Forms.Label();
            this.laXTitle2 = new System.Windows.Forms.Label();
            this.laDirection2 = new System.Windows.Forms.Label();
            this.laDirectionTitle2 = new System.Windows.Forms.Label();
            this.btResetTilt2 = new System.Windows.Forms.Button();
            this.btChangeTilt2 = new System.Windows.Forms.Button();
            this.cbTiltMode2 = new System.Windows.Forms.ComboBox();
            this.laTiltMode2 = new System.Windows.Forms.Label();
            this.laTiltConnectionId2 = new System.Windows.Forms.Label();
            this.laTiltConnectionCaption2 = new System.Windows.Forms.Label();
            this.laTiltDeviceType2 = new System.Windows.Forms.Label();
            this.laTiltDeviceTypeCaption2 = new System.Windows.Forms.Label();
            this.laTiltVersion2 = new System.Windows.Forms.Label();
            this.laTiltVersionCaption2 = new System.Windows.Forms.Label();
            this.laTiltDeviceInformation2 = new System.Windows.Forms.Label();
            this.tsMotor1 = new System.Windows.Forms.TabPage();
            this.btDrift1 = new System.Windows.Forms.Button();
            this.btBrake1 = new System.Windows.Forms.Button();
            this.btStart1 = new System.Windows.Forms.Button();
            this.edPower1 = new System.Windows.Forms.TextBox();
            this.laPower1 = new System.Windows.Forms.Label();
            this.cbMotorDirection1 = new System.Windows.Forms.ComboBox();
            this.laMotorDirectionCaption1 = new System.Windows.Forms.Label();
            this.laMotorConnectionId1 = new System.Windows.Forms.Label();
            this.laMotorConnectionIdCaption1 = new System.Windows.Forms.Label();
            this.laMotorDeviceType1 = new System.Windows.Forms.Label();
            this.laMotorDeviceTypeCaption1 = new System.Windows.Forms.Label();
            this.laMotorVersion1 = new System.Windows.Forms.Label();
            this.laMotorVersionCaption1 = new System.Windows.Forms.Label();
            this.laMotorDeviceInformation1 = new System.Windows.Forms.Label();
            this.tsMotor2 = new System.Windows.Forms.TabPage();
            this.btDrift2 = new System.Windows.Forms.Button();
            this.btBrake2 = new System.Windows.Forms.Button();
            this.btStart2 = new System.Windows.Forms.Button();
            this.edPower2 = new System.Windows.Forms.TextBox();
            this.laPower2 = new System.Windows.Forms.Label();
            this.cbMotorDirection2 = new System.Windows.Forms.ComboBox();
            this.laMotorDirectionCaption2 = new System.Windows.Forms.Label();
            this.laMotorConnectionId2 = new System.Windows.Forms.Label();
            this.laMotorConnectionIdCaption2 = new System.Windows.Forms.Label();
            this.laMotorDeviceType2 = new System.Windows.Forms.Label();
            this.laMotorDeviceTypeCaption2 = new System.Windows.Forms.Label();
            this.laMotorVersion2 = new System.Windows.Forms.Label();
            this.laMotorVersionCaption2 = new System.Windows.Forms.Label();
            this.laMotorDeviceInformation2 = new System.Windows.Forms.Label();
            this.pcHub.SuspendLayout();
            this.tsHubInfo.SuspendLayout();
            this.tsCurrent.SuspendLayout();
            this.tsVoltage.SuspendLayout();
            this.tsPiezo.SuspendLayout();
            this.tsRgb.SuspendLayout();
            this.tsMotion1.SuspendLayout();
            this.tsMotion2.SuspendLayout();
            this.tsTilt1.SuspendLayout();
            this.tsTilt2.SuspendLayout();
            this.tsMotor1.SuspendLayout();
            this.tsMotor2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btStart
            // 
            this.btStart.Location = new System.Drawing.Point(12, 12);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(75, 23);
            this.btStart.TabIndex = 0;
            this.btStart.Text = "Start";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // btStop
            // 
            this.btStop.Enabled = false;
            this.btStop.Location = new System.Drawing.Point(93, 12);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(75, 23);
            this.btStop.TabIndex = 1;
            this.btStop.Text = "Stop";
            this.btStop.UseVisualStyleBackColor = true;
            this.btStop.Click += new System.EventHandler(this.btStop_Click);
            // 
            // lbLog
            // 
            this.lbLog.FormattingEnabled = true;
            this.lbLog.Location = new System.Drawing.Point(12, 435);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(489, 121);
            this.lbLog.TabIndex = 2;
            // 
            // btClear
            // 
            this.btClear.Location = new System.Drawing.Point(426, 406);
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(75, 23);
            this.btClear.TabIndex = 3;
            this.btClear.Text = "Clear";
            this.btClear.UseVisualStyleBackColor = true;
            this.btClear.Click += new System.EventHandler(this.btClear_Click);
            // 
            // lvHubs
            // 
            this.lvHubs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chHubAddress});
            this.lvHubs.FullRowSelect = true;
            this.lvHubs.GridLines = true;
            this.lvHubs.HideSelection = false;
            this.lvHubs.Location = new System.Drawing.Point(12, 41);
            this.lvHubs.MultiSelect = false;
            this.lvHubs.Name = "lvHubs";
            this.lvHubs.Size = new System.Drawing.Size(408, 97);
            this.lvHubs.TabIndex = 4;
            this.lvHubs.UseCompatibleStateImageBehavior = false;
            this.lvHubs.View = System.Windows.Forms.View.Details;
            this.lvHubs.SelectedIndexChanged += new System.EventHandler(this.lvHubs_SelectedIndexChanged);
            // 
            // chHubAddress
            // 
            this.chHubAddress.Text = "Address";
            this.chHubAddress.Width = 300;
            // 
            // btDisconnect
            // 
            this.btDisconnect.Enabled = false;
            this.btDisconnect.Location = new System.Drawing.Point(428, 41);
            this.btDisconnect.Name = "btDisconnect";
            this.btDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btDisconnect.TabIndex = 5;
            this.btDisconnect.Text = "Disconnect";
            this.btDisconnect.UseVisualStyleBackColor = true;
            this.btDisconnect.Click += new System.EventHandler(this.btDisconnect_Click);
            // 
            // pcHub
            // 
            this.pcHub.Controls.Add(this.tsHubInfo);
            this.pcHub.Controls.Add(this.tsCurrent);
            this.pcHub.Controls.Add(this.tsVoltage);
            this.pcHub.Controls.Add(this.tsPiezo);
            this.pcHub.Controls.Add(this.tsRgb);
            this.pcHub.Controls.Add(this.tsMotion1);
            this.pcHub.Controls.Add(this.tsMotion2);
            this.pcHub.Controls.Add(this.tsTilt1);
            this.pcHub.Controls.Add(this.tsTilt2);
            this.pcHub.Controls.Add(this.tsMotor1);
            this.pcHub.Controls.Add(this.tsMotor2);
            this.pcHub.Location = new System.Drawing.Point(12, 144);
            this.pcHub.Name = "pcHub";
            this.pcHub.SelectedIndex = 0;
            this.pcHub.Size = new System.Drawing.Size(408, 285);
            this.pcHub.TabIndex = 6;
            // 
            // tsHubInfo
            // 
            this.tsHubInfo.Controls.Add(this.laButtonPressed);
            this.tsHubInfo.Controls.Add(this.laHighCurrent);
            this.tsHubInfo.Controls.Add(this.laLowSignal);
            this.tsHubInfo.Controls.Add(this.laLowVoltage);
            this.tsHubInfo.Controls.Add(this.laNewName);
            this.tsHubInfo.Controls.Add(this.btSetDeviceName);
            this.tsHubInfo.Controls.Add(this.edDeviceName);
            this.tsHubInfo.Controls.Add(this.laBattLevel);
            this.tsHubInfo.Controls.Add(this.laBattLevelTitle);
            this.tsHubInfo.Controls.Add(this.laDeviceName);
            this.tsHubInfo.Controls.Add(this.laDeviceNameCaption);
            this.tsHubInfo.Controls.Add(this.laBatteryType);
            this.tsHubInfo.Controls.Add(this.laBattTypeTitle);
            this.tsHubInfo.Controls.Add(this.laManufacturerName);
            this.tsHubInfo.Controls.Add(this.laSoftwareVersion);
            this.tsHubInfo.Controls.Add(this.laHardwareVersion);
            this.tsHubInfo.Controls.Add(this.laFirmwareVersion);
            this.tsHubInfo.Controls.Add(this.laManufacturerNameTitle);
            this.tsHubInfo.Controls.Add(this.laSoftwareVersionTitle);
            this.tsHubInfo.Controls.Add(this.laHardwareVersionTitle);
            this.tsHubInfo.Controls.Add(this.laFirmwareVersionTitle);
            this.tsHubInfo.Controls.Add(this.laDeviceInformationTitle);
            this.tsHubInfo.Location = new System.Drawing.Point(4, 22);
            this.tsHubInfo.Name = "tsHubInfo";
            this.tsHubInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tsHubInfo.Size = new System.Drawing.Size(400, 259);
            this.tsHubInfo.TabIndex = 0;
            this.tsHubInfo.Text = "Hub";
            this.tsHubInfo.UseVisualStyleBackColor = true;
            // 
            // laButtonPressed
            // 
            this.laButtonPressed.AutoSize = true;
            this.laButtonPressed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laButtonPressed.Location = new System.Drawing.Point(291, 136);
            this.laButtonPressed.Name = "laButtonPressed";
            this.laButtonPressed.Size = new System.Drawing.Size(92, 13);
            this.laButtonPressed.TabIndex = 46;
            this.laButtonPressed.Text = "Button pressed";
            this.laButtonPressed.Visible = false;
            // 
            // laHighCurrent
            // 
            this.laHighCurrent.AutoSize = true;
            this.laHighCurrent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laHighCurrent.ForeColor = System.Drawing.Color.Red;
            this.laHighCurrent.Location = new System.Drawing.Point(303, 86);
            this.laHighCurrent.Name = "laHighCurrent";
            this.laHighCurrent.Size = new System.Drawing.Size(81, 13);
            this.laHighCurrent.TabIndex = 45;
            this.laHighCurrent.Text = "High current!";
            this.laHighCurrent.Visible = false;
            // 
            // laLowSignal
            // 
            this.laLowSignal.AutoSize = true;
            this.laLowSignal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laLowSignal.ForeColor = System.Drawing.Color.Red;
            this.laLowSignal.Location = new System.Drawing.Point(303, 62);
            this.laLowSignal.Name = "laLowSignal";
            this.laLowSignal.Size = new System.Drawing.Size(71, 13);
            this.laLowSignal.TabIndex = 44;
            this.laLowSignal.Text = "Low signal!";
            this.laLowSignal.Visible = false;
            // 
            // laLowVoltage
            // 
            this.laLowVoltage.AutoSize = true;
            this.laLowVoltage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laLowVoltage.ForeColor = System.Drawing.Color.Red;
            this.laLowVoltage.Location = new System.Drawing.Point(303, 38);
            this.laLowVoltage.Name = "laLowVoltage";
            this.laLowVoltage.Size = new System.Drawing.Size(80, 13);
            this.laLowVoltage.TabIndex = 43;
            this.laLowVoltage.Text = "Low voltage!";
            this.laLowVoltage.Visible = false;
            // 
            // laNewName
            // 
            this.laNewName.AutoSize = true;
            this.laNewName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laNewName.Location = new System.Drawing.Point(15, 213);
            this.laNewName.Name = "laNewName";
            this.laNewName.Size = new System.Drawing.Size(70, 13);
            this.laNewName.TabIndex = 42;
            this.laNewName.Text = "New name:";
            // 
            // btSetDeviceName
            // 
            this.btSetDeviceName.Location = new System.Drawing.Point(251, 207);
            this.btSetDeviceName.Name = "btSetDeviceName";
            this.btSetDeviceName.Size = new System.Drawing.Size(75, 23);
            this.btSetDeviceName.TabIndex = 41;
            this.btSetDeviceName.Text = "Change";
            this.btSetDeviceName.UseVisualStyleBackColor = true;
            this.btSetDeviceName.Click += new System.EventHandler(this.btSetDeviceName_Click);
            // 
            // edDeviceName
            // 
            this.edDeviceName.Location = new System.Drawing.Point(91, 209);
            this.edDeviceName.Name = "edDeviceName";
            this.edDeviceName.Size = new System.Drawing.Size(154, 20);
            this.edDeviceName.TabIndex = 40;
            // 
            // laBattLevel
            // 
            this.laBattLevel.AutoSize = true;
            this.laBattLevel.Location = new System.Drawing.Point(106, 162);
            this.laBattLevel.Name = "laBattLevel";
            this.laBattLevel.Size = new System.Drawing.Size(24, 13);
            this.laBattLevel.TabIndex = 39;
            this.laBattLevel.Text = "0 %";
            // 
            // laBattLevelTitle
            // 
            this.laBattLevelTitle.AutoSize = true;
            this.laBattLevelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laBattLevelTitle.Location = new System.Drawing.Point(15, 162);
            this.laBattLevelTitle.Name = "laBattLevelTitle";
            this.laBattLevelTitle.Size = new System.Drawing.Size(82, 13);
            this.laBattLevelTitle.TabIndex = 38;
            this.laBattLevelTitle.Text = "Battery level:";
            // 
            // laDeviceName
            // 
            this.laDeviceName.AutoSize = true;
            this.laDeviceName.Location = new System.Drawing.Point(106, 187);
            this.laDeviceName.Name = "laDeviceName";
            this.laDeviceName.Size = new System.Drawing.Size(63, 13);
            this.laDeviceName.TabIndex = 37;
            this.laDeviceName.Text = "<unknonw>";
            // 
            // laDeviceNameCaption
            // 
            this.laDeviceNameCaption.AutoSize = true;
            this.laDeviceNameCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laDeviceNameCaption.Location = new System.Drawing.Point(15, 187);
            this.laDeviceNameCaption.Name = "laDeviceNameCaption";
            this.laDeviceNameCaption.Size = new System.Drawing.Size(85, 13);
            this.laDeviceNameCaption.TabIndex = 36;
            this.laDeviceNameCaption.Text = "Device name:";
            // 
            // laBatteryType
            // 
            this.laBatteryType.AutoSize = true;
            this.laBatteryType.Location = new System.Drawing.Point(106, 136);
            this.laBatteryType.Name = "laBatteryType";
            this.laBatteryType.Size = new System.Drawing.Size(56, 13);
            this.laBatteryType.TabIndex = 35;
            this.laBatteryType.Text = "Undefined";
            // 
            // laBattTypeTitle
            // 
            this.laBattTypeTitle.AutoSize = true;
            this.laBattTypeTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laBattTypeTitle.Location = new System.Drawing.Point(15, 136);
            this.laBattTypeTitle.Name = "laBattTypeTitle";
            this.laBattTypeTitle.Size = new System.Drawing.Size(79, 13);
            this.laBattTypeTitle.TabIndex = 34;
            this.laBattTypeTitle.Text = "Battery type:";
            // 
            // laManufacturerName
            // 
            this.laManufacturerName.AutoSize = true;
            this.laManufacturerName.Location = new System.Drawing.Point(147, 110);
            this.laManufacturerName.Name = "laManufacturerName";
            this.laManufacturerName.Size = new System.Drawing.Size(47, 13);
            this.laManufacturerName.TabIndex = 29;
            this.laManufacturerName.Text = "<empty>";
            // 
            // laSoftwareVersion
            // 
            this.laSoftwareVersion.AutoSize = true;
            this.laSoftwareVersion.Location = new System.Drawing.Point(147, 86);
            this.laSoftwareVersion.Name = "laSoftwareVersion";
            this.laSoftwareVersion.Size = new System.Drawing.Size(47, 13);
            this.laSoftwareVersion.TabIndex = 28;
            this.laSoftwareVersion.Text = "<empty>";
            // 
            // laHardwareVersion
            // 
            this.laHardwareVersion.AutoSize = true;
            this.laHardwareVersion.Location = new System.Drawing.Point(147, 62);
            this.laHardwareVersion.Name = "laHardwareVersion";
            this.laHardwareVersion.Size = new System.Drawing.Size(47, 13);
            this.laHardwareVersion.TabIndex = 27;
            this.laHardwareVersion.Text = "<empty>";
            // 
            // laFirmwareVersion
            // 
            this.laFirmwareVersion.AutoSize = true;
            this.laFirmwareVersion.Location = new System.Drawing.Point(147, 38);
            this.laFirmwareVersion.Name = "laFirmwareVersion";
            this.laFirmwareVersion.Size = new System.Drawing.Size(47, 13);
            this.laFirmwareVersion.TabIndex = 26;
            this.laFirmwareVersion.Text = "<empty>";
            // 
            // laManufacturerNameTitle
            // 
            this.laManufacturerNameTitle.AutoSize = true;
            this.laManufacturerNameTitle.Location = new System.Drawing.Point(37, 110);
            this.laManufacturerNameTitle.Name = "laManufacturerNameTitle";
            this.laManufacturerNameTitle.Size = new System.Drawing.Size(102, 13);
            this.laManufacturerNameTitle.TabIndex = 25;
            this.laManufacturerNameTitle.Text = "Manufacturer name:";
            // 
            // laSoftwareVersionTitle
            // 
            this.laSoftwareVersionTitle.AutoSize = true;
            this.laSoftwareVersionTitle.Location = new System.Drawing.Point(37, 86);
            this.laSoftwareVersionTitle.Name = "laSoftwareVersionTitle";
            this.laSoftwareVersionTitle.Size = new System.Drawing.Size(89, 13);
            this.laSoftwareVersionTitle.TabIndex = 24;
            this.laSoftwareVersionTitle.Text = "Software version:";
            // 
            // laHardwareVersionTitle
            // 
            this.laHardwareVersionTitle.AutoSize = true;
            this.laHardwareVersionTitle.Location = new System.Drawing.Point(37, 62);
            this.laHardwareVersionTitle.Name = "laHardwareVersionTitle";
            this.laHardwareVersionTitle.Size = new System.Drawing.Size(93, 13);
            this.laHardwareVersionTitle.TabIndex = 23;
            this.laHardwareVersionTitle.Text = "Hardware version:";
            // 
            // laFirmwareVersionTitle
            // 
            this.laFirmwareVersionTitle.AutoSize = true;
            this.laFirmwareVersionTitle.Location = new System.Drawing.Point(37, 38);
            this.laFirmwareVersionTitle.Name = "laFirmwareVersionTitle";
            this.laFirmwareVersionTitle.Size = new System.Drawing.Size(89, 13);
            this.laFirmwareVersionTitle.TabIndex = 22;
            this.laFirmwareVersionTitle.Text = "Firmware version:";
            // 
            // laDeviceInformationTitle
            // 
            this.laDeviceInformationTitle.AutoSize = true;
            this.laDeviceInformationTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laDeviceInformationTitle.Location = new System.Drawing.Point(15, 13);
            this.laDeviceInformationTitle.Name = "laDeviceInformationTitle";
            this.laDeviceInformationTitle.Size = new System.Drawing.Size(113, 13);
            this.laDeviceInformationTitle.TabIndex = 21;
            this.laDeviceInformationTitle.Text = "Device information";
            // 
            // tsCurrent
            // 
            this.tsCurrent.Controls.Add(this.laCurrent);
            this.tsCurrent.Controls.Add(this.laCurrentCaption);
            this.tsCurrent.Controls.Add(this.laCurrentConnectionId);
            this.tsCurrent.Controls.Add(this.laCurrentConnectionIdCaption);
            this.tsCurrent.Controls.Add(this.laCurrentDeviceType);
            this.tsCurrent.Controls.Add(this.laCurrentDeviceTypeCaption);
            this.tsCurrent.Controls.Add(this.laCurrentVersion);
            this.tsCurrent.Controls.Add(this.laCurrentVersionCaption);
            this.tsCurrent.Controls.Add(this.laCurrentDeviceInfo);
            this.tsCurrent.Location = new System.Drawing.Point(4, 22);
            this.tsCurrent.Name = "tsCurrent";
            this.tsCurrent.Padding = new System.Windows.Forms.Padding(3);
            this.tsCurrent.Size = new System.Drawing.Size(400, 259);
            this.tsCurrent.TabIndex = 1;
            this.tsCurrent.Text = "Current";
            this.tsCurrent.UseVisualStyleBackColor = true;
            // 
            // laCurrent
            // 
            this.laCurrent.AutoSize = true;
            this.laCurrent.Location = new System.Drawing.Point(73, 130);
            this.laCurrent.Name = "laCurrent";
            this.laCurrent.Size = new System.Drawing.Size(31, 13);
            this.laCurrent.TabIndex = 38;
            this.laCurrent.Text = "0 mA";
            // 
            // laCurrentCaption
            // 
            this.laCurrentCaption.AutoSize = true;
            this.laCurrentCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laCurrentCaption.Location = new System.Drawing.Point(6, 130);
            this.laCurrentCaption.Name = "laCurrentCaption";
            this.laCurrentCaption.Size = new System.Drawing.Size(52, 13);
            this.laCurrentCaption.TabIndex = 37;
            this.laCurrentCaption.Text = "Current:";
            // 
            // laCurrentConnectionId
            // 
            this.laCurrentConnectionId.AutoSize = true;
            this.laCurrentConnectionId.Location = new System.Drawing.Point(114, 83);
            this.laCurrentConnectionId.Name = "laCurrentConnectionId";
            this.laCurrentConnectionId.Size = new System.Drawing.Size(47, 13);
            this.laCurrentConnectionId.TabIndex = 36;
            this.laCurrentConnectionId.Text = "<empty>";
            // 
            // laCurrentConnectionIdCaption
            // 
            this.laCurrentConnectionIdCaption.AutoSize = true;
            this.laCurrentConnectionIdCaption.Location = new System.Drawing.Point(30, 83);
            this.laCurrentConnectionIdCaption.Name = "laCurrentConnectionIdCaption";
            this.laCurrentConnectionIdCaption.Size = new System.Drawing.Size(78, 13);
            this.laCurrentConnectionIdCaption.TabIndex = 35;
            this.laCurrentConnectionIdCaption.Text = "Connection ID:";
            // 
            // laCurrentDeviceType
            // 
            this.laCurrentDeviceType.AutoSize = true;
            this.laCurrentDeviceType.Location = new System.Drawing.Point(114, 59);
            this.laCurrentDeviceType.Name = "laCurrentDeviceType";
            this.laCurrentDeviceType.Size = new System.Drawing.Size(47, 13);
            this.laCurrentDeviceType.TabIndex = 34;
            this.laCurrentDeviceType.Text = "<empty>";
            // 
            // laCurrentDeviceTypeCaption
            // 
            this.laCurrentDeviceTypeCaption.AutoSize = true;
            this.laCurrentDeviceTypeCaption.Location = new System.Drawing.Point(30, 59);
            this.laCurrentDeviceTypeCaption.Name = "laCurrentDeviceTypeCaption";
            this.laCurrentDeviceTypeCaption.Size = new System.Drawing.Size(67, 13);
            this.laCurrentDeviceTypeCaption.TabIndex = 33;
            this.laCurrentDeviceTypeCaption.Text = "Device type:";
            // 
            // laCurrentVersion
            // 
            this.laCurrentVersion.AutoSize = true;
            this.laCurrentVersion.Location = new System.Drawing.Point(114, 36);
            this.laCurrentVersion.Name = "laCurrentVersion";
            this.laCurrentVersion.Size = new System.Drawing.Size(47, 13);
            this.laCurrentVersion.TabIndex = 31;
            this.laCurrentVersion.Text = "<empty>";
            // 
            // laCurrentVersionCaption
            // 
            this.laCurrentVersionCaption.AutoSize = true;
            this.laCurrentVersionCaption.Location = new System.Drawing.Point(30, 36);
            this.laCurrentVersionCaption.Name = "laCurrentVersionCaption";
            this.laCurrentVersionCaption.Size = new System.Drawing.Size(45, 13);
            this.laCurrentVersionCaption.TabIndex = 29;
            this.laCurrentVersionCaption.Text = "Version:";
            // 
            // laCurrentDeviceInfo
            // 
            this.laCurrentDeviceInfo.AutoSize = true;
            this.laCurrentDeviceInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laCurrentDeviceInfo.Location = new System.Drawing.Point(6, 13);
            this.laCurrentDeviceInfo.Name = "laCurrentDeviceInfo";
            this.laCurrentDeviceInfo.Size = new System.Drawing.Size(113, 13);
            this.laCurrentDeviceInfo.TabIndex = 28;
            this.laCurrentDeviceInfo.Text = "Device information";
            // 
            // tsVoltage
            // 
            this.tsVoltage.Controls.Add(this.laVoltage);
            this.tsVoltage.Controls.Add(this.laVoltageCaption);
            this.tsVoltage.Controls.Add(this.laVoltageConnectionId);
            this.tsVoltage.Controls.Add(this.laVoltageConnectionIdCaption);
            this.tsVoltage.Controls.Add(this.laVoltageDeviceType);
            this.tsVoltage.Controls.Add(this.laVoltageDeviceTypeCaption);
            this.tsVoltage.Controls.Add(this.laVoltageVersion);
            this.tsVoltage.Controls.Add(this.laVoltageVersionCaption);
            this.tsVoltage.Controls.Add(this.laVotageDeviceInformation);
            this.tsVoltage.Location = new System.Drawing.Point(4, 22);
            this.tsVoltage.Name = "tsVoltage";
            this.tsVoltage.Padding = new System.Windows.Forms.Padding(3);
            this.tsVoltage.Size = new System.Drawing.Size(400, 259);
            this.tsVoltage.TabIndex = 2;
            this.tsVoltage.Text = "Voltage";
            this.tsVoltage.UseVisualStyleBackColor = true;
            // 
            // laVoltage
            // 
            this.laVoltage.AutoSize = true;
            this.laVoltage.Location = new System.Drawing.Point(73, 131);
            this.laVoltage.Name = "laVoltage";
            this.laVoltage.Size = new System.Drawing.Size(31, 13);
            this.laVoltage.TabIndex = 47;
            this.laVoltage.Text = "0 mV";
            // 
            // laVoltageCaption
            // 
            this.laVoltageCaption.AutoSize = true;
            this.laVoltageCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laVoltageCaption.Location = new System.Drawing.Point(6, 131);
            this.laVoltageCaption.Name = "laVoltageCaption";
            this.laVoltageCaption.Size = new System.Drawing.Size(54, 13);
            this.laVoltageCaption.TabIndex = 46;
            this.laVoltageCaption.Text = "Voltage:";
            // 
            // laVoltageConnectionId
            // 
            this.laVoltageConnectionId.AutoSize = true;
            this.laVoltageConnectionId.Location = new System.Drawing.Point(114, 84);
            this.laVoltageConnectionId.Name = "laVoltageConnectionId";
            this.laVoltageConnectionId.Size = new System.Drawing.Size(47, 13);
            this.laVoltageConnectionId.TabIndex = 45;
            this.laVoltageConnectionId.Text = "<empty>";
            // 
            // laVoltageConnectionIdCaption
            // 
            this.laVoltageConnectionIdCaption.AutoSize = true;
            this.laVoltageConnectionIdCaption.Location = new System.Drawing.Point(30, 84);
            this.laVoltageConnectionIdCaption.Name = "laVoltageConnectionIdCaption";
            this.laVoltageConnectionIdCaption.Size = new System.Drawing.Size(78, 13);
            this.laVoltageConnectionIdCaption.TabIndex = 44;
            this.laVoltageConnectionIdCaption.Text = "Connection ID:";
            // 
            // laVoltageDeviceType
            // 
            this.laVoltageDeviceType.AutoSize = true;
            this.laVoltageDeviceType.Location = new System.Drawing.Point(114, 60);
            this.laVoltageDeviceType.Name = "laVoltageDeviceType";
            this.laVoltageDeviceType.Size = new System.Drawing.Size(47, 13);
            this.laVoltageDeviceType.TabIndex = 43;
            this.laVoltageDeviceType.Text = "<empty>";
            // 
            // laVoltageDeviceTypeCaption
            // 
            this.laVoltageDeviceTypeCaption.AutoSize = true;
            this.laVoltageDeviceTypeCaption.Location = new System.Drawing.Point(30, 60);
            this.laVoltageDeviceTypeCaption.Name = "laVoltageDeviceTypeCaption";
            this.laVoltageDeviceTypeCaption.Size = new System.Drawing.Size(67, 13);
            this.laVoltageDeviceTypeCaption.TabIndex = 42;
            this.laVoltageDeviceTypeCaption.Text = "Device type:";
            // 
            // laVoltageVersion
            // 
            this.laVoltageVersion.AutoSize = true;
            this.laVoltageVersion.Location = new System.Drawing.Point(114, 37);
            this.laVoltageVersion.Name = "laVoltageVersion";
            this.laVoltageVersion.Size = new System.Drawing.Size(47, 13);
            this.laVoltageVersion.TabIndex = 41;
            this.laVoltageVersion.Text = "<empty>";
            // 
            // laVoltageVersionCaption
            // 
            this.laVoltageVersionCaption.AutoSize = true;
            this.laVoltageVersionCaption.Location = new System.Drawing.Point(30, 37);
            this.laVoltageVersionCaption.Name = "laVoltageVersionCaption";
            this.laVoltageVersionCaption.Size = new System.Drawing.Size(45, 13);
            this.laVoltageVersionCaption.TabIndex = 40;
            this.laVoltageVersionCaption.Text = "Version:";
            // 
            // laVotageDeviceInformation
            // 
            this.laVotageDeviceInformation.AutoSize = true;
            this.laVotageDeviceInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laVotageDeviceInformation.Location = new System.Drawing.Point(6, 14);
            this.laVotageDeviceInformation.Name = "laVotageDeviceInformation";
            this.laVotageDeviceInformation.Size = new System.Drawing.Size(113, 13);
            this.laVotageDeviceInformation.TabIndex = 39;
            this.laVotageDeviceInformation.Text = "Device information";
            // 
            // tsPiezo
            // 
            this.tsPiezo.Controls.Add(this.btStopSound);
            this.tsPiezo.Controls.Add(this.btPlay);
            this.tsPiezo.Controls.Add(this.edDuration);
            this.tsPiezo.Controls.Add(this.laDuration);
            this.tsPiezo.Controls.Add(this.cbOctave);
            this.tsPiezo.Controls.Add(this.laOctave);
            this.tsPiezo.Controls.Add(this.cbNote);
            this.tsPiezo.Controls.Add(this.laNote);
            this.tsPiezo.Controls.Add(this.laPiezoConnectionId);
            this.tsPiezo.Controls.Add(this.laPiezoConnectionIdCaption);
            this.tsPiezo.Controls.Add(this.laPiezoDeviceType);
            this.tsPiezo.Controls.Add(this.laPiezoDeviceTypeCaption);
            this.tsPiezo.Controls.Add(this.laPiezoVersion);
            this.tsPiezo.Controls.Add(this.laPiezoVersionCaption);
            this.tsPiezo.Controls.Add(this.laPiezoDeviceInformation);
            this.tsPiezo.Location = new System.Drawing.Point(4, 22);
            this.tsPiezo.Name = "tsPiezo";
            this.tsPiezo.Padding = new System.Windows.Forms.Padding(3);
            this.tsPiezo.Size = new System.Drawing.Size(400, 259);
            this.tsPiezo.TabIndex = 4;
            this.tsPiezo.Text = "Piezo";
            this.tsPiezo.UseVisualStyleBackColor = true;
            // 
            // btStopSound
            // 
            this.btStopSound.Location = new System.Drawing.Point(130, 146);
            this.btStopSound.Name = "btStopSound";
            this.btStopSound.Size = new System.Drawing.Size(75, 23);
            this.btStopSound.TabIndex = 67;
            this.btStopSound.Text = "Stop";
            this.btStopSound.UseVisualStyleBackColor = true;
            this.btStopSound.Click += new System.EventHandler(this.button1_Click);
            // 
            // btPlay
            // 
            this.btPlay.Location = new System.Drawing.Point(49, 146);
            this.btPlay.Name = "btPlay";
            this.btPlay.Size = new System.Drawing.Size(75, 23);
            this.btPlay.TabIndex = 66;
            this.btPlay.Text = "Play";
            this.btPlay.UseVisualStyleBackColor = true;
            this.btPlay.Click += new System.EventHandler(this.btPlay_Click);
            // 
            // edDuration
            // 
            this.edDuration.Location = new System.Drawing.Point(325, 119);
            this.edDuration.Name = "edDuration";
            this.edDuration.Size = new System.Drawing.Size(63, 20);
            this.edDuration.TabIndex = 65;
            this.edDuration.Text = "500";
            // 
            // laDuration
            // 
            this.laDuration.AutoSize = true;
            this.laDuration.Location = new System.Drawing.Point(272, 122);
            this.laDuration.Name = "laDuration";
            this.laDuration.Size = new System.Drawing.Size(47, 13);
            this.laDuration.TabIndex = 64;
            this.laDuration.Text = "Duration";
            // 
            // cbOctave
            // 
            this.cbOctave.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOctave.FormattingEnabled = true;
            this.cbOctave.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.cbOctave.Location = new System.Drawing.Point(185, 119);
            this.cbOctave.Name = "cbOctave";
            this.cbOctave.Size = new System.Drawing.Size(81, 21);
            this.cbOctave.TabIndex = 63;
            // 
            // laOctave
            // 
            this.laOctave.AutoSize = true;
            this.laOctave.Location = new System.Drawing.Point(137, 122);
            this.laOctave.Name = "laOctave";
            this.laOctave.Size = new System.Drawing.Size(42, 13);
            this.laOctave.TabIndex = 62;
            this.laOctave.Text = "Octave";
            // 
            // cbNote
            // 
            this.cbNote.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNote.FormattingEnabled = true;
            this.cbNote.Items.AddRange(new object[] {
            "A",
            "A#",
            "B",
            "C",
            "C#",
            "D",
            "D#",
            "E",
            "F",
            "F#",
            "G",
            "G#"});
            this.cbNote.Location = new System.Drawing.Point(48, 119);
            this.cbNote.Name = "cbNote";
            this.cbNote.Size = new System.Drawing.Size(83, 21);
            this.cbNote.TabIndex = 61;
            // 
            // laNote
            // 
            this.laNote.AutoSize = true;
            this.laNote.Location = new System.Drawing.Point(13, 122);
            this.laNote.Name = "laNote";
            this.laNote.Size = new System.Drawing.Size(30, 13);
            this.laNote.TabIndex = 60;
            this.laNote.Text = "Note";
            // 
            // laPiezoConnectionId
            // 
            this.laPiezoConnectionId.AutoSize = true;
            this.laPiezoConnectionId.Location = new System.Drawing.Point(114, 84);
            this.laPiezoConnectionId.Name = "laPiezoConnectionId";
            this.laPiezoConnectionId.Size = new System.Drawing.Size(47, 13);
            this.laPiezoConnectionId.TabIndex = 59;
            this.laPiezoConnectionId.Text = "<empty>";
            // 
            // laPiezoConnectionIdCaption
            // 
            this.laPiezoConnectionIdCaption.AutoSize = true;
            this.laPiezoConnectionIdCaption.Location = new System.Drawing.Point(30, 84);
            this.laPiezoConnectionIdCaption.Name = "laPiezoConnectionIdCaption";
            this.laPiezoConnectionIdCaption.Size = new System.Drawing.Size(78, 13);
            this.laPiezoConnectionIdCaption.TabIndex = 58;
            this.laPiezoConnectionIdCaption.Text = "Connection ID:";
            // 
            // laPiezoDeviceType
            // 
            this.laPiezoDeviceType.AutoSize = true;
            this.laPiezoDeviceType.Location = new System.Drawing.Point(114, 60);
            this.laPiezoDeviceType.Name = "laPiezoDeviceType";
            this.laPiezoDeviceType.Size = new System.Drawing.Size(47, 13);
            this.laPiezoDeviceType.TabIndex = 57;
            this.laPiezoDeviceType.Text = "<empty>";
            // 
            // laPiezoDeviceTypeCaption
            // 
            this.laPiezoDeviceTypeCaption.AutoSize = true;
            this.laPiezoDeviceTypeCaption.Location = new System.Drawing.Point(30, 60);
            this.laPiezoDeviceTypeCaption.Name = "laPiezoDeviceTypeCaption";
            this.laPiezoDeviceTypeCaption.Size = new System.Drawing.Size(67, 13);
            this.laPiezoDeviceTypeCaption.TabIndex = 56;
            this.laPiezoDeviceTypeCaption.Text = "Device type:";
            // 
            // laPiezoVersion
            // 
            this.laPiezoVersion.AutoSize = true;
            this.laPiezoVersion.Location = new System.Drawing.Point(114, 37);
            this.laPiezoVersion.Name = "laPiezoVersion";
            this.laPiezoVersion.Size = new System.Drawing.Size(47, 13);
            this.laPiezoVersion.TabIndex = 55;
            this.laPiezoVersion.Text = "<empty>";
            // 
            // laPiezoVersionCaption
            // 
            this.laPiezoVersionCaption.AutoSize = true;
            this.laPiezoVersionCaption.Location = new System.Drawing.Point(30, 37);
            this.laPiezoVersionCaption.Name = "laPiezoVersionCaption";
            this.laPiezoVersionCaption.Size = new System.Drawing.Size(45, 13);
            this.laPiezoVersionCaption.TabIndex = 54;
            this.laPiezoVersionCaption.Text = "Version:";
            // 
            // laPiezoDeviceInformation
            // 
            this.laPiezoDeviceInformation.AutoSize = true;
            this.laPiezoDeviceInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laPiezoDeviceInformation.Location = new System.Drawing.Point(6, 14);
            this.laPiezoDeviceInformation.Name = "laPiezoDeviceInformation";
            this.laPiezoDeviceInformation.Size = new System.Drawing.Size(113, 13);
            this.laPiezoDeviceInformation.TabIndex = 53;
            this.laPiezoDeviceInformation.Text = "Device information";
            // 
            // tsRgb
            // 
            this.tsRgb.Controls.Add(this.btTurnOff);
            this.tsRgb.Controls.Add(this.btSetDefault);
            this.tsRgb.Controls.Add(this.cbColorIndex);
            this.tsRgb.Controls.Add(this.btSetIndex);
            this.tsRgb.Controls.Add(this.laColorIndex);
            this.tsRgb.Controls.Add(this.btSetRgb);
            this.tsRgb.Controls.Add(this.edB);
            this.tsRgb.Controls.Add(this.edG);
            this.tsRgb.Controls.Add(this.edR);
            this.tsRgb.Controls.Add(this.laB);
            this.tsRgb.Controls.Add(this.laG);
            this.tsRgb.Controls.Add(this.laR);
            this.tsRgb.Controls.Add(this.btSetMode);
            this.tsRgb.Controls.Add(this.cbColorMode);
            this.tsRgb.Controls.Add(this.laColorMode);
            this.tsRgb.Controls.Add(this.laRgbConnectionId);
            this.tsRgb.Controls.Add(this.laRgbConnectionIdCaption);
            this.tsRgb.Controls.Add(this.laRgbDeviceType);
            this.tsRgb.Controls.Add(this.laRgbDeviceTypeCaption);
            this.tsRgb.Controls.Add(this.laRgbVersion);
            this.tsRgb.Controls.Add(this.laRgbVersionCaption);
            this.tsRgb.Controls.Add(this.laRgbDeviceInformation);
            this.tsRgb.Location = new System.Drawing.Point(4, 22);
            this.tsRgb.Name = "tsRgb";
            this.tsRgb.Padding = new System.Windows.Forms.Padding(3);
            this.tsRgb.Size = new System.Drawing.Size(400, 259);
            this.tsRgb.TabIndex = 3;
            this.tsRgb.Text = "RGB";
            this.tsRgb.UseVisualStyleBackColor = true;
            // 
            // btTurnOff
            // 
            this.btTurnOff.Location = new System.Drawing.Point(151, 195);
            this.btTurnOff.Name = "btTurnOff";
            this.btTurnOff.Size = new System.Drawing.Size(75, 23);
            this.btTurnOff.TabIndex = 67;
            this.btTurnOff.Text = "Turn off";
            this.btTurnOff.UseVisualStyleBackColor = true;
            this.btTurnOff.Click += new System.EventHandler(this.btTurnOff_Click);
            // 
            // btSetDefault
            // 
            this.btSetDefault.Location = new System.Drawing.Point(70, 195);
            this.btSetDefault.Name = "btSetDefault";
            this.btSetDefault.Size = new System.Drawing.Size(75, 23);
            this.btSetDefault.TabIndex = 66;
            this.btSetDefault.Text = "Set default";
            this.btSetDefault.UseVisualStyleBackColor = true;
            this.btSetDefault.Click += new System.EventHandler(this.btSetDefault_Click);
            // 
            // cbColorIndex
            // 
            this.cbColorIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbColorIndex.FormattingEnabled = true;
            this.cbColorIndex.Items.AddRange(new object[] {
            "Black",
            "Pink",
            "Purple",
            "Blue",
            "Sky blue",
            "Teal",
            "Green",
            "Yellow",
            "Orange",
            "Red",
            "White"});
            this.cbColorIndex.Location = new System.Drawing.Point(73, 168);
            this.cbColorIndex.Name = "cbColorIndex";
            this.cbColorIndex.Size = new System.Drawing.Size(104, 21);
            this.cbColorIndex.TabIndex = 65;
            // 
            // btSetIndex
            // 
            this.btSetIndex.Location = new System.Drawing.Point(183, 166);
            this.btSetIndex.Name = "btSetIndex";
            this.btSetIndex.Size = new System.Drawing.Size(75, 23);
            this.btSetIndex.TabIndex = 64;
            this.btSetIndex.Text = "Set Index";
            this.btSetIndex.UseVisualStyleBackColor = true;
            this.btSetIndex.Click += new System.EventHandler(this.btSetIndex_Click);
            // 
            // laColorIndex
            // 
            this.laColorIndex.AutoSize = true;
            this.laColorIndex.Enabled = false;
            this.laColorIndex.Location = new System.Drawing.Point(8, 171);
            this.laColorIndex.Name = "laColorIndex";
            this.laColorIndex.Size = new System.Drawing.Size(59, 13);
            this.laColorIndex.TabIndex = 63;
            this.laColorIndex.Text = "Color index";
            // 
            // btSetRgb
            // 
            this.btSetRgb.Location = new System.Drawing.Point(299, 136);
            this.btSetRgb.Name = "btSetRgb";
            this.btSetRgb.Size = new System.Drawing.Size(75, 23);
            this.btSetRgb.TabIndex = 62;
            this.btSetRgb.Text = "Set RGB";
            this.btSetRgb.UseVisualStyleBackColor = true;
            this.btSetRgb.Click += new System.EventHandler(this.btSetRgb_Click);
            // 
            // edB
            // 
            this.edB.Location = new System.Drawing.Point(232, 138);
            this.edB.Name = "edB";
            this.edB.Size = new System.Drawing.Size(61, 20);
            this.edB.TabIndex = 61;
            // 
            // edG
            // 
            this.edG.Location = new System.Drawing.Point(130, 138);
            this.edG.Name = "edG";
            this.edG.Size = new System.Drawing.Size(61, 20);
            this.edG.TabIndex = 60;
            // 
            // edR
            // 
            this.edR.Location = new System.Drawing.Point(32, 138);
            this.edR.Name = "edR";
            this.edR.Size = new System.Drawing.Size(61, 20);
            this.edR.TabIndex = 59;
            // 
            // laB
            // 
            this.laB.AutoSize = true;
            this.laB.Enabled = false;
            this.laB.Location = new System.Drawing.Point(209, 141);
            this.laB.Name = "laB";
            this.laB.Size = new System.Drawing.Size(17, 13);
            this.laB.TabIndex = 58;
            this.laB.Text = "B:";
            // 
            // laG
            // 
            this.laG.AutoSize = true;
            this.laG.Enabled = false;
            this.laG.Location = new System.Drawing.Point(109, 141);
            this.laG.Name = "laG";
            this.laG.Size = new System.Drawing.Size(15, 13);
            this.laG.TabIndex = 57;
            this.laG.Text = "G";
            // 
            // laR
            // 
            this.laR.AutoSize = true;
            this.laR.Enabled = false;
            this.laR.Location = new System.Drawing.Point(8, 141);
            this.laR.Name = "laR";
            this.laR.Size = new System.Drawing.Size(18, 13);
            this.laR.TabIndex = 56;
            this.laR.Text = "R:";
            // 
            // btSetMode
            // 
            this.btSetMode.Location = new System.Drawing.Point(204, 106);
            this.btSetMode.Name = "btSetMode";
            this.btSetMode.Size = new System.Drawing.Size(75, 23);
            this.btSetMode.TabIndex = 55;
            this.btSetMode.Text = "Set mode";
            this.btSetMode.UseVisualStyleBackColor = true;
            this.btSetMode.Click += new System.EventHandler(this.btSetMode_Click);
            // 
            // cbColorMode
            // 
            this.cbColorMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbColorMode.FormattingEnabled = true;
            this.cbColorMode.Items.AddRange(new object[] {
            "Discrete",
            "Absolute"});
            this.cbColorMode.Location = new System.Drawing.Point(77, 108);
            this.cbColorMode.Name = "cbColorMode";
            this.cbColorMode.Size = new System.Drawing.Size(121, 21);
            this.cbColorMode.TabIndex = 54;
            // 
            // laColorMode
            // 
            this.laColorMode.AutoSize = true;
            this.laColorMode.Enabled = false;
            this.laColorMode.Location = new System.Drawing.Point(8, 111);
            this.laColorMode.Name = "laColorMode";
            this.laColorMode.Size = new System.Drawing.Size(63, 13);
            this.laColorMode.TabIndex = 53;
            this.laColorMode.Text = "Color mode:";
            // 
            // laRgbConnectionId
            // 
            this.laRgbConnectionId.AutoSize = true;
            this.laRgbConnectionId.Location = new System.Drawing.Point(114, 82);
            this.laRgbConnectionId.Name = "laRgbConnectionId";
            this.laRgbConnectionId.Size = new System.Drawing.Size(47, 13);
            this.laRgbConnectionId.TabIndex = 52;
            this.laRgbConnectionId.Text = "<empty>";
            // 
            // laRgbConnectionIdCaption
            // 
            this.laRgbConnectionIdCaption.AutoSize = true;
            this.laRgbConnectionIdCaption.Location = new System.Drawing.Point(30, 82);
            this.laRgbConnectionIdCaption.Name = "laRgbConnectionIdCaption";
            this.laRgbConnectionIdCaption.Size = new System.Drawing.Size(78, 13);
            this.laRgbConnectionIdCaption.TabIndex = 51;
            this.laRgbConnectionIdCaption.Text = "Connection ID:";
            // 
            // laRgbDeviceType
            // 
            this.laRgbDeviceType.AutoSize = true;
            this.laRgbDeviceType.Location = new System.Drawing.Point(114, 58);
            this.laRgbDeviceType.Name = "laRgbDeviceType";
            this.laRgbDeviceType.Size = new System.Drawing.Size(47, 13);
            this.laRgbDeviceType.TabIndex = 50;
            this.laRgbDeviceType.Text = "<empty>";
            // 
            // laRgbDeviceTypeCaption
            // 
            this.laRgbDeviceTypeCaption.AutoSize = true;
            this.laRgbDeviceTypeCaption.Location = new System.Drawing.Point(30, 58);
            this.laRgbDeviceTypeCaption.Name = "laRgbDeviceTypeCaption";
            this.laRgbDeviceTypeCaption.Size = new System.Drawing.Size(67, 13);
            this.laRgbDeviceTypeCaption.TabIndex = 49;
            this.laRgbDeviceTypeCaption.Text = "Device type:";
            // 
            // laRgbVersion
            // 
            this.laRgbVersion.AutoSize = true;
            this.laRgbVersion.Location = new System.Drawing.Point(114, 35);
            this.laRgbVersion.Name = "laRgbVersion";
            this.laRgbVersion.Size = new System.Drawing.Size(47, 13);
            this.laRgbVersion.TabIndex = 48;
            this.laRgbVersion.Text = "<empty>";
            // 
            // laRgbVersionCaption
            // 
            this.laRgbVersionCaption.AutoSize = true;
            this.laRgbVersionCaption.Location = new System.Drawing.Point(30, 35);
            this.laRgbVersionCaption.Name = "laRgbVersionCaption";
            this.laRgbVersionCaption.Size = new System.Drawing.Size(45, 13);
            this.laRgbVersionCaption.TabIndex = 47;
            this.laRgbVersionCaption.Text = "Version:";
            // 
            // laRgbDeviceInformation
            // 
            this.laRgbDeviceInformation.AutoSize = true;
            this.laRgbDeviceInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laRgbDeviceInformation.Location = new System.Drawing.Point(6, 12);
            this.laRgbDeviceInformation.Name = "laRgbDeviceInformation";
            this.laRgbDeviceInformation.Size = new System.Drawing.Size(113, 13);
            this.laRgbDeviceInformation.TabIndex = 46;
            this.laRgbDeviceInformation.Text = "Device information";
            // 
            // tsMotion1
            // 
            this.tsMotion1.Controls.Add(this.btReset1);
            this.tsMotion1.Controls.Add(this.laDistance1);
            this.tsMotion1.Controls.Add(this.laDistanceTitle1);
            this.tsMotion1.Controls.Add(this.laCount1);
            this.tsMotion1.Controls.Add(this.laCountTitle1);
            this.tsMotion1.Controls.Add(this.btChange1);
            this.tsMotion1.Controls.Add(this.cbMode1);
            this.tsMotion1.Controls.Add(this.laMode1);
            this.tsMotion1.Controls.Add(this.laMotionConnectionId1);
            this.tsMotion1.Controls.Add(this.laMotionConnectionIdCaption1);
            this.tsMotion1.Controls.Add(this.laMotionDeviceType1);
            this.tsMotion1.Controls.Add(this.laMotionDeviceTypeCaption1);
            this.tsMotion1.Controls.Add(this.laMotionVersion1);
            this.tsMotion1.Controls.Add(this.laMotionVersionCaption1);
            this.tsMotion1.Controls.Add(this.laMotionDeviceInformation1);
            this.tsMotion1.Location = new System.Drawing.Point(4, 22);
            this.tsMotion1.Name = "tsMotion1";
            this.tsMotion1.Padding = new System.Windows.Forms.Padding(3);
            this.tsMotion1.Size = new System.Drawing.Size(400, 259);
            this.tsMotion1.TabIndex = 5;
            this.tsMotion1.Text = "Motion sensor 1";
            this.tsMotion1.UseVisualStyleBackColor = true;
            // 
            // btReset1
            // 
            this.btReset1.Location = new System.Drawing.Point(176, 144);
            this.btReset1.Name = "btReset1";
            this.btReset1.Size = new System.Drawing.Size(75, 23);
            this.btReset1.TabIndex = 67;
            this.btReset1.Text = "Reset";
            this.btReset1.UseVisualStyleBackColor = true;
            this.btReset1.Click += new System.EventHandler(this.btReset1_Click);
            // 
            // laDistance1
            // 
            this.laDistance1.AutoSize = true;
            this.laDistance1.Location = new System.Drawing.Point(72, 195);
            this.laDistance1.Name = "laDistance1";
            this.laDistance1.Size = new System.Drawing.Size(13, 13);
            this.laDistance1.TabIndex = 66;
            this.laDistance1.Text = "0";
            // 
            // laDistanceTitle1
            // 
            this.laDistanceTitle1.AutoSize = true;
            this.laDistanceTitle1.Location = new System.Drawing.Point(6, 195);
            this.laDistanceTitle1.Name = "laDistanceTitle1";
            this.laDistanceTitle1.Size = new System.Drawing.Size(52, 13);
            this.laDistanceTitle1.TabIndex = 65;
            this.laDistanceTitle1.Text = "Distance:";
            // 
            // laCount1
            // 
            this.laCount1.AutoSize = true;
            this.laCount1.Location = new System.Drawing.Point(72, 166);
            this.laCount1.Name = "laCount1";
            this.laCount1.Size = new System.Drawing.Size(13, 13);
            this.laCount1.TabIndex = 64;
            this.laCount1.Text = "0";
            // 
            // laCountTitle1
            // 
            this.laCountTitle1.AutoSize = true;
            this.laCountTitle1.Location = new System.Drawing.Point(8, 166);
            this.laCountTitle1.Name = "laCountTitle1";
            this.laCountTitle1.Size = new System.Drawing.Size(38, 13);
            this.laCountTitle1.TabIndex = 63;
            this.laCountTitle1.Text = "Count:";
            // 
            // btChange1
            // 
            this.btChange1.Location = new System.Drawing.Point(176, 115);
            this.btChange1.Name = "btChange1";
            this.btChange1.Size = new System.Drawing.Size(75, 23);
            this.btChange1.TabIndex = 62;
            this.btChange1.Text = "Change";
            this.btChange1.UseVisualStyleBackColor = true;
            this.btChange1.Click += new System.EventHandler(this.btChange1_Click);
            // 
            // cbMode1
            // 
            this.cbMode1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMode1.FormattingEnabled = true;
            this.cbMode1.Items.AddRange(new object[] {
            "Detect",
            "Count"});
            this.cbMode1.Location = new System.Drawing.Point(49, 117);
            this.cbMode1.Name = "cbMode1";
            this.cbMode1.Size = new System.Drawing.Size(121, 21);
            this.cbMode1.TabIndex = 61;
            // 
            // laMode1
            // 
            this.laMode1.AutoSize = true;
            this.laMode1.Location = new System.Drawing.Point(6, 120);
            this.laMode1.Name = "laMode1";
            this.laMode1.Size = new System.Drawing.Size(37, 13);
            this.laMode1.TabIndex = 60;
            this.laMode1.Text = "Mode:";
            // 
            // laMotionConnectionId1
            // 
            this.laMotionConnectionId1.AutoSize = true;
            this.laMotionConnectionId1.Location = new System.Drawing.Point(114, 82);
            this.laMotionConnectionId1.Name = "laMotionConnectionId1";
            this.laMotionConnectionId1.Size = new System.Drawing.Size(47, 13);
            this.laMotionConnectionId1.TabIndex = 59;
            this.laMotionConnectionId1.Text = "<empty>";
            // 
            // laMotionConnectionIdCaption1
            // 
            this.laMotionConnectionIdCaption1.AutoSize = true;
            this.laMotionConnectionIdCaption1.Location = new System.Drawing.Point(30, 82);
            this.laMotionConnectionIdCaption1.Name = "laMotionConnectionIdCaption1";
            this.laMotionConnectionIdCaption1.Size = new System.Drawing.Size(78, 13);
            this.laMotionConnectionIdCaption1.TabIndex = 58;
            this.laMotionConnectionIdCaption1.Text = "Connection ID:";
            // 
            // laMotionDeviceType1
            // 
            this.laMotionDeviceType1.AutoSize = true;
            this.laMotionDeviceType1.Location = new System.Drawing.Point(114, 58);
            this.laMotionDeviceType1.Name = "laMotionDeviceType1";
            this.laMotionDeviceType1.Size = new System.Drawing.Size(47, 13);
            this.laMotionDeviceType1.TabIndex = 57;
            this.laMotionDeviceType1.Text = "<empty>";
            // 
            // laMotionDeviceTypeCaption1
            // 
            this.laMotionDeviceTypeCaption1.AutoSize = true;
            this.laMotionDeviceTypeCaption1.Location = new System.Drawing.Point(30, 58);
            this.laMotionDeviceTypeCaption1.Name = "laMotionDeviceTypeCaption1";
            this.laMotionDeviceTypeCaption1.Size = new System.Drawing.Size(67, 13);
            this.laMotionDeviceTypeCaption1.TabIndex = 56;
            this.laMotionDeviceTypeCaption1.Text = "Device type:";
            // 
            // laMotionVersion1
            // 
            this.laMotionVersion1.AutoSize = true;
            this.laMotionVersion1.Location = new System.Drawing.Point(114, 35);
            this.laMotionVersion1.Name = "laMotionVersion1";
            this.laMotionVersion1.Size = new System.Drawing.Size(47, 13);
            this.laMotionVersion1.TabIndex = 55;
            this.laMotionVersion1.Text = "<empty>";
            // 
            // laMotionVersionCaption1
            // 
            this.laMotionVersionCaption1.AutoSize = true;
            this.laMotionVersionCaption1.Location = new System.Drawing.Point(30, 35);
            this.laMotionVersionCaption1.Name = "laMotionVersionCaption1";
            this.laMotionVersionCaption1.Size = new System.Drawing.Size(45, 13);
            this.laMotionVersionCaption1.TabIndex = 54;
            this.laMotionVersionCaption1.Text = "Version:";
            // 
            // laMotionDeviceInformation1
            // 
            this.laMotionDeviceInformation1.AutoSize = true;
            this.laMotionDeviceInformation1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laMotionDeviceInformation1.Location = new System.Drawing.Point(6, 12);
            this.laMotionDeviceInformation1.Name = "laMotionDeviceInformation1";
            this.laMotionDeviceInformation1.Size = new System.Drawing.Size(113, 13);
            this.laMotionDeviceInformation1.TabIndex = 53;
            this.laMotionDeviceInformation1.Text = "Device information";
            // 
            // tsMotion2
            // 
            this.tsMotion2.Controls.Add(this.btReset2);
            this.tsMotion2.Controls.Add(this.laDistance2);
            this.tsMotion2.Controls.Add(this.laDistanceTitle2);
            this.tsMotion2.Controls.Add(this.laCount2);
            this.tsMotion2.Controls.Add(this.laCountTitle2);
            this.tsMotion2.Controls.Add(this.btChange2);
            this.tsMotion2.Controls.Add(this.cbMode2);
            this.tsMotion2.Controls.Add(this.laMode2);
            this.tsMotion2.Controls.Add(this.laMotionConnectionId2);
            this.tsMotion2.Controls.Add(this.laMotionConnectionIdCaption2);
            this.tsMotion2.Controls.Add(this.laMotionDeviceType2);
            this.tsMotion2.Controls.Add(this.laMotionDeviceTypeCaption2);
            this.tsMotion2.Controls.Add(this.laMotionVersion2);
            this.tsMotion2.Controls.Add(this.laMotionVersionCaption2);
            this.tsMotion2.Controls.Add(this.laMotionDeviceInformation2);
            this.tsMotion2.Location = new System.Drawing.Point(4, 22);
            this.tsMotion2.Name = "tsMotion2";
            this.tsMotion2.Padding = new System.Windows.Forms.Padding(3);
            this.tsMotion2.Size = new System.Drawing.Size(400, 259);
            this.tsMotion2.TabIndex = 6;
            this.tsMotion2.Text = "Motion sensor 2";
            this.tsMotion2.UseVisualStyleBackColor = true;
            // 
            // btReset2
            // 
            this.btReset2.Location = new System.Drawing.Point(176, 148);
            this.btReset2.Name = "btReset2";
            this.btReset2.Size = new System.Drawing.Size(75, 23);
            this.btReset2.TabIndex = 82;
            this.btReset2.Text = "Reset";
            this.btReset2.UseVisualStyleBackColor = true;
            this.btReset2.Click += new System.EventHandler(this.btReset2_Click);
            // 
            // laDistance2
            // 
            this.laDistance2.AutoSize = true;
            this.laDistance2.Location = new System.Drawing.Point(72, 199);
            this.laDistance2.Name = "laDistance2";
            this.laDistance2.Size = new System.Drawing.Size(13, 13);
            this.laDistance2.TabIndex = 81;
            this.laDistance2.Text = "0";
            // 
            // laDistanceTitle2
            // 
            this.laDistanceTitle2.AutoSize = true;
            this.laDistanceTitle2.Location = new System.Drawing.Point(6, 199);
            this.laDistanceTitle2.Name = "laDistanceTitle2";
            this.laDistanceTitle2.Size = new System.Drawing.Size(52, 13);
            this.laDistanceTitle2.TabIndex = 80;
            this.laDistanceTitle2.Text = "Distance:";
            // 
            // laCount2
            // 
            this.laCount2.AutoSize = true;
            this.laCount2.Location = new System.Drawing.Point(72, 170);
            this.laCount2.Name = "laCount2";
            this.laCount2.Size = new System.Drawing.Size(13, 13);
            this.laCount2.TabIndex = 79;
            this.laCount2.Text = "0";
            // 
            // laCountTitle2
            // 
            this.laCountTitle2.AutoSize = true;
            this.laCountTitle2.Location = new System.Drawing.Point(8, 170);
            this.laCountTitle2.Name = "laCountTitle2";
            this.laCountTitle2.Size = new System.Drawing.Size(38, 13);
            this.laCountTitle2.TabIndex = 78;
            this.laCountTitle2.Text = "Count:";
            // 
            // btChange2
            // 
            this.btChange2.Location = new System.Drawing.Point(176, 119);
            this.btChange2.Name = "btChange2";
            this.btChange2.Size = new System.Drawing.Size(75, 23);
            this.btChange2.TabIndex = 77;
            this.btChange2.Text = "Change";
            this.btChange2.UseVisualStyleBackColor = true;
            this.btChange2.Click += new System.EventHandler(this.btChange2_Click);
            // 
            // cbMode2
            // 
            this.cbMode2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMode2.FormattingEnabled = true;
            this.cbMode2.Items.AddRange(new object[] {
            "Detect",
            "Count"});
            this.cbMode2.Location = new System.Drawing.Point(49, 121);
            this.cbMode2.Name = "cbMode2";
            this.cbMode2.Size = new System.Drawing.Size(121, 21);
            this.cbMode2.TabIndex = 76;
            // 
            // laMode2
            // 
            this.laMode2.AutoSize = true;
            this.laMode2.Location = new System.Drawing.Point(6, 124);
            this.laMode2.Name = "laMode2";
            this.laMode2.Size = new System.Drawing.Size(37, 13);
            this.laMode2.TabIndex = 75;
            this.laMode2.Text = "Mode:";
            // 
            // laMotionConnectionId2
            // 
            this.laMotionConnectionId2.AutoSize = true;
            this.laMotionConnectionId2.Location = new System.Drawing.Point(114, 86);
            this.laMotionConnectionId2.Name = "laMotionConnectionId2";
            this.laMotionConnectionId2.Size = new System.Drawing.Size(47, 13);
            this.laMotionConnectionId2.TabIndex = 74;
            this.laMotionConnectionId2.Text = "<empty>";
            // 
            // laMotionConnectionIdCaption2
            // 
            this.laMotionConnectionIdCaption2.AutoSize = true;
            this.laMotionConnectionIdCaption2.Location = new System.Drawing.Point(30, 86);
            this.laMotionConnectionIdCaption2.Name = "laMotionConnectionIdCaption2";
            this.laMotionConnectionIdCaption2.Size = new System.Drawing.Size(78, 13);
            this.laMotionConnectionIdCaption2.TabIndex = 73;
            this.laMotionConnectionIdCaption2.Text = "Connection ID:";
            // 
            // laMotionDeviceType2
            // 
            this.laMotionDeviceType2.AutoSize = true;
            this.laMotionDeviceType2.Location = new System.Drawing.Point(114, 62);
            this.laMotionDeviceType2.Name = "laMotionDeviceType2";
            this.laMotionDeviceType2.Size = new System.Drawing.Size(47, 13);
            this.laMotionDeviceType2.TabIndex = 72;
            this.laMotionDeviceType2.Text = "<empty>";
            // 
            // laMotionDeviceTypeCaption2
            // 
            this.laMotionDeviceTypeCaption2.AutoSize = true;
            this.laMotionDeviceTypeCaption2.Location = new System.Drawing.Point(30, 62);
            this.laMotionDeviceTypeCaption2.Name = "laMotionDeviceTypeCaption2";
            this.laMotionDeviceTypeCaption2.Size = new System.Drawing.Size(67, 13);
            this.laMotionDeviceTypeCaption2.TabIndex = 71;
            this.laMotionDeviceTypeCaption2.Text = "Device type:";
            // 
            // laMotionVersion2
            // 
            this.laMotionVersion2.AutoSize = true;
            this.laMotionVersion2.Location = new System.Drawing.Point(114, 39);
            this.laMotionVersion2.Name = "laMotionVersion2";
            this.laMotionVersion2.Size = new System.Drawing.Size(47, 13);
            this.laMotionVersion2.TabIndex = 70;
            this.laMotionVersion2.Text = "<empty>";
            // 
            // laMotionVersionCaption2
            // 
            this.laMotionVersionCaption2.AutoSize = true;
            this.laMotionVersionCaption2.Location = new System.Drawing.Point(30, 39);
            this.laMotionVersionCaption2.Name = "laMotionVersionCaption2";
            this.laMotionVersionCaption2.Size = new System.Drawing.Size(45, 13);
            this.laMotionVersionCaption2.TabIndex = 69;
            this.laMotionVersionCaption2.Text = "Version:";
            // 
            // laMotionDeviceInformation2
            // 
            this.laMotionDeviceInformation2.AutoSize = true;
            this.laMotionDeviceInformation2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laMotionDeviceInformation2.Location = new System.Drawing.Point(6, 16);
            this.laMotionDeviceInformation2.Name = "laMotionDeviceInformation2";
            this.laMotionDeviceInformation2.Size = new System.Drawing.Size(113, 13);
            this.laMotionDeviceInformation2.TabIndex = 68;
            this.laMotionDeviceInformation2.Text = "Device information";
            // 
            // tsTilt1
            // 
            this.tsTilt1.Controls.Add(this.laZ1);
            this.tsTilt1.Controls.Add(this.laZTitle1);
            this.tsTilt1.Controls.Add(this.laY1);
            this.tsTilt1.Controls.Add(this.laYTitle1);
            this.tsTilt1.Controls.Add(this.laX1);
            this.tsTilt1.Controls.Add(this.laXTitle1);
            this.tsTilt1.Controls.Add(this.laDirection1);
            this.tsTilt1.Controls.Add(this.laDirectionTitle1);
            this.tsTilt1.Controls.Add(this.btResetTilt1);
            this.tsTilt1.Controls.Add(this.btChangeTilt1);
            this.tsTilt1.Controls.Add(this.cbTiltMode1);
            this.tsTilt1.Controls.Add(this.laTiltMode1);
            this.tsTilt1.Controls.Add(this.laTiltConnectionId1);
            this.tsTilt1.Controls.Add(this.laTiltConnectionCaption1);
            this.tsTilt1.Controls.Add(this.laTiltDeviceType1);
            this.tsTilt1.Controls.Add(this.laTiltDeviceTypeCaption1);
            this.tsTilt1.Controls.Add(this.laTiltVersion1);
            this.tsTilt1.Controls.Add(this.laTiltVersionCaption1);
            this.tsTilt1.Controls.Add(this.laTiltDeviceInformation1);
            this.tsTilt1.Location = new System.Drawing.Point(4, 22);
            this.tsTilt1.Name = "tsTilt1";
            this.tsTilt1.Padding = new System.Windows.Forms.Padding(3);
            this.tsTilt1.Size = new System.Drawing.Size(400, 259);
            this.tsTilt1.TabIndex = 7;
            this.tsTilt1.Text = "Tilt sensor 1";
            this.tsTilt1.UseVisualStyleBackColor = true;
            // 
            // laZ1
            // 
            this.laZ1.AutoSize = true;
            this.laZ1.Location = new System.Drawing.Point(82, 223);
            this.laZ1.Name = "laZ1";
            this.laZ1.Size = new System.Drawing.Size(13, 13);
            this.laZ1.TabIndex = 93;
            this.laZ1.Text = "0";
            // 
            // laZTitle1
            // 
            this.laZTitle1.AutoSize = true;
            this.laZTitle1.Location = new System.Drawing.Point(6, 223);
            this.laZTitle1.Name = "laZTitle1";
            this.laZTitle1.Size = new System.Drawing.Size(17, 13);
            this.laZTitle1.TabIndex = 92;
            this.laZTitle1.Text = "Z:";
            // 
            // laY1
            // 
            this.laY1.AutoSize = true;
            this.laY1.Location = new System.Drawing.Point(82, 198);
            this.laY1.Name = "laY1";
            this.laY1.Size = new System.Drawing.Size(13, 13);
            this.laY1.TabIndex = 91;
            this.laY1.Text = "0";
            // 
            // laYTitle1
            // 
            this.laYTitle1.AutoSize = true;
            this.laYTitle1.Location = new System.Drawing.Point(6, 198);
            this.laYTitle1.Name = "laYTitle1";
            this.laYTitle1.Size = new System.Drawing.Size(17, 13);
            this.laYTitle1.TabIndex = 90;
            this.laYTitle1.Text = "Y:";
            // 
            // laX1
            // 
            this.laX1.AutoSize = true;
            this.laX1.Location = new System.Drawing.Point(82, 175);
            this.laX1.Name = "laX1";
            this.laX1.Size = new System.Drawing.Size(13, 13);
            this.laX1.TabIndex = 89;
            this.laX1.Text = "0";
            // 
            // laXTitle1
            // 
            this.laXTitle1.AutoSize = true;
            this.laXTitle1.Location = new System.Drawing.Point(6, 175);
            this.laXTitle1.Name = "laXTitle1";
            this.laXTitle1.Size = new System.Drawing.Size(17, 13);
            this.laXTitle1.TabIndex = 88;
            this.laXTitle1.Text = "X:";
            // 
            // laDirection1
            // 
            this.laDirection1.AutoSize = true;
            this.laDirection1.Location = new System.Drawing.Point(82, 151);
            this.laDirection1.Name = "laDirection1";
            this.laDirection1.Size = new System.Drawing.Size(53, 13);
            this.laDirection1.TabIndex = 87;
            this.laDirection1.Text = "Unknown";
            // 
            // laDirectionTitle1
            // 
            this.laDirectionTitle1.AutoSize = true;
            this.laDirectionTitle1.Location = new System.Drawing.Point(6, 151);
            this.laDirectionTitle1.Name = "laDirectionTitle1";
            this.laDirectionTitle1.Size = new System.Drawing.Size(52, 13);
            this.laDirectionTitle1.TabIndex = 86;
            this.laDirectionTitle1.Text = "Direction:";
            // 
            // btResetTilt1
            // 
            this.btResetTilt1.Location = new System.Drawing.Point(176, 141);
            this.btResetTilt1.Name = "btResetTilt1";
            this.btResetTilt1.Size = new System.Drawing.Size(75, 23);
            this.btResetTilt1.TabIndex = 85;
            this.btResetTilt1.Text = "Reset";
            this.btResetTilt1.UseVisualStyleBackColor = true;
            this.btResetTilt1.Click += new System.EventHandler(this.btResetTilt1_Click);
            // 
            // btChangeTilt1
            // 
            this.btChangeTilt1.Location = new System.Drawing.Point(176, 112);
            this.btChangeTilt1.Name = "btChangeTilt1";
            this.btChangeTilt1.Size = new System.Drawing.Size(75, 23);
            this.btChangeTilt1.TabIndex = 84;
            this.btChangeTilt1.Text = "Change";
            this.btChangeTilt1.UseVisualStyleBackColor = true;
            this.btChangeTilt1.Click += new System.EventHandler(this.btChangeTilt1_Click);
            // 
            // cbTiltMode1
            // 
            this.cbTiltMode1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTiltMode1.FormattingEnabled = true;
            this.cbTiltMode1.Items.AddRange(new object[] {
            "Angle",
            "Tilt",
            "Crash"});
            this.cbTiltMode1.Location = new System.Drawing.Point(49, 114);
            this.cbTiltMode1.Name = "cbTiltMode1";
            this.cbTiltMode1.Size = new System.Drawing.Size(121, 21);
            this.cbTiltMode1.TabIndex = 83;
            // 
            // laTiltMode1
            // 
            this.laTiltMode1.AutoSize = true;
            this.laTiltMode1.Location = new System.Drawing.Point(6, 117);
            this.laTiltMode1.Name = "laTiltMode1";
            this.laTiltMode1.Size = new System.Drawing.Size(37, 13);
            this.laTiltMode1.TabIndex = 82;
            this.laTiltMode1.Text = "Mode:";
            // 
            // laTiltConnectionId1
            // 
            this.laTiltConnectionId1.AutoSize = true;
            this.laTiltConnectionId1.Location = new System.Drawing.Point(114, 86);
            this.laTiltConnectionId1.Name = "laTiltConnectionId1";
            this.laTiltConnectionId1.Size = new System.Drawing.Size(47, 13);
            this.laTiltConnectionId1.TabIndex = 81;
            this.laTiltConnectionId1.Text = "<empty>";
            // 
            // laTiltConnectionCaption1
            // 
            this.laTiltConnectionCaption1.AutoSize = true;
            this.laTiltConnectionCaption1.Location = new System.Drawing.Point(30, 86);
            this.laTiltConnectionCaption1.Name = "laTiltConnectionCaption1";
            this.laTiltConnectionCaption1.Size = new System.Drawing.Size(78, 13);
            this.laTiltConnectionCaption1.TabIndex = 80;
            this.laTiltConnectionCaption1.Text = "Connection ID:";
            // 
            // laTiltDeviceType1
            // 
            this.laTiltDeviceType1.AutoSize = true;
            this.laTiltDeviceType1.Location = new System.Drawing.Point(114, 62);
            this.laTiltDeviceType1.Name = "laTiltDeviceType1";
            this.laTiltDeviceType1.Size = new System.Drawing.Size(47, 13);
            this.laTiltDeviceType1.TabIndex = 79;
            this.laTiltDeviceType1.Text = "<empty>";
            // 
            // laTiltDeviceTypeCaption1
            // 
            this.laTiltDeviceTypeCaption1.AutoSize = true;
            this.laTiltDeviceTypeCaption1.Location = new System.Drawing.Point(30, 62);
            this.laTiltDeviceTypeCaption1.Name = "laTiltDeviceTypeCaption1";
            this.laTiltDeviceTypeCaption1.Size = new System.Drawing.Size(67, 13);
            this.laTiltDeviceTypeCaption1.TabIndex = 78;
            this.laTiltDeviceTypeCaption1.Text = "Device type:";
            // 
            // laTiltVersion1
            // 
            this.laTiltVersion1.AutoSize = true;
            this.laTiltVersion1.Location = new System.Drawing.Point(114, 39);
            this.laTiltVersion1.Name = "laTiltVersion1";
            this.laTiltVersion1.Size = new System.Drawing.Size(47, 13);
            this.laTiltVersion1.TabIndex = 77;
            this.laTiltVersion1.Text = "<empty>";
            // 
            // laTiltVersionCaption1
            // 
            this.laTiltVersionCaption1.AutoSize = true;
            this.laTiltVersionCaption1.Location = new System.Drawing.Point(30, 39);
            this.laTiltVersionCaption1.Name = "laTiltVersionCaption1";
            this.laTiltVersionCaption1.Size = new System.Drawing.Size(45, 13);
            this.laTiltVersionCaption1.TabIndex = 76;
            this.laTiltVersionCaption1.Text = "Version:";
            // 
            // laTiltDeviceInformation1
            // 
            this.laTiltDeviceInformation1.AutoSize = true;
            this.laTiltDeviceInformation1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laTiltDeviceInformation1.Location = new System.Drawing.Point(6, 16);
            this.laTiltDeviceInformation1.Name = "laTiltDeviceInformation1";
            this.laTiltDeviceInformation1.Size = new System.Drawing.Size(113, 13);
            this.laTiltDeviceInformation1.TabIndex = 75;
            this.laTiltDeviceInformation1.Text = "Device information";
            // 
            // tsTilt2
            // 
            this.tsTilt2.Controls.Add(this.laZ2);
            this.tsTilt2.Controls.Add(this.laZTitle2);
            this.tsTilt2.Controls.Add(this.laY2);
            this.tsTilt2.Controls.Add(this.laYTitle2);
            this.tsTilt2.Controls.Add(this.laX2);
            this.tsTilt2.Controls.Add(this.laXTitle2);
            this.tsTilt2.Controls.Add(this.laDirection2);
            this.tsTilt2.Controls.Add(this.laDirectionTitle2);
            this.tsTilt2.Controls.Add(this.btResetTilt2);
            this.tsTilt2.Controls.Add(this.btChangeTilt2);
            this.tsTilt2.Controls.Add(this.cbTiltMode2);
            this.tsTilt2.Controls.Add(this.laTiltMode2);
            this.tsTilt2.Controls.Add(this.laTiltConnectionId2);
            this.tsTilt2.Controls.Add(this.laTiltConnectionCaption2);
            this.tsTilt2.Controls.Add(this.laTiltDeviceType2);
            this.tsTilt2.Controls.Add(this.laTiltDeviceTypeCaption2);
            this.tsTilt2.Controls.Add(this.laTiltVersion2);
            this.tsTilt2.Controls.Add(this.laTiltVersionCaption2);
            this.tsTilt2.Controls.Add(this.laTiltDeviceInformation2);
            this.tsTilt2.Location = new System.Drawing.Point(4, 22);
            this.tsTilt2.Name = "tsTilt2";
            this.tsTilt2.Padding = new System.Windows.Forms.Padding(3);
            this.tsTilt2.Size = new System.Drawing.Size(400, 259);
            this.tsTilt2.TabIndex = 8;
            this.tsTilt2.Text = "Tilt sensor 2";
            this.tsTilt2.UseVisualStyleBackColor = true;
            // 
            // laZ2
            // 
            this.laZ2.AutoSize = true;
            this.laZ2.Location = new System.Drawing.Point(82, 226);
            this.laZ2.Name = "laZ2";
            this.laZ2.Size = new System.Drawing.Size(13, 13);
            this.laZ2.TabIndex = 105;
            this.laZ2.Text = "0";
            // 
            // laZTitle2
            // 
            this.laZTitle2.AutoSize = true;
            this.laZTitle2.Location = new System.Drawing.Point(6, 226);
            this.laZTitle2.Name = "laZTitle2";
            this.laZTitle2.Size = new System.Drawing.Size(17, 13);
            this.laZTitle2.TabIndex = 104;
            this.laZTitle2.Text = "Z:";
            // 
            // laY2
            // 
            this.laY2.AutoSize = true;
            this.laY2.Location = new System.Drawing.Point(82, 201);
            this.laY2.Name = "laY2";
            this.laY2.Size = new System.Drawing.Size(13, 13);
            this.laY2.TabIndex = 103;
            this.laY2.Text = "0";
            // 
            // laYTitle2
            // 
            this.laYTitle2.AutoSize = true;
            this.laYTitle2.Location = new System.Drawing.Point(6, 201);
            this.laYTitle2.Name = "laYTitle2";
            this.laYTitle2.Size = new System.Drawing.Size(17, 13);
            this.laYTitle2.TabIndex = 102;
            this.laYTitle2.Text = "Y:";
            // 
            // laX2
            // 
            this.laX2.AutoSize = true;
            this.laX2.Location = new System.Drawing.Point(82, 178);
            this.laX2.Name = "laX2";
            this.laX2.Size = new System.Drawing.Size(13, 13);
            this.laX2.TabIndex = 101;
            this.laX2.Text = "0";
            // 
            // laXTitle2
            // 
            this.laXTitle2.AutoSize = true;
            this.laXTitle2.Location = new System.Drawing.Point(6, 178);
            this.laXTitle2.Name = "laXTitle2";
            this.laXTitle2.Size = new System.Drawing.Size(17, 13);
            this.laXTitle2.TabIndex = 100;
            this.laXTitle2.Text = "X:";
            // 
            // laDirection2
            // 
            this.laDirection2.AutoSize = true;
            this.laDirection2.Location = new System.Drawing.Point(82, 154);
            this.laDirection2.Name = "laDirection2";
            this.laDirection2.Size = new System.Drawing.Size(53, 13);
            this.laDirection2.TabIndex = 99;
            this.laDirection2.Text = "Unknown";
            // 
            // laDirectionTitle2
            // 
            this.laDirectionTitle2.AutoSize = true;
            this.laDirectionTitle2.Location = new System.Drawing.Point(6, 154);
            this.laDirectionTitle2.Name = "laDirectionTitle2";
            this.laDirectionTitle2.Size = new System.Drawing.Size(52, 13);
            this.laDirectionTitle2.TabIndex = 98;
            this.laDirectionTitle2.Text = "Direction:";
            // 
            // btResetTilt2
            // 
            this.btResetTilt2.Location = new System.Drawing.Point(176, 144);
            this.btResetTilt2.Name = "btResetTilt2";
            this.btResetTilt2.Size = new System.Drawing.Size(75, 23);
            this.btResetTilt2.TabIndex = 97;
            this.btResetTilt2.Text = "Reset";
            this.btResetTilt2.UseVisualStyleBackColor = true;
            this.btResetTilt2.Click += new System.EventHandler(this.btResetTilt2_Click);
            // 
            // btChangeTilt2
            // 
            this.btChangeTilt2.Location = new System.Drawing.Point(176, 115);
            this.btChangeTilt2.Name = "btChangeTilt2";
            this.btChangeTilt2.Size = new System.Drawing.Size(75, 23);
            this.btChangeTilt2.TabIndex = 96;
            this.btChangeTilt2.Text = "Change";
            this.btChangeTilt2.UseVisualStyleBackColor = true;
            this.btChangeTilt2.Click += new System.EventHandler(this.btChangeTilt2_Click);
            // 
            // cbTiltMode2
            // 
            this.cbTiltMode2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTiltMode2.FormattingEnabled = true;
            this.cbTiltMode2.Items.AddRange(new object[] {
            "Angle",
            "Tilt",
            "Crash"});
            this.cbTiltMode2.Location = new System.Drawing.Point(49, 117);
            this.cbTiltMode2.Name = "cbTiltMode2";
            this.cbTiltMode2.Size = new System.Drawing.Size(121, 21);
            this.cbTiltMode2.TabIndex = 95;
            // 
            // laTiltMode2
            // 
            this.laTiltMode2.AutoSize = true;
            this.laTiltMode2.Location = new System.Drawing.Point(6, 120);
            this.laTiltMode2.Name = "laTiltMode2";
            this.laTiltMode2.Size = new System.Drawing.Size(37, 13);
            this.laTiltMode2.TabIndex = 94;
            this.laTiltMode2.Text = "Mode:";
            // 
            // laTiltConnectionId2
            // 
            this.laTiltConnectionId2.AutoSize = true;
            this.laTiltConnectionId2.Location = new System.Drawing.Point(114, 87);
            this.laTiltConnectionId2.Name = "laTiltConnectionId2";
            this.laTiltConnectionId2.Size = new System.Drawing.Size(47, 13);
            this.laTiltConnectionId2.TabIndex = 88;
            this.laTiltConnectionId2.Text = "<empty>";
            // 
            // laTiltConnectionCaption2
            // 
            this.laTiltConnectionCaption2.AutoSize = true;
            this.laTiltConnectionCaption2.Location = new System.Drawing.Point(30, 87);
            this.laTiltConnectionCaption2.Name = "laTiltConnectionCaption2";
            this.laTiltConnectionCaption2.Size = new System.Drawing.Size(78, 13);
            this.laTiltConnectionCaption2.TabIndex = 87;
            this.laTiltConnectionCaption2.Text = "Connection ID:";
            // 
            // laTiltDeviceType2
            // 
            this.laTiltDeviceType2.AutoSize = true;
            this.laTiltDeviceType2.Location = new System.Drawing.Point(114, 63);
            this.laTiltDeviceType2.Name = "laTiltDeviceType2";
            this.laTiltDeviceType2.Size = new System.Drawing.Size(47, 13);
            this.laTiltDeviceType2.TabIndex = 86;
            this.laTiltDeviceType2.Text = "<empty>";
            // 
            // laTiltDeviceTypeCaption2
            // 
            this.laTiltDeviceTypeCaption2.AutoSize = true;
            this.laTiltDeviceTypeCaption2.Location = new System.Drawing.Point(30, 63);
            this.laTiltDeviceTypeCaption2.Name = "laTiltDeviceTypeCaption2";
            this.laTiltDeviceTypeCaption2.Size = new System.Drawing.Size(67, 13);
            this.laTiltDeviceTypeCaption2.TabIndex = 85;
            this.laTiltDeviceTypeCaption2.Text = "Device type:";
            // 
            // laTiltVersion2
            // 
            this.laTiltVersion2.AutoSize = true;
            this.laTiltVersion2.Location = new System.Drawing.Point(114, 40);
            this.laTiltVersion2.Name = "laTiltVersion2";
            this.laTiltVersion2.Size = new System.Drawing.Size(47, 13);
            this.laTiltVersion2.TabIndex = 84;
            this.laTiltVersion2.Text = "<empty>";
            // 
            // laTiltVersionCaption2
            // 
            this.laTiltVersionCaption2.AutoSize = true;
            this.laTiltVersionCaption2.Location = new System.Drawing.Point(30, 40);
            this.laTiltVersionCaption2.Name = "laTiltVersionCaption2";
            this.laTiltVersionCaption2.Size = new System.Drawing.Size(45, 13);
            this.laTiltVersionCaption2.TabIndex = 83;
            this.laTiltVersionCaption2.Text = "Version:";
            // 
            // laTiltDeviceInformation2
            // 
            this.laTiltDeviceInformation2.AutoSize = true;
            this.laTiltDeviceInformation2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laTiltDeviceInformation2.Location = new System.Drawing.Point(6, 17);
            this.laTiltDeviceInformation2.Name = "laTiltDeviceInformation2";
            this.laTiltDeviceInformation2.Size = new System.Drawing.Size(113, 13);
            this.laTiltDeviceInformation2.TabIndex = 82;
            this.laTiltDeviceInformation2.Text = "Device information";
            // 
            // tsMotor1
            // 
            this.tsMotor1.Controls.Add(this.btDrift1);
            this.tsMotor1.Controls.Add(this.btBrake1);
            this.tsMotor1.Controls.Add(this.btStart1);
            this.tsMotor1.Controls.Add(this.edPower1);
            this.tsMotor1.Controls.Add(this.laPower1);
            this.tsMotor1.Controls.Add(this.cbMotorDirection1);
            this.tsMotor1.Controls.Add(this.laMotorDirectionCaption1);
            this.tsMotor1.Controls.Add(this.laMotorConnectionId1);
            this.tsMotor1.Controls.Add(this.laMotorConnectionIdCaption1);
            this.tsMotor1.Controls.Add(this.laMotorDeviceType1);
            this.tsMotor1.Controls.Add(this.laMotorDeviceTypeCaption1);
            this.tsMotor1.Controls.Add(this.laMotorVersion1);
            this.tsMotor1.Controls.Add(this.laMotorVersionCaption1);
            this.tsMotor1.Controls.Add(this.laMotorDeviceInformation1);
            this.tsMotor1.Location = new System.Drawing.Point(4, 22);
            this.tsMotor1.Name = "tsMotor1";
            this.tsMotor1.Padding = new System.Windows.Forms.Padding(3);
            this.tsMotor1.Size = new System.Drawing.Size(400, 259);
            this.tsMotor1.TabIndex = 9;
            this.tsMotor1.Text = "Motor 1";
            this.tsMotor1.UseVisualStyleBackColor = true;
            // 
            // btDrift1
            // 
            this.btDrift1.Location = new System.Drawing.Point(146, 166);
            this.btDrift1.Name = "btDrift1";
            this.btDrift1.Size = new System.Drawing.Size(75, 23);
            this.btDrift1.TabIndex = 95;
            this.btDrift1.Text = "Drift";
            this.btDrift1.UseVisualStyleBackColor = true;
            this.btDrift1.Click += new System.EventHandler(this.btDrift1_Click);
            // 
            // btBrake1
            // 
            this.btBrake1.Location = new System.Drawing.Point(65, 166);
            this.btBrake1.Name = "btBrake1";
            this.btBrake1.Size = new System.Drawing.Size(75, 23);
            this.btBrake1.TabIndex = 94;
            this.btBrake1.Text = "Brake";
            this.btBrake1.UseVisualStyleBackColor = true;
            this.btBrake1.Click += new System.EventHandler(this.btBrake1_Click);
            // 
            // btStart1
            // 
            this.btStart1.Location = new System.Drawing.Point(192, 138);
            this.btStart1.Name = "btStart1";
            this.btStart1.Size = new System.Drawing.Size(75, 23);
            this.btStart1.TabIndex = 93;
            this.btStart1.Text = "Start";
            this.btStart1.UseVisualStyleBackColor = true;
            this.btStart1.Click += new System.EventHandler(this.btStart1_Click);
            // 
            // edPower1
            // 
            this.edPower1.Location = new System.Drawing.Point(65, 140);
            this.edPower1.Name = "edPower1";
            this.edPower1.Size = new System.Drawing.Size(121, 20);
            this.edPower1.TabIndex = 92;
            this.edPower1.Text = "20";
            // 
            // laPower1
            // 
            this.laPower1.AutoSize = true;
            this.laPower1.Location = new System.Drawing.Point(7, 143);
            this.laPower1.Name = "laPower1";
            this.laPower1.Size = new System.Drawing.Size(37, 13);
            this.laPower1.TabIndex = 91;
            this.laPower1.Text = "Power";
            // 
            // cbMotorDirection1
            // 
            this.cbMotorDirection1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMotorDirection1.FormattingEnabled = true;
            this.cbMotorDirection1.Items.AddRange(new object[] {
            "Right",
            "Left"});
            this.cbMotorDirection1.Location = new System.Drawing.Point(65, 113);
            this.cbMotorDirection1.Name = "cbMotorDirection1";
            this.cbMotorDirection1.Size = new System.Drawing.Size(121, 21);
            this.cbMotorDirection1.TabIndex = 90;
            // 
            // laMotorDirectionCaption1
            // 
            this.laMotorDirectionCaption1.AutoSize = true;
            this.laMotorDirectionCaption1.Location = new System.Drawing.Point(7, 116);
            this.laMotorDirectionCaption1.Name = "laMotorDirectionCaption1";
            this.laMotorDirectionCaption1.Size = new System.Drawing.Size(52, 13);
            this.laMotorDirectionCaption1.TabIndex = 89;
            this.laMotorDirectionCaption1.Text = "Direction:";
            // 
            // laMotorConnectionId1
            // 
            this.laMotorConnectionId1.AutoSize = true;
            this.laMotorConnectionId1.Location = new System.Drawing.Point(114, 86);
            this.laMotorConnectionId1.Name = "laMotorConnectionId1";
            this.laMotorConnectionId1.Size = new System.Drawing.Size(47, 13);
            this.laMotorConnectionId1.TabIndex = 88;
            this.laMotorConnectionId1.Text = "<empty>";
            // 
            // laMotorConnectionIdCaption1
            // 
            this.laMotorConnectionIdCaption1.AutoSize = true;
            this.laMotorConnectionIdCaption1.Location = new System.Drawing.Point(30, 86);
            this.laMotorConnectionIdCaption1.Name = "laMotorConnectionIdCaption1";
            this.laMotorConnectionIdCaption1.Size = new System.Drawing.Size(78, 13);
            this.laMotorConnectionIdCaption1.TabIndex = 87;
            this.laMotorConnectionIdCaption1.Text = "Connection ID:";
            // 
            // laMotorDeviceType1
            // 
            this.laMotorDeviceType1.AutoSize = true;
            this.laMotorDeviceType1.Location = new System.Drawing.Point(114, 62);
            this.laMotorDeviceType1.Name = "laMotorDeviceType1";
            this.laMotorDeviceType1.Size = new System.Drawing.Size(47, 13);
            this.laMotorDeviceType1.TabIndex = 86;
            this.laMotorDeviceType1.Text = "<empty>";
            // 
            // laMotorDeviceTypeCaption1
            // 
            this.laMotorDeviceTypeCaption1.AutoSize = true;
            this.laMotorDeviceTypeCaption1.Location = new System.Drawing.Point(30, 62);
            this.laMotorDeviceTypeCaption1.Name = "laMotorDeviceTypeCaption1";
            this.laMotorDeviceTypeCaption1.Size = new System.Drawing.Size(67, 13);
            this.laMotorDeviceTypeCaption1.TabIndex = 85;
            this.laMotorDeviceTypeCaption1.Text = "Device type:";
            // 
            // laMotorVersion1
            // 
            this.laMotorVersion1.AutoSize = true;
            this.laMotorVersion1.Location = new System.Drawing.Point(114, 39);
            this.laMotorVersion1.Name = "laMotorVersion1";
            this.laMotorVersion1.Size = new System.Drawing.Size(47, 13);
            this.laMotorVersion1.TabIndex = 84;
            this.laMotorVersion1.Text = "<empty>";
            // 
            // laMotorVersionCaption1
            // 
            this.laMotorVersionCaption1.AutoSize = true;
            this.laMotorVersionCaption1.Location = new System.Drawing.Point(30, 39);
            this.laMotorVersionCaption1.Name = "laMotorVersionCaption1";
            this.laMotorVersionCaption1.Size = new System.Drawing.Size(45, 13);
            this.laMotorVersionCaption1.TabIndex = 83;
            this.laMotorVersionCaption1.Text = "Version:";
            // 
            // laMotorDeviceInformation1
            // 
            this.laMotorDeviceInformation1.AutoSize = true;
            this.laMotorDeviceInformation1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laMotorDeviceInformation1.Location = new System.Drawing.Point(6, 16);
            this.laMotorDeviceInformation1.Name = "laMotorDeviceInformation1";
            this.laMotorDeviceInformation1.Size = new System.Drawing.Size(113, 13);
            this.laMotorDeviceInformation1.TabIndex = 82;
            this.laMotorDeviceInformation1.Text = "Device information";
            // 
            // tsMotor2
            // 
            this.tsMotor2.Controls.Add(this.btDrift2);
            this.tsMotor2.Controls.Add(this.btBrake2);
            this.tsMotor2.Controls.Add(this.btStart2);
            this.tsMotor2.Controls.Add(this.edPower2);
            this.tsMotor2.Controls.Add(this.laPower2);
            this.tsMotor2.Controls.Add(this.cbMotorDirection2);
            this.tsMotor2.Controls.Add(this.laMotorDirectionCaption2);
            this.tsMotor2.Controls.Add(this.laMotorConnectionId2);
            this.tsMotor2.Controls.Add(this.laMotorConnectionIdCaption2);
            this.tsMotor2.Controls.Add(this.laMotorDeviceType2);
            this.tsMotor2.Controls.Add(this.laMotorDeviceTypeCaption2);
            this.tsMotor2.Controls.Add(this.laMotorVersion2);
            this.tsMotor2.Controls.Add(this.laMotorVersionCaption2);
            this.tsMotor2.Controls.Add(this.laMotorDeviceInformation2);
            this.tsMotor2.Location = new System.Drawing.Point(4, 22);
            this.tsMotor2.Name = "tsMotor2";
            this.tsMotor2.Padding = new System.Windows.Forms.Padding(3);
            this.tsMotor2.Size = new System.Drawing.Size(400, 259);
            this.tsMotor2.TabIndex = 10;
            this.tsMotor2.Text = "Motor 2";
            this.tsMotor2.UseVisualStyleBackColor = true;
            // 
            // btDrift2
            // 
            this.btDrift2.Location = new System.Drawing.Point(145, 168);
            this.btDrift2.Name = "btDrift2";
            this.btDrift2.Size = new System.Drawing.Size(75, 23);
            this.btDrift2.TabIndex = 102;
            this.btDrift2.Text = "Drift";
            this.btDrift2.UseVisualStyleBackColor = true;
            this.btDrift2.Click += new System.EventHandler(this.btDrift2_Click);
            // 
            // btBrake2
            // 
            this.btBrake2.Location = new System.Drawing.Point(64, 168);
            this.btBrake2.Name = "btBrake2";
            this.btBrake2.Size = new System.Drawing.Size(75, 23);
            this.btBrake2.TabIndex = 101;
            this.btBrake2.Text = "Brake";
            this.btBrake2.UseVisualStyleBackColor = true;
            this.btBrake2.Click += new System.EventHandler(this.btBrake2_Click);
            // 
            // btStart2
            // 
            this.btStart2.Location = new System.Drawing.Point(191, 140);
            this.btStart2.Name = "btStart2";
            this.btStart2.Size = new System.Drawing.Size(75, 23);
            this.btStart2.TabIndex = 100;
            this.btStart2.Text = "Start";
            this.btStart2.UseVisualStyleBackColor = true;
            this.btStart2.Click += new System.EventHandler(this.btStart2_Click);
            // 
            // edPower2
            // 
            this.edPower2.Location = new System.Drawing.Point(64, 142);
            this.edPower2.Name = "edPower2";
            this.edPower2.Size = new System.Drawing.Size(121, 20);
            this.edPower2.TabIndex = 99;
            this.edPower2.Text = "20";
            // 
            // laPower2
            // 
            this.laPower2.AutoSize = true;
            this.laPower2.Location = new System.Drawing.Point(6, 145);
            this.laPower2.Name = "laPower2";
            this.laPower2.Size = new System.Drawing.Size(40, 13);
            this.laPower2.TabIndex = 98;
            this.laPower2.Text = "Power:";
            // 
            // cbMotorDirection2
            // 
            this.cbMotorDirection2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMotorDirection2.FormattingEnabled = true;
            this.cbMotorDirection2.Items.AddRange(new object[] {
            "Right",
            "Left"});
            this.cbMotorDirection2.Location = new System.Drawing.Point(64, 115);
            this.cbMotorDirection2.Name = "cbMotorDirection2";
            this.cbMotorDirection2.Size = new System.Drawing.Size(121, 21);
            this.cbMotorDirection2.TabIndex = 97;
            // 
            // laMotorDirectionCaption2
            // 
            this.laMotorDirectionCaption2.AutoSize = true;
            this.laMotorDirectionCaption2.Location = new System.Drawing.Point(6, 118);
            this.laMotorDirectionCaption2.Name = "laMotorDirectionCaption2";
            this.laMotorDirectionCaption2.Size = new System.Drawing.Size(52, 13);
            this.laMotorDirectionCaption2.TabIndex = 96;
            this.laMotorDirectionCaption2.Text = "Direction:";
            // 
            // laMotorConnectionId2
            // 
            this.laMotorConnectionId2.AutoSize = true;
            this.laMotorConnectionId2.Location = new System.Drawing.Point(114, 85);
            this.laMotorConnectionId2.Name = "laMotorConnectionId2";
            this.laMotorConnectionId2.Size = new System.Drawing.Size(47, 13);
            this.laMotorConnectionId2.TabIndex = 95;
            this.laMotorConnectionId2.Text = "<empty>";
            // 
            // laMotorConnectionIdCaption2
            // 
            this.laMotorConnectionIdCaption2.AutoSize = true;
            this.laMotorConnectionIdCaption2.Location = new System.Drawing.Point(30, 85);
            this.laMotorConnectionIdCaption2.Name = "laMotorConnectionIdCaption2";
            this.laMotorConnectionIdCaption2.Size = new System.Drawing.Size(78, 13);
            this.laMotorConnectionIdCaption2.TabIndex = 94;
            this.laMotorConnectionIdCaption2.Text = "Connection ID:";
            // 
            // laMotorDeviceType2
            // 
            this.laMotorDeviceType2.AutoSize = true;
            this.laMotorDeviceType2.Location = new System.Drawing.Point(114, 61);
            this.laMotorDeviceType2.Name = "laMotorDeviceType2";
            this.laMotorDeviceType2.Size = new System.Drawing.Size(47, 13);
            this.laMotorDeviceType2.TabIndex = 93;
            this.laMotorDeviceType2.Text = "<empty>";
            // 
            // laMotorDeviceTypeCaption2
            // 
            this.laMotorDeviceTypeCaption2.AutoSize = true;
            this.laMotorDeviceTypeCaption2.Location = new System.Drawing.Point(30, 61);
            this.laMotorDeviceTypeCaption2.Name = "laMotorDeviceTypeCaption2";
            this.laMotorDeviceTypeCaption2.Size = new System.Drawing.Size(67, 13);
            this.laMotorDeviceTypeCaption2.TabIndex = 92;
            this.laMotorDeviceTypeCaption2.Text = "Device type:";
            // 
            // laMotorVersion2
            // 
            this.laMotorVersion2.AutoSize = true;
            this.laMotorVersion2.Location = new System.Drawing.Point(114, 38);
            this.laMotorVersion2.Name = "laMotorVersion2";
            this.laMotorVersion2.Size = new System.Drawing.Size(47, 13);
            this.laMotorVersion2.TabIndex = 91;
            this.laMotorVersion2.Text = "<empty>";
            // 
            // laMotorVersionCaption2
            // 
            this.laMotorVersionCaption2.AutoSize = true;
            this.laMotorVersionCaption2.Location = new System.Drawing.Point(30, 38);
            this.laMotorVersionCaption2.Name = "laMotorVersionCaption2";
            this.laMotorVersionCaption2.Size = new System.Drawing.Size(45, 13);
            this.laMotorVersionCaption2.TabIndex = 90;
            this.laMotorVersionCaption2.Text = "Version:";
            // 
            // laMotorDeviceInformation2
            // 
            this.laMotorDeviceInformation2.AutoSize = true;
            this.laMotorDeviceInformation2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laMotorDeviceInformation2.Location = new System.Drawing.Point(6, 15);
            this.laMotorDeviceInformation2.Name = "laMotorDeviceInformation2";
            this.laMotorDeviceInformation2.Size = new System.Drawing.Size(113, 13);
            this.laMotorDeviceInformation2.TabIndex = 89;
            this.laMotorDeviceInformation2.Text = "Device information";
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 568);
            this.Controls.Add(this.pcHub);
            this.Controls.Add(this.btDisconnect);
            this.Controls.Add(this.lvHubs);
            this.Controls.Add(this.btClear);
            this.Controls.Add(this.lbLog);
            this.Controls.Add(this.btStop);
            this.Controls.Add(this.btStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "fmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WeDo Robot Demo Application";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FmMain_FormClosed);
            this.Load += new System.EventHandler(this.FmMain_Load);
            this.pcHub.ResumeLayout(false);
            this.tsHubInfo.ResumeLayout(false);
            this.tsHubInfo.PerformLayout();
            this.tsCurrent.ResumeLayout(false);
            this.tsCurrent.PerformLayout();
            this.tsVoltage.ResumeLayout(false);
            this.tsVoltage.PerformLayout();
            this.tsPiezo.ResumeLayout(false);
            this.tsPiezo.PerformLayout();
            this.tsRgb.ResumeLayout(false);
            this.tsRgb.PerformLayout();
            this.tsMotion1.ResumeLayout(false);
            this.tsMotion1.PerformLayout();
            this.tsMotion2.ResumeLayout(false);
            this.tsMotion2.PerformLayout();
            this.tsTilt1.ResumeLayout(false);
            this.tsTilt1.PerformLayout();
            this.tsTilt2.ResumeLayout(false);
            this.tsTilt2.PerformLayout();
            this.tsMotor1.ResumeLayout(false);
            this.tsMotor1.PerformLayout();
            this.tsMotor2.ResumeLayout(false);
            this.tsMotor2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.ListBox lbLog;
        private System.Windows.Forms.Button btClear;
        private System.Windows.Forms.ListView lvHubs;
        private System.Windows.Forms.ColumnHeader chHubAddress;
        private System.Windows.Forms.Button btDisconnect;
        private System.Windows.Forms.TabControl pcHub;
        private System.Windows.Forms.TabPage tsHubInfo;
        private System.Windows.Forms.Label laManufacturerName;
        private System.Windows.Forms.Label laSoftwareVersion;
        private System.Windows.Forms.Label laHardwareVersion;
        private System.Windows.Forms.Label laFirmwareVersion;
        private System.Windows.Forms.Label laManufacturerNameTitle;
        private System.Windows.Forms.Label laSoftwareVersionTitle;
        private System.Windows.Forms.Label laHardwareVersionTitle;
        private System.Windows.Forms.Label laFirmwareVersionTitle;
        private System.Windows.Forms.Label laDeviceInformationTitle;
        private System.Windows.Forms.Label laBatteryType;
        private System.Windows.Forms.Label laBattTypeTitle;
        private System.Windows.Forms.Label laDeviceName;
        private System.Windows.Forms.Label laDeviceNameCaption;
        private System.Windows.Forms.Label laBattLevel;
        private System.Windows.Forms.Label laBattLevelTitle;
        private System.Windows.Forms.Label laNewName;
        private System.Windows.Forms.Button btSetDeviceName;
        private System.Windows.Forms.TextBox edDeviceName;
        private System.Windows.Forms.Label laLowVoltage;
        private System.Windows.Forms.Label laLowSignal;
        private System.Windows.Forms.Label laHighCurrent;
        private System.Windows.Forms.Label laButtonPressed;
        private System.Windows.Forms.TabPage tsCurrent;
        private System.Windows.Forms.Label laCurrentDeviceInfo;
        private System.Windows.Forms.Label laCurrentVersion;
        private System.Windows.Forms.Label laCurrentVersionCaption;
        private System.Windows.Forms.Label laCurrentDeviceType;
        private System.Windows.Forms.Label laCurrentDeviceTypeCaption;
        private System.Windows.Forms.Label laCurrentConnectionId;
        private System.Windows.Forms.Label laCurrentConnectionIdCaption;
        private System.Windows.Forms.Label laCurrent;
        private System.Windows.Forms.Label laCurrentCaption;
        private System.Windows.Forms.TabPage tsVoltage;
        private System.Windows.Forms.Label laVoltage;
        private System.Windows.Forms.Label laVoltageCaption;
        private System.Windows.Forms.Label laVoltageConnectionId;
        private System.Windows.Forms.Label laVoltageConnectionIdCaption;
        private System.Windows.Forms.Label laVoltageDeviceType;
        private System.Windows.Forms.Label laVoltageDeviceTypeCaption;
        private System.Windows.Forms.Label laVoltageVersion;
        private System.Windows.Forms.Label laVoltageVersionCaption;
        private System.Windows.Forms.Label laVotageDeviceInformation;
        private System.Windows.Forms.TabPage tsRgb;
        private System.Windows.Forms.Label laRgbConnectionId;
        private System.Windows.Forms.Label laRgbConnectionIdCaption;
        private System.Windows.Forms.Label laRgbDeviceType;
        private System.Windows.Forms.Label laRgbDeviceTypeCaption;
        private System.Windows.Forms.Label laRgbVersion;
        private System.Windows.Forms.Label laRgbVersionCaption;
        private System.Windows.Forms.Label laRgbDeviceInformation;
        private System.Windows.Forms.Button btSetMode;
        private System.Windows.Forms.ComboBox cbColorMode;
        private System.Windows.Forms.Label laColorMode;
        private System.Windows.Forms.Label laB;
        private System.Windows.Forms.Label laG;
        private System.Windows.Forms.Label laR;
        private System.Windows.Forms.TextBox edB;
        private System.Windows.Forms.TextBox edG;
        private System.Windows.Forms.TextBox edR;
        private System.Windows.Forms.Button btSetRgb;
        private System.Windows.Forms.ComboBox cbColorIndex;
        private System.Windows.Forms.Button btSetIndex;
        private System.Windows.Forms.Label laColorIndex;
        private System.Windows.Forms.Button btTurnOff;
        private System.Windows.Forms.Button btSetDefault;
        private System.Windows.Forms.TabPage tsPiezo;
        private System.Windows.Forms.Label laPiezoConnectionId;
        private System.Windows.Forms.Label laPiezoConnectionIdCaption;
        private System.Windows.Forms.Label laPiezoDeviceType;
        private System.Windows.Forms.Label laPiezoDeviceTypeCaption;
        private System.Windows.Forms.Label laPiezoVersion;
        private System.Windows.Forms.Label laPiezoVersionCaption;
        private System.Windows.Forms.Label laPiezoDeviceInformation;
        private System.Windows.Forms.TextBox edDuration;
        private System.Windows.Forms.Label laDuration;
        private System.Windows.Forms.ComboBox cbOctave;
        private System.Windows.Forms.Label laOctave;
        private System.Windows.Forms.ComboBox cbNote;
        private System.Windows.Forms.Label laNote;
        private System.Windows.Forms.Button btStopSound;
        private System.Windows.Forms.Button btPlay;
        private System.Windows.Forms.TabPage tsMotion1;
        private System.Windows.Forms.Label laMotionConnectionId1;
        private System.Windows.Forms.Label laMotionConnectionIdCaption1;
        private System.Windows.Forms.Label laMotionDeviceType1;
        private System.Windows.Forms.Label laMotionDeviceTypeCaption1;
        private System.Windows.Forms.Label laMotionVersion1;
        private System.Windows.Forms.Label laMotionVersionCaption1;
        private System.Windows.Forms.Label laMotionDeviceInformation1;
        private System.Windows.Forms.Button btReset1;
        private System.Windows.Forms.Label laDistance1;
        private System.Windows.Forms.Label laDistanceTitle1;
        private System.Windows.Forms.Label laCount1;
        private System.Windows.Forms.Label laCountTitle1;
        private System.Windows.Forms.Button btChange1;
        private System.Windows.Forms.ComboBox cbMode1;
        private System.Windows.Forms.Label laMode1;
        private System.Windows.Forms.TabPage tsMotion2;
        private System.Windows.Forms.Button btReset2;
        private System.Windows.Forms.Label laDistance2;
        private System.Windows.Forms.Label laDistanceTitle2;
        private System.Windows.Forms.Label laCount2;
        private System.Windows.Forms.Label laCountTitle2;
        private System.Windows.Forms.Button btChange2;
        private System.Windows.Forms.ComboBox cbMode2;
        private System.Windows.Forms.Label laMode2;
        private System.Windows.Forms.Label laMotionConnectionId2;
        private System.Windows.Forms.Label laMotionConnectionIdCaption2;
        private System.Windows.Forms.Label laMotionDeviceType2;
        private System.Windows.Forms.Label laMotionDeviceTypeCaption2;
        private System.Windows.Forms.Label laMotionVersion2;
        private System.Windows.Forms.Label laMotionVersionCaption2;
        private System.Windows.Forms.Label laMotionDeviceInformation2;
        private System.Windows.Forms.TabPage tsTilt1;
        private System.Windows.Forms.Label laTiltConnectionId1;
        private System.Windows.Forms.Label laTiltConnectionCaption1;
        private System.Windows.Forms.Label laTiltDeviceType1;
        private System.Windows.Forms.Label laTiltDeviceTypeCaption1;
        private System.Windows.Forms.Label laTiltVersion1;
        private System.Windows.Forms.Label laTiltVersionCaption1;
        private System.Windows.Forms.Label laTiltDeviceInformation1;
        private System.Windows.Forms.TabPage tsTilt2;
        private System.Windows.Forms.Label laTiltConnectionId2;
        private System.Windows.Forms.Label laTiltConnectionCaption2;
        private System.Windows.Forms.Label laTiltDeviceType2;
        private System.Windows.Forms.Label laTiltDeviceTypeCaption2;
        private System.Windows.Forms.Label laTiltVersion2;
        private System.Windows.Forms.Label laTiltVersionCaption2;
        private System.Windows.Forms.Label laTiltDeviceInformation2;
        private System.Windows.Forms.Button btResetTilt1;
        private System.Windows.Forms.Button btChangeTilt1;
        private System.Windows.Forms.ComboBox cbTiltMode1;
        private System.Windows.Forms.Label laTiltMode1;
        private System.Windows.Forms.Label laZ1;
        private System.Windows.Forms.Label laZTitle1;
        private System.Windows.Forms.Label laY1;
        private System.Windows.Forms.Label laYTitle1;
        private System.Windows.Forms.Label laX1;
        private System.Windows.Forms.Label laXTitle1;
        private System.Windows.Forms.Label laDirection1;
        private System.Windows.Forms.Label laDirectionTitle1;
        private System.Windows.Forms.Label laZ2;
        private System.Windows.Forms.Label laZTitle2;
        private System.Windows.Forms.Label laY2;
        private System.Windows.Forms.Label laYTitle2;
        private System.Windows.Forms.Label laX2;
        private System.Windows.Forms.Label laXTitle2;
        private System.Windows.Forms.Label laDirection2;
        private System.Windows.Forms.Label laDirectionTitle2;
        private System.Windows.Forms.Button btResetTilt2;
        private System.Windows.Forms.Button btChangeTilt2;
        private System.Windows.Forms.ComboBox cbTiltMode2;
        private System.Windows.Forms.Label laTiltMode2;
        private System.Windows.Forms.TabPage tsMotor1;
        private System.Windows.Forms.Label laMotorConnectionId1;
        private System.Windows.Forms.Label laMotorConnectionIdCaption1;
        private System.Windows.Forms.Label laMotorDeviceType1;
        private System.Windows.Forms.Label laMotorDeviceTypeCaption1;
        private System.Windows.Forms.Label laMotorVersion1;
        private System.Windows.Forms.Label laMotorVersionCaption1;
        private System.Windows.Forms.Label laMotorDeviceInformation1;
        private System.Windows.Forms.TabPage tsMotor2;
        private System.Windows.Forms.Label laMotorConnectionId2;
        private System.Windows.Forms.Label laMotorConnectionIdCaption2;
        private System.Windows.Forms.Label laMotorDeviceType2;
        private System.Windows.Forms.Label laMotorDeviceTypeCaption2;
        private System.Windows.Forms.Label laMotorVersion2;
        private System.Windows.Forms.Label laMotorVersionCaption2;
        private System.Windows.Forms.Label laMotorDeviceInformation2;
        private System.Windows.Forms.Button btDrift1;
        private System.Windows.Forms.Button btBrake1;
        private System.Windows.Forms.Button btStart1;
        private System.Windows.Forms.TextBox edPower1;
        private System.Windows.Forms.Label laPower1;
        private System.Windows.Forms.ComboBox cbMotorDirection1;
        private System.Windows.Forms.Label laMotorDirectionCaption1;
        private System.Windows.Forms.Button btDrift2;
        private System.Windows.Forms.Button btBrake2;
        private System.Windows.Forms.Button btStart2;
        private System.Windows.Forms.TextBox edPower2;
        private System.Windows.Forms.Label laPower2;
        private System.Windows.Forms.ComboBox cbMotorDirection2;
        private System.Windows.Forms.Label laMotorDirectionCaption2;
    }
}

