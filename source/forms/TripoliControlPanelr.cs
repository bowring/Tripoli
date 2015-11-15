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
        // added august 2011 to allow timer access
        public Timer RefreshView
        {
            get { return refreshView; }
        }
        private long timerStartTime;
        private long timerControlLengthTicks = 36000000000L; // ten-millionth of a second = 100 nanosections = 1 hour

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myTripoli"></param>
        public frmTripoliControlPanel(frmMainTripoli myTripoli)
        {
            this.myTripoli = myTripoli;
            InitializeComponent();
            SetDesktopLocation(myTripoli.DesktopLocation.X, myTripoli.DesktopLocation.Y);

            refreshView = new Timer();
            refreshView.Interval = 2500;
            // aug 2011 made default condition off
         //   refreshView.Start();
            refreshView.Tick += new EventHandler(Timer_Tick);

            timerStartTime = DateTime.Now.Ticks;

            currentDatFileName = "";
            sampleMetaDataFolder = null;

            // enable buttons
            startStopLiveWorkflow_button.Enabled = true;// (sampleMetaDataFolder != null);
            myTripoli.startStopLiveWorkflow_menuItem.Enabled = startStopLiveWorkflow_button.Enabled;
            toolTip1.SetToolTip(startStopLiveWorkflow_button, //
                "Starting Live Workflow causes Tripoli to automatically load the newest data file and to save the Tripoli Workfile (.trip).  "//
                 + "\n\nIf matching U-Pb_Redux - generated Sample metadata is present, then Tripoli automatically exports the XML file to the specified location.");
            toolTip1.ShowAlways = true;

            // first pass find and display newest live workflow filename
            tuneStatus();
        }

        public void Timer_Tick(object sender, EventArgs eArgs)
        {
            // reset timer to avoid leaks due to tripoli running several days
            if ((DateTime.Now.Ticks - timerStartTime) > timerControlLengthTicks)
            {
                ((Timer)sender).Stop();
                ((Timer)sender).Start();
            }

            if (sender == refreshView)
                tuneStatus();
        }


        public void tuneStatus()
        {
            FileInfo currentLiveWorkflowDataFile = locateCurrentLiveWorkflowDataFile();

            //// check for live update manager folder SampleMetaData
            //if (currentLiveWorkflowDataFile != null)
            //{
            //    DirectoryInfo[] parentFolder = currentLiveWorkflowDataFile.Directory.Parent.GetDirectories("SampleMetaData", SearchOption.TopDirectoryOnly);
            //    if (parentFolder.Length > 0)
            //    {

            // mar 2010 made SampleMetaDataFolder independent        
            try
            {
                SampleMetaDataFolder = new DirectoryInfo(TripoliRegistry.GetSampleMetaDataFolder());
            }
            catch (Exception)
            {
            }
            //    }
            //}

            loadNewestDataFile_button.Enabled = (currentLiveWorkflowDataFile != null);
            try
            {
                myTripoli.loadCurrentDatFile_menuItem.Enabled = loadNewestDataFile_button.Enabled;

            }
            catch (Exception)
            {
            }
            saveTripXML_button.Enabled = myTripoli.saveAsTripoliWorkFile_menuItem.Enabled;

            myTripoli.setSaveExportMenuItemsEnabled(saveTripXML_button.Enabled);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        public void setReportStatus(String status)
        {
            newestDataFileStatusLabel.Text = status;
            this.Refresh();
            try
            {
                myTripoli.loadCurrentDatFile_menuItem.Text = myTripoli.loadCurrentDatFile_menuItem.Tag + " (" + status + ")";

            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string getReportStatus()
        {
            return newestDataFileStatusLabel.Text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
                    liveWorkflowDataFolderName = TripoliRegistry.GetRecentThermoFinniganTritonLiveFolder();
                    break;

                case ".dat":
                    liveWorkflowDataFolderName = TripoliRegistry.GetRecentSector54LiveFolder();
                    break;

                case ".LiveDataStatus.txt":
                    liveWorkflowDataFolderName = TripoliRegistry.GetRecentIonVantageLiveDataStatusFileFolder();
                    break;

                default:
                    liveWorkflowDataFolderName = TripoliRegistry.GetRecentThermoFinniganMat261LiveFolder();
                    break;
            }


            try
            {
                DirectoryInfo currentLiveWorkflowDataFolder = new DirectoryInfo(liveWorkflowDataFolderName);
                if (currentLiveWorkflowDataFolder.Exists)
                {
                    // JULY 2011 modification for IonVantage software
                    // an extra layer is added for this scenario in that the file  LiveDataStatus.txt points to the
                    // current LIveDataFolder; in turn this text file can be changed and we need to listen
                    // and capture the change
                    // furthermore these files are distinct, whereas the the previous liveworkflows assumed a
                    // growing file ... the newest including everything from the past
                    if (dataTypeChoice.Equals(".LiveDataStatus.txt"))
                    {
                        string liveDataFolder = null;
                        // first detect if the file is present
                        FileInfo[] liveFiles = currentLiveWorkflowDataFolder.GetFiles("LiveDataStatus.txt", SearchOption.TopDirectoryOnly);
                        if (liveFiles.Length > 0)
                        {
                            // parse the file
                            // see email from Noah McLean July 12 2011
                            StreamReader liveDataStatus = new StreamReader(liveFiles[0].FullName);
                            string line = liveDataStatus.ReadLine();
                            // test first line
                            if (line.Contains("Version"))
                            {
                                // read 5 lines                          
                                for (int i = 0; i < 5; i++)
                                {
                                    line = liveDataStatus.ReadLine();
                                }
                                if (line.StartsWith("Method"))
                                {
                                    // proceed
                                    line = line.Replace("Method", "");
                                    line = line.Replace("\"", "");
                                    line = line.Replace(",", "");
                                    int rawSpot = line.ToUpper().IndexOf("RAW");
                                    liveDataFolder = line.Substring(0, rawSpot + 4) + "LiveData";
                                    // now redirect
                                    currentLiveWorkflowDataFolder = new DirectoryInfo(liveDataFolder);
                                    if (currentLiveWorkflowDataFolder.Exists)
                                    {
                                        // process the files
                                        // since Tripoli is being moved to U-Pb_Redux and java, we will merely fake out the live workflow mechanism
                                        // by sending the newest cycle file and having its containing folder processed
                                        FileInfo[] files = currentLiveWorkflowDataFolder.GetFiles("*" + ".txt", SearchOption.TopDirectoryOnly);
                                        Array.Sort<FileInfo>(files, new Comparison<FileInfo>(delegate(FileInfo f1, FileInfo f2)
                                        {
                                            // most recent first
                                            return DateTime.Compare(f2.LastWriteTimeUtc, f1.LastWriteTimeUtc);
                                        }));

                                        if (files.Length > 0)
                                        {
                                            retVal = files[0];

                                            // auto save and load this puppy if Live Workflow
                                                if (myTripoli.IsInLiveWorkflow && (TripoliRegistry.GetRecentLiveWorkflowFileAccessTime().CompareTo(retVal.LastWriteTimeUtc.ToBinary()) < 0))
                                            {
                                                myTripoli.loadCurrentLiveWorkflowDataFile(retVal.FullName);
                                            }
                                        }
                                    }
                                }

                                // close up
                                liveDataStatus.Close();
                            }
                        }
                    }

                    else
                    {
                        // assume that data files are in the top level , f.e. this is currentLiveWorkflowDataFolder
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
                            //Console.WriteLine("Times: stored = " + TripoliRegistry.GetRecentLiveWorkflowFileAccessTime().ToString() + "  retVal = " + retVal.LastWriteTimeUtc.ToBinary().ToString());
                            if (myTripoli.IsInLiveWorkflow && (TripoliRegistry.GetRecentLiveWorkflowFileAccessTime().CompareTo(retVal.LastWriteTimeUtc.ToBinary()) < 0))
                            {
                                myTripoli.loadCurrentLiveWorkflowDataFile(retVal.FullName);
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {
            }
            if (retVal == null)
                setReportStatus("No data files: " + ((liveWorkflowDataFolderName.Equals("")) ? "NONE" : liveWorkflowDataFolderName));
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

        public void Hide()
        {
            base.Hide();
            // August 2011  now handled by user
            //refreshView.Stop();
        }

        public void Show()
        {
            base.Show();
            // August 2011  now handled by user
           // refreshView.Start();
            locateCurrentLiveWorkflowDataFile();
        }

        private void startStopLiveWorkflow_button_Click(object sender, EventArgs e)
        {
            bool isLive = myTripoli.toggleLiveWorkflow();
        }

        public void tuneSaveExportButton(bool isLive)
        {
            if (isLive)
            {
                if (TripoliRegistry.GetIgnoreSampleMetaData().Equals("TRUE"))
                {
                    saveTripXML_button.Text = "Save / No Export";
                }
                else
                {
                    saveTripXML_button.Text = (string)saveTripXML_button.Tag;
                }
            }
            else
            {
                saveTripXML_button.Text = (string)saveTripXML_button.Tag;
            }

            this.Refresh();
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

        private void setDataFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Visible = false;
            frmWorkFlowFolder myLiveDataFolderForm = new frmWorkFlowFolder();
            myLiveDataFolderForm.ShowDialog();
            myLiveDataFolderForm.Dispose();
            Visible = true;
            tuneSaveExportButton(myTripoli.IsInLiveWorkflow);
            tuneStatus();
        }

        private void exportRatiosAsTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myTripoli.exportCheckedRatiosAsDelimitedText();
        }

        private void exportRatiosToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myTripoli.saveAndExportToClipboardForPbMacdat();

        }
    }
}