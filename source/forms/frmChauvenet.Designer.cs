namespace Tripoli
{
    partial class frmTritonCyclePerBlock
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
            this.btnViewChauvenet = new System.Windows.Forms.Button();
            this.lblDefaultChauvenet = new System.Windows.Forms.Label();
            this.lblCurrentChauvenet = new System.Windows.Forms.Label();
            this.tbDefaultChauvenet = new System.Windows.Forms.TextBox();
            this.mtbCurrentChauvenet = new System.Windows.Forms.MaskedTextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnUseDefault = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnViewChauvenet
            // 
            this.btnViewChauvenet.AutoSize = true;
            this.btnViewChauvenet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnViewChauvenet.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewChauvenet.Location = new System.Drawing.Point(23, 12);
            this.btnViewChauvenet.Name = "btnViewChauvenet";
            this.btnViewChauvenet.Size = new System.Drawing.Size(270, 26);
            this.btnViewChauvenet.TabIndex = 0;
            this.btnViewChauvenet.Text = "View explanation of Chauvenet\'s Criterion";
            this.btnViewChauvenet.UseVisualStyleBackColor = false;
            this.btnViewChauvenet.Click += new System.EventHandler(this.btnViewChauvenet_Click);
            // 
            // lblDefaultChauvenet
            // 
            this.lblDefaultChauvenet.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefaultChauvenet.Location = new System.Drawing.Point(12, 62);
            this.lblDefaultChauvenet.Name = "lblDefaultChauvenet";
            this.lblDefaultChauvenet.Size = new System.Drawing.Size(169, 32);
            this.lblDefaultChauvenet.TabIndex = 1;
            this.lblDefaultChauvenet.Text = "The default threshold value for Chauvenet\'s Criterion is :";
            // 
            // lblCurrentChauvenet
            // 
            this.lblCurrentChauvenet.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentChauvenet.Location = new System.Drawing.Point(12, 134);
            this.lblCurrentChauvenet.Name = "lblCurrentChauvenet";
            this.lblCurrentChauvenet.Size = new System.Drawing.Size(203, 39);
            this.lblCurrentChauvenet.TabIndex = 2;
            this.lblCurrentChauvenet.Text = "Specify the threshold value [0,1] for this Tripoli WorkFile:";
            // 
            // tbDefaultChauvenet
            // 
            this.tbDefaultChauvenet.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tbDefaultChauvenet.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Tripoli.Properties.Settings.Default, "ChauvenetCriterion", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbDefaultChauvenet.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDefaultChauvenet.Location = new System.Drawing.Point(220, 67);
            this.tbDefaultChauvenet.Name = "tbDefaultChauvenet";
            this.tbDefaultChauvenet.Size = new System.Drawing.Size(75, 22);
            this.tbDefaultChauvenet.TabIndex = 3;
            this.tbDefaultChauvenet.Text = "0.5";
            this.tbDefaultChauvenet.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // mtbCurrentChauvenet
            // 
            this.mtbCurrentChauvenet.AllowPromptAsInput = false;
            this.mtbCurrentChauvenet.AsciiOnly = true;
            this.mtbCurrentChauvenet.BeepOnError = true;
            this.mtbCurrentChauvenet.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mtbCurrentChauvenet.HidePromptOnLeave = true;
            this.mtbCurrentChauvenet.Location = new System.Drawing.Point(220, 140);
            this.mtbCurrentChauvenet.Mask = "0.##";
            this.mtbCurrentChauvenet.Name = "mtbCurrentChauvenet";
            this.mtbCurrentChauvenet.Size = new System.Drawing.Size(75, 22);
            this.mtbCurrentChauvenet.TabIndex = 5;
            this.mtbCurrentChauvenet.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(169, 233);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(126, 23);
            this.btnCancel.TabIndex = 41;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnOK.Location = new System.Drawing.Point(169, 207);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(126, 23);
            this.btnOK.TabIndex = 40;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnUseDefault
            // 
            this.btnUseDefault.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnUseDefault.Location = new System.Drawing.Point(220, 168);
            this.btnUseDefault.Name = "btnUseDefault";
            this.btnUseDefault.Size = new System.Drawing.Size(75, 23);
            this.btnUseDefault.TabIndex = 49;
            this.btnUseDefault.Text = "Use Default";
            this.btnUseDefault.UseVisualStyleBackColor = false;
            this.btnUseDefault.Click += new System.EventHandler(this.btnUseDefault_Click);
            // 
            // frmChauvenet
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LemonChiffon;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(319, 268);
            this.ControlBox = false;
            this.Controls.Add(this.btnUseDefault);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.mtbCurrentChauvenet);
            this.Controls.Add(this.tbDefaultChauvenet);
            this.Controls.Add(this.lblCurrentChauvenet);
            this.Controls.Add(this.lblDefaultChauvenet);
            this.Controls.Add(this.btnViewChauvenet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmChauvenet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chauvenet\'s Criterion Threshold";
            this.Load += new System.EventHandler(this.frmChauvenet_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmChauvenet_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnViewChauvenet;
        private System.Windows.Forms.Label lblDefaultChauvenet;
        private System.Windows.Forms.Label lblCurrentChauvenet;
        private System.Windows.Forms.TextBox tbDefaultChauvenet;
        private System.Windows.Forms.MaskedTextBox mtbCurrentChauvenet;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnUseDefault;



    }
}