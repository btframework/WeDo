namespace WeDoRgb
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
            this.laColorMode = new System.Windows.Forms.Label();
            this.cbColorMode = new System.Windows.Forms.ComboBox();
            this.laR = new System.Windows.Forms.Label();
            this.edR = new System.Windows.Forms.TextBox();
            this.laG = new System.Windows.Forms.Label();
            this.edG = new System.Windows.Forms.TextBox();
            this.laB = new System.Windows.Forms.Label();
            this.edB = new System.Windows.Forms.TextBox();
            this.btSetRgb = new System.Windows.Forms.Button();
            this.laColorIndex = new System.Windows.Forms.Label();
            this.edColorIndex = new System.Windows.Forms.TextBox();
            this.btSetIndex = new System.Windows.Forms.Button();
            this.btSetDefault = new System.Windows.Forms.Button();
            this.btTurnOff = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // laIoState
            // 
            this.laIoState.AutoSize = true;
            this.laIoState.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laIoState.Location = new System.Drawing.Point(181, 53);
            this.laIoState.Name = "laIoState";
            this.laIoState.Size = new System.Drawing.Size(62, 13);
            this.laIoState.TabIndex = 23;
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
            // laColorMode
            // 
            this.laColorMode.AutoSize = true;
            this.laColorMode.Enabled = false;
            this.laColorMode.Location = new System.Drawing.Point(21, 99);
            this.laColorMode.Name = "laColorMode";
            this.laColorMode.Size = new System.Drawing.Size(63, 13);
            this.laColorMode.TabIndex = 24;
            this.laColorMode.Text = "Color mode:";
            // 
            // cbColorMode
            // 
            this.cbColorMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbColorMode.Enabled = false;
            this.cbColorMode.FormattingEnabled = true;
            this.cbColorMode.Items.AddRange(new object[] {
            "Discrete",
            "Absolute"});
            this.cbColorMode.Location = new System.Drawing.Point(90, 96);
            this.cbColorMode.Name = "cbColorMode";
            this.cbColorMode.Size = new System.Drawing.Size(121, 21);
            this.cbColorMode.TabIndex = 25;
            this.cbColorMode.SelectedIndexChanged += new System.EventHandler(this.CbColorMode_SelectedIndexChanged);
            // 
            // laR
            // 
            this.laR.AutoSize = true;
            this.laR.Enabled = false;
            this.laR.Location = new System.Drawing.Point(21, 146);
            this.laR.Name = "laR";
            this.laR.Size = new System.Drawing.Size(18, 13);
            this.laR.TabIndex = 26;
            this.laR.Text = "R:";
            // 
            // edR
            // 
            this.edR.Enabled = false;
            this.edR.Location = new System.Drawing.Point(45, 143);
            this.edR.Name = "edR";
            this.edR.Size = new System.Drawing.Size(61, 20);
            this.edR.TabIndex = 27;
            // 
            // laG
            // 
            this.laG.AutoSize = true;
            this.laG.Enabled = false;
            this.laG.Location = new System.Drawing.Point(122, 146);
            this.laG.Name = "laG";
            this.laG.Size = new System.Drawing.Size(15, 13);
            this.laG.TabIndex = 28;
            this.laG.Text = "G";
            // 
            // edG
            // 
            this.edG.Enabled = false;
            this.edG.Location = new System.Drawing.Point(143, 143);
            this.edG.Name = "edG";
            this.edG.Size = new System.Drawing.Size(61, 20);
            this.edG.TabIndex = 29;
            // 
            // laB
            // 
            this.laB.AutoSize = true;
            this.laB.Enabled = false;
            this.laB.Location = new System.Drawing.Point(222, 146);
            this.laB.Name = "laB";
            this.laB.Size = new System.Drawing.Size(17, 13);
            this.laB.TabIndex = 30;
            this.laB.Text = "B:";
            // 
            // edB
            // 
            this.edB.Enabled = false;
            this.edB.Location = new System.Drawing.Point(245, 143);
            this.edB.Name = "edB";
            this.edB.Size = new System.Drawing.Size(61, 20);
            this.edB.TabIndex = 31;
            // 
            // btSetRgb
            // 
            this.btSetRgb.Enabled = false;
            this.btSetRgb.Location = new System.Drawing.Point(312, 141);
            this.btSetRgb.Name = "btSetRgb";
            this.btSetRgb.Size = new System.Drawing.Size(75, 23);
            this.btSetRgb.TabIndex = 32;
            this.btSetRgb.Text = "Set RGB";
            this.btSetRgb.UseVisualStyleBackColor = true;
            this.btSetRgb.Click += new System.EventHandler(this.BtSetRgb_Click);
            // 
            // laColorIndex
            // 
            this.laColorIndex.AutoSize = true;
            this.laColorIndex.Enabled = false;
            this.laColorIndex.Location = new System.Drawing.Point(21, 192);
            this.laColorIndex.Name = "laColorIndex";
            this.laColorIndex.Size = new System.Drawing.Size(59, 13);
            this.laColorIndex.TabIndex = 33;
            this.laColorIndex.Text = "Color index";
            // 
            // edColorIndex
            // 
            this.edColorIndex.Enabled = false;
            this.edColorIndex.Location = new System.Drawing.Point(86, 189);
            this.edColorIndex.Name = "edColorIndex";
            this.edColorIndex.Size = new System.Drawing.Size(82, 20);
            this.edColorIndex.TabIndex = 34;
            // 
            // btSetIndex
            // 
            this.btSetIndex.Enabled = false;
            this.btSetIndex.Location = new System.Drawing.Point(174, 187);
            this.btSetIndex.Name = "btSetIndex";
            this.btSetIndex.Size = new System.Drawing.Size(75, 23);
            this.btSetIndex.TabIndex = 35;
            this.btSetIndex.Text = "Set Index";
            this.btSetIndex.UseVisualStyleBackColor = true;
            this.btSetIndex.Click += new System.EventHandler(this.BtSetIndex_Click);
            // 
            // btSetDefault
            // 
            this.btSetDefault.Enabled = false;
            this.btSetDefault.Location = new System.Drawing.Point(22, 228);
            this.btSetDefault.Name = "btSetDefault";
            this.btSetDefault.Size = new System.Drawing.Size(75, 23);
            this.btSetDefault.TabIndex = 36;
            this.btSetDefault.Text = "Set default";
            this.btSetDefault.UseVisualStyleBackColor = true;
            this.btSetDefault.Click += new System.EventHandler(this.BtSetDefault_Click);
            // 
            // btTurnOff
            // 
            this.btTurnOff.Enabled = false;
            this.btTurnOff.Location = new System.Drawing.Point(103, 228);
            this.btTurnOff.Name = "btTurnOff";
            this.btTurnOff.Size = new System.Drawing.Size(75, 23);
            this.btTurnOff.TabIndex = 37;
            this.btTurnOff.Text = "Turn off";
            this.btTurnOff.UseVisualStyleBackColor = true;
            this.btTurnOff.Click += new System.EventHandler(this.BtTurnOff_Click);
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 450);
            this.Controls.Add(this.btTurnOff);
            this.Controls.Add(this.btSetDefault);
            this.Controls.Add(this.btSetIndex);
            this.Controls.Add(this.edColorIndex);
            this.Controls.Add(this.laColorIndex);
            this.Controls.Add(this.btSetRgb);
            this.Controls.Add(this.edB);
            this.Controls.Add(this.laB);
            this.Controls.Add(this.edG);
            this.Controls.Add(this.laG);
            this.Controls.Add(this.edR);
            this.Controls.Add(this.laR);
            this.Controls.Add(this.cbColorMode);
            this.Controls.Add(this.laColorMode);
            this.Controls.Add(this.laIoState);
            this.Controls.Add(this.laStatus);
            this.Controls.Add(this.btDisconnect);
            this.Controls.Add(this.btConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "fmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WeDo RGB Control Demo";
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
        private System.Windows.Forms.Label laColorMode;
        private System.Windows.Forms.ComboBox cbColorMode;
        private System.Windows.Forms.Label laR;
        private System.Windows.Forms.TextBox edR;
        private System.Windows.Forms.Label laG;
        private System.Windows.Forms.TextBox edG;
        private System.Windows.Forms.Label laB;
        private System.Windows.Forms.TextBox edB;
        private System.Windows.Forms.Button btSetRgb;
        private System.Windows.Forms.Label laColorIndex;
        private System.Windows.Forms.TextBox edColorIndex;
        private System.Windows.Forms.Button btSetIndex;
        private System.Windows.Forms.Button btSetDefault;
        private System.Windows.Forms.Button btTurnOff;
    }
}

