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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

namespace Tripoli
{
    /// <summary>
    /// frmRatioGraphLAICP is based on a refactoring of frmRatioGraph. Jan. 2010.
    /// </summary>
    public class frmRatioGraphLAICP : System.Windows.Forms.Form, IComparable
    {
        // Possible Constants or display parameters
        protected float LEFT_MARGIN = 100F;
        protected float RIGHT_MARGIN;// = 125F;
        protected float TOP_MARGIN = 55F;
        protected float BOT_MARGIN;// = 95F;
        protected float Y_AXIS_DIVISIONS;// = 11F;
        protected float ORIGINAL_STATS_MARGIN; //= LEFT_MARGIN + 350F;
        protected double Y_AXIS_CUSHION = 0.025;
        protected float FULL_DETAIL_WIDTH = 500F;
        protected float FULL_DETAIL_HEIGHT = 250F;
        protected float LEFT_LOC_STATS;
        protected float ScaledMean;
        protected float ScaledMeanAdj;
        protected string ORIGINAL_DATA_LABEL;

        // fields
        /// <summary>
        /// 
        /// </summary>
        public Panel caller = null;
        /// <summary>
        /// 
        /// </summary>
        public int myGraphIndex = 0;
        /// <summary>
        /// 
        /// </summary>
        protected string _RatioName;
        private string _SampleName;

        public string SampleName
        {
            get { return _SampleName; }
            set { _SampleName = value; }
        }
        private RawRatio _ActiveRatios;
        private double _ChauvenetsThreshold;

        protected float GraphHeight;
        protected float GraphWidth;
        /// <summary>
        /// Used by paint and mouse locator
        /// </summary>
        protected float XtickGap;

        protected int TickValueStart;
        protected int YStart;
        protected double YRange;
        protected double Ymax;
        protected bool TickTest;
        protected bool doClipboard = false;

        protected double DataMax;
        protected double DataMin;
        protected float DataPenSize;// = 1.5F; 
        protected bool DrawMeansOnly = false;

        // added july 2005 to flag graph with no data
        protected bool _DisableGraph = false;


        // added Dec 2005 to give identity
        protected string GraphIdentity;

        // added March 2006 to fix toggle block functionality
        protected bool BlockSelected = false;

        // added Jan 2010 to play with Gehrels data
        private bool _AmBlockSorted = false;
        private bool _ShowBlockMeans = false;



        // Fields for graphical UI
        protected System.Windows.Forms.Panel ButtonPanel;
        protected System.Windows.Forms.Button btnRestore;
        protected System.Windows.Forms.Button btnUndo;
        protected System.Windows.Forms.Panel pnlSelectZone;
        protected System.Windows.Forms.Button btnUndoGroup;
        protected System.Windows.Forms.MainMenu mainMenu1;
        protected System.Windows.Forms.MenuItem menuItem1;
        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.Button btnSynchToMe;
        protected System.Windows.Forms.ToolTip toolTip1;
        protected System.Windows.Forms.Panel pnlInfoZone;
        protected System.Windows.Forms.TextBox txtGainsFile;
        protected System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.MenuItem mnuPrintMain;
        protected System.Windows.Forms.MenuItem mnuPrintPortrait;
        protected System.Windows.Forms.MenuItem mnuPrintLandscape;
        protected System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem mnuGraphPointSize;
        private System.Windows.Forms.MenuItem mnuGraphDataDetail;
        private System.Windows.Forms.MenuItem mnuGraphDataDetailPM;
        private System.Windows.Forms.MenuItem mnuGraphDataDetailM;
        private System.ComponentModel.IContainer components;

        // initialize pens, brushes, and fonts
        protected Pen BlackPen;
        protected Pen RedPen;
        protected Pen BigRedPen;
        protected Pen RedThickPen;
        protected Pen BlackDataPen;
        protected Pen BlueDataPen;
        protected Pen GreyThinLinePen;

        protected Pen RedDataPen;
        protected Pen BluePen;

        protected Brush blackBrush;
        protected Brush redBrush;
        protected Brush blueBrush;

        protected SolidBrush FaintYellowBrush;
        protected SolidBrush FaintBlueBrush;
        protected SolidBrush FaintGreenBrush;
        protected SolidBrush FaintRedBrush;

        protected HatchBrush HatchBlueBrush;
        protected HatchBrush redHatchBrush;
        protected HatchBrush blueHatchBrushDiagUp;
        protected HatchBrush blueHatchBrushDiagDown;


        protected Font boldTimesFont;
        protected Font CourierFont10;
        protected Font CourierFont9;
        protected Font CourierFont8;
        protected Font ArialFont8;
        private System.Windows.Forms.ToolTip toolTip2;
        protected System.Windows.Forms.CheckBox chkboxAutoCalc;
        protected System.Windows.Forms.NumericUpDown numberBinsUpDown;
        protected System.Windows.Forms.PictureBox pictureBoxHistogram;
        private System.Windows.Forms.MenuItem mnuGraphStatsDetail;
        private System.Windows.Forms.MenuItem mnuGraphStatsDetail2SSSe;
        private System.Windows.Forms.MenuItem mnuGraphStatsDetailSSe;
        private System.Windows.Forms.MenuItem mnuGraphStatsDetailSe;
        private System.Windows.Forms.RadioButton rbChauvenet;
        private System.Windows.Forms.RadioButton rb2SigmaOutliers;
        private System.Windows.Forms.RadioButton rbManualOutliers;
        private System.Windows.Forms.Label lblOutliers;
        private System.Windows.Forms.RadioButton rbHideDiscards;
        private System.Windows.Forms.RadioButton rbIncludeDiscards;
        private System.Windows.Forms.Label lblDiscards;
        private System.Windows.Forms.RadioButton rbIgnoreDiscards;
        protected System.Windows.Forms.Button btnDiscardOutliers;
        protected System.Windows.Forms.Panel pnlChooseOutliers;
        private System.Windows.Forms.Label lblCAUTION;
        protected System.Windows.Forms.Panel pnlDiscards;
        private MenuItem menuItemToggleBlockSort;
        private MenuItem menuItemToggleBlockMeans;
        protected Font SymbolFont;

        public frmRatioGraphLAICP() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ActiveRatios"></param>
        public frmRatioGraphLAICP(RawRatio ActiveRatios, string SampleName, double ChauvenetsThreshold)
        {
            GraphIdentity = "RATIO";

            _ActiveRatios = ActiveRatios;
            _SampleName = SampleName;
            _ActiveRatios.CalcStats();
            _ChauvenetsThreshold = ChauvenetsThreshold;

            _RatioName = ActiveRatios.Name;

            InitializeComponent();

            //this.Text = _RatioName + " <" + SampleName + ">";
            //// April 2007 - strip out newline in graph window title bar
            //int index = this.Text.IndexOf(Environment.NewLine);
            //if (index > -1)
            //    this.Text = this.Text.Remove(index, 2);
            RefreshTitleText();


            this.SetStyle(ControlStyles.StandardDoubleClick, true);
            this.UpdateStyles();

            toolTip1.SetToolTip(this, "LEFT Click to copy Ratio mean and %Std Error to clipboard");
            toolTip1.Active = false;

            toolTip2.SetToolTip(this, "RIGHT Click to toggle discards within block (minority wins)");
            toolTip2.Active = false;

            // setup pensize menu
            if (Math.Abs(ActiveRatios.DataPenSize) == 1.0F)
                SetPenSizeMenus(true, false, false);
            if (Math.Abs(ActiveRatios.DataPenSize) == 1.5F)
                SetPenSizeMenus(false, true, false);
            if (Math.Abs(ActiveRatios.DataPenSize) == 2.0F)
                SetPenSizeMenus(false, false, true);

            mnuGraphDataDetailPM.Checked = (ActiveRatios.DataPenSize > 0);
            mnuGraphDataDetailM.Checked = !mnuGraphDataDetailPM.Checked;



        }


        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public RawRatio ActiveRatios
        {
            get { return _ActiveRatios; }
            set { _ActiveRatios = value; }
        }
        public double ChauvenetsThreshold
        {
            get { return _ChauvenetsThreshold; }
            set { _ChauvenetsThreshold = value; }
        }
        #endregion Properties

        #region Methods

        public void RefreshTitleText()
        {
            this.Text = _RatioName + " <" + SampleName + ">";
            // April 2007 - strip out newline in graph window title bar
            int index = this.Text.IndexOf(Environment.NewLine);
            if (index > -1)
                this.Text = this.Text.Remove(index, 2);
        }

        public override void Refresh()
        {           
            base.Refresh();
            resetControlsOnForm();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics dc = e.Graphics;

            OnPaintDirective(dc, this.Width, 0);

            /**			chkboxAutoCalc.Checked = ActiveRatios.AutoCalculate;
                        numberBinsUpDown.Value = ActiveRatios.HistogramBinCount;

                        // set radio buttons for discards
                        if (ActiveRatios.HandleDiscardsFlag == (int)RawRatio.HandleDiscards.Include)
                            rbIncludeDiscards.Checked = true;
                        if (ActiveRatios.HandleDiscardsFlag == (int)RawRatio.HandleDiscards.Ignore)
                            rbIgnoreDiscards.Checked = true;
                        if (ActiveRatios.HandleDiscardsFlag == (int)RawRatio.HandleDiscards.Hide)
                            rbHideDiscards.Checked = true;

                    // set radio buttons for outliers - click to get enabled/not action
                        if (ActiveRatios.HandleOutliersFlag == (int)RawRatio.HandleOutliers.Manual)
                        {
                            rbManualOutliers.Checked = true;
                            rb2SigmaOutliers.Enabled = true;
                            rbChauvenet.Enabled = true;
                            btnUndo.Enabled = true;
                            btnUndoGroup.Enabled = true;
                        }
                        if (ActiveRatios.HandleOutliersFlag == (int)RawRatio.HandleOutliers.TwoSigma)
                        {
                            rb2SigmaOutliers.Checked = true;
                            rb2SigmaOutliers.Enabled = false;
                            rbChauvenet.Enabled = false;
                            btnUndo.Enabled = false;
                            btnUndoGroup.Enabled = false;
                        }
                        if (ActiveRatios.HandleOutliersFlag == (int)RawRatio.HandleOutliers.Chauvenet)
                        {
                            rbChauvenet.Checked = true;
                            rb2SigmaOutliers.Enabled = false;
                            rbChauvenet.Enabled = false;
                            btnUndo.Enabled = false;
                            btnUndoGroup.Enabled = false;
                        }
 **/           
            // express caution for synchronized outliers
            if (ActiveRatios.AmSynchronized
                &&
                ActiveRatios.HandleOutliersFlag != (int)RawRatio.HandleOutliers.Manual)
            {
                lblCAUTION.Visible = true;
            }
            else
            {
                lblCAUTION.Visible = false;
            }
            base.OnPaint(e);
        }

        private void OnPaintDirective(Graphics dc, int Width, int Height)
        {
            if (Width < FULL_DETAIL_WIDTH)
            {
                RIGHT_MARGIN = 20;
            }
            else
            {
                RIGHT_MARGIN = 125;
            }

            ORIGINAL_DATA_LABEL = "Original";

            DisplayGraph(dc, Width, Height);
            DisplayMean(dc);
            DisplayMissingCount(dc);

            if (ActiveRatios.AutoCalculate
                &&
                (ActiveRatios.HistogramBinCount > 0))
            {
                DisplayHistogram(dc);
            }

            DisplayActiveDataHandleDiscardsFlag(dc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="Gwidth"></param>
        /// <param name="Gheight"></param>
        public void DisplayGraph(Graphics dc, int Gwidth, int Gheight)
        {
            // modification to allow printing to specific sizes AUG 2005
            int myWidth = this.Width;
            int myHeight = this.Height;
            bool UsePrinterShading = false;

            if ((Gwidth * Gheight) > 0)
            {
                // reset width for printing
                myWidth = Gwidth;
                myHeight = Gheight;
                UsePrinterShading = true;
            }
            else
            {
                // keep screen width for repaint
                // do nothing
            }

            // negative datapensize means draw means only
            DataPenSize = Math.Abs(ActiveRatios.DataPenSize);
            DrawMeansOnly = (ActiveRatios.DataPenSize < 0);

            if (this.MdiParent != null)
            {
                BOT_MARGIN = 90;//75;
            }
            else
            {
                BOT_MARGIN = 105;//95;
            }

            if (myHeight > FULL_DETAIL_HEIGHT)
                Y_AXIS_DIVISIONS = 11F;
            else
                Y_AXIS_DIVISIONS = 5F;

            // color synch button
            if (ActiveRatios.AmSynchronizer)
                btnSynchToMe.BackColor = Color.Red;
            else
                btnSynchToMe.BackColor = Color.Bisque;

            // enable synch button
            if (ActiveRatios.IsActive)
                btnSynchToMe.Enabled = true;
            else
                btnSynchToMe.Enabled = false;

            // label show hide button
            //	if (ActiveRatios.DiscardOutliers)
            //	{
            //		btnDiscardOutliers.Text = "Show Discards";
            //				toolTip2.Active = false;
            //	}
            //	else
            //	{
            //		btnDiscardOutliers.Text = (string)btnDiscardOutliers.Tag; //"Hide Outliers";
            //				toolTip2.Active = true;
            //	}
            // initialize pens, brushes, and fonts
            BlackPen = new Pen(Color.Black, 1);
            RedPen = new Pen(Color.Red, 1);
            BigRedPen = new Pen(Color.Red, 4);
            RedThickPen = new Pen(Color.Red, 2);
            BlackDataPen = new Pen(Color.Black, DataPenSize);
            BlueDataPen = new Pen(Color.Blue, DataPenSize);
            GreyThinLinePen = new Pen(Color.Gray, 1F);
            GreyThinLinePen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;

            RedDataPen = new Pen(Color.Red, DataPenSize);
            BluePen = new Pen(Color.Blue, 1);

            blackBrush = Brushes.Black;
            redBrush = Brushes.Red;
            blueBrush = Brushes.Blue;

            HatchBlueBrush = new HatchBrush(HatchStyle.LightUpwardDiagonal, Color.Blue, Color.Transparent);
            redHatchBrush = new HatchBrush(HatchStyle.LightVertical, Color.Red, Color.Transparent);
            blueHatchBrushDiagUp = new HatchBrush(HatchStyle.LightUpwardDiagonal, Color.Blue, Color.Transparent);
            blueHatchBrushDiagDown = new HatchBrush(HatchStyle.LightDownwardDiagonal, Color.Blue, Color.Transparent);

            boldTimesFont = new Font("Times New Roman", 12, FontStyle.Bold);
            CourierFont10 = new Font("Courier New", 10, FontStyle.Bold);
            // changed Jan 2010 to improve look and feel as a test 
            // TODO: refactor name
            CourierFont9 = new Font("Courier New", 8, FontStyle.Bold);
            CourierFont8 = new Font("Courier New", 8, FontStyle.Bold);
            ArialFont8 = new Font("Arial", 8, FontStyle.Bold);
            SymbolFont = new Font("Symbol", 12, FontStyle.Bold);

            if (UsePrinterShading)
            {
                FaintYellowBrush = new SolidBrush(Color.Yellow);//.LemonChiffon);
                FaintBlueBrush = new SolidBrush(Color.Cyan);
                FaintGreenBrush = new SolidBrush(Color.LightGreen);
                FaintRedBrush = new SolidBrush(Color.Pink);
            }
            else
            {
                FaintYellowBrush = new SolidBrush(Color.LemonChiffon);
                FaintBlueBrush = new SolidBrush(Color.LightCyan);
                FaintGreenBrush = new SolidBrush(Color.LightGreen);
                FaintRedBrush = new SolidBrush(Color.Pink);
            }


            // establish y axis labels from min, max
            if ((ActiveRatios.HandleDiscardsFlag != (int)RawRatio.HandleDiscards.Include))
            {
                // also check to keep mean +/- 1 sigma in view
                DataMax = Math.Max(ActiveRatios.Max, ActiveRatios.StdDev + ActiveRatios.Mean);
                DataMin = Math.Min(ActiveRatios.Min, ActiveRatios.Mean - ActiveRatios.StdDev);
            }
            else
            {	// show outliers
                // also check to keep mean +/- 1 sigma in view
                DataMax = Math.Max(ActiveRatios.AllMax, ActiveRatios.StdDev + ActiveRatios.Mean);
                DataMin = Math.Min(ActiveRatios.AllMin, ActiveRatios.Mean - ActiveRatios.StdDev);
            }

            double DataRange = (DataMax - DataMin);

            // march 2005 - trap for DataRange == 0
            if (DataRange == 0)
                DataRange = DataMin;

            // give a little top and bottom buffer
            double Ymin = DataMin - DataRange * Y_AXIS_CUSHION;
            // stop at zero
            if (Ymin < 0) Ymin = 0;
            Ymax = DataMax + DataRange * Y_AXIS_CUSHION;
            YRange = Ymax - Ymin;

            // establish bounding box for graph
            GraphHeight = (float)myHeight - TOP_MARGIN - BOT_MARGIN;
            GraphWidth = (float)myWidth - LEFT_MARGIN - RIGHT_MARGIN;
            if (myWidth > FULL_DETAIL_WIDTH)
                ORIGINAL_STATS_MARGIN = GraphWidth - 25;
            else
                ORIGINAL_STATS_MARGIN = 25;//LEFT_MARGIN;

            // June 2005 test for ActiveRatios.AllCount == 0 : all values < 0,etc.
            if ((ActiveRatios.AllCount == 0)
                ||
                (YRange == 0))
            {
                // display some message about no data
                dc.DrawString(
                    "***** N O     G O O D    D A T A *****",
                    CourierFont9, blackBrush,
                    new PointF(5F, TOP_MARGIN - 8F));
                _DisableGraph = true;
                return;
            }
            else
            {
                _DisableGraph = false; // reset
            }


            // display order is critical for correct appearance ***********************************

            if (ActiveRatios.StatsDetailDisplay == (int)RawRatio.StatsDisplayed.TwoSigmaSigmaStdErr)
            {
                // display shaded 2 std dev each side of mean
                float ScaledTop2Std =
                    (float)((Ymax - (ActiveRatios.Mean + (2F * ActiveRatios.StdDev))) / YRange) * GraphHeight;
                float Scaled2StdWidth =
                    (float)(((4F * ActiveRatios.StdDev)) / YRange) * GraphHeight;
                dc.FillRectangle(FaintRedBrush,
                    LEFT_MARGIN, TOP_MARGIN + ScaledTop2Std,
                    GraphWidth, Scaled2StdWidth);
            }

            if (ActiveRatios.StatsDetailDisplay >= (int)RawRatio.StatsDisplayed.SigmaStdErr)
            {
                // display shaded 1 std dev each side of mean
                float ScaledTopStd =
                    (float)((Ymax - (ActiveRatios.Mean + ActiveRatios.StdDev)) / YRange) * GraphHeight;
                float ScaledStdWidth =
                    (float)(((2F * ActiveRatios.StdDev)) / YRange) * GraphHeight;
                dc.FillRectangle(FaintYellowBrush,
                    LEFT_MARGIN, TOP_MARGIN + ScaledTopStd,
                    GraphWidth, ScaledStdWidth);
            }

            if (ActiveRatios.StatsDetailDisplay >= (int)RawRatio.StatsDisplayed.StdErr)
            {
                // display shaded standard error of mean on both sides
                float ScaledTopError =
                    (float)((Ymax - (ActiveRatios.Mean + ActiveRatios.StdErr)) / YRange) * GraphHeight;
                float ScaledErrorWidth =
                    (float)(((2F * ActiveRatios.StdErr)) / YRange) * GraphHeight;
                dc.FillRectangle(FaintGreenBrush, //   FaintBlueBrush, 
                    LEFT_MARGIN, TOP_MARGIN + ScaledTopError,
                    GraphWidth, ScaledErrorWidth);
            }

            // calculate mean location
            ScaledMean =
                (float)((Ymax - ActiveRatios.Mean) / YRange) * GraphHeight;

            // draw bounding box for graph
            dc.DrawRectangle(BlackPen, LEFT_MARGIN, TOP_MARGIN, GraphWidth, GraphHeight);

            // display original stats  ************************************************************************

            if (myWidth > FULL_DETAIL_WIDTH)
            {
                // Jan 2010 changed font from COurierFont10
                dc.DrawString("  " + _RatioName, ArialFont8, blueBrush,
                    new PointF(0F, 1.5F * 8F));
                dc.DrawString(ORIGINAL_DATA_LABEL, CourierFont9, blueBrush,
                    new PointF(ORIGINAL_STATS_MARGIN - 75F, 1.5F * 8F));
                dc.DrawString("    Data", CourierFont9, blueBrush,
                    new PointF(ORIGINAL_STATS_MARGIN - 75F, 3.0F * 8F));
            }

            // x bar
            dc.DrawString("x", boldTimesFont, blueBrush,
                new PointF(ORIGINAL_STATS_MARGIN + 6F, 1.0F * 8F - 11F));
            dc.DrawString("\u0060", SymbolFont, blueBrush,
                new PointF(ORIGINAL_STATS_MARGIN - 2F, 1.0F * 8F - 6F));
            // x bar value = mean
            dc.DrawString(
                ActiveRatios.AllMean.ToString("E5"),
                CourierFont9, blueBrush,
                new PointF(ORIGINAL_STATS_MARGIN + 24F, 1.0F * 8F - 8F));

            // standard deviation sigma
            dc.DrawString(
                "s",
                SymbolFont, blueBrush,
                new PointF(ORIGINAL_STATS_MARGIN + 3F, (1.5F * 8F) - 4F));
            dc.DrawString(
                ActiveRatios.AllStdDev.ToString("E5"),
                CourierFont9, blueBrush,
                new PointF(ORIGINAL_STATS_MARGIN + 24F, (1.5F * 8F)));

            // percent standard error of the mean sigma sub x bar
            dc.DrawString(
                "%",
                CourierFont9, blueBrush,
                new PointF(ORIGINAL_STATS_MARGIN + 0F, (3.0F * 8F)));
            dc.DrawString(
                "s",
                SymbolFont, blueBrush,
                new PointF(ORIGINAL_STATS_MARGIN + 4F, (3.0F * 8F) - 4F));
            dc.DrawString("x", CourierFont8, blueBrush,
                new PointF(ORIGINAL_STATS_MARGIN + 14F, (3.0F * 8F) + 6F));
            dc.DrawString("\u0060", SymbolFont, blueBrush,
                new PointF(ORIGINAL_STATS_MARGIN + 3F, (3.0F * 8F) + 9F));
            dc.DrawString(
                ActiveRatios.AllPctStdErr.ToString("E5"),
                CourierFont9, blueBrush,
                new PointF(ORIGINAL_STATS_MARGIN + 24F, (3.0F * 8F)));

            // sample population n
            dc.DrawString(
                " n " + ActiveRatios.AllCount.ToString(),
                CourierFont9, blueBrush,
                new PointF(ORIGINAL_STATS_MARGIN + 3F, (4.8F * 8F)));

            // check if mean is too close to bottom and set ScaledMeanAdj to
            // place stats above mean
            ScaledMeanAdj = ScaledMean;
            if ((TOP_MARGIN + ScaledMean + 40) > GraphHeight)
                ScaledMeanAdj -= 60;

            if (myWidth < FULL_DETAIL_WIDTH)
                ScaledMeanAdj = -1 * TOP_MARGIN;// this moves all to top so graph can be wider

            // display stats legend below mean
            if (myWidth < FULL_DETAIL_WIDTH)
                LEFT_LOC_STATS = LEFT_MARGIN + GraphWidth - 125;
            else
                LEFT_LOC_STATS = LEFT_MARGIN + GraphWidth;


            // label Y axis and draw lines across
            float YtickGap = GraphHeight / Y_AXIS_DIVISIONS;
            float YDataGap = (float)YRange / Y_AXIS_DIVISIONS;
            for (int tick = 0; tick < (Y_AXIS_DIVISIONS - 1); tick++)
            {
                dc.DrawLine(BlackPen,
                    LEFT_MARGIN, TOP_MARGIN + (float)(tick + 1) * YtickGap,
                    LEFT_MARGIN + GraphWidth, TOP_MARGIN + (float)(tick + 1) * YtickGap);

                dc.DrawString(
                    ((double)((double)Ymax - (double)(tick + 1) * (double)YDataGap)).ToString("E5"),
                    CourierFont9, blackBrush,
                    new PointF(5F, TOP_MARGIN + (float)(tick + 1) * YtickGap - 8F));
            }

            // label top and bottom of graph y axis
            dc.DrawString(
                Ymax.ToString("E5"),
                CourierFont9, blackBrush,
                new PointF(5F, TOP_MARGIN - 8F));
            dc.DrawString(
                Ymin.ToString("E5"),
                CourierFont9, blackBrush,
                new PointF(5, TOP_MARGIN + GraphHeight - 8F));

            // draw key legend ****************************************************
            if ((myWidth > FULL_DETAIL_WIDTH) && (myHeight > FULL_DETAIL_HEIGHT))
            //if (this.MdiParent == null)
            {
                dc.FillRectangle(FaintRedBrush,
                    LEFT_MARGIN + GraphWidth + 25, 0, 20, 50);
                dc.FillRectangle(FaintYellowBrush,
                    LEFT_MARGIN + GraphWidth + 45, 25, 20, 25);
                dc.FillRectangle(FaintGreenBrush,
                    LEFT_MARGIN + GraphWidth + 65, 35, 20, 15);
                // mean
                dc.DrawLine(BluePen,
                    LEFT_MARGIN + GraphWidth + 22, 50,
                    LEFT_MARGIN + GraphWidth + 27 + 60, 50);

                // 2sigma
                dc.DrawString("2s", SymbolFont, blackBrush,
                    new PointF(LEFT_MARGIN + GraphWidth + 23F, 8F));

                // sigma
                dc.DrawString("s", SymbolFont, blackBrush,
                    new PointF(LEFT_MARGIN + GraphWidth + 46F, 22F));

                // x bar
                dc.DrawString("x", boldTimesFont, blackBrush,
                    new PointF(LEFT_MARGIN + GraphWidth + 90F, 39F));
                dc.DrawString("\u0060", SymbolFont, blackBrush,
                    new PointF(LEFT_MARGIN + GraphWidth + 82F, 44F));

                // sigma sub x bar
                dc.DrawString("s", SymbolFont, blackBrush,
                    new PointF(LEFT_MARGIN + GraphWidth + 63F, 28F));
                dc.DrawString("x", CourierFont8, blackBrush,
                    new PointF(LEFT_MARGIN + GraphWidth + 73F, 38F));
                dc.DrawString("\u0060", SymbolFont, blackBrush,
                    new PointF(LEFT_MARGIN + GraphWidth + 62F, 41F));

                // label legend
                dc.DrawString("legend", CourierFont8, blackBrush,
                    new PointF(LEFT_MARGIN + GraphWidth + 60F, 5F));
            }

            // draw mean line and value *****************************************************
            // dec 2006 show frozen mean in red
            if (_ActiveRatios.AutoCalculate)
            {
                dc.DrawLine(BluePen,
                    LEFT_MARGIN - 5, TOP_MARGIN + ScaledMean,
                    LEFT_MARGIN + GraphWidth + 5, TOP_MARGIN + ScaledMean);
            }
            else
            {
                dc.DrawLine(BigRedPen,
                    LEFT_MARGIN - 5, TOP_MARGIN + ScaledMean,
                    LEFT_MARGIN + GraphWidth + 5, TOP_MARGIN + ScaledMean);
            }

            if (myWidth < FULL_DETAIL_WIDTH)
                ScaledMean = -1 * TOP_MARGIN + 1F * 8F;

        }

        // Dec 2006 made DisplayActiveDataHandleDiscardsFlag where
        // modes include, ignore, and hide could be supported
        // NOTE: as of Version 4.3.2.2, the discardOutliers variable
        // is abandoned in favor of the more meaningful
        // HandleDiscards enumeration [Include, Ignore, Hide]
        // saved as the HandleDiscardsFlag

        private void DisplayActiveDataHandleDiscardsFlag(Graphics dc)
        {
            //******************* do this last ************************

            // jan 2008 perform safety check on cyclesperblock to avoid div by 0
            if (ActiveRatios.CyclesPerBlock == 0)
            {
                // set it arbitrarily at end of active data
                ActiveRatios.CyclesPerBlock = (int)ActiveRatios.AllCount;
            }

            XtickGap = GraphWidth / ((float)ActiveRatios.ActiveRatios.Count + 1F);
            for (int RealTick = 0; RealTick < ActiveRatios.ActiveRatios.Count; RealTick++)
            {
                int tick = RealTick;

                dc.DrawLine(BlackPen,
                    LEFT_MARGIN + (float)(tick + 1) * XtickGap, TOP_MARGIN + GraphHeight,
                    LEFT_MARGIN + (float)(tick + 1) * XtickGap, TOP_MARGIN + GraphHeight + 5);

                // calculate data point
                float ScaledData =
                    (float)((Ymax - Math.Abs((double)ActiveRatios.ActiveRatios[RealTick])) / YRange) * GraphHeight;

                // decide if discarded based on sign - 
                // these will be here if HandleDiscards is Include
                if ((double)ActiveRatios.ActiveRatios[RealTick] > 0)//ignore 0s here
                {
                    if (!DrawMeansOnly)
                    {
                        dc.FillRectangle(blackBrush,
                            LEFT_MARGIN + (float)(tick + 1) * XtickGap - 0.5F * DataPenSize,
                            TOP_MARGIN + ScaledData - 1.0F * DataPenSize, // shift up a little
                            2F * DataPenSize, 2F * DataPenSize);
                    }
                }
                else if ((double)ActiveRatios.ActiveRatios[RealTick] < 0)//ignore 0s here
                {
                    // dec 2006 also add check for ignore flag so as to not render
                    // points outside the actual graph
                    if ((!DrawMeansOnly)
                        &&
                        (ScaledData >= 0)
                        &&
                        (ScaledData <= GraphHeight)
                        )
                    {
                        dc.FillRectangle(redBrush,
                            LEFT_MARGIN + (float)(tick + 1) * XtickGap - 0.5F * DataPenSize,
                            TOP_MARGIN + ScaledData - 1.0F * DataPenSize, // shift up a little
                            2F * DataPenSize, 2F * DataPenSize);

                        // dec 2006 check for CurrentOutliers and draw circle around them
                        if (ActiveRatios.CurrentOutliersSelected.Contains(RealTick)
                            &&
                            (RealTick
                            != (int)ActiveRatios.CurrentOutliersSelected[ActiveRatios.CurrentOutliersSelected.Count - 1]))
                        {
                            dc.DrawRectangle(RedPen,
                                LEFT_MARGIN + (float)(tick + 1) * XtickGap - 1.5F * DataPenSize,
                                TOP_MARGIN + ScaledData - 2.0F * DataPenSize, // shift up a little
                                4F * DataPenSize, 4F * DataPenSize);
                        }
                    }
                }
                else //mark the axis since missing value
                    dc.DrawLine(RedThickPen,
                        LEFT_MARGIN + (float)(tick + 1) * XtickGap, TOP_MARGIN + GraphHeight - 5,
                        LEFT_MARGIN + (float)(tick + 1) * XtickGap, TOP_MARGIN + GraphHeight + 5);

                // decide which cycleblock - ie whether to draw vertical dashed line
                if ((ActiveRatios.HandleDiscardsFlag != (int)RawRatio.HandleDiscards.Hide)
                    &&
                    ((RealTick % ActiveRatios.CyclesPerBlock) == 0))
                {
                    // draw vertical line
                    if (tick > 0)
                    {
                        dc.DrawLine(GreyThinLinePen,
                            LEFT_MARGIN + (float)(tick + 0.6) * XtickGap, TOP_MARGIN,
                            LEFT_MARGIN + (float)(tick + 0.6) * XtickGap, TOP_MARGIN + GraphHeight);
                    }

                    // jan 2010 draw block mean if exists (for Gehrels)
                    if (_ShowBlockMeans)
                    {
                        int myBlockNo = (int)Math.Truncate((double)(RealTick / ActiveRatios.CyclesPerBlock));

                        float ScaledBlockMean =
                             (float)((Ymax - ActiveRatios.GetBlockMeans()[myBlockNo]) / YRange) * GraphHeight;

                        dc.DrawLine(BluePen,
                            LEFT_MARGIN + (float)(tick + 0.6) * XtickGap, TOP_MARGIN + ScaledBlockMean,
                            LEFT_MARGIN + (float)((tick + ActiveRatios.CyclesPerBlock) + 0.6) * XtickGap, TOP_MARGIN + ScaledBlockMean);
                    }
                }
                
            }
        }

        /*		private void DisplayActiveDataIgnoreDiscardsOnFlag(Graphics dc)
                {
                    //******************* do this last ************************

                    XtickGap = GraphWidth / ((float)ActiveRatios.AllCount + 1F);
                    for (int RealTick = 0; RealTick < ActiveRatios.AllCount; RealTick ++)
                    {
                        int tick = RealTick;

                        dc.DrawLine(BlackPen, 
                            LEFT_MARGIN + (float)(tick + 1) * XtickGap, TOP_MARGIN + GraphHeight,
                            LEFT_MARGIN + (float)(tick + 1) * XtickGap, TOP_MARGIN + GraphHeight + 5);

                        // calculate data point
                        float ScaledData = 
                            (float)((Ymax - Math.Abs((double)ActiveRatios.Ratios[RealTick])) / YRange) * GraphHeight;
				
                        // decide if outlier based on sign - these will be here if DiscardOutliers is false
                        if ((double)ActiveRatios.Ratios[RealTick] > 0)//ignore 0s here
                        {
                            if (! DrawMeansOnly)
                            {
                                dc.FillRectangle(blackBrush, 
                                    LEFT_MARGIN + (float)(tick + 1) * XtickGap - 0.5F * DataPenSize,
                                    TOP_MARGIN + ScaledData - 1.0F * DataPenSize, // shift up a little
                                    2F * DataPenSize, 2F * DataPenSize);
                            }
                        }
                        else if ((double)ActiveRatios.Ratios[RealTick] < 0)//ignore 0s here
                        {
                            if (! DrawMeansOnly)
                            {
                                dc.FillRectangle(redBrush, 
                                    LEFT_MARGIN + (float)(tick + 1) * XtickGap - 0.5F * DataPenSize,
                                    TOP_MARGIN + ScaledData - 1.0F * DataPenSize, // shift up a little
                                    2F * DataPenSize, 2F * DataPenSize);
                            }
                        }
                        else //mark the axis since missing value
                            dc.DrawLine(RedThickPen, 
                                LEFT_MARGIN + (float)(tick + 1) * XtickGap, TOP_MARGIN + GraphHeight - 5,
                                LEFT_MARGIN + (float)(tick + 1) * XtickGap, TOP_MARGIN + GraphHeight + 5);

                        // decide which cycleblock - ie whether to draw vertical dashed line
                        if (((RealTick % ActiveRatios.CyclesPerBlock) == 0))
                        {
                            // draw vertical line
                            if (tick > 0)
                            {
                                dc.DrawLine(GreyThinLinePen, 
                                    LEFT_MARGIN + (float)(tick + 0.6) * XtickGap, TOP_MARGIN,
                                    LEFT_MARGIN + (float)(tick + 0.6) * XtickGap, TOP_MARGIN + GraphHeight);
                            }

                        }


                    }
                }
        */
        // dec 2006
        private void DisplayHistogram(Graphics dc)
        {
            int binCount = ActiveRatios.HistogramBinCount;

            double DataMax = Math.Max(ActiveRatios.Max, ActiveRatios.StdDev + ActiveRatios.Mean);
            double DataMin = Math.Min(ActiveRatios.Min, ActiveRatios.Mean - ActiveRatios.StdDev);

            int[] histogram = new int[binCount];
            double[] histogramNorm = new double[binCount];

            double binWidth = (DataMax - DataMin) / (double)binCount;

            // generate histogram
            int maxBinCount = 0;
            for (int RealTick = 0; RealTick < ActiveRatios.ActiveRatios.Count; RealTick++)
            {
                double datum = (double)ActiveRatios.ActiveRatios[RealTick];

                if (datum > 0)//ignore 0s here
                {
                    int binNum = (int)Math.Floor(Math.Abs((datum - DataMin * 1.000000001) / binWidth));
                    try
                    {
                        histogram[binNum]++;
                        if (histogram[binNum] > maxBinCount)
                            maxBinCount++;
                    }
                    catch (Exception eHist)
                    {
                        Console.WriteLine(eHist.Message);
                    }
                }
            }

            // Normalize histogram and draw it bottom to top
            for (int h = 0; h < binCount; h++)
            {
                histogramNorm[h] = histogram[h] / (double)maxBinCount;
                // calculate sides of bin
                float ScaledBinRight =
                    (float)((Ymax - (DataMin + (h * binWidth))) / YRange) * GraphHeight;

                float scaledBinWidth =
                    (float)(binWidth / YRange) * GraphHeight;

                if (h % 2 == 0)
                {
                    dc.FillRectangle(blueHatchBrushDiagUp,
                        LEFT_MARGIN, TOP_MARGIN + ScaledBinRight - scaledBinWidth,
                        GraphWidth * (float)histogramNorm[h], scaledBinWidth);//(float)binWidth);
                }
                else
                {
                    dc.FillRectangle(blueHatchBrushDiagDown,
                        LEFT_MARGIN, TOP_MARGIN + ScaledBinRight - scaledBinWidth,
                        GraphWidth * (float)histogramNorm[h], scaledBinWidth);//(float)binWidth);

                }
            }


        }


        private void DisplayMean(Graphics dc)
        {
            // draw mean value *****************************************************

            // x bar
            dc.DrawString("x", boldTimesFont, blackBrush,
                new PointF(LEFT_LOC_STATS + 6F, TOP_MARGIN + ScaledMean - 11F));
            dc.DrawString("\u0060", SymbolFont, blackBrush,
                new PointF(LEFT_LOC_STATS - 2F, TOP_MARGIN + ScaledMean - 6F));
            // x bar value = mean
            dc.DrawString(
                ActiveRatios.Mean.ToString("E5"),
                CourierFont9, blackBrush,
                new PointF(LEFT_LOC_STATS + 24F, TOP_MARGIN + ScaledMean - 8F));

            // print stats

            dc.DrawString(
                "s",
                SymbolFont, blackBrush,
                new PointF(LEFT_LOC_STATS + 3F, TOP_MARGIN + ScaledMeanAdj + (1.5F * 8F) - 4F));
            dc.DrawString(
                ActiveRatios.StdDev.ToString("E5"),
                CourierFont9, blackBrush,
                new PointF(LEFT_LOC_STATS + 24F, TOP_MARGIN + ScaledMeanAdj + (1.5F * 8F)));

            // percent standard error of the mean sigma sub x bar
            dc.DrawString(
                "%",
                CourierFont9, blackBrush,
                new PointF(LEFT_LOC_STATS + 0F, TOP_MARGIN + ScaledMeanAdj + (3.0F * 8F)));
            dc.DrawString(
                "s",
                SymbolFont, blackBrush,
                new PointF(LEFT_LOC_STATS + 4F, TOP_MARGIN + ScaledMeanAdj + (3.0F * 8F) - 4F));
            dc.DrawString("x", CourierFont8, blackBrush,
                new PointF(LEFT_LOC_STATS + 14F, TOP_MARGIN + ScaledMeanAdj + (3.0F * 8F) + 6F));
            dc.DrawString("\u0060", SymbolFont, blackBrush,
                new PointF(LEFT_LOC_STATS + 3F, TOP_MARGIN + ScaledMeanAdj + (3.0F * 8F) + 9F));
            dc.DrawString(
                ActiveRatios.PctStdErr.ToString("E5"),
                CourierFont9, blackBrush,
                new PointF(LEFT_LOC_STATS + 24F, TOP_MARGIN + ScaledMeanAdj + (3.0F * 8F)));

            // sample population n
            dc.DrawString(
                " n   " + ActiveRatios.LiveDataCount.ToString(),
                CourierFont9, blackBrush,
                new PointF(LEFT_LOC_STATS + 3F, TOP_MARGIN + ScaledMeanAdj + (4.8F * 8F)));
        }

        private void DisplayMissingCount(Graphics dc)
        {
            // number missing or bad - if any
            if (ActiveRatios.AllMissing != 0)
            {
                dc.DrawString(
                    "      (" + ActiveRatios.AllMissing.ToString() + " absent)",
                    CourierFont9, redBrush,
                    new PointF(ORIGINAL_STATS_MARGIN + 3F, (4.8F * 8F)));
            }
        }

        #endregion Methods


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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRatioGraph));
            this.ButtonPanel = new System.Windows.Forms.Panel();
            this.btnDiscardOutliers = new System.Windows.Forms.Button();
            this.pnlDiscards = new System.Windows.Forms.Panel();
            this.rbIgnoreDiscards = new System.Windows.Forms.RadioButton();
            this.rbHideDiscards = new System.Windows.Forms.RadioButton();
            this.rbIncludeDiscards = new System.Windows.Forms.RadioButton();
            this.lblDiscards = new System.Windows.Forms.Label();
            this.pnlChooseOutliers = new System.Windows.Forms.Panel();
            this.lblCAUTION = new System.Windows.Forms.Label();
            this.rbChauvenet = new System.Windows.Forms.RadioButton();
            this.rb2SigmaOutliers = new System.Windows.Forms.RadioButton();
            this.rbManualOutliers = new System.Windows.Forms.RadioButton();
            this.lblOutliers = new System.Windows.Forms.Label();
            this.pictureBoxHistogram = new System.Windows.Forms.PictureBox();
            this.numberBinsUpDown = new System.Windows.Forms.NumericUpDown();
            this.chkboxAutoCalc = new System.Windows.Forms.CheckBox();
            this.btnSynchToMe = new System.Windows.Forms.Button();
            this.btnUndoGroup = new System.Windows.Forms.Button();
            this.btnUndo = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.pnlSelectZone = new System.Windows.Forms.Panel();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.mnuPrintMain = new System.Windows.Forms.MenuItem();
            this.mnuPrintPortrait = new System.Windows.Forms.MenuItem();
            this.mnuPrintLandscape = new System.Windows.Forms.MenuItem();
            this.mnuGraphPointSize = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.mnuGraphDataDetail = new System.Windows.Forms.MenuItem();
            this.mnuGraphDataDetailPM = new System.Windows.Forms.MenuItem();
            this.mnuGraphDataDetailM = new System.Windows.Forms.MenuItem();
            this.mnuGraphStatsDetail = new System.Windows.Forms.MenuItem();
            this.mnuGraphStatsDetail2SSSe = new System.Windows.Forms.MenuItem();
            this.mnuGraphStatsDetailSSe = new System.Windows.Forms.MenuItem();
            this.mnuGraphStatsDetailSe = new System.Windows.Forms.MenuItem();
            this.menuItemToggleBlockSort = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlInfoZone = new System.Windows.Forms.Panel();
            this.txtGainsFile = new System.Windows.Forms.TextBox();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.menuItemToggleBlockMeans = new System.Windows.Forms.MenuItem();
            this.ButtonPanel.SuspendLayout();
            this.pnlDiscards.SuspendLayout();
            this.pnlChooseOutliers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHistogram)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberBinsUpDown)).BeginInit();
            this.pnlInfoZone.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonPanel
            // 
            this.ButtonPanel.BackColor = System.Drawing.Color.Linen;
            this.ButtonPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ButtonPanel.Controls.Add(this.btnDiscardOutliers);
            this.ButtonPanel.Controls.Add(this.pnlDiscards);
            this.ButtonPanel.Controls.Add(this.pnlChooseOutliers);
            this.ButtonPanel.Controls.Add(this.pictureBoxHistogram);
            this.ButtonPanel.Controls.Add(this.numberBinsUpDown);
            this.ButtonPanel.Controls.Add(this.chkboxAutoCalc);
            this.ButtonPanel.Controls.Add(this.btnSynchToMe);
            this.ButtonPanel.Controls.Add(this.btnUndoGroup);
            this.ButtonPanel.Controls.Add(this.btnUndo);
            this.ButtonPanel.Controls.Add(this.btnRestore);
            this.ButtonPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.ButtonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ButtonPanel.Location = new System.Drawing.Point(0, 372);
            this.ButtonPanel.Name = "ButtonPanel";
            this.ButtonPanel.Size = new System.Drawing.Size(670, 42);
            this.ButtonPanel.TabIndex = 4;
            // 
            // btnDiscardOutliers
            // 
            this.btnDiscardOutliers.BackColor = System.Drawing.Color.Bisque;
            this.btnDiscardOutliers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDiscardOutliers.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDiscardOutliers.Location = new System.Drawing.Point(58, 20);
            this.btnDiscardOutliers.Name = "btnDiscardOutliers";
            this.btnDiscardOutliers.Size = new System.Drawing.Size(108, 18);
            this.btnDiscardOutliers.TabIndex = 4;
            this.btnDiscardOutliers.Tag = "";
            this.btnDiscardOutliers.Text = "Remove Marked";
            this.btnDiscardOutliers.UseCompatibleTextRendering = true;
            this.btnDiscardOutliers.UseVisualStyleBackColor = false;
            this.btnDiscardOutliers.Visible = false;
            this.btnDiscardOutliers.Click += new System.EventHandler(this.btnDiscardOutliers_Click);
            // 
            // pnlDiscards
            // 
            this.pnlDiscards.BackColor = System.Drawing.Color.PeachPuff;
            this.pnlDiscards.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlDiscards.Controls.Add(this.rbIgnoreDiscards);
            this.pnlDiscards.Controls.Add(this.rbHideDiscards);
            this.pnlDiscards.Controls.Add(this.rbIncludeDiscards);
            this.pnlDiscards.Controls.Add(this.lblDiscards);
            this.pnlDiscards.Location = new System.Drawing.Point(175, -3);
            this.pnlDiscards.Name = "pnlDiscards";
            this.pnlDiscards.Size = new System.Drawing.Size(141, 46);
            this.pnlDiscards.TabIndex = 20;
            // 
            // rbIgnoreDiscards
            // 
            this.rbIgnoreDiscards.BackColor = System.Drawing.Color.Bisque;
            this.rbIgnoreDiscards.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbIgnoreDiscards.Location = new System.Drawing.Point(62, 14);
            this.rbIgnoreDiscards.Name = "rbIgnoreDiscards";
            this.rbIgnoreDiscards.Size = new System.Drawing.Size(74, 12);
            this.rbIgnoreDiscards.TabIndex = 21;
            this.rbIgnoreDiscards.Text = "IGNORE";
            this.rbIgnoreDiscards.UseCompatibleTextRendering = true;
            this.rbIgnoreDiscards.UseVisualStyleBackColor = false;
            this.rbIgnoreDiscards.Click += new System.EventHandler(this.rbIgnoreDiscards_Click);
            // 
            // rbHideDiscards
            // 
            this.rbHideDiscards.BackColor = System.Drawing.Color.Bisque;
            this.rbHideDiscards.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbHideDiscards.Location = new System.Drawing.Point(62, 27);
            this.rbHideDiscards.Name = "rbHideDiscards";
            this.rbHideDiscards.Size = new System.Drawing.Size(74, 12);
            this.rbHideDiscards.TabIndex = 22;
            this.rbHideDiscards.Text = "HIDE";
            this.rbHideDiscards.UseCompatibleTextRendering = true;
            this.rbHideDiscards.UseVisualStyleBackColor = false;
            this.rbHideDiscards.Click += new System.EventHandler(this.rbHideDiscards_Click);
            // 
            // rbIncludeDiscards
            // 
            this.rbIncludeDiscards.BackColor = System.Drawing.Color.Bisque;
            this.rbIncludeDiscards.Checked = true;
            this.rbIncludeDiscards.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbIncludeDiscards.Location = new System.Drawing.Point(62, 1);
            this.rbIncludeDiscards.Name = "rbIncludeDiscards";
            this.rbIncludeDiscards.Size = new System.Drawing.Size(74, 12);
            this.rbIncludeDiscards.TabIndex = 20;
            this.rbIncludeDiscards.TabStop = true;
            this.rbIncludeDiscards.Text = "INCLUDE";
            this.rbIncludeDiscards.UseCompatibleTextRendering = true;
            this.rbIncludeDiscards.UseVisualStyleBackColor = false;
            this.rbIncludeDiscards.Click += new System.EventHandler(this.rbIncludeDiscards_Click);
            // 
            // lblDiscards
            // 
            this.lblDiscards.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscards.Location = new System.Drawing.Point(-1, 8);
            this.lblDiscards.Name = "lblDiscards";
            this.lblDiscards.Size = new System.Drawing.Size(59, 22);
            this.lblDiscards.TabIndex = 19;
            this.lblDiscards.Text = "Display of Discards:";
            this.lblDiscards.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDiscards.UseCompatibleTextRendering = true;
            // 
            // pnlChooseOutliers
            // 
            this.pnlChooseOutliers.BackColor = System.Drawing.Color.PeachPuff;
            this.pnlChooseOutliers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlChooseOutliers.Controls.Add(this.lblCAUTION);
            this.pnlChooseOutliers.Controls.Add(this.rbChauvenet);
            this.pnlChooseOutliers.Controls.Add(this.rb2SigmaOutliers);
            this.pnlChooseOutliers.Controls.Add(this.rbManualOutliers);
            this.pnlChooseOutliers.Controls.Add(this.lblOutliers);
            this.pnlChooseOutliers.Location = new System.Drawing.Point(316, -3);
            this.pnlChooseOutliers.Name = "pnlChooseOutliers";
            this.pnlChooseOutliers.Size = new System.Drawing.Size(142, 46);
            this.pnlChooseOutliers.TabIndex = 19;
            // 
            // lblCAUTION
            // 
            this.lblCAUTION.BackColor = System.Drawing.Color.Transparent;
            this.lblCAUTION.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCAUTION.ForeColor = System.Drawing.Color.Red;
            this.lblCAUTION.Location = new System.Drawing.Point(6, 2);
            this.lblCAUTION.Name = "lblCAUTION";
            this.lblCAUTION.Size = new System.Drawing.Size(48, 18);
            this.lblCAUTION.TabIndex = 23;
            this.lblCAUTION.Text = "Caution";
            this.lblCAUTION.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCAUTION.UseCompatibleTextRendering = true;
            this.lblCAUTION.Visible = false;
            // 
            // rbChauvenet
            // 
            this.rbChauvenet.BackColor = System.Drawing.Color.Bisque;
            this.rbChauvenet.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbChauvenet.Location = new System.Drawing.Point(62, 28);
            this.rbChauvenet.Name = "rbChauvenet";
            this.rbChauvenet.Size = new System.Drawing.Size(81, 12);
            this.rbChauvenet.TabIndex = 21;
            this.rbChauvenet.Text = "Chauvenet";
            this.rbChauvenet.UseCompatibleTextRendering = true;
            this.rbChauvenet.UseVisualStyleBackColor = false;
            this.rbChauvenet.Click += new System.EventHandler(this.rbChauvenetOutliers_Click);
            this.rbChauvenet.MouseHover += new System.EventHandler(this.rbChauvenet_MouseHover);
            // 
            // rb2SigmaOutliers
            // 
            this.rb2SigmaOutliers.BackColor = System.Drawing.Color.Bisque;
            this.rb2SigmaOutliers.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rb2SigmaOutliers.Location = new System.Drawing.Point(62, 14);
            this.rb2SigmaOutliers.Name = "rb2SigmaOutliers";
            this.rb2SigmaOutliers.Size = new System.Drawing.Size(72, 12);
            this.rb2SigmaOutliers.TabIndex = 22;
            this.rb2SigmaOutliers.Text = "2SIGMA";
            this.rb2SigmaOutliers.UseCompatibleTextRendering = true;
            this.rb2SigmaOutliers.UseVisualStyleBackColor = false;
            this.rb2SigmaOutliers.Click += new System.EventHandler(this.rb2SigmaOutliers_Click);
            // 
            // rbManualOutliers
            // 
            this.rbManualOutliers.BackColor = System.Drawing.Color.Bisque;
            this.rbManualOutliers.Checked = true;
            this.rbManualOutliers.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbManualOutliers.Location = new System.Drawing.Point(62, 1);
            this.rbManualOutliers.Margin = new System.Windows.Forms.Padding(0);
            this.rbManualOutliers.Name = "rbManualOutliers";
            this.rbManualOutliers.Size = new System.Drawing.Size(72, 12);
            this.rbManualOutliers.TabIndex = 20;
            this.rbManualOutliers.TabStop = true;
            this.rbManualOutliers.Text = "MANUAL";
            this.rbManualOutliers.UseCompatibleTextRendering = true;
            this.rbManualOutliers.UseVisualStyleBackColor = false;
            this.rbManualOutliers.Click += new System.EventHandler(this.rbManualOutliers_Click);
            // 
            // lblOutliers
            // 
            this.lblOutliers.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutliers.Location = new System.Drawing.Point(6, 8);
            this.lblOutliers.Name = "lblOutliers";
            this.lblOutliers.Size = new System.Drawing.Size(53, 28);
            this.lblOutliers.TabIndex = 19;
            this.lblOutliers.Text = "Choose Outliers:";
            this.lblOutliers.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblOutliers.UseCompatibleTextRendering = true;
            // 
            // pictureBoxHistogram
            // 
            this.pictureBoxHistogram.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxHistogram.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxHistogram.Image")));
            this.pictureBoxHistogram.Location = new System.Drawing.Point(137, 20);
            this.pictureBoxHistogram.Name = "pictureBoxHistogram";
            this.pictureBoxHistogram.Size = new System.Drawing.Size(38, 18);
            this.pictureBoxHistogram.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxHistogram.TabIndex = 8;
            this.pictureBoxHistogram.TabStop = false;
            // 
            // numberBinsUpDown
            // 
            this.numberBinsUpDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.numberBinsUpDown.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numberBinsUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numberBinsUpDown.Location = new System.Drawing.Point(137, 2);
            this.numberBinsUpDown.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numberBinsUpDown.Name = "numberBinsUpDown";
            this.numberBinsUpDown.Size = new System.Drawing.Size(38, 20);
            this.numberBinsUpDown.TabIndex = 7;
            this.toolTip1.SetToolTip(this.numberBinsUpDown, "Set number of Histogram Bins to overlay on graph");
            this.numberBinsUpDown.ValueChanged += new System.EventHandler(this.numberBinsUpDown_ValueChanged);
            // 
            // chkboxAutoCalc
            // 
            this.chkboxAutoCalc.BackColor = System.Drawing.Color.LightGreen;
            this.chkboxAutoCalc.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkboxAutoCalc.Checked = true;
            this.chkboxAutoCalc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkboxAutoCalc.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkboxAutoCalc.Location = new System.Drawing.Point(455, 2);
            this.chkboxAutoCalc.Name = "chkboxAutoCalc";
            this.chkboxAutoCalc.Size = new System.Drawing.Size(89, 36);
            this.chkboxAutoCalc.TabIndex = 6;
            this.chkboxAutoCalc.Text = "AutoCalc Stats";
            this.chkboxAutoCalc.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.chkboxAutoCalc.UseCompatibleTextRendering = true;
            this.chkboxAutoCalc.UseVisualStyleBackColor = false;
            this.chkboxAutoCalc.CheckedChanged += new System.EventHandler(this.chkboxAutoCalc_CheckedChanged);
            // 
            // btnSynchToMe
            // 
            this.btnSynchToMe.BackColor = System.Drawing.Color.Bisque;
            this.btnSynchToMe.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSynchToMe.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSynchToMe.Location = new System.Drawing.Point(2, 20);
            this.btnSynchToMe.Name = "btnSynchToMe";
            this.btnSynchToMe.Size = new System.Drawing.Size(56, 18);
            this.btnSynchToMe.TabIndex = 5;
            this.btnSynchToMe.Text = "SYNCH";
            this.btnSynchToMe.UseCompatibleTextRendering = true;
            this.btnSynchToMe.UseVisualStyleBackColor = false;
            this.btnSynchToMe.Click += new System.EventHandler(this.btnSynchToMe_Click);
            // 
            // btnUndoGroup
            // 
            this.btnUndoGroup.BackColor = System.Drawing.Color.Bisque;
            this.btnUndoGroup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUndoGroup.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUndoGroup.Location = new System.Drawing.Point(58, 20);
            this.btnUndoGroup.Name = "btnUndoGroup";
            this.btnUndoGroup.Size = new System.Drawing.Size(78, 18);
            this.btnUndoGroup.TabIndex = 3;
            this.btnUndoGroup.Text = "Undo Group";
            this.btnUndoGroup.UseVisualStyleBackColor = false;
            this.btnUndoGroup.Click += new System.EventHandler(this.btnUndoGroup_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.BackColor = System.Drawing.Color.Bisque;
            this.btnUndo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUndo.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUndo.Location = new System.Drawing.Point(58, 2);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(78, 18);
            this.btnUndo.TabIndex = 1;
            this.btnUndo.Text = "Undo Point";
            this.btnUndo.UseCompatibleTextRendering = true;
            this.btnUndo.UseVisualStyleBackColor = false;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // btnRestore
            // 
            this.btnRestore.BackColor = System.Drawing.Color.Bisque;
            this.btnRestore.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRestore.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRestore.Location = new System.Drawing.Point(2, 2);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(56, 18);
            this.btnRestore.TabIndex = 0;
            this.btnRestore.Text = "Restore";
            this.btnRestore.UseCompatibleTextRendering = true;
            this.btnRestore.UseVisualStyleBackColor = false;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // pnlSelectZone
            // 
            this.pnlSelectZone.BackColor = System.Drawing.Color.Transparent;
            this.pnlSelectZone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSelectZone.Location = new System.Drawing.Point(278, 54);
            this.pnlSelectZone.Name = "pnlSelectZone";
            this.pnlSelectZone.Size = new System.Drawing.Size(24, 284);
            this.pnlSelectZone.TabIndex = 5;
            this.pnlSelectZone.Visible = false;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuPrintMain,
            this.menuItem1});
            // 
            // mnuPrintMain
            // 
            this.mnuPrintMain.Index = 0;
            this.mnuPrintMain.MdiList = true;
            this.mnuPrintMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuPrintPortrait,
            this.mnuPrintLandscape,
            this.mnuGraphPointSize,
            this.mnuGraphDataDetail,
            this.mnuGraphStatsDetail,
            this.menuItemToggleBlockSort});
            this.mnuPrintMain.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
            this.mnuPrintMain.Text = "Graph";
            // 
            // mnuPrintPortrait
            // 
            this.mnuPrintPortrait.Index = 0;
            this.mnuPrintPortrait.Text = "Print Portrait";
            this.mnuPrintPortrait.Click += new System.EventHandler(this.mnuPrintPortrait_Click);
            // 
            // mnuPrintLandscape
            // 
            this.mnuPrintLandscape.Index = 1;
            this.mnuPrintLandscape.Text = "Print Landscape";
            this.mnuPrintLandscape.Click += new System.EventHandler(this.mnuPrintLandscape_Click);
            // 
            // mnuGraphPointSize
            // 
            this.mnuGraphPointSize.Index = 2;
            this.mnuGraphPointSize.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem3,
            this.menuItem4,
            this.menuItem5});
            this.mnuGraphPointSize.Text = "Point Size";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 0;
            this.menuItem3.RadioCheck = true;
            this.menuItem3.Text = "Small";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Checked = true;
            this.menuItem4.Index = 1;
            this.menuItem4.RadioCheck = true;
            this.menuItem4.Text = "Medium";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 2;
            this.menuItem5.RadioCheck = true;
            this.menuItem5.Text = "Large";
            this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
            // 
            // mnuGraphDataDetail
            // 
            this.mnuGraphDataDetail.Index = 3;
            this.mnuGraphDataDetail.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuGraphDataDetailPM,
            this.mnuGraphDataDetailM,
            this.menuItemToggleBlockMeans});
            this.mnuGraphDataDetail.Text = "Data Detail";
            // 
            // mnuGraphDataDetailPM
            // 
            this.mnuGraphDataDetailPM.Checked = true;
            this.mnuGraphDataDetailPM.Index = 0;
            this.mnuGraphDataDetailPM.RadioCheck = true;
            this.mnuGraphDataDetailPM.Text = "Points and Mean";
            this.mnuGraphDataDetailPM.Click += new System.EventHandler(this.mnuGraphDataDetailPM_Click);
            // 
            // mnuGraphDataDetailM
            // 
            this.mnuGraphDataDetailM.Index = 1;
            this.mnuGraphDataDetailM.RadioCheck = true;
            this.mnuGraphDataDetailM.Text = "Mean Only";
            this.mnuGraphDataDetailM.Click += new System.EventHandler(this.mnuGraphDataDetailM_Click);
            // 
            // mnuGraphStatsDetail
            // 
            this.mnuGraphStatsDetail.Index = 4;
            this.mnuGraphStatsDetail.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuGraphStatsDetail2SSSe,
            this.mnuGraphStatsDetailSSe,
            this.mnuGraphStatsDetailSe});
            this.mnuGraphStatsDetail.Text = "Stats Detail";
            // 
            // mnuGraphStatsDetail2SSSe
            // 
            this.mnuGraphStatsDetail2SSSe.Index = 0;
            this.mnuGraphStatsDetail2SSSe.Text = "2-sigma, sigma, stderr";
            this.mnuGraphStatsDetail2SSSe.Click += new System.EventHandler(this.mnuGraphStatsDetail2SSSe_Click);
            // 
            // mnuGraphStatsDetailSSe
            // 
            this.mnuGraphStatsDetailSSe.Index = 1;
            this.mnuGraphStatsDetailSSe.Text = "sigma, stderr";
            this.mnuGraphStatsDetailSSe.Click += new System.EventHandler(this.mnuGraphStatsDetailSSe_Click);
            // 
            // mnuGraphStatsDetailSe
            // 
            this.mnuGraphStatsDetailSe.Index = 2;
            this.mnuGraphStatsDetailSe.Text = "stderr";
            this.mnuGraphStatsDetailSe.Click += new System.EventHandler(this.mnuGraphStatsDetailSe_Click);
            // 
            // menuItemToggleBlockSort
            // 
            this.menuItemToggleBlockSort.Index = 5;
            this.menuItemToggleBlockSort.Text = "Experiment: Toggle Block Sort and toss Hi 2 / Lo 2";
            this.menuItemToggleBlockSort.Click += new System.EventHandler(this.menuItemToggleBlockSort_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.MergeOrder = 9;
            this.menuItem1.Text = "Help";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 5;
            this.toolTip1.AutoPopDelay = 1000;
            this.toolTip1.InitialDelay = 5;
            this.toolTip1.ReshowDelay = 1;
            // 
            // pnlInfoZone
            // 
            this.pnlInfoZone.BackColor = System.Drawing.Color.Azure;
            this.pnlInfoZone.Controls.Add(this.txtGainsFile);
            this.pnlInfoZone.Location = new System.Drawing.Point(416, 94);
            this.pnlInfoZone.Name = "pnlInfoZone";
            this.pnlInfoZone.Size = new System.Drawing.Size(108, 58);
            this.pnlInfoZone.TabIndex = 6;
            this.pnlInfoZone.Visible = false;
            // 
            // txtGainsFile
            // 
            this.txtGainsFile.BackColor = System.Drawing.Color.Azure;
            this.txtGainsFile.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtGainsFile.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGainsFile.Location = new System.Drawing.Point(11, 9);
            this.txtGainsFile.Multiline = true;
            this.txtGainsFile.Name = "txtGainsFile";
            this.txtGainsFile.Size = new System.Drawing.Size(86, 40);
            this.txtGainsFile.TabIndex = 0;
            this.txtGainsFile.Text = "0";
            // 
            // toolTip2
            // 
            this.toolTip2.AutomaticDelay = 5;
            this.toolTip2.AutoPopDelay = 1000;
            this.toolTip2.InitialDelay = 5;
            this.toolTip2.ReshowDelay = 1;
            // 
            // menuItemToggleBlockMeans
            // 
            this.menuItemToggleBlockMeans.Index = 2;
            this.menuItemToggleBlockMeans.Text = "Toggle Block Means";
            this.menuItemToggleBlockMeans.Click += new System.EventHandler(this.menuItemToggleBlockMeans_Click);
            // 
            // frmRatioGraph
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(670, 414);
            this.Controls.Add(this.pnlInfoZone);
            this.Controls.Add(this.pnlSelectZone);
            this.Controls.Add(this.ButtonPanel);
            this.Cursor = System.Windows.Forms.Cursors.Cross;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.Name = "frmRatioGraph";
            this.Load += new System.EventHandler(this.frmRatioGraph_Load);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmRatioGraph_MouseUp);
            this.SizeChanged += new System.EventHandler(this.frmRatioGraph_SizeChanged);
            this.Enter += new System.EventHandler(this.frmRatioGraph_Enter);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmRatioGraph_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmRatioGraph_MouseMove);
            this.ButtonPanel.ResumeLayout(false);
            this.pnlDiscards.ResumeLayout(false);
            this.pnlChooseOutliers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHistogram)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberBinsUpDown)).EndInit();
            this.pnlInfoZone.ResumeLayout(false);
            this.pnlInfoZone.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private void frmRatioGraph_SizeChanged(object sender, System.EventArgs e)
        {
            this.Refresh();
        }

        private void btnRestore_Click(object sender, System.EventArgs e)
        {
            // check for inherited form
            if (((Button)sender).Text.EndsWith("Restore"))
            {
                RestoreGraphData();
            }
        }

        public void RestoreGraphData()
        {
            ActiveRatios.RestoreData();

            ResetSynchButton();
            rbIncludeDiscards.Checked = true;
            rbManualOutliers.Checked = true;
            rb2SigmaOutliers.Enabled = true;
            rbChauvenet.Enabled = true;

            Refresh();
        }

        private void btnUndo_Click(object sender, System.EventArgs e)
        {
            UndoPoints();
            ActiveRatios.CalcStats();
            ResetSynchButton();
            Refresh();
        }

        private void UndoPoints()
        {
            //first see if last cluster is empty and if it is dump it
            if (ActiveRatios.UndoHistory.Count > 0)
            {
                if (((ArrayList)ActiveRatios.UndoHistory[ActiveRatios.UndoHistory.Count - 1]).Count == 0)
                    ActiveRatios.UndoHistory.RemoveAt(ActiveRatios.UndoHistory.Count - 1);
            }
            // if any left proceed
            if (ActiveRatios.UndoHistory.Count > 0)
            {
                try
                {
                    ActiveRatios.ToggleRatio(//.RestoreRatio(
                        (int)((ArrayList)ActiveRatios.UndoHistory[ActiveRatios.UndoHistory.Count - 1])[((ArrayList)ActiveRatios.UndoHistory[ActiveRatios.UndoHistory.Count - 1]).Count - 1]);
                    ((ArrayList)ActiveRatios.UndoHistory[ActiveRatios.UndoHistory.Count - 1]).RemoveAt(((ArrayList)ActiveRatios.UndoHistory[ActiveRatios.UndoHistory.Count - 1]).Count - 1);
                }
                catch { }
            }
        }


        private ArrayList DataSelected()
        {
            // retval stores the indexes of activeratios that are still in the set
            ArrayList retval = new ArrayList();
            int DataCount = 0;
            // figure out what is in bounding box
            // first narrow by x-axis ticks then do value check
            int LeftTick = (int)((float)(pnlSelectZone.Left - LEFT_MARGIN) / XtickGap + 1.0F) - 1;//+ 0.5F) - 1;
            if (LeftTick < 0) LeftTick = 0;
            int RightTick =
                (int)((float)(pnlSelectZone.Left + pnlSelectZone.Width - LEFT_MARGIN) / XtickGap + 1.0F) - 2;//+ 0.5F) - 1;
            // edge condition
            if (RightTick > ActiveRatios.ActiveRatios.Count - 1)
                RightTick = ActiveRatios.ActiveRatios.Count - 1;

            for (int tick = LeftTick; tick <= RightTick; tick++)
            {
                // check to see if value in box
                // Dec 2006 with the addition of the ignore mode for
                // viewing data, we must be sure that the height of the box
                // is effectively infinite when blocks are toggled, as
                // some of the data are off the map so to speak
                // so test for BlockSelected

                float ScaledData =
                    (float)
                    ((Ymax - Math.Abs((double)ActiveRatios.ActiveRatios[tick])) / YRange)
                    * GraphHeight;

                if (BlockSelected
                    ||
                    (((TOP_MARGIN + ScaledData) >= (float)(pnlSelectZone.Top - 1F))
                    &&
                    ((TOP_MARGIN + ScaledData)
                    <= (float)(pnlSelectZone.Top + pnlSelectZone.Height + 1))))
                {
                    retval.Add(tick);
                    // if not selected as outlier previously, increment count
                    if ((double)ActiveRatios.ActiveRatios[tick] > 0)//>= 0)
                        DataCount++;
                }
            }
            // add count as last element
            retval.Add(DataCount);
            return retval;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myText"></param>
        public static void SendTextToClipboard(string myText)
        {
            if (myText != "") Clipboard.SetDataObject(myText);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetPctStdErrAsText()
        {
            return
                ActiveRatios.Name + "  (Mean, %StdErr)"
                + "\r\n"
                + ActiveRatios.Mean.ToString("E7") // try F20
                + "\r\n"
                + ActiveRatios.PctStdErr.ToString("E7")
                + "\r\n" + "\r\n";
        }

        private void frmRatioGraph_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // check for inherited form
            if (sender.GetType() != typeof(frmRatioGraph))
                return;

            if (DoTickTest(e))
            {
                MouseDownResponse(e);
            }
        }

        protected bool DoTickTest(System.Windows.Forms.MouseEventArgs e)
        {
            TickTest = false;

            if (_DisableGraph)
            {
                this.Cursor = Cursors.Default;
                return TickTest;
            }

            // Suppress data selection if means only
            if (mnuGraphDataDetailM.Checked)
            {
                this.Cursor = Cursors.Default;
                return TickTest;
            }

            if (doClipboard)
            {
                SendTextToClipboard(GetPctStdErrAsText());
            }
            else
            {
                // Left mouse down
                // we log where the mouse goes down and will zap everything
                // that is included in the selection zone when mouse raised

                // detect which value based on x coordinate
                TickValueStart = (int)(((float)e.X - LEFT_MARGIN) / XtickGap + 0.5F) - 2;
                //test tick value
                TickTest = ((TickValueStart >= -2)
                    &&
                    (e.Y > (int)TOP_MARGIN)
                    &&
                    (e.Y < (int)(TOP_MARGIN + GraphHeight))
                    &&
                    (TickValueStart < ActiveRatios.ActiveRatios.Count + 3)
                    &&
                    (ActiveRatios.ActiveRatios.Count > 2));
            }
            return TickTest;
        }

        private void MouseDownResponse(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    // set up selection zone
                    pnlSelectZone.Top = e.Y;
                    YStart = e.Y;
                    pnlSelectZone.Left = e.X;
                    pnlSelectZone.Height = 1;//(int)GraphHeight;
                    pnlSelectZone.Width = 1;
                    pnlSelectZone.Visible = true;
                }
                catch { }

            }
            else if (e.Button == MouseButtons.Right
                &&
                (ActiveRatios.HandleDiscardsFlag != (int)RawRatio.HandleDiscards.Hide))
            {
                // we toggle a block per the following rule: March 2006
                // color the data by minority color (2 red, 8 black => 10 red)
                // where 0 is a minority
                // check to make sure there is more than one block
                if (ActiveRatios.CyclesPerBlock < ActiveRatios.AllCount)
                {
                    try
                    {
                        // here we set up a block selection
                        // make the panel big enough to get the whole block
                        pnlSelectZone.Left
                            = (int)((((int)(1F +
                            ((int)(Math.Floor((double)(TickValueStart + 1) / ActiveRatios.CyclesPerBlock)))
                            * ActiveRatios.CyclesPerBlock)) - 0.25F)
                            * XtickGap + LEFT_MARGIN);
                        pnlSelectZone.Top = (int)TOP_MARGIN;
                        pnlSelectZone.Height = (int)GraphHeight;
                        pnlSelectZone.Width = (int)(XtickGap * (0.25F + ActiveRatios.CyclesPerBlock));
                        TickTest = true;
                        pnlSelectZone.Visible = true;
                        pnlSelectZone.BackColor = Color.MistyRose;

                        BlockSelected = true;
                    }
                    catch { }
                }
                else
                {
                    TickTest = false;
                    BlockSelected = false;
                }

            }
        }

        private void frmRatioGraph_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // check for inherited form
            if (sender.GetType() != typeof(frmRatioGraph))
                return;

            if (_DisableGraph)
            {
                this.Cursor = Cursors.Default;
                return;
            }
            // works for either left or right mouse up

            // always zap info panel
            pnlInfoZone.Visible = false;

            pnlSelectZone.BackColor = Color.Transparent;
            // only if TickTest is true (ie mouse down was legal) and select zone still visible
            if ((TickTest) && pnlSelectZone.Visible)
            {
                // make all block members outliers March 2006
                // see mousedownresponse above for rule
                if (BlockSelected)
                {
                    // dec 2006 moved after next line BlockSelected = false;
                    ActiveRatios.SetBlockOutliers(DataSelected());
                    BlockSelected = false;
                }
                else
                    ActiveRatios.SetOutliers(DataSelected());


                ResetSynchButton();

                try
                {
                    Refresh();
                }
                catch { }

                pnlSelectZone.Visible = false;
            }
        }

        private void frmRatioGraph_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // check for inherited form
            if (sender.GetType() != typeof(frmRatioGraph))
                return;

            if (_DisableGraph)
            {
                this.Cursor = Cursors.Default;
                return;
            }

            // check if in zone for stats to send to reportOfPbMacDat
            if (e.Button == MouseButtons.None)
            {
                if ((e.X > LEFT_LOC_STATS)
                    &&
                    (e.Y > (TOP_MARGIN + ScaledMeanAdj))
                    &&
                    (e.Y < (TOP_MARGIN + ScaledMeanAdj + 60F)))
                {
                    this.Cursor = Cursors.Hand;
                    doClipboard = true;
                    toolTip1.Active = true;
                    toolTip2.Active = false;
                }
                else
                {
                    // this added feb 2005 to correct bug
                    // modified July 2005 to restrict cross cursor to actual graph
                    if ((e.X > LEFT_MARGIN)
                        &&
                        (e.X < (LEFT_MARGIN + GraphWidth))
                        &&
                        (e.Y > TOP_MARGIN)
                        &&
                        (e.Y < (TOP_MARGIN + GraphHeight))
                        )
                    {
                        this.Cursor = Cursors.Cross;
                        //toolTip2.Active = !ActiveRatios.DiscardOutliers;//   true;
                        // changed for ThermoFinniganTriton March 2006 (has only 1 block)
                        toolTip2.Active =
                            (ActiveRatios.HandleDiscardsFlag != (int)RawRatio.HandleDiscards.Hide)
                            &&
                            (ActiveRatios.AllCount > ActiveRatios.CyclesPerBlock);


                    }
                    else
                    {
                        this.Cursor = Cursors.Default;
                        toolTip2.Active = false;
                    }
                    doClipboard = false;
                    toolTip1.Active = false;
                }
                //}
            }

            if (e.Button == MouseButtons.Left)
            {
                if ((pnlSelectZone.Visible)
                    &&
                    (e.X >= (int)(0.8F * LEFT_MARGIN))
                    &&
                    (e.X <= (int)(1.2F * LEFT_MARGIN + GraphWidth))
                    &&
                    (e.Y > (int)(0.8F * TOP_MARGIN))
                    &&
                    (e.Y < (int)(1.2F * TOP_MARGIN + GraphHeight)))
                {
                    int TickValue = (int)((float)(e.X - LEFT_MARGIN) / XtickGap + 0.0F) - 1;//+ 0.5F) - 1;
                    if (TickValue >= TickValueStart)
                        pnlSelectZone.Width = (int)(((float)(TickValue - TickValueStart) + 1.0F) * XtickGap);// + 0.5F) * XtickGap);
                    else
                    {
                        pnlSelectZone.Left = e.X;
                        pnlSelectZone.Width = (int)(((float)(TickValueStart - TickValue) + 1.0F) * XtickGap);//+ 0.5F) * XtickGap);
                    }
                    if (e.Y >= YStart)
                    {
                        pnlSelectZone.Top = YStart;
                        pnlSelectZone.Height = e.Y - YStart + 1;
                    }
                    else
                    {
                        pnlSelectZone.Top = e.Y;
                        pnlSelectZone.Height = YStart - e.Y;
                    }

                }
                else
                    pnlSelectZone.Visible = false;
            }
        }

        private void btnUndoGroup_Click(object sender, System.EventArgs e)
        {
            // undo last cluster of discards
            if (ActiveRatios.UndoHistory.Count > 0)
            {
                int counter = (((ArrayList)ActiveRatios.UndoHistory[ActiveRatios.UndoHistory.Count - 1]).Count);
                for (int i = 0; i < counter; i++)
                {
                    UndoPoints();
                }
                ActiveRatios.UndoHistory.RemoveAt(ActiveRatios.UndoHistory.Count - 1);
            }
            ActiveRatios.CalcStats();
            ResetSynchButton();
            Refresh();
        }

        private void btnDiscardOutliers_Click(object sender, System.EventArgs e)
        {
            /*// check for inherited form (could be history graph)
            if (!((Button)sender).Text.EndsWith("Discards")) 
                return;
			
            ActiveRatios.DiscardOutliers = !ActiveRatios.DiscardOutliers;
            if (ActiveRatios.DiscardOutliers)
            {
                btnDiscardOutliers.Text = "Show Discards";
                //toolTip2.Active = false;
            }
            else
            {
                btnDiscardOutliers.Text = (string)btnDiscardOutliers.Tag; //"Hide Outliers";
//				toolTip2.Active = true;
            }
            ActiveRatios.CalcStats();
            Refresh();*/
        }

        #region //  menu point size and data detail

        private void menuItem3_Click(object sender, System.EventArgs e)
        {
            // small
            ActiveRatios.DataPenSize = 1.0F * Math.Sign(ActiveRatios.DataPenSize);
            SetPenSizeMenus(true, false, false);
            Refresh();
        }

        private void menuItem4_Click(object sender, System.EventArgs e)
        {
            // medium = DEFAULT
            ActiveRatios.DataPenSize = 1.5F * Math.Sign(ActiveRatios.DataPenSize);
            SetPenSizeMenus(false, true, false);
            Refresh();
        }

        private void menuItem5_Click(object sender, System.EventArgs e)
        {
            // large
            ActiveRatios.DataPenSize = 2.0F * Math.Sign(ActiveRatios.DataPenSize);
            SetPenSizeMenus(false, false, true);
            Refresh();
        }


        private void SetPenSizeMenus(bool small, bool med, bool large)
        {
            menuItem3.Checked = small;
            menuItem4.Checked = med;
            menuItem5.Checked = large;
        }

        /// <summary>
        /// Show points and mean = default f.e. pensize >0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuGraphDataDetailPM_Click(object sender, System.EventArgs e)
        {
            ActiveRatios.DataPenSize = Math.Abs(ActiveRatios.DataPenSize);
            mnuGraphDataDetailPM.Checked = true;
            mnuGraphDataDetailM.Checked = false;
            Refresh();
        }

        /// <summary>
        /// Show mean only =  f.e. pensize < 0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuGraphDataDetailM_Click(object sender, System.EventArgs e)
        {
            ActiveRatios.DataPenSize = -1.0F * Math.Abs(ActiveRatios.DataPenSize);
            mnuGraphDataDetailPM.Checked = false;
            mnuGraphDataDetailM.Checked = true;
            Refresh();
        }

        #endregion // menu point size and data detail

        private void btnSynchToMe_Click(object sender, System.EventArgs e)
        {
            /*// this prevents synchronizing to bad (ie created by another synch)
            if (ActiveRatios.HandleOutliersFlag != (int)RawRatio.HandleOutliers.Manual
                &&
                ActiveRatios.AmSynchronized)
            {
                MessageBox.Show("Please set Outlier Selection to MANUAL because "
                    + " the outliers shown here were created by a previous"
                    + " synchronization and are not valid for this ratio.",
                    "Tripoli CAUTION");
            }
            else if (ActiveRatios.HandleOutliersFlag != (int)RawRatio.HandleOutliers.Manual)
            {
                MessageBox.Show("CAUTION - the outliers created in the synchronized ratio graphs"	
                    + " are NOT valid outliers and are for information only.",
                    "Tripoli CAUTION");

                ((TripoliDisplayPanel)caller).SynchRatios[myGraphIndex].PerformClick();
            }
            else 
            {*/
            ((TripoliDisplayPanel)caller).SynchRatios[myGraphIndex].PerformClick();
            //}

        }

        protected void ResetSynchButton()
        {
            ActiveRatios.AmSynchronizer = false;
            ActiveRatios.AmSynchronized = false;
            ((TripoliDisplayPanel)caller).SynchRatios[myGraphIndex].BackColor = Color.Cornsilk;
        }

        private void frmRatioGraph_Enter(object sender, System.EventArgs e)
        {
            // setup pensize menu
            if (Math.Abs(ActiveRatios.DataPenSize) == 1.0F)
                SetPenSizeMenus(true, false, false);
            if (Math.Abs(ActiveRatios.DataPenSize) == 1.5F)
                SetPenSizeMenus(false, true, false);
            if (Math.Abs(ActiveRatios.DataPenSize) == 2.0F)
                SetPenSizeMenus(false, false, true);

            mnuGraphDataDetailPM.Checked = (ActiveRatios.DataPenSize > 0);
            mnuGraphDataDetailM.Checked = !mnuGraphDataDetailPM.Checked;
        }

        private void menuItem1_Click(object sender, System.EventArgs e)
        {
            Process TripoliHelp = new Process();
            TripoliHelp.StartInfo.FileName = "Notepad";
            TripoliHelp.StartInfo.Arguments = Application.StartupPath + @"\docs\TripoliHelp.txt";
            TripoliHelp.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            TripoliHelp.Start();

        }

        private void mnuPrintPortrait_Click(object sender, System.EventArgs e)
        {
            // check for inherited form
            if (GraphIdentity != "RATIO")
                return;

            printPrinter(false);
        }

        private void mnuPrintLandscape_Click(object sender, System.EventArgs e)
        {
            // check for inherited form
            if (GraphIdentity != "RATIO")
                return;

            printPrinter(true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="landscape"></param>
        public void printPrinter(bool landscape)
        {
            try
            {
                // Assumes the default printer.
                PrintDocument pd = new PrintDocument();
                if (landscape)
                    pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPageLandscape);
                else
                    pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPagePortrait);
                pd.DocumentName = "Tripoli Graph";
                pd.OriginAtMargins = true;

                pageSetupDialog1.Document = pd;
                pageSetupDialog1.PageSettings.Landscape = landscape;
                pageSetupDialog1.PageSettings.Margins = new Margins(50, 50, 50, 50);

                printDialog1.Document = pd;
                if (printDialog1.ShowDialog() == DialogResult.OK)
                    pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while printing", ex.ToString());
            }
        }

        // Specifies what happens when the PrintPage event is raised.

        private void pd_PrintPageLandscape(object sender, PrintPageEventArgs ev)
        {
            pd_PrintPage(ev, 950, 450, 25);
        }

        private void pd_PrintPagePortrait(object sender, PrintPageEventArgs ev)
        {
            pd_PrintPage(ev, 750, 450, 50);
        }

        private void pd_PrintPage(PrintPageEventArgs ev, int width, int height, int rows)
        {
            // check for inherited form
            if (GraphIdentity != "RATIO")
                return;

            // Draw a picture.
            Graphics dc = ev.Graphics;

            OnPaintDirective(dc, width, height);

            // Nov 2006 test for TripoliWorkFile
            string workFileName = "NONE";
            if (((frmMainTripoli)((TripoliDisplayPanel)caller).Parent).TripoliFileInfo != null)
                workFileName =
                    ((frmMainTripoli)((TripoliDisplayPanel)caller).Parent)
                    .TripoliFileInfo.Name;


            dc.DrawString(
                "Produced from Tripoli Work File: "
                + workFileName
                + "\r\n"
                + "   on date: " + System.DateTime.Now.ToLongDateString()
                + "\r\n"
                + "Based on source file: "
                //+ "\r\n"
                + ((TripoliWorkProduct)((TripoliDisplayPanel)caller)
                .CurrentWorkProduct).SourceFileInfo.Name
                + "\r\n"
                + "Chauvenet's Criterion Threshold = " 
                + ChauvenetsThreshold
                + "\r\n" ,
                CourierFont9, blackBrush,
                new PointF(5F, TOP_MARGIN + GraphHeight + 25F));

            // Indicate that this is the last page to print.
            ev.HasMorePages = false;
        }
        #region IComparable Members




        public int CompareTo(object obj)
        {
            // TODO:  Add frmRatioGraph.CompareTo implementation

            frmRatioGraph f = (frmRatioGraph)obj;
            return this.myGraphIndex.CompareTo(f.myGraphIndex);
        }

        #endregion

        private void chkboxAutoCalc_CheckedChanged(object sender, System.EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            { // green appearance
                ((CheckBox)sender).BackColor = Color.LightGreen;
                this._ActiveRatios.AutoCalculate = true;
                this._ActiveRatios.CalcStats();
                this.rbChauvenet.Enabled = this.rb2SigmaOutliers.Enabled;
            }
            else
            { // red appearance
                ((CheckBox)sender).BackColor = Color.Red;
                this._ActiveRatios.AutoCalculate = false;
                this.rbChauvenet.Enabled = false;
            }

            Refresh();
        }

        private void numberBinsUpDown_ValueChanged(object sender, System.EventArgs e)
        {
            ActiveRatios.HistogramBinCount = (int)((NumericUpDown)sender).Value;
            Refresh();
        }

        private void mnuGraphStatsDetail2SSSe_Click(object sender, System.EventArgs e)
        {
            ActiveRatios.StatsDetailDisplay = (int)RawRatio.StatsDisplayed.TwoSigmaSigmaStdErr;
            mnuGraphStatsDetail2SSSe.Checked = true;
            mnuGraphStatsDetailSSe.Checked = false;
            mnuGraphStatsDetailSe.Checked = false;
            Refresh();
        }

        private void mnuGraphStatsDetailSSe_Click(object sender, System.EventArgs e)
        {
            ActiveRatios.StatsDetailDisplay = (int)RawRatio.StatsDisplayed.SigmaStdErr;
            mnuGraphStatsDetail2SSSe.Checked = false;
            mnuGraphStatsDetailSSe.Checked = true;
            mnuGraphStatsDetailSe.Checked = false;
            Refresh();

        }

        private void mnuGraphStatsDetailSe_Click(object sender, System.EventArgs e)
        {
            ActiveRatios.StatsDetailDisplay = (int)RawRatio.StatsDisplayed.StdErr;
            mnuGraphStatsDetail2SSSe.Checked = false;
            mnuGraphStatsDetailSSe.Checked = false;
            mnuGraphStatsDetailSe.Checked = true;
            Refresh();

        }

        private void rbIncludeDiscards_Click(object sender, System.EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                ActiveRatios.HandleDiscardsFlag = (int)RawRatio.HandleDiscards.Include;
            ActiveRatios.CalcStats();
            ResetSynchButton();

            Refresh();
        }

        private void rbIgnoreDiscards_Click(object sender, System.EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                ActiveRatios.HandleDiscardsFlag = (int)RawRatio.HandleDiscards.Ignore;
            ActiveRatios.CalcStats();
            ResetSynchButton();

            Refresh();

        }

        private void rbHideDiscards_Click(object sender, System.EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                ActiveRatios.HandleDiscardsFlag = (int)RawRatio.HandleDiscards.Hide;
            ActiveRatios.CalcStats();
            ResetSynchButton();

            Refresh();

        }

        // Outlier handling ***********************************************
        private void rbManualOutliers_Click(object sender, System.EventArgs e)
        {
            btnUndo.Enabled = true;
            btnUndoGroup.Enabled = true;

            if ((ActiveRatios.HandleOutliersFlag
                != (int)RawRatio.HandleOutliers.Manual)
                &&
                ActiveRatios.CurrentOutliersSelected.Count > 1)
            {
                // undo either 2-sigma or Chauvenet

                btnUndoGroup.PerformClick();
                ActiveRatios.CurrentOutliersSelected = new ArrayList();
            }

            rb2SigmaOutliers.Enabled = true;
            rbChauvenet.Enabled = chkboxAutoCalc.Checked;
            ActiveRatios.HandleOutliersFlag = (int)RawRatio.HandleOutliers.Manual;
            //ActiveRatios.AmSynchronizer = true; // this allows for synch to proceed
            ResetSynchButton();

            Refresh();
        }

        private void rb2SigmaOutliers_Click(object sender, System.EventArgs e)
        {
            // perform 2-sigma outlier selection on existing set of active points
            // make this and Chauvenet and all other choices inactive until manual clicked
            // disable graph?? see line 456
            //_DisableGraph = true;
            rb2SigmaOutliers.Enabled = false;
            rbChauvenet.Enabled = false;

            btnUndo.Enabled = false;
            btnUndoGroup.Enabled = false;

            ActiveRatios.HandleOutliersFlag = (int)RawRatio.HandleOutliers.TwoSigma;

            ActiveRatios.CurrentOutliersSelected = new ArrayList();
            int counter = 0;

            for (int i = 0; i < ActiveRatios.ActiveRatios.Count; i++)
            {
                if ((double)ActiveRatios.ActiveRatios[i] > 0 // f.e. active
                    &&
                    (Math.Abs((double)ActiveRatios.ActiveRatios[i] - ActiveRatios.Mean)
                    >
                    (ActiveRatios.StdDev * 2.0)))
                {
                    ActiveRatios.CurrentOutliersSelected.Add(i);
                    counter++;
                }
            }
            ActiveRatios.CurrentOutliersSelected.Add(counter);

            // this also puts these points into the undo group
            ActiveRatios.SetOutliers(ActiveRatios.CurrentOutliersSelected);
            ResetSynchButton();


            Refresh();
        }

        private void rbChauvenetOutliers_Click(object sender, System.EventArgs e)
        {
            rb2SigmaOutliers.Enabled = false;
            rbChauvenet.Enabled = false;

            btnUndo.Enabled = false;
            btnUndoGroup.Enabled = false;

            ActiveRatios.HandleOutliersFlag = (int)RawRatio.HandleOutliers.Chauvenet;

            // find the point the greatest number of sigma away
            double chauvenet = 0.0;
            ArrayList saveIndexes = new ArrayList();

            do
            {
                double z_value = 0.0;
                int z_index = -1;
                int countedActive = 0;
                ActiveRatios.CurrentOutliersSelected = new ArrayList();

                for (int i = 0; i < ActiveRatios.ActiveRatios.Count; i++)
                {
                    double z_temp = 0.0;
                    if ((double)ActiveRatios.ActiveRatios[i] > 0)// f.e. active
                    {
                        countedActive += 1;
                        z_temp =
                            (Math.Abs((double)ActiveRatios.ActiveRatios[i] - ActiveRatios.Mean) / ActiveRatios.StdDev);
                        if (z_temp > z_value)
                        {
                            z_value = z_temp;
                            z_index = i;
                        }
                    }
                }

                chauvenet = countedActive * SpecialFunction.erfc(z_value / Math.Sqrt(2.0));
                if (chauvenet < ChauvenetsThreshold)
                {
                    // toss the point
                    saveIndexes.Add(z_index);

                    ActiveRatios.CurrentOutliersSelected.Add(z_index);
                    ActiveRatios.CurrentOutliersSelected.Add(1);

                    // this also puts these points into the undo group
                    ActiveRatios.SetOutliers(ActiveRatios.CurrentOutliersSelected);
                }
            }
            while (chauvenet < ChauvenetsThreshold);

            // now restore the data and re-mark it as a group
            for (int i = 0; i < saveIndexes.Count; i++)
                UndoPoints();

            ActiveRatios.CurrentOutliersSelected = saveIndexes;
            ActiveRatios.CurrentOutliersSelected.Add(saveIndexes.Count);
            ActiveRatios.SetOutliers(ActiveRatios.CurrentOutliersSelected);

            ResetSynchButton();

            Refresh();
        }

        private void frmRatioGraph_Load(object sender, EventArgs e)
        {
            resetControlsOnForm();
        }

        private void resetControlsOnForm(){

            chkboxAutoCalc.Checked = ActiveRatios.AutoCalculate;
            numberBinsUpDown.Value = ActiveRatios.HistogramBinCount;

            // set radio buttons for discards
            if (ActiveRatios.HandleDiscardsFlag == (int)RawRatio.HandleDiscards.Include)
                rbIncludeDiscards.Checked = true;
            if (ActiveRatios.HandleDiscardsFlag == (int)RawRatio.HandleDiscards.Ignore)
                rbIgnoreDiscards.Checked = true;
            if (ActiveRatios.HandleDiscardsFlag == (int)RawRatio.HandleDiscards.Hide)
                rbHideDiscards.Checked = true;
            // set radio buttons for outliers - click to get enabled/not action
            if (ActiveRatios.HandleOutliersFlag == (int)RawRatio.HandleOutliers.Manual)
            {
                rbManualOutliers.Checked = true;
                rb2SigmaOutliers.Enabled = true;
                rbChauvenet.Enabled = true;
                btnUndo.Enabled = true;
                btnUndoGroup.Enabled = true;
            }
            if (ActiveRatios.HandleOutliersFlag == (int)RawRatio.HandleOutliers.TwoSigma)
            {
                rb2SigmaOutliers.Checked = true;
                rb2SigmaOutliers.Enabled = false;
                rbChauvenet.Enabled = false;
                btnUndo.Enabled = false;
                btnUndoGroup.Enabled = false;
            }
            if (ActiveRatios.HandleOutliersFlag == (int)RawRatio.HandleOutliers.Chauvenet)
            {
                rbChauvenet.Checked = true;
                rb2SigmaOutliers.Enabled = false;
                rbChauvenet.Enabled = false;
                btnUndo.Enabled = false;
                btnUndoGroup.Enabled = false;
            }

            if (ActiveRatios.AmSynchronized
                &&
                ActiveRatios.HandleOutliersFlag != (int)RawRatio.HandleOutliers.Manual)
            {
                lblCAUTION.Visible = true;
            }
            else
            {
                lblCAUTION.Visible = false;
            }
        }

        private void rbChauvenet_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(rbChauvenet, "using threshold of " + ChauvenetsThreshold);
            toolTip1.Active = true;
        }

        private void menuItemToggleBlockSort_Click(object sender, EventArgs e)
        {
            ActiveRatios.ToggleBlockSort(_AmBlockSorted);
            _AmBlockSorted = !_AmBlockSorted;

            ResetSynchButton();
            rbIncludeDiscards.Checked = true;
            rbManualOutliers.Checked = true;
            rb2SigmaOutliers.Enabled = true;
            rbChauvenet.Enabled = true;

            ActiveRatios.CalcStats();
            Refresh();
        }

        private void menuItemToggleBlockMeans_Click(object sender, EventArgs e)
        {
            _ShowBlockMeans = !_ShowBlockMeans;
            Refresh();
        }

    }
}
