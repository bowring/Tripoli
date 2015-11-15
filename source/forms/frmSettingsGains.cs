/****************************************************************************
 * Copyright 2004-2015 James F. Bowring and www.Earth-Time.org
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
	/// Summary description for frmSettingsGains.
	/// </summary>
	public class frmSettingsGains : System.Windows.Forms.Form
	{
		//fields
		int _RejectLevelSigma;

		private System.Windows.Forms.NumericUpDown updownGainsStdDev;
		private System.Windows.Forms.Label lblSetting1;
		private System.Windows.Forms.Label lblSettingGainsNote1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSettingsGains()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettingsGains));
            this.updownGainsStdDev = new System.Windows.Forms.NumericUpDown();
            this.lblSetting1 = new System.Windows.Forms.Label();
            this.lblSettingGainsNote1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.updownGainsStdDev)).BeginInit();
            this.SuspendLayout();
            // 
            // updownGainsStdDev
            // 
            this.updownGainsStdDev.BackColor = System.Drawing.Color.Azure;
            this.updownGainsStdDev.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updownGainsStdDev.Location = new System.Drawing.Point(214, 26);
            this.updownGainsStdDev.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.updownGainsStdDev.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.updownGainsStdDev.Name = "updownGainsStdDev";
            this.updownGainsStdDev.ReadOnly = true;
            this.updownGainsStdDev.Size = new System.Drawing.Size(48, 22);
            this.updownGainsStdDev.TabIndex = 0;
            this.updownGainsStdDev.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.updownGainsStdDev.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.updownGainsStdDev.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.updownGainsStdDev.ValueChanged += new System.EventHandler(this.updownGainsStdDev_ValueChanged);
            // 
            // lblSetting1
            // 
            this.lblSetting1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSetting1.Location = new System.Drawing.Point(8, 28);
            this.lblSetting1.Name = "lblSetting1";
            this.lblSetting1.Size = new System.Drawing.Size(318, 23);
            this.lblSetting1.TabIndex = 1;
            this.lblSetting1.Text = "1.  Reject gains values outside                  sigmas.";
            this.lblSetting1.UseCompatibleTextRendering = true;
            // 
            // lblSettingGainsNote1
            // 
            this.lblSettingGainsNote1.Location = new System.Drawing.Point(30, 60);
            this.lblSettingGainsNote1.Name = "lblSettingGainsNote1";
            this.lblSettingGainsNote1.Size = new System.Drawing.Size(274, 72);
            this.lblSettingGainsNote1.TabIndex = 2;
            this.lblSettingGainsNote1.Text = resources.GetString("lblSettingGainsNote1.Text");
            this.lblSettingGainsNote1.UseCompatibleTextRendering = true;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(4, 237);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(161, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel (No Change)";
            this.btnCancel.UseCompatibleTextRendering = true;
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(172, 237);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(161, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK (Refresh Data)";
            this.btnOK.UseCompatibleTextRendering = true;
            this.btnOK.UseVisualStyleBackColor = false;
            // 
            // frmSettingsGains
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(338, 266);
            this.ControlBox = false;
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblSettingGainsNote1);
            this.Controls.Add(this.updownGainsStdDev);
            this.Controls.Add(this.lblSetting1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmSettingsGains";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings for Gains";
            ((System.ComponentModel.ISupportInitialize)(this.updownGainsStdDev)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void updownGainsStdDev_ValueChanged(object sender, System.EventArgs e)
		{
			// must use field, since proerty changes updown == circular
			_RejectLevelSigma = (int)((NumericUpDown)sender).Value;
		}

		public int RejectLevelSigma
		{
			get
			{
				return _RejectLevelSigma;
			}
			set
			{
				_RejectLevelSigma = value;
				// assume safely [0,10]
				updownGainsStdDev.Value = RejectLevelSigma;
			}
		}
	}
}
