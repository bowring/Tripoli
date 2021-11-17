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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace Tripoli
{
    public partial class frmTritonCyclePerBlock : Form
    {
        //public static double CHAUVENET_DEFAULT = 0.50;
        
        private double _ChauvenetsThreshold;
        private bool _cancel = true;
               
        public frmTritonCyclePerBlock(double _ChauvenetsThreshold, Boolean fractionMode)
        {
            InitializeComponent();
            lblCurrentChauvenet.Visible = fractionMode;
            mtbCurrentChauvenet.Visible = fractionMode;
            btnUseDefault.Visible = fractionMode;
            this._ChauvenetsThreshold = _ChauvenetsThreshold;
        }

        public double ChauvenetsThreshold
        {
            get { return _ChauvenetsThreshold; }
            set { _ChauvenetsThreshold = (value <= 1.0) && (value >= 0.0) ? value : Convert.ToDouble(Properties.Settings.Default.ChauvenetCriterion); }
        }
        public bool cancel
        {
            get { return _cancel; }
            set { _cancel = value; }
        }

        private void btnViewChauvenet_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + @"\docs\ChauvenetsCriterion.pdf");
            }
            catch (System.UnauthorizedAccessException eFile)
            {
                MessageBox.Show(
                    "ChauvenetsCriterion.pdf not found ... please repair your installation.\n\n"
                    + eFile.Message,
                    "Tripoli Warning");
            }
        }

        private void frmChauvenet_Load(object sender, EventArgs e)
        {
            tbDefaultChauvenet.Text = Properties.Settings.Default.ChauvenetCriterion.ToString();
            
            mtbCurrentChauvenet.DataBindings.Add(
                "Text", this, "ChauvenetsThreshold", true, DataSourceUpdateMode.OnValidation);

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cancel = true;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            cancel = false;
            this.Close();
        }

        private void frmChauvenet_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!cancel)
            {
                mtbCurrentChauvenet.DataBindings["Text"].WriteValue();
            }
        }

        private void btnUseDefault_Click(object sender, EventArgs e)
        {
            mtbCurrentChauvenet.Text = Properties.Settings.Default.ChauvenetCriterion.ToString();
        }


    }
}