using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Tripoli.earth_time_org
{
    public partial class frmBariumPhosphateICDetails : Form
    {
        // Fields
        private BariumPhosphateIC _myBariumPhosphateIC;
        private bool _readOnly = false;
        private bool _cancel = true;

        public frmBariumPhosphateICDetails(BariumPhosphateIC bariumPhosphateIC, bool readOnly)
        {
            _myBariumPhosphateIC = bariumPhosphateIC;
            _readOnly = readOnly;

            InitializeComponent();
        }

        #region Properties
        public BariumPhosphateIC myBariumPhosphateIC
        {
            get { return _myBariumPhosphateIC; }
            set { _myBariumPhosphateIC = value; }
        }

        public bool readOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }

        public bool cancel
        {
            get { return _cancel; }
            set { _cancel = value; }
        }

        #endregion Properties

        private void frmBariumPhosphateICDetails_Load(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                if (c.GetType().Equals(typeof(TextBox)))
                    ((TextBox)c).ReadOnly = _readOnly;
            }

            txtBariumPhosphateIC.DataBindings.Add(
                    "Text", myBariumPhosphateIC, "bariumPhosphateICName", true, DataSourceUpdateMode.OnValidation);

            txtVersion.DataBindings.Add(
                    "Text", myBariumPhosphateIC, "versionNumber", true, DataSourceUpdateMode.OnValidation);

            txtLab.DataBindings.Add(
                "Text", myBariumPhosphateIC, "labName", true, DataSourceUpdateMode.OnValidation);

            txtDateCertified.DataBindings.Add(
                "Text", myBariumPhosphateIC, "dateCertified", true, DataSourceUpdateMode.OnValidation);


            txt130Abundance.DataBindings.Add(
                "Text", myBariumPhosphateIC.getIsotopeAbundanceByName("pct130Ba_BaPO2"), "value", true, DataSourceUpdateMode.OnValidation);

            txt132Abundance.DataBindings.Add(
                "Text", myBariumPhosphateIC.getIsotopeAbundanceByName("pct132Ba_BaPO2"), "value", true, DataSourceUpdateMode.OnValidation);

            txt134Abundance.DataBindings.Add(
                "Text", myBariumPhosphateIC.getIsotopeAbundanceByName("pct134Ba_BaPO2"), "value", true, DataSourceUpdateMode.OnValidation);

            txt135Abundance.DataBindings.Add(
                "Text", myBariumPhosphateIC.getIsotopeAbundanceByName("pct135Ba_BaPO2"), "value", true, DataSourceUpdateMode.OnValidation);

            txt136Abundance.DataBindings.Add(
                "Text", myBariumPhosphateIC.getIsotopeAbundanceByName("pct136Ba_BaPO2"), "value", true, DataSourceUpdateMode.OnValidation);

            txt137Abundance.DataBindings.Add(
                "Text", myBariumPhosphateIC.getIsotopeAbundanceByName("pct137Ba_BaPO2"), "value", true, DataSourceUpdateMode.OnValidation);

            txt138Abundance.DataBindings.Add(
                 "Text", myBariumPhosphateIC.getIsotopeAbundanceByName("pct138Ba_BaPO2"), "value", true, DataSourceUpdateMode.OnValidation);


            txt16Abundance.DataBindings.Add(
                "Text", myBariumPhosphateIC.getIsotopeAbundanceByName("pct16O_BaPO2"), "value", true, DataSourceUpdateMode.OnValidation);

            txt17Abundance.DataBindings.Add(
                 "Text", myBariumPhosphateIC.getIsotopeAbundanceByName("pct17O_BaPO2"), "value", true, DataSourceUpdateMode.OnValidation);

            txt18Abundance.DataBindings.Add(
                "Text", myBariumPhosphateIC.getIsotopeAbundanceByName("pct18O_BaPO2"), "value", true, DataSourceUpdateMode.OnValidation);


        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            cancel = false;
            this.Close();
        }
    }
}
