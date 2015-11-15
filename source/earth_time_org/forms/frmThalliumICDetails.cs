using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Tripoli.earth_time_org
{
    public partial class frmThalliumICDetails : Form
    {
        // Fields
        private ThalliumIC _myThalliumIC;
        private bool _readOnly = false;
        private bool _cancel = true;

        public frmThalliumICDetails(ThalliumIC thalliumIC, bool readOnly)
        {
            _myThalliumIC = thalliumIC;
            _readOnly = readOnly;

            InitializeComponent();
        }

        #region Properties
        public ThalliumIC myThalliumIC
        {
            get { return _myThalliumIC; }
            set { _myThalliumIC = value; }
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
        private void frmThalliumICDetails_Load(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                if (c.GetType().Equals(typeof(TextBox)))
                    ((TextBox)c).ReadOnly = _readOnly;
            }

            txtThalliumIC.DataBindings.Add(
                    "Text", myThalliumIC, "thalliumICName", true, DataSourceUpdateMode.OnValidation);

            txtVersion.DataBindings.Add(
                    "Text", myThalliumIC, "versionNumber", true, DataSourceUpdateMode.OnValidation);

            txtLab.DataBindings.Add(
                "Text", myThalliumIC, "labName", true, DataSourceUpdateMode.OnValidation);

            txtDateCertified.DataBindings.Add(
                "Text", myThalliumIC, "dateCertified", true, DataSourceUpdateMode.OnValidation);


            txt203Abundance.DataBindings.Add(
                "Text", myThalliumIC.getIsotopeAbundanceByName("pct203Tl_Tl"), "value", true, DataSourceUpdateMode.OnValidation);

            txt205Abundance.DataBindings.Add(
                "Text", myThalliumIC.getIsotopeAbundanceByName("pct205Tl_Tl"), "value", true, DataSourceUpdateMode.OnValidation);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            cancel = false;
            this.Close();
        }

    }
}
