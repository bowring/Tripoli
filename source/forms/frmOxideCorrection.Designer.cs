namespace Tripoli
{
    partial class frmOxideCorrection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOxideCorrection));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblDefaultr18O_16O = new System.Windows.Forms.Label();
            this.lblr18O_16O = new System.Windows.Forms.Label();
            this.tbr18O_16O = new System.Windows.Forms.TextBox();
            this.lblOxideExplain = new System.Windows.Forms.Label();
            this.tbr18O_16Odefault = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(219, 269);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(126, 23);
            this.btnCancel.TabIndex = 43;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnOK.Location = new System.Drawing.Point(219, 243);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(126, 23);
            this.btnOK.TabIndex = 42;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblDefaultr18O_16O
            // 
            this.lblDefaultr18O_16O.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefaultr18O_16O.Location = new System.Drawing.Point(12, 12);
            this.lblDefaultr18O_16O.Name = "lblDefaultr18O_16O";
            this.lblDefaultr18O_16O.Size = new System.Drawing.Size(201, 22);
            this.lblDefaultr18O_16O.TabIndex = 44;
            this.lblDefaultr18O_16O.Text = "The default value of 18O / 16O is:";
            this.lblDefaultr18O_16O.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblr18O_16O
            // 
            this.lblr18O_16O.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblr18O_16O.Location = new System.Drawing.Point(12, 45);
            this.lblr18O_16O.Name = "lblr18O_16O";
            this.lblr18O_16O.Size = new System.Drawing.Size(201, 68);
            this.lblr18O_16O.TabIndex = 45;
            this.lblr18O_16O.Text = "Value of 18O / 16O that was applied automatically when raw data was imported into" +
                " this Tripoli WorkFile:";
            this.lblr18O_16O.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbr18O_16O
            // 
            this.tbr18O_16O.BackColor = System.Drawing.Color.White;
            this.tbr18O_16O.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbr18O_16O.Location = new System.Drawing.Point(239, 91);
            this.tbr18O_16O.Name = "tbr18O_16O";
            this.tbr18O_16O.ReadOnly = true;
            this.tbr18O_16O.Size = new System.Drawing.Size(87, 22);
            this.tbr18O_16O.TabIndex = 47;
            this.tbr18O_16O.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblOxideExplain
            // 
            this.lblOxideExplain.ForeColor = System.Drawing.Color.Red;
            this.lblOxideExplain.Location = new System.Drawing.Point(11, 132);
            this.lblOxideExplain.Name = "lblOxideExplain";
            this.lblOxideExplain.Size = new System.Drawing.Size(313, 108);
            this.lblOxideExplain.TabIndex = 49;
            this.lblOxideExplain.Text = resources.GetString("lblOxideExplain.Text");
            // 
            // tbr18O_16Odefault
            // 
            this.tbr18O_16Odefault.BackColor = System.Drawing.Color.White;
            this.tbr18O_16Odefault.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbr18O_16Odefault.Location = new System.Drawing.Point(239, 9);
            this.tbr18O_16Odefault.Name = "tbr18O_16Odefault";
            this.tbr18O_16Odefault.Size = new System.Drawing.Size(87, 22);
            this.tbr18O_16Odefault.TabIndex = 50;
            this.tbr18O_16Odefault.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frmOxideCorrection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(355, 296);
            this.ControlBox = false;
            this.Controls.Add(this.tbr18O_16Odefault);
            this.Controls.Add(this.lblOxideExplain);
            this.Controls.Add(this.tbr18O_16O);
            this.Controls.Add(this.lblr18O_16O);
            this.Controls.Add(this.lblDefaultr18O_16O);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmOxideCorrection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Oxide Correction";
            this.Load += new System.EventHandler(this.frmOxideCorrection_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmOxideCorrection_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblDefaultr18O_16O;
        private System.Windows.Forms.Label lblr18O_16O;
        private System.Windows.Forms.TextBox tbr18O_16O;
        private System.Windows.Forms.Label lblOxideExplain;
        private System.Windows.Forms.TextBox tbr18O_16Odefault;
    }
}