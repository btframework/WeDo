namespace WeDoMotor
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
            this.btDisconnect = new System.Windows.Forms.Button();
            this.btConnect = new System.Windows.Forms.Button();
            this.tcMotors = new System.Windows.Forms.TabControl();
            this.tpMotor1 = new System.Windows.Forms.TabPage();
            this.btDrift1 = new System.Windows.Forms.Button();
            this.btBrake1 = new System.Windows.Forms.Button();
            this.btStart1 = new System.Windows.Forms.Button();
            this.edPower1 = new System.Windows.Forms.TextBox();
            this.laPower1 = new System.Windows.Forms.Label();
            this.cbDirection1 = new System.Windows.Forms.ComboBox();
            this.laDirection1 = new System.Windows.Forms.Label();
            this.laIoState1 = new System.Windows.Forms.Label();
            this.tpMotor2 = new System.Windows.Forms.TabPage();
            this.btDrift2 = new System.Windows.Forms.Button();
            this.btBrake2 = new System.Windows.Forms.Button();
            this.btStart2 = new System.Windows.Forms.Button();
            this.edPower2 = new System.Windows.Forms.TextBox();
            this.laPower2 = new System.Windows.Forms.Label();
            this.cbDirection2 = new System.Windows.Forms.ComboBox();
            this.laDirection2 = new System.Windows.Forms.Label();
            this.laIoState2 = new System.Windows.Forms.Label();
            this.laStatus = new System.Windows.Forms.Label();
            this.laMV = new System.Windows.Forms.Label();
            this.laVoltage = new System.Windows.Forms.Label();
            this.laVoltageTitle = new System.Windows.Forms.Label();
            this.laMA = new System.Windows.Forms.Label();
            this.laCurrent = new System.Windows.Forms.Label();
            this.laCurrentTitle = new System.Windows.Forms.Label();
            this.laHighCurrent = new System.Windows.Forms.Label();
            this.laLowVoltage = new System.Windows.Forms.Label();
            this.tcMotors.SuspendLayout();
            this.tpMotor1.SuspendLayout();
            this.tpMotor2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btDisconnect
            // 
            this.btDisconnect.Enabled = false;
            this.btDisconnect.Location = new System.Drawing.Point(93, 12);
            this.btDisconnect.Name = "btDisconnect";
            this.btDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btDisconnect.TabIndex = 13;
            this.btDisconnect.Text = "Disconnect";
            this.btDisconnect.UseVisualStyleBackColor = true;
            this.btDisconnect.Click += new System.EventHandler(this.BtDisconnect_Click);
            // 
            // btConnect
            // 
            this.btConnect.Location = new System.Drawing.Point(12, 12);
            this.btConnect.Name = "btConnect";
            this.btConnect.Size = new System.Drawing.Size(75, 23);
            this.btConnect.TabIndex = 12;
            this.btConnect.Text = "Connect";
            this.btConnect.UseVisualStyleBackColor = true;
            this.btConnect.Click += new System.EventHandler(this.BtConnect_Click);
            // 
            // tcMotors
            // 
            this.tcMotors.Controls.Add(this.tpMotor1);
            this.tcMotors.Controls.Add(this.tpMotor2);
            this.tcMotors.Location = new System.Drawing.Point(12, 105);
            this.tcMotors.Name = "tcMotors";
            this.tcMotors.SelectedIndex = 0;
            this.tcMotors.Size = new System.Drawing.Size(459, 136);
            this.tcMotors.TabIndex = 31;
            // 
            // tpMotor1
            // 
            this.tpMotor1.Controls.Add(this.btDrift1);
            this.tpMotor1.Controls.Add(this.btBrake1);
            this.tpMotor1.Controls.Add(this.btStart1);
            this.tpMotor1.Controls.Add(this.edPower1);
            this.tpMotor1.Controls.Add(this.laPower1);
            this.tpMotor1.Controls.Add(this.cbDirection1);
            this.tpMotor1.Controls.Add(this.laDirection1);
            this.tpMotor1.Controls.Add(this.laIoState1);
            this.tpMotor1.Location = new System.Drawing.Point(4, 22);
            this.tpMotor1.Name = "tpMotor1";
            this.tpMotor1.Padding = new System.Windows.Forms.Padding(3);
            this.tpMotor1.Size = new System.Drawing.Size(451, 110);
            this.tpMotor1.TabIndex = 0;
            this.tpMotor1.Text = "Motor 1";
            this.tpMotor1.UseVisualStyleBackColor = true;
            // 
            // btDrift1
            // 
            this.btDrift1.Enabled = false;
            this.btDrift1.Location = new System.Drawing.Point(164, 77);
            this.btDrift1.Name = "btDrift1";
            this.btDrift1.Size = new System.Drawing.Size(75, 23);
            this.btDrift1.TabIndex = 39;
            this.btDrift1.Text = "Drift";
            this.btDrift1.UseVisualStyleBackColor = true;
            this.btDrift1.Click += new System.EventHandler(this.btDrift1_Click);
            // 
            // btBrake1
            // 
            this.btBrake1.Enabled = false;
            this.btBrake1.Location = new System.Drawing.Point(66, 77);
            this.btBrake1.Name = "btBrake1";
            this.btBrake1.Size = new System.Drawing.Size(75, 23);
            this.btBrake1.TabIndex = 38;
            this.btBrake1.Text = "Brake";
            this.btBrake1.UseVisualStyleBackColor = true;
            this.btBrake1.Click += new System.EventHandler(this.btBrake1_Click);
            // 
            // btStart1
            // 
            this.btStart1.Enabled = false;
            this.btStart1.Location = new System.Drawing.Point(356, 37);
            this.btStart1.Name = "btStart1";
            this.btStart1.Size = new System.Drawing.Size(75, 23);
            this.btStart1.TabIndex = 37;
            this.btStart1.Text = "Start";
            this.btStart1.UseVisualStyleBackColor = true;
            this.btStart1.Click += new System.EventHandler(this.btStart1_Click);
            // 
            // edPower1
            // 
            this.edPower1.Enabled = false;
            this.edPower1.Location = new System.Drawing.Point(250, 39);
            this.edPower1.Name = "edPower1";
            this.edPower1.Size = new System.Drawing.Size(100, 20);
            this.edPower1.TabIndex = 36;
            this.edPower1.Text = "20";
            // 
            // laPower1
            // 
            this.laPower1.AutoSize = true;
            this.laPower1.Enabled = false;
            this.laPower1.Location = new System.Drawing.Point(204, 42);
            this.laPower1.Name = "laPower1";
            this.laPower1.Size = new System.Drawing.Size(40, 13);
            this.laPower1.TabIndex = 35;
            this.laPower1.Text = "Power:";
            // 
            // cbDirection1
            // 
            this.cbDirection1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDirection1.Enabled = false;
            this.cbDirection1.FormattingEnabled = true;
            this.cbDirection1.Items.AddRange(new object[] {
            "Right",
            "Left"});
            this.cbDirection1.Location = new System.Drawing.Point(66, 39);
            this.cbDirection1.Name = "cbDirection1";
            this.cbDirection1.Size = new System.Drawing.Size(121, 21);
            this.cbDirection1.TabIndex = 34;
            // 
            // laDirection1
            // 
            this.laDirection1.AutoSize = true;
            this.laDirection1.Enabled = false;
            this.laDirection1.Location = new System.Drawing.Point(8, 42);
            this.laDirection1.Name = "laDirection1";
            this.laDirection1.Size = new System.Drawing.Size(52, 13);
            this.laDirection1.TabIndex = 33;
            this.laDirection1.Text = "Direction:";
            // 
            // laIoState1
            // 
            this.laIoState1.AutoSize = true;
            this.laIoState1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laIoState1.Location = new System.Drawing.Point(6, 16);
            this.laIoState1.Name = "laIoState1";
            this.laIoState1.Size = new System.Drawing.Size(62, 13);
            this.laIoState1.TabIndex = 32;
            this.laIoState1.Text = "Detached";
            // 
            // tpMotor2
            // 
            this.tpMotor2.Controls.Add(this.btDrift2);
            this.tpMotor2.Controls.Add(this.btBrake2);
            this.tpMotor2.Controls.Add(this.btStart2);
            this.tpMotor2.Controls.Add(this.edPower2);
            this.tpMotor2.Controls.Add(this.laPower2);
            this.tpMotor2.Controls.Add(this.cbDirection2);
            this.tpMotor2.Controls.Add(this.laDirection2);
            this.tpMotor2.Controls.Add(this.laIoState2);
            this.tpMotor2.Location = new System.Drawing.Point(4, 22);
            this.tpMotor2.Name = "tpMotor2";
            this.tpMotor2.Padding = new System.Windows.Forms.Padding(3);
            this.tpMotor2.Size = new System.Drawing.Size(451, 110);
            this.tpMotor2.TabIndex = 1;
            this.tpMotor2.Text = "Motor 2";
            this.tpMotor2.UseVisualStyleBackColor = true;
            // 
            // btDrift2
            // 
            this.btDrift2.Enabled = false;
            this.btDrift2.Location = new System.Drawing.Point(171, 74);
            this.btDrift2.Name = "btDrift2";
            this.btDrift2.Size = new System.Drawing.Size(75, 23);
            this.btDrift2.TabIndex = 47;
            this.btDrift2.Text = "Drift";
            this.btDrift2.UseVisualStyleBackColor = true;
            this.btDrift2.Click += new System.EventHandler(this.btDrift2_Click);
            // 
            // btBrake2
            // 
            this.btBrake2.Enabled = false;
            this.btBrake2.Location = new System.Drawing.Point(73, 74);
            this.btBrake2.Name = "btBrake2";
            this.btBrake2.Size = new System.Drawing.Size(75, 23);
            this.btBrake2.TabIndex = 46;
            this.btBrake2.Text = "Brake";
            this.btBrake2.UseVisualStyleBackColor = true;
            this.btBrake2.Click += new System.EventHandler(this.btBrake2_Click);
            // 
            // btStart2
            // 
            this.btStart2.Enabled = false;
            this.btStart2.Location = new System.Drawing.Point(363, 34);
            this.btStart2.Name = "btStart2";
            this.btStart2.Size = new System.Drawing.Size(75, 23);
            this.btStart2.TabIndex = 45;
            this.btStart2.Text = "Start";
            this.btStart2.UseVisualStyleBackColor = true;
            this.btStart2.Click += new System.EventHandler(this.btStart2_Click);
            // 
            // edPower2
            // 
            this.edPower2.Enabled = false;
            this.edPower2.Location = new System.Drawing.Point(257, 36);
            this.edPower2.Name = "edPower2";
            this.edPower2.Size = new System.Drawing.Size(100, 20);
            this.edPower2.TabIndex = 44;
            this.edPower2.Text = "20";
            // 
            // laPower2
            // 
            this.laPower2.AutoSize = true;
            this.laPower2.Enabled = false;
            this.laPower2.Location = new System.Drawing.Point(211, 39);
            this.laPower2.Name = "laPower2";
            this.laPower2.Size = new System.Drawing.Size(40, 13);
            this.laPower2.TabIndex = 43;
            this.laPower2.Text = "Power:";
            // 
            // cbDirection2
            // 
            this.cbDirection2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDirection2.Enabled = false;
            this.cbDirection2.FormattingEnabled = true;
            this.cbDirection2.Items.AddRange(new object[] {
            "Right",
            "Left"});
            this.cbDirection2.Location = new System.Drawing.Point(73, 36);
            this.cbDirection2.Name = "cbDirection2";
            this.cbDirection2.Size = new System.Drawing.Size(121, 21);
            this.cbDirection2.TabIndex = 42;
            // 
            // laDirection2
            // 
            this.laDirection2.AutoSize = true;
            this.laDirection2.Enabled = false;
            this.laDirection2.Location = new System.Drawing.Point(15, 39);
            this.laDirection2.Name = "laDirection2";
            this.laDirection2.Size = new System.Drawing.Size(52, 13);
            this.laDirection2.TabIndex = 41;
            this.laDirection2.Text = "Direction:";
            // 
            // laIoState2
            // 
            this.laIoState2.AutoSize = true;
            this.laIoState2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laIoState2.Location = new System.Drawing.Point(13, 13);
            this.laIoState2.Name = "laIoState2";
            this.laIoState2.Size = new System.Drawing.Size(62, 13);
            this.laIoState2.TabIndex = 40;
            this.laIoState2.Text = "Detached";
            // 
            // laStatus
            // 
            this.laStatus.AutoSize = true;
            this.laStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laStatus.Location = new System.Drawing.Point(179, 17);
            this.laStatus.Name = "laStatus";
            this.laStatus.Size = new System.Drawing.Size(85, 13);
            this.laStatus.TabIndex = 32;
            this.laStatus.Text = "Disconnected";
            // 
            // laMV
            // 
            this.laMV.AutoSize = true;
            this.laMV.Enabled = false;
            this.laMV.Location = new System.Drawing.Point(319, 50);
            this.laMV.Name = "laMV";
            this.laMV.Size = new System.Drawing.Size(22, 13);
            this.laMV.TabIndex = 51;
            this.laMV.Text = "mV";
            // 
            // laVoltage
            // 
            this.laVoltage.AutoSize = true;
            this.laVoltage.Enabled = false;
            this.laVoltage.Location = new System.Drawing.Point(260, 50);
            this.laVoltage.Name = "laVoltage";
            this.laVoltage.Size = new System.Drawing.Size(13, 13);
            this.laVoltage.TabIndex = 50;
            this.laVoltage.Text = "0";
            // 
            // laVoltageTitle
            // 
            this.laVoltageTitle.AutoSize = true;
            this.laVoltageTitle.Enabled = false;
            this.laVoltageTitle.Location = new System.Drawing.Point(208, 50);
            this.laVoltageTitle.Name = "laVoltageTitle";
            this.laVoltageTitle.Size = new System.Drawing.Size(46, 13);
            this.laVoltageTitle.TabIndex = 49;
            this.laVoltageTitle.Text = "Voltage:";
            // 
            // laMA
            // 
            this.laMA.AutoSize = true;
            this.laMA.Enabled = false;
            this.laMA.Location = new System.Drawing.Point(123, 50);
            this.laMA.Name = "laMA";
            this.laMA.Size = new System.Drawing.Size(22, 13);
            this.laMA.TabIndex = 48;
            this.laMA.Text = "mA";
            // 
            // laCurrent
            // 
            this.laCurrent.AutoSize = true;
            this.laCurrent.Enabled = false;
            this.laCurrent.Location = new System.Drawing.Point(67, 50);
            this.laCurrent.Name = "laCurrent";
            this.laCurrent.Size = new System.Drawing.Size(13, 13);
            this.laCurrent.TabIndex = 47;
            this.laCurrent.Text = "0";
            // 
            // laCurrentTitle
            // 
            this.laCurrentTitle.AutoSize = true;
            this.laCurrentTitle.Enabled = false;
            this.laCurrentTitle.Location = new System.Drawing.Point(12, 50);
            this.laCurrentTitle.Name = "laCurrentTitle";
            this.laCurrentTitle.Size = new System.Drawing.Size(47, 13);
            this.laCurrentTitle.TabIndex = 46;
            this.laCurrentTitle.Text = "Current: ";
            // 
            // laHighCurrent
            // 
            this.laHighCurrent.AutoSize = true;
            this.laHighCurrent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laHighCurrent.ForeColor = System.Drawing.Color.Red;
            this.laHighCurrent.Location = new System.Drawing.Point(12, 80);
            this.laHighCurrent.Name = "laHighCurrent";
            this.laHighCurrent.Size = new System.Drawing.Size(81, 13);
            this.laHighCurrent.TabIndex = 52;
            this.laHighCurrent.Text = "High current!";
            this.laHighCurrent.Visible = false;
            // 
            // laLowVoltage
            // 
            this.laLowVoltage.AutoSize = true;
            this.laLowVoltage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laLowVoltage.ForeColor = System.Drawing.Color.Red;
            this.laLowVoltage.Location = new System.Drawing.Point(208, 80);
            this.laLowVoltage.Name = "laLowVoltage";
            this.laLowVoltage.Size = new System.Drawing.Size(81, 13);
            this.laLowVoltage.TabIndex = 53;
            this.laLowVoltage.Text = "Low Voltage!";
            this.laLowVoltage.Visible = false;
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 253);
            this.Controls.Add(this.laLowVoltage);
            this.Controls.Add(this.laHighCurrent);
            this.Controls.Add(this.laMV);
            this.Controls.Add(this.laVoltage);
            this.Controls.Add(this.laVoltageTitle);
            this.Controls.Add(this.laMA);
            this.Controls.Add(this.laCurrent);
            this.Controls.Add(this.laCurrentTitle);
            this.Controls.Add(this.laStatus);
            this.Controls.Add(this.tcMotors);
            this.Controls.Add(this.btDisconnect);
            this.Controls.Add(this.btConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "fmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WeDo Motor Test Application";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FmMain_FormClosed);
            this.Load += new System.EventHandler(this.FmMain_Load);
            this.tcMotors.ResumeLayout(false);
            this.tpMotor1.ResumeLayout(false);
            this.tpMotor1.PerformLayout();
            this.tpMotor2.ResumeLayout(false);
            this.tpMotor2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btDisconnect;
        private System.Windows.Forms.Button btConnect;
        private System.Windows.Forms.TabControl tcMotors;
        private System.Windows.Forms.TabPage tpMotor1;
        private System.Windows.Forms.Button btDrift1;
        private System.Windows.Forms.Button btBrake1;
        private System.Windows.Forms.Button btStart1;
        private System.Windows.Forms.TextBox edPower1;
        private System.Windows.Forms.Label laPower1;
        private System.Windows.Forms.ComboBox cbDirection1;
        private System.Windows.Forms.Label laDirection1;
        private System.Windows.Forms.Label laIoState1;
        private System.Windows.Forms.TabPage tpMotor2;
        private System.Windows.Forms.Label laStatus;
        private System.Windows.Forms.Label laMV;
        private System.Windows.Forms.Label laVoltage;
        private System.Windows.Forms.Label laVoltageTitle;
        private System.Windows.Forms.Label laMA;
        private System.Windows.Forms.Label laCurrent;
        private System.Windows.Forms.Label laCurrentTitle;
        private System.Windows.Forms.Label laHighCurrent;
        private System.Windows.Forms.Label laLowVoltage;
        private System.Windows.Forms.Button btDrift2;
        private System.Windows.Forms.Button btBrake2;
        private System.Windows.Forms.Button btStart2;
        private System.Windows.Forms.TextBox edPower2;
        private System.Windows.Forms.Label laPower2;
        private System.Windows.Forms.ComboBox cbDirection2;
        private System.Windows.Forms.Label laDirection2;
        private System.Windows.Forms.Label laIoState2;
    }
}

