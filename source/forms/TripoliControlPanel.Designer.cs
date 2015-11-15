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
            this.btnExportToPbMacDat = new System.Windows.Forms.Button();
            this.btnReportPbMacdatExport = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.sector54_tab = new System.Windows.Forms.TabPage();
            this.saveTripXML_button = new System.Windows.Forms.Button();
            this.loadNewestDataFile_button = new System.Windows.Forms.Button();
            this.startStopLiveWorkflow_button = new System.Windows.Forms.Button();
            this.exportForPbMacDat_tab = new System.Windows.Forms.TabPage();
            this.exportAsText_tab = new System.Windows.Forms.TabPage();
            this.exportAsDelimitedText_button = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.newestDataFileStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.sector54_tab.SuspendLayout();
            this.exportForPbMacDat_tab.SuspendLayout();
            this.exportAsText_tab.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExportToPbMacDat
            // 
            this.btnExportToPbMacDat.BackColor = System.Drawing.Color.LightCyan;
            this.btnExportToPbMacDat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportToPbMacDat.Location = new System.Drawing.Point(-1, -1);
            this.btnExportToPbMacDat.Name = "btnExportToPbMacDat";
            this.btnExportToPbMacDat.Size = new System.Drawing.Size(222, 23);
            this.btnExportToPbMacDat.TabIndex = 0;
            this.btnExportToPbMacDat.Text = "Export Checked Ratios to Clipboard";
            this.btnExportToPbMacDat.UseVisualStyleBackColor = false;
            this.btnExportToPbMacDat.Click += new System.EventHandler(this.btnExportToPbMacDat_Click);
            // 
            // btnReportPbMacdatExport
            // 
            this.btnReportPbMacdatExport.BackColor = System.Drawing.Color.LightCyan;
            this.btnReportPbMacdatExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReportPbMacdatExport.Location = new System.Drawing.Point(224, -1);
            this.btnReportPbMacdatExport.Name = "btnReportPbMacdatExport";
            this.btnReportPbMacdatExport.Size = new System.Drawing.Size(58, 23);
            this.btnReportPbMacdatExport.TabIndex = 1;
            this.btnReportPbMacdatExport.Text = "Report";
            this.btnReportPbMacdatExport.UseVisualStyleBackColor = false;
            this.btnReportPbMacdatExport.Click += new System.EventHandler(this.btnReportPbMacdatExport_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControl1.Controls.Add(this.sector54_tab);
            this.tabControl1.Controls.Add(this.exportForPbMacDat_tab);
            this.tabControl1.Controls.Add(this.exportAsText_tab);
            this.tabControl1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(448, 52);
            this.tabControl1.TabIndex = 4;
            // 
            // sector54_tab
            // 
            this.sector54_tab.Controls.Add(this.saveTripXML_button);
            this.sector54_tab.Controls.Add(this.loadNewestDataFile_button);
            this.sector54_tab.Controls.Add(this.startStopLiveWorkflow_button);
            this.sector54_tab.Location = new System.Drawing.Point(4, 27);
            this.sector54_tab.Name = "sector54_tab";
            this.sector54_tab.Padding = new System.Windows.Forms.Padding(3);
            this.sector54_tab.Size = new System.Drawing.Size(440, 21);
            this.sector54_tab.TabIndex = 0;
            this.sector54_tab.Text = "Sector 54 .dat";
            this.sector54_tab.UseVisualStyleBackColor = true;
            // 
            // saveTripXML_button
            // 
            this.saveTripXML_button.BackColor = System.Drawing.Color.LightCyan;
            this.saveTripXML_button.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.saveTripXML_button.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveTripXML_button.Location = new System.Drawing.Point(334, 0);
            this.saveTripXML_button.Name = "saveTripXML_button";
            this.saveTripXML_button.Size = new System.Drawing.Size(104, 21);
            this.saveTripXML_button.TabIndex = 14;
            this.saveTripXML_button.Text = "Save trip + xml";
            this.saveTripXML_button.UseVisualStyleBackColor = false;
            this.saveTripXML_button.Click += new System.EventHandler(this.saveTripXML_button_Click);
            // 
            // loadNewestDataFile_button
            // 
            this.loadNewestDataFile_button.BackColor = System.Drawing.Color.LightCyan;
            this.loadNewestDataFile_button.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.loadNewestDataFile_button.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadNewestDataFile_button.Location = new System.Drawing.Point(215, 0);
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
            this.startStopLiveWorkflow_button.Location = new System.Drawing.Point(87, 0);
            this.startStopLiveWorkflow_button.Name = "startStopLiveWorkflow_button";
            this.startStopLiveWorkflow_button.Size = new System.Drawing.Size(129, 21);
            this.startStopLiveWorkflow_button.TabIndex = 11;
            this.startStopLiveWorkflow_button.Text = "Start Live Workflow";
            this.startStopLiveWorkflow_button.UseVisualStyleBackColor = false;
            this.startStopLiveWorkflow_button.Click += new System.EventHandler(this.startStopLiveWorkflow_button_Click);
            // 
            // exportForPbMacDat_tab
            // 
            this.exportForPbMacDat_tab.Controls.Add(this.btnExportToPbMacDat);
            this.exportForPbMacDat_tab.Controls.Add(this.btnReportPbMacdatExport);
            this.exportForPbMacDat_tab.Location = new System.Drawing.Point(4, 27);
            this.exportForPbMacDat_tab.Name = "exportForPbMacDat_tab";
            this.exportForPbMacDat_tab.Padding = new System.Windows.Forms.Padding(3);
            this.exportForPbMacDat_tab.Size = new System.Drawing.Size(346, 21);
            this.exportForPbMacDat_tab.TabIndex = 1;
            this.exportForPbMacDat_tab.Text = "PbMacDat";
            this.exportForPbMacDat_tab.UseVisualStyleBackColor = true;
            // 
            // exportAsText_tab
            // 
            this.exportAsText_tab.Controls.Add(this.exportAsDelimitedText_button);
            this.exportAsText_tab.Location = new System.Drawing.Point(4, 27);
            this.exportAsText_tab.Name = "exportAsText_tab";
            this.exportAsText_tab.Size = new System.Drawing.Size(346, 21);
            this.exportAsText_tab.TabIndex = 2;
            this.exportAsText_tab.Text = "Export as text";
            this.exportAsText_tab.UseVisualStyleBackColor = true;
            // 
            // exportAsDelimitedText_button
            // 
            this.exportAsDelimitedText_button.BackColor = System.Drawing.Color.LightCyan;
            this.exportAsDelimitedText_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportAsDelimitedText_button.Location = new System.Drawing.Point(3, -1);
            this.exportAsDelimitedText_button.Name = "exportAsDelimitedText_button";
            this.exportAsDelimitedText_button.Size = new System.Drawing.Size(269, 23);
            this.exportAsDelimitedText_button.TabIndex = 1;
            this.exportAsDelimitedText_button.Text = "Export checked ratios as delimited text";
            this.exportAsDelimitedText_button.UseVisualStyleBackColor = false;
            this.exportAsDelimitedText_button.Click += new System.EventHandler(this.exportAsDelimitedText_button_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.White;
            this.statusStrip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.newestDataFileStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 50);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(451, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(44, 17);
            this.toolStripStatusLabel1.Text = "Status:";
            // 
            // newestDataFileStatusLabel
            // 
            this.newestDataFileStatusLabel.Name = "newestDataFileStatusLabel";
            this.newestDataFileStatusLabel.Size = new System.Drawing.Size(392, 17);
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
            // frmTripoliControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(451, 72);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmTripoliControlPanel";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tripoli Control Panel";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTripoliControlPanel_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.sector54_tab.ResumeLayout(false);
            this.exportForPbMacDat_tab.ResumeLayout(false);
            this.exportAsText_tab.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExportToPbMacDat;
        private System.Windows.Forms.Button btnReportPbMacdatExport;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage sector54_tab;
        private System.Windows.Forms.TabPage exportForPbMacDat_tab;
        private System.Windows.Forms.Button saveTripXML_button;
        private System.Windows.Forms.Button loadNewestDataFile_button;
        private System.Windows.Forms.Button startStopLiveWorkflow_button;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.TabPage exportAsText_tab;
        private System.Windows.Forms.Button exportAsDelimitedText_button;
        private System.Windows.Forms.ToolStripStatusLabel newestDataFileStatusLabel;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}