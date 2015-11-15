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
using System.Drawing.Printing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Tripoli
{
	/// <summary>
	/// Summary description for GraphPageDisplay.
	/// </summary>
	public class GraphPageDisplay : System.Windows.Forms.Form
	{

		// Fields
		MdiLayout LayoutStyle = MdiLayout.TileHorizontal;
		public string SampleName = "";


		private System.Windows.Forms.MainMenu mainMenu1;
		public System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem7;
		public System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.PrintDialog printDialog1;
		private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
		private System.Windows.Forms.MenuItem mnuGraphPrint;
		private System.Windows.Forms.MenuItem mnuGraphMain;
		private System.Windows.Forms.MenuItem mnuLayoutMain;
		private System.Windows.Forms.MenuItem mnuClipboardMain;
		private System.Windows.Forms.MenuItem mnuPrintAllMain;
		private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem mnuPrintAllPortrait;
        private IContainer components;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sampleName"></param>
		public GraphPageDisplay(string sampleName)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			SampleName = sampleName;
            RefreshTitle();
		}

        public void RefreshTitle()
        {
            this.Text = "Tripoli - Selected Graphs from: " + SampleName;
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphPageDisplay));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.mnuGraphMain = new System.Windows.Forms.MenuItem();
            this.mnuLayoutMain = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.mnuClipboardMain = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.mnuPrintAllMain = new System.Windows.Forms.MenuItem();
            this.mnuGraphPrint = new System.Windows.Forms.MenuItem();
            this.mnuPrintAllPortrait = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuGraphMain,
            this.mnuLayoutMain,
            this.mnuClipboardMain,
            this.mnuPrintAllMain});
            // 
            // mnuGraphMain
            // 
            this.mnuGraphMain.Index = 0;
            this.mnuGraphMain.MdiList = true;
            this.mnuGraphMain.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
            this.mnuGraphMain.Text = "Graph";
            // 
            // mnuLayoutMain
            // 
            this.mnuLayoutMain.Index = 1;
            this.mnuLayoutMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem6,
            this.menuItem1,
            this.menuItem7,
            this.menuItem4});
            this.mnuLayoutMain.MergeOrder = 1;
            this.mnuLayoutMain.Text = "Layout";
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 0;
            this.menuItem6.Text = "Tile Horizontal";
            this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.Text = "Tile Vertical";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 2;
            this.menuItem7.Text = "Cascade";
            this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 3;
            this.menuItem4.Text = "Iconify";
            this.menuItem4.Visible = false;
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // mnuClipboardMain
            // 
            this.mnuClipboardMain.Index = 2;
            this.mnuClipboardMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem8});
            this.mnuClipboardMain.MergeOrder = 2;
            this.mnuClipboardMain.Text = "Clipboard";
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 0;
            this.menuItem8.Text = "Copy All Ratio Means with PctStdErr";
            this.menuItem8.Click += new System.EventHandler(this.menuItem8_Click);
            // 
            // mnuPrintAllMain
            // 
            this.mnuPrintAllMain.Index = 3;
            this.mnuPrintAllMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuGraphPrint,
            this.mnuPrintAllPortrait,
            this.menuItem3});
            this.mnuPrintAllMain.Text = "PrintAll";
            // 
            // mnuGraphPrint
            // 
            this.mnuGraphPrint.Index = 0;
            this.mnuGraphPrint.Text = "Print Screen ...";
            this.mnuGraphPrint.Click += new System.EventHandler(this.mnuGraphPrint_Click);
            // 
            // mnuPrintAllPortrait
            // 
            this.mnuPrintAllPortrait.Index = 1;
            this.mnuPrintAllPortrait.Text = "Print Each Portrait";
            this.mnuPrintAllPortrait.Click += new System.EventHandler(this.mnuPrintAllPortrait_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 2;
            this.menuItem3.Text = "Print Each Landscape";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // printDialog1
            // 
            this.printDialog1.ShowHelp = true;
            // 
            // GraphPageDisplay
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(612, 436);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Menu = this.mainMenu1;
            this.Name = "GraphPageDisplay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.SizeChanged += new System.EventHandler(this.GraphPageDisplay_SizeChanged);
            this.ResumeLayout(false);

		}
		#endregion

		
		private void menuItem6_Click(object sender, System.EventArgs e)
		{
			LayoutStyle = MdiLayout.TileHorizontal;
			this.LayoutMdi(LayoutStyle);
		}

		private void menuItem7_Click(object sender, System.EventArgs e)
		{
			LayoutStyle = MdiLayout.Cascade;
			this.LayoutMdi(LayoutStyle);
		}

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			LayoutStyle = MdiLayout.TileVertical;
			this.LayoutMdi(LayoutStyle);
		}

		private void GraphPageDisplay_SizeChanged(object sender, System.EventArgs e)
		{
			this.LayoutMdi(LayoutStyle);
		}


		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			// first need to make them icons
			
			
			LayoutStyle = MdiLayout.ArrangeIcons;
			this.LayoutMdi(LayoutStyle);
		}

		private void menuItem8_Click(object sender, System.EventArgs e)
		{
			// make list of all percent std errors on child forms and put on reportOfPbMacDat
			string retval = "";
			//for (int f = 0; f < getMDIChildren().Length; f ++)
		//	foreach (frmRatioGraph frm in getMDIChildren())
		//	{
		//		retval += frm.GetPctStdErrAsText();
		//	}
			
			for (int i = getMDIChildren().Length - 1; i >= 0; i --)
			{
				retval += ((frmRatioGraph)getMDIChildren()[i]).GetPctStdErrAsText();
			}
			frmRatioGraph.SendTextToClipboard(retval);

		}

		private void mnuGraphPrint_Click(object sender, System.EventArgs e)
		{
			// Assumes default printer.
			
			PrintDocument pd = new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);

			pd.DocumentName = "Tripoli Graph Display";
			pd.OriginAtMargins = true;
				
			pageSetupDialog1.Document = pd;
			pageSetupDialog1.PageSettings.Landscape = true;
			pageSetupDialog1.PageSettings.Margins = new Margins(55,55,55,55);

			printDialog1.Document = pd;
			if (printDialog1.ShowDialog() == DialogResult.OK)
			{
				// setup form to print
				this.Width = 1000;
				this.Height = 800;
				this.WindowState = FormWindowState.Maximized;// this caused a retiling.Normal;
				this.Refresh();
				this.Focus();

				try 
				{
					CaptureScreen();
				
					pd.Print();
				}  
				catch(Exception ex) 
				{
					MessageBox.Show("An error occurred while printing", ex.Message);
				}
			}

			this.WindowState = FormWindowState.Maximized;

		}
		
		// used to print screen (of this form)
		// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cscon/html/vclrfCodePrintPreviewingFormVisualC.asp
		[System.Runtime.InteropServices.DllImport("gdi32.dll")]
		public static extern long BitBlt (IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, 
			IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
		
		private Bitmap memoryImage;
		
		private void CaptureScreen()
		{
			Graphics mygraphics = this.CreateGraphics();
			Size s = this.Size;
			memoryImage = new Bitmap(s.Width, s.Height, mygraphics);
			Graphics memoryGraphics = Graphics.FromImage(memoryImage);
			IntPtr dc1 = mygraphics.GetHdc();
			IntPtr dc2 = memoryGraphics.GetHdc();
			BitBlt(dc2, 0, 0, this.ClientRectangle.Width, 
				this.ClientRectangle.Height, dc1, 0, 0, 
				13369376); //http://www.codeproject.com/vb/net/Bitblt_wrapper_class.asp
			mygraphics.ReleaseHdc(dc1);
			memoryGraphics.ReleaseHdc(dc2);
		}
		
		private void pd_PrintPage(System.Object sender, PrintPageEventArgs e)
		{
			e.Graphics.DrawImage(memoryImage, 0, 0);
			
			// Indicate that this is the last page to print.
			e.HasMorePages = false;
		}

		private void mnuPrintAllPortrait_Click(object sender, System.EventArgs e)
		{
			foreach (frmRatioGraph frm in getMDIChildren())
			{
				frm.printPrinter(false);
			}
			//for (int f = getMDIChildren().Length - 1; f >= 0; f --)
			//{
			//	((frmRatioGraph)getMDIChildren()[f]).printPrinter(false);
			//}
		}

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			foreach (frmRatioGraph frm in getMDIChildren())
			{
				frm.printPrinter(true);
			}
			//for (int f = getMDIChildren().Length - 1; f >= 0; f --)
			//{
			//	((frmRatioGraph)getMDIChildren()[f]).printPrinter(true);
			//}
		}

		// nov 2006 keep order of mdi children
		private Form[] getMDIChildren()
		{
			//Form[] retVal = new Form [MdiChildren.Length];
			//for (int f = 0; f < MdiChildren.Length; f ++)
			//{
			//	retval[f] = MdiChildren[f];
			//}
			Array.Sort(this.MdiChildren);
			// try to restore tiling order
			this.MdiChildren[0].Select();

			return this.MdiChildren;
		}

		



	}
}
