/****************************************************************************
 * Copyright 2004-2017 James F. Bowring and www.Earth-Time.org
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 ****************************************************************************/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Tripoli
{
	/// <summary>
	/// Summary description for frmSaveUPbFractionDialogBox.
	/// </summary>
	public class frmSaveUPbFractionDialogBox : System.Windows.Forms.Form
	{
		// Fields
		string sampleName;
		string fractionID;


		private System.Windows.Forms.Button butOK;
		private System.Windows.Forms.Button butCancel;
		private System.Windows.Forms.TextBox textSampleName;
		private System.Windows.Forms.Label labelSampleName;
		private System.Windows.Forms.Label labelFractionID;
		private System.Windows.Forms.TextBox textFractionID;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSaveUPbFractionDialogBox()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.butOK = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.textSampleName = new System.Windows.Forms.TextBox();
            this.labelSampleName = new System.Windows.Forms.Label();
            this.labelFractionID = new System.Windows.Forms.Label();
            this.textFractionID = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // butOK
            // 
            this.butOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.butOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butOK.Location = new System.Drawing.Point(74, 156);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(75, 23);
            this.butOK.TabIndex = 0;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = false;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // butCancel
            // 
            this.butCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(208, 156);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 23);
            this.butCancel.TabIndex = 1;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = false;
            // 
            // textSampleName
            // 
            this.textSampleName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textSampleName.Location = new System.Drawing.Point(128, 40);
            this.textSampleName.Name = "textSampleName";
            this.textSampleName.Size = new System.Drawing.Size(200, 26);
            this.textSampleName.TabIndex = 2;
            // 
            // labelSampleName
            // 
            this.labelSampleName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSampleName.Location = new System.Drawing.Point(18, 40);
            this.labelSampleName.Name = "labelSampleName";
            this.labelSampleName.Size = new System.Drawing.Size(100, 23);
            this.labelSampleName.TabIndex = 3;
            this.labelSampleName.Text = "Sample Name:";
            this.labelSampleName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelFractionID
            // 
            this.labelFractionID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFractionID.Location = new System.Drawing.Point(18, 88);
            this.labelFractionID.Name = "labelFractionID";
            this.labelFractionID.Size = new System.Drawing.Size(100, 23);
            this.labelFractionID.TabIndex = 5;
            this.labelFractionID.Text = "Fraction ID:";
            this.labelFractionID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textFractionID
            // 
            this.textFractionID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textFractionID.Location = new System.Drawing.Point(128, 88);
            this.textFractionID.Name = "textFractionID";
            this.textFractionID.Size = new System.Drawing.Size(200, 26);
            this.textFractionID.TabIndex = 4;
            // 
            // frmSaveUPbFractionDialogBox
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(364, 202);
            this.ControlBox = false;
            this.Controls.Add(this.labelFractionID);
            this.Controls.Add(this.textFractionID);
            this.Controls.Add(this.labelSampleName);
            this.Controls.Add(this.textSampleName);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSaveUPbFractionDialogBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "U-Pb Redux Sample Name and Fraction ID";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		public string SampleName
		{
			get
			{
				return sampleName;
			}
			set
			{
                if (value != "") 
                {
                    sampleName = value;
                    textSampleName.Text = sampleName;
                }
			}
		}

		public string FractionID
		{
			get
			{
				return fractionID;
			}
			set
			{
                if  (value != null)// nov 2009  (value != "")
                {
                    fractionID = value;
                    textFractionID.Text = fractionID;
                }
			}
		}

        private void butOK_Click(object sender, EventArgs e)
        {
            SampleName = textSampleName.Text;
            FractionID = textFractionID.Text;
        }
	}
}
