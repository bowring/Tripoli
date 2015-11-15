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
using System.Collections;
using System.Reflection;
using System.IO;

using Excel = Microsoft.Office.Interop.Excel;


namespace Tripoli
{
	/// <summary>
	/// Tripoli work product NOTE: should have common tree with MassLynxDataFile = LATER
	/// </summary>
	public class TripoliExcelFile :MarshalByRefObject
	{
		// Possible constants
		int ROW_OFFSET = 17;	// this is the same as MassLynx
		int xlNormal = -4143;
		
		// Fields
		Excel.Application ExcelObj = null;
		Excel.Workbooks TripoliBooks = null;
		Excel._Workbook TripoliWorkbook = null;

		string _TripoliFileName = null;


		public TripoliExcelFile(string TripoliFileName)
		{
			_TripoliFileName = TripoliFileName;
		}

		#region Properties

		public string TripoliFileName
		{
			get 
			{
				return _TripoliFileName;
			}
			set
			{
				_TripoliFileName = value;
			}
		}

		#endregion Properties

		public bool SaveTripoliExcelFile(FileInfo MassLynxFileInfo, ArrayList RawRatios)
		{
			// first save off a new TripoliWorkFile

			if (OpenExcelFile(MassLynxFileInfo.FullName))
			{
				try 
				{
					TripoliWorkbook.SaveAs(_TripoliFileName, xlNormal, "", "", false, false,
						Excel.XlSaveAsAccessMode.xlExclusive,
						true, true,
						Missing.Value, Missing.Value, Missing.Value);

					DisposeExcelApp();
					// now execute a write to save tripoli state
					WriteTripoliWorkFileExcel(RawRatios);
					//DisposeExcelApp();
					return true;
				}
				catch (Exception ee)
				{
					Console.WriteLine(ee.Message);
					DisposeExcelApp();
					return false;
				}
			}
			else
				return false;

		}
		

		public bool OpenExcelFile(string FileName)
		{
			// this belongs in ancestor of both excel classes

			bool retval = false;

			ExcelObj = new Excel.ApplicationClass();
			ExcelObj.UserControl = false;

			TripoliWorkbook = ExcelObj.Workbooks.Open(
				FileName, false, false, 5,
				"", "", true, Excel.XlPlatform.xlWindows, "\t", true, false,
				0, true, Missing.Value, Missing.Value);
			retval = true;

			return retval;

		}
		

		public void WriteTripoliWorkFileExcel(ArrayList RawRatios)
		{
			Excel.Sheets TripoliSheets = null;
			Excel._Worksheet Tsheet1 = null;

			// first open existing file again
			if (OpenExcelFile(_TripoliFileName))
			{
				//ExcelObj.Visible = true;
				// insert a new sheet at the head of the line
				TripoliBooks = ExcelObj.Workbooks;
				TripoliWorkbook = (Excel._Workbook)TripoliBooks.get_Item(1);
				TripoliSheets = TripoliWorkbook.Worksheets;
				
				// see if we already have the sheet
				try 
				{
					Tsheet1 = (Excel._Worksheet)TripoliSheets["TRIPOLI SAVED DATA"];
				}
				catch(Exception eExcel)
				{
					Tsheet1 = (Excel._Worksheet)TripoliSheets.get_Item(1);
					Tsheet1.Select(Missing.Value);
					Tsheet1 = (Excel._Worksheet)TripoliSheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
					Tsheet1.Name = "TRIPOLI SAVED DATA";
				}
				
				WriteTriopliDataToExcel(Tsheet1, RawRatios);

				TripoliWorkbook.Save();

				System.Runtime.InteropServices.Marshal.ReleaseComObject (Tsheet1);
				Tsheet1 = null;
				System.Runtime.InteropServices.Marshal.ReleaseComObject (TripoliSheets);
				TripoliSheets = null;

			}

			DisposeExcelApp();
		}

		public TripoliWorkProduct ReadTripoliWorkFileExcel()
		{
			TripoliWorkProduct retval = null;

			Excel.Sheets TripoliSheets = null;
			Excel._Worksheet Tsheet1 = null;

			// first open existing file again
			if (OpenExcelFile(_TripoliFileName))
			{
				//ExcelObj.Visible = true;
				// insert a new sheet at the head of the line
				TripoliBooks = ExcelObj.Workbooks;
				TripoliWorkbook = (Excel._Workbook)TripoliBooks.get_Item(1);
				TripoliSheets = TripoliWorkbook.Worksheets;
				
				// see if we already have the sheet
				try 
				{
					Tsheet1 = (Excel._Worksheet)TripoliSheets["TRIPOLI SAVED DATA"];
				}
				catch(Exception eExcel)
				{
					DisposeExcelApp();
					return retval;
				}
				
				// read in data ***************************************************
				retval = new TripoliWorkProduct();

				// this works by leveraging the rest of the masslynx data in this copy
				// this is copied form MassLynxDataFile --- needs to move up ancestor
				// get the CTRL and CYCLES worksheet from the collection
				Excel._Worksheet CYCLEsheet;
				Excel._Worksheet CTRLsheet;
				
				// be sure we have the Masslynx facts maam
				try
				{
					CYCLEsheet = (Excel._Worksheet)TripoliSheets["CYCLE"];
					CTRLsheet = (Excel._Worksheet)TripoliSheets["CTRL"];
				}
				catch
				{
					DisposeExcelApp();
					return retval;
				}
				
				// get number of functions
				Excel.Range range = CTRLsheet.get_Range("ILV_NoFunctions", "ILV_NoFunctions");
				int FunctionCount = Convert.ToInt16(range.Cells.Value2) ;

				// get number of USER functions
				range = CTRLsheet.get_Range("ILV_NoUserFunctions", "ILV_NoUserFunctions");
				int UserFunctionCount = Convert.ToInt16(range.Cells.Value2) ;

				// get number of rows ie ratios
				range = CTRLsheet.get_Range("ILV_Ratios", "ILV_Ratios");
				int RatioCount = Convert.ToInt16(range.Cells.Value2) ;

				// get number of cycles per block
				range = CTRLsheet.get_Range("ILV_CyclesPerBlk", "ILV_CyclesPerBlk");
				int CyclesPerBlock = Convert.ToInt16(range.Cells.Value2) ;

				// get the starting point of the cycle data
				range = CYCLEsheet.get_Range("CD_Origin", "CD_Origin" );
				int startCol = range.Column;
				int startRow = range.Row;

				// get the location of the first function name
				range = CYCLEsheet.get_Range("CD_FunctionNamesDefault", "CD_FunctionNamesDefault" );
				int startFuncNameCol = range.Column;
				int FuncNameRow = range.Row;

				int TripoliStartCol = 1;

				for (int col = 0; col < FunctionCount ; col++)
				{
					range = (Excel.Range)CYCLEsheet.Cells[ FuncNameRow, ( col + startFuncNameCol ) ];
					// get name of function
					string FuncName = Convert.ToString(range.Cells.Value2);

					range = (Excel.Range)Tsheet1.Cells[ 2, ( col + TripoliStartCol ) ];
					// get boolean IsActive
					bool IsActive = Convert.ToBoolean(range.Cells.Value2);

					// define range of ratios in the spreadsheet
					range = Tsheet1.get_Range(
						Tsheet1.Cells[startRow, col + TripoliStartCol], 
						Tsheet1.Cells[startRow + RatioCount - 1, col + TripoliStartCol]);
					// extract the ratios for this function
					Array FunctionValues = (Array)range.Cells.Value2;

					// populate a double [] with these ratios
					double[] tempRatios = new double[FunctionValues.GetLength(0)];
					// populate another double with the absolute value of these
					double[] tempRatiosABS = new double[FunctionValues.GetLength(0)];
					for (int row = 1; row <= FunctionValues.GetLength(0); row++)
					{
						// shift indexes to standard 0-based array
						tempRatios[row - 1] = Convert.ToDouble(FunctionValues.GetValue(row, 1));
						tempRatiosABS[row - 1] = Math.Abs(Convert.ToDouble(FunctionValues.GetValue(row, 1)));
					}

					// create a new RawRatio object to return first calc ststs based on its abs values
					RawRatio myRR = new RawRatio(FuncName, tempRatiosABS);
					myRR.CalcStats();
					myRR.Ratios = tempRatios;
					myRR.CalcStats();

					myRR.IsActive = IsActive;
					// check if user function - these are always last - and label
					if (col > (FunctionCount - UserFunctionCount - 1)) // -1 adjusts for 0-based
						myRR.IsUserFunction = true;
					else
						myRR.IsUserFunction = false;

					myRR.CyclesPerBlock = CyclesPerBlock;
				
					retval.Add(myRR);
				}

				System.Runtime.InteropServices.Marshal.ReleaseComObject (CYCLEsheet);
				CYCLEsheet = null;
				System.Runtime.InteropServices.Marshal.ReleaseComObject (CTRLsheet);
				CTRLsheet = null;
				System.Runtime.InteropServices.Marshal.ReleaseComObject (range);
				range = null;
				System.Runtime.InteropServices.Marshal.ReleaseComObject (Tsheet1);
				Tsheet1 = null;
				System.Runtime.InteropServices.Marshal.ReleaseComObject (TripoliSheets);
				TripoliSheets = null;

				DisposeExcelApp();
				return retval;
			}
			else 
			{
				DisposeExcelApp();
				return retval;
			}

		}


		public void WriteTriopliDataToExcel(Excel._Worksheet Tsheet, ArrayList RawRatios)
		{
			//http://support.microsoft.com/?kbid=317109
			//http://msdn.microsoft.com/vstudio/downloads/tools/
			//http://support.microsoft.com/default.aspx?kbid=302096
			//http://support.microsoft.com/default.aspx?kbid=302084

			Excel.Range RatioRange = null;

			// fix where this comes from
			int NumberRatios = ((RawRatio)RawRatios[0]).Ratios.Length;
			RatioRange = Tsheet.get_Range("A" + ROW_OFFSET.ToString(), Missing.Value);
			RatioRange = RatioRange.get_Resize(NumberRatios, RawRatios.Count );
			RatioRange.NumberFormat = "0.0000000E+00";

			// STORE THE DATA FOR RETRIEVAL Create an array.
			double[,] RatioArray = new double[NumberRatios, RawRatios.Count];
			
			// transfer ratios to array
			Excel.Range temp = null;
			int CountOfActiveFormulae = 0;
			for(int col = 0; col < RatioArray.GetLength(1); col++)
			{
				// need to label the columns
				temp = (Excel.Range)Tsheet.Cells[1, col + 1];
				temp.Cells.Value2 = ((RawRatio)RawRatios[col]).Name;
				temp.ColumnWidth = 15;//.AutoFit();
				temp = (Excel.Range)Tsheet.Cells[2, col + 1];
				temp.Cells.Value2 = ((RawRatio)RawRatios[col]).IsActive;
				// count checked ones for graphing
				if (((RawRatio)RawRatios[col]).IsActive )
					CountOfActiveFormulae ++;

				double[] FunctionRatios = (double[])((RawRatio)RawRatios[col]).Ratios;
				for (int row = 0; row < NumberRatios; row ++)
				{
					RatioArray[row, col] = FunctionRatios[row];
				}

			}


			//Set the range value to the array.
			RatioRange.set_Value(Missing.Value, RatioArray );

			System.Runtime.InteropServices.Marshal.ReleaseComObject (RatioRange);
			RatioRange = null;
		}

		/// <summary>
		/// abandoned code saved for the hard work of discovery !!!
		/// </summary>
		/// <param name="FileName"></param>
		/// <param name="rawRatios"></param>
		private void WriteTripoliWorkFile(string FileName, ArrayList RawRatios)
		{
			//http://support.microsoft.com/?kbid=317109
			//http://msdn.microsoft.com/vstudio/downloads/tools/
			//http://support.microsoft.com/default.aspx?kbid=302096
			//http://support.microsoft.com/default.aspx?kbid=302084


			// abandoned code saved for the hard work of discovery !!!

			Excel.Sheets TripoliSheets = null;
			Excel._Worksheet Tsheet1 = null;
			Excel._Worksheet Tsheet2 = null;
			Excel._Worksheet Tsheet3 = null;
			Excel.Range RatioRange = null;
			Excel.Range RRange2 = null;
			Excel.Range temp = null;
			Excel.Range ChartRange = null;
			Excel.Sheets TripoliCharts = null;
			Excel._Chart Tchart1 = null;

			ExcelObj = new Excel.ApplicationClass();
			ExcelObj.UserControl = false;
			TripoliBooks = ExcelObj.Workbooks;
			TripoliWorkbook = (Excel._Workbook)TripoliBooks.Add( Missing.Value );
			
			TripoliSheets = TripoliWorkbook.Worksheets;
			Tsheet1 = (Excel._Worksheet)TripoliSheets.get_Item(1);
			Tsheet2 = (Excel._Worksheet)TripoliSheets.get_Item(2);
			Tsheet3 = (Excel._Worksheet)TripoliSheets.get_Item(3);

			Tsheet1.Name = "Introduction Tripoli";
			System.Runtime.InteropServices.Marshal.ReleaseComObject (Tsheet1);
			Tsheet1 = null;

			Tsheet2.Name = "Saved Data";
			Tsheet3.Name = "Saved Data II";

			// fix where this comes from
			int NumberRatios = ((RawRatio)RawRatios[0]).Ratios.Length;
			RatioRange = Tsheet2.get_Range("A" + ROW_OFFSET.ToString(), Missing.Value);
			RatioRange = RatioRange.get_Resize(NumberRatios, RawRatios.Count );
			RatioRange.NumberFormat = "0.0000000E+00";

			// STORE THE DATA FOR RETRIEVAL Create an array.
			double[,] RatioArray = new double[NumberRatios, RawRatios.Count];
			// transfer ratios to array
			
			temp = null;
			int CountOfActiveFormulae = 0;
			for(int col = 0; col < RatioArray.GetLength(1); col++)
			{
				// need to label the columns
				temp = (Excel.Range)Tsheet2.Cells[1, col + 1];
				temp.Cells.Value2 = ((RawRatio)RawRatios[col]).Name;
				temp.ColumnWidth = 15;//.AutoFit();
				temp = (Excel.Range)Tsheet2.Cells[2, col + 1];
				temp.Cells.Value2 = ((RawRatio)RawRatios[col]).IsActive;
				// count checked ones for graphing
				if (((RawRatio)RawRatios[col]).IsActive )
					CountOfActiveFormulae ++;

				double[] FunctionRatios = (double[])((RawRatio)RawRatios[col]).Ratios;
				for (int row = 0; row < NumberRatios; row ++)
				{
					RatioArray[row, col] = FunctionRatios[row];
				}

			}


			//Set the range value to the array.
			RatioRange.set_Value(Missing.Value, RatioArray );

			System.Runtime.InteropServices.Marshal.ReleaseComObject (RatioRange);
			RatioRange = null;
			System.Runtime.InteropServices.Marshal.ReleaseComObject (Tsheet2);
			Tsheet2 = null;
			
			
			// STORE THE DATA FOR Charting in Excel 2 columns per function
			//RRange2 = Tsheet3.get_Range(Tsheet3.Cells[ROW_OFFSET, 1], Missing.Value);
			RRange2 = Tsheet3.get_Range("A" + ROW_OFFSET.ToString(), Missing.Value);
			// add 1 here so that column width will include the inserted column belowoops
			RRange2 = RRange2.get_Resize(NumberRatios, CountOfActiveFormulae * 2 + 0);
			RRange2.ColumnWidth = 15;
			RRange2.NumberFormat = "0.0000000E+00";

			// STORE THE DATA FOR RETRIEVAL Create an array.
			double[,] RatioArray2 = new double[NumberRatios, CountOfActiveFormulae * 2];
			// transfer ratios to array
			
			int ActiveColumnCounter = 0;
			for(int col = 0; col < RawRatios.Count; col++)
			{
				if (((RawRatio)RawRatios[col]).IsActive)
				{
					// need to label the columns
					temp = (Excel.Range)Tsheet3.Cells[1, ActiveColumnCounter * 2 + 1];
					temp.Cells.Value2 = ((RawRatio)RawRatios[col]).Name;
					//tempWorkProduct.EntireColumn.AutoFit();
					//tempWorkProduct = (Excel.Range)Tsheet3.Cells[2, col * 2 + 1];
					//tempWorkProduct.Cells.Value2 = ((RawRatio)rawRatios[col]).IsActive;

					double[] FunctionRatios = (double[])((RawRatio)RawRatios[col]).Ratios;
					for (int row = 0; row < NumberRatios; row ++)
					{
						// split into positive and negative
						if (FunctionRatios[row] >= 0)
							RatioArray2[row, ActiveColumnCounter * 2] = FunctionRatios[row];
						else
							RatioArray2[row, ActiveColumnCounter * 2 + 1] = -1.0 * FunctionRatios[row];
					}
					ActiveColumnCounter ++;
				}

			}

			//	Set the range value to the array.
			RRange2.set_Value(Missing.Value, RatioArray2 );

			// now get rid of zeroes by clearing
			for(int col = 0; col < CountOfActiveFormulae * 2; col++)
			{
				for (int row = ROW_OFFSET; row < (NumberRatios + ROW_OFFSET); row ++)
				{
					temp = (Excel.Range)Tsheet3.Cells[row , col + 1];
					double Ratio = Convert.ToDouble(temp.Cells.Value2) ;
					if (Ratio == 0.0)
						temp.Cells.ClearContents();
				}
				//tempWorkProduct.ColumnWidth = 15;//tempWorkProduct.EntireColumn.AutoFit();

			}
			System.Runtime.InteropServices.Marshal.ReleaseComObject (temp);
			temp = null;

			System.Runtime.InteropServices.Marshal.ReleaseComObject (RRange2);
			RRange2 = null;
			
			//chart

			// http://support.microsoft.com/default.aspx?scid=kb;en-us;302084&Product=xl2002

				/*	Range("A10:A28,D10:E28").Select
				Range("D10").Activate
				Charts.Add
				ActiveChart.ChartType = xlXYScatter - 4169
				ActiveChart.SetSourceData Source:=Sheets("Saved Data II").Range( _
				"A10:A28,D10:E28"), PlotBy:=xlColumns 2
				ActiveChart.SeriesCollection(1).Name = "='Saved Data II'!R9C4"
				ActiveChart.SeriesCollection(2).Name = "='Saved Data II'!R9C5"
				ActiveChart.Location Where:=xlLocationAsObject2, Name:="Saved Data II"
				With ActiveChart
					.HasTitle = True
					.ChartTitle.Characters.Text = "206/204"
					.Axes(xlCategory1, xlPrimary1).HasTitle = False
					.Axes(xlValue2, xlPrimary).HasTitle = False
				End With
				*/
			
			// insert a counting column
			Excel.Range Col1 
				= (Excel.Range)Tsheet3.get_Range(
					Tsheet3.Cells[1, 1], Tsheet3.Cells[ROW_OFFSET + NumberRatios, 1]);
			Col1.Insert( -4161, Missing.Value); //xlToRight - 4161
			// now fill with 1,2....NumberRatios
			int[,] counters = new int[NumberRatios, 1];
			for (int i = 0; i < NumberRatios ; i ++)
				counters[i, 0] = i + 1;
			Col1 
				= (Excel.Range)Tsheet3.get_Range(
				Tsheet3.Cells[ROW_OFFSET, 1], Tsheet3.Cells[ROW_OFFSET + NumberRatios - 1, 1]);
			Col1.set_Value( Missing.Value, counters);
			System.Runtime.InteropServices.Marshal.ReleaseComObject (Col1);
			Col1 = null;
	
			//Add a Chart for the selected data.
			
			ChartRange = (Excel.Range)Tsheet3.get_Range("D10" ,"E110");
			//Excel.Range LabelRange = (Excel.Range)Tsheet3.get_Range("A10" ,"A50");
			
			TripoliCharts = TripoliWorkbook.Charts; //TripoliWorkbook.Charts;
			Tchart1 = (Excel._Chart)TripoliCharts.Add(Missing.Value,  Missing.Value, Missing.Value, Missing.Value);
			//Tchart1.ChartType = Excel.XlChartType.xlXYScatter;
			//Tchart1.SetSourceData(ChartRange, Missing.Value);
			Tchart1.ChartWizard( 
				ChartRange, Excel.XlChartType.xlXYScatter, 
				Missing.Value,
				Excel.XlRowCol.xlColumns, 
				Missing.Value,//LabelRange, 
				Missing.Value, Missing.Value, 
				Missing.Value, Missing.Value, Missing.Value, Missing.Value );
			Excel.Series oSeries = (Excel.Series)Tchart1.SeriesCollection(1);
			//oSeries.XValues = Tsheet3.get_Range("A10", "A110");
			oSeries.Name = "Keepers";

			
			Tchart1.Location( Excel.XlChartLocation.xlLocationAsObject, Tsheet3.Name );

/*			Excel.ChartObjects charts = 
				(Excel.ChartObjects)Tsheet3.ChartObjects(Type.Missing);

			// Adds a chart at x = 100, y = 300, 500 points wide and 300 tall.
			Excel.ChartObject chartObj = charts.Add(100, 300, 500, 300);
			Excel.Chart chart = chartObj.Chart;

			// Gets the cells that define the bounds of the data to be charted.
			Excel.Range chartRange = Tsheet3.get_Range("B10","C50");
			chart.SetSourceData(chartRange,Type.Missing);

			chart.ChartType = Excel.XlChartType.xlXYScatter;
			Excel.SeriesCollection seriesCollection= 
				(Excel.SeriesCollection)chart.SeriesCollection(Type.Missing);
			Excel.Series series = seriesCollection.Item(seriesCollection.Count);

/*			//oWB = (Excel._Workbook)oWS.Parent;
			//Excel._Workbook oWB = (Excel._Workbook)Tsheet3.Parent;
			Tchart1 = (Excel._Chart)TripoliWorkbook.Charts.Add( Missing.Value, Missing.Value, 
				Missing.Value, Missing.Value );

			//Use the ChartWizard to create a new chart from the selected data.
			ChartRange = Tsheet3.get_Range("B10:C10", Missing.Value ).get_Resize( 
				NumberRatios, Missing.Value);
			Tchart1.ChartWizard( 
				ChartRange, Excel.XlChartType.xlXYScatter, Missing.Value,
				Excel.XlRowCol.xlColumns, Missing.Value, Missing.Value, Missing.Value, 
				Missing.Value, Missing.Value, Missing.Value, Missing.Value );
/*			oSeries = (Excel.Series)oChart.SeriesCollection(1);
			oSeries.XValues = oWS.get_Range("A2", "A6");
			for( int iRet = 1; iRet <= iNumQtrs; iRet++)
			{
				oSeries = (Excel.Series)oChart.SeriesCollection(iRet);
				String seriesName;
				seriesName = "=\"Q";
				seriesName = String.Concat( seriesName, iRet );
				seriesName = String.Concat( seriesName, "\"" );
				oSeries.Name = seriesName;
			}														  
	
			oChart.Location( Excel.XlChartLocation.xlLocationAsObject, oWS.Name );



*/

			
			//System.Runtime.InteropServices.Marshal.ReleaseComObject (LabelRange);
			//LabelRange = null;
			System.Runtime.InteropServices.Marshal.ReleaseComObject (ChartRange);
			ChartRange = null;
			System.Runtime.InteropServices.Marshal.ReleaseComObject (Tchart1);
			Tchart1 = null;
			System.Runtime.InteropServices.Marshal.ReleaseComObject (TripoliCharts);
			TripoliCharts = null;
			System.Runtime.InteropServices.Marshal.ReleaseComObject (Tsheet3);
			Tsheet3 = null;

			try
			{
				TripoliWorkbook.SaveAs(FileName, xlNormal, "", "", false, false,
					Excel.XlSaveAsAccessMode.xlNoChange,
					true, true,
					Missing.Value, Missing.Value, Missing.Value);
			}
			catch (Exception ee)
			{
				Console.WriteLine(ee.Message);
			}

			try
			{
				System.Runtime.InteropServices.Marshal.ReleaseComObject (TripoliSheets);
				TripoliSheets = null;
			}
			catch{}
			finally
			{
				DisposeExcelApp();

			}
		}

		public void DisposeExcelApp()
		{
			try
			{
				TripoliWorkbook.Close(true, Missing.Value, Missing.Value);
				System.Runtime.InteropServices.Marshal.ReleaseComObject (TripoliWorkbook);
				TripoliWorkbook = null;
			}
			catch(Exception etw)
			{
				Console.WriteLine(etw.Message);
			}
		
			try
			{
				TripoliBooks.Close();
				System.Runtime.InteropServices.Marshal.ReleaseComObject (TripoliBooks);
				TripoliBooks = null;
			}
			catch(Exception etw2)
			{
				Console.WriteLine(etw2.Message);
			}

			
			try
			{
				ExcelObj.Quit();
					
				System.Runtime.InteropServices.Marshal.ReleaseComObject (ExcelObj);
				ExcelObj = null;
			}
			catch(Exception etw3)
			{
				Console.WriteLine(etw3.Message);
			}

			// force final cleanup!
			GC.Collect(); 
			GC.WaitForPendingFinalizers();

			


		}
	}
}
