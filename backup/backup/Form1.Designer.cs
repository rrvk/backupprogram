namespace backup
{
    partial class Form1
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
            this.lstbackup = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.asdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblProgress = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // lstbackup
            // 
            this.lstbackup.FormattingEnabled = true;
            this.lstbackup.Location = new System.Drawing.Point(12, 26);
            this.lstbackup.Name = "lstbackup";
            this.lstbackup.Size = new System.Drawing.Size(157, 121);
            this.lstbackup.TabIndex = 0;
            this.lstbackup.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstbackup_KeyDown);
            this.lstbackup.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstbackup_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Wat te backupen";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(175, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Backup locatie";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoEllipsis = true;
            this.lblLocation.Location = new System.Drawing.Point(175, 26);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(154, 15);
            this.lblLocation.TabIndex = 3;
            this.lblLocation.Click += new System.EventHandler(this.lblLocation_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(178, 118);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(84, 23);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Start backup";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // asdToolStripMenuItem
            // 
            this.asdToolStripMenuItem.Name = "asdToolStripMenuItem";
            this.asdToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // lblProgress
            // 
            this.lblProgress.AutoEllipsis = true;
            this.lblProgress.Location = new System.Drawing.Point(175, 59);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(154, 56);
            this.lblProgress.TabIndex = 5;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(178, 44);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(151, 12);
            this.progressBar1.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 152);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstbackup);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstbackup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ToolStripMenuItem asdToolStripMenuItem;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

