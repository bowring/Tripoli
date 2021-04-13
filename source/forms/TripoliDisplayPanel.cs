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
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using System.Reflection;

using System.Diagnostics;

using Excel = Microsoft.Office.Interop.Excel;

namespace Tripoli
{
	/// <summary>
	/// Wrapper class to customize display panels
	/// </summary>
	public class TripoliDisplayPanel : System.Windows.Forms.Panel
	{
		//  constants
		private int CONTROLS_TOP_TOP_Y = 30;
		/// <summary>
		/// 
		/// </summary>
        public int PANEL_BOT_OFFSET = 125;
		/// <summary>
		/// 
		/// </summary>
        public int PANEL_RIGHT_OFFSET = 30;//105;
		private int PANEL_TOP_MARGIN = 20;

		// Fields
		/// <summary>
		/// The ChooseItem button allows user to select displayed row for graphing.
		/// </summary>
        public CheckBox [] ChooseItem;
		
        /// <summary>
        /// The OpenFile button allows the user to open the source file of the data in a row.
        /// </summary>
        public Button [] OpenFile;

		// Fields for Secondary Choices such as collectors or ratios
		/// <summary>
		/// 
		/// </summary>
        public CheckBox [] ChooseDetail;

		// used for tiling
		GraphPageDisplay allGraphs = null;

		// used to open excel
		Excel.Application ExcelObj = null;

		/// <summary>
		/// The Synch-to-me button synchronizes all selected graphs and
        /// If the synchronization occurs on the graph itself, this button matches 
        /// the color of the graph's synch button.
		/// </summary>
		public Button [] SynchRatios;
		private Button [] GraphRatios;
		private Button [] RefreshRatios;

		// collection of graphs
		frmRatioGraph [] ratioGraphs;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.ComponentModel.IContainer components;

		private TripoliWorkProduct _CurrentWorkProduct = null;
		
		/// <summary>
		/// 
		/// </summary>
		public TripoliDisplayPanel(int parentWidth, int parentHeight, Point location) : base()
		{
            InitializeComponent();

			// control for menu wrapped to more than one line if user has very narrow main
            if (parentWidth < 400) { parentHeight = parentHeight - 20; }
            Location = location; // new Point(PANEL_X, PANEL_Y);
			Size = new Size(parentWidth - PANEL_RIGHT_OFFSET, parentHeight - PANEL_BOT_OFFSET - Location.Y);
			BackColor = Color.Bisque;
			AutoScroll = true;
			AutoScrollMargin = new Size(5, 5);
			BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			Visible = true;
            Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right); //(AnchorStyles)15;

			CurrentWorkProduct = new TripoliWorkProduct();
		}


		#region Properties
		
		/// <summary>
		/// Local reference to CurrentWorkProduct
		/// </summary>
		public TripoliWorkProduct CurrentWorkProduct
		{
			get
			{
				return _CurrentWorkProduct;
			}
			set
			{
				_CurrentWorkProduct = value;
			}
		}

		#endregion Properties

		/// <summary>
		/// 
		/// </summary>
		/// <param name="myCurrentGainsFolder"></param>
		public void DisplayHistoryFiles(TripoliWorkProduct myCurrentGainsFolder)
		{
			this.CurrentWorkProduct = myCurrentGainsFolder;

			this.Controls.Clear();

			// show list of files in folder that correspond to gains
			ChooseItem = new CheckBox[CurrentWorkProduct.Count];

			// set up buttons to open excel files
			OpenFile = new Button[CurrentWorkProduct.Count];

			// set up refresh file buttons for standards
			RefreshRatios = new Button[CurrentWorkProduct.Count];

			// sort ourself for init date
			CurrentWorkProduct.Sort(new TripoliDetailFile.DateOrderClass());

			int ShowIndex = 0;

			for (int index = 0; 
				index < (CurrentWorkProduct.Count); index ++)
			{
				// display a checkbox for each gains file
				ChooseItem[index] = new CheckBox();
				ChooseItem[index].BackColor = Color.LightYellow;
				ChooseItem[index].CheckAlign = ContentAlignment.MiddleRight;
				ChooseItem[index].Font = new Font("Courier New", 10);
				ChooseItem[index].Location = 
					new System.Drawing.Point(10, PANEL_TOP_MARGIN + ShowIndex * CONTROLS_TOP_TOP_Y);
				ChooseItem[index].Size = new System.Drawing.Size(600, 25);
				ChooseItem[index].Text = 
					((TripoliDetailFile)CurrentWorkProduct[index]).FileName.PadRight(45)
					+ ((TripoliDetailFile)CurrentWorkProduct[index])
					.InitDate.ToString("g");
				ChooseItem[index].Name = index.ToString();
				ChooseItem[index].Checked = 
					((TripoliDetailFile)CurrentWorkProduct[index]).IsActive;

				ChooseItem[index].CheckedChanged += 
					new System.EventHandler(this.chkChoose_CheckedChanged);

				// display a open file button for each file
				OpenFile[index] = new Button();
				
				// try to display image and default to open otherwise
				try
				{
					OpenFile[index].Location = new System.Drawing.Point(
						612, PANEL_TOP_MARGIN + ShowIndex * CONTROLS_TOP_TOP_Y - 1);
					if (CurrentWorkProduct.GetType().Equals(typeof(TripoliHistoryFolderGains)))
					{
						OpenFile[index].Image = Image.FromFile(
							Application.StartupPath + @"\images\Excel_Ico.jpg");
                        toolTip1.SetToolTip(OpenFile[index], "Click to open underlying Excel file.");
                    }
					else if (CurrentWorkProduct.GetType().Equals(typeof(TripoliHistoryFolderStandards)))
					{
						OpenFile[index].BackgroundImage = Image.FromFile(
                            Application.StartupPath + @"\images\Tripoli2009.bmp");//Tripoli2.jpg");
                        OpenFile[index].BackgroundImageLayout = ImageLayout.Zoom;
                        toolTip1.SetToolTip(OpenFile[index], "Click to open underlying Tripoli file.");
					}
					OpenFile[index].ImageAlign = ContentAlignment.TopLeft;
					OpenFile[index].Size = new Size(27, 27);
				}
				catch
				{
					OpenFile[index].Location = new System.Drawing.Point(612, PANEL_TOP_MARGIN + ShowIndex * CONTROLS_TOP_TOP_Y + 1);
					OpenFile[index].Width = 50;
					OpenFile[index].Font = new Font("ArialBold", 10, FontStyle.Bold);
					OpenFile[index].Text = "Open";
				}

				OpenFile[index].Cursor = Cursors.Hand;
				OpenFile[index].Name = index.ToString();
				OpenFile[index].BackColor = Color.LightYellow;
				OpenFile[index].Click += new System.EventHandler(this.btnOpenFile_Click);

				// display a refresh button
				RefreshRatios[index] = new Button();
				RefreshRatios[index].Cursor = Cursors.Hand;
				RefreshRatios[index].Location = new System.Drawing.Point(642, PANEL_TOP_MARGIN + ShowIndex * CONTROLS_TOP_TOP_Y - 1);
				RefreshRatios[index].Size = new Size(27, 27);
				RefreshRatios[index].Name = index.ToString();
				RefreshRatios[index].BackColor = Color.Cornsilk;
				RefreshRatios[index].Click += new System.EventHandler(this.btnRefreshCollectors_Click);
				RefreshRatios[index].Visible = ChooseItem[index].Checked;
				RefreshRatios[index].Font = new Font("Wingdings", 10 ,FontStyle.Bold);
				RefreshRatios[index].Text = "a";
                toolTip1.SetToolTip(RefreshRatios[index], "Click to refresh if you have changed underlying Tripoli file.");

				
				this.Controls.Add(ChooseItem[index]);
				this.Controls.Add(OpenFile[index]);
				if (CurrentWorkProduct.GetType().Equals(typeof(TripoliHistoryFolderStandards)))
				{
					this.Controls.Add(RefreshRatios[index]);
				}
				
				ShowIndex ++;
			}

			RefreshOpenCollectorGraphs();
		}

		private void chkChoose_CheckedChanged(object sender, System.EventArgs e)
		{
			// toggle checked state of TripoliDetailFile object
			int chkboxIndex = Convert.ToInt32(((CheckBox)sender).Name);
			((TripoliDetailFile)CurrentWorkProduct[chkboxIndex]).IsActive 
				= ((CheckBox)sender).Checked;

			RefreshRatios[chkboxIndex].Visible = ((CheckBox)sender).Checked;

			// oct 2005 now go through each collector and change status of member file
			CurrentWorkProduct.setFileIsActive(chkboxIndex, ((CheckBox)sender).Checked);

			RefreshOpenCollectorGraphs();
		}

		private void btnRefreshCollectors_Click(object sender, System.EventArgs e)
		{
			// only used for standards histories
			int buttonIndex = Convert.ToInt32(((Button)sender).Name);

			// dec 2005 cause the data to be refeshed from the trip file
				((TripoliHistoryFolderStandards)CurrentWorkProduct)
					.UpdateHistoryFile(buttonIndex);
				
			RefreshOpenCollectorGraphs();
		}

		public void RefreshOpenCollectorGraphs()
		{
			// july 2005
			// refresh all open graphs to show change
			
			try 
			{
				for (int item = 0; item < ratioGraphs.Length; item ++)
				{
					RefreshOpenCollectorGraph(item);
				}
			}
			catch{}
		}

		public void RefreshOpenCollectorGraph(int item)
		{
			if ((ratioGraphs[item] != null)
				&&
				((frmRatioGraph)ratioGraphs[item]).Created)
			{
				((frmRatioGraph)ratioGraphs[item]).ActiveRatios = 
					(RawRatio)CurrentWorkProduct.GainsHistory(item);
				((frmRatioGraph)ratioGraphs[item]).ActiveRatios.CalcStats();
				((frmRatioGraph)ratioGraphs[item]).Refresh();
			}
		}

		private void btnOpenFile_Click(object sender, System.EventArgs e)
		{
			FileInfo fi = new FileInfo(
				CurrentWorkProduct.FolderInfo.FullName
				+ @"\"
				+ ((TripoliDetailFile)CurrentWorkProduct[Convert.ToInt32(((Button)sender).Name)]).FileName);

			if (CurrentWorkProduct.GetType().Equals(typeof(TripoliHistoryFolderGains)))
			{
				// open in excel
				DisposeExcel();
				ExcelObj = new Excel.ApplicationClass();
				ExcelObj.UserControl = true;
				ExcelObj.Visible = true;
				ExcelObj.WindowState = Excel.XlWindowState.xlNormal;

				ExcelObj.Workbooks.Open(
					fi.FullName, false, true, 5,
					"", "", true, Excel.XlPlatform.xlWindows, "\t", true, false,
					0, true, Missing.Value, Missing.Value);
			}
			else if (CurrentWorkProduct.GetType().Equals(typeof(TripoliHistoryFolderStandards)))
			{
                Process Tripoli = new Process();
                Tripoli.StartInfo.FileName = fi.FullName; //"Tripoli.exe";
                Tripoli.StartInfo.Verb = "Open";//   .Arguments = fi.FullName;
                //Tripoli.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                Tripoli.Start();
			}
		}

		private void DisposeExcel()
		{
			if (ExcelObj != null)
			{
				ExcelObj.Quit();
					
				System.Runtime.InteropServices.Marshal.ReleaseComObject (ExcelObj);
				ExcelObj = null;
				GC.Collect();
				GC.WaitForPendingFinalizers();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="myCurrentGainsFolder"></param>
		public void DisplayGainsCollectors(TripoliWorkProduct myCurrentGainsFolder)
		{
			if (myCurrentGainsFolder == null)
				return;
			
			this.CurrentWorkProduct = myCurrentGainsFolder;

			this.Controls.Clear();

			// show list of collectors from first members of AllCollectors arrayList members
			ChooseDetail = 
				new CheckBox[CurrentWorkProduct.AllRatioHistories.Count];
			SynchRatios = 
				new Button[CurrentWorkProduct.AllRatioHistories.Count];
			GraphRatios = 
				new Button[CurrentWorkProduct.AllRatioHistories.Count];

			int ShowIndex = 0;

			for (int index = 0; 
				index < CurrentWorkProduct.AllRatioHistories.Count; 
				index ++)
			{
				// display a checkbox for each gains file
				// using the first rawraio object to store isactive, synch, etc.
				ChooseDetail[index] = new CheckBox();
				ChooseDetail[index].BackColor = Color.Cornsilk;
				ChooseDetail[index].CheckAlign = ContentAlignment.MiddleRight;

				ChooseDetail[index].Location = 
					new System.Drawing.Point(25, PANEL_TOP_MARGIN + ShowIndex * CONTROLS_TOP_TOP_Y);
				ChooseDetail[index].Size = new System.Drawing.Size(250, 25);
				ChooseDetail[index].Text = 
					((TripoliDetailHistory)
						CurrentWorkProduct.AllRatioHistories.GetByIndex(index)).DetailHistory.Name;
				// Dec 2005 long names appear in some samples so change font for them
				if (ChooseDetail[index].Text.Trim().Length < 28)
					ChooseDetail[index].Font = new Font("Courier New", 10);
				else
					ChooseDetail[index].Font = new Font("Courier New", 8);
				
				ChooseDetail[index].Name = index.ToString();
				ChooseDetail[index].Checked = 
					((TripoliDetailHistory)
						CurrentWorkProduct.AllRatioHistories.GetByIndex(index)).DetailHistory.IsActive;

				ChooseDetail[index].CheckedChanged += 
					new System.EventHandler(this.chkChoose_CollectorCheckedChanged);
				
				// display a master synch button for each ratio
				SynchRatios[index] = new Button();
				SynchRatios[index].Cursor = Cursors.Hand;
				SynchRatios[index].Location = 
					new System.Drawing.Point(285, PANEL_TOP_MARGIN + ShowIndex * CONTROLS_TOP_TOP_Y + 1);
				SynchRatios[index].Width = 80;
				SynchRatios[index].Text = "Synch to me";
				SynchRatios[index].Name = index.ToString();
				if (((TripoliDetailHistory)
						CurrentWorkProduct.AllRatioHistories.GetByIndex(index)).DetailHistory.AmSynchronizer)
					SynchRatios[index].BackColor = Color.Red;
				else
					SynchRatios[index].BackColor = Color.Cornsilk;
				SynchRatios[index].Enabled = 
					ChooseDetail[index].Checked = 
						((TripoliDetailHistory)
							CurrentWorkProduct.AllRatioHistories.GetByIndex(index)).DetailHistory.IsActive;
				SynchRatios[index].Click += new System.EventHandler(this.btnSynchCollectors_Click);

				// display a graph button for each ratio
				GraphRatios[index] = new Button();
				GraphRatios[index].Cursor = Cursors.Hand;
				GraphRatios[index].Location = new System.Drawing.Point(375, PANEL_TOP_MARGIN + ShowIndex * CONTROLS_TOP_TOP_Y + 1);
				GraphRatios[index].Width = 120;
				GraphRatios[index].Text = "GRAPH History";
				GraphRatios[index].Font = new Font("ArialBold", 10, FontStyle.Bold);
				GraphRatios[index].Name = index.ToString();
				GraphRatios[index].BackColor = Color.Cornsilk;
				GraphRatios[index].Click += new System.EventHandler(this.btnGraphCollectors_Click);
		
				this.Controls.Add(ChooseDetail[index]);
				this.Controls.Add(SynchRatios[index]);
				this.Controls.Add(GraphRatios[index]);

				ShowIndex ++;
			}

			if (ratioGraphs == null)
			{
				// this is the first time 
				ratioGraphs = new frmHistoryGraph[CurrentWorkProduct.AllRatioHistories.Count];
			}
		}

		private void chkChoose_CollectorCheckedChanged(object sender, System.EventArgs e)
		{
			int index = Convert.ToInt32(((CheckBox)sender).Name);
			// disable Synch button  on uncheck
			bool tempState = SynchRatios[index].Enabled 
				= ((CheckBox)sender).Checked;
			if (!tempState)
			{
				SynchRatios[index].BackColor = Color.Cornsilk;
				((TripoliDetailHistory)
					CurrentWorkProduct
						.AllRatioHistories.GetByIndex(index))
							.DetailHistory.AmSynchronizer = false;
			}
			
			// toggle checked state of RawRatio = collector object
			((TripoliDetailHistory)
				CurrentWorkProduct
					.AllRatioHistories.GetByIndex(index))
						.DetailHistory.IsActive = ((CheckBox)sender).Checked;

			// kill the graph if unchecked
			if ( !((CheckBox)sender).Checked)
			{
				try
				{
					((frmRatioGraph)ratioGraphs[index]).Close();
				}
				catch{}
			}
		}

		private void btnSynchCollectors_Click(object sender, System.EventArgs e)
		{
			// here go through all the other collectors and discard same ratios
			int graphNumber = Convert.ToInt32(((Button)sender).Name);

			ArrayList synchMembers = 
				((TripoliDetailHistory)CurrentWorkProduct
				.AllRatioHistories.GetByIndex(graphNumber)).MemberRatios;
			
			// determine if all details are active 
			bool allDetailsSelected = true;
			for (int detail = 0; detail < SynchRatios.Length; detail ++)
			{
				allDetailsSelected = allDetailsSelected && ChooseDetail[detail].Checked;
			}
			
			if (allDetailsSelected)
			{
				for (int iFile = 0; iFile < synchMembers.Count; iFile ++)
				{
					// set check box on files to same if all are selected
					ChooseItem[iFile].Checked = ((RawRatio)synchMembers[iFile]).IsActive;
				}
			}

			// first recolor the buttons as memory aid and set graph synch flag
			for (int i = 0; i < SynchRatios.Length; i ++)
			{
				if (i == graphNumber)
				{
					SynchRatios[i].BackColor = Color.Red;
					((TripoliDetailHistory)
						CurrentWorkProduct
							.AllRatioHistories.GetByIndex(i)).DetailHistory.AmSynchronizer = true;
					((TripoliDetailHistory)
						CurrentWorkProduct
						.AllRatioHistories.GetByIndex(i)).DetailHistory.AmSynchronized = false;
				}
				else
				{
					SynchRatios[i].BackColor = Color.Cornsilk;
					((TripoliDetailHistory)
						CurrentWorkProduct
						.AllRatioHistories.GetByIndex(i)).DetailHistory.AmSynchronizer = false;
				}
			}
			
			for (int ratio = 0; 
				ratio < CurrentWorkProduct.AllRatioHistories.Count; 
				ratio ++)
			{
				if ((ratio != graphNumber)
					&&
					(ChooseDetail[ratio].Checked))
				{
					DetailHistory tempRR =
						((TripoliDetailHistory)
							CurrentWorkProduct
								.AllRatioHistories.GetByIndex(ratio)).DetailHistory;

					tempRR.SynchDetailHistory(
						((TripoliDetailHistory)
							CurrentWorkProduct
								.AllRatioHistories.GetByIndex(graphNumber)).DetailHistory);

				
					// set the flags to match so the same files are displayed in histories
					ArrayList tempMembers = 
						((TripoliDetailHistory)CurrentWorkProduct
						.AllRatioHistories.GetByIndex(ratio)).MemberRatios;
					for (int iFile = 0; iFile < tempMembers.Count; iFile ++)
					{
						((RawRatio)tempMembers[iFile]).IsActive 
							= ((RawRatio)synchMembers[iFile]).IsActive;
					}
					
				}
			}

			// dec 2005 use this 
			RefreshOpenCollectorGraphs();
		}

		private void btnGraphCollectors_Click(object sender, System.EventArgs e)
		{
			int graphNumber = Convert.ToInt32(((Button)sender).Name);
			// init the graph
			
			if ((ratioGraphs[graphNumber] == null)
				||
				( !((frmRatioGraph)ratioGraphs[graphNumber]).Created))
			{
				// dec 2005 to catch graphs based on duplicate ratio names
				try
				{
					ratioGraphs[graphNumber] = 
						new frmHistoryGraph(CurrentWorkProduct
						.GainsHistory(graphNumber),
						CurrentWorkProduct.SourceFileInfo.Directory.Name
					+ @"\" + CurrentWorkProduct.SourceFileInfo.Name);

					ratioGraphs[graphNumber].caller = this;
					ratioGraphs[graphNumber].myGraphIndex = graphNumber;
					ratioGraphs[graphNumber].Show();
				}
				catch (Exception egraph)
				{
					MessageBox.Show("This graph cannot be displayed - check for duplicate ratio names.   "
						+ egraph.Message );
				}

			}
			else
			{
				((frmRatioGraph)ratioGraphs[graphNumber]).Refresh();
				ratioGraphs[graphNumber].BringToFront();

			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void GraphSelectedCollectors()
		{
			// ********************************
			// here we tile those graphs that are selected
			// we also suppress the legend on the each for readability

			DisposeGraphs();
			
			allGraphs = 
				new GraphPageDisplay(
					CurrentWorkProduct.SourceFileInfo.Directory.Name
					+ @"\" + CurrentWorkProduct.SourceFileInfo.Name);//.DirectoryName);
			
			bool ReadyToShow = false;
			
			for (int graphNumber = ratioGraphs.Length - 1; graphNumber >= 0; graphNumber --)
			{
				if (((TripoliDetailHistory)
					CurrentWorkProduct
					.AllRatioHistories.GetByIndex(graphNumber)).DetailHistory.IsActive)
				{
					ratioGraphs[graphNumber] = 
						new frmHistoryGraph(CurrentWorkProduct
							.GainsHistory(graphNumber),
						allGraphs.SampleName);
					ratioGraphs[graphNumber].caller = this;
					ratioGraphs[graphNumber].myGraphIndex = graphNumber;
					ratioGraphs[graphNumber].MdiParent = allGraphs;
					ratioGraphs[graphNumber].Show();
					ReadyToShow = true;
				}
			}
			if (ReadyToShow)
			{
				// tile them horizontally
				allGraphs.Show();
				allGraphs.menuItem6.PerformClick();
			}
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);

		}

		/// <summary>
		/// 
		/// </summary>
		public void DisposeGraphs()
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
	}
}
