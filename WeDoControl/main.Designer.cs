namespace WeDoControl
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
            this.components = new System.ComponentModel.Container();
            this.btConnect = new System.Windows.Forms.Button();
            this.btDisconnect = new System.Windows.Forms.Button();
            this.laState = new System.Windows.Forms.Label();
            this.laDeviceInformationTitle = new System.Windows.Forms.Label();
            this.laFirmwareVersionTitle = new System.Windows.Forms.Label();
            this.laHardwareVersionTitle = new System.Windows.Forms.Label();
            this.laSoftwareVersionTitle = new System.Windows.Forms.Label();
            this.laManufacturerNameTitle = new System.Windows.Forms.Label();
            this.laFirmwareVersion = new System.Windows.Forms.Label();
            this.laHardwareVersion = new System.Windows.Forms.Label();
            this.laSoftwareVersion = new System.Windows.Forms.Label();
            this.laManufacturerName = new System.Windows.Forms.Label();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btConnect
            // 
            this.btConnect.Location = new System.Drawing.Point(12, 12);
            this.btConnect.Name = "btConnect";
            this.btConnect.Size = new System.Drawing.Size(75, 23);
            this.btConnect.TabIndex = 0;
            this.btConnect.Text = "Connect";
            this.btConnect.UseVisualStyleBackColor = true;
            this.btConnect.Click += new System.EventHandler(this.BtConnect_Click);
            // 
            // btDisconnect
            // 
            this.btDisconnect.Enabled = false;
            this.btDisconnect.Location = new System.Drawing.Point(93, 12);
            this.btDisconnect.Name = "btDisconnect";
            this.btDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btDisconnect.TabIndex = 1;
            this.btDisconnect.Text = "Disconnect";
            this.btDisconnect.UseVisualStyleBackColor = true;
            this.btDisconnect.Click += new System.EventHandler(this.BtDisconnect_Click);
            // 
            // laState
            // 
            this.laState.AutoSize = true;
            this.laState.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laState.Location = new System.Drawing.Point(174, 17);
            this.laState.Name = "laState";
            this.laState.Size = new System.Drawing.Size(85, 13);
            this.laState.TabIndex = 2;
            this.laState.Text = "Disconnected";
            // 
            // laDeviceInformationTitle
            // 
            this.laDeviceInformationTitle.AutoSize = true;
            this.laDeviceInformationTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laDeviceInformationTitle.Location = new System.Drawing.Point(30, 49);
            this.laDeviceInformationTitle.Name = "laDeviceInformationTitle";
            this.laDeviceInformationTitle.Size = new System.Drawing.Size(113, 13);
            this.laDeviceInformationTitle.TabIndex = 3;
            this.laDeviceInformationTitle.Text = "Device information";
            // 
            // laFirmwareVersionTitle
            // 
            this.laFirmwareVersionTitle.AutoSize = true;
            this.laFirmwareVersionTitle.Location = new System.Drawing.Point(52, 74);
            this.laFirmwareVersionTitle.Name = "laFirmwareVersionTitle";
            this.laFirmwareVersionTitle.Size = new System.Drawing.Size(89, 13);
            this.laFirmwareVersionTitle.TabIndex = 4;
            this.laFirmwareVersionTitle.Text = "Firmware version:";
            // 
            // laHardwareVersionTitle
            // 
            this.laHardwareVersionTitle.AutoSize = true;
            this.laHardwareVersionTitle.Location = new System.Drawing.Point(52, 98);
            this.laHardwareVersionTitle.Name = "laHardwareVersionTitle";
            this.laHardwareVersionTitle.Size = new System.Drawing.Size(93, 13);
            this.laHardwareVersionTitle.TabIndex = 5;
            this.laHardwareVersionTitle.Text = "Hardware version:";
            // 
            // laSoftwareVersionTitle
            // 
            this.laSoftwareVersionTitle.AutoSize = true;
            this.laSoftwareVersionTitle.Location = new System.Drawing.Point(52, 122);
            this.laSoftwareVersionTitle.Name = "laSoftwareVersionTitle";
            this.laSoftwareVersionTitle.Size = new System.Drawing.Size(89, 13);
            this.laSoftwareVersionTitle.TabIndex = 6;
            this.laSoftwareVersionTitle.Text = "Software version:";
            // 
            // laManufacturerNameTitle
            // 
            this.laManufacturerNameTitle.AutoSize = true;
            this.laManufacturerNameTitle.Location = new System.Drawing.Point(52, 146);
            this.laManufacturerNameTitle.Name = "laManufacturerNameTitle";
            this.laManufacturerNameTitle.Size = new System.Drawing.Size(102, 13);
            this.laManufacturerNameTitle.TabIndex = 7;
            this.laManufacturerNameTitle.Text = "Manufacturer name:";
            // 
            // laFirmwareVersion
            // 
            this.laFirmwareVersion.AutoSize = true;
            this.laFirmwareVersion.Location = new System.Drawing.Point(162, 74);
            this.laFirmwareVersion.Name = "laFirmwareVersion";
            this.laFirmwareVersion.Size = new System.Drawing.Size(47, 13);
            this.laFirmwareVersion.TabIndex = 8;
            this.laFirmwareVersion.Text = "<empty>";
            // 
            // laHardwareVersion
            // 
            this.laHardwareVersion.AutoSize = true;
            this.laHardwareVersion.Location = new System.Drawing.Point(162, 98);
            this.laHardwareVersion.Name = "laHardwareVersion";
            this.laHardwareVersion.Size = new System.Drawing.Size(47, 13);
            this.laHardwareVersion.TabIndex = 9;
            this.laHardwareVersion.Text = "<empty>";
            // 
            // laSoftwareVersion
            // 
            this.laSoftwareVersion.AutoSize = true;
            this.laSoftwareVersion.Location = new System.Drawing.Point(162, 122);
            this.laSoftwareVersion.Name = "laSoftwareVersion";
            this.laSoftwareVersion.Size = new System.Drawing.Size(47, 13);
            this.laSoftwareVersion.TabIndex = 10;
            this.laSoftwareVersion.Text = "<empty>";
            // 
            // laManufacturerName
            // 
            this.laManufacturerName.AutoSize = true;
            this.laManufacturerName.Location = new System.Drawing.Point(162, 146);
            this.laManufacturerName.Name = "laManufacturerName";
            this.laManufacturerName.Size = new System.Drawing.Size(47, 13);
            this.laManufacturerName.TabIndex = 11;
            this.laManufacturerName.Text = "<empty>";
            // 
            // Timer
            // 
            this.Timer.Interval = 60000;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.laManufacturerName);
            this.Controls.Add(this.laSoftwareVersion);
            this.Controls.Add(this.laHardwareVersion);
            this.Controls.Add(this.laFirmwareVersion);
            this.Controls.Add(this.laManufacturerNameTitle);
            this.Controls.Add(this.laSoftwareVersionTitle);
            this.Controls.Add(this.laHardwareVersionTitle);
            this.Controls.Add(this.laFirmwareVersionTitle);
            this.Controls.Add(this.laDeviceInformationTitle);
            this.Controls.Add(this.laState);
            this.Controls.Add(this.btDisconnect);
            this.Controls.Add(this.btConnect);
            this.Name = "fmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LEGO WeDo Control";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FmMain_FormClosed);
            this.Load += new System.EventHandler(this.FmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btConnect;
        private System.Windows.Forms.Button btDisconnect;
        private System.Windows.Forms.Label laState;
        private System.Windows.Forms.Label laDeviceInformationTitle;
        private System.Windows.Forms.Label laFirmwareVersionTitle;
        private System.Windows.Forms.Label laHardwareVersionTitle;
        private System.Windows.Forms.Label laSoftwareVersionTitle;
        private System.Windows.Forms.Label laManufacturerNameTitle;
        private System.Windows.Forms.Label laFirmwareVersion;
        private System.Windows.Forms.Label laHardwareVersion;
        private System.Windows.Forms.Label laSoftwareVersion;
        private System.Windows.Forms.Label laManufacturerName;
        private System.Windows.Forms.Timer Timer;
    }
}

