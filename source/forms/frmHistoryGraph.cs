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
	/// specialization to handle histories
	/// </summary>
	public class frmHistoryGraph : Tripoli.frmRatioGraph
	{
		//Fields
		bool MOUSE_DOWN_VALUE = false;

		/// <summary>
		/// 
		/// </summary>
		public frmHistoryGraph()
		{
			// 
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ActiveRatios"></param>
		/// <param name="HistoryName"></param>
		public frmHistoryGraph(RawRatio ActiveRatios, string HistoryName)
			:base(ActiveRatios, HistoryName, 0.0) 
		{
			GraphIdentity = "HISTORY";

			btnDiscardOutliers.Visible = true;
			//btnDiscardOutliers.Width = (int)(btnDiscardOutliers.Width * 1.25);

			btnRestore.Text = "Show Selected Files";
			btnRestore.Width = //(int)(btnDiscardOutliers.Width * 1.25);
				btnSynchToMe.Width + btnDiscardOutliers.Width;

			btnUndo.Visible = false;
			btnUndoGroup.Visible = false;

			chkboxAutoCalc.Visible = false;
			numberBinsUpDown.Visible = false;
			pictureBoxHistogram.Visible = false;
			pnlDiscards.Visible = false;
			pnlChooseOutliers.Visible = false;
 
			this.btnDiscardOutliers.Click += new System.EventHandler(this.btnChooseToLoseGroup_Click);
			this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);

			this.Cursor = Cursors.Default;
			toolTip1.SetToolTip(this, 
					"LEFT Click graph to view File name and date."
					+ "\r\n" + "\r\n"
					+ "RIGHT click to mark File.");

			this.MouseMove 
				+= new System.Windows.Forms.MouseEventHandler(this.frmHistoryGraph_MouseMove);
			this.MouseDown 
				+= new System.Windows.Forms.MouseEventHandler(this.frmHistoryGraph_MouseDown);
			this.MouseUp
				+= new System.Windows.Forms.MouseEventHandler(this.frmHistoryGraph_MouseUp);
			this.mnuPrintPortrait.Click 
				+= new System.EventHandler(this.mnuPrintPortrait_Click);
			this.mnuPrintLandscape.Click 
				+= new System.EventHandler(this.mnuPrintLandscape_Click);



		}

		#region Properties

	
		#endregion Properties

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint( PaintEventArgs e )
		{
			Graphics dc = e.Graphics;

			OnPaintDirective(dc, this.Width, 0);
		}

		private void OnPaintDirective(Graphics dc, int Width, int Height)
		{
			RIGHT_MARGIN = 20;
            //if (Width < FULL_DETAIL_WIDTH)
            //{
            //    RIGHT_MARGIN = 20;
            //}
            //else
            //{
            //    RIGHT_MARGIN = 125;
            //}

			ORIGINAL_DATA_LABEL = "     ALL";

			DisplayGraph(dc, Width, Height);
			DisplayData(dc);
		}

		private void DisplayData(Graphics dc)
		{
			//******************* do this last ************************
			// label x axis with tick marks and plot data points
			XtickGap = (float)(GraphWidth / 
				(((float)ActiveRatios.ActiveRatios.Count + 1F) - 
				Math.Floor((ActiveRatios.ActiveRatios.Count + 1F) / 
				ActiveRatios.CyclesPerBlock)));
			
			for (int RealTick = 0; RealTick < ActiveRatios.ActiveRatios.Count; RealTick ++)
			{
				int	tick = RealTick - (int)Math.Floor((double)RealTick / ActiveRatios.CyclesPerBlock);
				
				dc.DrawLine(BlackPen, 
					LEFT_MARGIN + (float)(tick + 1) * XtickGap, TOP_MARGIN + GraphHeight,
					LEFT_MARGIN + (float)(tick + 1) * XtickGap, TOP_MARGIN + GraphHeight + 5);

				// calculate data point
				float ScaledData = 
					(float)((Ymax - Math.Abs((double)ActiveRatios.ActiveRatios[RealTick])) / YRange) * GraphHeight;
				
				// decide if outlier based on sign - these will be here if DiscardOutliers is false
				if ((double)ActiveRatios.ActiveRatios[RealTick] > 0)//ignore 0s here
				{
					if (! DrawMeansOnly)
					{
						dc.FillRectangle(blackBrush, 
							LEFT_MARGIN + (float)(tick + 1) * XtickGap - 0.5F * DataPenSize,
							TOP_MARGIN + ScaledData - 1.0F * DataPenSize, // shift up a little
							2F * DataPenSize, 2F * DataPenSize);
					}
				}
				else if ((double)ActiveRatios.ActiveRatios[RealTick] < 0)//ignore 0s here
				{
					// added for history
					// we know this is the last one in the set AND the only negative value

					dc.DrawLine(BlueDataPen, 
						LEFT_MARGIN + (float)(tick + 1.35 - ActiveRatios.CyclesPerBlock) * XtickGap + 2.5F,// * DataPenSize,
						TOP_MARGIN + ScaledData,// - 1.0F,// * DataPenSize, // shift up a little
						LEFT_MARGIN + (float)(tick + 1.35 - ActiveRatios.CyclesPerBlock) * XtickGap + 2.5F + (float)(ActiveRatios.CyclesPerBlock - 1) * XtickGap,
						TOP_MARGIN + ScaledData);// - 1.0F);
				}

				else //mark the axis since missing value
					dc.DrawLine(RedThickPen, 
						LEFT_MARGIN + (float)(tick + 1) * XtickGap, TOP_MARGIN + GraphHeight - 5,
						LEFT_MARGIN + (float)(tick + 1) * XtickGap, TOP_MARGIN + GraphHeight + 5);

				// decide which cycleblock - ie whether to draw vertical dashed line
				//if (!ActiveRatios.DiscardOutliers && ((RealTick % ActiveRatios.CyclesPerBlock) == 0))
				if ((ActiveRatios.HandleDiscardsFlag != (int)RawRatio.HandleDiscards.Hide)
					&& ((RealTick % ActiveRatios.CyclesPerBlock) == 0))
					{
					// draw vertical line
					if (tick > 0)
					{
						dc.DrawLine(GreyThinLinePen, 
							LEFT_MARGIN + (float)(tick + 0.6) * XtickGap, TOP_MARGIN,
							LEFT_MARGIN + (float)(tick + 0.6) * XtickGap, TOP_MARGIN + GraphHeight);
					}

				}

				// historymode: use same cycleblock logic to color the clicked files
				if ((RealTick % ActiveRatios.CyclesPerBlock) == 0)
				{
					// color in the previous panel if flag is false
					int fileIndex =
                        ((int)(Math.Floor((double)(tick + 1) / (ActiveRatios.CyclesPerBlock - 1))));

					if (! ((bool)((ArrayList)((DetailHistory)ActiveRatios).GainsFilesInfo[fileIndex])[0]))
					{
						dc.FillRectangle(HatchBlueBrush,
							LEFT_MARGIN + (float)(tick + 0.6) * XtickGap, 
							TOP_MARGIN,
							(float)(ActiveRatios.CyclesPerBlock - 1) * XtickGap,
							GraphHeight);
					}

				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="landscape"></param>
		public new void printPrinter(bool landscape)
		{
			try 
			{
				// Assumes the default printer.
				PrintDocument pd = new PrintDocument();
				if (landscape)
					pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPageLandscape);
				else
					pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPagePortrait);
				pd.DocumentName = "Tripoli Graph History";
				pd.OriginAtMargins = true;

				pageSetupDialog1.Document = pd;
				pageSetupDialog1.PageSettings.Landscape = landscape;
				pageSetupDialog1.PageSettings.Margins = new Margins(50,50,50,50);

				printDialog1.Document = pd;
				if (printDialog1.ShowDialog() == DialogResult.OK)
					pd.Print();
			}  
			catch(Exception ex) 
			{
				MessageBox.Show("An error occurred while printing", ex.ToString());
			}
		}

		#endregion Methods
	
		#region Delegates
		// NOTE: Be sure to add a line to check type in frmRatioGraph for each delegate overLoad here


		private void btnRestore_Click(object sender, System.EventArgs e)
		{
			// the idea here is to poll the checkboxes for the files as the restore point
			ArrayList synchMembers = 
				((TripoliDetailHistory)
					((TripoliDisplayPanel)caller).CurrentWorkProduct
				.AllRatioHistories.GetByIndex(myGraphIndex)).MemberRatios;
			
			for (int detail = 0; detail < ((TripoliDisplayPanel)caller).SynchRatios.Length; detail ++)
			{
				if (((TripoliDisplayPanel)caller).ChooseDetail[detail].Checked)
				{
					for (int iFile = 0; iFile < synchMembers.Count; iFile ++)
					{
						((RawRatio)synchMembers[iFile]).IsActive 
							= ((TripoliDisplayPanel)caller).ChooseItem[iFile].Checked;
					}
				}
			}
			ResetSynchButton();
			((TripoliDisplayPanel)caller).RefreshOpenCollectorGraphs();

		}


		/// <summary>
		/// Re-use the discardOutliers group to lose the clicked files
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnChooseToLoseGroup_Click(object sender, System.EventArgs e)
		{
			// dec 2005
			// we make active the chosen files for this graph this means
			// that any that have the first element of gainsfilesinfo as true are hidden

			for (int i = 0; i < ((DetailHistory)ActiveRatios).GainsFilesInfo.Count; i ++)
			{
				if(! ((bool)((ArrayList)((DetailHistory)ActiveRatios).GainsFilesInfo[i])[0]))
				{
					((RawRatio)((TripoliDetailHistory)
						((TripoliDisplayPanel)caller).CurrentWorkProduct.AllRatioHistories
						.GetByIndex(myGraphIndex))
						.MemberRatios[((int)((ArrayList)((DetailHistory)ActiveRatios).GainsFilesInfo[i])[2])]).IsActive = false;
				}
			}
			
			// test
			try 
			{
				((TripoliDisplayPanel)caller).RefreshOpenCollectorGraph(myGraphIndex);
			}
			catch{}

			ResetSynchButton();

		}


		private void frmHistoryGraph_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (_DisableGraph) 
			{
				this.Cursor = Cursors.Default;
				return;
			}

			// check if in zone for stats to send to reportOfPbMacDat
			if (e.Button == MouseButtons.None)
			{
				doClipboard = false;
				// lets show the cursor hand within the graph box
				if ((e.X > LEFT_MARGIN)
					&&
					(e.X < (LEFT_MARGIN + GraphWidth))
					&&
					(e.Y > TOP_MARGIN)
					&&
					(e.Y < (TOP_MARGIN + GraphHeight))
					)
				{
					this.Cursor = Cursors.Hand;
					// the tooltip will display clicking instructions 
					toolTip1.Active = true;
				}
				else
				{
					this.Cursor = Cursors.Default;
					toolTip1.Active = false;
				}
			}

			// see about constant motion to select
			if (e.Button == MouseButtons.Right)
			{
				doClipboard = false;
				// lets show the cursor hand within the graph box
				if ((e.X > LEFT_MARGIN)
					&&
					(e.X < (LEFT_MARGIN + GraphWidth))
					&&
					(e.Y > TOP_MARGIN)
					&&
					(e.Y < (TOP_MARGIN + GraphHeight))
					)
				{
					if (DoTickTest(e))
					{
						MouseFileSelector(true);
					}
				}
			}
		}

		private void frmHistoryGraph_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (DoTickTest(e))
			{
				HistoryMouseDownResponse(e);
			}
		}

		private void HistoryMouseDownResponse(System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{	// set up selection zone and display file name and date
				try
				{
					pnlInfoZone.Top = e.Y;
					YStart = e.Y;
					if ((this.Width - e.X) < 200)
						pnlInfoZone.Left = e.X - 200;
					else
						pnlInfoZone.Left = e.X;
					pnlInfoZone.Height = 50; 
					pnlInfoZone.Width = 200;
					pnlInfoZone.Visible = true;
					int fileIndex =
                        ((int)(Math.Floor((double)(TickValueStart + 1) / (ActiveRatios.CyclesPerBlock - 1))));
					txtGainsFile.Text = 
						((string)((ArrayList)((DetailHistory)ActiveRatios).GainsFilesInfo[fileIndex])[1]);
					txtGainsFile.Width = pnlInfoZone.Width - 10;
				}
				catch{}

			}
			if (e.Button == MouseButtons.Right)
			{	// select file(s) for viewing or removing
				MOUSE_DOWN_VALUE = MouseFileSelector(false);
			}
			else
			{
				return;
			}
		}

		private bool MouseFileSelector(bool doChoose)
		{
			bool retval = false;
			try
			{
				int fileIndex =
                    ((int)(Math.Floor((double)(TickValueStart + 1) / (ActiveRatios.CyclesPerBlock - 1))));
				// marked as clicked by mouse by toggle value
				((ArrayList)((DetailHistory)ActiveRatios).GainsFilesInfo[fileIndex])[0] = 
					(doChoose ? 
						MOUSE_DOWN_VALUE
						:
						(! ((bool)((ArrayList)((DetailHistory)ActiveRatios).GainsFilesInfo[fileIndex])[0])));
				if (! doChoose)this.Refresh();

				retval = (bool)((ArrayList)((DetailHistory)ActiveRatios).GainsFilesInfo[fileIndex])[0];
			}
			catch(Exception e){Console.WriteLine("MOUSE FILE SELECTOR ERROR  "+ e.Message);}

			return retval;
		}


		private void frmHistoryGraph_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			// always zap info panel
			pnlInfoZone.Visible = false;

			if (DoTickTest(e))
			{
				this.Refresh();
			}
		}

		// Specifies what happens when the PrintPage event is raised.

		private void pd_PrintPageLandscape(object sender, PrintPageEventArgs ev) 
		{
			pd_PrintPage(ev, 850, 450, 25);
		}

		private void pd_PrintPagePortrait(object sender, PrintPageEventArgs ev) 
		{
			pd_PrintPage(ev, 650, 450, 50);
		}

		private void pd_PrintPage(PrintPageEventArgs ev, int width, int height, int rows) 
		{   
			// Draw a picture.
			Graphics dc = ev.Graphics;

			OnPaintDirective(dc, width, height);

			dc.DrawString(
				"Produced from Tripoli History File: "
				+ ((TripoliDisplayPanel)caller)
					.CurrentWorkProduct.SourceFileInfo.Name
				+ "\r\n"
				+ "   on date: " + System.DateTime.Now.ToLongDateString()
				+ "\r\n"
				+ "History Folder: "
				+ "\r\n   "
				+ ((TripoliDisplayPanel)caller)
					.CurrentWorkProduct.SourceFileInfo.DirectoryName
				+ "\r\n"
				+ "* * * List of Files as displayed left to right in graph * * *", 
				CourierFont9, blackBrush,
				new PointF(5F, TOP_MARGIN + GraphHeight + 25F));

			ArrayList GainsFilesInfo = 
				((TripoliDetailHistory)
				((TripoliDisplayPanel)caller).CurrentWorkProduct
				.AllRatioHistories.GetByIndex(myGraphIndex)).DetailHistory.GainsFilesInfo;			
			
			int count = 0;
			for (int detail = 0; detail < GainsFilesInfo.Count; detail ++)
			{
				if (((bool)((ArrayList)((DetailHistory)ActiveRatios).GainsFilesInfo[detail])[0]))
				{
					TripoliDetailFile tempGainsFile = 
						(TripoliDetailFile)
							((TripoliDisplayPanel)caller).CurrentWorkProduct
								[((int)((ArrayList)((DetailHistory)ActiveRatios).GainsFilesInfo[detail])[2])];

					count ++;
                    float lMargin = (float)(Math.Floor((double)(count - 1) / rows)) * 250F + 5F;
					float tMargin = (float)((count - 1) % rows) * 12F;
					dc.DrawString(
						count.ToString("000") + "  "
						+ tempGainsFile.FileName,// + "{ " + tempGainsFile.InitDate.ToString("g") + " }", 
						CourierFont9, blackBrush,
						new PointF(lMargin, TOP_MARGIN + GraphHeight + 100F + tMargin));
				}
			}
			// Indicate that this is the last page to print.
			ev.HasMorePages = false;
		}

		private void mnuPrintPortrait_Click(object sender, System.EventArgs e)
		{
			printPrinter(false);
		}

		private void mnuPrintLandscape_Click(object sender, System.EventArgs e)
		{
			printPrinter(true);
		}
		#endregion Delegates
	}
}
