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
using Tripoli.earth_time_org;

namespace Tripoli
{
    public partial class frmOxideCorrection : Form
    {
        private decimal _r18O_16O;
        USampleComponents _uSampleComponents;
        private bool _cancel = true;

        public frmOxideCorrection(decimal _r18O_16O, USampleComponents uSampleComponents)
        {
            this._r18O_16O = _r18O_16O;
            this._uSampleComponents = uSampleComponents;

            InitializeComponent();

            tbr18O_16Odefault.Text = Convert.ToString(uSampleComponents.DefaultR18O_16O);
        }

        public decimal r18O_16O
        {
            get { return _r18O_16O; }
            set { _r18O_16O = value; }
        }
        public bool cancel
        {
            get { return _cancel; }
            set { _cancel = value; }
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


        private void frmOxideCorrection_Load(object sender, EventArgs e)
        {           
            //tbr18O_16O.DataBindings.Add(
            //    "Text", this, "r18O_16O", true, DataSourceUpdateMode.OnValidation);
            tbr18O_16O.Text = Convert.ToString(_r18O_16O);

        }

        private void frmOxideCorrection_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!cancel)
            {
                ////tbr18O_16O.DataBindings["Text"].WriteValue();
                _uSampleComponents.DefaultR18O_16O = Convert.ToDecimal(tbr18O_16Odefault.Text);
                _uSampleComponents.SetDefaultUsampleComponents();

            }
        }

        private void btnUseDefault_Click(object sender, EventArgs e)
        {
            //tbr18O_16O.Text = Properties.Settings.Default.r18O_16O.ToString();
            tbr18O_16O.Text = Convert.ToString(_uSampleComponents.DefaultR18O_16O);
        }


    }
}