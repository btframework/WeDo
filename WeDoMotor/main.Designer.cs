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
            this.laIoState = new System.Windows.Forms.Label();
            this.laStatus = new System.Windows.Forms.Label();
            this.btDisconnect = new System.Windows.Forms.Button();
            this.btConnect = new System.Windows.Forms.Button();
            this.laDirection = new System.Windows.Forms.Label();
            this.cbDirection = new System.Windows.Forms.ComboBox();
            this.laPower = new System.Windows.Forms.Label();
            this.edPower = new System.Windows.Forms.TextBox();
            this.btStart = new System.Windows.Forms.Button();
            this.btBrake = new System.Windows.Forms.Button();
            this.btDrift = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // laIoState
            // 
            this.laIoState.AutoSize = true;
            this.laIoState.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laIoState.Location = new System.Drawing.Point(181, 53);
            this.laIoState.Name = "laIoState";
            this.laIoState.Size = new System.Drawing.Size(62, 13);
            this.laIoState.TabIndex = 15;
            this.laIoState.Text = "Detached";
            // 
            // laStatus
            // 
            this.laStatus.AutoSize = true;
            this.laStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laStatus.Location = new System.Drawing.Point(12, 53);
            this.laStatus.Name = "laStatus";
            this.laStatus.Size = new System.Drawing.Size(85, 13);
            this.laStatus.TabIndex = 14;
            this.laStatus.Text = "Disconnected";
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
            // laDirection
            // 
            this.laDirection.AutoSize = true;
            this.laDirection.Enabled = false;
            this.laDirection.Location = new System.Drawing.Point(12, 111);
            this.laDirection.Name = "laDirection";
            this.laDirection.Size = new System.Drawing.Size(52, 13);
            this.laDirection.TabIndex = 16;
            this.laDirection.Text = "Direction:";
            // 
            // cbDirection
            // 
            this.cbDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDirection.Enabled = false;
            this.cbDirection.FormattingEnabled = true;
            this.cbDirection.Items.AddRange(new object[] {
            "Right",
            "Left"});
            this.cbDirection.Location = new System.Drawing.Point(70, 108);
            this.cbDirection.Name = "cbDirection";
            this.cbDirection.Size = new System.Drawing.Size(121, 21);
            this.cbDirection.TabIndex = 17;
            // 
            // laPower
            // 
            this.laPower.AutoSize = true;
            this.laPower.Enabled = false;
            this.laPower.Location = new System.Drawing.Point(208, 111);
            this.laPower.Name = "laPower";
            this.laPower.Size = new System.Drawing.Size(40, 13);
            this.laPower.TabIndex = 18;
            this.laPower.Text = "Power:";
            // 
            // edPower
            // 
            this.edPower.Enabled = false;
            this.edPower.Location = new System.Drawing.Point(254, 108);
            this.edPower.Name = "edPower";
            this.edPower.Size = new System.Drawing.Size(100, 20);
            this.edPower.TabIndex = 19;
            this.edPower.Text = "20";
            // 
            // btStart
            // 
            this.btStart.Enabled = false;
            this.btStart.Location = new System.Drawing.Point(360, 106);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(75, 23);
            this.btStart.TabIndex = 20;
            this.btStart.Text = "Start";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.BtStart_Click);
            // 
            // btBrake
            // 
            this.btBrake.Enabled = false;
            this.btBrake.Location = new System.Drawing.Point(70, 146);
            this.btBrake.Name = "btBrake";
            this.btBrake.Size = new System.Drawing.Size(75, 23);
            this.btBrake.TabIndex = 21;
            this.btBrake.Text = "Brake";
            this.btBrake.UseVisualStyleBackColor = true;
            this.btBrake.Click += new System.EventHandler(this.BtBrake_Click);
            // 
            // btDrift
            // 
            this.btDrift.Enabled = false;
            this.btDrift.Location = new System.Drawing.Point(168, 146);
            this.btDrift.Name = "btDrift";
            this.btDrift.Size = new System.Drawing.Size(75, 23);
            this.btDrift.TabIndex = 22;
            this.btDrift.Text = "Drift";
            this.btDrift.UseVisualStyleBackColor = true;
            this.btDrift.Click += new System.EventHandler(this.BtDrift_Click);
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 185);
            this.Controls.Add(this.btDrift);
            this.Controls.Add(this.btBrake);
            this.Controls.Add(this.btStart);
            this.Controls.Add(this.edPower);
            this.Controls.Add(this.laPower);
            this.Controls.Add(this.cbDirection);
            this.Controls.Add(this.laDirection);
            this.Controls.Add(this.laIoState);
            this.Controls.Add(this.laStatus);
            this.Controls.Add(this.btDisconnect);
            this.Controls.Add(this.btConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "fmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WeDo Motor Test Application";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FmMain_FormClosed);
            this.Load += new System.EventHandler(this.FmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label laIoState;
        private System.Windows.Forms.Label laStatus;
        private System.Windows.Forms.Button btDisconnect;
        private System.Windows.Forms.Button btConnect;
        private System.Windows.Forms.Label laDirection;
        private System.Windows.Forms.ComboBox cbDirection;
        private System.Windows.Forms.Label laPower;
        private System.Windows.Forms.TextBox edPower;
        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.Button btBrake;
        private System.Windows.Forms.Button btDrift;
    }
}

