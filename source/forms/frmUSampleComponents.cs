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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tripoli.earth_time_org;

namespace Tripoli
{
    public partial class frmUsampleComponents : Form
    {
        private USampleComponents _uSampleComponents;

        public USampleComponents uSampleComponents
        {
            get { return _uSampleComponents; }
            set { _uSampleComponents = value; }
        }

        public frmUsampleComponents(USampleComponents uSampleComponents)
        {
            InitializeComponent();
            
            _uSampleComponents = uSampleComponents;
            txtDefaultLabUBlankMass.Text = Convert.ToString(uSampleComponents.DefaultLabUBlankMass);
            txtDefaultR238_235b.Text = Convert.ToString(uSampleComponents.DefaultR238_235b);
            txtDefaultR238_235s.Text = Convert.ToString(uSampleComponents.DefaultR238_235s);
            txtDefaultTracerMass.Text = Convert.ToString(uSampleComponents.DefaultTracerMass);
            txtDefaultGMol235.Text = Convert.ToString(uSampleComponents.DefaultGmol235);
            txtDefaultGMol238.Text = Convert.ToString(uSampleComponents.DefaultGmol238);

            txtLabUBlankMass.Text = Convert.ToString(uSampleComponents.labUBlankMass);
            txtR238_235b.Text = Convert.ToString(uSampleComponents.r238_235b);
            txtR238_235s.Text = Convert.ToString(uSampleComponents.r238_235s);
            txtTracerMass.Text = Convert.ToString(uSampleComponents.tracerMass);
            txtGMol235.Text = Convert.ToString(uSampleComponents.Gmol235);
            txtGMol238.Text = Convert.ToString(uSampleComponents.Gmol238);

            txtTracerMass.Focus();
        }



        private void btnEditDefaultValues_Click(object sender, EventArgs e)
        {
            txtDefaultLabUBlankMass.ReadOnly = false;
            txtDefaultR238_235b.ReadOnly = false;
            txtDefaultR238_235s.ReadOnly = false;
            txtDefaultTracerMass.ReadOnly = false;
            txtDefaultGMol235.ReadOnly = false;
            txtDefaultGMol238.ReadOnly = false;

            btnSaveDefaultValues.Visible = true;
            ((Button)sender).Visible = false;
        }

        private void btnSaveDefaultValues_Click(object sender, EventArgs e)
        {
            uSampleComponents.DefaultLabUBlankMass = Convert.ToDecimal(txtDefaultLabUBlankMass.Text);
            uSampleComponents.DefaultR238_235b = Convert.ToDecimal(txtDefaultR238_235b.Text);
            uSampleComponents.DefaultR238_235s = Convert.ToDecimal(txtDefaultR238_235s.Text);
            uSampleComponents.DefaultTracerMass = Convert.ToDecimal(txtDefaultTracerMass.Text);
            uSampleComponents.DefaultGmol235 = Convert.ToDecimal(txtDefaultGMol235.Text);
            uSampleComponents.DefaultGmol238 = Convert.ToDecimal(txtDefaultGMol238.Text);

            uSampleComponents.SetDefaultUsampleComponents();

            txtDefaultLabUBlankMass.ReadOnly = true;
            txtDefaultR238_235b.ReadOnly = true;
            txtDefaultR238_235s.ReadOnly = true;
            txtDefaultTracerMass.ReadOnly = true;
            txtDefaultGMol235.ReadOnly = true;
            txtDefaultGMol238.ReadOnly = true;

            btnEditDefaultValues.Visible = true;
            ((Button)sender).Visible = false;

        }

        private void btnUseDefaultVaules_Click(object sender, EventArgs e)
        {
            txtLabUBlankMass.Text = Convert.ToString(uSampleComponents.DefaultLabUBlankMass);
            txtR238_235b.Text = Convert.ToString(uSampleComponents.DefaultR238_235b);
            txtR238_235s.Text = Convert.ToString(uSampleComponents.DefaultR238_235s);
            txtTracerMass.Text = Convert.ToString(uSampleComponents.DefaultTracerMass);
            txtGMol235.Text = Convert.ToString(uSampleComponents.DefaultGmol235);
            txtGMol238.Text = Convert.ToString(uSampleComponents.DefaultGmol238);
        }

        private void btnSaveCurrentValues_Click(object sender, EventArgs e)
        {
            uSampleComponents.labUBlankMass = Convert.ToDecimal(txtLabUBlankMass.Text);
            uSampleComponents.r238_235b = Convert.ToDecimal(txtR238_235b.Text);
            uSampleComponents.r238_235s = Convert.ToDecimal(txtR238_235s.Text);
            uSampleComponents.tracerMass = Convert.ToDecimal(txtTracerMass.Text);
            uSampleComponents.Gmol235 = Convert.ToDecimal(txtGMol235.Text);
            uSampleComponents.Gmol238 = Convert.ToDecimal(txtGMol238.Text);

        }


    }
}