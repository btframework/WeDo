namespace WeDoScan
{
    partial class fmDevInfo
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
            this.laManufacturerName = new System.Windows.Forms.Label();
            this.laSoftwareVersion = new System.Windows.Forms.Label();
            this.laHardwareVersion = new System.Windows.Forms.Label();
            this.laFirmwareVersion = new System.Windows.Forms.Label();
            this.laManufacturerNameTitle = new System.Windows.Forms.Label();
            this.laSoftwareVersionTitle = new System.Windows.Forms.Label();
            this.laHardwareVersionTitle = new System.Windows.Forms.Label();
            this.laFirmwareVersionTitle = new System.Windows.Forms.Label();
            this.laDeviceInformationTitle = new System.Windows.Forms.Label();
            this.btSetDeviceName = new System.Windows.Forms.Button();
            this.edDeviceName = new System.Windows.Forms.TextBox();
            this.laDeviceName = new System.Windows.Forms.Label();
            this.btClose = new System.Windows.Forms.Button();
            this.laBattLevelTitle = new System.Windows.Forms.Label();
            this.pbBattLevel = new System.Windows.Forms.ProgressBar();
            this.laBattLevel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // laManufacturerName
            // 
            this.laManufacturerName.AutoSize = true;
            this.laManufacturerName.Location = new System.Drawing.Point(144, 106);
            this.laManufacturerName.Name = "laManufacturerName";
            this.laManufacturerName.Size = new System.Drawing.Size(47, 13);
            this.laManufacturerName.TabIndex = 20;
            this.laManufacturerName.Text = "<empty>";
            // 
            // laSoftwareVersion
            // 
            this.laSoftwareVersion.AutoSize = true;
            this.laSoftwareVersion.Location = new System.Drawing.Point(144, 82);
            this.laSoftwareVersion.Name = "laSoftwareVersion";
            this.laSoftwareVersion.Size = new System.Drawing.Size(47, 13);
            this.laSoftwareVersion.TabIndex = 19;
            this.laSoftwareVersion.Text = "<empty>";
            // 
            // laHardwareVersion
            // 
            this.laHardwareVersion.AutoSize = true;
            this.laHardwareVersion.Location = new System.Drawing.Point(144, 58);
            this.laHardwareVersion.Name = "laHardwareVersion";
            this.laHardwareVersion.Size = new System.Drawing.Size(47, 13);
            this.laHardwareVersion.TabIndex = 18;
            this.laHardwareVersion.Text = "<empty>";
            // 
            // laFirmwareVersion
            // 
            this.laFirmwareVersion.AutoSize = true;
            this.laFirmwareVersion.Location = new System.Drawing.Point(144, 34);
            this.laFirmwareVersion.Name = "laFirmwareVersion";
            this.laFirmwareVersion.Size = new System.Drawing.Size(47, 13);
            this.laFirmwareVersion.TabIndex = 17;
            this.laFirmwareVersion.Text = "<empty>";
            // 
            // laManufacturerNameTitle
            // 
            this.laManufacturerNameTitle.AutoSize = true;
            this.laManufacturerNameTitle.Location = new System.Drawing.Point(34, 106);
            this.laManufacturerNameTitle.Name = "laManufacturerNameTitle";
            this.laManufacturerNameTitle.Size = new System.Drawing.Size(102, 13);
            this.laManufacturerNameTitle.TabIndex = 16;
            this.laManufacturerNameTitle.Text = "Manufacturer name:";
            // 
            // laSoftwareVersionTitle
            // 
            this.laSoftwareVersionTitle.AutoSize = true;
            this.laSoftwareVersionTitle.Location = new System.Drawing.Point(34, 82);
            this.laSoftwareVersionTitle.Name = "laSoftwareVersionTitle";
            this.laSoftwareVersionTitle.Size = new System.Drawing.Size(89, 13);
            this.laSoftwareVersionTitle.TabIndex = 15;
            this.laSoftwareVersionTitle.Text = "Software version:";
            // 
            // laHardwareVersionTitle
            // 
            this.laHardwareVersionTitle.AutoSize = true;
            this.laHardwareVersionTitle.Location = new System.Drawing.Point(34, 58);
            this.laHardwareVersionTitle.Name = "laHardwareVersionTitle";
            this.laHardwareVersionTitle.Size = new System.Drawing.Size(93, 13);
            this.laHardwareVersionTitle.TabIndex = 14;
            this.laHardwareVersionTitle.Text = "Hardware version:";
            // 
            // laFirmwareVersionTitle
            // 
            this.laFirmwareVersionTitle.AutoSize = true;
            this.laFirmwareVersionTitle.Location = new System.Drawing.Point(34, 34);
            this.laFirmwareVersionTitle.Name = "laFirmwareVersionTitle";
            this.laFirmwareVersionTitle.Size = new System.Drawing.Size(89, 13);
            this.laFirmwareVersionTitle.TabIndex = 13;
            this.laFirmwareVersionTitle.Text = "Firmware version:";
            // 
            // laDeviceInformationTitle
            // 
            this.laDeviceInformationTitle.AutoSize = true;
            this.laDeviceInformationTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laDeviceInformationTitle.Location = new System.Drawing.Point(12, 9);
            this.laDeviceInformationTitle.Name = "laDeviceInformationTitle";
            this.laDeviceInformationTitle.Size = new System.Drawing.Size(113, 13);
            this.laDeviceInformationTitle.TabIndex = 12;
            this.laDeviceInformationTitle.Text = "Device information";
            // 
            // btSetDeviceName
            // 
            this.btSetDeviceName.Location = new System.Drawing.Point(114, 229);
            this.btSetDeviceName.Name = "btSetDeviceName";
            this.btSetDeviceName.Size = new System.Drawing.Size(75, 23);
            this.btSetDeviceName.TabIndex = 23;
            this.btSetDeviceName.Text = "Set";
            this.btSetDeviceName.UseVisualStyleBackColor = true;
            this.btSetDeviceName.Click += new System.EventHandler(this.BtSetDeviceName_Click);
            // 
            // edDeviceName
            // 
            this.edDeviceName.Location = new System.Drawing.Point(37, 203);
            this.edDeviceName.Name = "edDeviceName";
            this.edDeviceName.Size = new System.Drawing.Size(152, 20);
            this.edDeviceName.TabIndex = 22;
            // 
            // laDeviceName
            // 
            this.laDeviceName.AutoSize = true;
            this.laDeviceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laDeviceName.Location = new System.Drawing.Point(12, 181);
            this.laDeviceName.Name = "laDeviceName";
            this.laDeviceName.Size = new System.Drawing.Size(85, 13);
            this.laDeviceName.TabIndex = 21;
            this.laDeviceName.Text = "Device name:";
            // 
            // btClose
            // 
            this.btClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btClose.Location = new System.Drawing.Point(234, 275);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(75, 23);
            this.btClose.TabIndex = 24;
            this.btClose.Text = "Close";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.BtClose_Click);
            // 
            // laBattLevelTitle
            // 
            this.laBattLevelTitle.AutoSize = true;
            this.laBattLevelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laBattLevelTitle.Location = new System.Drawing.Point(12, 148);
            this.laBattLevelTitle.Name = "laBattLevelTitle";
            this.laBattLevelTitle.Size = new System.Drawing.Size(82, 13);
            this.laBattLevelTitle.TabIndex = 25;
            this.laBattLevelTitle.Text = "Battery level:";
            // 
            // pbBattLevel
            // 
            this.pbBattLevel.Location = new System.Drawing.Point(100, 138);
            this.pbBattLevel.Name = "pbBattLevel";
            this.pbBattLevel.Size = new System.Drawing.Size(75, 23);
            this.pbBattLevel.TabIndex = 26;
            // 
            // laBattLevel
            // 
            this.laBattLevel.AutoSize = true;
            this.laBattLevel.Location = new System.Drawing.Point(192, 148);
            this.laBattLevel.Name = "laBattLevel";
            this.laBattLevel.Size = new System.Drawing.Size(24, 13);
            this.laBattLevel.TabIndex = 27;
            this.laBattLevel.Text = "0 %";
            // 
            // fmDevInfo
            // 
            this.AcceptButton = this.btClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btClose;
            this.ClientSize = new System.Drawing.Size(321, 306);
            this.Controls.Add(this.laBattLevel);
            this.Controls.Add(this.pbBattLevel);
            this.Controls.Add(this.laBattLevelTitle);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.btSetDeviceName);
            this.Controls.Add(this.edDeviceName);
            this.Controls.Add(this.laDeviceName);
            this.Controls.Add(this.laManufacturerName);
            this.Controls.Add(this.laSoftwareVersion);
            this.Controls.Add(this.laHardwareVersion);
            this.Controls.Add(this.laFirmwareVersion);
            this.Controls.Add(this.laManufacturerNameTitle);
            this.Controls.Add(this.laSoftwareVersionTitle);
            this.Controls.Add(this.laHardwareVersionTitle);
            this.Controls.Add(this.laFirmwareVersionTitle);
            this.Controls.Add(this.laDeviceInformationTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "fmDevInfo";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WeDo Device Information";
            this.Load += new System.EventHandler(this.FmDevInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label laManufacturerName;
        private System.Windows.Forms.Label laSoftwareVersion;
        private System.Windows.Forms.Label laHardwareVersion;
        private System.Windows.Forms.Label laFirmwareVersion;
        private System.Windows.Forms.Label laManufacturerNameTitle;
        private System.Windows.Forms.Label laSoftwareVersionTitle;
        private System.Windows.Forms.Label laHardwareVersionTitle;
        private System.Windows.Forms.Label laFirmwareVersionTitle;
        private System.Windows.Forms.Label laDeviceInformationTitle;
        private System.Windows.Forms.Button btSetDeviceName;
        private System.Windows.Forms.TextBox edDeviceName;
        private System.Windows.Forms.Label laDeviceName;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Label laBattLevelTitle;
        private System.Windows.Forms.ProgressBar pbBattLevel;
        private System.Windows.Forms.Label laBattLevel;
    }
}