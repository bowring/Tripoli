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
using Wintellect.PowerCollections;
using Tripoli.earth_time_org;

namespace Tripoli
{
    public partial class frmBaPO2_Tl_IsotopicComposition : Form
    {
        private TripoliWorkProduct RawRatios;

        /// <summary>
        /// Displays BaPO2 and Tl Isotopic Composition calculated from models.
        /// Provides for user to specify alphaPb when 202/205 measured is missing.
        /// </summary>
        /// <param name="CurrentBariumPhosphateIC"></param>
        /// <param name="CurrentThalliumIC"></param>
        /// <param name="myRawRatios"></param>
        public frmBaPO2_Tl_IsotopicComposition( //
            BariumPhosphateIC CurrentBariumPhosphateIC, 
            ThalliumIC CurrentThalliumIC,
            TripoliWorkProduct myRawRatios)
        {
            this.RawRatios = myRawRatios;
            InitializeComponent();

            OrderedDictionary<string, decimal> BaPO2IC = null;
            decimal r201_205BaPO2 = 0.0m;
            decimal r202_205BaPO2 = 0.0m;
            decimal r203_205BaPO2 = 0.0m;
            decimal r204_205BaPO2 = 0.0m;
            if (CurrentBariumPhosphateIC != null)
            {
                BaPO2IC = CurrentBariumPhosphateIC.calculateIsotopicComposition();
                BaPO2IC.TryGetValue("r201_205BaPO2", out r201_205BaPO2);
                BaPO2IC.TryGetValue("r202_205BaPO2", out r202_205BaPO2);
                BaPO2IC.TryGetValue("r203_205BaPO2", out r203_205BaPO2);
                BaPO2IC.TryGetValue("r204_205BaPO2", out r204_205BaPO2);
            }

            OrderedDictionary<string, decimal> TlIC = null;
            decimal r203_205Tl = 0.0m;
            if (CurrentThalliumIC != null)
            {
                TlIC = CurrentThalliumIC.calculateIsotopicComposition();
                TlIC.TryGetValue("r203_205Tl", out r203_205Tl);
            }


            // populate form
            lblLoadedBaPO2ModelName.Text += CurrentBariumPhosphateIC.getNameAndVersion();
            txtr201_205BaPO2.Text = String.Format("{0:#####0.00000}", r201_205BaPO2);
            txtr202_205BaPO2.Text = String.Format("{0:#####0.00000}", r202_205BaPO2);
            txtr203_205BaPO2.Text = String.Format("{0:#####0.00000}", r203_205BaPO2);
            txtr204_205BaPO2.Text = String.Format("{0:#####0.00000}", r204_205BaPO2);

            lblLoadedTlModelName.Text += CurrentThalliumIC.getNameAndVersion();
            txtr203_205Tl.Text = String.Format("{0:#####0.00000}", r203_205Tl);

            if (RawRatios == null)
            {
                alphaPbPanel.Visible = false;
                txtAlphaPb.Text = "0.00";
            }
            else
            {
                alphaPbPanel.Visible = true;
                txtAlphaPb.Text = Convert.ToString(RawRatios.AlphaPb * 100.0m);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (RawRatios != null)
                    RawRatios.AlphaPb = Convert.ToDecimal(txtAlphaPb.Text) / 100.0m;
            }
            catch (Exception)
            {

            }
        }

        private void btn_Explanation_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + @"\docs\BaPO2_Tl_Interference.pdf");
            }
            catch (System.UnauthorizedAccessException eFile)
            {
                MessageBox.Show(
                    "BaPO2_Tl_Interference.pdf not found ... please repair your installation.\n\n"
                    + eFile.Message,
                    "Tripoli Warning");
            }
        }

        }
}
