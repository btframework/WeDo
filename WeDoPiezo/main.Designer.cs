namespace WeDoPiezo
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
            this.btConnect = new System.Windows.Forms.Button();
            this.btDisconnect = new System.Windows.Forms.Button();
            this.laStatus = new System.Windows.Forms.Label();
            this.laNote = new System.Windows.Forms.Label();
            this.cbNote = new System.Windows.Forms.ComboBox();
            this.laOctave = new System.Windows.Forms.Label();
            this.cbOctave = new System.Windows.Forms.ComboBox();
            this.laDuration = new System.Windows.Forms.Label();
            this.edDuration = new System.Windows.Forms.TextBox();
            this.btPlay = new System.Windows.Forms.Button();
            this.btStop = new System.Windows.Forms.Button();
            this.laIoState = new System.Windows.Forms.Label();
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
            // laStatus
            // 
            this.laStatus.AutoSize = true;
            this.laStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laStatus.Location = new System.Drawing.Point(12, 53);
            this.laStatus.Name = "laStatus";
            this.laStatus.Size = new System.Drawing.Size(85, 13);
            this.laStatus.TabIndex = 2;
            this.laStatus.Text = "Disconnected";
            // 
            // laNote
            // 
            this.laNote.AutoSize = true;
            this.laNote.Location = new System.Drawing.Point(12, 100);
            this.laNote.Name = "laNote";
            this.laNote.Size = new System.Drawing.Size(30, 13);
            this.laNote.TabIndex = 3;
            this.laNote.Text = "Note";
            // 
            // cbNote
            // 
            this.cbNote.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNote.Enabled = false;
            this.cbNote.FormattingEnabled = true;
            this.cbNote.Items.AddRange(new object[] {
            "A",
            "A#",
            "B",
            "C",
            "C#",
            "D",
            "D#",
            "E",
            "F",
            "F#",
            "G",
            "G#"});
            this.cbNote.Location = new System.Drawing.Point(47, 97);
            this.cbNote.Name = "cbNote";
            this.cbNote.Size = new System.Drawing.Size(83, 21);
            this.cbNote.TabIndex = 4;
            // 
            // laOctave
            // 
            this.laOctave.AutoSize = true;
            this.laOctave.Location = new System.Drawing.Point(136, 100);
            this.laOctave.Name = "laOctave";
            this.laOctave.Size = new System.Drawing.Size(42, 13);
            this.laOctave.TabIndex = 5;
            this.laOctave.Text = "Octave";
            // 
            // cbOctave
            // 
            this.cbOctave.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOctave.Enabled = false;
            this.cbOctave.FormattingEnabled = true;
            this.cbOctave.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.cbOctave.Location = new System.Drawing.Point(184, 97);
            this.cbOctave.Name = "cbOctave";
            this.cbOctave.Size = new System.Drawing.Size(81, 21);
            this.cbOctave.TabIndex = 6;
            // 
            // laDuration
            // 
            this.laDuration.AutoSize = true;
            this.laDuration.Location = new System.Drawing.Point(271, 100);
            this.laDuration.Name = "laDuration";
            this.laDuration.Size = new System.Drawing.Size(47, 13);
            this.laDuration.TabIndex = 7;
            this.laDuration.Text = "Duration";
            // 
            // edDuration
            // 
            this.edDuration.Enabled = false;
            this.edDuration.Location = new System.Drawing.Point(324, 97);
            this.edDuration.Name = "edDuration";
            this.edDuration.Size = new System.Drawing.Size(63, 20);
            this.edDuration.TabIndex = 8;
            this.edDuration.Text = "500";
            // 
            // btPlay
            // 
            this.btPlay.Enabled = false;
            this.btPlay.Location = new System.Drawing.Point(12, 124);
            this.btPlay.Name = "btPlay";
            this.btPlay.Size = new System.Drawing.Size(75, 23);
            this.btPlay.TabIndex = 9;
            this.btPlay.Text = "Play";
            this.btPlay.UseVisualStyleBackColor = true;
            this.btPlay.Click += new System.EventHandler(this.BtPlay_Click);
            // 
            // btStop
            // 
            this.btStop.Enabled = false;
            this.btStop.Location = new System.Drawing.Point(93, 124);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(75, 23);
            this.btStop.TabIndex = 10;
            this.btStop.Text = "Stop";
            this.btStop.UseVisualStyleBackColor = true;
            this.btStop.Click += new System.EventHandler(this.BtStop_Click);
            // 
            // laIoState
            // 
            this.laIoState.AutoSize = true;
            this.laIoState.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laIoState.Location = new System.Drawing.Point(181, 53);
            this.laIoState.Name = "laIoState";
            this.laIoState.Size = new System.Drawing.Size(62, 13);
            this.laIoState.TabIndex = 11;
            this.laIoState.Text = "Detached";
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 158);
            this.Controls.Add(this.laIoState);
            this.Controls.Add(this.btStop);
            this.Controls.Add(this.btPlay);
            this.Controls.Add(this.edDuration);
            this.Controls.Add(this.laDuration);
            this.Controls.Add(this.cbOctave);
            this.Controls.Add(this.laOctave);
            this.Controls.Add(this.cbNote);
            this.Controls.Add(this.laNote);
            this.Controls.Add(this.laStatus);
            this.Controls.Add(this.btDisconnect);
            this.Controls.Add(this.btConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "fmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WeDo Piezo Test Application";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FmMain_FormClosed);
            this.Load += new System.EventHandler(this.FmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btConnect;
        private System.Windows.Forms.Button btDisconnect;
        private System.Windows.Forms.Label laStatus;
        private System.Windows.Forms.Label laNote;
        private System.Windows.Forms.ComboBox cbNote;
        private System.Windows.Forms.Label laOctave;
        private System.Windows.Forms.ComboBox cbOctave;
        private System.Windows.Forms.Label laDuration;
        private System.Windows.Forms.TextBox edDuration;
        private System.Windows.Forms.Button btPlay;
        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.Label laIoState;
    }
}

