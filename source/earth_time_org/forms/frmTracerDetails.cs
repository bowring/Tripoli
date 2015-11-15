using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tripoli.earth_time_org;

namespace Tripoli.earth_time_org
{
    public partial class frmTracerDetails : Form
    {
        // Fields
        private Tracer _myTracer;
        private bool _readOnly = false;
        private bool _cancel = true;

        public frmTracerDetails(Tracer tracer, bool readOnly)
        {
            myTracer = tracer;
            _readOnly = readOnly;

            InitializeComponent();

            // setup combo box for tracer type
            for (int t = 0; t < DataDictionary.TracerTypes.Length; t++)
            {
                comboTracerType.Items.Add(DataDictionary.TracerTypes[t]);
            }

        }

        #region Properties
        public Tracer myTracer
        {
            get{return _myTracer;}
            set {_myTracer = value;}
        }
        public bool cancel
        {
            get { return _cancel; }
            set { _cancel = value; }
        }
        #endregion Properties

        private void frmTracerDetails_Load(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                if (c.GetType().Equals(typeof(TextBox)))
                    ((TextBox)c).ReadOnly = _readOnly;
            }

            comboTracerType.Enabled = !_readOnly;


            // hide cancel button for readonly
            btnCancel.Visible = !_readOnly;

            txtTracerName.DataBindings.Add(
                "Text", myTracer, "tracerName", true, DataSourceUpdateMode.OnValidation);

            txtVersion.DataBindings.Add(
                "Text", myTracer, "versionNumber", true, DataSourceUpdateMode.OnValidation);

            comboTracerType.DataBindings.Add(
                "SelectedItem", myTracer, "tracerType", true, DataSourceUpdateMode.OnValidation);

            txtLab.DataBindings.Add(
                "Text", myTracer, "labName", true, DataSourceUpdateMode.OnValidation);

            txtDateCertified.DataBindings.Add(
                "Text", myTracer, "dateCertified", true, DataSourceUpdateMode.OnValidation);

            
            
            txtr206_204t.DataBindings.Add(
                "Text", myTracer.getRatioByName("r206_204t"), "value", true, DataSourceUpdateMode.OnValidation);

            txtr206_204terr.DataBindings.Add(
                "Text", myTracer.getRatioByName("r206_204t"), "oneSigma", true, DataSourceUpdateMode.OnValidation);

            txtr207_206t.DataBindings.Add(
                "Text", myTracer.getRatioByName("r207_206t"), "value", true, DataSourceUpdateMode.OnValidation);

            txtr207_206terr.DataBindings.Add(
                "Text", myTracer.getRatioByName("r207_206t"), "oneSigma", true, DataSourceUpdateMode.OnValidation);

            txtr206_208t.DataBindings.Add(
                "Text", myTracer.getRatioByName("r206_208t"), "value", true, DataSourceUpdateMode.OnValidation);

            txtr206_208terr.DataBindings.Add(
                "Text", myTracer.getRatioByName("r206_208t"), "oneSigma", true, DataSourceUpdateMode.OnValidation);

            txtr206_205t.DataBindings.Add(
                "Text", myTracer.getRatioByName("r206_205t"), "value", true, DataSourceUpdateMode.OnValidation);

            txtr206_205terr.DataBindings.Add(
                "Text", myTracer.getRatioByName("r206_205t"), "oneSigma", true, DataSourceUpdateMode.OnValidation);

            txtr207_205t.DataBindings.Add(
                "Text", myTracer.getRatioByName("r207_205t"), "value", true, DataSourceUpdateMode.OnValidation);

            txtr207_205terr.DataBindings.Add(
                "Text", myTracer.getRatioByName("r207_205t"), "oneSigma", true, DataSourceUpdateMode.OnValidation);

            txtr208_205t.DataBindings.Add(
                "Text", myTracer.getRatioByName("r208_205t"), "value", true, DataSourceUpdateMode.OnValidation);

            txtr208_205terr.DataBindings.Add(
               "Text", myTracer.getRatioByName("r208_205t"), "oneSigma", true, DataSourceUpdateMode.OnValidation);

            txtr202_205t.DataBindings.Add(
                "Text", myTracer.getRatioByName("r202_205t"), "value", true, DataSourceUpdateMode.OnValidation);

            txtr202_205terr.DataBindings.Add(
               "Text", myTracer.getRatioByName("r202_205t"), "oneSigma", true, DataSourceUpdateMode.OnValidation);

            

            txtr238_235t.DataBindings.Add(
                "Text", myTracer.getRatioByName("r238_235t"), "value", true, DataSourceUpdateMode.OnValidation);

            txtr238_235terr.DataBindings.Add(
                "Text", myTracer.getRatioByName("r238_235t"), "oneSigma", true, DataSourceUpdateMode.OnValidation);

            txtr233_235t.DataBindings.Add(
                "Text", myTracer.getRatioByName("r233_235t"), "value", true, DataSourceUpdateMode.OnValidation);

            txtr233_235terr.DataBindings.Add(
                "Text", myTracer.getRatioByName("r233_235t"), "oneSigma", true, DataSourceUpdateMode.OnValidation);

            txtr235_205t.DataBindings.Add(
                "Text", myTracer.getRatioByName("r235_205t"), "value", true, DataSourceUpdateMode.OnValidation);

            txtr235_205terr.DataBindings.Add(
                "Text", myTracer.getRatioByName("r235_205t"), "oneSigma", true, DataSourceUpdateMode.OnValidation);

            txtr233_236t.DataBindings.Add(
                "Text", myTracer.getRatioByName("r233_236t"), "value", true, DataSourceUpdateMode.OnValidation);

            txtr233_236terr.DataBindings.Add(
                "Text", myTracer.getRatioByName("r233_236t"), "oneSigma", true, DataSourceUpdateMode.OnValidation);
            
            
            
            txtconcPb205t.DataBindings.Add(
                "Text", myTracer.getIsotopeConcByName("concPb205t"), "value", true, DataSourceUpdateMode.OnValidation);

            txtconcPb205terr.DataBindings.Add(
                "Text", myTracer.getIsotopeConcByName("concPb205t"), "oneSigma", true, DataSourceUpdateMode.OnValidation);
            
            txtconcU235t.DataBindings.Add(
                "Text", myTracer.getIsotopeConcByName("concU235t"), "value", true, DataSourceUpdateMode.OnValidation);
            
            txtconcU235terr.DataBindings.Add(
                "Text", myTracer.getIsotopeConcByName("concU235t"), "oneSigma", true, DataSourceUpdateMode.OnValidation);
        }


        private void frmTracerDetails_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!cancel)
            {
                foreach (Control c in this.Controls)
                {
                    if (c.GetType().Equals(typeof(TextBox)))
                        ((TextBox)c).DataBindings["Text"].WriteValue();
                }
            }

            comboTracerType.DataBindings["SelectedItem"].WriteValue();
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



    }
}