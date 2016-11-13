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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using System.Xml;
// Celexis licensing
////using TRXTD200;

using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.Xsl;
using Microsoft.Win32;
using Tripoli.earth_time_org;
using Tripoli.source.forms;
using Tripoli.utilities;
using Tripoli.vendors_data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.CompilerServices;
using Tripoli.source;

[assembly: RegistryPermissionAttribute(SecurityAction.RequestMinimum,
ViewAndModify = "HKEY_CURRENT_USER")]
namespace Tripoli
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class frmMainTripoli : System.Windows.Forms.Form
    {
        // possible constants
        private int CONTROLS_TOP_TOP_Y = 30;// vertical space for each fraction row
        private int RATIO_PANEL_TOP_MARGIN = 20;
        private int LINE_LENGTH_RATIO_NAME = 38;
        private int HEIGHT_OF_COMPONENT = 25;

        //      private int MRU_LIST_COUNT = 5;

        private string CCGainsFileNamePattern = "CC*.xls";
        private string StandardsFileNamePattern = "*.trip";

        // Fields
        private MassSpecDataFile CurrentDataFile = null;

        /// <summary>
        /// used for serialization
        /// </summary>
        public FileInfo TripoliFileInfo = null;

        /// <summary>
        /// the tracer selected for use in fractionation correction
        /// </summary>
        public Tracer TripoliTracer = null;
        /// <summary>
        /// 
        /// </summary>
        public BariumPhosphateIC TripoliBariumPhosphateIC = null;
        /// <summary>
        /// 
        /// </summary>
        public ThalliumIC TripoliThalliumIC = null;


        private RunParameters myParameters;
        private FileInfo CurrentDataFileInfo = null;

        private TripoliWorkProduct rawRatios = null;

        /// <summary>
        /// 
        /// </summary>
        public TripoliWorkProduct RawRatios
        {
            get { return rawRatios; }
            set { rawRatios = value; }
        }
        private frmRatioGraph[] ratioGraphs;
        private CheckBox[] ChooseItem;
        private Button[] GraphRatios;
        // Jan 2008
        private Button[] InvertRatios;

        // for CCGains declare tripoli object for history
        private TripoliWorkProduct currentGainsFolder = null;

        //// used to store Celexis code
        //private static string TripoliLeaseNumber = "NONE";
        //private static int TripoliLeaseDaysRemaining = 0;

        private Panel panelDisplaySummary = null;

        // flag whether to show checked and unchecked data items on panel
        private bool ShowAll = true;

        // used for tiling
        private GraphPageDisplay allGraphs = null;

        // used to open excel
        private Excel.Application ExcelObj = null;

        private String reportOfPbMacDat;

        public String ReportOfPbMacDat
        {
            get { return reportOfPbMacDat; }
            set { reportOfPbMacDat = value; }
        }

        private Form myTCP;

        private bool isInLiveWorkflow;

        public bool IsInLiveWorkflow
        {
            get { return isInLiveWorkflow; }
            set { isInLiveWorkflow = value; }
        }


        #region System Fields

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuHelp;
        private System.Windows.Forms.MenuItem menuItemAbout;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private MenuItem saveTripoliWorkFile_menuItem;
        public MenuItem saveAsTripoliWorkFile_menuItem;
        private System.Windows.Forms.MenuItem menuItem13;
        private System.Windows.Forms.MenuItem menuItemExitTripoli;
        private System.Windows.Forms.Button btnShowChecked;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnSelectUserFunctions;
        private System.Windows.Forms.Button btnTileSelected;
        private System.Windows.Forms.Button btnSelectRatios;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.MenuItem menuItemOpenTripWorkFile;
        private System.Windows.Forms.MenuItem menuAnnotate;
        private System.Windows.Forms.Panel pnlAnnotate;
        private System.Windows.Forms.TextBox txtNotations;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblTripoliWithVersion;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.MenuItem menuWorkFile;
        private System.Windows.Forms.MenuItem menuDataFile;
        private System.Windows.Forms.Panel pnlIntro;
        private System.Windows.Forms.Label lblStandBy;
        private System.Windows.Forms.MenuItem menuWorkFileCloseFiles;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel ButtonPanelHistory;
        private System.Windows.Forms.Button btnSaveHistory;
        private System.Windows.Forms.Button btnSelectNewFiles;
        private System.Windows.Forms.Button btnSelectSaved;
        private System.Windows.Forms.Button btnSelectNoneFiles;
        private System.Windows.Forms.Button btnSelectAllFiles;
        private System.Windows.Forms.Button btnShowCollectors;
        private System.Windows.Forms.Panel buttonPanelCollectors;
        private System.Windows.Forms.Button btnShowGainsFolder;
        private System.Windows.Forms.Button btnTileSelectedCollectors;
        private System.Windows.Forms.Button btnSelectNoneCollectors;
        private System.Windows.Forms.Button btnSelectAllCollectors;
        private System.Windows.Forms.Button btnSaveCollectorHistory;
        private System.Windows.Forms.Panel ButtonPanelRatios;
        private System.Windows.Forms.MenuItem mnuRecentTWF;
        private System.Windows.Forms.MenuItem mnu_mruWF1;
        private System.Windows.Forms.MenuItem mnu_mruWF2;
        private System.Windows.Forms.MenuItem mnuOpenTHF;
        private System.Windows.Forms.MenuItem mnuSaveTHF;
        private System.Windows.Forms.MenuItem mnuSaveAsTHF;
        private System.Windows.Forms.MenuItem mnuRecentTHF;
        private System.Windows.Forms.MenuItem mnu_mruHF1;
        private System.Windows.Forms.MenuItem mnu_mruHF2;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.MenuItem mnu_mruWF3;
        private System.Windows.Forms.MenuItem mnu_mruWF4;
        private System.Windows.Forms.MenuItem mnu_mruWF5;
        private System.Windows.Forms.MenuItem mnu_mruHF3;
        private System.Windows.Forms.MenuItem mnu_mruHF4;
        private System.Windows.Forms.MenuItem mnu_mruHF5;
        private System.Windows.Forms.MenuItem menuSettings;
        private System.Windows.Forms.MenuItem mnuSettingsGains;
        private System.Windows.Forms.MenuItem menuControlPanel;
        public MenuItem saveTripAndExportToRedux_menuItem;
        private System.Windows.Forms.MenuItem saveExportToTextFile_menuItem;
        private MenuItem menuResources;
        private MenuItem menuItemETOWebSite;
        private MenuItem menuItemDataDictionary;
        private MenuItem menuItemChauvenet;
        private MenuItem menuItemUSampleComponents;
        private MenuItem menuItemHelpText;
        private MenuItem menuItemRevisionHistory;
        private MenuItem menuItemCheckForUpdates;
        private MenuItem menuItem4;
        private MenuItem menuItem6;
        private StatusStrip statusBar1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private MenuItem menuItemLaunchKwikiExporter;
        private Label label3;
        private MenuItem menuCorrections;
        private MenuItem menuTracer;
        private MenuItem menuImportTracerFromETO;
        private MenuItem menuItemImportTracer;
        private MenuItem menuItem9;
        private MenuItem menuItemShowActiveTracerAsForm;
        private MenuItem menuItemShowActiveTracerAsText;
        private MenuItem menuItemShowActiveTracerAsHTML;
        private MenuItem menuItem5;
        private MenuItem menuBaPO2IC;
        private MenuItem menuTlIC;
        private MenuItem menuItemActivateDefaultEARTHTIMEBaPO2IC;
        private MenuItem menuItemCreateNewTracer;
        private MenuItem menuItemSaveTracerLocally;
        private MenuItem menuItemEditLocalTracer;
        private MenuItem menuItemActivateDefaultEARTHTIMETlIC;
        private MenuItem menuItem7;
        private MenuItem menuItemCorrectFraction;
        private MenuItem menuItemShowConfirmList;
        private MenuItem menuItemRemoveCorrection;
        private MenuItem menuItem8;
        private Panel panelCorrections;
        private CheckBox chkBoxUseIC;
        private CheckBox chkBoxUseTracer;
        private Label lblAppliedTlIC;
        private Label lblActiveTlIC;
        private Label lblAppliedBaPO2IC;
        private Label lblActiveBaPO2IC;
        private Button btnFractionationCorrection;
        private Label lblAppliedTracer;
        private Label lblActiveTracer;
        private Timer timerForLiveUpdate;
        private MenuItem menuItemOxideCorrection;
        private MenuItem menuItem10;
        private Label lblOxideCorrectionValue;
        public MenuItem startStopLiveWorkflow_menuItem;
        private MenuItem saveExportToClipboardForPbMacDat_menuItem;
        public MenuItem loadCurrentDatFile_menuItem;
        private MenuItem BaPO2TlIsotopicComposition_menuItem;
        private MenuItem menuItem1;
        private MenuItem menuItemShowActiveBaPO2;
        private MenuItem menuItem11;
        private MenuItem menuItemShowActiveTlIC;
        private MenuItem menuItem3;
        private PictureBox pictureBox2;
        private Label label1;
        private MenuItem menuItem15;
        private MenuItem menuItem12;
        private MenuItem setLiveWorkflowDataFolder_menuItem;
        private MenuItem menuItem17;
        private MenuItem menuItemCredits;
        private MenuItem menuItem21;
        private MenuItem menuReadSector54DatFile;
        private MenuItem menuItem23;
        private MenuItem menuItem24;
        private MenuItem menuItem25;
        private MenuItem menuItem27;
        private MenuItem menuItemReadGVGainsFolder;
        private MenuItem menuItemReadStandardsFolder;
        private MenuItem menuItemReadIDTIMS_ImportedCSV;
        private MenuItem menuTools;
        private MenuItem menuItemOpenIDTIMS_CSV_Template;
        private MenuItem menuItem16;
        private MenuItem menuItemApplyTracerToReCorrectFractions;
        private MenuItem menuItem14;
        private MenuItem menuItemReadIonVantageFolder;
        private MenuItem menuItem20;
        private MenuItem menuItemAppendIonVantageFolder;
        private MenuItem version47ReleaseNotesMenuItem;
        private MenuItem CIRDLESonGitHubMenuItem;
        private System.ComponentModel.IContainer components;
        #endregion System Fields

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TripoliFile"></param>
        public frmMainTripoli(string TripoliFile)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            toolTip1.SetToolTip(statusBar1, "Dbl-Click to open source file in native application.");

            SetStatusBar("NONE");

            pnlAnnotate.Location = new Point(110, 1);
            pnlAnnotate.Width = 378;
            pnlAnnotate.Height = 168;

            // button panels in same location
            this.ButtonPanelHistory.Location = this.ButtonPanelRatios.Location;
            this.buttonPanelCollectors.Location = this.ButtonPanelRatios.Location;

            // control pnlIntro on size change
            //pnlIntro.Anchor = (AnchorStyles)15;
            pnlIntro.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right);
            pnlIntro.AutoScroll = true;
            /*
             *          JANUARY 2008 CONVERT TO SHAREWARE
                        // ******************************************************************************
                        // july 2005 moved this logic here to help with old trialx dll button text display
                        if (new TripoliLicense().CheckLicense())
                        {
                            TripoliLeaseNumber = TripoliLicense.TripoliLeaseNumber;
                            TripoliLeaseDaysRemaining = TripoliLicense.TripoliLeaseDaysRemaining;
                        }
                        else
                        {
                            throw new System.Exception("\n\nYou have cancelled the startup of Tripoli!");
                        }
                        // *******************************************************************************
            */

            // nov 2009
            // set bounds for use by tripoli control panel, as main frame is centered and does not set bounds
            SetBounds(400, 400, 0, 0, BoundsSpecified.Location);

            myTCP = new frmTripoliControlPanel(this);
            //   launchTripoliControlPanel();

            // set defaults built in can be changed by user
            TripoliBariumPhosphateIC = BariumPhosphateIC.EARTHTIME_BaPO2IC();
            TripoliThalliumIC = ThalliumIC.EARTHTIME_TlIC();

            panelCorrections.Visible = false;

            // aug 2011
            updateTitleBarForLiveWorkFlow();

        }

        private void OpenFile(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);

            if (fi.Exists)
            {
                if (fi.Extension.ToLower().CompareTo(".trip") == 0)
                {
                    OpenTripoliWorkFile(fi.FullName);
                }
                else if (fi.Extension.ToLower().CompareTo(".triphis") == 0)
                {
                    OpenTripoliHistoryFile(fi.FullName);
                }
                // Micromass excel
                else if (fi.Extension.ToLower().CompareTo(".xls") == 0)
                {
                    OpenIsoprobXExcelFile(fi.FullName);
                }
                // sector 54
                else if (fi.Extension.ToLower().CompareTo(".dat") == 0)
                {
                    OpenSector54DatFile(fi.FullName);
                }
                // Thermo-Finnigan Triton
                else if (fi.Extension.ToLower().CompareTo(".exp") == 0)
                {
                    OpenThermoFinniganTritonExpFile(fi.FullName);
                }
                else
                {
                    MessageBox.Show(
                        "Tripoli is not able to open this file...",
                        "Tripoli Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(fileName);
                if (di.Exists)
                {
                    if (((FileInfo[])di.GetFiles("Standard.triphis")).Length > 0)
                    {
                        // we have a standards folder
                        openStandardsFolder(di.FullName);
                    }
                    else if (((FileInfo[])di.GetFiles("CCGains.triphis")).Length > 0)
                    {
                        // we have a gains folder
                        OpenGainsFolder(di.FullName);
                    }
                    else
                    {
                        MessageBox.Show(
                            "Tripoli cannot determine if this is a Standards or Gains Folder\n"
                            + "... Please use the menu to open it.",
                            "Tripoli Warning",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }
            }
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainTripoli));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuWorkFile = new System.Windows.Forms.MenuItem();
            this.menuItemOpenTripWorkFile = new System.Windows.Forms.MenuItem();
            this.saveTripoliWorkFile_menuItem = new System.Windows.Forms.MenuItem();
            this.saveAsTripoliWorkFile_menuItem = new System.Windows.Forms.MenuItem();
            this.mnuRecentTWF = new System.Windows.Forms.MenuItem();
            this.mnu_mruWF1 = new System.Windows.Forms.MenuItem();
            this.mnu_mruWF2 = new System.Windows.Forms.MenuItem();
            this.mnu_mruWF3 = new System.Windows.Forms.MenuItem();
            this.mnu_mruWF4 = new System.Windows.Forms.MenuItem();
            this.mnu_mruWF5 = new System.Windows.Forms.MenuItem();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.mnuOpenTHF = new System.Windows.Forms.MenuItem();
            this.mnuSaveTHF = new System.Windows.Forms.MenuItem();
            this.mnuSaveAsTHF = new System.Windows.Forms.MenuItem();
            this.mnuRecentTHF = new System.Windows.Forms.MenuItem();
            this.mnu_mruHF1 = new System.Windows.Forms.MenuItem();
            this.mnu_mruHF2 = new System.Windows.Forms.MenuItem();
            this.mnu_mruHF3 = new System.Windows.Forms.MenuItem();
            this.mnu_mruHF4 = new System.Windows.Forms.MenuItem();
            this.mnu_mruHF5 = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuWorkFileCloseFiles = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItemExitTripoli = new System.Windows.Forms.MenuItem();
            this.menuDataFile = new System.Windows.Forms.MenuItem();
            this.menuItem21 = new System.Windows.Forms.MenuItem();
            this.menuReadSector54DatFile = new System.Windows.Forms.MenuItem();
            this.menuItem23 = new System.Windows.Forms.MenuItem();
            this.menuItem24 = new System.Windows.Forms.MenuItem();
            this.menuItemReadIDTIMS_ImportedCSV = new System.Windows.Forms.MenuItem();
            this.menuItem25 = new System.Windows.Forms.MenuItem();
            this.menuItemReadIonVantageFolder = new System.Windows.Forms.MenuItem();
            this.menuItemAppendIonVantageFolder = new System.Windows.Forms.MenuItem();
            this.menuItem20 = new System.Windows.Forms.MenuItem();
            this.menuItemReadGVGainsFolder = new System.Windows.Forms.MenuItem();
            this.menuItem27 = new System.Windows.Forms.MenuItem();
            this.menuItemReadStandardsFolder = new System.Windows.Forms.MenuItem();
            this.menuSettings = new System.Windows.Forms.MenuItem();
            this.menuItemChauvenet = new System.Windows.Forms.MenuItem();
            this.menuItemUSampleComponents = new System.Windows.Forms.MenuItem();
            this.BaPO2TlIsotopicComposition_menuItem = new System.Windows.Forms.MenuItem();
            this.mnuSettingsGains = new System.Windows.Forms.MenuItem();
            this.menuCorrections = new System.Windows.Forms.MenuItem();
            this.menuTracer = new System.Windows.Forms.MenuItem();
            this.menuImportTracerFromETO = new System.Windows.Forms.MenuItem();
            this.menuItemImportTracer = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItemApplyTracerToReCorrectFractions = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.menuItemShowActiveTracerAsForm = new System.Windows.Forms.MenuItem();
            this.menuItemShowActiveTracerAsText = new System.Windows.Forms.MenuItem();
            this.menuItemShowActiveTracerAsHTML = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItemCreateNewTracer = new System.Windows.Forms.MenuItem();
            this.menuItemSaveTracerLocally = new System.Windows.Forms.MenuItem();
            this.menuItemEditLocalTracer = new System.Windows.Forms.MenuItem();
            this.menuBaPO2IC = new System.Windows.Forms.MenuItem();
            this.menuItemActivateDefaultEARTHTIMEBaPO2IC = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemShowActiveBaPO2 = new System.Windows.Forms.MenuItem();
            this.menuTlIC = new System.Windows.Forms.MenuItem();
            this.menuItemActivateDefaultEARTHTIMETlIC = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.menuItemShowActiveTlIC = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItemCorrectFraction = new System.Windows.Forms.MenuItem();
            this.menuItemShowConfirmList = new System.Windows.Forms.MenuItem();
            this.menuItemRemoveCorrection = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItemOxideCorrection = new System.Windows.Forms.MenuItem();
            this.menuAnnotate = new System.Windows.Forms.MenuItem();
            this.menuControlPanel = new System.Windows.Forms.MenuItem();
            this.menuItemLaunchKwikiExporter = new System.Windows.Forms.MenuItem();
            this.menuItem15 = new System.Windows.Forms.MenuItem();
            this.startStopLiveWorkflow_menuItem = new System.Windows.Forms.MenuItem();
            this.loadCurrentDatFile_menuItem = new System.Windows.Forms.MenuItem();
            this.saveTripAndExportToRedux_menuItem = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.setLiveWorkflowDataFolder_menuItem = new System.Windows.Forms.MenuItem();
            this.menuItem17 = new System.Windows.Forms.MenuItem();
            this.saveExportToClipboardForPbMacDat_menuItem = new System.Windows.Forms.MenuItem();
            this.saveExportToTextFile_menuItem = new System.Windows.Forms.MenuItem();
            this.menuResources = new System.Windows.Forms.MenuItem();
            this.menuTools = new System.Windows.Forms.MenuItem();
            this.menuItemOpenIDTIMS_CSV_Template = new System.Windows.Forms.MenuItem();
            this.menuItem16 = new System.Windows.Forms.MenuItem();
            this.menuItemETOWebSite = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItemDataDictionary = new System.Windows.Forms.MenuItem();
            this.CIRDLESonGitHubMenuItem = new System.Windows.Forms.MenuItem();
            this.menuHelp = new System.Windows.Forms.MenuItem();
            this.menuItemHelpText = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.version47ReleaseNotesMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItemRevisionHistory = new System.Windows.Forms.MenuItem();
            this.menuItemCheckForUpdates = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItemAbout = new System.Windows.Forms.MenuItem();
            this.menuItemCredits = new System.Windows.Forms.MenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.ButtonPanelRatios = new System.Windows.Forms.Panel();
            this.btnSelectRatios = new System.Windows.Forms.Button();
            this.btnTileSelected = new System.Windows.Forms.Button();
            this.btnSelectUserFunctions = new System.Windows.Forms.Button();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnShowChecked = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlAnnotate = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.txtNotations = new System.Windows.Forms.TextBox();
            this.pnlIntro = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblStandBy = new System.Windows.Forms.Label();
            this.lblTripoliWithVersion = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.ButtonPanelHistory = new System.Windows.Forms.Panel();
            this.btnShowCollectors = new System.Windows.Forms.Button();
            this.btnSelectNewFiles = new System.Windows.Forms.Button();
            this.btnSelectSaved = new System.Windows.Forms.Button();
            this.btnSelectNoneFiles = new System.Windows.Forms.Button();
            this.btnSelectAllFiles = new System.Windows.Forms.Button();
            this.btnSaveHistory = new System.Windows.Forms.Button();
            this.buttonPanelCollectors = new System.Windows.Forms.Panel();
            this.btnSaveCollectorHistory = new System.Windows.Forms.Button();
            this.btnShowGainsFolder = new System.Windows.Forms.Button();
            this.btnTileSelectedCollectors = new System.Windows.Forms.Button();
            this.btnSelectNoneCollectors = new System.Windows.Forms.Button();
            this.btnSelectAllCollectors = new System.Windows.Forms.Button();
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.statusBar1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelCorrections = new System.Windows.Forms.Panel();
            this.lblOxideCorrectionValue = new System.Windows.Forms.Label();
            this.chkBoxUseIC = new System.Windows.Forms.CheckBox();
            this.chkBoxUseTracer = new System.Windows.Forms.CheckBox();
            this.lblAppliedTlIC = new System.Windows.Forms.Label();
            this.lblActiveTlIC = new System.Windows.Forms.Label();
            this.lblAppliedBaPO2IC = new System.Windows.Forms.Label();
            this.lblActiveBaPO2IC = new System.Windows.Forms.Label();
            this.btnFractionationCorrection = new System.Windows.Forms.Button();
            this.lblAppliedTracer = new System.Windows.Forms.Label();
            this.lblActiveTracer = new System.Windows.Forms.Label();
            this.timerForLiveUpdate = new System.Windows.Forms.Timer(this.components);
            this.ButtonPanelRatios.SuspendLayout();
            this.pnlAnnotate.SuspendLayout();
            this.pnlIntro.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.ButtonPanelHistory.SuspendLayout();
            this.buttonPanelCollectors.SuspendLayout();
            this.statusBar1.SuspendLayout();
            this.panelCorrections.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuWorkFile,
            this.menuDataFile,
            this.menuSettings,
            this.menuCorrections,
            this.menuAnnotate,
            this.menuControlPanel,
            this.menuResources,
            this.menuHelp});
            // 
            // menuWorkFile
            // 
            this.menuWorkFile.Index = 0;
            this.menuWorkFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemOpenTripWorkFile,
            this.saveTripoliWorkFile_menuItem,
            this.saveAsTripoliWorkFile_menuItem,
            this.mnuRecentTWF,
            this.menuItem13,
            this.mnuOpenTHF,
            this.mnuSaveTHF,
            this.mnuSaveAsTHF,
            this.mnuRecentTHF,
            this.menuItem8,
            this.menuWorkFileCloseFiles,
            this.menuItem2,
            this.menuItemExitTripoli});
            this.menuWorkFile.Text = "Tripoli File";
            // 
            // menuItemOpenTripWorkFile
            // 
            this.menuItemOpenTripWorkFile.Index = 0;
            this.menuItemOpenTripWorkFile.Text = "Open Tripoli WorkFile";
            this.menuItemOpenTripWorkFile.Click += new System.EventHandler(this.menuItemOpenTripWorkFile_Click);
            // 
            // saveTripoliWorkFile_menuItem
            // 
            this.saveTripoliWorkFile_menuItem.Enabled = false;
            this.saveTripoliWorkFile_menuItem.Index = 1;
            this.saveTripoliWorkFile_menuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.saveTripoliWorkFile_menuItem.Text = "Save Tripoli WorkFile";
            this.saveTripoliWorkFile_menuItem.Click += new System.EventHandler(this.menuItem11_Click);
            // 
            // saveAsTripoliWorkFile_menuItem
            // 
            this.saveAsTripoliWorkFile_menuItem.Enabled = false;
            this.saveAsTripoliWorkFile_menuItem.Index = 2;
            this.saveAsTripoliWorkFile_menuItem.Text = "SaveAs Tripoli WorkFile";
            this.saveAsTripoliWorkFile_menuItem.Click += new System.EventHandler(this.saveAsTripoliWorkFile_menuItem_Click);
            // 
            // mnuRecentTWF
            // 
            this.mnuRecentTWF.Index = 3;
            this.mnuRecentTWF.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnu_mruWF1,
            this.mnu_mruWF2,
            this.mnu_mruWF3,
            this.mnu_mruWF4,
            this.mnu_mruWF5});
            this.mnuRecentTWF.Text = "Recent Tripoli WorkFiles ";
            this.mnuRecentTWF.Select += new System.EventHandler(this.mnuRecentTWF_Select);
            // 
            // mnu_mruWF1
            // 
            this.mnu_mruWF1.Index = 0;
            this.mnu_mruWF1.Text = "mruWF1";
            this.mnu_mruWF1.Click += new System.EventHandler(this.mnu_mruWF_Click);
            // 
            // mnu_mruWF2
            // 
            this.mnu_mruWF2.Index = 1;
            this.mnu_mruWF2.Text = "mruWF2";
            this.mnu_mruWF2.Click += new System.EventHandler(this.mnu_mruWF_Click);
            // 
            // mnu_mruWF3
            // 
            this.mnu_mruWF3.Index = 2;
            this.mnu_mruWF3.Text = "mruWF3";
            this.mnu_mruWF3.Click += new System.EventHandler(this.mnu_mruWF_Click);
            // 
            // mnu_mruWF4
            // 
            this.mnu_mruWF4.Index = 3;
            this.mnu_mruWF4.Text = "mruWF4";
            this.mnu_mruWF4.Click += new System.EventHandler(this.mnu_mruWF_Click);
            // 
            // mnu_mruWF5
            // 
            this.mnu_mruWF5.Index = 4;
            this.mnu_mruWF5.Text = "mruWF5";
            this.mnu_mruWF5.Click += new System.EventHandler(this.mnu_mruWF_Click);
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 4;
            this.menuItem13.Text = "-";
            // 
            // mnuOpenTHF
            // 
            this.mnuOpenTHF.Index = 5;
            this.mnuOpenTHF.Text = "Open Tripoli HistoryFile";
            this.mnuOpenTHF.Click += new System.EventHandler(this.mnuOpenTHF_Click);
            // 
            // mnuSaveTHF
            // 
            this.mnuSaveTHF.Enabled = false;
            this.mnuSaveTHF.Index = 6;
            this.mnuSaveTHF.Text = "Save Tripoli HistoryFile";
            this.mnuSaveTHF.Click += new System.EventHandler(this.mnuSaveTHF_Click);
            // 
            // mnuSaveAsTHF
            // 
            this.mnuSaveAsTHF.Enabled = false;
            this.mnuSaveAsTHF.Index = 7;
            this.mnuSaveAsTHF.Text = "SaveAs Tripoli HistoryFile";
            this.mnuSaveAsTHF.Click += new System.EventHandler(this.mnuSaveAsTHF_Click);
            // 
            // mnuRecentTHF
            // 
            this.mnuRecentTHF.Index = 8;
            this.mnuRecentTHF.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnu_mruHF1,
            this.mnu_mruHF2,
            this.mnu_mruHF3,
            this.mnu_mruHF4,
            this.mnu_mruHF5});
            this.mnuRecentTHF.Text = "Recent Tripoli HistoryFiles";
            this.mnuRecentTHF.Select += new System.EventHandler(this.mnuRecentTHF_Select);
            // 
            // mnu_mruHF1
            // 
            this.mnu_mruHF1.Index = 0;
            this.mnu_mruHF1.Text = "mruHF1";
            this.mnu_mruHF1.Click += new System.EventHandler(this.mnu_mruHF_Click);
            // 
            // mnu_mruHF2
            // 
            this.mnu_mruHF2.Index = 1;
            this.mnu_mruHF2.Text = "mruHF2";
            this.mnu_mruHF2.Click += new System.EventHandler(this.mnu_mruHF_Click);
            // 
            // mnu_mruHF3
            // 
            this.mnu_mruHF3.Index = 2;
            this.mnu_mruHF3.Text = "mruHF3";
            this.mnu_mruHF3.Click += new System.EventHandler(this.mnu_mruHF_Click);
            // 
            // mnu_mruHF4
            // 
            this.mnu_mruHF4.Index = 3;
            this.mnu_mruHF4.Text = "mruHF4";
            this.mnu_mruHF4.Click += new System.EventHandler(this.mnu_mruHF_Click);
            // 
            // mnu_mruHF5
            // 
            this.mnu_mruHF5.Index = 4;
            this.mnu_mruHF5.Text = "mruHF5";
            this.mnu_mruHF5.Click += new System.EventHandler(this.mnu_mruHF_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 9;
            this.menuItem8.Text = "-";
            // 
            // menuWorkFileCloseFiles
            // 
            this.menuWorkFileCloseFiles.Enabled = false;
            this.menuWorkFileCloseFiles.Index = 10;
            this.menuWorkFileCloseFiles.Text = "Close Tripoli File";
            this.menuWorkFileCloseFiles.Click += new System.EventHandler(this.menuWorkFileCloseFiles_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 11;
            this.menuItem2.Text = "-";
            // 
            // menuItemExitTripoli
            // 
            this.menuItemExitTripoli.Index = 12;
            this.menuItemExitTripoli.Text = "Exit Tripoli";
            this.menuItemExitTripoli.Click += new System.EventHandler(this.menuItem14_Click);
            // 
            // menuDataFile
            // 
            this.menuDataFile.Index = 1;
            this.menuDataFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem21,
            this.menuReadSector54DatFile,
            this.menuItem23,
            this.menuItem24,
            this.menuItemReadIDTIMS_ImportedCSV,
            this.menuItem25,
            this.menuItemReadIonVantageFolder,
            this.menuItemAppendIonVantageFolder,
            this.menuItem20,
            this.menuItemReadGVGainsFolder,
            this.menuItem27,
            this.menuItemReadStandardsFolder});
            this.menuDataFile.Text = "ID TIMS Data";
            this.menuDataFile.Click += new System.EventHandler(this.menuDataFile_Click);
            // 
            // menuItem21
            // 
            this.menuItem21.Index = 0;
            this.menuItem21.Text = "Read IsotopX \'.xls\' Data File";
            this.menuItem21.Click += new System.EventHandler(this.menuReadMassLynxExcel_Click);
            // 
            // menuReadSector54DatFile
            // 
            this.menuReadSector54DatFile.Index = 1;
            this.menuReadSector54DatFile.Text = "Read Sector54 \'.dat\' Data File";
            this.menuReadSector54DatFile.Click += new System.EventHandler(this.menuReadSector54DatFile_Click);
            // 
            // menuItem23
            // 
            this.menuItem23.Index = 2;
            this.menuItem23.Text = "Read Thermo-Finnigan Triton \'.exp\' Data File";
            this.menuItem23.Click += new System.EventHandler(this.menuReadThermoFinniganTriton_Click);
            // 
            // menuItem24
            // 
            this.menuItem24.Index = 3;
            this.menuItem24.Text = "Read Thermo-Finnigan Mat 261 and 262 \'.txt\' Data File";
            this.menuItem24.Click += new System.EventHandler(this.menuReadThermoFinniganMat261TxtFile_Click);
            // 
            // menuItemReadIDTIMS_ImportedCSV
            // 
            this.menuItemReadIDTIMS_ImportedCSV.Index = 4;
            this.menuItemReadIDTIMS_ImportedCSV.Text = "Read Data from \".csv\" import file";
            this.menuItemReadIDTIMS_ImportedCSV.Click += new System.EventHandler(this.menuItemReadIDTIMS_ImportedCSV_Click);
            // 
            // menuItem25
            // 
            this.menuItem25.Index = 5;
            this.menuItem25.Text = "-";
            // 
            // menuItemReadIonVantageFolder
            // 
            this.menuItemReadIonVantageFolder.Index = 6;
            this.menuItemReadIonVantageFolder.Text = "Read IonVantage Folder of Cycle Data Files (.txt)";
            this.menuItemReadIonVantageFolder.Click += new System.EventHandler(this.menuItemReadIonVantageFolder_click);
            // 
            // menuItemAppendIonVantageFolder
            // 
            this.menuItemAppendIonVantageFolder.Enabled = false;
            this.menuItemAppendIonVantageFolder.Index = 7;
            this.menuItemAppendIonVantageFolder.Text = "Append IonVantage Folder of Cycle Data Files (.txt)";
            this.menuItemAppendIonVantageFolder.Click += new System.EventHandler(this.menuItemAppendIonVantageFolder_click);
            // 
            // menuItem20
            // 
            this.menuItem20.Index = 8;
            this.menuItem20.Text = "-";
            // 
            // menuItemReadGVGainsFolder
            // 
            this.menuItemReadGVGainsFolder.Index = 9;
            this.menuItemReadGVGainsFolder.Text = "Read Folder of GV Gains Excel files";
            this.menuItemReadGVGainsFolder.Click += new System.EventHandler(this.menuReadGVGainsFolder_Click);
            // 
            // menuItem27
            // 
            this.menuItem27.Index = 10;
            this.menuItem27.Text = "-";
            // 
            // menuItemReadStandardsFolder
            // 
            this.menuItemReadStandardsFolder.Index = 11;
            this.menuItemReadStandardsFolder.Text = "Read Folder of Tripolized Standards";
            this.menuItemReadStandardsFolder.Click += new System.EventHandler(this.menuReadStandardsFolder_Click);
            // 
            // menuSettings
            // 
            this.menuSettings.Index = 2;
            this.menuSettings.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemChauvenet,
            this.menuItemUSampleComponents,
            this.BaPO2TlIsotopicComposition_menuItem,
            this.mnuSettingsGains});
            this.menuSettings.Text = "Parameters";
            this.menuSettings.Popup += new System.EventHandler(this.ProcessSettingsMenu);
            // 
            // menuItemChauvenet
            // 
            this.menuItemChauvenet.Index = 0;
            this.menuItemChauvenet.Text = "Chauvenet\'s Criterion";
            this.menuItemChauvenet.Click += new System.EventHandler(this.menuItemChauvenet_Click);
            // 
            // menuItemUSampleComponents
            // 
            this.menuItemUSampleComponents.Enabled = false;
            this.menuItemUSampleComponents.Index = 1;
            this.menuItemUSampleComponents.Text = "U Sample Components";
            this.menuItemUSampleComponents.Click += new System.EventHandler(this.menuItemUsampleComponents_Click);
            // 
            // BaPO2TlIsotopicComposition_menuItem
            // 
            this.BaPO2TlIsotopicComposition_menuItem.Index = 2;
            this.BaPO2TlIsotopicComposition_menuItem.Text = "BaPO2 and Tl Isotopic Composition";
            this.BaPO2TlIsotopicComposition_menuItem.Click += new System.EventHandler(this.BaPO2TlIsotopicComposition_menuItem_Click);
            // 
            // mnuSettingsGains
            // 
            this.mnuSettingsGains.Index = 3;
            this.mnuSettingsGains.Text = "Gains";
            this.mnuSettingsGains.Click += new System.EventHandler(this.mnuSettingsGains_Click);
            // 
            // menuCorrections
            // 
            this.menuCorrections.Index = 3;
            this.menuCorrections.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuTracer,
            this.menuBaPO2IC,
            this.menuTlIC,
            this.menuItem7,
            this.menuItemCorrectFraction,
            this.menuItemShowConfirmList,
            this.menuItemRemoveCorrection,
            this.menuItem10,
            this.menuItemOxideCorrection});
            this.menuCorrections.Text = "Corrections";
            this.menuCorrections.Popup += new System.EventHandler(this.ProcessCorrectionsMenu);
            // 
            // menuTracer
            // 
            this.menuTracer.Index = 0;
            this.menuTracer.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuImportTracerFromETO,
            this.menuItemImportTracer,
            this.menuItem9,
            this.menuItemApplyTracerToReCorrectFractions,
            this.menuItem14,
            this.menuItemShowActiveTracerAsForm,
            this.menuItemShowActiveTracerAsText,
            this.menuItemShowActiveTracerAsHTML,
            this.menuItem5,
            this.menuItemCreateNewTracer,
            this.menuItemSaveTracerLocally,
            this.menuItemEditLocalTracer});
            this.menuTracer.Text = "Tracer";
            this.menuTracer.Popup += new System.EventHandler(this.ProcessTracerMenu);
            // 
            // menuImportTracerFromETO
            // 
            this.menuImportTracerFromETO.Index = 0;
            this.menuImportTracerFromETO.Text = "Open and Activate Tracer from EARTHTIME";
            this.menuImportTracerFromETO.Click += new System.EventHandler(this.menuImportTracerFromETO_Click);
            // 
            // menuItemImportTracer
            // 
            this.menuItemImportTracer.Index = 1;
            this.menuItemImportTracer.Text = "Open and Activate Tracer from local directory";
            this.menuItemImportTracer.Click += new System.EventHandler(this.menuItemImportTracer_Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 2;
            this.menuItem9.Text = "-";
            // 
            // menuItemApplyTracerToReCorrectFractions
            // 
            this.menuItemApplyTracerToReCorrectFractions.Index = 3;
            this.menuItemApplyTracerToReCorrectFractions.Text = "Apply Active Tracer to Re-Correct Tripolized Fractions";
            this.menuItemApplyTracerToReCorrectFractions.Click += new System.EventHandler(this.menuItemApplyTracerToReCorrectFractions_Click);
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 4;
            this.menuItem14.Text = "-";
            // 
            // menuItemShowActiveTracerAsForm
            // 
            this.menuItemShowActiveTracerAsForm.Index = 5;
            this.menuItemShowActiveTracerAsForm.Text = "Show Active Tracer in a Tripoli window";
            this.menuItemShowActiveTracerAsForm.Click += new System.EventHandler(this.menuItemShowSelectedTracer_Click);
            // 
            // menuItemShowActiveTracerAsText
            // 
            this.menuItemShowActiveTracerAsText.Index = 6;
            this.menuItemShowActiveTracerAsText.Text = "Show Active Tracer as a text file";
            this.menuItemShowActiveTracerAsText.Click += new System.EventHandler(this.menuItemShowActiveTracerAsText_Click);
            // 
            // menuItemShowActiveTracerAsHTML
            // 
            this.menuItemShowActiveTracerAsHTML.Index = 7;
            this.menuItemShowActiveTracerAsHTML.Text = "Show Active Tracer as a html file";
            this.menuItemShowActiveTracerAsHTML.Click += new System.EventHandler(this.menuItemShowActiveTracerAsHTML_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 8;
            this.menuItem5.Text = "-";
            // 
            // menuItemCreateNewTracer
            // 
            this.menuItemCreateNewTracer.Index = 9;
            this.menuItemCreateNewTracer.Text = "Create a New Tracer and Save as local file";
            this.menuItemCreateNewTracer.Click += new System.EventHandler(this.menuItemCreateNewTracer_Click);
            // 
            // menuItemSaveTracerLocally
            // 
            this.menuItemSaveTracerLocally.Index = 10;
            this.menuItemSaveTracerLocally.Text = "Save Active Tracer as local file";
            this.menuItemSaveTracerLocally.Click += new System.EventHandler(this.menuItemSaveTracerLocally_Click);
            // 
            // menuItemEditLocalTracer
            // 
            this.menuItemEditLocalTracer.Index = 11;
            this.menuItemEditLocalTracer.Text = "Edit Active Tracer and Save as a local file";
            this.menuItemEditLocalTracer.Click += new System.EventHandler(this.menuItemEditLocalTracer_Click);
            // 
            // menuBaPO2IC
            // 
            this.menuBaPO2IC.Index = 1;
            this.menuBaPO2IC.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemActivateDefaultEARTHTIMEBaPO2IC,
            this.menuItem1,
            this.menuItemShowActiveBaPO2});
            this.menuBaPO2IC.Text = "BaPO2 IC";
            // 
            // menuItemActivateDefaultEARTHTIMEBaPO2IC
            // 
            this.menuItemActivateDefaultEARTHTIMEBaPO2IC.Enabled = false;
            this.menuItemActivateDefaultEARTHTIMEBaPO2IC.Index = 0;
            this.menuItemActivateDefaultEARTHTIMEBaPO2IC.Text = "Activate Built-In Default EARTHTIME BaPO2 IC";
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.Text = "-";
            // 
            // menuItemShowActiveBaPO2
            // 
            this.menuItemShowActiveBaPO2.Index = 2;
            this.menuItemShowActiveBaPO2.Text = "Show Active BaPO2 in a Tripoli window";
            this.menuItemShowActiveBaPO2.Click += new System.EventHandler(this.menuItemShowActiveBaPO2_Click);
            // 
            // menuTlIC
            // 
            this.menuTlIC.Index = 2;
            this.menuTlIC.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemActivateDefaultEARTHTIMETlIC,
            this.menuItem11,
            this.menuItemShowActiveTlIC});
            this.menuTlIC.Text = "Tl IC";
            // 
            // menuItemActivateDefaultEARTHTIMETlIC
            // 
            this.menuItemActivateDefaultEARTHTIMETlIC.Enabled = false;
            this.menuItemActivateDefaultEARTHTIMETlIC.Index = 0;
            this.menuItemActivateDefaultEARTHTIMETlIC.Text = "Activate Built-In Default EARTHTIME Tl IC";
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 1;
            this.menuItem11.Text = "-";
            // 
            // menuItemShowActiveTlIC
            // 
            this.menuItemShowActiveTlIC.Index = 2;
            this.menuItemShowActiveTlIC.Text = "Show Active Tl IC in a Tripoli window";
            this.menuItemShowActiveTlIC.Click += new System.EventHandler(this.menuItemShowActiveTlIC_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 3;
            this.menuItem7.Text = "-";
            // 
            // menuItemCorrectFraction
            // 
            this.menuItemCorrectFraction.Index = 4;
            this.menuItemCorrectFraction.Text = "Perform Corrections";
            this.menuItemCorrectFraction.Click += new System.EventHandler(this.menuItemCorrectFraction_Click);
            // 
            // menuItemShowConfirmList
            // 
            this.menuItemShowConfirmList.Enabled = false;
            this.menuItemShowConfirmList.Index = 5;
            this.menuItemShowConfirmList.Text = "Show corrections report";
            this.menuItemShowConfirmList.Click += new System.EventHandler(this.menuItemShowConfirmList_Click);
            // 
            // menuItemRemoveCorrection
            // 
            this.menuItemRemoveCorrection.Index = 6;
            this.menuItemRemoveCorrection.Text = "Undo Corrections";
            this.menuItemRemoveCorrection.Click += new System.EventHandler(this.menuItemRemoveCorrection_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 7;
            this.menuItem10.Text = "-";
            // 
            // menuItemOxideCorrection
            // 
            this.menuItemOxideCorrection.Index = 8;
            this.menuItemOxideCorrection.Text = "Oxide settings for automatic correction";
            this.menuItemOxideCorrection.Click += new System.EventHandler(this.menuItemOxideCorrection_Click);
            // 
            // menuAnnotate
            // 
            this.menuAnnotate.Enabled = false;
            this.menuAnnotate.Index = 4;
            this.menuAnnotate.Text = "Annotate";
            this.menuAnnotate.Click += new System.EventHandler(this.menuItem9_Click_2);
            // 
            // menuControlPanel
            // 
            this.menuControlPanel.Index = 5;
            this.menuControlPanel.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemLaunchKwikiExporter,
            this.menuItem15,
            this.startStopLiveWorkflow_menuItem,
            this.loadCurrentDatFile_menuItem,
            this.saveTripAndExportToRedux_menuItem,
            this.menuItem12,
            this.setLiveWorkflowDataFolder_menuItem,
            this.menuItem17,
            this.saveExportToClipboardForPbMacDat_menuItem,
            this.saveExportToTextFile_menuItem});
            this.menuControlPanel.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.menuControlPanel.Text = "Control Panel";
            this.menuControlPanel.Click += new System.EventHandler(this.menuControlPanel_Click);
            this.menuControlPanel.Select += new System.EventHandler(this.menuControlPanel_Click);
            // 
            // menuItemLaunchKwikiExporter
            // 
            this.menuItemLaunchKwikiExporter.Index = 0;
            this.menuItemLaunchKwikiExporter.Text = "Launch Control Panel";
            this.menuItemLaunchKwikiExporter.Click += new System.EventHandler(this.menuItemLaunchKwikiExporter_Click);
            // 
            // menuItem15
            // 
            this.menuItem15.Index = 1;
            this.menuItem15.Text = "-";
            // 
            // startStopLiveWorkflow_menuItem
            // 
            this.startStopLiveWorkflow_menuItem.Enabled = false;
            this.startStopLiveWorkflow_menuItem.Index = 2;
            this.startStopLiveWorkflow_menuItem.Text = "Start Live Mode";
            this.startStopLiveWorkflow_menuItem.Click += new System.EventHandler(this.startStopLiveWorkflow_menuItem_Click);
            // 
            // loadCurrentDatFile_menuItem
            // 
            this.loadCurrentDatFile_menuItem.Index = 3;
            this.loadCurrentDatFile_menuItem.Tag = "Load Current data file";
            this.loadCurrentDatFile_menuItem.Text = "Load Current data file";
            this.loadCurrentDatFile_menuItem.Click += new System.EventHandler(this.loadCurrentDatFile_menuItem_Click);
            // 
            // saveTripAndExportToRedux_menuItem
            // 
            this.saveTripAndExportToRedux_menuItem.Index = 4;
            this.saveTripAndExportToRedux_menuItem.Tag = "Save work and export for ET_Redux";
            this.saveTripAndExportToRedux_menuItem.Text = "Save work and export for ET_Redux";
            this.saveTripAndExportToRedux_menuItem.Click += new System.EventHandler(this.menuExportToUPbRedux_Click);
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 5;
            this.menuItem12.Text = "-";
            // 
            // setLiveWorkflowDataFolder_menuItem
            // 
            this.setLiveWorkflowDataFolder_menuItem.Index = 6;
            this.setLiveWorkflowDataFolder_menuItem.Text = "Set Live Workflow folders";
            this.setLiveWorkflowDataFolder_menuItem.Click += new System.EventHandler(this.setLiveWorkflowDataFolder_menuItem_Click);
            // 
            // menuItem17
            // 
            this.menuItem17.Index = 7;
            this.menuItem17.Text = "-";
            // 
            // saveExportToClipboardForPbMacDat_menuItem
            // 
            this.saveExportToClipboardForPbMacDat_menuItem.Index = 8;
            this.saveExportToClipboardForPbMacDat_menuItem.Text = "Save, export to Clipboard for PbMacdat";
            this.saveExportToClipboardForPbMacDat_menuItem.Click += new System.EventHandler(this.menuItem14_Click_1);
            // 
            // saveExportToTextFile_menuItem
            // 
            this.saveExportToTextFile_menuItem.Index = 9;
            this.saveExportToTextFile_menuItem.Text = "Save work and export as text";
            this.saveExportToTextFile_menuItem.Click += new System.EventHandler(this.menuExportToTextFile_Click);
            // 
            // menuResources
            // 
            this.menuResources.Index = 6;
            this.menuResources.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuTools,
            this.menuItem16,
            this.menuItemETOWebSite,
            this.menuItem3,
            this.menuItemDataDictionary,
            this.CIRDLESonGitHubMenuItem});
            this.menuResources.Text = "Resources";
            // 
            // menuTools
            // 
            this.menuTools.Index = 0;
            this.menuTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemOpenIDTIMS_CSV_Template});
            this.menuTools.Text = "Tools";
            // 
            // menuItemOpenIDTIMS_CSV_Template
            // 
            this.menuItemOpenIDTIMS_CSV_Template.Index = 0;
            this.menuItemOpenIDTIMS_CSV_Template.Text = "Open ID-TIMS \".csv\" template with Excel";
            this.menuItemOpenIDTIMS_CSV_Template.Click += new System.EventHandler(this.menuItemOpenIDTIMS_CSV_Template_Click);
            // 
            // menuItem16
            // 
            this.menuItem16.Index = 1;
            this.menuItem16.Text = "-";
            // 
            // menuItemETOWebSite
            // 
            this.menuItemETOWebSite.Index = 2;
            this.menuItemETOWebSite.Text = "EARTHTIME website";
            this.menuItemETOWebSite.Click += new System.EventHandler(this.menuItemETOWebSite_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 3;
            this.menuItem3.Text = "CIRDLES website";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItemDataDictionary
            // 
            this.menuItemDataDictionary.Index = 4;
            this.menuItemDataDictionary.Text = "Data Dictionary";
            this.menuItemDataDictionary.Visible = false;
            this.menuItemDataDictionary.Click += new System.EventHandler(this.menuItemDataDictionary_Click);
            // 
            // CIRDLESonGitHubMenuItem
            // 
            this.CIRDLESonGitHubMenuItem.Index = 5;
            this.CIRDLESonGitHubMenuItem.Text = "CIRDLES on GitHub.com";
            this.CIRDLESonGitHubMenuItem.Click += new System.EventHandler(this.CIRDLESonGitHubMenuItem_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.Index = 7;
            this.menuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemHelpText,
            this.menuItem4,
            this.version47ReleaseNotesMenuItem,
            this.menuItemRevisionHistory,
            this.menuItemCheckForUpdates,
            this.menuItem6,
            this.menuItemAbout,
            this.menuItemCredits});
            this.menuHelp.Text = "Help";
            // 
            // menuItemHelpText
            // 
            this.menuItemHelpText.Index = 0;
            this.menuItemHelpText.Text = "Help text";
            this.menuItemHelpText.Click += new System.EventHandler(this.menuItemHelpText_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 1;
            this.menuItem4.Text = "-";
            // 
            // version47ReleaseNotesMenuItem
            // 
            this.version47ReleaseNotesMenuItem.Index = 2;
            this.version47ReleaseNotesMenuItem.Text = "Release Notes for version 4.7 and later";
            this.version47ReleaseNotesMenuItem.Click += new System.EventHandler(this.version47ReleaseNotesMenuItem_Click);
            // 
            // menuItemRevisionHistory
            // 
            this.menuItemRevisionHistory.Index = 3;
            this.menuItemRevisionHistory.Text = "Release Notes";
            this.menuItemRevisionHistory.Click += new System.EventHandler(this.menuItemRevisionHistory_Click);
            // 
            // menuItemCheckForUpdates
            // 
            this.menuItemCheckForUpdates.Index = 4;
            this.menuItemCheckForUpdates.Text = "Check for updates to Tripoli";
            this.menuItemCheckForUpdates.Click += new System.EventHandler(this.menuItemCheckForUpdates_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 5;
            this.menuItem6.Text = "-";
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Index = 6;
            this.menuItemAbout.Text = "About";
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // menuItemCredits
            // 
            this.menuItemCredits.Index = 7;
            this.menuItemCredits.Text = "Credits";
            this.menuItemCredits.Click += new System.EventHandler(this.menuItemCredits_Click);
            // 
            // ButtonPanelRatios
            // 
            this.ButtonPanelRatios.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonPanelRatios.BackColor = System.Drawing.Color.Bisque;
            this.ButtonPanelRatios.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ButtonPanelRatios.Controls.Add(this.btnSelectRatios);
            this.ButtonPanelRatios.Controls.Add(this.btnTileSelected);
            this.ButtonPanelRatios.Controls.Add(this.btnSelectUserFunctions);
            this.ButtonPanelRatios.Controls.Add(this.btnSelectNone);
            this.ButtonPanelRatios.Controls.Add(this.btnSelectAll);
            this.ButtonPanelRatios.Controls.Add(this.btnShowChecked);
            this.ButtonPanelRatios.Cursor = System.Windows.Forms.Cursors.Default;
            this.ButtonPanelRatios.Location = new System.Drawing.Point(0, 274);
            this.ButtonPanelRatios.Name = "ButtonPanelRatios";
            this.ButtonPanelRatios.Size = new System.Drawing.Size(688, 36);
            this.ButtonPanelRatios.TabIndex = 5;
            this.ButtonPanelRatios.Visible = false;
            // 
            // btnSelectRatios
            // 
            this.btnSelectRatios.BackColor = System.Drawing.Color.Cornsilk;
            this.btnSelectRatios.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectRatios.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectRatios.Location = new System.Drawing.Point(348, 4);
            this.btnSelectRatios.Name = "btnSelectRatios";
            this.btnSelectRatios.Size = new System.Drawing.Size(110, 26);
            this.btnSelectRatios.TabIndex = 10;
            this.btnSelectRatios.Text = "Select Ratios";
            this.btnSelectRatios.UseCompatibleTextRendering = true;
            this.btnSelectRatios.UseVisualStyleBackColor = false;
            this.btnSelectRatios.Click += new System.EventHandler(this.btnSelectRatios_Click);
            // 
            // btnTileSelected
            // 
            this.btnTileSelected.BackColor = System.Drawing.Color.Cornsilk;
            this.btnTileSelected.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTileSelected.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTileSelected.Location = new System.Drawing.Point(567, 4);
            this.btnTileSelected.Name = "btnTileSelected";
            this.btnTileSelected.Size = new System.Drawing.Size(121, 26);
            this.btnTileSelected.TabIndex = 9;
            this.btnTileSelected.Text = "GRAPH Selected";
            this.btnTileSelected.UseCompatibleTextRendering = true;
            this.btnTileSelected.UseVisualStyleBackColor = false;
            this.btnTileSelected.Click += new System.EventHandler(this.btnTileSelected_Click);
            // 
            // btnSelectUserFunctions
            // 
            this.btnSelectUserFunctions.BackColor = System.Drawing.Color.Cornsilk;
            this.btnSelectUserFunctions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectUserFunctions.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectUserFunctions.Location = new System.Drawing.Point(218, 4);
            this.btnSelectUserFunctions.Name = "btnSelectUserFunctions";
            this.btnSelectUserFunctions.Size = new System.Drawing.Size(128, 26);
            this.btnSelectUserFunctions.TabIndex = 7;
            this.btnSelectUserFunctions.Text = "Select User Funcs";
            this.btnSelectUserFunctions.UseCompatibleTextRendering = true;
            this.btnSelectUserFunctions.UseVisualStyleBackColor = false;
            this.btnSelectUserFunctions.Click += new System.EventHandler(this.btnSelectUserFunctions_Click);
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.BackColor = System.Drawing.Color.Cornsilk;
            this.btnSelectNone.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectNone.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectNone.Location = new System.Drawing.Point(111, 4);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(106, 26);
            this.btnSelectNone.TabIndex = 6;
            this.btnSelectNone.Text = "Select None";
            this.btnSelectNone.UseCompatibleTextRendering = true;
            this.btnSelectNone.UseVisualStyleBackColor = false;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.BackColor = System.Drawing.Color.Cornsilk;
            this.btnSelectAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectAll.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectAll.Location = new System.Drawing.Point(2, 4);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(108, 26);
            this.btnSelectAll.TabIndex = 5;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseCompatibleTextRendering = true;
            this.btnSelectAll.UseVisualStyleBackColor = false;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnShowChecked
            // 
            this.btnShowChecked.BackColor = System.Drawing.Color.Cornsilk;
            this.btnShowChecked.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnShowChecked.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowChecked.Location = new System.Drawing.Point(459, 4);
            this.btnShowChecked.Name = "btnShowChecked";
            this.btnShowChecked.Size = new System.Drawing.Size(108, 26);
            this.btnShowChecked.TabIndex = 4;
            this.btnShowChecked.Text = "Show Selected";
            this.btnShowChecked.UseCompatibleTextRendering = true;
            this.btnShowChecked.UseVisualStyleBackColor = false;
            this.btnShowChecked.Click += new System.EventHandler(this.btnShowChecked_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 50;
            this.toolTip1.AutoPopDelay = 1000;
            this.toolTip1.InitialDelay = 50;
            this.toolTip1.ReshowDelay = 10;
            // 
            // pnlAnnotate
            // 
            this.pnlAnnotate.BackColor = System.Drawing.Color.DarkSalmon;
            this.pnlAnnotate.Controls.Add(this.button1);
            this.pnlAnnotate.Controls.Add(this.txtNotations);
            this.pnlAnnotate.Location = new System.Drawing.Point(142, 53);
            this.pnlAnnotate.Name = "pnlAnnotate";
            this.pnlAnnotate.Size = new System.Drawing.Size(378, 40);
            this.pnlAnnotate.TabIndex = 7;
            this.pnlAnnotate.Visible = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button1.Font = new System.Drawing.Font("Arial Black", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(356, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(14, 14);
            this.button1.TabIndex = 1;
            this.button1.Text = "X";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtNotations
            // 
            this.txtNotations.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNotations.Location = new System.Drawing.Point(4, 17);
            this.txtNotations.Multiline = true;
            this.txtNotations.Name = "txtNotations";
            this.txtNotations.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNotations.Size = new System.Drawing.Size(368, 146);
            this.txtNotations.TabIndex = 0;
            this.txtNotations.Text = "Notations";
            // 
            // pnlIntro
            // 
            this.pnlIntro.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlIntro.BackColor = System.Drawing.Color.Gainsboro;
            this.pnlIntro.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlIntro.Controls.Add(this.pictureBox2);
            this.pnlIntro.Controls.Add(this.label1);
            this.pnlIntro.Controls.Add(this.label3);
            this.pnlIntro.Controls.Add(this.label2);
            this.pnlIntro.Controls.Add(this.lblStandBy);
            this.pnlIntro.Controls.Add(this.lblTripoliWithVersion);
            this.pnlIntro.Controls.Add(this.pictureBox1);
            this.pnlIntro.Location = new System.Drawing.Point(62, 74);
            this.pnlIntro.Name = "pnlIntro";
            this.pnlIntro.Size = new System.Drawing.Size(564, 172);
            this.pnlIntro.TabIndex = 10;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(490, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(67, 55);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.IndianRed;
            this.label1.Location = new System.Drawing.Point(175, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(289, 23);
            this.label1.TabIndex = 9;
            this.label1.Text = ">>>  preparing data for ET_Redux >>> ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.LightSalmon;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(121, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(329, 14);
            this.label3.TabIndex = 8;
            this.label3.Text = "Tripoli is licensed as open-source software: click on About.\r\n";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(102, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(402, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "Graphical Data Processor for Mass Spectrometry";
            // 
            // lblStandBy
            // 
            this.lblStandBy.AutoSize = true;
            this.lblStandBy.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblStandBy.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblStandBy.Font = new System.Drawing.Font("Arial", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStandBy.ForeColor = System.Drawing.Color.Red;
            this.lblStandBy.Location = new System.Drawing.Point(178, 154);
            this.lblStandBy.Name = "lblStandBy";
            this.lblStandBy.Size = new System.Drawing.Size(225, 25);
            this.lblStandBy.TabIndex = 6;
            this.lblStandBy.Text = "   Please standby .  .  .   ";
            this.lblStandBy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStandBy.Visible = false;
            // 
            // lblTripoliWithVersion
            // 
            this.lblTripoliWithVersion.Font = new System.Drawing.Font("Arial Black", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTripoliWithVersion.Location = new System.Drawing.Point(56, 16);
            this.lblTripoliWithVersion.Margin = new System.Windows.Forms.Padding(0);
            this.lblTripoliWithVersion.Name = "lblTripoliWithVersion";
            this.lblTripoliWithVersion.Size = new System.Drawing.Size(133, 23);
            this.lblTripoliWithVersion.TabIndex = 4;
            this.lblTripoliWithVersion.Text = "Tripoli  4.9";
            this.lblTripoliWithVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // ButtonPanelHistory
            // 
            this.ButtonPanelHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonPanelHistory.BackColor = System.Drawing.Color.Bisque;
            this.ButtonPanelHistory.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ButtonPanelHistory.Controls.Add(this.btnShowCollectors);
            this.ButtonPanelHistory.Controls.Add(this.btnSelectNewFiles);
            this.ButtonPanelHistory.Controls.Add(this.btnSelectSaved);
            this.ButtonPanelHistory.Controls.Add(this.btnSelectNoneFiles);
            this.ButtonPanelHistory.Controls.Add(this.btnSelectAllFiles);
            this.ButtonPanelHistory.Controls.Add(this.btnSaveHistory);
            this.ButtonPanelHistory.Cursor = System.Windows.Forms.Cursors.Default;
            this.ButtonPanelHistory.Location = new System.Drawing.Point(-1, 240);
            this.ButtonPanelHistory.Name = "ButtonPanelHistory";
            this.ButtonPanelHistory.Size = new System.Drawing.Size(688, 36);
            this.ButtonPanelHistory.TabIndex = 11;
            this.ButtonPanelHistory.Visible = false;
            // 
            // btnShowCollectors
            // 
            this.btnShowCollectors.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.btnShowCollectors.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnShowCollectors.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowCollectors.Location = new System.Drawing.Point(565, 4);
            this.btnShowCollectors.Name = "btnShowCollectors";
            this.btnShowCollectors.Size = new System.Drawing.Size(122, 26);
            this.btnShowCollectors.TabIndex = 11;
            this.btnShowCollectors.Text = "Show Details";
            this.btnShowCollectors.UseCompatibleTextRendering = true;
            this.btnShowCollectors.UseVisualStyleBackColor = false;
            this.btnShowCollectors.Click += new System.EventHandler(this.btnShowCollectors_Click);
            // 
            // btnSelectNewFiles
            // 
            this.btnSelectNewFiles.BackColor = System.Drawing.Color.LightYellow;
            this.btnSelectNewFiles.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectNewFiles.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectNewFiles.Location = new System.Drawing.Point(328, 4);
            this.btnSelectNewFiles.Name = "btnSelectNewFiles";
            this.btnSelectNewFiles.Size = new System.Drawing.Size(118, 26);
            this.btnSelectNewFiles.TabIndex = 10;
            this.btnSelectNewFiles.Text = "Select New Files";
            this.btnSelectNewFiles.UseCompatibleTextRendering = true;
            this.btnSelectNewFiles.UseVisualStyleBackColor = false;
            this.btnSelectNewFiles.Click += new System.EventHandler(this.btnSelectNewFiles_Click);
            // 
            // btnSelectSaved
            // 
            this.btnSelectSaved.BackColor = System.Drawing.Color.LightYellow;
            this.btnSelectSaved.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectSaved.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectSaved.Location = new System.Drawing.Point(220, 4);
            this.btnSelectSaved.Name = "btnSelectSaved";
            this.btnSelectSaved.Size = new System.Drawing.Size(106, 26);
            this.btnSelectSaved.TabIndex = 7;
            this.btnSelectSaved.Text = "Select Saved";
            this.btnSelectSaved.UseCompatibleTextRendering = true;
            this.btnSelectSaved.UseVisualStyleBackColor = false;
            this.btnSelectSaved.Click += new System.EventHandler(this.btnSelectSaved_Click);
            // 
            // btnSelectNoneFiles
            // 
            this.btnSelectNoneFiles.BackColor = System.Drawing.Color.LightYellow;
            this.btnSelectNoneFiles.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectNoneFiles.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectNoneFiles.Location = new System.Drawing.Point(112, 4);
            this.btnSelectNoneFiles.Name = "btnSelectNoneFiles";
            this.btnSelectNoneFiles.Size = new System.Drawing.Size(106, 26);
            this.btnSelectNoneFiles.TabIndex = 6;
            this.btnSelectNoneFiles.Text = "Select None";
            this.btnSelectNoneFiles.UseCompatibleTextRendering = true;
            this.btnSelectNoneFiles.UseVisualStyleBackColor = false;
            this.btnSelectNoneFiles.Click += new System.EventHandler(this.btnSelectNoneFiles_Click);
            // 
            // btnSelectAllFiles
            // 
            this.btnSelectAllFiles.BackColor = System.Drawing.Color.LightYellow;
            this.btnSelectAllFiles.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectAllFiles.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectAllFiles.Location = new System.Drawing.Point(2, 4);
            this.btnSelectAllFiles.Name = "btnSelectAllFiles";
            this.btnSelectAllFiles.Size = new System.Drawing.Size(108, 26);
            this.btnSelectAllFiles.TabIndex = 5;
            this.btnSelectAllFiles.Text = "Select All";
            this.btnSelectAllFiles.UseCompatibleTextRendering = true;
            this.btnSelectAllFiles.UseVisualStyleBackColor = false;
            this.btnSelectAllFiles.Click += new System.EventHandler(this.btnSelectAllFiles_Click);
            // 
            // btnSaveHistory
            // 
            this.btnSaveHistory.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.btnSaveHistory.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveHistory.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveHistory.Location = new System.Drawing.Point(449, 4);
            this.btnSaveHistory.Name = "btnSaveHistory";
            this.btnSaveHistory.Size = new System.Drawing.Size(114, 26);
            this.btnSaveHistory.TabIndex = 4;
            this.btnSaveHistory.Text = "SAVE  History";
            this.btnSaveHistory.UseCompatibleTextRendering = true;
            this.btnSaveHistory.UseVisualStyleBackColor = false;
            this.btnSaveHistory.Click += new System.EventHandler(this.btnSaveHistory_Click);
            // 
            // buttonPanelCollectors
            // 
            this.buttonPanelCollectors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPanelCollectors.BackColor = System.Drawing.Color.Bisque;
            this.buttonPanelCollectors.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.buttonPanelCollectors.Controls.Add(this.btnSaveCollectorHistory);
            this.buttonPanelCollectors.Controls.Add(this.btnShowGainsFolder);
            this.buttonPanelCollectors.Controls.Add(this.btnTileSelectedCollectors);
            this.buttonPanelCollectors.Controls.Add(this.btnSelectNoneCollectors);
            this.buttonPanelCollectors.Controls.Add(this.btnSelectAllCollectors);
            this.buttonPanelCollectors.Cursor = System.Windows.Forms.Cursors.Default;
            this.buttonPanelCollectors.Location = new System.Drawing.Point(0, 203);
            this.buttonPanelCollectors.Name = "buttonPanelCollectors";
            this.buttonPanelCollectors.Size = new System.Drawing.Size(688, 36);
            this.buttonPanelCollectors.TabIndex = 12;
            this.buttonPanelCollectors.Visible = false;
            // 
            // btnSaveCollectorHistory
            // 
            this.btnSaveCollectorHistory.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.btnSaveCollectorHistory.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveCollectorHistory.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveCollectorHistory.Location = new System.Drawing.Point(449, 4);
            this.btnSaveCollectorHistory.Name = "btnSaveCollectorHistory";
            this.btnSaveCollectorHistory.Size = new System.Drawing.Size(112, 26);
            this.btnSaveCollectorHistory.TabIndex = 11;
            this.btnSaveCollectorHistory.Text = "SAVE  History";
            this.btnSaveCollectorHistory.UseCompatibleTextRendering = true;
            this.btnSaveCollectorHistory.UseVisualStyleBackColor = false;
            this.btnSaveCollectorHistory.Click += new System.EventHandler(this.btnSaveHistory_Click);
            // 
            // btnShowGainsFolder
            // 
            this.btnShowGainsFolder.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.btnShowGainsFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnShowGainsFolder.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowGainsFolder.Location = new System.Drawing.Point(563, 4);
            this.btnShowGainsFolder.Name = "btnShowGainsFolder";
            this.btnShowGainsFolder.Size = new System.Drawing.Size(124, 26);
            this.btnShowGainsFolder.TabIndex = 10;
            this.btnShowGainsFolder.Text = "Show  Files";
            this.btnShowGainsFolder.UseCompatibleTextRendering = true;
            this.btnShowGainsFolder.UseVisualStyleBackColor = false;
            this.btnShowGainsFolder.Click += new System.EventHandler(this.btnShowGainsFolder_Click);
            // 
            // btnTileSelectedCollectors
            // 
            this.btnTileSelectedCollectors.BackColor = System.Drawing.Color.Cornsilk;
            this.btnTileSelectedCollectors.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTileSelectedCollectors.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTileSelectedCollectors.Location = new System.Drawing.Point(218, 4);
            this.btnTileSelectedCollectors.Name = "btnTileSelectedCollectors";
            this.btnTileSelectedCollectors.Size = new System.Drawing.Size(228, 26);
            this.btnTileSelectedCollectors.TabIndex = 9;
            this.btnTileSelectedCollectors.Text = "GRAPH Selected Histories";
            this.btnTileSelectedCollectors.UseCompatibleTextRendering = true;
            this.btnTileSelectedCollectors.UseVisualStyleBackColor = false;
            this.btnTileSelectedCollectors.Click += new System.EventHandler(this.btnTileSelectedCollectors_Click);
            // 
            // btnSelectNoneCollectors
            // 
            this.btnSelectNoneCollectors.BackColor = System.Drawing.Color.Cornsilk;
            this.btnSelectNoneCollectors.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectNoneCollectors.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectNoneCollectors.Location = new System.Drawing.Point(111, 4);
            this.btnSelectNoneCollectors.Name = "btnSelectNoneCollectors";
            this.btnSelectNoneCollectors.Size = new System.Drawing.Size(106, 26);
            this.btnSelectNoneCollectors.TabIndex = 6;
            this.btnSelectNoneCollectors.Text = "Select None";
            this.btnSelectNoneCollectors.UseCompatibleTextRendering = true;
            this.btnSelectNoneCollectors.UseVisualStyleBackColor = false;
            this.btnSelectNoneCollectors.Click += new System.EventHandler(this.btnSelectNoneCollectors_Click);
            // 
            // btnSelectAllCollectors
            // 
            this.btnSelectAllCollectors.BackColor = System.Drawing.Color.Cornsilk;
            this.btnSelectAllCollectors.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectAllCollectors.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectAllCollectors.Location = new System.Drawing.Point(2, 4);
            this.btnSelectAllCollectors.Name = "btnSelectAllCollectors";
            this.btnSelectAllCollectors.Size = new System.Drawing.Size(108, 26);
            this.btnSelectAllCollectors.TabIndex = 5;
            this.btnSelectAllCollectors.Text = "Select All";
            this.btnSelectAllCollectors.UseCompatibleTextRendering = true;
            this.btnSelectAllCollectors.UseVisualStyleBackColor = false;
            this.btnSelectAllCollectors.Click += new System.EventHandler(this.btnSelectAllCollectors_Click);
            // 
            // toolTip2
            // 
            this.toolTip2.AutoPopDelay = 1000;
            this.toolTip2.InitialDelay = 500;
            this.toolTip2.ReshowDelay = 100;
            // 
            // statusBar1
            // 
            this.statusBar1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusBar1.Location = new System.Drawing.Point(0, 305);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(688, 22);
            this.statusBar1.TabIndex = 15;
            this.statusBar1.Text = "statusStrip1";
            this.statusBar1.DoubleClick += new System.EventHandler(this.statusBar1_DoubleClick);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.BackColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(14, 17);
            this.toolStripStatusLabel1.Text = "-";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // panelCorrections
            // 
            this.panelCorrections.BackColor = System.Drawing.Color.Transparent;
            this.panelCorrections.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelCorrections.Controls.Add(this.lblOxideCorrectionValue);
            this.panelCorrections.Controls.Add(this.chkBoxUseIC);
            this.panelCorrections.Controls.Add(this.chkBoxUseTracer);
            this.panelCorrections.Controls.Add(this.lblAppliedTlIC);
            this.panelCorrections.Controls.Add(this.lblActiveTlIC);
            this.panelCorrections.Controls.Add(this.lblAppliedBaPO2IC);
            this.panelCorrections.Controls.Add(this.lblActiveBaPO2IC);
            this.panelCorrections.Controls.Add(this.btnFractionationCorrection);
            this.panelCorrections.Controls.Add(this.lblAppliedTracer);
            this.panelCorrections.Controls.Add(this.lblActiveTracer);
            this.panelCorrections.Location = new System.Drawing.Point(7, 0);
            this.panelCorrections.Name = "panelCorrections";
            this.panelCorrections.Size = new System.Drawing.Size(680, 53);
            this.panelCorrections.TabIndex = 26;
            // 
            // lblOxideCorrectionValue
            // 
            this.lblOxideCorrectionValue.BackColor = System.Drawing.Color.AliceBlue;
            this.lblOxideCorrectionValue.Location = new System.Drawing.Point(162, 19);
            this.lblOxideCorrectionValue.Name = "lblOxideCorrectionValue";
            this.lblOxideCorrectionValue.Size = new System.Drawing.Size(351, 14);
            this.lblOxideCorrectionValue.TabIndex = 34;
            this.lblOxideCorrectionValue.Tag = "Automatic Oxide Correction Performed with 18O / 16O = ";
            this.lblOxideCorrectionValue.Text = "Automatic Oxide Correction Performed with 18O / 16O = ";
            this.lblOxideCorrectionValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblOxideCorrectionValue.Visible = false;
            // 
            // chkBoxUseIC
            // 
            this.chkBoxUseIC.BackColor = System.Drawing.Color.BurlyWood;
            this.chkBoxUseIC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chkBoxUseIC.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkBoxUseIC.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkBoxUseIC.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxUseIC.Location = new System.Drawing.Point(205, 17);
            this.chkBoxUseIC.Name = "chkBoxUseIC";
            this.chkBoxUseIC.Size = new System.Drawing.Size(98, 31);
            this.chkBoxUseIC.TabIndex = 33;
            this.chkBoxUseIC.Text = "Interference";
            this.chkBoxUseIC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkBoxUseIC.UseCompatibleTextRendering = true;
            this.chkBoxUseIC.UseVisualStyleBackColor = false;
            this.chkBoxUseIC.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chkBoxUseIC_MouseClick);
            // 
            // chkBoxUseTracer
            // 
            this.chkBoxUseTracer.BackColor = System.Drawing.Color.BurlyWood;
            this.chkBoxUseTracer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chkBoxUseTracer.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkBoxUseTracer.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkBoxUseTracer.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxUseTracer.Location = new System.Drawing.Point(205, 1);
            this.chkBoxUseTracer.Name = "chkBoxUseTracer";
            this.chkBoxUseTracer.Size = new System.Drawing.Size(98, 17);
            this.chkBoxUseTracer.TabIndex = 32;
            this.chkBoxUseTracer.Text = "Fractionation";
            this.chkBoxUseTracer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkBoxUseTracer.UseCompatibleTextRendering = true;
            this.chkBoxUseTracer.UseVisualStyleBackColor = false;
            this.chkBoxUseTracer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chkBoxUseTracer_MouseClick);
            // 
            // lblAppliedTlIC
            // 
            this.lblAppliedTlIC.BackColor = System.Drawing.Color.BurlyWood;
            this.lblAppliedTlIC.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblAppliedTlIC.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppliedTlIC.Location = new System.Drawing.Point(462, 32);
            this.lblAppliedTlIC.Name = "lblAppliedTlIC";
            this.lblAppliedTlIC.Size = new System.Drawing.Size(213, 17);
            this.lblAppliedTlIC.TabIndex = 31;
            this.lblAppliedTlIC.Tag = "Applied Tl IC:  ";
            this.lblAppliedTlIC.Text = "Applied Tl IC:  ";
            this.lblAppliedTlIC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblActiveTlIC
            // 
            this.lblActiveTlIC.BackColor = System.Drawing.Color.BurlyWood;
            this.lblActiveTlIC.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblActiveTlIC.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActiveTlIC.Location = new System.Drawing.Point(2, 32);
            this.lblActiveTlIC.Name = "lblActiveTlIC";
            this.lblActiveTlIC.Size = new System.Drawing.Size(202, 17);
            this.lblActiveTlIC.TabIndex = 30;
            this.lblActiveTlIC.Tag = "Active Tl IC:  ";
            this.lblActiveTlIC.Text = "Active Tl IC:  ";
            this.lblActiveTlIC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAppliedBaPO2IC
            // 
            this.lblAppliedBaPO2IC.BackColor = System.Drawing.Color.BurlyWood;
            this.lblAppliedBaPO2IC.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblAppliedBaPO2IC.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppliedBaPO2IC.Location = new System.Drawing.Point(462, 18);
            this.lblAppliedBaPO2IC.Name = "lblAppliedBaPO2IC";
            this.lblAppliedBaPO2IC.Size = new System.Drawing.Size(213, 17);
            this.lblAppliedBaPO2IC.TabIndex = 29;
            this.lblAppliedBaPO2IC.Tag = "Applied BaPO2 IC:  ";
            this.lblAppliedBaPO2IC.Text = "Applied BaPO2 IC:  ";
            this.lblAppliedBaPO2IC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblActiveBaPO2IC
            // 
            this.lblActiveBaPO2IC.BackColor = System.Drawing.Color.BurlyWood;
            this.lblActiveBaPO2IC.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblActiveBaPO2IC.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActiveBaPO2IC.Location = new System.Drawing.Point(2, 18);
            this.lblActiveBaPO2IC.Name = "lblActiveBaPO2IC";
            this.lblActiveBaPO2IC.Size = new System.Drawing.Size(202, 17);
            this.lblActiveBaPO2IC.TabIndex = 28;
            this.lblActiveBaPO2IC.Tag = "Active BaPO2 IC:  ";
            this.lblActiveBaPO2IC.Text = "Active BaPO2 IC:  ";
            this.lblActiveBaPO2IC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnFractionationCorrection
            // 
            this.btnFractionationCorrection.BackColor = System.Drawing.Color.Cornsilk;
            this.btnFractionationCorrection.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFractionationCorrection.Enabled = false;
            this.btnFractionationCorrection.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnFractionationCorrection.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFractionationCorrection.ForeColor = System.Drawing.Color.Red;
            this.btnFractionationCorrection.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnFractionationCorrection.Location = new System.Drawing.Point(305, 0);
            this.btnFractionationCorrection.Margin = new System.Windows.Forms.Padding(0);
            this.btnFractionationCorrection.Name = "btnFractionationCorrection";
            this.btnFractionationCorrection.Size = new System.Drawing.Size(156, 48);
            this.btnFractionationCorrection.TabIndex = 26;
            this.btnFractionationCorrection.Tag = "";
            this.btnFractionationCorrection.Text = "No Corrections";
            this.btnFractionationCorrection.UseCompatibleTextRendering = true;
            this.btnFractionationCorrection.UseVisualStyleBackColor = false;
            this.btnFractionationCorrection.Click += new System.EventHandler(this.btnFractionationCorrection_Click);
            // 
            // lblAppliedTracer
            // 
            this.lblAppliedTracer.BackColor = System.Drawing.Color.BurlyWood;
            this.lblAppliedTracer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblAppliedTracer.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppliedTracer.Location = new System.Drawing.Point(462, 1);
            this.lblAppliedTracer.Name = "lblAppliedTracer";
            this.lblAppliedTracer.Size = new System.Drawing.Size(213, 18);
            this.lblAppliedTracer.TabIndex = 27;
            this.lblAppliedTracer.Tag = "Applied Tracer:  ";
            this.lblAppliedTracer.Text = "Applied Tracer:  ";
            this.lblAppliedTracer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblActiveTracer
            // 
            this.lblActiveTracer.BackColor = System.Drawing.Color.BurlyWood;
            this.lblActiveTracer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblActiveTracer.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActiveTracer.Location = new System.Drawing.Point(2, 1);
            this.lblActiveTracer.Name = "lblActiveTracer";
            this.lblActiveTracer.Size = new System.Drawing.Size(202, 18);
            this.lblActiveTracer.TabIndex = 25;
            this.lblActiveTracer.Tag = "Active Tracer:  ";
            this.lblActiveTracer.Text = "Active Tracer:  ";
            this.lblActiveTracer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmMainTripoli
            // 
            this.AllowDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.Linen;
            this.ClientSize = new System.Drawing.Size(688, 327);
            this.Controls.Add(this.panelCorrections);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.buttonPanelCollectors);
            this.Controls.Add(this.ButtonPanelHistory);
            this.Controls.Add(this.ButtonPanelRatios);
            this.Controls.Add(this.pnlIntro);
            this.Controls.Add(this.pnlAnnotate);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.Name = "frmMainTripoli";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tripoli";
            this.Closed += new System.EventHandler(this.frmMainTripoli_Closed);
            this.SizeChanged += new System.EventHandler(this.frmMainTripoli_SizeChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmMainTripoli_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.frmMainTripoli_DragEnter);
            this.ButtonPanelRatios.ResumeLayout(false);
            this.pnlAnnotate.ResumeLayout(false);
            this.pnlAnnotate.PerformLayout();
            this.pnlIntro.ResumeLayout(false);
            this.pnlIntro.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ButtonPanelHistory.ResumeLayout(false);
            this.buttonPanelCollectors.ResumeLayout(false);
            this.statusBar1.ResumeLayout(false);
            this.statusBar1.PerformLayout();
            this.panelCorrections.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Test if input arguments were supplied:
            // only allowed are filenames *.trip, *.triphis
            string temp = null;
            if (args.Length == 1)
                temp = args[0];

            if (!TripoliUtilities.checkForTripoliUpdates(false))
            {
                try
                {
                    Form myForm = new frmMainTripoli(temp);
                    if (temp != null)
                    {
                        ((frmMainTripoli)myForm).OpenFile(temp);
                    }
                    else
                    {
                        ((frmMainTripoli)myForm).tuneGUIStatus();
                    }

                    Application.Run(myForm);
                }
                catch (Exception eee)
                {
                    MessageBox.Show("Tripoli bids you a fond farewell !"
                        + eee.Message
                        );
                }
            }
        }

        #region IonVantage created July 2011 *************************************************************************************

        private void menuItemAppendIonVantageFolder_click(object sender, EventArgs e)
        {
            // read folder of cycle data text files
            pnlAnnotate.Visible = false;
            folderBrowserDialog1.Reset();
            folderBrowserDialog1.Description = "Select LiveData Folder";
            folderBrowserDialog1.ShowNewFolderButton = false;
            folderBrowserDialog1.SelectedPath = TripoliRegistry.GetRecentIonVantageLiveDataFolder();
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                AppendIonVantageLiveDataFolder(folderBrowserDialog1.SelectedPath);
            }
        }

        public void AppendIonVantageLiveDataFolder(string folderName)
        {
            DisplayStandByMessage(true);

            MassSpecDataFile appendedDataFile = null;

            try
            {
                appendedDataFile =
                    new IonVantageLiveDataCyclesFolder(new DirectoryInfo(folderName));
            }
            catch
            {
                MessageBox.Show("Failed to Open for Append IonVantage LiveData Folder - check to see it contains cycle data files.");
            }

            if (appendedDataFile != null)
            {
                string testValid = ((IonVantageLiveDataCyclesFolder)appendedDataFile).TestFileValidity();
                if (testValid.CompareTo("TRUE") == 0)
                {
                    Boolean proceedWithAppend = true;

                    TripoliWorkProduct appendedRawRatios = ((IonVantageLiveDataCyclesFolder)appendedDataFile).LoadRatios();
                    // check sample name
                    if (proceedWithAppend && !appendedRawRatios.SampleName.Equals(RawRatios.SampleName)){
                        proceedWithAppend = false;                       
                        MessageBox.Show(
                        "The IonVantage LiveData folder that you are attempting to append "
                        + "\nhas a different Sample Name. ",
                        "Tripoli Warning");
                    }

                    // check fraction name
                    if (proceedWithAppend && !appendedRawRatios.FractionName.Equals(RawRatios.FractionName)){
                        proceedWithAppend = false;                       
                        MessageBox.Show(
                        "The IonVantage LiveData folder that you are attempting to append "
                        + "\nhas a different Fraction Name. ",
                        "Tripoli Warning");
                    }

                    // check time stamp
                    if (proceedWithAppend && appendedRawRatios.TimeStamp.CompareTo(RawRatios.TimeStamp) < 1)
                    {
                        // data has same or older time stamp, so reject and tell user
                        proceedWithAppend = false;
                        MessageBox.Show(
                        "The IonVantage LiveData folder that you are attempting to append "
                        + "\ndoes not have a newer time stamp. ",
                        "Tripoli Warning");
                    }


                    // perform automated oxide correction on new data so it matches existing in ratio names
                    appendedRawRatios.PerformOxideCorrection();

                    // check ratio map
                    for (int i = 0; i < appendedRawRatios.Count; i++)
                    {
                        if (proceedWithAppend && !RawRatios.Contains(appendedRawRatios[i]))
                        {
                            proceedWithAppend = false;
                            MessageBox.Show(
                        "The IonVantage LiveData folder that you are attempting to append "
                        + "\ndoes not contain the same functions (ratios). ",
                        "Tripoli Warning");
                        }
                    }

                    if (proceedWithAppend)
                    {
                        // walk rawratios and append
                        for (int i = 0; i < appendedRawRatios.Count; i++)
                        {
                            // equality defined in RawRatio as name equality only for now
                            if (RawRatios.Contains(appendedRawRatios[i]))
                            {
                                // concatenate data
                                RawRatio sourceRatio = (RawRatio)appendedRawRatios[i];
                                RawRatio sinkRatio = (RawRatio)RawRatios[RawRatios.IndexOf(sourceRatio)];

                                double[] sinkRatios = sinkRatio.Ratios;
                                double[] sourceRatios = sourceRatio.Ratios;
                                double[] concatRatios = new double[sinkRatios.Length + sourceRatios.Length];
                                for (int j = 0; j < sinkRatios.Length; j++)
                                    concatRatios[j] = sinkRatios[j];
                                for (int j = 0; j < sourceRatios.Length; j++)
                                    concatRatios[sinkRatios.Length + j] = sourceRatios[j];

                                sinkRatio.Ratios = concatRatios;

                                Console.WriteLine("match");

                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(
                        "The folder that you are attempting to append "
                        + "\nis not a valid IonVantage LiveData folder. ",
                        "Tripoli Warning");
                }
            }

            DisplayStandByMessage(false);
        }

        private void menuItemReadIonVantageFolder_click(object sender, EventArgs e)
        {
            // read folder of cycle data text files
            pnlAnnotate.Visible = false;
            folderBrowserDialog1.Reset();
            folderBrowserDialog1.Description = "Select LiveData Folder";
            folderBrowserDialog1.ShowNewFolderButton = false;
            folderBrowserDialog1.SelectedPath = TripoliRegistry.GetRecentIonVantageLiveDataFolder();
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                // reset mainwindow info
                TripoliFileInfo = null;

                OpenIonVantageLiveDataFolder(folderBrowserDialog1.SelectedPath);
            }
        }

        public void OpenIonVantageLiveDataFolder(string folderName)
        {
            DisplayStandByMessage(true);
            AbortCurrentDataFile();

            try
            {
                CurrentDataFile =
                    new IonVantageLiveDataCyclesFolder(new DirectoryInfo(folderName));
            }
            catch
            {
                MessageBox.Show("Failed to Open IonVantage LiveData Folder - check to see it contains cycle data files.");
                AbortCurrentDataFile();
            }

            // refresh screen
            this.Refresh();

            if (CurrentDataFile != null)
            {
                string testValid = ((IonVantageLiveDataCyclesFolder)CurrentDataFile).TestFileValidity();
                if (testValid.CompareTo("TRUE") == 0)
                {
                    saveTripoliWorkFile_menuItem.Enabled = false; //save
                    setSaveExportMenuItemsEnabled(true);
                    saveAsTripoliWorkFile_menuItem.Enabled = true; //saveas
                    menuItemAppendIonVantageFolder.Enabled = true;
                    menuWorkFileCloseFiles.Enabled = true; // close tripoli file
                    TripoliRegistry.SetRecentIonVantageLiveDataFolder(folderName);
                    CurrentDataFile.DataFileInfo.Refresh();
 //   QUESTION (July 2011) ?? belong here in this scenario                TripoliRegistry.SetRecentLiveWorkflowFileAccessTime(DateTime.Now.ToUniversalTime().ToBinary());   
                    ExtractIonVantageLiveData();
                }
                else
                {
                    MessageBox.Show(
                        "The folder that you are attempting to open "
                        + "\nis not a valid IonVantage LiveData folder. ",
                        "Tripoli Warning");
                    AbortCurrentDataFile();
                }
            }
            DisplayStandByMessage(false);
        }

        private void ExtractIonVantageLiveData()
        {
            DisplayStandByMessage(true);

            CurrentDataFileInfo = ((IonVantageLiveDataCyclesFolder)CurrentDataFile).DataFileInfo;

            RawRatios = ((IonVantageLiveDataCyclesFolder)CurrentDataFile).LoadRatios();

            if (!isInLiveWorkflow && (RawRatios == null))
            {
                AbortCurrentDataFile();
            }
            else
            {
                RawRatios.SourceFileInfo = CurrentDataFileInfo;

                // default built-in BaPO2IC Oct 2009
                if (RawRatios.CurrentBariumPhosphateIC == null)
                {
                    TripoliBariumPhosphateIC = BariumPhosphateIC.EARTHTIME_BaPO2IC();
                }
                // default built-in TlIC Oct 2009
                if (RawRatios.CurrentThalliumIC == null)
                {
                    TripoliThalliumIC = ThalliumIC.EARTHTIME_TlIC();
                }

                RawRatios.PrepareCycleSelections2011();
                DisplayCycleSelections();

                ButtonPanelRatios.Visible = true;
                SetStatusBar(CurrentDataFileInfo.FullName);
                SetTitleBar("NONE");
            }

            try
            {
                CurrentDataFile.close();
            }
            catch { }

            CurrentDataFile = null;
            DisplayStandByMessage(false);
        }


        #endregion IonVantage *************************************************************************************

        #region Sector54

        private void menuReadSector54DatFile_Click(object sender, System.EventArgs e)
        {
            // read sector54 .dat file
            pnlAnnotate.Visible = false;
            openFileDialog1.Reset();
            openFileDialog1.Title = "Select Sector 54 .dat File(s)";
            try
            {
                openFileDialog1.InitialDirectory = (new FileInfo(TripoliRegistry.GetRecentSector54DatFile())).DirectoryName;
            }
            catch { }
            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.AutoUpgradeEnabled = true;
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "dat files (*.dat)|*.dat";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // reset mainwindow info
                TripoliFileInfo = null;

                OpenSector54DatFiles(openFileDialog1.FileNames);
            }

        }

        /// <summary>
        /// Provides for concatenating .dat files
        /// </summary>
        /// <param name="fileNames"></param>
        public void OpenSector54DatFiles(string[] fileNames)
        {
            // check for more than one file
            if (fileNames.Length > 1)
            {
                // multiselect new for Nov 2009
                DisplayStandByMessage(true);
                AbortCurrentDataFile();

                // lets get the data so we can see if they are consistent for sample, fraction, timestamp
                ArrayList dataFiles = new ArrayList();
                for (int i = 0; i < fileNames.Length; i++)
                {
                    MassSpecDataFile tempCurrentDataFile = new Sector54DataFile(new FileInfo(fileNames[i]));

                    string testValid = tempCurrentDataFile.TestFileValidity();
                    if (testValid.CompareTo("TRUE") == 0)
                    {
                        // returns null for empty data files
                        TripoliWorkProduct temp = tempCurrentDataFile.LoadRatios();
                        if (temp != null)
                        {
                            temp.SourceFileInfo = tempCurrentDataFile.DataFileInfo;
                            dataFiles.Add(temp);
                        }
                    }
                }

                // determine if there really is only one file, due to empty files
                if (!isInLiveWorkflow && (dataFiles.Count == 0))
                {
                    MessageBox.Show(
                         "The  .dat files you attempted to open do not appear to contain any valid Sector54 raw data !",
                         "Tripoli Warning");
                }
                else if (dataFiles.Count == 1)
                {
                    OpenSector54DatFile(fileNames[0]);
                }
                else
                {//***************************************************************
                    concatenateDataFiles(dataFiles);
                    TripoliRegistry.SetRecentSector54DatFile(((TripoliWorkProduct)dataFiles[0]).SourceFileInfo.FullName);
                }//***************************************************************

            }
            else
            {
                OpenSector54DatFile(fileNames[0]);
            }

        }

        public void OpenSector54DatFile(string fileName)
        {
            DisplayStandByMessage(true);
            AbortCurrentDataFile();

            try
            {
                CurrentDataFile =
                    new Sector54DataFile(new FileInfo(fileName));
            }
            catch
            {
                MessageBox.Show("Failed to Open .dat File");
                AbortCurrentDataFile();
            }

            // refresh screen
            this.Refresh();

            if (CurrentDataFile != null)
            {
                string testValid = ((Sector54DataFile)CurrentDataFile).TestFileValidity();
                if (testValid.CompareTo("TRUE") == 0)
                {
                    saveTripoliWorkFile_menuItem.Enabled = false; //save
                    setSaveExportMenuItemsEnabled(true);
                    saveAsTripoliWorkFile_menuItem.Enabled = true; //saveas
                    menuWorkFileCloseFiles.Enabled = true; // close tripoli file
                    TripoliRegistry.SetRecentSector54DatFile(fileName);
                    CurrentDataFile.DataFileInfo.Refresh();
                    TripoliRegistry.SetRecentLiveWorkflowFileAccessTime(DateTime.Now.ToUniversalTime().ToBinary()); 
                    ExtractSector54Data();
                }
                else
                {
                    MessageBox.Show(
                        "The file that you are attempting to open "
                        + "\nis not a valid Sector54 .dat file. ",
                        "Tripoli Warning");
                    AbortCurrentDataFile();
                }
            }
            DisplayStandByMessage(false);

        }

        private void ExtractSector54Data()
        {
            DisplayStandByMessage(true);

            CurrentDataFileInfo = ((Sector54DataFile)CurrentDataFile).DataFileInfo;

            RawRatios = ((Sector54DataFile)CurrentDataFile).LoadRatios();

            if (!isInLiveWorkflow && (RawRatios == null))
            {
                AbortCurrentDataFile();
            }
            else
            {
                RawRatios.SourceFileInfo = CurrentDataFileInfo;

                // default built-in BaPO2IC Oct 2009
                if (RawRatios.CurrentBariumPhosphateIC == null)
                {
                    TripoliBariumPhosphateIC = BariumPhosphateIC.EARTHTIME_BaPO2IC();
                }
                // default built-in TlIC Oct 2009
                if (RawRatios.CurrentThalliumIC == null)
                {
                    TripoliThalliumIC = ThalliumIC.EARTHTIME_TlIC();
                }

                RawRatios.PrepareCycleSelections2011();
                DisplayCycleSelections();

                ButtonPanelRatios.Visible = true;
                SetStatusBar(CurrentDataFileInfo.FullName);
                SetTitleBar("NONE");
            }

            try
            {
                CurrentDataFile.close();
            }
            catch { }

            CurrentDataFile = null;
            DisplayStandByMessage(false);
        }

        #endregion Sector54


        private void concatenateDataFiles(ArrayList dataFiles)
        {
            dataFiles.Sort();

            // determine mix of files by checking sample, fraction, ratiotype timestamp
            ArrayList sampleNames = new ArrayList();
            ArrayList fractionIDs = new ArrayList();
            ArrayList ratioTypes = new ArrayList();
            ArrayList timeStamps = new ArrayList();
            for (int i = 0; i < dataFiles.Count; i++)
            {
                TripoliWorkProduct temp = (TripoliWorkProduct)dataFiles[i];
                if (!sampleNames.Contains(temp.SampleName))
                    sampleNames.Add(temp.SampleName);
                if (!fractionIDs.Contains(temp.FractionName))
                    fractionIDs.Add(temp.FractionName);
                if (!ratioTypes.Contains(temp.RatioType))
                    ratioTypes.Add(temp.RatioType);
                if (!timeStamps.Contains(temp.TimeStamp))
                    timeStamps.Add(temp.TimeStamp);
            }
            // analyze and report results
            if (sampleNames.Count * fractionIDs.Count * ratioTypes.Count > 1)
            {
                MessageBox.Show(
                    "You  chose data files representing:\n"
                    + "     " + sampleNames.Count + " Sample(s),\n"
                    + "     " + fractionIDs.Count + " fraction(s),\n"
                    + "     " + ratioTypes.Count + " element(s).\n\n"
                    + " Please narrow your choices to one of each.",
                    "Tripoli Warning");
            }
            // let's check the timestamps
            else if (timeStamps.Count != dataFiles.Count)
            {
                MessageBox.Show(
                    "At least " + (dataFiles.Count - timeStamps.Count + 1) + " of the data files you chose have the same internal time stamp.\n\n"
                     + " Please narrow your choices.",
                     "Tripoli Warning");
            }
            else
            {
                // assume we will concatenate in timestamp order
                RawRatios = (TripoliWorkProduct)dataFiles[0];

                // big assumptioon that all files are the same "shape" in terms of ratios
                for (int myFile = 1; myFile < dataFiles.Count; myFile++)
                {
                    // walk rawratios
                    for (int i = 0; i < ((TripoliWorkProduct)dataFiles[myFile]).Count; i++)
                    {
                        // equality defined in RawRatio as name equality only for now
                        if (RawRatios.Contains(((TripoliWorkProduct)dataFiles[myFile])[i]))
                        {
                            // concatenate data
                            RawRatio sourceRatio = (RawRatio)((TripoliWorkProduct)dataFiles[myFile])[i];
                            RawRatio sinkRatio = (RawRatio)RawRatios[RawRatios.IndexOf(sourceRatio)];

                            double[] sinkRatios = sinkRatio.Ratios;
                            double[] sourceRatios = sourceRatio.Ratios;
                            double[] concatRatios = new double[sinkRatios.Length + sourceRatios.Length];
                            for (int j = 0; j < sinkRatios.Length; j++)
                                concatRatios[j] = sinkRatios[j];
                            for (int j = 0; j < sourceRatios.Length; j++)
                                concatRatios[sinkRatios.Length + j] = sourceRatios[j];

                            sinkRatio.Ratios = concatRatios;

                            Console.WriteLine("match");

                        }
                    }
                }
                // TODO: Refactor because copied and modified from ExtractSector54Data() 
                CurrentDataFileInfo = RawRatios.SourceFileInfo;

                TripoliBariumPhosphateIC = BariumPhosphateIC.EARTHTIME_BaPO2IC();
                TripoliThalliumIC = ThalliumIC.EARTHTIME_TlIC();

                RawRatios.PrepareCycleSelections2011(); 
                DisplayCycleSelections();

                ButtonPanelRatios.Visible = true;
                SetStatusBar(CurrentDataFileInfo.FullName);
                SetTitleBar("NONE");

                saveTripoliWorkFile_menuItem.Enabled = false; //save
                setSaveExportMenuItemsEnabled(true);
                saveAsTripoliWorkFile_menuItem.Enabled = true; //saveas
                menuWorkFileCloseFiles.Enabled = true; // close tripoli file
               /// can't be here to serve all uses TripoliRegistry.SetRecentSector54DatFile(((TripoliWorkProduct)dataFiles[0]).SourceFileInfo.FullName);

                // output message about success
            }

            try
            {
                CurrentDataFile.close();
            }
            catch { }

            CurrentDataFile = null;
            DisplayStandByMessage(false);

        }

        #region ThermoFinniganTriton
        private void menuReadThermoFinniganTriton_Click(object sender, System.EventArgs e)
        {
            // read Thermo-Finnigan Triton.exp file file
            pnlAnnotate.Visible = false;
            openFileDialog1.Reset();
            openFileDialog1.Title = "Select Thermo-Finnigan .exp File(s)";
            try
            {
                openFileDialog1.InitialDirectory = (new FileInfo(TripoliRegistry.GetRecentThermoFinniganTritonExpFile())).DirectoryName;
            }
            catch { }

            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.AutoUpgradeEnabled = true;
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "exp files (*.exp)|*.exp";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // reset mainwindow info
                TripoliFileInfo = null;

                OpenThermoFinniganTritonExpFiles(openFileDialog1.FileNames);
            }

        }

        /// <summary>
        /// Provides for concatenating .exp files
        /// </summary>
        /// <param name="fileNames"></param>
        public void OpenThermoFinniganTritonExpFiles(string[] fileNames)
        {
            // check for more than one file
            if (fileNames.Length > 1)
            {
                // multiselect new for Dec 2009
                DisplayStandByMessage(true);
                AbortCurrentDataFile();

                // lets get the data so we can see if they are consistent for sample, fraction, timestamp
                ArrayList dataFiles = new ArrayList();
                for (int i = 0; i < fileNames.Length; i++)
                {
                    MassSpecDataFile tempCurrentDataFile = new ThermoFinniganTriton(new FileInfo(fileNames[i]));

                    string testValid = tempCurrentDataFile.TestFileValidity();
                    if (testValid.CompareTo("TRUE") == 0)
                    {
                        // returns null for empty data files
                        TripoliWorkProduct temp = tempCurrentDataFile.LoadRatios();
                        if (temp != null)
                        {
                            temp.SourceFileInfo = tempCurrentDataFile.DataFileInfo;
                            dataFiles.Add(temp);
                        }
                    }
                }

                // determine if there really is only one file, due to empty files
                if (!isInLiveWorkflow && (dataFiles.Count == 0))
                {
                    MessageBox.Show(
                         "The  .exp files you attempted to open do not appear to contain any valid ThermoFinnigan Triton raw data !",
                         "Tripoli Warning");
                }
                else if (dataFiles.Count == 1)
                {
                    OpenThermoFinniganTritonExpFile(fileNames[0]);
                }
                else
                {//***************************************************************
                    concatenateDataFiles(dataFiles);
                    TripoliRegistry.SetRecentThermoFinniganTritonExpFile(((TripoliWorkProduct)dataFiles[0]).SourceFileInfo.FullName);
                }//***************************************************************                }
            }
            else
            {
                OpenThermoFinniganTritonExpFile(fileNames[0]);
            }
        }

        private void OpenThermoFinniganTritonExpFile(string fileName)
        {
            DisplayStandByMessage(true);
            AbortCurrentDataFile();

            try
            {
                CurrentDataFile =
                    new ThermoFinniganTriton(new FileInfo(fileName));
            }
            catch
            {
                MessageBox.Show("Failed to Open .exp File");
                AbortCurrentDataFile();
            }

            // refresh screen
            this.Refresh();

            if (CurrentDataFile != null)
            {
                string testValid = ((ThermoFinniganTriton)CurrentDataFile).TestFileValidity();
                if (testValid == "TRUE")
                {
                    saveTripoliWorkFile_menuItem.Enabled = false; //save
                    setSaveExportMenuItemsEnabled(true);
                    saveAsTripoliWorkFile_menuItem.Enabled = true; //saveas
                    menuWorkFileCloseFiles.Enabled = true; // close tripoli file
                    TripoliRegistry.SetRecentThermoFinniganTritonExpFile(fileName);
                    CurrentDataFile.DataFileInfo.Refresh();
                    TripoliRegistry.SetRecentLiveWorkflowFileAccessTime(DateTime.Now.ToUniversalTime().ToBinary());
                    ExtractThermoFinniganTritonData();
                }
                else
                {
                    MessageBox.Show(
                        "The file that you are attempting to open "
                        + "\nis not a valid Thermo-Finnigan Triton .exp file. ",
                        "Tripoli Warning");
                    AbortCurrentDataFile();
                }
            }
            DisplayStandByMessage(false);
        }

        private void ExtractThermoFinniganTritonData()
        {
            DisplayStandByMessage(true);

            CurrentDataFileInfo = ((ThermoFinniganTriton)CurrentDataFile).DataFileInfo;

            RawRatios = ((ThermoFinniganTriton)CurrentDataFile).LoadRatios();

            if (!isInLiveWorkflow && (RawRatios == null))
            {
                //MessageBox.Show(
                //    "The Thermo-Finnigan Triton .exp file you attempted to open does not appear to contain any raw data !",
                //    "Tripoli Warning");

                AbortCurrentDataFile();

            }
            else
            {
                RawRatios.SourceFileInfo = CurrentDataFileInfo;

                // default built-in BaPO2IC Oct 2009
                if (RawRatios.CurrentBariumPhosphateIC == null)
                {
                    TripoliBariumPhosphateIC = BariumPhosphateIC.EARTHTIME_BaPO2IC();
                }
                // default built-in TlIC Oct 2009
                if (RawRatios.CurrentThalliumIC == null)
                {
                    TripoliThalliumIC = ThalliumIC.EARTHTIME_TlIC();
                }

                RawRatios.PrepareCycleSelections2011();
                DisplayCycleSelections();

                ButtonPanelRatios.Visible = true;
                SetStatusBar(CurrentDataFileInfo.FullName);
                SetTitleBar("NONE");
            }

            try
            {
                CurrentDataFile.close();
                tuneGUIStatus();

            }
            catch { }

            CurrentDataFile = null;
            DisplayStandByMessage(false);

            //tuneGUIStatus();

        }
        #endregion ThermoFinniganTriton

        #region IsoprobX
        /// <summary>
        /// Provides for concatenating .xls files
        /// </summary>
        /// <param name="fileNames"></param>
        public void OpenIsoprobXExcelFiles(string[] fileNames)
        {
            // check for more than one file
            if (fileNames.Length > 1)
            {
                // multiselect new for Dec 2009
                DisplayStandByMessage(true);
                AbortCurrentDataFile();

                // lets get the data so we can see if they are consistent for sample, fraction, timestamp
                ArrayList dataFiles = new ArrayList();
                for (int i = 0; i < fileNames.Length; i++)
                {
                    MassSpecDataFile tempCurrentDataFile = new MassLynxDataFile(new FileInfo(fileNames[i]));

                    string testValid = tempCurrentDataFile.TestFileValidity();
                    if (testValid.CompareTo("TRUE") == 0)
                    {
                        // returns null for empty data files
                        TripoliWorkProduct temp = tempCurrentDataFile.LoadRatios();
                        if (temp != null)
                        {
                            temp.SourceFileInfo = tempCurrentDataFile.DataFileInfo;
                            dataFiles.Add(temp);
                        }
                    }
                }

                // determine if there really is only one file, due to empty files
                if (!isInLiveWorkflow && (dataFiles.Count == 0))
                {
                    MessageBox.Show(
                         "The  .xls files you attempted to open do not appear to contain any valid IsoprobX raw data !",
                         "Tripoli Warning");
                }
                else if (dataFiles.Count == 1)
                {
                    OpenIsoprobXExcelFile(fileNames[0]);
                }
                else
                {//***************************************************************
                    concatenateDataFiles(dataFiles);
                    TripoliRegistry.SetRecentIsoprobXExcelDataFile(((TripoliWorkProduct)dataFiles[0]).SourceFileInfo.FullName);
                }//***************************************************************                }
            }
            else
            {
                OpenIsoprobXExcelFile(fileNames[0]);
            }
        }

        private void OpenIsoprobXExcelFile(string fileName)
        {
            DisplayStandByMessage(true);
            AbortCurrentDataFile();

            try
            {
                CurrentDataFile =
                    new MassLynxDataFile(new FileInfo(fileName));
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to Open .xls File because: " + e.Message);
                AbortCurrentDataFile();
            }

            // refresh screen
            this.Refresh();

            if (CurrentDataFile != null)
            {
                string testValid = ((MassLynxDataFile)CurrentDataFile).TestFileValidity();
                if (testValid == "TRIPOLI")
                {
                    MessageBox.Show(
                        "You are attempting to open a TRIPOLI copy "
                        + "\nof the original MassLynx Excel file. "
                        + "\n\nPlease use the WorkFile menu to open this file.",
                        "Tripoli Warning");
                    AbortCurrentDataFile();
                }
                else if (testValid == "TRUE")
                {
                    //TripoliWorkFile = null;
                    saveTripoliWorkFile_menuItem.Enabled = false; //save
                    setSaveExportMenuItemsEnabled(true);
                    saveAsTripoliWorkFile_menuItem.Enabled = true; //saveas
                    menuWorkFileCloseFiles.Enabled = true; // close tripoli file
                    TripoliRegistry.SetRecentIsoprobXExcelDataFile(openFileDialog1.FileName);
                    CurrentDataFile.DataFileInfo.Refresh();
                    TripoliRegistry.SetRecentLiveWorkflowFileAccessTime(DateTime.Now.ToUniversalTime().ToBinary());
                    ExtractIsoprobXData();
                }
                else
                {
                    MessageBox.Show(
                        "The file that you are attempting to open "
                        + "\nis not a MassLynx Excel file. ",
                        "Tripoli Warning");
                    AbortCurrentDataFile();
                }

            }
            DisplayStandByMessage(false);
        }

        private void ExtractIsoprobXData()
        {
            DisplayStandByMessage(true);

            // save file info for use in creating name of Tripoli work file
            CurrentDataFileInfo = ((MassLynxDataFile)CurrentDataFile).DataFileInfo;

            RawRatios = ((MassLynxDataFile)CurrentDataFile).LoadRatios();
            if (!isInLiveWorkflow && (RawRatios == null))
            {
                MessageBox.Show(
                    "The Excel file you attempted to open is NOT in the MassLynx format !",
                    "Tripoli Warning");

                AbortCurrentDataFile();
            }
            else
            {
                RawRatios.SourceFileInfo = CurrentDataFileInfo;//((MassLynxDataFile)CurrentDataFile).DataFileInfo;             

                // default built-in BaPO2IC Oct 2009
                if (RawRatios.CurrentBariumPhosphateIC == null)
                {
                    TripoliBariumPhosphateIC = BariumPhosphateIC.EARTHTIME_BaPO2IC();
                }
                // default built-in TlIC Oct 2009
                if (RawRatios.CurrentThalliumIC == null)
                {
                    TripoliThalliumIC = ThalliumIC.EARTHTIME_TlIC();
                }

                RawRatios.PrepareCycleSelections2011();
                DisplayCycleSelections();
                
                // get run parameters from MassLynx
                myParameters = ((MassLynxDataFile)CurrentDataFile).LoadRunParameters();

                ButtonPanelRatios.Visible = true;

                SetStatusBar(CurrentDataFileInfo.FullName);
                SetTitleBar("NONE");

                CurrentDataFile.close();
                CurrentDataFile = null;

            }

            DisplayStandByMessage(false);

            // tuneGUIStatus();

        }

        #endregion IsoprobX


        private void DisposeGraphs()
        {
            if (ratioGraphs != null)
            {
                for (int i = 0; i < ratioGraphs.Length; i++)
                    if (ratioGraphs[i] != null)
                    {
                        ((frmRatioGraph)ratioGraphs[i]).Close();
                        ((frmRatioGraph)ratioGraphs[i]).Dispose();
                    }
            }
            if (allGraphs != null)
            {
                allGraphs.Close();
                allGraphs = null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void DisplayCycleSelections()
        {
            DisplayStandByMessage(true);

            //if (RawRatios.RatioType != null)
            //{
            //    // Jan 2011 refactored RawRatio corrections to TripoliWorkProduct class
            //    // now called only on first pass of data processing RawRatios.PrepareCycleSelections2011();
            //}
            //else
            //{
            //    ((frmTripoliControlPanel)myTCP).Visible = false;
            //}

            tuneGUIStatus();

            // first dispose of any existing graphs
            DisposeGraphs();
            // initialize graphs
            ratioGraphs = new frmRatioGraph[RawRatios.Count];

            // init notations
            txtNotations.Text = RawRatios.Notations;

            // remove existing display summary
            try
            {
                this.Controls.Remove(panelDisplaySummary);
            }
            catch (Exception)
            {
            }
            // prepare new display summary
            // oct 2009 determine if corrections panel is visible
            int locationY = 10;

            // Jan 2010 need to force this visible setting as the auto-open feature of windows somehow stops it in tuneGui
            bool temp = ((RawRatios.AmFractionationCorrectable) || (RawRatios.AmOxideCorrected));
            panelCorrections.Visible = temp;
            if (temp)//panelCorrections.Visible)
                locationY = panelCorrections.Top + panelCorrections.Height + 2;// 55;

            panelDisplaySummary = new TripoliDisplayPanel(this.Width, this.Height, new Point(8, locationY));

            // dec 2005 use this to transmit info to panel ABOUT rawRatios
            ((TripoliDisplayPanel)panelDisplaySummary).CurrentWorkProduct = RawRatios;

            // set buttonpanel
            ButtonPanelRatios.Visible = true;
            ButtonPanelRatios.BringToFront();

            // init components based on count of ratios
            ChooseItem = new CheckBox[RawRatios.Count];
            GraphRatios = new Button[RawRatios.Count];
            ((TripoliDisplayPanel)panelDisplaySummary).SynchRatios = new Button[RawRatios.Count];
            InvertRatios = new Button[RawRatios.Count];

            int ShowIndex = 0;

            for (int index = 0; index < RawRatios.Count; index++)
            {
                // display a Inverter Button for each ratio
                InvertRatios[index] = new Button();
                InvertRatios[index].Size = new System.Drawing.Size(26, HEIGHT_OF_COMPONENT);
                InvertRatios[index].Font = new Font("Courier New", 7, FontStyle.Bold);
                InvertRatios[index].Text = "1/x";
                InvertRatios[index].Location = new System.Drawing.Point(1, RATIO_PANEL_TOP_MARGIN + ShowIndex * CONTROLS_TOP_TOP_Y + 1);
                InvertRatios[index].Name = index.ToString();
                InvertRatios[index].BackColor = Color.Cornsilk;
                // InvertRatios[index].Click += new System.EventHandler(this.btnGraph_Click);


                // display a checkbox for each ratio
                ChooseItem[index] = new CheckBox();
                // change color if fractionation corrected
                if (((RawRatio)RawRatios[index]).fractionationCorrected)
                    ChooseItem[index].BackColor = Color.Tan;
                else if (((RawRatio)RawRatios[index]).OxidationCorrected)
                {
                    ChooseItem[index].BackColor = Color.AliceBlue;
                    // jan 2008 a special color for 238/233
                    if (((RawRatio)RawRatios[index]).Name.Contains("238/233"))
                        ChooseItem[index].BackColor = Color.LightSteelBlue;
                }
                else
                    ChooseItem[index].BackColor = Color.Cornsilk;

                ChooseItem[index].Padding = new Padding(0);

                ChooseItem[index].CheckAlign = ContentAlignment.MiddleRight;
                ChooseItem[index].Checked = ((RawRatio)RawRatios[index]).IsActive;

                // June 2005 long names appear in some samples so change font for them
                if (((RawRatio)(RawRatios[index])).Name.Trim().Length < LINE_LENGTH_RATIO_NAME)
                {
                    ChooseItem[index].Size = new System.Drawing.Size(450, HEIGHT_OF_COMPONENT);
                    ChooseItem[index].Font = new Font("Courier New", 10);
                }
                else
                {
                    ChooseItem[index].Size = new System.Drawing.Size(450, HEIGHT_OF_COMPONENT + 2);
                    ChooseItem[index].Font = new Font("Courier New", 8);
                }

                // march 2008 make all fractionation corrected smaller font for two rows
                if (((RawRatio)(RawRatios[index])).fractionationCorrected)
                {
                    ChooseItem[index].Size = new System.Drawing.Size(450, HEIGHT_OF_COMPONENT + 2);
                    ChooseItem[index].Font = new Font("Courier New", 8);
                }

                ChooseItem[index].Location = new System.Drawing
                    .Point(15, RATIO_PANEL_TOP_MARGIN + ShowIndex * CONTROLS_TOP_TOP_Y);
                ChooseItem[index].Text = ((RawRatio)(RawRatios[index])).Name;
                ChooseItem[index].Name = index.ToString();
                ChooseItem[index].CheckedChanged += new System.EventHandler(this.chkChoose_CheckedChanged);

                // display a master synch button for each ratio
                ((TripoliDisplayPanel)panelDisplaySummary).SynchRatios[index] = new Button();
                ((TripoliDisplayPanel)panelDisplaySummary).SynchRatios[index].Cursor = Cursors.Hand;
                ((TripoliDisplayPanel)panelDisplaySummary).SynchRatios[index].Location =
                    new System.Drawing.Point(480, RATIO_PANEL_TOP_MARGIN + ShowIndex * CONTROLS_TOP_TOP_Y + 1);
                ((TripoliDisplayPanel)panelDisplaySummary).SynchRatios[index].Size =
                    new System.Drawing.Size(80, HEIGHT_OF_COMPONENT);
                //((TripoliDisplayPanel)panelDisplaySummary).SynchRatios[index].Width = 80;
                ((TripoliDisplayPanel)panelDisplaySummary).SynchRatios[index].Text = "Synch to me";
                ((TripoliDisplayPanel)panelDisplaySummary).SynchRatios[index].Name = index.ToString();
                if (((RawRatio)RawRatios[index]).AmSynchronizer)
                    ((TripoliDisplayPanel)panelDisplaySummary).SynchRatios[index].BackColor = Color.Red;
                else
                    ((TripoliDisplayPanel)panelDisplaySummary).SynchRatios[index].BackColor = Color.Cornsilk;
                ((TripoliDisplayPanel)panelDisplaySummary).SynchRatios[index].Enabled = //
                    ((RawRatio)RawRatios[index]).IsActive;
                ((TripoliDisplayPanel)panelDisplaySummary).SynchRatios[index].Click
                    += new System.EventHandler(this.btnSynch_Click);

                // display a graph button for each ratio
                GraphRatios[index] = new Button();
                GraphRatios[index].Cursor = Cursors.Hand;
                GraphRatios[index].Location = new System.Drawing.Point(570, RATIO_PANEL_TOP_MARGIN + ShowIndex * CONTROLS_TOP_TOP_Y + 1);
                GraphRatios[index].Text = "Graph";
                GraphRatios[index].Font = new Font("ArialBold", 10, FontStyle.Bold);
                GraphRatios[index].Name = index.ToString();
                GraphRatios[index].BackColor = Color.Cornsilk;
                GraphRatios[index].Click += new System.EventHandler(this.btnGraph_Click);


                // decide to display based on ShowAll
                if (ShowAll)
                {
                    //                  panelDisplaySummary.Controls.Add(InvertRatios[index]);
                    panelDisplaySummary.Controls.Add(ChooseItem[index]);
                    panelDisplaySummary.Controls.Add(((TripoliDisplayPanel)panelDisplaySummary).SynchRatios[index]);
                    panelDisplaySummary.Controls.Add(GraphRatios[index]);
                    ShowIndex++;
                }
                else if (((RawRatio)RawRatios[index]).IsActive)
                {
                    //                   panelDisplaySummary.Controls.Add(InvertRatios[index]);
                    panelDisplaySummary.Controls.Add(ChooseItem[index]);
                    panelDisplaySummary.Controls.Add(((TripoliDisplayPanel)panelDisplaySummary).SynchRatios[index]);
                    panelDisplaySummary.Controls.Add(GraphRatios[index]);
                    ShowIndex++;
                }
            }

            this.Controls.Add(panelDisplaySummary);
            DisplayStandByMessage(false);
            panelDisplaySummary.BringToFront();

        }

        private void tuneGUIStatus()
        {
            //menuCorrections.Enabled = (RawRatios != null);
            //    menuSettings.Enabled = (RawRatios != null);
            menuAnnotate.Enabled = (RawRatios != null);
            // menuControlPanel.Enabled = (RawRatios != null);

            // determine what is visible
            lblOxideCorrectionValue.Visible = false;


            if (RawRatios != null)
            {
                bool temp = ((RawRatios.AmFractionationCorrectable) || (RawRatios.AmOxideCorrected));
                panelCorrections.Visible = temp;
                this.ValidateChildren();

                lblActiveTracer.Visible = RawRatios.AmFractionationCorrectable;
                chkBoxUseTracer.Visible = RawRatios.AmFractionationCorrectable;
                btnFractionationCorrection.Visible = RawRatios.AmFractionationCorrectable;

                lblActiveBaPO2IC.Visible = //
                    RawRatios.AmFractionationCorrectable && RawRatios.ContainMeasuredPb201_205andPb203_205;
                lblActiveTlIC.Visible =  //
                    RawRatios.AmFractionationCorrectable && RawRatios.ContainMeasuredPb201_205andPb203_205;
                chkBoxUseIC.Visible =  //
                    RawRatios.AmFractionationCorrectable && RawRatios.ContainMeasuredPb201_205andPb203_205;

                lblAppliedTracer.Visible = RawRatios.AmFractionationCorrectable;

                lblAppliedBaPO2IC.Visible =  //
                    RawRatios.AmFractionationCorrectable && RawRatios.ContainMeasuredPb201_205andPb203_205;
                lblAppliedTlIC.Visible =  //
                    RawRatios.AmFractionationCorrectable && RawRatios.ContainMeasuredPb201_205andPb203_205;

                // default for fractionation correction only
                int panelHeight = 23;
                btnFractionationCorrection.Height = 18;


                //            MessageBox.Show("Data "
                //+ "\n\n panelCorrections.Visible = " + panelCorrections.Visible
                // + "\n\n ((Panel)panelCorrections).Visible = " + ((Panel)panelCorrections).Visible
                //                + "\n\n RawRatios.AmFractionationCorrectable = " + RawRatios.AmFractionationCorrectable
                //                + "\n\n RawRatios.AmOxideCorrected = " + RawRatios.AmOxideCorrected
                //                + "\n\n Double or = " + ((RawRatios.AmFractionationCorrectable) || (RawRatios.AmOxideCorrected))
                //                + "\n\n temp= " + temp,
                //"Tripoli INFO");

                if (RawRatios.ContainMeasuredUraniumForFractionationCorrection || RawRatios.AmOxideCorrected)
                {
                    panelHeight = 35;
                    // show oxide correction facts for uranium ratios
                    lblOxideCorrectionValue.Visible = RawRatios.AmOxideCorrected;
                    lblOxideCorrectionValue.Text =//
                        lblOxideCorrectionValue.Tag //
                        + RawRatios.r18O_16O.ToString();
                }

                // extra height for interference corrections
                if (RawRatios.ContainMeasuredPb201_205andPb203_205)
                {
                    panelHeight = 53;
                    btnFractionationCorrection.Height = 48;
                }


                panelCorrections.Height = panelHeight;


                // }

                lblActiveTracer.Text = lblActiveTracer.Tag + getActiveTripoliTracerNameAndVersion();
                lblActiveBaPO2IC.Text = lblActiveBaPO2IC.Tag + getActiveTripoliBariumPhosphateICNameAndVersion();
                lblActiveTlIC.Text = lblActiveTlIC.Tag + getActiveTripoliThalliumICNameAndVersion();

                // set default condition
                chkBoxUseTracer.Enabled = false;
                chkBoxUseIC.Enabled = false;

                //if ((RawRatios == null)||
                //    !RawRatios.AmFractionationCorrectable)
                //{
                //    lblAppliedTracer.Text = lblAppliedTracer.Tag + "NONE";
                //    lblAppliedBaPO2IC.Text = lblAppliedBaPO2IC.Tag + "NONE";
                //    lblAppliedTlIC.Text = lblAppliedTlIC.Tag + "NONE";
                //    chkBoxUseTracer.Checked = false;
                //    chkBoxUseIC.Checked = false;
                //    btnFractionationCorrection.Enabled = false;
                //    btnFractionationCorrection.Text = "No Corrections Available";
                //}
                //else
                //{
                lblAppliedTracer.Text = lblAppliedTracer.Tag + RawRatios.getCurrentTripoliTracerNameAndVersion();
                lblAppliedBaPO2IC.Text = lblAppliedBaPO2IC.Tag + RawRatios.getCurrentTripoliBariumPhosphateICNameAndVersion();
                lblAppliedTlIC.Text = lblAppliedTlIC.Tag + RawRatios.getCurrentTripoliBariumPhosphateICNameAndVersion();
                btnFractionationCorrection.Enabled = RawRatios.AmFractionationCorrectable;
                if (RawRatios.AmFractionationCorrected)
                {
                    btnFractionationCorrection.Text = "Undo \u221A'd Corrections";
                }
                else
                {
                    btnFractionationCorrection.Text = "Perform \u221A'd Corrections";
                    chkBoxUseTracer.Enabled = true;
                    chkBoxUseIC.Enabled = true;
                }
                chkBoxUseTracer.Checked = //
                    (TripoliTracer != null) &&//
                    ((RawRatios.ContainMeasuredPb202_205) ||
                        (RawRatios.ContainMeasuredUraniumForFractionationCorrection)) &&
                    (RawRatios.UseTracerCorrection);
                chkBoxUseTracer.Enabled = //
                    (TripoliTracer != null) && //
                    (RawRatios.CurrentTracer == null);

                chkBoxUseIC.Checked = //
                    (TripoliThalliumIC != null) &&//
                    (TripoliBariumPhosphateIC != null) &&//
                    (RawRatios.ContainMeasuredPb201_205andPb203_205) &&
                    (RawRatios.UseInterferenceCorrection);

                chkBoxUseIC.Enabled = //
                   (TripoliThalliumIC != null) &&//
                   (TripoliBariumPhosphateIC != null) && //
                   (RawRatios.CurrentBariumPhosphateIC == null) && //
                   (RawRatios.CurrentThalliumIC == null) &&//
                   (RawRatios.ContainMeasuredPb201_205andPb203_205) &&//
                   chkBoxUseTracer.Enabled;

                btnFractionationCorrection.Enabled = //
                    chkBoxUseTracer.Checked;
            }

        }

        private void menuItem14_Click(object sender, System.EventArgs e)
        {
            // check for save
            ExitTripoli();
            Application.Exit();
            Environment.Exit(-1);

        }

        private void chkChoose_CheckedChanged(object sender, System.EventArgs e)
        {
            int index = Convert.ToInt32(((CheckBox)sender).Name);

            // disable Synch button on uncheck
            bool tempState = ((TripoliDisplayPanel)panelDisplaySummary).SynchRatios[index].Enabled
                = ((CheckBox)sender).Checked;

            if (!tempState)
                ((TripoliDisplayPanel)panelDisplaySummary).SynchRatios[index].BackColor = Color.Cornsilk;

            // set active flag in ratio
            ((RawRatio)RawRatios[index]).IsActive = ((CheckBox)sender).Checked;

            if (!ShowAll && !tempState)
            {
                // we blow away the selection
                DisplayCycleSelections();
            }

            // kill the graph if unchecked
            if (!tempState)
            {
                try
                {
                    ((frmRatioGraph)ratioGraphs[index]).Close();
                }
                catch { }
            }

        }

        private void btnGraph_Click(object sender, System.EventArgs e)
        {
            string SampleName = "Not Saved as Tripoli File";

            // feb 2010
            if (RawRatios != null)
            {
                SampleName += "  [sample_fraction_type = " + RawRatios.getSampleNameFractionNameRatioTypeFileName() + "]";
            }

            if (TripoliFileInfo != null)
                SampleName = TripoliFileInfo.Name;

            int graphNumber = Convert.ToInt32(((Button)sender).Name);
            // init the graph

            if ((ratioGraphs[graphNumber] == null)
                ||
                (!((frmRatioGraph)ratioGraphs[graphNumber]).Created))
            {
                ratioGraphs[graphNumber] =
                    new frmRatioGraph(((RawRatio)(RawRatios[graphNumber])),
                    SampleName, RawRatios.ChauvenetsThreshold);
                ratioGraphs[graphNumber].caller = panelDisplaySummary;//  this;
                ratioGraphs[graphNumber].myGraphIndex = graphNumber;
                ratioGraphs[graphNumber].Show();
            }
            else
            {
                ((frmRatioGraph)ratioGraphs[graphNumber]).Refresh();
                ratioGraphs[graphNumber].BringToFront();

            }
        }

        private void btnSynch_Click(object sender, System.EventArgs e)
        {
            int graphNumber = Convert.ToInt32(((Button)sender).Name);

            // this prevents synchronizing to bad (ie created by another synch)
            if (((RawRatio)RawRatios[graphNumber]).HandleOutliersFlag != (int)RawRatio.HandleOutliers.Manual
                &&
                ((RawRatio)RawRatios[graphNumber]).AmSynchronized)
            {
                MessageBox.Show("Please set Outlier Selection to MANUAL because "
                    + " the outliers shown here were created by a previous"
                    + " synchronization and are not valid for this ratio.",
                    "Tripoli CAUTION");
                return;
            }

            if (((RawRatio)RawRatios[graphNumber]).HandleOutliersFlag != (int)RawRatio.HandleOutliers.Manual)
            {
                MessageBox.Show("CAUTION - the outliers created in the synchronized ratio graphs"
                    + " are NOT valid outliers and are for information only.",
                    "Tripoli CAUTION");
            }



            // here go through all the other ratios and discard same ratios

            // first recolor the buttons as memory aid and set graph synch flag
            for (int i = 0; i < ((TripoliDisplayPanel)panelDisplaySummary).SynchRatios.Length; i++)
            {
                if (i == graphNumber)
                {
                    ((TripoliDisplayPanel)panelDisplaySummary).SynchRatios[i].BackColor = Color.Red;
                    ((RawRatio)RawRatios[i]).AmSynchronizer = true;
                    ((RawRatio)RawRatios[i]).AmSynchronized = false;
                    // make button change color
                    // check if graph showing and refresh
                    if ((ratioGraphs[i] != null)
                        &&
                        (((frmRatioGraph)ratioGraphs[i]).Created))
                    {
                        ((frmRatioGraph)ratioGraphs[i]).Refresh();
                    }

                }
                else
                {
                    ((TripoliDisplayPanel)panelDisplaySummary).SynchRatios[i].BackColor = Color.Cornsilk;
                    ((RawRatio)RawRatios[i]).AmSynchronizer = false;
                    ((RawRatio)RawRatios[i]).AmSynchronized = true;
                }

            }

            for (int ratio = 0; ratio < RawRatios.Count; ratio++)
            {
                if ((ratio != graphNumber)
                    &&
                    (ChooseItem[ratio].Checked))
                {
                    ((RawRatio)(RawRatios[ratio])).SynchRatios(((RawRatio)(RawRatios[graphNumber])));
                    ((RawRatio)(RawRatios[ratio])).CalcStats();

                    // check if graph showing and refresh
                    if ((ratioGraphs[ratio] != null)
                        &&
                        (((frmRatioGraph)ratioGraphs[ratio]).Created))
                    {
                        ((frmRatioGraph)ratioGraphs[ratio]).Refresh();
                    }
                }
            }
        }

        private void FractionationCorr_CheckedChanged(object sender, System.EventArgs e)
        {
            // set fractionationCorrected for ratio
            ((RawRatio)RawRatios[Convert.ToInt32(((CheckBox)sender).Name)])
                .fractionationCorrected = ((CheckBox)sender).Checked;

        }

        private void frmMainTripoli_Closed(object sender, System.EventArgs e)
        {
            ExitTripoli();
        }

        private void ExitTripoli()
        {
            DisposeExcel();

            //if (CurrentDataFile != null)
            //{
            try
            {
                AbortCurrentDataFile();
            }
            catch { }
            //}
        }



        private void frmMainTripoli_SizeChanged(object sender, System.EventArgs e)
        {
            //override anchor when created incorrectly by user
            try
            {
                if (panelDisplaySummary != null)//.Created)
                {
                    panelDisplaySummary.Size
                        = new Size(
                        this.Width
                        - ((TripoliDisplayPanel)panelDisplaySummary).PANEL_RIGHT_OFFSET,
                        this.Height
                        - ((TripoliDisplayPanel)panelDisplaySummary).PANEL_BOT_OFFSET//
                        - ((TripoliDisplayPanel)panelDisplaySummary).Location.Y);
                }
            }
            catch { }

        }


        private void btnShowChecked_Click(object sender, System.EventArgs e)
        {
            ShowAll = !ShowAll;
            if (ShowAll)
                btnShowChecked.Text = "Show Selected";
            else
                btnShowChecked.Text = "Show All";

            DisplayCycleSelections();
        }

        private void btnSelectAll_Click(object sender, System.EventArgs e)
        {
            for (int index = 0; index < RawRatios.Count; index++)
                ((RawRatio)RawRatios[index]).IsActive = true;

            ShowAll = false; // forces button to right state
            btnShowChecked.PerformClick();
        }

        private void btnSelectNone_Click(object sender, System.EventArgs e)
        {
            for (int index = 0; index < RawRatios.Count; index++)
                ((RawRatio)RawRatios[index]).IsActive = false;

            ShowAll = false; // forces button to right state
            btnShowChecked.PerformClick();
        }


        private void saveAsTripoliWorkFile_menuItem_Click(object sender, System.EventArgs e)
        {
            saveAsTripoliWorkFile(true);
        }

        private void saveAsTripoliWorkFile(bool showSaveDialog)
        {

            if ((RawRatios != null) && (RawRatios.SourceFileInfo != null))
            {
                //////////DisplayStandByMessage(true);

                // nov 2009
                string tripoliWorkFileName = "";
                if (RawRatios.SampleName != null)//  .Length > 0)//* RawRatios.FractionName.Length * RawRatios.RatioType.Length > 0)
                {

                    // add test for IonVantage so that trip files are pushed up a level
                    string saveToDirectory = RawRatios.SourceFileInfo.DirectoryName;
                    if (RawRatios.SourceFileInfo.Directory.Name.Equals("LiveData"))
                    {
                        saveToDirectory = RawRatios.SourceFileInfo.Directory.Parent.FullName;
                    }

                    tripoliWorkFileName = //
                        saveToDirectory//
                        + @"\" //
                        + RawRatios.getSampleNameFractionNameRatioTypeFileName();
                    if (RawRatios.isPartialResult)
                    {
                        tripoliWorkFileName += "_partial";
                    }
                    else
                    {
                        // final result so we delete partial trip file
                        try
                        {
                            FileInfo tempFile = new FileInfo(tripoliWorkFileName + "_partial.trip");
                            tempFile.Delete();
                        }
                        catch (Exception)
                        {
                        }
                    }

                    tripoliWorkFileName += ".trip";
                }
                else
                {
                    tripoliWorkFileName = RawRatios.SourceFileInfo.Name + ".trip";
                }

                if (showSaveDialog)
                {
                    // mar 2010 moved to here
                    DisplayStandByMessage(true);

                    bool saveLiveWorkflowStatus = IsInLiveWorkflow;
                    if (saveLiveWorkflowStatus)
                        toggleLiveWorkflow();

                    saveFileDialog1.Reset();
                    saveFileDialog1.Title = "Save Tripoli WorkFile";
                    saveFileDialog1.Filter = ".trip files (*.trip)|*.trip";
                    saveFileDialog1.FileName = tripoliWorkFileName;
                    // add test for IonVantage so that trip files are pushed up a level
                    string saveToDirectory = RawRatios.SourceFileInfo.DirectoryName;
                    if (RawRatios.SourceFileInfo.Directory.Name.Equals("LiveData"))
                    {
                        saveToDirectory = RawRatios.SourceFileInfo.Directory.Parent.FullName;
                    }
                    saveFileDialog1.InitialDirectory = saveToDirectory;
                    saveFileDialog1.OverwritePrompt = true;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        saveTripoliFileInfo(saveFileDialog1.FileName);
                    }
                    if (saveLiveWorkflowStatus)
                        toggleLiveWorkflow();

                    // mar 2010 moved to here
                    DisplayStandByMessage(false);

                }
                else
                {
                    saveTripoliFileInfo(tripoliWorkFileName);
                }
            }
            ////////DisplayStandByMessage(false);
        }

        private void saveTripoliFileInfo(String fileFullName)
        {
            TripoliFileInfo = new FileInfo(fileFullName);

            // retrieve special field
            showAnnotationStatus(RawRatios);
            Serializer(TripoliFileInfo, RawRatios);
            TripoliRegistry.SetRecentTripoliWorkFile(TripoliFileInfo.FullName);

            saveTripoliWorkFile_menuItem.Enabled = true; //save
            setSaveExportMenuItemsEnabled(true);
            SetTitleBar(TripoliFileInfo.Name);

            // Jan 2010 rename all ratiographs
            if (allGraphs != null)
            {
                try
                {
                    allGraphs.SampleName = TripoliFileInfo.Name;
                    allGraphs.RefreshTitle();
                }
                catch (Exception)
                {
                }
            }

            for (int i = 0; i < ratioGraphs.Length; i++)
            {
                if (ratioGraphs[i] != null)
                {
                    try
                    {
                        ((frmRatioGraph)ratioGraphs[i]).SampleName = TripoliFileInfo.Name;
                        ((frmRatioGraph)ratioGraphs[i]).RefreshTitleText();
                        ((frmRatioGraph)ratioGraphs[i]).Refresh();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }


        private void btnSelectUserFunctions_Click(object sender, System.EventArgs e)
        {
            // select only owner defined functions
            for (int index = 0; index < RawRatios.Count; index++)
                ((RawRatio)RawRatios[index]).IsActive = ((RawRatio)RawRatios[index]).IsUserFunction;

            ShowAll = true; // forces button to right state for next call
            btnShowChecked.PerformClick();
        }

        private void menuItem11_Click(object sender, System.EventArgs e)
        {
            saveTripoliWorkFile();
        }

        public void saveTripoliWorkFile()
        {

            bool showSaveDialog = true;

            if (RawRatios != null)
            {
                showSaveDialog = (!RawRatios.isPartialResult || !isInLiveWorkflow); // Jan 2010 tweak to allow choice // prevent accidental overwriting
                // mar 2010 conditional on standby mess
                if (showSaveDialog)
                    DisplayStandByMessage(true);
            }

            if (TripoliFileInfo != null)
            {
                // retrieve special field
                showAnnotationStatus(RawRatios);
                Serializer(TripoliFileInfo, (TripoliWorkProduct)RawRatios);
                TripoliRegistry.SetRecentTripoliWorkFile(TripoliFileInfo.FullName);
            }
            else
                saveAsTripoliWorkFile(showSaveDialog);

            DisplayStandByMessage(false);
        }

        private void btnTileSelected_Click(object sender, System.EventArgs e)
        {
            // ********************************
            // here we tile those graphs that are selected
            // we also suppress the legend on each for readability

            TileGraphs();
        }

        private void TileGraphs()
        {
            // in response to a request from MIT, we pass along the name of the
            // tripoli file to display at the top of graphs...unless there is none yet

            DisposeGraphs();

            if (TripoliFileInfo != null)
            {
                allGraphs = new GraphPageDisplay(TripoliFileInfo.Name);
            }
            else
            {
                // feb 2010
                string SampleName = "Not Saved as Tripoli File";
                if (RawRatios != null)
                {
                    SampleName += "  [sample_fraction_type = " + RawRatios.getSampleNameFractionNameRatioTypeFileName() + "]";
                }

                allGraphs = new GraphPageDisplay(SampleName);
            }


            bool ReadyToShow = false;

            //allGraphs.Show();

            for (int graphNumber = ratioGraphs.Length - 1; graphNumber >= 0; graphNumber--)
            {
                if (((RawRatio)RawRatios[graphNumber]).IsActive)
                {
                    ratioGraphs[graphNumber] =
                        new frmRatioGraph(((RawRatio)(RawRatios[graphNumber])),
                        allGraphs.SampleName, RawRatios.ChauvenetsThreshold);
                    ratioGraphs[graphNumber].caller = panelDisplaySummary;//  this;
                    ratioGraphs[graphNumber].myGraphIndex = graphNumber;
                    ratioGraphs[graphNumber].MdiParent = allGraphs;
                    ratioGraphs[graphNumber].Show();
                    ReadyToShow = true;
                }
            }
            if (ReadyToShow)
            {
                // tile them
                allGraphs.Show();
                allGraphs.menuItem6.PerformClick();

            }

        }

        private void btnSelectRatios_Click(object sender, System.EventArgs e)
        {
            // select and show those with the famous "/" in the name
            for (int index = 0; index < RawRatios.Count; index++)
            {
                if (((RawRatio)RawRatios[index]).Name.IndexOfAny(new Char[] { '/' }) >= 0)
                    ((RawRatio)RawRatios[index]).IsActive = true;
                else
                    ((RawRatio)RawRatios[index]).IsActive = false;
            }

            ShowAll = true; // forces button to right state for next call
            btnShowChecked.PerformClick();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serialInitFile"></param>
        /// <param name="graph"></param>
        public static void Serializer(FileInfo serialInitFile, object graph)
        {
            // Open a file and serialize the object into it in binary format.
            serialInitFile.Delete();
            Stream stream = null;
            try
            {
                stream = File.Open(serialInitFile.FullName, FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();

                bformatter.Serialize(stream, graph);
                stream.Close();
            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.Message);
            }
            finally
            {
                stream.Close();
            }
        }

        #region StatusBar

        private void SetStatusBar(String name)
        {
            statusBar1.Tag = name;
            toolStripStatusLabel2.Text = " Source File: " + name;
            if (name != "NONE")
            {
                toolTip1.Active = true;
                toolTip2.Active = true;
                toolTip2.SetToolTip(statusBar1, name);
            }
            else
            {
                toolTip1.Active = false;
                toolTip2.Active = false;
            }

        }
        private void showAnnotationStatus(TripoliWorkProduct savedFile)
        {
            // Jan 2008

            // set annotations
            savedFile.Notations = txtNotations.Text;

            if (savedFile != null)
            {
                if (savedFile.Notations.Trim().ToUpper().Equals("NOTATIONS") ||
                    savedFile.Notations.Trim().ToUpper().Equals(""))
                {
                    toolStripStatusLabel1.Text = "-";
                }
                else
                {
                    toolStripStatusLabel1.Text = "A";
                }
            }

        }
        private void SetStatusBarFolder(String name)
        {
            statusBar1.Tag = name;
            toolStripStatusLabel2.Text = " Source Folder: " + name;
            if (name != "NONE")
                toolTip1.Active = true;
            else
                toolTip1.Active = false;
        }

        private void statusBar1_DoubleClick(object sender, System.EventArgs e)
        {
            // check if "NONE"
            if (((string)statusBar1.Tag).IndexOf("NONE") > -1)
            {
                return;
            }

            // check if folder or file
            DirectoryInfo di = new DirectoryInfo((string)statusBar1.Tag);
            if (di.Exists)
            {
                // try to open in explorer
                Process ExploreFolder = new Process();
                ExploreFolder.StartInfo.FileName = "Explorer";
                ExploreFolder.StartInfo.Arguments = di.FullName;
                ExploreFolder.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                ExploreFolder.Start();
            }
            else
            {
                // here we try to open the source file
                FileInfo fi = new FileInfo((string)statusBar1.Tag);

                if (!fi.Exists)
                    MessageBox.Show("This source is not available - please confirm.",
                        "Tripoli Warning");
                else
                {
                    if (fi.Extension.ToUpper() == ".DAT")
                    {
                        // open in notepad
                        Process TripoliSector54 = new Process();
                        TripoliSector54.StartInfo.FileName = "Notepad";
                        TripoliSector54.StartInfo.Arguments = fi.FullName;
                        TripoliSector54.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                        TripoliSector54.Start();

                    }
                    else if (fi.Extension.ToUpper() == ".EXP")
                    {

                        // open in excel
                        DisposeExcel();
                        ExcelObj = new Excel.ApplicationClass();
                        ExcelObj.UserControl = true;
                        ExcelObj.Visible = true;
                        ExcelObj.WindowState = Excel.XlWindowState.xlNormal;

                        try
                        {
                            ExcelObj.Workbooks.Open(
                                fi.FullName, false, true, 1,
                                "", "", true, Excel.XlPlatform.xlWindows, "\t", true, false,
                                0, true, Missing.Value, Missing.Value);
                        }
                        catch { }
                    }
                    else if (fi.Extension.ToUpper() == ".XLS")
                    {
                        // open in excel
                        DisposeExcel();
                        ExcelObj = new Excel.ApplicationClass();
                        ExcelObj.UserControl = true;
                        ExcelObj.Visible = true;
                        ExcelObj.WindowState = Excel.XlWindowState.xlNormal;

                        try
                        {
                            ExcelObj.Workbooks.Open(
                                fi.FullName, false, true, 5,
                                "", "", true, Excel.XlPlatform.xlWindows, "\t", true, false,
                                0, true, Missing.Value, Missing.Value);
                        }
                        catch { }

                    }
                }
            }
        }


        #endregion StatusBar

        private void menuItemOpenTripWorkFile_Click(object sender, System.EventArgs e)
        {
            // open serialized tripoli work file
            // extension is *.trip

            openFileDialog1.Reset();
            openFileDialog1.Title = "Select Tripoli WorkFile";
            try
            {
                openFileDialog1.InitialDirectory = (new FileInfo(TripoliRegistry.GetRecentTripoliWorkFile(1))).DirectoryName;
            }
            catch { }
            openFileDialog1.AutoUpgradeEnabled = true;
            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.Filter = ".trip files (*.trip)|*.trip";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                OpenTripoliWorkFile(openFileDialog1.FileName);
            }

        }

        private void OpenTripoliWorkFile(string fileName)
        {
            DisplayStandByMessage(true);
            AbortCurrentDataFile();

            TripoliFileInfo = new FileInfo(fileName);
            // recover the serialized form 
            Stream stream = null;
            try
            {
                stream = File.Open(TripoliFileInfo.FullName, FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();

                RawRatios = null;
                RawRatios = (TripoliWorkProduct)bformatter.Deserialize(stream);
                // this traps for accidentally opening tripoli history file
                if (RawRatios.GetType().Name != "TripoliWorkProduct")
                {
                    RawRatios = null;
                    throw (new Exception());
                }
                if (RawRatios.CurrentTracer != null)
                    TripoliTracer = RawRatios.CurrentTracer;

                // oct 2009
                if (RawRatios.CurrentBariumPhosphateIC != null)
                    TripoliBariumPhosphateIC = RawRatios.CurrentBariumPhosphateIC;
                if (RawRatios.CurrentThalliumIC != null)
                    TripoliThalliumIC = RawRatios.CurrentThalliumIC;
                // backwards compatible
                if (RawRatios.AmFractionationCorrected)
                {
                    RawRatios.UseTracerCorrection = true;
                }

                // mar 2010 ... backwards compatibility
                if (RawRatios.SampleName == null)
                    RawRatios.SampleName = "Unknown";

            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.Message);
                MessageBox.Show(
                    "Failed to open Tripoli WorkFile !\n\n"
                    + "The most likely cause is an incompatible Tracer.\n"
                    + "Open with the previous version of Tripoli \n"
                    + "and remove the fractionation correction, and then re-save. \n\n"
                    + eee.Message, //
                    "Tripoli Warning");
            }
            try
            {
                stream.Close();
            }
            catch { }

            if (RawRatios != null)
            {
                // default built-in BaPO2IC Oct 2009
                if (RawRatios.CurrentBariumPhosphateIC == null)
                {
                    TripoliBariumPhosphateIC = BariumPhosphateIC.EARTHTIME_BaPO2IC();
                }
                // default built-in TlIC Oct 2009
                if (RawRatios.CurrentThalliumIC == null)
                {
                    TripoliThalliumIC = ThalliumIC.EARTHTIME_TlIC();
                }

                RawRatios.PrepareCycleSelections2011(); 
                DisplayCycleSelections();
                ButtonPanelRatios.Visible = true;
                SetStatusBar(RawRatios.SourceFileInfo.FullName);
                SetTitleBar(TripoliFileInfo.Name);
                saveTripoliWorkFile_menuItem.Enabled = true; //save
                setSaveExportMenuItemsEnabled(true);
                saveAsTripoliWorkFile_menuItem.Enabled = true; //saveas
                menuWorkFileCloseFiles.Enabled = true; // close tripoli file

                TripoliRegistry.SetRecentTripoliWorkFile(TripoliFileInfo.FullName);

                showAnnotationStatus(RawRatios);

            }

            DisplayStandByMessage(false);
        }

        public void setSaveExportMenuItemsEnabled(bool enabled)
        {
            saveTripAndExportToRedux_menuItem.Enabled = enabled;
            saveExportToClipboardForPbMacDat_menuItem.Enabled = enabled;
            saveExportToTextFile_menuItem.Enabled = enabled;
        }

        private void SetTitleBar(string TripoliWorkFileName)
        {
            // feb 2010
            if ((TripoliWorkFileName.Equals("NONE")) && (RawRatios != null))
            {
                TripoliWorkFileName += "  [sample_fraction_type = " + RawRatios.getSampleNameFractionNameRatioTypeFileName() + "]";
            }

            this.Text = "Tripoli: " + TripoliWorkFileName;

            // aug 2011 added liveworkflow announcement
            updateTitleBarForLiveWorkFlow();

            //this.ShowInTaskbar = false;
            this.ShowInTaskbar = true;
        }

        private void updateTitleBarForLiveWorkFlow()
        {
            if (isInLiveWorkflow)
            {
                if (this.Text.Contains("  <<LiveWorkFlow Stopped>>"))
                {
                    this.Text = this.Text.Replace("  <<LiveWorkFlow Stopped>>", "  <<LiveWorkFlow Started>>");
                }
                else
                {
                    this.Text = this.Text + "  <<LiveWorkFlow Started>>";
                }
            }
            else
            {
                if (this.Text.Contains("  <<LiveWorkFlow Started>>"))
                {
                    this.Text = this.Text.Replace("  <<LiveWorkFlow Started>>", "  <<LiveWorkFlow Stopped>>");
                }
                else
                {
                    this.Text = this.Text + "  <<LiveWorkFlow Stopped>>";
                }
            }
        }

        private void menuItem9_Click_2(object sender, System.EventArgs e)
        {
            // toggle notations
            if ((RawRatios != null)
                ||
                (currentGainsFolder != null))
                pnlAnnotate.Visible = !pnlAnnotate.Visible;
            if (pnlAnnotate.Visible)
            {
                pnlAnnotate.BringToFront();
                txtNotations.Focus();
                txtNotations.Select(0, 0);
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            pnlAnnotate.Visible = false;
        }

        private void DisposeExcel()
        {
            if (ExcelObj != null)
            {
                ExcelObj.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(ExcelObj);
                ExcelObj = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }


        }

        private void AbortCurrentDataFile()
        {
            this.Refresh();

            if (CurrentDataFile != null)
            {
                try
                {
                    CurrentDataFile.close();
                }
                catch { }
            }

            if (currentGainsFolder != null)
            {
                try
                {
                    currentGainsFolder.Clear();
                }
                catch { }
            }

            CurrentDataFile = null;
            RawRatios = null;
            currentGainsFolder = null;
            TripoliFileInfo = null;

            // october 2009
            //   TripoliTracer = null;
            //    TripoliBariumPhosphateIC = null;
            //    TripoliThalliumIC = null;

            ButtonPanelRatios.Visible = false;
            ButtonPanelHistory.Visible = false;
            buttonPanelCollectors.Visible = false;

            SetStatusBar("NONE");
            SetTitleBar("NONE");

            tuneGUIStatus();

            saveTripoliWorkFile_menuItem.Enabled = false; //save
            setSaveExportMenuItemsEnabled(false);
            saveAsTripoliWorkFile_menuItem.Enabled = false; //saveas
            mnuSaveTHF.Enabled = false; //save
            mnuSaveAsTHF.Enabled = false; //saveas
            menuWorkFileCloseFiles.Enabled = false; // close tripoli file

            menuItemAppendIonVantageFolder.Enabled = false;

            //   menuSettings.Enabled = false;
            //menuCorrections.Enabled = false;

            menuAnnotate.Enabled = false;
            //  menuControlPanel.Enabled = false;

            pnlAnnotate.Visible = false;
            toolStripStatusLabel1.Text = "-";

            try
            {
                DisposeGraphs();
                if (panelDisplaySummary != null)
                {
                    ((TripoliDisplayPanel)panelDisplaySummary).DisposeGraphs();
                }
            }
            catch { }

            if (panelDisplaySummary != null)
            {
                this.Controls.Remove(panelDisplaySummary);
                try
                {
                    this.panelDisplaySummary.Dispose();
                }
                catch { }
            }

            panelCorrections.Visible = false;

            pnlIntro.BringToFront();

            if (IsInLiveWorkflow)
            {
                toggleLiveWorkflow();
            }

            this.Refresh();
        }

        #region Menus ********************************************************* Menus
        /// <summary>
        /// Read Data from MassLynx excel file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuReadMassLynxExcel_Click(object sender, System.EventArgs e)
        {
            pnlAnnotate.Visible = false;
            // Open excel data file from mass spectrometer vendor
            openFileDialog1.Reset();
            openFileDialog1.Title = "Select Excel Data File";
            try
            {
                openFileDialog1.InitialDirectory =
                    (new FileInfo(TripoliRegistry.GetRecentIsoprobXExcelDataFile())).DirectoryName;
            }
            catch { }
            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.AutoUpgradeEnabled = true;
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "IsoprobX '.xls' files (*.xls)|*.xls";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // reset mainwindow info
                TripoliFileInfo = null;

                OpenIsoprobXExcelFiles(openFileDialog1.FileNames);
            }

        }

        /// <summary>
        /// Read gains files from folder and initiate history maintenance tasks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuReadGVGainsFolder_Click(object sender, System.EventArgs e)
        {
            pnlAnnotate.Visible = false;

            folderBrowserDialog1.Reset();
            folderBrowserDialog1.Description =
                "Select the folder containing the GV GAINS files";
            folderBrowserDialog1.ShowNewFolderButton = false;
            folderBrowserDialog1.SelectedPath = TripoliRegistry.GetRecentGainsFolder();
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                // reset mainwindow info
                TripoliFileInfo = null;

                OpenGainsFolder(folderBrowserDialog1.SelectedPath);
            }
        }

        private void OpenGainsFolder(string folderName)
        {
            DisplayStandByMessage(true);
            AbortCurrentDataFile();
            TripoliRegistry.SetRecentGainsFolder(folderName);
            // june 2005
            // new approach now that this is getting serious attention
            // class TripoliWorkProduct is still parent class
            // class TripoliHistoryFolderGains will inherit and handle GAINS folder
            // 
            // Gains folder can contain excel files
            // and Tripoli historical summary of folder
            // idea is that tripoli will maintain historical summary with user
            // input of contents of gains folder
            // it may be best to store tripolized gains individually and load them
            // up as a history - then the technique will generalize to cycle data etc.

            // refresh screen
            this.Refresh();

            // discover whether there are any existing history files in the folder
            FileInfo[] tempFileInfo = (new DirectoryInfo(folderName)).GetFiles("*.triphis");

            // if there is one, then we need to deserialize it and load it
            if (tempFileInfo.Length == 0)
            {
                // NAME a new triphis file
                TripoliFileInfo = new FileInfo(folderName + @"\CCGains.triphis");

                // create TripoliHistory and serialize it
                currentGainsFolder = new TripoliHistoryFolderGains(folderName, CCGainsFileNamePattern);
                // retrieve special field
                showAnnotationStatus(currentGainsFolder);
                //currentGainsFolder.Notations = txtNotations.Text;
                Serializer(TripoliFileInfo, currentGainsFolder);
            }
            else if (tempFileInfo.Length == 1)
            {
                TripoliFileInfo = tempFileInfo[0];
            }

            else
            {
                // there is more than one tripoli history files so CHOOSE
                openFileDialog1.Reset();
                openFileDialog1.AutoUpgradeEnabled = true;
                openFileDialog1.InitialDirectory = folderName;
                openFileDialog1.Title = "Select Tripoi History .triphis File";
                openFileDialog1.ReadOnlyChecked = true;
                openFileDialog1.Filter = "triphis files (*.triphis)|*.triphis";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        TripoliFileInfo = new FileInfo(openFileDialog1.FileName);
                    }
                    catch
                    {
                        TripoliFileInfo = null;
                    }
                }
                else
                    TripoliFileInfo = null;
            }

            // recover the serialized form of history file including the case we just wrote it = test
            currentGainsFolder = null;
            if (TripoliFileInfo != null)
            {
                currentGainsFolder = TripoliWorkProduct.GetTripoliHistoryFile(TripoliFileInfo.FullName);
                if (!currentGainsFolder.GetType().Equals(typeof(TripoliHistoryFolderGains)))
                {
                    MessageBox.Show("This TripoliHistory is not a Gains History");
                    currentGainsFolder = null;
                }
            }

            if (currentGainsFolder != null)
            {
                // july 2005 to enable portability, need to check for validity of stored path
                // and alert user.
                if (!((TripoliHistoryFolderGains)currentGainsFolder).FolderInfo.Exists)
                {
                    // update folder info
                    ((TripoliHistoryFolderGains)currentGainsFolder).FolderInfo
                        = new DirectoryInfo(folderName);
                    // no notification for now
                }

                // october 2005 new efficiency = we only prepare once!!
                //((TripoliHistoryFolderGains)currentGainsFolder).PrepareGainsCollectors();

                // dec 2005 changed order of these two commands to allow adaptation to new files
                ((TripoliHistoryFolderGains)currentGainsFolder).RefreshFolder(CCGainsFileNamePattern);
                currentGainsFolder.PrepareGainsCollectors();

                // save off any new files found
                SaveTripoliHistoryFile();

                OpenTripoliHistoryFile(TripoliFileInfo.FullName);



            }


            DisplayStandByMessage(false);

        }

        /// <summary>
        /// Dec 2005 this copies the gains folder code for now = refactor later
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuReadStandardsFolder_Click(object sender, System.EventArgs e)
        {
            pnlAnnotate.Visible = false;

            folderBrowserDialog1.Reset();
            folderBrowserDialog1.Description =
                "Select the folder containing the Tripolized Standards files";
            folderBrowserDialog1.ShowNewFolderButton = false;
            folderBrowserDialog1.SelectedPath = TripoliRegistry.GetRecentStandardsFolder();
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                // reset mainwindow info
                TripoliFileInfo = null;

                openStandardsFolder(folderBrowserDialog1.SelectedPath);
            }
        }

        private void openStandardsFolder(string folderName)
        {
            DisplayStandByMessage(true);
            AbortCurrentDataFile();
            TripoliRegistry.SetRecentStandardsFolder(folderName);

            // dec 2005
            // Standards folder contains triphis files and trip files
            // TripoliHistoryFolderStandards inherits fromTripoliWorkProduct

            // refresh screen
            this.Refresh();

            // discover whether there are any existing history files in the folder
            FileInfo[] tempFileInfo = (new DirectoryInfo(folderName)).GetFiles("*.triphis");

            // if there is one, then we need to deserialize it and load it
            if (tempFileInfo.Length == 0)
            {
                // NAME a new triphis file
                TripoliFileInfo = new FileInfo(folderName + @"\Standard.triphis");

                // create TripoliHistory and serialize it
                currentGainsFolder =
                    new TripoliHistoryFolderStandards(folderName, StandardsFileNamePattern);

                // retrieve special field
                showAnnotationStatus(currentGainsFolder);
                //currentGainsFolder.Notations = txtNotations.Text;
                Serializer(TripoliFileInfo, currentGainsFolder);
            }
            else if (tempFileInfo.Length == 1)
            {
                TripoliFileInfo = tempFileInfo[0];
            }

            else
            {
                // there is more than one tripoli history files so CHOOSE
                openFileDialog1.Reset();
                openFileDialog1.AutoUpgradeEnabled = true;
                openFileDialog1.InitialDirectory = folderName;
                openFileDialog1.Title = "Select Tripoi History .triphis File";
                openFileDialog1.ReadOnlyChecked = true;
                openFileDialog1.Filter = "triphis files (*.triphis)|*.triphis";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        TripoliFileInfo = new FileInfo(openFileDialog1.FileName);
                    }
                    catch
                    {
                        TripoliFileInfo = null;
                    }
                }
                else
                    TripoliFileInfo = null;
            }

            // recover the serialized form of history file including the case we just wrote it = test
            currentGainsFolder = null;
            if (TripoliFileInfo != null)
            {
                currentGainsFolder = TripoliWorkProduct.GetTripoliHistoryFile(TripoliFileInfo.FullName);
                if (!currentGainsFolder.GetType().Equals(typeof(TripoliHistoryFolderStandards)))
                {
                    MessageBox.Show("This TripoliHistory is not a Standards History");
                    currentGainsFolder = null;
                }
            }

            if (currentGainsFolder != null)
            {
                // july 2005 to enable portability, need to check for validity of stored path
                // and alert user.
                if (!((TripoliHistoryFolderStandards)currentGainsFolder).FolderInfo.Exists)
                {
                    // update folder info
                    ((TripoliHistoryFolderStandards)currentGainsFolder).FolderInfo
                        = new DirectoryInfo(folderName);
                    // no notification for now
                }

                // dec 2005 changed order of these two commands to allow adaptation to new files
                ((TripoliHistoryFolderStandards)currentGainsFolder).RefreshFolder(StandardsFileNamePattern);
                currentGainsFolder.PrepareGainsCollectors();

                // save off any new files found
                SaveTripoliHistoryFile();

                OpenTripoliHistoryFile(TripoliFileInfo.FullName);

            }


            DisplayStandByMessage(false);

        }

        #endregion Menus



        private void DisplayPanelGainsFolder()
        {
            pnlAnnotate.Visible = false;

            if (currentGainsFolder != null)
                ((TripoliDisplayPanel)panelDisplaySummary).DisplayHistoryFiles(currentGainsFolder);

            this.Controls.Add((Panel)panelDisplaySummary);
            panelDisplaySummary.BringToFront();

            ButtonPanelHistory.Visible = true;
            ButtonPanelHistory.BringToFront();

            SetStatusBarFolder(TripoliFileInfo.DirectoryName);
            SetTitleBar(TripoliFileInfo.Name);

        }

        private void DisplayPanelCollectors()
        {
            // assume first GainsFile has the correct list of collectors
            ((TripoliDisplayPanel)panelDisplaySummary).DisplayGainsCollectors(currentGainsFolder);

            this.Controls.Add((Panel)panelDisplaySummary);
            panelDisplaySummary.BringToFront();

            buttonPanelCollectors.Visible = true;
            buttonPanelCollectors.BringToFront();

        }

        private void DisplayStandByMessage(bool flag)
        {
            this.Cursor = Cursors.WaitCursor;

            if (panelDisplaySummary != null) panelDisplaySummary.Visible = !flag;

            lblStandBy.Visible = flag;
            if (flag)
            {
                pnlIntro.BringToFront();
            }
            else
            {
                pnlIntro.SendToBack();
            }
            this.Refresh();

            this.Cursor = Cursors.Default;
        }

        private void menuWorkFileCloseFiles_Click(object sender, System.EventArgs e)
        {
            AbortCurrentDataFile();
        }



        #region GainsHistory Buttons
        private void btnSelectAllFiles_Click(object sender, System.EventArgs e)
        {
            if (currentGainsFolder != null)
            {
                for (int item = 0; item < currentGainsFolder.Count; item++)
                {
                    ((TripoliDetailFile)currentGainsFolder[item]).IsActive = true;

                    // oct 2005 now go through each collector and change status of member file
                    currentGainsFolder.setFileIsActive(item, true);
                }
                ((TripoliDisplayPanel)panelDisplaySummary).DisplayHistoryFiles(currentGainsFolder);
            }
        }


        private void btnSelectNoneFiles_Click(object sender, System.EventArgs e)
        {
            if (currentGainsFolder != null)
            {
                for (int item = 0; item < currentGainsFolder.Count; item++)
                {
                    ((TripoliDetailFile)currentGainsFolder[item]).IsActive = false;

                    // oct 2005 now go through each collector and change status of member file
                    currentGainsFolder.setFileIsActive(item, false);
                }
                ((TripoliDisplayPanel)panelDisplaySummary).DisplayHistoryFiles(currentGainsFolder);
            }
        }

        private void btnSaveHistory_Click(object sender, System.EventArgs e)
        {
            SaveTripoliHistoryFile();

        }

        private void SaveTripoliHistoryFile()
        {
            //depends on history showing
            if (currentGainsFolder != null)
            {
                //if (currentGainsFolder.GetType().Equals(typeof(TripoliHistoryFolderGains)))
                //{
                for (int item = 0; item < currentGainsFolder.Count; item++)
                    ((TripoliDetailFile)currentGainsFolder[item]).IsActiveSaved =
                        ((TripoliDetailFile)currentGainsFolder[item]).IsActive;
                //}
                //else if (currentGainsFolder.GetType().Equals(typeof(TripoliHistoryFolderStandards)))
                //{
                //	Console.WriteLine("should be saving");
                //}
            }

            // retrieve special field
            showAnnotationStatus(currentGainsFolder);
            // currentGainsFolder.Notations = txtNotations.Text;
            // save it off 
            Serializer(TripoliFileInfo, currentGainsFolder);
            TripoliRegistry.SetRecentTripoliHistoryFile(TripoliFileInfo.FullName);


        }

        private void btnSelectSaved_Click(object sender, System.EventArgs e)
        {
            if (currentGainsFolder != null)
            {
                for (int item = 0; item < currentGainsFolder.Count; item++)
                {
                    ((TripoliDetailFile)currentGainsFolder[item]).IsActive =
                        ((TripoliDetailFile)currentGainsFolder[item]).IsActiveSaved;

                    // oct 2005 now go through each collector and change status of member file
                    currentGainsFolder
                        .setFileIsActive(item, ((TripoliDetailFile)currentGainsFolder[item]).IsActiveSaved);
                }
                ((TripoliDisplayPanel)panelDisplaySummary).DisplayHistoryFiles(currentGainsFolder);
            }
        }

        private void btnSelectNewFiles_Click(object sender, System.EventArgs e)
        {
            if (currentGainsFolder != null)
            {
                for (int item = 0; item < currentGainsFolder.Count; item++)
                {
                    ((TripoliDetailFile)currentGainsFolder[item]).IsActive =
                        !((TripoliDetailFile)currentGainsFolder[item]).IsActiveSaved;

                    // oct 2005 now go through each collector and change status of member file
                    currentGainsFolder
                        .setFileIsActive(item, !((TripoliDetailFile)currentGainsFolder[item]).IsActiveSaved);
                }
                ((TripoliDisplayPanel)panelDisplaySummary).DisplayHistoryFiles(currentGainsFolder);
            }
        }

        private void btnShowCollectors_Click(object sender, System.EventArgs e)
        {
            DisplayGainsCollectors();

        }

        #endregion GainsHistory Buttons

        private void DisplayGainsCollectors()
        {
            // we can show collectors once currentGainsFolder is built
            // each member of currentGainsFolder is GainsFile object

            pnlAnnotate.Visible = false;

            if (currentGainsFolder.getCountActiveFiles() > 0)
            {
                DisplayPanelCollectors();
            }
        }



        #region Collectors Buttons



        private void btnShowGainsFolder_Click(object sender, System.EventArgs e)
        {
            DisplayPanelGainsFolder();
        }


        private void btnSelectAllCollectors_Click(object sender, System.EventArgs e)
        {
            for (int collector = 0;
                collector < currentGainsFolder.AllRatioHistories.Count;
                collector++)
            {
                ((TripoliDetailHistory)currentGainsFolder
                    .AllRatioHistories.GetByIndex(collector)).DetailHistory.IsActive = true;
                ((TripoliDisplayPanel)panelDisplaySummary).ChooseDetail[collector].Checked = true;
            }
        }


        private void btnSelectNoneCollectors_Click(object sender, System.EventArgs e)
        {
            for (int collector = 0;
                collector < currentGainsFolder.AllRatioHistories.Count;
                collector++)
            {
                ((TripoliDetailHistory)currentGainsFolder
                    .AllRatioHistories.GetByIndex(collector)).DetailHistory.IsActive = false;
                ((TripoliDisplayPanel)panelDisplaySummary).ChooseDetail[collector].Checked = false;
            }
        }

        private void btnTileSelectedCollectors_Click(object sender, System.EventArgs e)
        {
            ((TripoliDisplayPanel)panelDisplaySummary).GraphSelectedCollectors();
        }

        #endregion Collectors Buttons



        private void mnuRecentTWF_Select(object sender, System.EventArgs e)
        {
            FileInfo fi = null;

            foreach (MenuItem mi in mnuRecentTWF.MenuItems)
            {
                fi = new FileInfo(TripoliRegistry.GetRecentTripoliWorkFile(mi.Index + 1));
                if (fi.Exists)
                {
                    mi.Text = fi.FullName;
                    mi.Visible = true;
                    mi.Enabled = true;
                }
                else
                {
                    if (mi.Index == 0)
                    {
                        mi.Text = "{empty}";
                        mi.Visible = true;
                        mi.Enabled = false;
                    }
                    else
                    {
                        mi.Visible = false;
                    }
                }
            }

        }

        /// <summary>
        /// Opens file stored as text in menuitem for mru WorkFiles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnu_mruWF_Click(object sender, System.EventArgs e)
        {
            OpenTripoliWorkFile(((MenuItem)sender).Text);
        }



        private void mnuOpenTHF_Click(object sender, System.EventArgs e)
        {
            // open serialized tripoli history file
            // extension is *.triphis

            openFileDialog1.Reset();
            openFileDialog1.AutoUpgradeEnabled = true;
            openFileDialog1.Title = "Select Tripoli HistoryFile";
            try
            {
                openFileDialog1.InitialDirectory = (new FileInfo(TripoliRegistry.GetRecentTripoliHistoryFile(1))).DirectoryName;
            }
            catch { }
            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.Filter = ".triphis files (*.triphis)|*.triphis";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    OpenTripoliHistoryFile(openFileDialog1.FileName);
                }
                catch (Exception eOpen)
                {
                    MessageBox.Show("This file is not a legitimate Tripoli File.");
                }

            }
        }

        private void OpenTripoliHistoryFile(string fileName)
        {
            DisplayStandByMessage(true);
            AbortCurrentDataFile();

            ((frmTripoliControlPanel)myTCP).Visible = false;

            currentGainsFolder = TripoliWorkProduct.GetTripoliHistoryFile(fileName);

            if (currentGainsFolder != null)
            {
                TripoliFileInfo = new FileInfo(fileName);

                panelDisplaySummary = new TripoliDisplayPanel(this.Width, this.Height, new Point(8, 10));
                panelCorrections.Visible = false;

                ((TripoliDisplayPanel)panelDisplaySummary).DisplayHistoryFiles(currentGainsFolder);

                this.Controls.Add((Panel)panelDisplaySummary);
                panelDisplaySummary.BringToFront();

                ButtonPanelHistory.Visible = true;
                SetStatusBarFolder(TripoliFileInfo.DirectoryName);
                SetTitleBar(TripoliFileInfo.Name);

                mnuSaveTHF.Enabled = true; //save
                mnuSaveAsTHF.Enabled = true; //saveas
                //   menuSettings.Enabled = true;
                menuWorkFileCloseFiles.Enabled = true; // close tripoli file

                menuAnnotate.Enabled = true;

                TripoliRegistry.SetRecentTripoliHistoryFile(TripoliFileInfo.FullName);

                // init notations
                txtNotations.Text = currentGainsFolder.Notations;
                currentGainsFolder.SourceFileInfo = TripoliFileInfo;

                showAnnotationStatus(currentGainsFolder);
            }

            DisplayStandByMessage(false);

        }

        private void mnuSaveTHF_Click(object sender, System.EventArgs e)
        {
            // tripoli Historyfile save

            DisplayStandByMessage(true);
            if (TripoliFileInfo != null)
            {
                SaveTripoliHistoryFile();
            }
            else
                mnuSaveAsTHF.PerformClick();

            DisplayStandByMessage(false);
        }

        private void mnuSaveAsTHF_Click(object sender, System.EventArgs e)
        {
            // tripoli history file save-as
            DisplayStandByMessage(true);

            if ((currentGainsFolder != null) && (TripoliFileInfo != null))
            {
                saveFileDialog1.Reset();
                saveFileDialog1.Title = "Select Tripoli HistoryFile";
                saveFileDialog1.Filter = ".triphis files (*.triphis)|*.triphis";
                saveFileDialog1.FileName
                    = TripoliFileInfo.FullName;
                saveFileDialog1.OverwritePrompt = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    //this.Cursor = Cursors.WaitCursor;
                    TripoliFileInfo = new FileInfo(saveFileDialog1.FileName);
                    // retrieve special field
                    showAnnotationStatus(currentGainsFolder);
                    // currentGainsFolder.Notations = txtNotations.Text;
                    Serializer(TripoliFileInfo, currentGainsFolder);
                    TripoliRegistry.SetRecentTripoliHistoryFile(TripoliFileInfo.FullName);

                    SetTitleBar(TripoliFileInfo.Name);
                }
            }
            DisplayStandByMessage(false);
        }

        private void mnuRecentTHF_Select(object sender, System.EventArgs e)
        {
            FileInfo fi = null;

            foreach (MenuItem mi in mnuRecentTHF.MenuItems)
            {
                fi = new FileInfo(TripoliRegistry.GetRecentTripoliHistoryFile(mi.Index + 1));
                if (fi.Exists)
                {
                    mi.Text = fi.FullName;
                    mi.Visible = true;
                    mi.Enabled = true;
                }
                else
                {
                    if (mi.Index == 0)
                    {
                        mi.Text = "{empty}";
                        mi.Visible = true;
                        mi.Enabled = false;
                    }
                    else
                    {
                        mi.Visible = false;
                    }
                }
            }

        }

        /// <summary>
        /// Opens file stored as text in menuitem for mru WorkFiles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnu_mruHF_Click(object sender, System.EventArgs e)
        {
            OpenTripoliHistoryFile(((MenuItem)sender).Text);
        }

        public void saveAndExportToClipboardForPbMacdat()
        {
            saveTripoliWorkFile();

            ReportOfPbMacDat = "You have exported the means and std errors of these ratios:\r\n\r\n";
            string clipboard = "";


            Dictionary<String, RawRatio> PbMacDat = new Dictionary<String, RawRatio>();
            PbMacDat.Add("206/204", null);
            PbMacDat.Add("206/207", null);
            PbMacDat.Add("206/208", null);
            PbMacDat.Add("206/205", null);
            PbMacDat.Add("238/235", null);
            PbMacDat.Add("233/235", null);

            try
            {
                // we take the first match only ... if you have more then you made a mistake selecting
                for (int g = 0; g < RawRatios.Count; g++)
                {
                    String saveKey = null;
                    foreach (KeyValuePair<string, RawRatio> kvp in PbMacDat)
                    {
                        if ((kvp.Value == null) &&
                            ((RawRatio)RawRatios[g]).IsActive &&
                            ((RawRatio)RawRatios[g]).Name.Contains(kvp.Key) &&
                            !((RawRatio)RawRatios[g]).Name.Contains("238/233"))
                        {
                            saveKey = kvp.Key;
                            break;
                        }
                    }
                    if (saveKey != null)
                        PbMacDat[saveKey] = (RawRatio)RawRatios[g];
                }

                foreach (KeyValuePair<string, RawRatio> kvp in PbMacDat)
                {
                    if (kvp.Value != null)
                    {
                        kvp.Value.CalcStats();
                        ReportOfPbMacDat += kvp.Key + " >> " + kvp.Value.Name + "\r\n";
                        //clipboard += kvp.Value.Mean.ToString("E7")
                        //            + "\r\n"
                        //            + kvp.Value.PctStdErr.ToString("E7")
                        //            + "\r\n";
                        clipboard += kvp.Value.Mean.ToString("000000.000000000000000")
                                    + "\r\n"
                                    + kvp.Value.PctStdErr.ToString("000000.000000000000000")
                                    + "\r\n";
                    }
                    else
                        ReportOfPbMacDat += kvp.Key + " >> " + "None Found" + "\r\n";
                }
                // remove last return characters
                clipboard = clipboard.Remove(clipboard.LastIndexOf("\r\n"), 2);
            }
            catch (Exception)
            {
                //throw;
            }
            Clipboard.SetDataObject(clipboard);

            ReportOfPbMacDat += "\r\n\r\nUse Control-V to paste the values into PbMacDat.";

        }



        private void ProcessSettingsMenu(object sender, EventArgs e)
        {
            mnuSettingsGains.Enabled = (currentGainsFolder != null);

            menuItemChauvenet.Enabled = true;// (RawRatios != null);
            menuItemOxideCorrection.Enabled = true;// (RawRatios != null);

            menuItemUSampleComponents.Enabled = (RawRatios != null);
        }

        private void mnuSettingsGains_Click(object sender, System.EventArgs e)
        {
            Form gainsSetting = new frmSettingsGains();

            if (currentGainsFolder != null)
            {
                if (currentGainsFolder.GetType().Equals(typeof(TripoliHistoryFolderGains)))
                {
                    ((frmSettingsGains)gainsSetting).RejectLevelSigma =
                        ((TripoliHistoryFolderGains)currentGainsFolder).RejectLevelSigma;

                    gainsSetting.ShowDialog(this);

                    if (gainsSetting.DialogResult == DialogResult.OK)
                    {
                        // store any change
                        ((TripoliHistoryFolderGains)currentGainsFolder).RejectLevelSigma =
                            ((frmSettingsGains)gainsSetting).RejectLevelSigma;
                        ((TripoliDisplayPanel)panelDisplaySummary).RefreshOpenCollectorGraphs();
                    }
                }

            }
            gainsSetting.Dispose();
        }

        private void frmMainTripoli_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;

        }

        private void frmMainTripoli_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            OpenFile(((string[])e.Data.GetData(DataFormats.FileDrop))[0]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="showSaveDialog"></param>
        public void ExportReduxFraction(bool showSaveDialog)
        {
            //april 2007 be sure to set cursor and check for saved *************************

            // try to extract sample name and fraction id from file name
            // name and id are first strings delimited by " "
            string[] names;
            string fractionID = "NoFractionID";
            string sampleName = "NoSampleName";
            DirectoryInfo aliquotFolder = null;


            // Noah says :  sampleName<space>fractionID<space>PborU 
            // the basic idea is to split and if there are 3 or more units, take the last
            // three, toss the last one and use the remaining two
            // otherwise do the best you can and user overwrites it

            names = TripoliFileInfo.Name.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (names.Length < 2)
                names = TripoliFileInfo.Name.Split(new Char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            int len = names.Length;
            if (len >= 3)
            {
                sampleName = names[len - 3];
                fractionID = names[len - 2];
            }
            else
            {
                try
                {
                    sampleName = names[0];
                    fractionID = names[1];
                }
                catch { }
            }

            frmSaveUPbFractionDialogBox FDB = new frmSaveUPbFractionDialogBox();
            // modified nov 2009 since we added these fields to workproduct and filled them at sector54 so far
            // TODO = NASTY !!
            if (RawRatios.SampleName != null)
                if (RawRatios.SampleName.Length > 0)
                    FDB.SampleName = RawRatios.SampleName;
                else
                    FDB.SampleName = sampleName;
            else
                FDB.SampleName = sampleName;

            if (RawRatios.FractionName != null)
                if (RawRatios.FractionName.Length > 0)
                    FDB.FractionID = RawRatios.FractionName;
                else
                    FDB.FractionID = fractionID;
            else
                FDB.FractionID = fractionID;

            // save for use in live mode below
            sampleName = FDB.SampleName;
            fractionID = FDB.FractionID;

            bool result = true;
            if (showSaveDialog)
            {
                FDB.ShowDialog();
                result = (FDB.DialogResult == DialogResult.OK);
            }

            if (result)//FDB.DialogResult == DialogResult.OK)
            {
                // proceed
                UPbReduxFraction fraction =
                    new UPbReduxFraction();
                fraction.sampleName = FDB.SampleName;
                fraction.fractionID = FDB.FractionID;
                fraction.AppliedTracer = RawRatios.CurrentTracer;
                fraction.pedigree = TripoliFileInfo.FullName;

                // the mean was stored for each pair of uranium ratios
                // now the meanAlphaU is set during export fraction below
                // oxide correction
                fraction.r18O16O = RawRatios.r18O_16O;

                // export fraction to xml file
                fraction.prepareForExportToUPbRedux(RawRatios);

                // now prompt to save the file or reportOfPbMacDat that no recognized ratios
                // were selected

                if (fraction.ratioType.Equals(""))
                {
                    if (isInLiveWorkflow)
                        toggleLiveWorkflow();

                    MessageBox.Show("EITHER \n\nyou chose no Pb or U ratios used by Redux \n\nOR\n\n you chose duplicate ratio names.",
                        "Tripoli warning");
                }
                else
                {
                    RawRatios.RatioType = fraction.ratioType;
                    // bool savedCompleted = true;
                    String fractionXMLFileFullName = "";

                    if (showSaveDialog || (!isInLiveWorkflow && RawRatios.FractionName.CompareTo("") != 0))
                    {
                        saveFileDialog1.Reset();
                        saveFileDialog1.Title = "Save Exported U-Pb Redux Fraction File";
                        saveFileDialog1.Filter = ".xml files (*.xml)|*.xml";

                        // nov 2009 fixup
                        saveFileDialog1.InitialDirectory = TripoliRegistry.GetRecentUPbReduxExportFolder();//      TripoliFileInfo.DirectoryName;
                        String fractionXMLFileName = TripoliFileInfo.Name.Remove(TripoliFileInfo.Name.IndexOf(".trip"), 5);//     TripoliFileInfo.FullName.Remove(TripoliFileInfo.FullName.IndexOf(".trip"), 5);
                        fractionXMLFileFullName = TripoliRegistry.GetRecentUPbReduxExportFolder() + @"\" + fractionXMLFileName + ".xml";
                        saveFileDialog1.FileName
                            = fractionXMLFileFullName;
                        saveFileDialog1.OverwritePrompt = true;
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            //FileInfo redux = new FileInfo(saveFileDialog1.FileName);
                            fractionXMLFileFullName = saveFileDialog1.FileName;
                            fraction.SerializeXMLUPbReduxFraction(fractionXMLFileFullName);//redux.FullName);
                            //TripoliRegistry.SetRecentUPbReduxExportFolder(new FileInfo(fractionXMLFileFullName).DirectoryName);
                            TripoliRegistry.SetRecentUPbReduxExportFolder(new FileInfo(saveFileDialog1.FileName).DirectoryName);
                        }
                        //else
                        //    savedCompleted = false;
                    }
                    else
                    {
                        // nov 2009 setup autosave of xml to folder when open .dat file
                        FileInfo fractionFileInfo = RawRatios.SourceFileInfo;
                        fractionXMLFileFullName = //
                             TripoliRegistry.GetRecentUPbReduxExportFolder()//           fractionFileInfo.DirectoryName //
                            + @"\" //
                            + RawRatios.getSampleNameFractionNameRatioTypeFileName();

                        //////if (RawRatios.isPartialResult)
                        //////    fractionXMLFileFullName += "_partial";
                        fractionXMLFileFullName += ".xml";
                        //fraction.SerializeXMLUPbReduxFraction(fractionXMLFileFullName);
                    }

                    // nov 2009
                    //check for live update mode and other choices for saving copies
                    if (isInLiveWorkflow && (TripoliRegistry.GetIgnoreSampleMetaData().Equals("FALSE")))
                    {
                        //string fractionXMLFileShortName = (new FileInfo(fractionXMLFileFullName)).Name;

                        DirectoryInfo sampleMetaDataFolder = null;
                        FileInfo[] sampleMetaDataFiles = new FileInfo[0];
                        try
                        {
                            sampleMetaDataFolder = new DirectoryInfo(TripoliRegistry.GetSampleMetaDataFolder());
                            sampleMetaDataFiles = sampleMetaDataFolder.GetFiles(sampleName + ".xml", SearchOption.TopDirectoryOnly);
                            if (sampleMetaDataFiles.Length > 0)
                            {
                                try
                                {
                                    SampleMetaData smd = new SampleMetaData();
                                    smd = smd.ReadSampleMetaData(sampleMetaDataFiles[0].FullName);

                                    Console.WriteLine("SampleMetaData read: " + smd.sampleName + "  " + smd.sampleAnalysisFolderPath);

                                    // get file paths for xml
                                    String fractionFilePathForXML_Pb = smd.getFractionFilePathForXML_Pb(fractionID);
                                    String fractionFilePathForXML_U = smd.getFractionFilePathForXML_U(fractionID);

                                    // test for fraction being present
                                    if (fractionFilePathForXML_Pb.Equals(""))
                                    {
                                        // only show message if we are really needing sample meta data
                                        if (fractionID.CompareTo("") != 0)
                                        {
                                            MessageBox.Show("The meta-data for sample " + sampleName //
                                                + "\ndoes not include fraction " + fractionID //
                                                + "\n\nPlease use ET_Redux to update the meta-data or turn off Live Mode.",
                                                "Tripoli Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        }
                                    }
                                    else
                                    {

                                        // write out a marker file "CHANGED" so that Redux can detect date
                                        aliquotFolder = (new FileInfo(fractionFilePathForXML_Pb)).Directory;

                                        FileInfo changed = new FileInfo("NOTHING");
                                        try
                                        {
                                            changed = new FileInfo(aliquotFolder.FullName + @"\CHANGED");
                                            FileStream x = changed.Create();
                                            x.Close();

                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine("writing changed " + changed.FullName + "  error =  " + e.Message);
                                        }

                                        // now kill it to update folder date and leave no mess
                                        try
                                        {
                                            changed.Delete();

                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine("deleting changed " + changed.FullName + "  error =  " + e.Message);

                                        }
                                        // write the xml files for live update
                                        if (fraction.ratioType.Equals("Pb"))
                                        {
                                            fraction.SerializeXMLUPbReduxFraction(fractionFilePathForXML_Pb);
                                            TripoliRegistry.SetRecentUPbReduxExportFolder(new FileInfo(fractionFilePathForXML_Pb).DirectoryName);
                                        }
                                        else
                                        {
                                            fraction.SerializeXMLUPbReduxFraction(fractionFilePathForXML_U);
                                            TripoliRegistry.SetRecentUPbReduxExportFolder(new FileInfo(fractionFilePathForXML_U).DirectoryName);
                                        }
                                    }

                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                            else
                            {
                                // only show message if we are really needing sample meta data
                                if (fractionID.CompareTo("") != 0)
                                {
                                    MessageBox.Show("Tripoli is in LiveWorkflow and reports that "
                                            + "\nthe meta-data for sample " + sampleName //
                                            + " \ndoes not exist in the current SampleMetaData folder." //
                                            + "\n\nPlease use ET_Redux to create the meta-data.",
                                            "Tripoli Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            // no SampleMetaData folder specified
                            MessageBox.Show("Tripoli is in LiveWorkflow and reports that "
                                                     + "\nno SampleMetaData Folder has been specified." //
                                                     + "\n\nPlease use the Tripoli Control Panel to set " //
                                                     + "\nthe SampleMetaData Folder or to choose the 'ignore-option'.",
                                                     "Tripoli Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }


                    }

                    else // not in live workflow or ignoring samplemetadata
                    {
                        if (!(isInLiveWorkflow && (TripoliRegistry.GetIgnoreSampleMetaData().Equals("TRUE"))))
                        {
                            // default location for xml is in Live Workflow data folder
                            fraction.SerializeXMLUPbReduxFraction(fractionXMLFileFullName);
                            TripoliRegistry.SetRecentUPbReduxExportFolder(new FileInfo(fractionXMLFileFullName).DirectoryName);
                        }
                    }

                }
            }


        }


        // handles EARTHTIME U-Pb Redux output of fractions
        private void menuExportToUPbRedux_Click(object sender, System.EventArgs e)
        {
            saveTripAndExportXML();
        }

        public void saveTripAndExportXML()
        {
            // nov 2009 duh
            saveTripoliWorkFile();

            this.Cursor = Cursors.WaitCursor;

            try
            {
                // Jan 2010 modified to handle case where partial result opened without liveworkflow
                //ExportReduxFraction(!RawRatios.isPartialResult || !isInLiveWorkflow);

                // march 2010 refined
                // live workflow approach is user does not ineract with xml saving at all if samplemetadata exists
                ExportReduxFraction(!isInLiveWorkflow);
            }
            catch (Exception e2)
            {
                Console.WriteLine(e2.Message);
            }
            //}
            //else
            //{
            //    MessageBox.Show("Please save the Tripoli WorkFile before exporting.",
            //        "Tripoli Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void menuExportToTextFile_Click(object sender, System.EventArgs e)
        {
            exportCheckedRatiosAsDelimitedText();
        }

        /// <summary>
        /// 
        /// </summary>
        public void exportCheckedRatiosAsDelimitedText()
        {
            DisplayStandByMessage(true);

            saveTripoliWorkFile();

            if (TripoliFileInfo != null)
            {
                this.Cursor = Cursors.WaitCursor;
                string exportedContents = RawRatios.ExportRatioData(TripoliFileInfo.Name);
                this.Cursor = Cursors.Default;

                saveFileDialog1.Reset();
                saveFileDialog1.Title = "Save Tripolized Data text File";
                saveFileDialog1.Filter = ".txt files (*.txt)|*.txt";
                saveFileDialog1.FileName
                    = TripoliFileInfo.FullName.Remove(TripoliFileInfo.FullName.IndexOf(".trip"), 5) + "_TripolizedData_" + ".txt";
                saveFileDialog1.OverwritePrompt = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileInfo redux = new FileInfo(saveFileDialog1.FileName);
                    StreamWriter s = new StreamWriter(redux.FullName);
                    s.Write(exportedContents);
                    s.Flush();
                    s.Close();

                    try
                    {
                        System.Diagnostics.Process.Start(redux.FullName);
                    }
                    catch (System.UnauthorizedAccessException eFile)
                    {
                        MessageBox.Show(
                            redux.FullName + " not found ... please try again.\n\n"
                            + eFile.Message,
                            "Tripoli Warning");
                    }



                }
            }


            DisplayStandByMessage(false);

        }


        #region Menu Tracer

        /// <summary>
        /// March 2007 provide for the import and use of Tracers.  ETO = EARTHTIME.
        /// User imports a tracer for use in calculating fractionation corrections.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuImportTracerFromETO_Click(object sender, EventArgs e)
        {
            DisplayStandByMessage(true);

            // get app.config key data
            string earthTimeOrgCurrentURL = ConfigurationManager.AppSettings["EarthTimeOrgCurrentURL"];
            string earthTimeOrgCurrentDirectoryForTracerXML =
                ConfigurationManager.AppSettings["EarthTimeOrgCurrentDirectoryForTracerXML"];

            // create a new tracer object
            Tracer myTracer = new Tracer();
            string[] myTracerList = { };

            try
            {
                myTracerList = myTracer.getListOfEarthTimeTracers();
            }
            catch (Exception eFile)
            {
                MessageBox.Show(eFile.Message);
            }

            if (myTracerList.Length > 0)
            {
                // display list for user to choose
                frmSelectETOTracer selFile = new frmSelectETOTracer(myTracerList);
                selFile.ShowDialog();

                this.Refresh();

                if (!selFile.cancel)
                {
                    PrepareTripoliTracer(
                                    earthTimeOrgCurrentDirectoryForTracerXML
                                    + myTracerList[selFile.selectedIndex]);
                }
            }

            DisplayStandByMessage(false);

        }

        private void menuItemImportTracer_Click(object sender, EventArgs e)
        {
            DisplayStandByMessage(true);

            openFileDialog1.Reset();
            openFileDialog1.AutoUpgradeEnabled = true;
            openFileDialog1.Title = "Select Tracer XML file";
            openFileDialog1.InitialDirectory = TripoliRegistry.GetRecentTracersFolder();

            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.Filter = ".xml files (*.xml)|*.xml";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.Refresh();
                PrepareTripoliTracer(openFileDialog1.FileName);
            }
            DisplayStandByMessage(false);
        }

        private void PrepareTripoliTracer(string fileName)
        {
            Tripoli.earth_time_org.Tracer myTracer = new Tripoli.earth_time_org.Tracer();

            try
            {
                myTracer = (Tripoli.earth_time_org.Tracer)myTracer.ReadTracer(fileName);

                if (myTracer != null)
                    TripoliTracer = myTracer;

                try
                {
                    DisplayCycleSelections();
                }
                catch (Exception ee ){
                    Debug.Print(ee.Message);
                }

            }
            catch (Exception eFile)
            {
                // seems to catch validation errors again 
                MessageBox.Show("Please report this error! \n\n" + eFile.Message);
            }


            // tuneGUIStatus();

        }

        private void menuItemShowSelectedTracer_Click(object sender, EventArgs e)
        {
            if (TripoliTracer != null)
            {
                Form showMyTracer = new frmTracerDetails(TripoliTracer, true);
                showMyTracer.ShowDialog();
            }
        }

        private void menuItemSaveTracerLocally_Click(object sender, EventArgs e)
        {
            DisplayStandByMessage(true);

            SaveTracerLocally();

            DisplayStandByMessage(false);
        }

        private void SaveTracerLocally()
        {
            if (TripoliTracer != null)
            {
                saveFileDialog1.Reset();
                saveFileDialog1.Title = "Save Tracer as local XML file";
                saveFileDialog1.Filter = ".xml files (*.xml)|*.xml";
                saveFileDialog1.FileName
                    = "Tracer-" + TripoliTracer.tracerName + ".xml";
                saveFileDialog1.OverwritePrompt = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    TripoliTracer.SerializeXMLTracer(saveFileDialog1.FileName);

                    // validate tracer to reportOfPbMacDat any weirdness
                    try
                    {
                        TripoliTracer.ReadTracer(saveFileDialog1.FileName);
                    }
                    catch { }

                    TripoliRegistry.SetRecentTracersFolder(new FileInfo(saveFileDialog1.FileName).DirectoryName);
                }
            }
        }

        private void menuItemCreateNewTracer_Click(object sender, EventArgs e)
        {
            DisplayStandByMessage(true);

            Tripoli.earth_time_org.Tracer myTracer = new Tripoli.earth_time_org.Tracer();
            frmTracerDetails createMyTracer = new frmTracerDetails(myTracer, false);
            createMyTracer.ShowDialog();
            if (!createMyTracer.cancel)
            {
                if (myTracer != null)
                    TripoliTracer = myTracer;
                SaveTracerLocally();
            }

            DisplayStandByMessage(false);

        }

        private void menuItemEditLocalTracer_Click(object sender, EventArgs e)
        {
            DisplayStandByMessage(true);

            if (TripoliTracer != null)
            {
                frmTracerDetails editMyTracer = new frmTracerDetails(TripoliTracer, false);
                editMyTracer.ShowDialog();
                if (!editMyTracer.cancel)
                {
                    lblActiveTracer.Text = lblActiveTracer.Tag
                            + getActiveTripoliTracerNameAndVersion();

                    SaveTracerLocally();
                }
            }

            DisplayStandByMessage(false);

        }

        private void ProcessCorrectionsMenu(object sender, EventArgs e)
        {
            menuItemCorrectFraction.Enabled = false;
            menuItemShowConfirmList.Enabled = false;
            menuItemRemoveCorrection.Enabled = false;

            if ((TripoliTracer != null) && (RawRatios != null))
            {
                menuItemCorrectFraction.Enabled = !RawRatios.AmFractionationCorrected;
                menuItemRemoveCorrection.Enabled = RawRatios.AmFractionationCorrected;
                menuItemShowConfirmList.Enabled = RawRatios.AmFractionationCorrected;
            }
        }

        private void ProcessTracerMenu(object sender, EventArgs e)
        {
            menuItemApplyTracerToReCorrectFractions.Enabled = false;
            menuItemShowActiveTracerAsText.Enabled = false;
            menuItemShowActiveTracerAsHTML.Enabled = false;
            menuItemShowActiveTracerAsForm.Enabled = false;
            menuItemCreateNewTracer.Enabled = true;
            menuItemSaveTracerLocally.Enabled = false;
            menuItemEditLocalTracer.Enabled = false;


            if ((TripoliTracer != null))
            {
                menuItemApplyTracerToReCorrectFractions.Enabled = true;
                menuItemShowActiveTracerAsText.Enabled = true;  // rawRatios.AmFractionationCorrected;
                menuItemShowActiveTracerAsHTML.Enabled = true;  //  rawRatios.AmFractionationCorrected;
                menuItemShowActiveTracerAsForm.Enabled = true;  //  rawRatios.AmFractionationCorrected;
                menuItemCreateNewTracer.Enabled = true;  //  rawRatios.AmFractionationCorrected;
                menuItemSaveTracerLocally.Enabled = true;  //  rawRatios.AmFractionationCorrected;
                menuItemEditLocalTracer.Enabled = true;  //  rawRatios.AmFractionationCorrected;
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string getActiveTripoliTracerNameAndVersion()
        {
            if (TripoliTracer == null)
                return "NONE";
            else
                return TripoliTracer.getNameAndVersion();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string getActiveTripoliBariumPhosphateICNameAndVersion()
        {
            if (TripoliBariumPhosphateIC == null)
            {
                return "NONE";
            }
            else
            {
                return TripoliBariumPhosphateIC.getNameAndVersion();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string getActiveTripoliThalliumICNameAndVersion()
        {
            if (TripoliThalliumIC == null)
            {
                return "NONE";
            }
            else
            {
                return TripoliThalliumIC.getNameAndVersion();
            }
        }

        private void menuItemCorrectFraction_Click(object sender, EventArgs e)
        {
            if (TripoliTracer == null)
            {
                MessageBox.Show("Please load a Tracer using the Corrections menu.");
            }
            else
            {
                RawRatios.PerformFractionationCorrection(TripoliTracer, TripoliBariumPhosphateIC, TripoliThalliumIC);

                // force fractionation results to show
                try
                {   
                    DisplayCycleSelections();
                }
                catch { }
            }
        }

        private void menuItemRemoveCorrection_Click(object sender, EventArgs e)
        {
    
            RawRatios.RemoveFractionationCorrection();

            // force fractionation results to show
            try
            {
                //tuneGUIStatus();
                DisplayCycleSelections();
            }
            catch { }
        }

        private void menuItemShowConfirmList_Click(object sender, EventArgs e)
        {
            // write out a tempWorkProduct file and then show it in notepad
            FileInfo clist = new FileInfo("FractionationReport.txt");
            StreamWriter s = new StreamWriter(clist.FullName);
            s.Write(RawRatios.getFractionationCorrectionReport());
            s.Flush();
            s.Close();

            try
            {
                System.Diagnostics.Process.Start(clist.FullName);
            }
            catch { }

        }

        private void menuItemShowActiveTracerAsText_Click(object sender, EventArgs e)
        {
            string responseFromServer = TripoliUtilities.getTextFromURI(
                    ConfigurationManager.AppSettings["EarthTimeOrgCurrentURL"]
                    + ConfigurationManager.AppSettings["EarthTimeOrgCurrentDirectoryForXST"]
                    + @"TracerTEXT.xslt");

            TripoliTracer.RenderTracerWithXSL(responseFromServer, "tempTracer.txt");

            DisplayStandByMessage(false);
        }

        private void menuItemShowActiveTracerAsHTML_Click(object sender, EventArgs e)
        {
            string responseFromServer = TripoliUtilities.getTextFromURI(
                    ConfigurationManager.AppSettings["EarthTimeOrgCurrentURL"]
                    + ConfigurationManager.AppSettings["EarthTimeOrgCurrentDirectoryForXST"]
                    + @"TracerHTML.xslt");

            TripoliTracer.RenderTracerWithXSL(responseFromServer, "tempTracer.html");

            DisplayStandByMessage(false);
        }

        private void btnFractionationCorrection_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text.Contains("Perform"))
                menuItemCorrectFraction.PerformClick();
            else
                menuItemRemoveCorrection.PerformClick();
        }

        #endregion Menu Tracer

        #region Menu Settings

        private void menuItemChauvenet_Click(object sender, EventArgs e)
        {
            frmChauvenet myChauvenet = null;

            if (RawRatios != null)
            {
                myChauvenet =
                        new frmChauvenet(RawRatios.ChauvenetsThreshold == 0
                                            ?
                                            Convert.ToDouble(Properties.Settings.Default.ChauvenetCriterion)
                                            :
                                            RawRatios.ChauvenetsThreshold, true);
                myChauvenet.ShowDialog();
                RawRatios.ChauvenetsThreshold = myChauvenet.ChauvenetsThreshold;
            }
            else
            {
                myChauvenet =
                    new frmChauvenet(Convert.ToDouble(Properties.Settings.Default.ChauvenetCriterion), false);
                myChauvenet.ShowDialog();
            }
        }

        private void menuItemUsampleComponents_Click(object sender, EventArgs e)
        {
            frmUsampleComponents myUsampleComponents = null;
            try
            {
                myUsampleComponents = new frmUsampleComponents(RawRatios.UsampleComponents);
                myUsampleComponents.ShowDialog();
            }
            catch (Exception)
            {
            }
        }

        private void menuItemOxideCorrection_Click(object sender, EventArgs e)
        {
            frmOxideCorrection myOxideCorrection = null;

            try
            {
                myOxideCorrection = new frmOxideCorrection(RawRatios.r18O_16O, new USampleComponents());
                //new frmOxideCorrection(RawRatios.r18O_16O == 0.0m
                //                    ?
                //                    Convert.ToDecimal(Properties.Settings.Default.r18O_16O)
                //                    :
                //                    RawRatios.r18O_16O);
                myOxideCorrection.ShowDialog();
                // aug 2010 why do we need this?  set during creation of RawRatios !!! RawRatios.r18O_16O = myOxideCorrection.r18O_16O;
            }
            catch (Exception)
            {

                //throw;
            }
        }
        #endregion Menu Settings

        #region Menu EARTHTIME

        private void menuItemETOWebSite_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(ConfigurationManager.AppSettings["EarthTimeOrgWebSite"]);
            }
            catch (System.UnauthorizedAccessException eInternet)
            {
                MessageBox.Show(
                    "EARTHTIME website not found ... are you connected to the Internet?\n\n"
                    + eInternet.Message,
                    "Tripoli Warning");
            }
        }

        private void menuItemDataDictionary_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(ConfigurationManager.AppSettings["EarthTimeOrgDataDictionaryUri"]);
            }
            catch (System.UnauthorizedAccessException eInternet)
            {
                MessageBox.Show(
                    "DataDictionary not found ... are you connected to the Internet?\n\n"
                    + eInternet.Message,
                    "Tripoli Warning");
            }
        }
        #endregion Menu EarthTime

        #region Menu Help

        private void menuItemHelpText_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + @"\docs\TripoliHelp.txt");
            }
            catch (System.UnauthorizedAccessException eFile)
            {
                MessageBox.Show(
                    "TripoliHelp.txt not found ... please repair your installation.\n\n"
                    + eFile.Message,
                    "Tripoli Warning");
            }
        }

        private void menuItemRevisionHistory_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + @"\docs\ReleaseNotes.txt");
            }
            catch (System.UnauthorizedAccessException eFile)
            {
                MessageBox.Show(
                    "ReleaseNotes.txt not found ... please repair your installation.\n\n"
                    + eFile.Message,
                    "Tripoli Warning");
            }
        }

        private void menuItemCheckForUpdates_Click(object sender, EventArgs e)
        {
            TripoliUtilities.checkForTripoliUpdates(true);
        }

        private void menuItemAbout_Click(object sender, System.EventArgs e)
        {
            frmAbout myFrmAbout = new frmAbout();
            //myFrmAbout.TripoliLeaseNumber = TripoliLeaseNumber;
            //myFrmAbout.TripoliLeaseDaysRemaining = TripoliLeaseDaysRemaining;
            myFrmAbout.ShowDialog();
        }

        #endregion Menu Help


        private void menuItemLaunchKwikiExporter_Click(object sender, EventArgs e)
        {
            launchTripoliControlPanel();
        }

        private void launchTripoliControlPanel()
        {
            //if (!myTCP.)
            //    myTCP = new frmTripoliControlPanel(this);

            ((frmTripoliControlPanel)myTCP).Show();
        }

        private void chkBoxUseTracer_MouseClick(object sender, MouseEventArgs e)
        {
            RawRatios.UseTracerCorrection = !RawRatios.UseTracerCorrection;
            tuneGUIStatus();
        }

        private void chkBoxUseIC_MouseClick(object sender, MouseEventArgs e)
        {
            RawRatios.UseInterferenceCorrection = !RawRatios.UseInterferenceCorrection;
            tuneGUIStatus();
        }

        private void openSector54LiveFolder(String folderPath)
        {
            DisplayStandByMessage(true);
            AbortCurrentDataFile();
            TripoliRegistry.SetRecentSector54LiveFolder(folderPath);

            // refresh screen
            this.Refresh();

            // detect the newest file ending with .dat
            FileInfo[] tempFileInfo = (new DirectoryInfo(folderPath)).GetFiles("*.dat");

            // handle 0 and more than 0
            if (tempFileInfo.Length == 0)
            {
                // tell user folder is empty and ask if want to go into live update mode and wait
            }
            else
            {
                // find the newest file and prompt user to create and save new trip file.
                FileInfo latest = tempFileInfo[0];
                for (int i = 1; i < tempFileInfo.Length; i++)
                {
                    if (tempFileInfo[i].LastWriteTimeUtc.CompareTo(latest.LastWriteTimeUtc) > 0)
                    {
                        latest = tempFileInfo[i];
                    }
                }

                TripoliFileInfo = null;
                OpenSector54DatFile(latest.FullName);

            }

            //else
            //{
            //    // there is more than one tripoli history files so CHOOSE
            //    openFileDialog1.Reset();
            //    openFileDialog1.InitialDirectory = folderName;
            //    openFileDialog1.Title = "Select Tripoi History .triphis File";
            //    openFileDialog1.ReadOnlyChecked = true;
            //    openFileDialog1.Filter = "triphis files (*.triphis)|*.triphis";
            //    if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //    {
            //        try
            //        {
            //            TripoliFileInfo = new FileInfo(openFileDialog1.FileName);
            //        }
            //        catch
            //        {
            //            TripoliFileInfo = null;
            //        }
            //    }
            //    else
            //        TripoliFileInfo = null;
            //}
        }

        private void fileLocationsSettings_menuItem_Click(object sender, EventArgs e)
        {
            pnlAnnotate.Visible = false;

            frmWorkFlowFolder myLiveDataFolderForm =
                 new frmWorkFlowFolder();
            myLiveDataFolderForm.ShowDialog();
            myLiveDataFolderForm.Dispose();
            ((frmTripoliControlPanel)myTCP).tuneStatus();
        }

        private void menuItem14_Click_1(object sender, EventArgs e)
        {
            saveAndExportToClipboardForPbMacdat();
        }

        private void loadCurrentDatFile_menuItem_Click(object sender, EventArgs e)
        {
            loadCurrentLiveWorkflowDataFile(((frmTripoliControlPanel)myTCP).locateCurrentLiveWorkflowDataFile().FullName);
        }

        /// <summary>
        /// 
        /// </summary>
        public void loadCurrentLiveWorkflowDataFile(string currentDataFileFullName)
        {
            // this line would provide auto saving of xml without user, but for now (mar 2010), user must click save/export
            // saveTripAndExportXML();      

            saveTripoliWorkFile();
            try
            {
                // let's see if we have a file open
                // also for a saved .trip file, we need to make sure it was created with live update implemented so
                // that it has samplename, etc

                bool loadDataAsNewFile = false;
                bool saveLiveWorkFlowStatus = isInLiveWorkflow;

                TripoliWorkProduct tempWorkProduct = null;
                MassSpecDataFile tempCurrentDataFile = null;

                String saveSStext = "No Status";
                if ((RawRatios != null) && (RawRatios.SampleName != null))
                {
                    // locateCurrentLiveWorkflowDataFile file sets this label
                    saveSStext = ((frmTripoliControlPanel)myTCP).getReportStatus();
                    ((frmTripoliControlPanel)myTCP).setReportStatus("PLEASE STAND BY ...");


                    string dataTypeChoice = TripoliRegistry.GetCurrentDataTypeChoice();
                    switch (dataTypeChoice)
                    {
                        case ".xls":
                            tempCurrentDataFile = new MassLynxDataFile(new FileInfo(currentDataFileFullName));
                            break;

                        case ".exp":
                            tempCurrentDataFile = new ThermoFinniganTriton(new FileInfo(currentDataFileFullName));
                            break;

                        case ".dat":
                            tempCurrentDataFile = new Sector54DataFile(new FileInfo(currentDataFileFullName));
                            break;

                        case ".LiveDataStatus.txt":
                            // forcing up to directory level for processing
                            tempCurrentDataFile = new IonVantageLiveDataCyclesFolder(new FileInfo(currentDataFileFullName).Directory);
                            break;


                        default:
                            tempCurrentDataFile = new ThermoFinniganMat261(new FileInfo(currentDataFileFullName));
                            break;
                    }

                    // here we test if live update mode so we don't unload work, rather append work
                    string testValid = tempCurrentDataFile.TestFileValidity();
                    if (testValid.CompareTo("TRUE") == 0)
                    {
                        // returns null for empty .dat files
                        tempWorkProduct = tempCurrentDataFile.LoadRatios();
                        if (tempWorkProduct != null)
                        {
                            tempWorkProduct.SourceFileInfo = tempCurrentDataFile.DataFileInfo;
                          //  RawRatio[] saveRawRatios = null;

                            // let's verify we have same sample, fraction, element, and timestamp 
                            if ((RawRatios.SampleName.CompareTo(tempWorkProduct.SampleName) == 0) &&
                                (RawRatios.FractionName.CompareTo(tempWorkProduct.FractionName) == 0) &&
                                (RawRatios.RatioType.CompareTo(tempWorkProduct.RatioType) == 0) &&
                                (RawRatios.TimeStamp.CompareTo(tempWorkProduct.TimeStamp) == 0))
                            {

                                mergeRawRatiosWithTripoliWorkProductPreservingTripolization(tempWorkProduct);

                                // show or refresh graphs
                                // TODO: refactor this to refreshGraphs()
                                if (allGraphs != null)
                                {
                                    if (TripoliFileInfo != null)
                                    {
                                        allGraphs.SampleName = TripoliFileInfo.Name;
                                    }
                                    else
                                    {
                                        allGraphs.SampleName = "Not Saved as Tripoli File   [sample_fraction_type = " + RawRatios.getSampleNameFractionNameRatioTypeFileName() + "]";
                                    }
                                    allGraphs.RefreshTitle();

                                    for (int graphNumber = ratioGraphs.Length - 1; graphNumber >= 0; graphNumber--)
                                    {
                                        if (((RawRatio)RawRatios[graphNumber]).IsActive)
                                        {
                                            ratioGraphs[graphNumber].ActiveRatios = (RawRatio)RawRatios[graphNumber];
                                            ratioGraphs[graphNumber].ActiveRatios.CalcStats();
                                            ratioGraphs[graphNumber].SampleName = allGraphs.SampleName;
                                            ratioGraphs[graphNumber].Refresh();
                                        }
                                    }
                                }
                                else
                                {
                                    TileGraphs();
                                }

                                // copy partial status in case user loaded a partial after a #END# (final)
                                RawRatios.isPartialResult = tempWorkProduct.isPartialResult;

                                // determined by partial status on save
                                TripoliFileInfo = null;

                                updateRegistryForLiveWorkflow(tempWorkProduct);
                            }
                            else
                            {
                                // report that new .dat file does not match
                                loadDataAsNewFile = true;// (RawRatios != null);// false;
                            }
                        }
                        else
                        {
                            // tempWorkproduct was null so close
                            ((frmTripoliControlPanel)myTCP).setReportStatus(saveSStext);
                            AbortCurrentDataFile();
                            loadDataAsNewFile = true;
                        }
                    }
                    else
                    {
                        // not valid dat file 
                        // message generated elsewhere during attempted load
                        ((frmTripoliControlPanel)myTCP).setReportStatus(saveSStext);
                    }

                }
                else
                {
                    // we have no existing file or an older or incompatible file
                    loadDataAsNewFile = true;
                }


                if ((RawRatios != null) && loadDataAsNewFile)
                {
                   
                    // replace existing with new
                    RawRatios = tempWorkProduct;
                    TripoliFileInfo = null;

                    RawRatios.PrepareCycleSelections2011();
                    DisplayCycleSelections();

                    updateRegistryForLiveWorkflow(tempWorkProduct);
                    SetTitleBar("NONE");

                }
                else if (loadDataAsNewFile)
                {
                    // temporarily suspend
                    if (isInLiveWorkflow)
                        toggleLiveWorkflow();

                    // first file opened
                    TripoliFileInfo = null;

                    //String newFileName = ((frmTripoliControlPanel)myTCP).locateCurrentLiveWorkflowDataFile().FullName;

                    saveSStext = ((frmTripoliControlPanel)myTCP).getReportStatus();
                    ((frmTripoliControlPanel)myTCP).setReportStatus("PLEASE STAND BY ...");

                    string dataTypeChoice = TripoliRegistry.GetCurrentDataTypeChoice();
                    switch (dataTypeChoice)
                    {
                        case ".xls":
                            OpenIsoprobXExcelFile(currentDataFileFullName);
                            break;

                        case ".exp":
                            OpenThermoFinniganTritonExpFile(currentDataFileFullName);
                            break;

                        case ".dat":
                            OpenSector54DatFile(currentDataFileFullName);
                            break;

                        case ".LiveDataStatus.txt":
                            // forcing up to directory level for processing
                           OpenIonVantageLiveDataFolder(new FileInfo(currentDataFileFullName).Directory.FullName);
                            break;

                        default:
                            OpenThermoFinniganMat261TxtFile(currentDataFileFullName);
                            break;
                    }

                    ((frmTripoliControlPanel)myTCP).setReportStatus(saveSStext);
                    updateRegistryForLiveWorkflow(RawRatios);

                    if (saveLiveWorkFlowStatus)
                    {
                        toggleLiveWorkflow();
                        saveLiveWorkFlowStatus = false;
                    }
                }
            }
            catch (Exception e)
            {
                if (isInLiveWorkflow)
                    toggleLiveWorkflow();

                MessageBox.Show(
                        "Tripoli is not able to open this file..." + e.Message,
                        "Tripoli Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
            }

        }

        private void mergeRawRatiosWithTripoliWorkProductPreservingTripolization(//
            TripoliWorkProduct tempWorkProduct)
        {
            // save tripolized state as represented by sign of raw data points, in order to restore after live update
            // the technique is to make a copy of RawRatios, since removing fractionation correction will
            // remove renamed and created ratios; these will be restored exactly when correction is applied to update
            // relying on shallow copy because all we really want is the signs of ratios within RawRatio
            RawRatio[]  saveRawRatios = new RawRatio[RawRatios.Count];
            RawRatios.CopyTo(saveRawRatios);
            //  deep copy of ratios
            for (int j = 0; j < saveRawRatios.Length; j++)
            {
                saveRawRatios[j].SavedRatios = new double[saveRawRatios[j].Ratios.Length];
                saveRawRatios[j].Ratios.CopyTo(saveRawRatios[j].SavedRatios, 0);
            }

            // if rawratios is fractionation corrected, undo for all data
            bool wasFractionationCorrected = RawRatios.AmFractionationCorrected;
            if (RawRatios.AmFractionationCorrected)
            {
                RawRatios.RemoveFractionationCorrection();
            }

            // perform automated oxide correction on new data so it matches existing in ratio names
            tempWorkProduct.PerformOxideCorrection();

            // let's join them at the hip
            // big assumption that all files are the same "shape" in terms of ratios
            // walk through the newly read in ratios
            for (int i = 0; i < ((TripoliWorkProduct)tempWorkProduct).Count; i++)
            {
                // equality defined in RawRatio as name equality only for now
                if (RawRatios.Contains(((TripoliWorkProduct)tempWorkProduct)[i]))
                {
                    // concatenate data
                    RawRatio updatedRatio = (RawRatio)((TripoliWorkProduct)tempWorkProduct)[i];
                    RawRatio currentRatio = (RawRatio)RawRatios[RawRatios.IndexOf(updatedRatio)];

                    // copy cycles per block (needed for case of IonVantage in liveworkflow)
                    currentRatio.CyclesPerBlock = updatedRatio.CyclesPerBlock;

                    double[] currentRatios = currentRatio.Ratios;
                    double[] updatedRatios = updatedRatio.Ratios;

                    // this saves the sign of each value representing its tripolized state to reapply to extended data set
                    // after every ratio processed and fractionation correction re-applied if necessary
                    currentRatio.SavedRatios = new double[currentRatio.Ratios.Length];
                    currentRatio.Ratios.CopyTo(currentRatio.SavedRatios, 0);

                    // set ratios to new source and do statistics (see definition of property Ratios) as they include existing
                    currentRatio.Ratios = updatedRatio.Ratios;

                    //        Console.WriteLine("current and updated ratios match");
                }
            }

            // if rawratios is fractionation corrected, re-do for all data
            if (wasFractionationCorrected)
            {
                RawRatios.PerformFractionationCorrection(TripoliTracer, TripoliBariumPhosphateIC, TripoliThalliumIC);
            }

            // now reapply signs to previously tripolized data ... -1 means de-selected
            for (int i = 0; i < RawRatios.Count; i++)
            {
                RawRatio updatedRatio = (RawRatio)RawRatios[i];
                RawRatio savedRatio = (RawRatio)saveRawRatios[i];

                for (int j = 0; j < savedRatio.SavedRatios.Length; j++)
                {
                    if (savedRatio.SavedRatios[j] < 0.0)
                        updatedRatio.Ratios[j] = updatedRatio.Ratios[j] * -1.0;
                }
            }
        }

        private void updateRegistryForLiveWorkflow(TripoliWorkProduct tempWorkProduct)
        {
            if (tempWorkProduct != null)
            {
                string dataTypeChoice = TripoliRegistry.GetCurrentDataTypeChoice();
                switch (dataTypeChoice)
                {
                    case ".xls":
                        TripoliRegistry.SetRecentIsoprobXExcelDataFile(tempWorkProduct.SourceFileInfo.FullName);
                        break;

                    case ".exp":
                        TripoliRegistry.SetRecentThermoFinniganTritonExpFile(tempWorkProduct.SourceFileInfo.FullName);
                        break;

                    case ".dat":
                        TripoliRegistry.SetRecentSector54DatFile(tempWorkProduct.SourceFileInfo.FullName);
                        break;

                    case ".LiveDataStatus.txt":
                        // cant do this because we overwrote currentDataFile for this case TripoliRegistry.SetRecentIonVantageLiveDataStatusFileFolder(tempWorkProduct.SourceFileInfo.DirectoryName);
                        break;

                    default:
                        TripoliRegistry.SetRecentThermoFinniganMat261TxtFile(tempWorkProduct.SourceFileInfo.FullName);
                        break;
                }


                tempWorkProduct.SourceFileInfo.Refresh();
                TripoliRegistry.SetRecentLiveWorkflowFileAccessTime(DateTime.Now.ToUniversalTime().ToBinary());//                          tempWorkProduct.SourceFileInfo.LastWriteTimeUtc.ToBinary());
                SetStatusBar(tempWorkProduct.SourceFileInfo.FullName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool toggleLiveWorkflow()
        {
            if (isInLiveWorkflow)
            {
                isInLiveWorkflow = false;
                startStopLiveWorkflow_menuItem.Text = "Start Live Workflow";
                ((frmTripoliControlPanel)myTCP).RefreshView.Stop();
            }
            else
            {
                try
                {
                    ((frmTripoliControlPanel)myTCP).locateCurrentLiveWorkflowDataFile();
                    // moved here mar 2010
                    isInLiveWorkflow = true;
                    startStopLiveWorkflow_menuItem.Text = "Stop Live Workflow";
                    ((frmTripoliControlPanel)myTCP).RefreshView.Start();
                }
                catch (Exception)
                {
                }
            }

            updateTitleBarForLiveWorkFlow();

            ((frmTripoliControlPanel)myTCP).tuneLiveWorkflowButton(isInLiveWorkflow);
            ((frmTripoliControlPanel)myTCP).tuneSaveExportButton(isInLiveWorkflow);
            tuneSaveExportMenuItem();

            return isInLiveWorkflow;
        }

        private void tuneSaveExportMenuItem()
        {
            if (isInLiveWorkflow)
            {
                if (TripoliRegistry.GetIgnoreSampleMetaData().Equals("TRUE"))
                {
                    saveTripAndExportToRedux_menuItem.Text = "Save work with No Export";
                }
                else
                {
                    saveTripAndExportToRedux_menuItem.Text = (string)saveTripAndExportToRedux_menuItem.Tag;
                }
            }
            else
            {
                saveTripAndExportToRedux_menuItem.Text = (string)saveTripAndExportToRedux_menuItem.Tag;
            }
        }

        private void startStopLiveWorkflow_menuItem_Click(object sender, EventArgs e)
        {
            toggleLiveWorkflow();
        }

        private void BaPO2TlIsotopicComposition_menuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Form myBaPO2Composition = //
                    new frmBaPO2_Tl_IsotopicComposition(//
                        TripoliBariumPhosphateIC,
                        TripoliThalliumIC,
                        RawRatios);

                myBaPO2Composition.ShowDialog(this);
            }
            catch (Exception)
            {

            }
        }

        private void menuItemShowActiveBaPO2_Click(object sender, EventArgs e)
        {
            if (TripoliBariumPhosphateIC != null)
            {
                Form showMyBaPO2 = new frmBariumPhosphateICDetails(TripoliBariumPhosphateIC, true);
                showMyBaPO2.ShowDialog();
            }
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(ConfigurationManager.AppSettings["CIRDLESWebSite"]);
            }
            catch (System.UnauthorizedAccessException eInternet)
            {
                MessageBox.Show(
                    "CIRDLES website not found ... are you connected to the Internet?\n\n"
                    + eInternet.Message,
                    "Tripoli Warning");
            }
        }

        private void menuItemShowActiveTlIC_Click(object sender, EventArgs e)
        {
            if (TripoliThalliumIC != null)
            {
                Form showMyTl = new frmThalliumICDetails(TripoliThalliumIC, true);
                showMyTl.ShowDialog();
            }
        }

        private void setLiveWorkflowDataFolder_menuItem_Click(object sender, EventArgs e)
        {
            pnlAnnotate.Visible = false;
            frmWorkFlowFolder myLiveDataFolderForm = new frmWorkFlowFolder();
            myLiveDataFolderForm.ShowDialog();
            myLiveDataFolderForm.Dispose();
            ((frmTripoliControlPanel)myTCP).tuneStatus();
            ((frmTripoliControlPanel)myTCP).tuneSaveExportButton(isInLiveWorkflow);
            tuneSaveExportMenuItem();
        }

        private void menuItemCredits_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + @"\docs\TripoliCredits.txt");
            }
            catch (System.UnauthorizedAccessException eFile)
            {
                MessageBox.Show(
                    "TripoliCredits.txt not found ... please repair your installation.\n\n"
                    + eFile.Message,
                    "Tripoli Warning");
            }
        }



        #region ThermoFinniganMat261
        // jan 2010  based on data from 
        //    Andrew Kylander-Clark
        //      Development Engineer
        //      Dept. of Earth Science
        //      UC Santa Barbara
        //      805-893-7097
        // March 2015 - modified for Mat262 per files supplied by Lars Elvind Augland

        private void menuReadThermoFinniganMat261TxtFile_Click(object sender, System.EventArgs e)
        {
            // read ThermoFinniganMat261 .txt file
            pnlAnnotate.Visible = false;
            openFileDialog1.Reset();
            openFileDialog1.Title = "Select ThermoFinnigan Mat261 or Mat262 .txt File(s)";
            try
            {
                openFileDialog1.InitialDirectory = (new FileInfo(TripoliRegistry.GetRecentThermoFinniganMat261TxtFile())).DirectoryName;
            }
            catch { }
            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.AutoUpgradeEnabled = true;
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // reset mainwindow info
                TripoliFileInfo = null;

                OpenThermoFinniganMat261TxtFiles(openFileDialog1.FileNames);
            }

        }

        /// <summary>
        /// Provides for concatenating .txt files
        /// </summary>
        /// <param name="fileNames"></param>
        public void OpenThermoFinniganMat261TxtFiles(string[] fileNames)
        {
            // check for more than one file
            if (fileNames.Length > 1)
            {
                // multiselect new for Nov 2009
                DisplayStandByMessage(true);
                AbortCurrentDataFile();

                // lets get the data so we can see if they are consistent for sample, fraction, timestamp
                ArrayList dataFiles = new ArrayList();
                for (int i = 0; i < fileNames.Length; i++)
                {
                    MassSpecDataFile tempCurrentDataFile = new ThermoFinniganMat261(new FileInfo(fileNames[i]));

                    string testValid = tempCurrentDataFile.TestFileValidity();
                    if (testValid.CompareTo("TRUE") == 0)
                    {
                        // returns null for empty data files
                        TripoliWorkProduct temp = tempCurrentDataFile.LoadRatios();
                        if (temp != null)
                        {
                            temp.SourceFileInfo = tempCurrentDataFile.DataFileInfo;
                            dataFiles.Add(temp);
                        }
                    }
                }

                // determine if there really is only one file, due to empty files
                if (!isInLiveWorkflow && (dataFiles.Count == 0))
                {
                    MessageBox.Show(
                         "The  .txt files you attempted to open do not appear to contain any valid ThermoFinnigan Mat 261 raw data !",
                         "Tripoli Warning");
                }
                else if (dataFiles.Count == 1)
                {
                    OpenThermoFinniganMat261TxtFile(fileNames[0]);
                }
                else
                {//***************************************************************
                    concatenateDataFiles(dataFiles);
                    TripoliRegistry.SetRecentThermoFinniganMat261TxtFile(((TripoliWorkProduct)dataFiles[0]).SourceFileInfo.FullName);
                }//***************************************************************

            }
            else
            {
                OpenThermoFinniganMat261TxtFile(fileNames[0]);
            }

        }

        public void OpenThermoFinniganMat261TxtFile(string fileName)
        {
            DisplayStandByMessage(true);
            AbortCurrentDataFile();

            try
            {
                CurrentDataFile =
                    new ThermoFinniganMat261(new FileInfo(fileName));
            }
            catch
            {
                MessageBox.Show("Failed to Open .txt File");
                AbortCurrentDataFile();
            }

            // refresh screen
            this.Refresh();

            if (CurrentDataFile != null)
            {
                string testValid = ((ThermoFinniganMat261)CurrentDataFile).TestFileValidity();
                if (testValid.CompareTo("TRUE") == 0)
                {
                    saveTripoliWorkFile_menuItem.Enabled = false; //save
                    setSaveExportMenuItemsEnabled(true);
                    saveAsTripoliWorkFile_menuItem.Enabled = true; //saveas
                    menuWorkFileCloseFiles.Enabled = true; // close tripoli file
                    TripoliRegistry.SetRecentThermoFinniganMat261TxtFile(fileName);
                    CurrentDataFile.DataFileInfo.Refresh();
                    TripoliRegistry.SetRecentLiveWorkflowFileAccessTime(DateTime.Now.ToUniversalTime().ToBinary());
                    ExtractThermoFinniganMat261Data();
                }
                else
                {
                    MessageBox.Show(
                        "The file that you are attempting to open "
                        + "\nis not a valid ThermoFinnigan Mat 261 .txt file. ",
                        "Tripoli Warning");
                    AbortCurrentDataFile();
                }
            }
            DisplayStandByMessage(false);

        }

        private void ExtractThermoFinniganMat261Data()
        {
            DisplayStandByMessage(true);

            CurrentDataFileInfo = ((ThermoFinniganMat261)CurrentDataFile).DataFileInfo;

            RawRatios = ((ThermoFinniganMat261)CurrentDataFile).LoadRatios();

            if (!isInLiveWorkflow && (RawRatios == null))
            {
                //MessageBox.Show(
                //    "The ThermoFinnigan Mat 261 .txt file you attempted to open does not appear to contain any raw data !",
                //    "Tripoli Warning");

                //ClearSelections();
                AbortCurrentDataFile();
            }
            else
            {
                RawRatios.SourceFileInfo = CurrentDataFileInfo;//CurrentDataFile.DataFileInfo;

                // default built-in BaPO2IC Oct 2009
                if (RawRatios.CurrentBariumPhosphateIC == null)
                {
                    TripoliBariumPhosphateIC = BariumPhosphateIC.EARTHTIME_BaPO2IC();
                }
                // default built-in TlIC Oct 2009
                if (RawRatios.CurrentThalliumIC == null)
                {
                    TripoliThalliumIC = ThalliumIC.EARTHTIME_TlIC();
                }

                RawRatios.PrepareCycleSelections2011();
                DisplayCycleSelections();

                ButtonPanelRatios.Visible = true;
                SetStatusBar(CurrentDataFileInfo.FullName);
                SetTitleBar("NONE");

                //    tuneGUIStatus();
            }

            try
            {
                CurrentDataFile.close();
            }
            catch { }

            CurrentDataFile = null;
            DisplayStandByMessage(false);
        }

        #endregion ThermoFinniganMat261

        #region NuPlasma
        // jan 2010  based on data from 
        //    George Gehrels
        //    University of Arizona

        private void menuReadNuPlasmaTxtFile_Click(object sender, System.EventArgs e)
        {
            // read NuPlasma .txt file
            pnlAnnotate.Visible = false;
            openFileDialog1.Reset();
            openFileDialog1.Title = "Select NuPlasma .txt File(s)";
            try
            {
                openFileDialog1.InitialDirectory = (new FileInfo(TripoliRegistry.GetRecentNuPlasmaTxtFile())).DirectoryName;
            }
            catch { }
            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.AutoUpgradeEnabled = true;
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // reset mainwindow info
                TripoliFileInfo = null;

                OpenNuPlasmaTxtFiles(openFileDialog1.FileNames);
            }

        }

        /// <summary>
        /// Provides for concatenating .txt files
        /// </summary>
        /// <param name="fileNames"></param>
        public void OpenNuPlasmaTxtFiles(string[] fileNames)
        {
            // check for more than one file
            if (fileNames.Length > 1)
            {
                // multiselect new for Nov 2009
                DisplayStandByMessage(true);
                AbortCurrentDataFile();

                // lets get the data so we can see if they are consistent for sample, fraction, timestamp
                ArrayList dataFiles = new ArrayList();
                for (int i = 0; i < fileNames.Length; i++)
                {
                    MassSpecDataFile tempCurrentDataFile = new NuPlasma(new FileInfo(fileNames[i]));

                    string testValid = tempCurrentDataFile.TestFileValidity();
                    if (testValid.CompareTo("TRUE") == 0)
                    {
                        // returns null for empty data files
                        TripoliWorkProduct temp = tempCurrentDataFile.LoadRatios();
                        if (temp != null)
                        {
                            temp.SourceFileInfo = tempCurrentDataFile.DataFileInfo;
                            dataFiles.Add(temp);
                        }
                    }
                }

                // determine if there really is only one file, due to empty files
                if (!isInLiveWorkflow && (dataFiles.Count == 0))
                {
                    MessageBox.Show(
                         "The  .txt files you attempted to open do not appear to contain any valid NuPlasma raw data !",
                         "Tripoli Warning");
                }
                else if (dataFiles.Count == 1)
                {
                    OpenNuPlasmaTxtFile(fileNames[0]);
                }
                else
                {//***************************************************************
                    concatenateDataFiles(dataFiles);
                    TripoliRegistry.SetRecentNuPlasmaTxtFile(((TripoliWorkProduct)dataFiles[0]).SourceFileInfo.FullName);
                }//***************************************************************

            }
            else
            {
                OpenNuPlasmaTxtFile(fileNames[0]);
            }

        }

        public void OpenNuPlasmaTxtFile(string fileName)
        {
            DisplayStandByMessage(true);
            AbortCurrentDataFile();

            try
            {
                CurrentDataFile =
                    new NuPlasma(new FileInfo(fileName));
            }
            catch
            {
                MessageBox.Show("Failed to Open .txt File");
                AbortCurrentDataFile();
            }

            // refresh screen
            this.Refresh();

            if (CurrentDataFile != null)
            {
                string testValid = ((NuPlasma)CurrentDataFile).TestFileValidity();
                if (testValid.CompareTo("TRUE") == 0)
                {
                    saveTripoliWorkFile_menuItem.Enabled = false; //save
                    setSaveExportMenuItemsEnabled(true);
                    saveAsTripoliWorkFile_menuItem.Enabled = true; //saveas
                    menuWorkFileCloseFiles.Enabled = true; // close tripoli file
                    TripoliRegistry.SetRecentNuPlasmaTxtFile(fileName);
                    CurrentDataFile.DataFileInfo.Refresh();
                    TripoliRegistry.SetRecentLiveWorkflowFileAccessTime(DateTime.Now.ToUniversalTime().ToBinary());
                    ExtractNuPlasmaData();
                }
                else
                {
                    MessageBox.Show(
                        "The file that you are attempting to open "
                        + "\nis not a valid NuPlasma .txt file. ",
                        "Tripoli Warning");
                    AbortCurrentDataFile();
                }
            }
            DisplayStandByMessage(false);

        }

        private void ExtractNuPlasmaData()
        {
            DisplayStandByMessage(true);

            CurrentDataFileInfo = ((NuPlasma)CurrentDataFile).DataFileInfo;

            RawRatios = ((NuPlasma)CurrentDataFile).LoadRatios();

            if (!isInLiveWorkflow && (RawRatios == null))
            {
                //MessageBox.Show(
                //    "The NuPlasma .txt file you attempted to open does not appear to contain any raw data !",
                //    "Tripoli Warning");

                //ClearSelections();
                AbortCurrentDataFile();
            }
            else
            {
                RawRatios.SourceFileInfo = CurrentDataFileInfo;

                // default built-in BaPO2IC Oct 2009
                if (RawRatios.CurrentBariumPhosphateIC == null)
                {
                    TripoliBariumPhosphateIC = BariumPhosphateIC.EARTHTIME_BaPO2IC();
                }
                // default built-in TlIC Oct 2009
                if (RawRatios.CurrentThalliumIC == null)
                {
                    TripoliThalliumIC = ThalliumIC.EARTHTIME_TlIC();
                }

                DisplayCycleSelections();

                ButtonPanelRatios.Visible = true;
                SetStatusBar(CurrentDataFileInfo.FullName);
                SetTitleBar("NONE");
            }

            try
            {
                CurrentDataFile.close();
            }
            catch { }

            CurrentDataFile = null;
            DisplayStandByMessage(false);
        }

        #endregion NuPlasma

        private void menuDataFile_Click(object sender, EventArgs e)
        {

        }

        private void menuItem20_Click(object sender, EventArgs e)
        {

        }

        private void menuItemOpenIDTIMS_CSV_Template_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + @"\docs\ID_TIMS_DataImportTemplate.csv");
            }
            catch (System.UnauthorizedAccessException eFile)
            {
                MessageBox.Show(
                    "ID_TIMS_DataImportTemplate.csv not found ... please repair your installation.\n\n"
                    + eFile.Message,
                    "Tripoli Warning");
            }
        }


        // march 2010 ************************************************************************

        private void menuItemReadIDTIMS_ImportedCSV_Click(object sender, EventArgs e)
        {
            // read IDTIMS imported .csv file
            pnlAnnotate.Visible = false;
            openFileDialog1.Reset();
            openFileDialog1.Title = "Select ID-TIMS Imported Data .csv File";
            try
            {
                openFileDialog1.InitialDirectory = (new FileInfo(TripoliRegistry.GetRecentIDTIMS_ImportCSVFile())).DirectoryName;
            }
            catch { }
            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.AutoUpgradeEnabled = true;
            openFileDialog1.Multiselect = false;
            openFileDialog1.Filter = "csv files (*.csv)|*.csv";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // reset mainwindow info
                TripoliFileInfo = null;

                OpenIDTIMS_ImportedCSVFile(openFileDialog1.FileName);
            }

        }

        public void OpenIDTIMS_ImportedCSVFile(string fileName)
        {
            DisplayStandByMessage(true);
            AbortCurrentDataFile();

            try
            {
                CurrentDataFile =
                    new IDTIMS_ImportedCSVDataFile(new FileInfo(fileName));
            }
            catch
            {
                MessageBox.Show("Failed to Open .csv File");
                AbortCurrentDataFile();
            }

            // refresh screen
            this.Refresh();

            if (CurrentDataFile != null)
            {
                string testValid = ((IDTIMS_ImportedCSVDataFile)CurrentDataFile).TestFileValidity();
                if (testValid.CompareTo("TRUE") == 0)
                {
                    saveTripoliWorkFile_menuItem.Enabled = false; //save
                    setSaveExportMenuItemsEnabled(true);
                    saveAsTripoliWorkFile_menuItem.Enabled = true; //saveas
                    menuWorkFileCloseFiles.Enabled = true; // close tripoli file
                    TripoliRegistry.SetRecentIDTIMS_ImportCSVFile(fileName);
                    CurrentDataFile.DataFileInfo.Refresh();
                    TripoliRegistry.SetRecentLiveWorkflowFileAccessTime(DateTime.Now.ToUniversalTime().ToBinary());
                    ExtractIDTIMSImportedCSVData();

                    ((frmTripoliControlPanel)myTCP).Visible = false;

                }
                else
                {
                    MessageBox.Show(
                        "The file that you are attempting to open "
                        + "\nis not a valid ID_TIMS imported .csv file. ",
                        "Tripoli Warning");
                    AbortCurrentDataFile();
                }
            }
            DisplayStandByMessage(false);

        }
        private void ExtractIDTIMSImportedCSVData()
        {
            DisplayStandByMessage(true);

            CurrentDataFileInfo = ((IDTIMS_ImportedCSVDataFile)CurrentDataFile).DataFileInfo;

            RawRatios = ((IDTIMS_ImportedCSVDataFile)CurrentDataFile).LoadRatios();

            if (!isInLiveWorkflow && (RawRatios == null))
            {
                AbortCurrentDataFile();
            }
            else
            {
                RawRatios.SourceFileInfo = CurrentDataFileInfo;

                // default built-in BaPO2IC Oct 2009
                if (RawRatios.CurrentBariumPhosphateIC == null)
                {
                    TripoliBariumPhosphateIC = BariumPhosphateIC.EARTHTIME_BaPO2IC();
                }
                // default built-in TlIC Oct 2009
                if (RawRatios.CurrentThalliumIC == null)
                {
                    TripoliThalliumIC = ThalliumIC.EARTHTIME_TlIC();
                }

                RawRatios.PrepareCycleSelections2011();
                DisplayCycleSelections();

                ButtonPanelRatios.Visible = true;
                SetStatusBar(CurrentDataFileInfo.FullName);
                SetTitleBar("NONE");
            }

            try
            {
                CurrentDataFile.close();
            }
            catch { }

            CurrentDataFile = null;
            DisplayStandByMessage(false);
        }

        // mar 2010 start LA-ICP MS methods ===> refactoring **************************************************************
        private void menuItemReadElement2DataFolder_Click(object sender, EventArgs e)
        {
            pnlAnnotate.Visible = false;

            folderBrowserDialog1.Reset();
            folderBrowserDialog1.Description =
                "Select the folder containing the Element2 LA-ICP MS files";
            folderBrowserDialog1.ShowNewFolderButton = false;
            folderBrowserDialog1.SelectedPath = TripoliRegistry.GetRecentElement2DataFolder();
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                // reset mainwindow info
                TripoliFileInfo = null;

                OpenElement2DataFolder(folderBrowserDialog1.SelectedPath);
            }
        }

        public void OpenElement2DataFolder(string folderName)
        {
            DisplayStandByMessage(true);
            AbortCurrentDataFile();
            TripoliRegistry.SetRecentElement2DataFolder(folderName);
            this.Refresh();

            // discover whether there are any existing history files in the folder
            FileInfo[] tempFileInfo = (new DirectoryInfo(folderName)).GetFiles("*.txt");


            // for now, let's just open the first file
            try
            {
                CurrentDataFile =
                    new ThermoFinniganElement2DataFile(tempFileInfo[0]);
            }
            catch
            {
                MessageBox.Show("Failed to Open .txt File");
                AbortCurrentDataFile();
            }

            // refresh screen
            this.Refresh();

            if (CurrentDataFile != null)
            {
                string testValid = ((ThermoFinniganElement2DataFile)CurrentDataFile).TestFileValidity();
                if (testValid.CompareTo("TRUE") == 0)
                {
                    saveTripoliWorkFile_menuItem.Enabled = false; //save
                    setSaveExportMenuItemsEnabled(true);
                    saveAsTripoliWorkFile_menuItem.Enabled = true; //saveas
                    menuWorkFileCloseFiles.Enabled = true; // close tripoli file
                    CurrentDataFile.DataFileInfo.Refresh();
                    ExtractElement2DataFile();

                    ((frmTripoliControlPanel)myTCP).Visible = false;

                }
                else
                {
                    MessageBox.Show(
                        "The file that you are attempting to open "
                        + "\nis not a valid Element2 LA-ICP MS data .txt file. ",
                        "Tripoli Warning");
                    AbortCurrentDataFile();
                }
            }
            DisplayStandByMessage(false);

        }
        private void ExtractElement2DataFile()
        {
            DisplayStandByMessage(true);

            CurrentDataFileInfo = ((ThermoFinniganElement2DataFile)CurrentDataFile).DataFileInfo;

            RawRatios = ((ThermoFinniganElement2DataFile)CurrentDataFile).LoadRatios();

            if (!isInLiveWorkflow && (RawRatios == null))
            {
                AbortCurrentDataFile();
            }
            else
            {
                RawRatios.SourceFileInfo = CurrentDataFileInfo;

                DisplayCycleSelections();

                ButtonPanelRatios.Visible = true;
                SetStatusBar(CurrentDataFileInfo.FullName);
                SetTitleBar("NONE");
            }

            try
            {
                CurrentDataFile.close();
            }
            catch { }

            CurrentDataFile = null;
            DisplayStandByMessage(false);
        }


        // oct 2010 added functionality to re-correct already corrected tripolized files
        private void menuItemApplyTracerToReCorrectFractions_Click(object sender, EventArgs e)
        {
            pnlAnnotate.Visible = false;
            openFileDialog1.Reset();
            openFileDialog1.Title = "Select Tripolized file(s) for re-correction";
            try
            {
                openFileDialog1.InitialDirectory = (new FileInfo(TripoliRegistry.GetRecentTripoliWorkFile(1))).DirectoryName;
            }
            catch { }

            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.AutoUpgradeEnabled = true;
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "Tripoli Work Files '.trip' files (*.trip)|*.trip";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // reset mainwindow info
                TripoliFileInfo = null;

                ReCorrectTripoliWorkFiles(openFileDialog1.FileNames);
            }
        }

        public void ReCorrectTripoliWorkFiles(string[] fileNames)
        {
            DisplayStandByMessage(true);
            AbortCurrentDataFile();

            // write out a report file and then show it in notepad
            FileInfo clist = new FileInfo("LogOfReCorrectionByTracer.txt");
            StreamWriter s = new StreamWriter(clist.FullName);
            s.WriteLine("Active Tracer = " + TripoliTracer.getNameAndVersion());
            s.WriteLine();
            s.WriteLine("log of re-corrections follows:");
            s.WriteLine();

            for (int f = 0; f < fileNames.Length; f++)
            {
                FileInfo tripFileInfo = new FileInfo(fileNames[f]);
                try
                {
                    Stream stream = File.Open(tripFileInfo.FullName, FileMode.Open);
                    BinaryFormatter bformatter = new BinaryFormatter();

                    RawRatios = null;
                    RawRatios = (TripoliWorkProduct)bformatter.Deserialize(stream);

                    stream.Close();
                 
                    // this traps for accidentally opening tripoli history file
                    if (RawRatios.GetType().Name != "TripoliWorkProduct")
                    {
                        RawRatios = null;
                        throw (new Exception());
                    }
                    if (RawRatios.CurrentTracer != null)
                    {   // copied from open tripoli work file ... needs to be refactored into one method
                        Tracer currentTripoliTracer = RawRatios.CurrentTracer;

                        if (RawRatios.CurrentBariumPhosphateIC != null)
                            TripoliBariumPhosphateIC = RawRatios.CurrentBariumPhosphateIC;

                        if (RawRatios.CurrentThalliumIC != null)
                            TripoliThalliumIC = RawRatios.CurrentThalliumIC;

                        // backwards compatible
                        if (RawRatios.AmFractionationCorrected)
                        {
                            RawRatios.UseTracerCorrection = true;
                        }


                        // mar 2010 ... backwards compatibility
                        if (RawRatios.SampleName == null)
                            RawRatios.SampleName = "Unknown";

                        s.WriteLine("Trip file: " + tripFileInfo.Name + "  with old tracer " + currentTripoliTracer.getNameAndVersion()//
                             + "  with new tracer " + TripoliTracer.getNameAndVersion());


                        // June 2011 per request from Terry and Noah, preserve Tripolization ************************************************
                        // this code modified from LiveUpdate at line 5745
                        // save tripolized state as represented by sign of raw data points, in order to restore after new Tracer
                        // the technique is to make a copy of RawRatios, since removing fractionation correction will
                        // remove renamed and created ratios; these will be restored exactly when correction is applied to update

                        RawRatio[] saveRawRatios = new RawRatio[RawRatios.Count];
                        RawRatios.CopyTo(saveRawRatios);
                        //  deep copy of ratios
                        for (int j = 0; j < saveRawRatios.Length; j++)
                        {
                            saveRawRatios[j].SavedRatios = new double[saveRawRatios[j].Ratios.Length];
                            saveRawRatios[j].Ratios.CopyTo(saveRawRatios[j].SavedRatios, 0);
                            saveRawRatios[j]._CurrentOutliersSelected = ((RawRatio)RawRatios[j])._CurrentOutliersSelected;
                            saveRawRatios[j]._HandleDiscardsFlag = ((RawRatio)RawRatios[j])._HandleDiscardsFlag;
                            saveRawRatios[j].HandleOutliersFlag = ((RawRatio)RawRatios[j]).HandleOutliersFlag;

                        }

                        
                        // if rawratios is fractionation corrected, undo for all data
                        bool wasFractionationCorrected = RawRatios.AmFractionationCorrected;
                        if (RawRatios.AmFractionationCorrected)
                        {
                            RawRatios.RemoveFractionationCorrection();
                        }

                        // if rawratios is fractionation corrected, re-do for all data
                        if (wasFractionationCorrected)
                        {
                            RawRatios.PerformFractionationCorrection(TripoliTracer, TripoliBariumPhosphateIC, TripoliThalliumIC);
                        }

                        // now reapply signs to previously tripolized data ... -1 means de-selected
                        for (int i = 0; i < RawRatios.Count; i++)
                        {
                            RawRatio updatedRatio = (RawRatio)RawRatios[i];
                            RawRatio savedRatio = (RawRatio)saveRawRatios[i];
                            for (int j = 0; j < savedRatio.SavedRatios.Length; j++)
                            {
                                if (savedRatio.SavedRatios[j] < 0.0)
                                    updatedRatio.Ratios[j] = updatedRatio.Ratios[j] * -1.0;
                            }

                            updatedRatio._CurrentOutliersSelected = savedRatio._CurrentOutliersSelected;
                            updatedRatio._HandleDiscardsFlag = savedRatio._HandleDiscardsFlag;
                            updatedRatio.HandleOutliersFlag = savedRatio.HandleOutliersFlag;
                        }

                        // end of June 2011 update *******************************************************************************************

                        Serializer(tripFileInfo, (TripoliWorkProduct)RawRatios);
                    }
                    else
                    {
                        s.WriteLine("Trip file: " + tripFileInfo.Name + " was not changed.");
                    }
                }
                catch (Exception eee)
                {
                    Console.WriteLine(eee.Message);
                    MessageBox.Show(
                        "Failed to open Tripoli WorkFile !\n\n"
                        + eee.Message, //
                        "Tripoli Warning");
                }
            }

            // wrap up special report ********************************************
            s.Flush();
            s.Close();
            try
            {
                System.Diagnostics.Process.Start(clist.FullName);
            }
           catch { }

            DisplayStandByMessage(false);

 
        }

        private void menuControlPanel_Click(object sender, EventArgs e)
        {
            // aug 2011 refresh newest file name in menu below
            ((frmTripoliControlPanel)myTCP).locateCurrentLiveWorkflowDataFile();

        }

        private void version47ReleaseNotesMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(ConfigurationManager.AppSettings["ReleaseNotesOct2012"]);
            }
            catch (System.UnauthorizedAccessException eInternet)
            {
                MessageBox.Show(
                    "Release Notes not found ... are you connected to the Internet?\n\n"
                    + eInternet.Message,
                    "Tripoli Warning");
            }
        }

        private void CIRDLESonGitHubMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(ConfigurationManager.AppSettings["CIRDLESonGitHub"]);
            }
            catch (System.UnauthorizedAccessException eInternet)
            {
                MessageBox.Show(
                    "CIRDLES on GitHub.com website not found ... are you connected to the Internet?\n\n"
                    + eInternet.Message,
                    "Tripoli Warning");
            }
        }

  

    }



}

