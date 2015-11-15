/****************************************************************************
 * Copyright 2004-2009 James F. Bowring
 * Sunchex Systems
 * for Sam Bowring, MIT
 * 
 * 
 * 
 ****************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tripoli.source;
using System.IO;
using Tripoli.source.forms;

namespace Tripoli
{
    public partial class frmTripoliControlPanel : Form
    {
        private frmMainTripoli myTripoli;
        private TripoliWorkProduct rawRatios;
        private Timer refreshView;
        private string currentDatFileName;
        private DirectoryInfo sampleMetaDataFolder;

        public DirectoryInfo SampleMetaDataFolder
        {
            get { return sampleMetaDataFolder; }
            set { sampleMetaDataFolder = value; }
        }

        public string CurrentDatFileName
        {
            get { return currentDatFileName; }
            set { currentDatFileName = value; }
        }

        public frmTripoliControlPanel(frmMainTripoli myTripoli)
        {
            this.myTripoli = myTripoli;
            InitializeComponent();
            SetDesktopLocation(myTripoli.DesktopLocation.X, myTripoli.DesktopLocation.Y);

            refreshView = new Timer();
            refreshView.Interval = 5000;
            refreshView.Start();
            refreshView.Tick += new EventHandler(Timer_Tick);

            currentDatFileName = "";
            sampleMetaDataFolder = null;

            tuneStatus();
        }

        public void Timer_Tick(object sender, EventArgs eArgs)
        {
            if (sender == refreshView)
                tuneStatus();
        }


        public void tuneStatus()
        {            
            FileInfo currentLiveWorkflowDataFile = locateCurrentLiveWorkflowDataFile();

            // check for live update manager folder SampleMetaData
            if (currentLiveWorkflowDataFile != null)
            {
                DirectoryInfo[] parentFolder = currentLiveWorkflowDataFile.Directory.Parent.GetDirectories("SampleMetaData", SearchOption.TopDirectoryOnly);
                if (parentFolder.Length > 0)
                {
                    sampleMetaDataFolder = parentFolder[0];
                }
            }
           
            // enable buttons
            startStopLiveWorkflow_button.Enabled = true;// (sampleMetaDataFolder != null);
            myTripoli.startStopLiveWorkflow_menuItem.Enabled = startStopLiveWorkflow_button.Enabled;
            toolTip1.SetToolTip(startStopLiveWorkflow_button, //
                "Starting Live Workflow causes Tripoli to automatically load the newest data file and to save the Tripoli Workfile (.trip).  "//
                 + "\n\nIf matching U-Pb_Redux - generated Sample metadata is present, then Tripoli automatically exports the XML file to the specified location.");
            toolTip1.ShowAlways = true;

            loadNewestDataFile_button.Enabled = (currentLiveWorkflowDataFile != null);
            myTripoli.loadCurrentDatFile_menuItem.Enabled = loadNewestDataFile_button.Enabled;

            saveTripXML_button.Enabled = loadNewestDataFile_button.Enabled;
            myTripoli.saveTripAndExportToRedux_menuItem.Enabled = saveTripXML_button.Enabled;
        }

        public void setReportStatus(String status)
        {
            newestDataFileStatusLabel.Text = status;
            this.Refresh();
            myTripoli.loadCurrentDatFile_menuItem.Text = myTripoli.loadCurrentDatFile_menuItem.Tag + " (" + status + ")";
        }

        public string getReportStatus()
        {
            return newestDataFileStatusLabel.Text;
        }


        public FileInfo locateCurrentLiveWorkflowDataFile()
        {
            FileInfo retVal = null;
            String liveWorkflowDataFolderName = "";

            // detect and prepare for chosen datatype
            string dataTypeChoice = TripoliRegistry.GetCurrentDataTypeChoice();
            switch (dataTypeChoice)
            {
                case ".xls":
                    liveWorkflowDataFolderName = TripoliRegistry.GetRecentIsotopXLiveFolder();
                    break;

                case ".exp":
                    liveWorkflowDataFolderName = TripoliRegistry.GetRecentThermoFinneganLiveFolder();
                    break;

                default:
                    liveWorkflowDataFolderName = TripoliRegistry.GetRecentSector54LiveFolder();
                    break;
            }

          
            try
            {
                DirectoryInfo currentLiveWorkflowDataFolder = new DirectoryInfo(liveWorkflowDataFolderName);
                if (currentLiveWorkflowDataFolder.Exists)
                {
                    // assume that data files are in the top level , i.e. this is currentLiveWorkflowDataFolder
                    FileInfo[] files = currentLiveWorkflowDataFolder.GetFiles("*" + dataTypeChoice, SearchOption.TopDirectoryOnly);
                    Array.Sort<FileInfo>(files, new Comparison<FileInfo>(delegate(FileInfo f1, FileInfo f2)
                    {
                        // most recent first
                        return DateTime.Compare(f2.LastWriteTimeUtc, f1.LastWriteTimeUtc);
                    }));

                    if (files.Length > 0)
                    {
                        retVal = files[0];

                        // auto save and load this puppy if Live Workflow
                        Console.WriteLine("Times: stored = " + TripoliRegistry.GetRecentLiveWorkflowFileAccessTime().ToString() + "  retVal = " + retVal.LastWriteTimeUtc.ToBinary().ToString());
                        if (myTripoli.IsInLiveWorkflow && (TripoliRegistry.GetRecentLiveWorkflowFileAccessTime().CompareTo(retVal.LastWriteTimeUtc.ToBinary()) < 0))
                        {
                            myTripoli.loadCurrentLiveWorkflowDataFile(retVal.FullName);
                        }
                    }
                }

            }
            catch (Exception)
            {
            }
            if (retVal == null)
                setReportStatus( "No data files: " + ((liveWorkflowDataFolderName.Equals("")) ? "NONE" : liveWorkflowDataFolderName));
            else
                setReportStatus("Newest data: " + retVal.Name);

            Refresh();

            return retVal;
        }

        private void btnExportToPbMacDat_Click(object sender, EventArgs e)
        {
            myTripoli.saveAndExportToClipboardForPbMacdat();
        }

        private void btnReportPbMacdatExport_Click(object sender, EventArgs e)
        {
            MessageBox.Show(myTripoli.ReportOfPbMacDat, "Tripoli Kwiki Report");
        }

        public frmMainTripoli MyTripoli
        {
            get { return myTripoli; }
            set { myTripoli = value; }
        }

        public TripoliWorkProduct RawRatios
        {
            get { return rawRatios; }
            set { rawRatios = value; }
        }

        private void getCurrentData_button_Click(object sender, EventArgs e)
        {
            try
            {
                string currentDatFileFullName = locateCurrentLiveWorkflowDataFile().FullName;
                myTripoli.loadCurrentLiveWorkflowDataFile(currentDatFileFullName);
            }
            catch (Exception)
            {

            }
            //currentStatusLabel.Text = saveSStext;
            this.Refresh();
        }

        private void saveTripXML_button_Click(object sender, EventArgs e)
        {
            myTripoli.saveTripAndExportXML(); 
        }

        private void exportAsDelimitedText_button_Click(object sender, EventArgs e)
        {
            myTripoli.exportCheckedRatiosAsDelimitedText();
        }

        private void frmTripoliControlPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            ((frmTripoliControlPanel)sender).Hide();
            e.Cancel = true; ;
        }

        public void Hide() {
            base.Hide();
            refreshView.Stop();
        }

        public void Show()
        {
            base.Show();
            refreshView.Start();
        }

        private void startStopLiveWorkflow_button_Click(object sender, EventArgs e)
        {
            bool isLive = myTripoli.toggleLiveWorkflow();
        }

        public void tuneLiveWorkflowButton(bool isLive)
        {
            if (isLive)
            {
                startStopLiveWorkflow_button.Text = "Stop Live Workflow";
                startStopLiveWorkflow_button.BackColor = Color.MistyRose;
            }
            else
            {
                startStopLiveWorkflow_button.Text = "Start Live Workflow";
                startStopLiveWorkflow_button.BackColor = Color.LightCyan;
            }

            this.Refresh();
        }
    }
}