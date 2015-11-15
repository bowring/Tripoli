namespace Tripoli.source.forms
{
    partial class frmWorkFlowFolder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWorkFlowFolder));
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.liveWorkflowDataFolderName_label = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.changeLiveWorkflowDataFolder_button = new System.Windows.Forms.Button();
            this.step1Text_label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.introLbl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.IDTIMS_DataFileTypes_panel = new System.Windows.Forms.Panel();
            this.radioBut_IonVantage = new System.Windows.Forms.RadioButton();
            this.lblIDTIMS = new System.Windows.Forms.Label();
            this.radioBut_ThermoFinniganMat261 = new System.Windows.Forms.RadioButton();
            this.radioBut_ThermoFinniganTriton = new System.Windows.Forms.RadioButton();
            this.radioBut_Sector54 = new System.Windows.Forms.RadioButton();
            this.radioBut_IsotopX = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSampleMetaDataFolder_label = new System.Windows.Forms.Label();
            this.changeSampleMetaDataFolder_button = new System.Windows.Forms.Button();
            this.ignoreSampleMetaData_chkbox = new System.Windows.Forms.CheckBox();
            this.IDTIMS_DataFileTypes_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // liveWorkflowDataFolderName_label
            // 
            this.liveWorkflowDataFolderName_label.BackColor = System.Drawing.Color.AntiqueWhite;
            this.liveWorkflowDataFolderName_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.liveWorkflowDataFolderName_label.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.liveWorkflowDataFolderName_label.Location = new System.Drawing.Point(74, 296);
            this.liveWorkflowDataFolderName_label.Name = "liveWorkflowDataFolderName_label";
            this.liveWorkflowDataFolderName_label.Size = new System.Drawing.Size(585, 30);
            this.liveWorkflowDataFolderName_label.TabIndex = 5;
            this.liveWorkflowDataFolderName_label.Tag = "No Live Workflow Folder Chosen";
            this.liveWorkflowDataFolderName_label.Text = "Live Workflow Data Folder Name";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // changeLiveWorkflowDataFolder_button
            // 
            this.changeLiveWorkflowDataFolder_button.BackColor = System.Drawing.Color.LightCyan;
            this.changeLiveWorkflowDataFolder_button.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.changeLiveWorkflowDataFolder_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changeLiveWorkflowDataFolder_button.Location = new System.Drawing.Point(8, 301);
            this.changeLiveWorkflowDataFolder_button.Name = "changeLiveWorkflowDataFolder_button";
            this.changeLiveWorkflowDataFolder_button.Size = new System.Drawing.Size(58, 21);
            this.changeLiveWorkflowDataFolder_button.TabIndex = 13;
            this.changeLiveWorkflowDataFolder_button.Text = "Set";
            this.changeLiveWorkflowDataFolder_button.UseVisualStyleBackColor = false;
            this.changeLiveWorkflowDataFolder_button.Click += new System.EventHandler(this.changeLiveWorkflowDataFolder_button_Click);
            // 
            // step1Text_label
            // 
            this.step1Text_label.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.step1Text_label.Location = new System.Drawing.Point(8, 278);
            this.step1Text_label.Name = "step1Text_label";
            this.step1Text_label.Size = new System.Drawing.Size(646, 15);
            this.step1Text_label.TabIndex = 14;
            this.step1Text_label.Tag = "";
            this.step1Text_label.Text = "Set  the location of your Live Workflow data folder (contains data files of the t" +
                "ype specified above):";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(72, 328);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(588, 21);
            this.label1.TabIndex = 19;
            this.label1.Tag = "";
            this.label1.Text = "With Live Workflow started,Tripoli automatically saves partial Tripoli WorkFiles " +
                "(\'.trip\') to this data folder.";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.LightCyan;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(8, 452);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(657, 24);
            this.btnClose.TabIndex = 20;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // introLbl
            // 
            this.introLbl.BackColor = System.Drawing.Color.Cornsilk;
            this.introLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.introLbl.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.introLbl.Location = new System.Drawing.Point(8, 8);
            this.introLbl.Name = "introLbl";
            this.introLbl.Size = new System.Drawing.Size(650, 194);
            this.introLbl.TabIndex = 21;
            this.introLbl.Tag = "";
            this.introLbl.Text = resources.GetString("introLbl.Text");
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 208);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(391, 21);
            this.label2.TabIndex = 23;
            this.label2.Tag = "";
            this.label2.Text = "Choose the type of data files you will use for Live Workflow:";
            // 
            // IDTIMS_DataFileTypes_panel
            // 
            this.IDTIMS_DataFileTypes_panel.BackColor = System.Drawing.Color.AntiqueWhite;
            this.IDTIMS_DataFileTypes_panel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.IDTIMS_DataFileTypes_panel.Controls.Add(this.radioBut_IonVantage);
            this.IDTIMS_DataFileTypes_panel.Controls.Add(this.lblIDTIMS);
            this.IDTIMS_DataFileTypes_panel.Controls.Add(this.radioBut_ThermoFinniganMat261);
            this.IDTIMS_DataFileTypes_panel.Controls.Add(this.radioBut_ThermoFinniganTriton);
            this.IDTIMS_DataFileTypes_panel.Controls.Add(this.radioBut_Sector54);
            this.IDTIMS_DataFileTypes_panel.Controls.Add(this.radioBut_IsotopX);
            this.IDTIMS_DataFileTypes_panel.Location = new System.Drawing.Point(24, 227);
            this.IDTIMS_DataFileTypes_panel.Name = "IDTIMS_DataFileTypes_panel";
            this.IDTIMS_DataFileTypes_panel.Size = new System.Drawing.Size(612, 48);
            this.IDTIMS_DataFileTypes_panel.TabIndex = 24;
            // 
            // radioBut_IonVantage
            // 
            this.radioBut_IonVantage.AutoSize = true;
            this.radioBut_IonVantage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioBut_IonVantage.Location = new System.Drawing.Point(66, 25);
            this.radioBut_IonVantage.Name = "radioBut_IonVantage";
            this.radioBut_IonVantage.Size = new System.Drawing.Size(260, 17);
            this.radioBut_IonVantage.TabIndex = 5;
            this.radioBut_IonVantage.Text = "IonVantage LiveData \'LiveDataStatus.txt\'";
            this.radioBut_IonVantage.UseVisualStyleBackColor = true;
            this.radioBut_IonVantage.Click += new System.EventHandler(this.radioBut_IonVantage_Click);
            // 
            // lblIDTIMS
            // 
            this.lblIDTIMS.AutoSize = true;
            this.lblIDTIMS.ForeColor = System.Drawing.Color.Red;
            this.lblIDTIMS.Location = new System.Drawing.Point(3, 17);
            this.lblIDTIMS.Name = "lblIDTIMS";
            this.lblIDTIMS.Size = new System.Drawing.Size(50, 13);
            this.lblIDTIMS.TabIndex = 4;
            this.lblIDTIMS.Text = "ID-TIMS:";
            // 
            // radioBut_ThermoFinniganMat261
            // 
            this.radioBut_ThermoFinniganMat261.AutoSize = true;
            this.radioBut_ThermoFinniganMat261.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioBut_ThermoFinniganMat261.Location = new System.Drawing.Point(454, 4);
            this.radioBut_ThermoFinniganMat261.Name = "radioBut_ThermoFinniganMat261";
            this.radioBut_ThermoFinniganMat261.Size = new System.Drawing.Size(148, 17);
            this.radioBut_ThermoFinniganMat261.TabIndex = 3;
            this.radioBut_ThermoFinniganMat261.Text = "Thermo-F Mat261 \'txt\'";
            this.radioBut_ThermoFinniganMat261.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.radioBut_ThermoFinniganMat261.UseVisualStyleBackColor = true;
            this.radioBut_ThermoFinniganMat261.Click += new System.EventHandler(this.radioBut_ThermoFinniganMat261_Click);
            // 
            // radioBut_ThermoFinniganTriton
            // 
            this.radioBut_ThermoFinniganTriton.AutoSize = true;
            this.radioBut_ThermoFinniganTriton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioBut_ThermoFinniganTriton.Location = new System.Drawing.Point(290, 4);
            this.radioBut_ThermoFinniganTriton.Name = "radioBut_ThermoFinniganTriton";
            this.radioBut_ThermoFinniganTriton.Size = new System.Drawing.Size(149, 17);
            this.radioBut_ThermoFinniganTriton.TabIndex = 2;
            this.radioBut_ThermoFinniganTriton.Text = "Thermo-F Triton \'.exp\'";
            this.radioBut_ThermoFinniganTriton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.radioBut_ThermoFinniganTriton.UseVisualStyleBackColor = true;
            this.radioBut_ThermoFinniganTriton.Click += new System.EventHandler(this.radioBut_ThermoFinniganTriton_Click);
            // 
            // radioBut_Sector54
            // 
            this.radioBut_Sector54.AutoSize = true;
            this.radioBut_Sector54.Checked = true;
            this.radioBut_Sector54.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioBut_Sector54.Location = new System.Drawing.Point(168, 4);
            this.radioBut_Sector54.Name = "radioBut_Sector54";
            this.radioBut_Sector54.Size = new System.Drawing.Size(112, 17);
            this.radioBut_Sector54.TabIndex = 1;
            this.radioBut_Sector54.TabStop = true;
            this.radioBut_Sector54.Text = "Sector 54 \'.dat\'";
            this.radioBut_Sector54.UseVisualStyleBackColor = true;
            this.radioBut_Sector54.Click += new System.EventHandler(this.radioBut_Sector54_Click);
            // 
            // radioBut_IsotopX
            // 
            this.radioBut_IsotopX.AutoSize = true;
            this.radioBut_IsotopX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioBut_IsotopX.Location = new System.Drawing.Point(66, 4);
            this.radioBut_IsotopX.Name = "radioBut_IsotopX";
            this.radioBut_IsotopX.Size = new System.Drawing.Size(97, 17);
            this.radioBut_IsotopX.TabIndex = 0;
            this.radioBut_IsotopX.Text = "IsotopX \'.xls\'";
            this.radioBut_IsotopX.UseVisualStyleBackColor = true;
            this.radioBut_IsotopX.Click += new System.EventHandler(this.radioBut_IsotopX_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 361);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(646, 15);
            this.label3.TabIndex = 25;
            this.label3.Tag = "";
            this.label3.Text = "Set  the location of the SampleMetaData folder you created with U-Pb_Redux:";
            // 
            // lblSampleMetaDataFolder_label
            // 
            this.lblSampleMetaDataFolder_label.BackColor = System.Drawing.Color.AntiqueWhite;
            this.lblSampleMetaDataFolder_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSampleMetaDataFolder_label.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSampleMetaDataFolder_label.Location = new System.Drawing.Point(73, 379);
            this.lblSampleMetaDataFolder_label.Name = "lblSampleMetaDataFolder_label";
            this.lblSampleMetaDataFolder_label.Size = new System.Drawing.Size(585, 30);
            this.lblSampleMetaDataFolder_label.TabIndex = 26;
            this.lblSampleMetaDataFolder_label.Tag = "No SampleMetaData Folder specified - use the \'SampleMetaData\' folder created by U" +
                "-Pb_Redux.";
            this.lblSampleMetaDataFolder_label.Text = "SampleMetaData Folder";
            // 
            // changeSampleMetaDataFolder_button
            // 
            this.changeSampleMetaDataFolder_button.BackColor = System.Drawing.Color.LightCyan;
            this.changeSampleMetaDataFolder_button.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.changeSampleMetaDataFolder_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changeSampleMetaDataFolder_button.Location = new System.Drawing.Point(8, 383);
            this.changeSampleMetaDataFolder_button.Name = "changeSampleMetaDataFolder_button";
            this.changeSampleMetaDataFolder_button.Size = new System.Drawing.Size(58, 21);
            this.changeSampleMetaDataFolder_button.TabIndex = 27;
            this.changeSampleMetaDataFolder_button.Text = "Set";
            this.changeSampleMetaDataFolder_button.UseVisualStyleBackColor = false;
            this.changeSampleMetaDataFolder_button.Click += new System.EventHandler(this.changeSampleMetaDataFolder_button_Click);
            // 
            // ignoreSampleMetaData_chkbox
            // 
            this.ignoreSampleMetaData_chkbox.AutoSize = true;
            this.ignoreSampleMetaData_chkbox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ignoreSampleMetaData_chkbox.Location = new System.Drawing.Point(76, 412);
            this.ignoreSampleMetaData_chkbox.Name = "ignoreSampleMetaData_chkbox";
            this.ignoreSampleMetaData_chkbox.Size = new System.Drawing.Size(291, 19);
            this.ignoreSampleMetaData_chkbox.TabIndex = 28;
            this.ignoreSampleMetaData_chkbox.Text = "Simple Live Workflow: ignore SampleMetaData";
            this.ignoreSampleMetaData_chkbox.UseVisualStyleBackColor = true;
            this.ignoreSampleMetaData_chkbox.Click += new System.EventHandler(this.ignoreSampleMetaData_chkbox_Click);
            // 
            // frmWorkFlowFolder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(670, 486);
            this.ControlBox = false;
            this.Controls.Add(this.ignoreSampleMetaData_chkbox);
            this.Controls.Add(this.changeSampleMetaDataFolder_button);
            this.Controls.Add(this.lblSampleMetaDataFolder_label);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.IDTIMS_DataFileTypes_panel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.introLbl);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.step1Text_label);
            this.Controls.Add(this.changeLiveWorkflowDataFolder_button);
            this.Controls.Add(this.liveWorkflowDataFolderName_label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmWorkFlowFolder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Specify Live Workflow Folders";
            this.IDTIMS_DataFileTypes_panel.ResumeLayout(false);
            this.IDTIMS_DataFileTypes_panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label liveWorkflowDataFolderName_label;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button changeLiveWorkflowDataFolder_button;
        private System.Windows.Forms.Label step1Text_label;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label introLbl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel IDTIMS_DataFileTypes_panel;
        private System.Windows.Forms.RadioButton radioBut_ThermoFinniganTriton;
        private System.Windows.Forms.RadioButton radioBut_Sector54;
        private System.Windows.Forms.RadioButton radioBut_IsotopX;
        private System.Windows.Forms.RadioButton radioBut_ThermoFinniganMat261;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblIDTIMS;
        private System.Windows.Forms.Label lblSampleMetaDataFolder_label;
        private System.Windows.Forms.Button changeSampleMetaDataFolder_button;
        private System.Windows.Forms.CheckBox ignoreSampleMetaData_chkbox;
        private System.Windows.Forms.RadioButton radioBut_IonVantage;
    }
}