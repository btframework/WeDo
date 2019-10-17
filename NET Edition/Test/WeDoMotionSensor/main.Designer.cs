namespace WeDoMotionSensor
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
            this.laMode = new System.Windows.Forms.Label();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.btChange = new System.Windows.Forms.Button();
            this.laCountTitle = new System.Windows.Forms.Label();
            this.laCount = new System.Windows.Forms.Label();
            this.laDistanceTitle = new System.Windows.Forms.Label();
            this.laDistance = new System.Windows.Forms.Label();
            this.btReset = new System.Windows.Forms.Button();
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
            // laMode
            // 
            this.laMode.AutoSize = true;
            this.laMode.Enabled = false;
            this.laMode.Location = new System.Drawing.Point(24, 96);
            this.laMode.Name = "laMode";
            this.laMode.Size = new System.Drawing.Size(37, 13);
            this.laMode.TabIndex = 16;
            this.laMode.Text = "Mode:";
            // 
            // cbMode
            // 
            this.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMode.Enabled = false;
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Items.AddRange(new object[] {
            "Detect",
            "Count"});
            this.cbMode.Location = new System.Drawing.Point(67, 93);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(121, 21);
            this.cbMode.TabIndex = 17;
            // 
            // btChange
            // 
            this.btChange.Enabled = false;
            this.btChange.Location = new System.Drawing.Point(194, 91);
            this.btChange.Name = "btChange";
            this.btChange.Size = new System.Drawing.Size(75, 23);
            this.btChange.TabIndex = 18;
            this.btChange.Text = "Change";
            this.btChange.UseVisualStyleBackColor = true;
            this.btChange.Click += new System.EventHandler(this.BtChange_Click);
            // 
            // laCountTitle
            // 
            this.laCountTitle.AutoSize = true;
            this.laCountTitle.Enabled = false;
            this.laCountTitle.Location = new System.Drawing.Point(26, 142);
            this.laCountTitle.Name = "laCountTitle";
            this.laCountTitle.Size = new System.Drawing.Size(38, 13);
            this.laCountTitle.TabIndex = 19;
            this.laCountTitle.Text = "Count:";
            // 
            // laCount
            // 
            this.laCount.AutoSize = true;
            this.laCount.Enabled = false;
            this.laCount.Location = new System.Drawing.Point(90, 142);
            this.laCount.Name = "laCount";
            this.laCount.Size = new System.Drawing.Size(13, 13);
            this.laCount.TabIndex = 20;
            this.laCount.Text = "0";
            // 
            // laDistanceTitle
            // 
            this.laDistanceTitle.AutoSize = true;
            this.laDistanceTitle.Enabled = false;
            this.laDistanceTitle.Location = new System.Drawing.Point(24, 171);
            this.laDistanceTitle.Name = "laDistanceTitle";
            this.laDistanceTitle.Size = new System.Drawing.Size(52, 13);
            this.laDistanceTitle.TabIndex = 21;
            this.laDistanceTitle.Text = "Distance:";
            // 
            // laDistance
            // 
            this.laDistance.AutoSize = true;
            this.laDistance.Enabled = false;
            this.laDistance.Location = new System.Drawing.Point(90, 171);
            this.laDistance.Name = "laDistance";
            this.laDistance.Size = new System.Drawing.Size(13, 13);
            this.laDistance.TabIndex = 22;
            this.laDistance.Text = "0";
            // 
            // btReset
            // 
            this.btReset.Enabled = false;
            this.btReset.Location = new System.Drawing.Point(194, 120);
            this.btReset.Name = "btReset";
            this.btReset.Size = new System.Drawing.Size(75, 23);
            this.btReset.TabIndex = 23;
            this.btReset.Text = "Reset";
            this.btReset.UseVisualStyleBackColor = true;
            this.btReset.Click += new System.EventHandler(this.BtReset_Click);
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 205);
            this.Controls.Add(this.btReset);
            this.Controls.Add(this.laDistance);
            this.Controls.Add(this.laDistanceTitle);
            this.Controls.Add(this.laCount);
            this.Controls.Add(this.laCountTitle);
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
            this.Text = "WeDo Motion Sensor Test";
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
        private System.Windows.Forms.Label laMode;
        private System.Windows.Forms.ComboBox cbMode;
        private System.Windows.Forms.Button btChange;
        private System.Windows.Forms.Label laCountTitle;
        private System.Windows.Forms.Label laCount;
        private System.Windows.Forms.Label laDistanceTitle;
        private System.Windows.Forms.Label laDistance;
        private System.Windows.Forms.Button btReset;
    }
}

