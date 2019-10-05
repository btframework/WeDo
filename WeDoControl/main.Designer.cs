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
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.pbBatt = new System.Windows.Forms.ProgressBar();
            this.laBatt = new System.Windows.Forms.Label();
            this.laBattPercent = new System.Windows.Forms.Label();
            this.btDevInfo = new System.Windows.Forms.Button();
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
            // Timer
            // 
            this.Timer.Interval = 60000;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // pbBatt
            // 
            this.pbBatt.Location = new System.Drawing.Point(646, 12);
            this.pbBatt.Name = "pbBatt";
            this.pbBatt.Size = new System.Drawing.Size(100, 23);
            this.pbBatt.TabIndex = 12;
            // 
            // laBatt
            // 
            this.laBatt.AutoSize = true;
            this.laBatt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laBatt.Location = new System.Drawing.Point(610, 17);
            this.laBatt.Name = "laBatt";
            this.laBatt.Size = new System.Drawing.Size(30, 13);
            this.laBatt.TabIndex = 13;
            this.laBatt.Text = "Batt";
            // 
            // laBattPercent
            // 
            this.laBattPercent.AutoSize = true;
            this.laBattPercent.Location = new System.Drawing.Point(752, 17);
            this.laBattPercent.Name = "laBattPercent";
            this.laBattPercent.Size = new System.Drawing.Size(21, 13);
            this.laBattPercent.TabIndex = 18;
            this.laBattPercent.Text = "0%";
            // 
            // btDevInfo
            // 
            this.btDevInfo.Enabled = false;
            this.btDevInfo.Location = new System.Drawing.Point(12, 59);
            this.btDevInfo.Name = "btDevInfo";
            this.btDevInfo.Size = new System.Drawing.Size(120, 23);
            this.btDevInfo.TabIndex = 19;
            this.btDevInfo.Text = "Device Information";
            this.btDevInfo.UseVisualStyleBackColor = true;
            this.btDevInfo.Click += new System.EventHandler(this.BtDevInfo_Click);
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btDevInfo);
            this.Controls.Add(this.laBattPercent);
            this.Controls.Add(this.laBatt);
            this.Controls.Add(this.pbBatt);
            this.Controls.Add(this.laState);
            this.Controls.Add(this.btDisconnect);
            this.Controls.Add(this.btConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
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
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.ProgressBar pbBatt;
        private System.Windows.Forms.Label laBatt;
        private System.Windows.Forms.Label laBattPercent;
        private System.Windows.Forms.Button btDevInfo;
    }
}

