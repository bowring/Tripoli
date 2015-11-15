namespace Tripoli
{
    partial class frmTripoliControlPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTripoliControlPanel));
            this.saveTripXML_button = new System.Windows.Forms.Button();
            this.loadNewestDataFile_button = new System.Windows.Forms.Button();
            this.startStopLiveWorkflow_button = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.newestDataFileStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.setDataFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportRatiosToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveTripXML_button
            // 
            this.saveTripXML_button.BackColor = System.Drawing.Color.LightCyan;
            this.saveTripXML_button.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.saveTripXML_button.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveTripXML_button.Location = new System.Drawing.Point(250, 26);
            this.saveTripXML_button.Name = "saveTripXML_button";
            this.saveTripXML_button.Size = new System.Drawing.Size(112, 21);
            this.saveTripXML_button.TabIndex = 14;
            this.saveTripXML_button.Tag = "Save / Export";
            this.saveTripXML_button.Text = "Save / Export";
            this.saveTripXML_button.UseVisualStyleBackColor = false;
            this.saveTripXML_button.Click += new System.EventHandler(this.saveTripXML_button_Click);
            // 
            // loadNewestDataFile_button
            // 
            this.loadNewestDataFile_button.BackColor = System.Drawing.Color.LightCyan;
            this.loadNewestDataFile_button.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.loadNewestDataFile_button.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadNewestDataFile_button.Location = new System.Drawing.Point(130, 26);
            this.loadNewestDataFile_button.Name = "loadNewestDataFile_button";
            this.loadNewestDataFile_button.Size = new System.Drawing.Size(119, 21);
            this.loadNewestDataFile_button.TabIndex = 13;
            this.loadNewestDataFile_button.Text = "Load Newest data";
            this.loadNewestDataFile_button.UseVisualStyleBackColor = false;
            this.loadNewestDataFile_button.Click += new System.EventHandler(this.getCurrentData_button_Click);
            // 
            // startStopLiveWorkflow_button
            // 
            this.startStopLiveWorkflow_button.BackColor = System.Drawing.Color.LightCyan;
            this.startStopLiveWorkflow_button.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.startStopLiveWorkflow_button.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startStopLiveWorkflow_button.Location = new System.Drawing.Point(1, 26);
            this.startStopLiveWorkflow_button.Name = "startStopLiveWorkflow_button";
            this.startStopLiveWorkflow_button.Size = new System.Drawing.Size(129, 21);
            this.startStopLiveWorkflow_button.TabIndex = 11;
            this.startStopLiveWorkflow_button.Text = "Start Live Workflow";
            this.startStopLiveWorkflow_button.UseVisualStyleBackColor = false;
            this.startStopLiveWorkflow_button.Click += new System.EventHandler(this.startStopLiveWorkflow_button_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.Bisque;
            this.statusStrip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.newestDataFileStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 46);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(366, 23);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(44, 18);
            this.toolStripStatusLabel1.Text = "Status:";
            // 
            // newestDataFileStatusLabel
            // 
            this.newestDataFileStatusLabel.BackColor = System.Drawing.Color.White;
            this.newestDataFileStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.newestDataFileStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.newestDataFileStatusLabel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newestDataFileStatusLabel.Name = "newestDataFileStatusLabel";
            this.newestDataFileStatusLabel.Size = new System.Drawing.Size(307, 18);
            this.newestDataFileStatusLabel.Spring = true;
            this.newestDataFileStatusLabel.Text = "current status";
            this.newestDataFileStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.newestDataFileStatusLabel.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 1000;
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 200;
            this.toolTip1.ShowAlways = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Bisque;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setDataFolderToolStripMenuItem,
            this.exportRatiosToClipboardToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(366, 24);
            this.menuStrip1.TabIndex = 16;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // setDataFolderToolStripMenuItem
            // 
            this.setDataFolderToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.setDataFolderToolStripMenuItem.Name = "setDataFolderToolStripMenuItem";
            this.setDataFolderToolStripMenuItem.Size = new System.Drawing.Size(166, 20);
            this.setDataFolderToolStripMenuItem.Text = "Set Live Workflow Folders";
            this.setDataFolderToolStripMenuItem.Click += new System.EventHandler(this.setDataFolderToolStripMenuItem_Click);
            // 
            // exportRatiosToClipboardToolStripMenuItem
            // 
            this.exportRatiosToClipboardToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportRatiosToClipboardToolStripMenuItem.Name = "exportRatiosToClipboardToolStripMenuItem";
            this.exportRatiosToClipboardToolStripMenuItem.Size = new System.Drawing.Size(126, 20);
            this.exportRatiosToClipboardToolStripMenuItem.Text = "Copy for PbMacDat";
            this.exportRatiosToClipboardToolStripMenuItem.Click += new System.EventHandler(this.exportRatiosToClipboardToolStripMenuItem_Click);
            // 
            // frmTripoliControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Bisque;
            this.ClientSize = new System.Drawing.Size(366, 69);
            this.Controls.Add(this.saveTripXML_button);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.loadNewestDataFile_button);
            this.Controls.Add(this.startStopLiveWorkflow_button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "frmTripoliControlPanel";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tripoli Workflow Control Panel";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTripoliControlPanel_FormClosing);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveTripXML_button;
        private System.Windows.Forms.Button loadNewestDataFile_button;
        private System.Windows.Forms.Button startStopLiveWorkflow_button;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel newestDataFileStatusLabel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem setDataFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportRatiosToClipboardToolStripMenuItem;
    }
}