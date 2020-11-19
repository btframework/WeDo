namespace WeDoTiltSensor
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
            this.btReset = new System.Windows.Forms.Button();
            this.btChange = new System.Windows.Forms.Button();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.laMode = new System.Windows.Forms.Label();
            this.laIoState = new System.Windows.Forms.Label();
            this.laStatus = new System.Windows.Forms.Label();
            this.btDisconnect = new System.Windows.Forms.Button();
            this.btConnect = new System.Windows.Forms.Button();
            this.laDirectionTitle = new System.Windows.Forms.Label();
            this.laDirection = new System.Windows.Forms.Label();
            this.laXTitle = new System.Windows.Forms.Label();
            this.laX = new System.Windows.Forms.Label();
            this.laYTitle = new System.Windows.Forms.Label();
            this.laY = new System.Windows.Forms.Label();
            this.laZTitle = new System.Windows.Forms.Label();
            this.laZ = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btReset
            // 
            this.btReset.Enabled = false;
            this.btReset.Location = new System.Drawing.Point(194, 120);
            this.btReset.Name = "btReset";
            this.btReset.Size = new System.Drawing.Size(75, 23);
            this.btReset.TabIndex = 31;
            this.btReset.Text = "Reset";
            this.btReset.UseVisualStyleBackColor = true;
            this.btReset.Click += new System.EventHandler(this.BtReset_Click);
            // 
            // btChange
            // 
            this.btChange.Enabled = false;
            this.btChange.Location = new System.Drawing.Point(194, 91);
            this.btChange.Name = "btChange";
            this.btChange.Size = new System.Drawing.Size(75, 23);
            this.btChange.TabIndex = 30;
            this.btChange.Text = "Change";
            this.btChange.UseVisualStyleBackColor = true;
            this.btChange.Click += new System.EventHandler(this.BtChange_Click);
            // 
            // cbMode
            // 
            this.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMode.Enabled = false;
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Items.AddRange(new object[] {
            "Angle",
            "Tilt",
            "Crash"});
            this.cbMode.Location = new System.Drawing.Point(67, 93);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(121, 21);
            this.cbMode.TabIndex = 29;
            this.cbMode.SelectedIndexChanged += new System.EventHandler(this.cbMode_SelectedIndexChanged);
            // 
            // laMode
            // 
            this.laMode.AutoSize = true;
            this.laMode.Enabled = false;
            this.laMode.Location = new System.Drawing.Point(24, 96);
            this.laMode.Name = "laMode";
            this.laMode.Size = new System.Drawing.Size(37, 13);
            this.laMode.TabIndex = 28;
            this.laMode.Text = "Mode:";
            this.laMode.Click += new System.EventHandler(this.laMode_Click);
            // 
            // laIoState
            // 
            this.laIoState.AutoSize = true;
            this.laIoState.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laIoState.Location = new System.Drawing.Point(181, 53);
            this.laIoState.Name = "laIoState";
            this.laIoState.Size = new System.Drawing.Size(62, 13);
            this.laIoState.TabIndex = 27;
            this.laIoState.Text = "Detached";
            // 
            // laStatus
            // 
            this.laStatus.AutoSize = true;
            this.laStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laStatus.Location = new System.Drawing.Point(12, 53);
            this.laStatus.Name = "laStatus";
            this.laStatus.Size = new System.Drawing.Size(85, 13);
            this.laStatus.TabIndex = 26;
            this.laStatus.Text = "Disconnected";
            // 
            // btDisconnect
            // 
            this.btDisconnect.Enabled = false;
            this.btDisconnect.Location = new System.Drawing.Point(93, 12);
            this.btDisconnect.Name = "btDisconnect";
            this.btDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btDisconnect.TabIndex = 25;
            this.btDisconnect.Text = "Disconnect";
            this.btDisconnect.UseVisualStyleBackColor = true;
            this.btDisconnect.Click += new System.EventHandler(this.BtDisconnect_Click);
            // 
            // btConnect
            // 
            this.btConnect.Location = new System.Drawing.Point(12, 12);
            this.btConnect.Name = "btConnect";
            this.btConnect.Size = new System.Drawing.Size(75, 23);
            this.btConnect.TabIndex = 24;
            this.btConnect.Text = "Connect";
            this.btConnect.UseVisualStyleBackColor = true;
            this.btConnect.Click += new System.EventHandler(this.BtConnect_Click);
            // 
            // laDirectionTitle
            // 
            this.laDirectionTitle.AutoSize = true;
            this.laDirectionTitle.Enabled = false;
            this.laDirectionTitle.Location = new System.Drawing.Point(24, 176);
            this.laDirectionTitle.Name = "laDirectionTitle";
            this.laDirectionTitle.Size = new System.Drawing.Size(52, 13);
            this.laDirectionTitle.TabIndex = 32;
            this.laDirectionTitle.Text = "Direction:";
            // 
            // laDirection
            // 
            this.laDirection.AutoSize = true;
            this.laDirection.Enabled = false;
            this.laDirection.Location = new System.Drawing.Point(100, 176);
            this.laDirection.Name = "laDirection";
            this.laDirection.Size = new System.Drawing.Size(53, 13);
            this.laDirection.TabIndex = 33;
            this.laDirection.Text = "Unknown";
            // 
            // laXTitle
            // 
            this.laXTitle.AutoSize = true;
            this.laXTitle.Enabled = false;
            this.laXTitle.Location = new System.Drawing.Point(24, 200);
            this.laXTitle.Name = "laXTitle";
            this.laXTitle.Size = new System.Drawing.Size(17, 13);
            this.laXTitle.TabIndex = 34;
            this.laXTitle.Text = "X:";
            // 
            // laX
            // 
            this.laX.AutoSize = true;
            this.laX.Enabled = false;
            this.laX.Location = new System.Drawing.Point(100, 200);
            this.laX.Name = "laX";
            this.laX.Size = new System.Drawing.Size(13, 13);
            this.laX.TabIndex = 35;
            this.laX.Text = "0";
            // 
            // laYTitle
            // 
            this.laYTitle.AutoSize = true;
            this.laYTitle.Enabled = false;
            this.laYTitle.Location = new System.Drawing.Point(24, 223);
            this.laYTitle.Name = "laYTitle";
            this.laYTitle.Size = new System.Drawing.Size(17, 13);
            this.laYTitle.TabIndex = 36;
            this.laYTitle.Text = "Y:";
            // 
            // laY
            // 
            this.laY.AutoSize = true;
            this.laY.Enabled = false;
            this.laY.Location = new System.Drawing.Point(100, 223);
            this.laY.Name = "laY";
            this.laY.Size = new System.Drawing.Size(13, 13);
            this.laY.TabIndex = 37;
            this.laY.Text = "0";
            // 
            // laZTitle
            // 
            this.laZTitle.AutoSize = true;
            this.laZTitle.Enabled = false;
            this.laZTitle.Location = new System.Drawing.Point(24, 248);
            this.laZTitle.Name = "laZTitle";
            this.laZTitle.Size = new System.Drawing.Size(17, 13);
            this.laZTitle.TabIndex = 38;
            this.laZTitle.Text = "Z:";
            // 
            // laZ
            // 
            this.laZ.AutoSize = true;
            this.laZ.Enabled = false;
            this.laZ.Location = new System.Drawing.Point(100, 248);
            this.laZ.Name = "laZ";
            this.laZ.Size = new System.Drawing.Size(13, 13);
            this.laZ.TabIndex = 39;
            this.laZ.Text = "0";
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 279);
            this.Controls.Add(this.laZ);
            this.Controls.Add(this.laZTitle);
            this.Controls.Add(this.laY);
            this.Controls.Add(this.laYTitle);
            this.Controls.Add(this.laX);
            this.Controls.Add(this.laXTitle);
            this.Controls.Add(this.laDirection);
            this.Controls.Add(this.laDirectionTitle);
            this.Controls.Add(this.btReset);
            this.Controls.Add(this.btChange);
            this.Controls.Add(this.cbMode);
            this.Controls.Add(this.laMode);
            this.Controls.Add(this.laIoState);
            this.Controls.Add(this.laStatus);
            this.Controls.Add(this.btDisconnect);
            this.Controls.Add(this.btConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "fmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WeDo Tilt Sensor Test";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FmMain_FormClosed);
            this.Load += new System.EventHandler(this.FmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btReset;
        private System.Windows.Forms.Button btChange;
        private System.Windows.Forms.ComboBox cbMode;
        private System.Windows.Forms.Label laMode;
        private System.Windows.Forms.Label laIoState;
        private System.Windows.Forms.Label laStatus;
        private System.Windows.Forms.Button btDisconnect;
        private System.Windows.Forms.Button btConnect;
        private System.Windows.Forms.Label laDirectionTitle;
        private System.Windows.Forms.Label laDirection;
        private System.Windows.Forms.Label laXTitle;
        private System.Windows.Forms.Label laX;
        private System.Windows.Forms.Label laYTitle;
        private System.Windows.Forms.Label laY;
        private System.Windows.Forms.Label laZTitle;
        private System.Windows.Forms.Label laZ;
    }
}

