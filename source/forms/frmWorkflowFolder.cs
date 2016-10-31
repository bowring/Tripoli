using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;
using Microsoft.Win32;
using System.IO;
using Tripoli.earth_time_org;

[assembly: RegistryPermissionAttribute(SecurityAction.RequestMinimum,
ViewAndModify = "HKEY_CURRENT_USER")]

namespace Tripoli.source.forms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmWorkFlowFolder : Form
    {
        private string dataTypeChoice;
        private string liveWorkflowDataFolderName;

        /// <summary>
        /// 
        /// </summary>
        public frmWorkFlowFolder()
        {
            InitializeComponent();

            // detect and prepare for chosen datatype
            dataTypeChoice = TripoliRegistry.GetCurrentDataTypeChoice();

            switch (dataTypeChoice)
            {
                case ".xls":
                    radioBut_IsotopX.Checked = true;
                    liveWorkflowDataFolderName = TripoliRegistry.GetRecentIsotopXLiveFolder();
                    break;

                case ".exp":
                    radioBut_ThermoFinniganTriton.Checked = true;
                    liveWorkflowDataFolderName = TripoliRegistry.GetRecentThermoFinniganTritonLiveFolder();
                    break;

                case ".dat":
                    radioBut_Sector54.Checked = true;
                    liveWorkflowDataFolderName = TripoliRegistry.GetRecentSector54LiveFolder();
                    break;

                case ".LiveDataStatus.txt":
                    radioBut_IonVantage.Checked= true;
                    liveWorkflowDataFolderName = TripoliRegistry.GetRecentIonVantageLiveDataStatusFileFolder();
                    break;
                
                default:
                    radioBut_ThermoFinniganMat261.Checked = true;
                    liveWorkflowDataFolderName = TripoliRegistry.GetRecentThermoFinniganMat261LiveFolder();
                    break;

            }

            // set ignore check box
            if (TripoliRegistry.GetIgnoreSampleMetaData().Equals("TRUE"))
            {
                ignoreSampleMetaData_chkbox.Checked = true;
            }


            try
            {
                processLiveWorkflowDataFolder();
            }
            catch (Exception)
            {
            }
        }

        private void processLiveWorkflowDataFolder()
        {
            if ((new DirectoryInfo(liveWorkflowDataFolderName)).Exists)
            {
                    ////////tempDirectoryInfo = (new DirectoryInfo(liveWorkflowDataFolderName)).Parent.GetDirectories("SampleMetaData");
                    ////////if (tempDirectoryInfo.Length > 0)
                    ////////{
                    ////////    sampleMetaDataExists_label.Text = "Parent directory does contain a SampleMetaData folder.";
                    ////////    sampleMetaDataExists_label.ForeColor = Color.Black;
                    ////////}
                    ////////else
                    ////////{
                    ////////    sampleMetaDataExists_label.Text = "Parent directory does NOT contain a SampleMetaData folder, so no data will be exported to ET_Redux.";
                    ////////    sampleMetaDataExists_label.ForeColor = Color.Red;
                    ////////}

                liveWorkflowDataFolderName_label.Text = liveWorkflowDataFolderName;
            } else
            {
                 liveWorkflowDataFolderName_label.Text = (string)liveWorkflowDataFolderName_label.Tag;
            }

            string sampleMetaDataFolderName = TripoliRegistry.GetSampleMetaDataFolder();
            if ((sampleMetaDataFolderName != null) && (new DirectoryInfo(sampleMetaDataFolderName).Exists))
            {
                lblSampleMetaDataFolder_label.Text = TripoliRegistry.GetSampleMetaDataFolder();
            } else
            {
                lblSampleMetaDataFolder_label.Text = (string)lblSampleMetaDataFolder_label.Tag;
            }
        }

        private void changeLiveWorkflowDataFolder_button_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Reset();
            folderBrowserDialog1.Description = "Select your Live Workflow Data folder:";
            folderBrowserDialog1.ShowNewFolderButton = false;
            folderBrowserDialog1.SelectedPath = liveWorkflowDataFolderName;
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                liveWorkflowDataFolderName = folderBrowserDialog1.SelectedPath;
                dataTypeChoice = TripoliRegistry.GetCurrentDataTypeChoice();
                switch (dataTypeChoice)
                {
                    case ".xls":
                        TripoliRegistry.SetRecentIsotopXLiveFolder(liveWorkflowDataFolderName);
                        break;

                    case ".exp":
                        TripoliRegistry.SetRecentThermoFinniganTritonLiveFolder(liveWorkflowDataFolderName);
                        break;

                    case ".dat":
                        TripoliRegistry.SetRecentSector54LiveFolder(liveWorkflowDataFolderName);
                        break;

                    case ".LiveDataStatus.txt":
                        TripoliRegistry.SetRecentIonVantageLiveDataStatusFileFolder(liveWorkflowDataFolderName);
                        break;

                    default:
                        TripoliRegistry.SetRecentThermoFinniganMat261LiveFolder(liveWorkflowDataFolderName);
                        break;
                }
                processLiveWorkflowDataFolder();
            }
        }

        private void radioBut_IsotopX_Click(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                TripoliRegistry.SetCurrentDataTypeChoice(".xls");
                liveWorkflowDataFolderName = TripoliRegistry.GetRecentIsotopXLiveFolder();
                if (liveWorkflowDataFolderName.CompareTo("") == 0)
                    changeLiveWorkflowDataFolder_button.PerformClick();
                else
                    processLiveWorkflowDataFolder();
            }
        }

        private void radioBut_Sector54_Click(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                TripoliRegistry.SetCurrentDataTypeChoice(".dat");
                liveWorkflowDataFolderName = TripoliRegistry.GetRecentSector54LiveFolder();
                if (liveWorkflowDataFolderName.CompareTo("") == 0)
                    changeLiveWorkflowDataFolder_button.PerformClick();
                else
                    processLiveWorkflowDataFolder();
            }
        }

        private void radioBut_ThermoFinniganTriton_Click(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                TripoliRegistry.SetCurrentDataTypeChoice(".exp");
                liveWorkflowDataFolderName = TripoliRegistry.GetRecentThermoFinniganTritonLiveFolder();
                if (liveWorkflowDataFolderName.CompareTo("") == 0)
                    changeLiveWorkflowDataFolder_button.PerformClick();
                else
                    processLiveWorkflowDataFolder();
            }
        }

        private void radioBut_ThermoFinniganMat261_Click(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                TripoliRegistry.SetCurrentDataTypeChoice(".txt");
                liveWorkflowDataFolderName = TripoliRegistry.GetRecentThermoFinniganMat261LiveFolder();
                if (liveWorkflowDataFolderName.CompareTo("") == 0)
                    changeLiveWorkflowDataFolder_button.PerformClick();
                else
                    processLiveWorkflowDataFolder();
            }
        }

        // July 2011
        private void radioBut_IonVantage_Click(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                TripoliRegistry.SetCurrentDataTypeChoice(".LiveDataStatus.txt");
                liveWorkflowDataFolderName =  TripoliRegistry.GetRecentIonVantageLiveDataStatusFileFolder();
                if (liveWorkflowDataFolderName.CompareTo("") == 0)
                    changeLiveWorkflowDataFolder_button.PerformClick();
                else
                    processLiveWorkflowDataFolder();
            }
        }

        // end July 2011

        private void changeSampleMetaDataFolder_button_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Reset();
            folderBrowserDialog1.Description = "Select your SampleMetaData folder:";
            folderBrowserDialog1.ShowNewFolderButton = false;
            folderBrowserDialog1.SelectedPath = TripoliRegistry.GetSampleMetaDataFolder();
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if ((result == DialogResult.OK) && (folderBrowserDialog1.SelectedPath.ToUpper().Contains("SAMPLEMETADATA")))
            {
                lblSampleMetaDataFolder_label.Text = folderBrowserDialog1.SelectedPath;
                TripoliRegistry.SetSampleMetaDataFolder(folderBrowserDialog1.SelectedPath);
            }
            else  if (result == DialogResult.OK)
            {
                lblSampleMetaDataFolder_label.Text = (string)lblSampleMetaDataFolder_label.Tag;
                TripoliRegistry.SetSampleMetaDataFolder("");
            }
        }

        private void ignoreSampleMetaData_chkbox_Click(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                TripoliRegistry.SetIgnoreSampleMetaData("TRUE");
            }
            else
            {
                TripoliRegistry.SetIgnoreSampleMetaData("FALSE");
            }
        }



    }
}
