namespace WeDoScan
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
            this.chHubAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chHubName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btInfo = new System.Windows.Forms.Button();
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
            this.btStart.Click += new System.EventHandler(this.BtStart_Click);
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
            this.btStop.Click += new System.EventHandler(this.BtStop_Click);
            // 
            // lvHubs
            // 
            this.lvHubs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chHubAddress,
            this.chHubName});
            this.lvHubs.FullRowSelect = true;
            this.lvHubs.GridLines = true;
            this.lvHubs.HideSelection = false;
            this.lvHubs.Location = new System.Drawing.Point(12, 41);
            this.lvHubs.Name = "lvHubs";
            this.lvHubs.Size = new System.Drawing.Size(406, 140);
            this.lvHubs.TabIndex = 2;
            this.lvHubs.UseCompatibleStateImageBehavior = false;
            this.lvHubs.View = System.Windows.Forms.View.Details;
            this.lvHubs.SelectedIndexChanged += new System.EventHandler(this.LvHubs_SelectedIndexChanged);
            // 
            // chHubAddress
            // 
            this.chHubAddress.Text = "Address";
            this.chHubAddress.Width = 140;
            // 
            // chHubName
            // 
            this.chHubName.Text = "Name";
            this.chHubName.Width = 210;
            // 
            // btInfo
            // 
            this.btInfo.Enabled = false;
            this.btInfo.Location = new System.Drawing.Point(12, 187);
            this.btInfo.Name = "btInfo";
            this.btInfo.Size = new System.Drawing.Size(406, 23);
            this.btInfo.TabIndex = 3;
            this.btInfo.Text = "Get Hub Information";
            this.btInfo.UseVisualStyleBackColor = true;
            this.btInfo.Click += new System.EventHandler(this.BtInfo_Click);
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 221);
            this.Controls.Add(this.btInfo);
            this.Controls.Add(this.lvHubs);
            this.Controls.Add(this.btStop);
            this.Controls.Add(this.btStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "fmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WeDo Search Application";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FmMain_FormClosed);
            this.Load += new System.EventHandler(this.FmMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.ListView lvHubs;
        private System.Windows.Forms.ColumnHeader chHubAddress;
        private System.Windows.Forms.ColumnHeader chHubName;
        private System.Windows.Forms.Button btInfo;
    }
}

