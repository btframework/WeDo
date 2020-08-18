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
            this.lvHubs = new System.Windows.Forms.ListView();
            this.chAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lbLog = new System.Windows.Forms.ListBox();
            this.btClear = new System.Windows.Forms.Button();
            this.btDisconnect = new System.Windows.Forms.Button();
            this.btLedOn = new System.Windows.Forms.Button();
            this.btLedOff = new System.Windows.Forms.Button();
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
            this.btStop.Location = new System.Drawing.Point(93, 12);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(75, 23);
            this.btStop.TabIndex = 1;
            this.btStop.Text = "Stop";
            this.btStop.UseVisualStyleBackColor = true;
            this.btStop.Click += new System.EventHandler(this.btStop_Click);
            // 
            // lvHubs
            // 
            this.lvHubs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chAddress});
            this.lvHubs.FullRowSelect = true;
            this.lvHubs.GridLines = true;
            this.lvHubs.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvHubs.HideSelection = false;
            this.lvHubs.Location = new System.Drawing.Point(12, 41);
            this.lvHubs.MultiSelect = false;
            this.lvHubs.Name = "lvHubs";
            this.lvHubs.Size = new System.Drawing.Size(184, 141);
            this.lvHubs.TabIndex = 2;
            this.lvHubs.UseCompatibleStateImageBehavior = false;
            this.lvHubs.View = System.Windows.Forms.View.Details;
            // 
            // chAddress
            // 
            this.chAddress.Text = "Address";
            this.chAddress.Width = 150;
            // 
            // lbLog
            // 
            this.lbLog.FormattingEnabled = true;
            this.lbLog.Location = new System.Drawing.Point(12, 214);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(525, 225);
            this.lbLog.TabIndex = 3;
            // 
            // btClear
            // 
            this.btClear.Location = new System.Drawing.Point(12, 188);
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(75, 23);
            this.btClear.TabIndex = 4;
            this.btClear.Text = "Clear";
            this.btClear.UseVisualStyleBackColor = true;
            // 
            // btDisconnect
            // 
            this.btDisconnect.Location = new System.Drawing.Point(202, 41);
            this.btDisconnect.Name = "btDisconnect";
            this.btDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btDisconnect.TabIndex = 5;
            this.btDisconnect.Text = "Disconnect";
            this.btDisconnect.UseVisualStyleBackColor = true;
            this.btDisconnect.Click += new System.EventHandler(this.btDisconnect_Click);
            // 
            // btLedOn
            // 
            this.btLedOn.Location = new System.Drawing.Point(202, 70);
            this.btLedOn.Name = "btLedOn";
            this.btLedOn.Size = new System.Drawing.Size(75, 23);
            this.btLedOn.TabIndex = 6;
            this.btLedOn.Text = "LED ON";
            this.btLedOn.UseVisualStyleBackColor = true;
            this.btLedOn.Click += new System.EventHandler(this.btLedOn_Click);
            // 
            // btLedOff
            // 
            this.btLedOff.Location = new System.Drawing.Point(202, 99);
            this.btLedOff.Name = "btLedOff";
            this.btLedOff.Size = new System.Drawing.Size(75, 23);
            this.btLedOff.TabIndex = 7;
            this.btLedOff.Text = "LED OFF";
            this.btLedOff.UseVisualStyleBackColor = true;
            this.btLedOff.Click += new System.EventHandler(this.btLedOff_Click);
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 450);
            this.Controls.Add(this.btLedOff);
            this.Controls.Add(this.btLedOn);
            this.Controls.Add(this.btDisconnect);
            this.Controls.Add(this.btClear);
            this.Controls.Add(this.lbLog);
            this.Controls.Add(this.lvHubs);
            this.Controls.Add(this.btStop);
            this.Controls.Add(this.btStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "fmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WeDo Robot Demo Application";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FmMain_FormClosed);
            this.Load += new System.EventHandler(this.FmMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.ListView lvHubs;
        private System.Windows.Forms.ColumnHeader chAddress;
        private System.Windows.Forms.ListBox lbLog;
        private System.Windows.Forms.Button btClear;
        private System.Windows.Forms.Button btDisconnect;
        private System.Windows.Forms.Button btLedOn;
        private System.Windows.Forms.Button btLedOff;
    }
}

