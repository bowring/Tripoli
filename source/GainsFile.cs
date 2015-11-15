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
using System.IO;
using System.Collections;
using System.Security; 
using System.Security.Permissions;
using System.Runtime.Serialization;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
//using Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace Tripoli
{
	/// <summary>
	/// Tripoli representation of GV Excel Gains File all detectors
	/// </summary>
	[Serializable]
	public class GainsFile :TripoliDetailFile
	{
		// Fields
		//private string _FileName = "";
		//private DateTime _InitDate = new DateTime(1);
		//private bool _IsActive;
		//private bool _IsActiveSaved;

		//private ArrayList _Collectors;

		// Fields not to serialize
		private Excel.Application ExcelObj = null;
		private Excel.Workbook theWorkbook = null;
        
		/// <summary>
		/// 
		/// </summary>
		/// <param name="FileName"></param>
		public GainsFile(string FileName):base(FileName)
		{
			//_FileName = FileName;
			//_IsActive = false;
			//_IsActiveSaved = false;
			//_Collectors = new ArrayList();
		}

		#region Serialization and deserialization
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="si"></param>
		/// <param name="context"></param>
		protected GainsFile(SerializationInfo si, StreamingContext context)
			: base(si, context)
		{
			//_FileName = ( string )si.GetValue("FileName",  typeof( string ));
			//_InitDate = ( DateTime )si.GetValue("InitDate",  typeof( DateTime ));
			//_IsActive = ( bool )si.GetValue("IsActive",  typeof( bool ));
			//_IsActiveSaved = ( bool )si.GetValue("IsActiveSaved",  typeof( bool ));
			//_Collectors = ( ArrayList )si.GetValue("Collectors", typeof( ArrayList ));

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="si"></param>
		/// <param name="context"></param>
		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
		public override void GetObjectData(SerializationInfo si, StreamingContext context)
		{
			base.GetObjectData(si,context);
			//si.AddValue("FileName", _FileName, typeof( string ));
			//si.AddValue("InitDate", _InitDate, typeof( DateTime ));
			//si.AddValue("IsActive", _IsActive, typeof( bool ));
			//si.AddValue("IsActiveSaved", _IsActiveSaved, typeof( bool ));
			//si.AddValue("Collectors", _Collectors, typeof( ArrayList ));
		}

		#endregion Serialization and deserialization



		#region Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="FileInfo"></param>
		public void OpenCCGainsExcelFile(DirectoryInfo FileInfo)
		{
			// get rid of any open excel instances
			close();

			FileInfo fi = new FileInfo(FileInfo.FullName + @"\" +  FileName);
			
			ExcelObj = new Excel.Application();

			theWorkbook = ExcelObj.Workbooks.Open(
					fi.FullName, 0, true, 5,
					"", "", true, Excel.XlPlatform.xlWindows, "\t", false, false,
					0, true, Missing.Value, Missing.Value);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool TestFileValidity()
		{
			// check to see if this is a GV CCGains excel file
			// get the collection of sheets in the workbook
			Excel.Sheets sheets = theWorkbook.Worksheets;

			try
			{
				// check for the SUMMARY  and Gains worksheets from the collection
				Excel._Worksheet CTRLsheet = (Excel._Worksheet)sheets["CTRL"];
				Excel._Worksheet Gainssheet = (Excel._Worksheet)sheets["Gains"];

				// check for aborted
				Excel.Range range = CTRLsheet.get_Range("ILV_ErrorMessage", "ILV_ErrorMessage");
				string errorMess = Convert.ToString(range.Cells.Value2);
				if (errorMess.Trim().EndsWith("ABORTED!"))
					return false;

				// get number of cycles
				range = CTRLsheet.get_Range("CCG_TotalCycles", "CCG_TotalCycles");
				int CyclesCount = Convert.ToInt16(range.Cells.Value2);

				if (CyclesCount <= 0) return false;
            }
			catch 
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		public void LoadGains()
		{
			// get the collection of sheets in the workbook
			Excel.Sheets sheets = theWorkbook.Worksheets;

			// get the CTRL and CYCLES worksheet from the collection
			// if fails - this is bad file
			Excel._Worksheet Gainssheet;
			Excel._Worksheet CTRLsheet;
			try 
			{
				Gainssheet = (Excel._Worksheet)sheets["Gains"];
				CTRLsheet = (Excel._Worksheet)sheets["CTRL"];
			}
			catch(Exception eExcel)
			{
				return;
			}
				
			// get initial date
			Excel.Range range = CTRLsheet.get_Range("ILV_StartTime", "ILV_StartTime");
			InitDate = Convert.ToDateTime(range.Cells.Value2);

			// get number collectors
			range = CTRLsheet.get_Range("ILV_NoCollectors", "ILV_NoCollectors");
			int CollectorCount = Convert.ToInt16(range.Cells.Value2);

			// get number of cycles
			range = CTRLsheet.get_Range("CCG_TotalCycles", "CCG_TotalCycles");
			int CyclesCount = Convert.ToInt16(range.Cells.Value2);

			// get the starting point of the cycle data
			range = Gainssheet.get_Range("CCG_Gains", "CCG_Gains" );
			int startRow = range.Row;
			
			// get list of Collector names ILV_CollNames
			range = CTRLsheet.get_Range("ILV_CollNames", "ILV_CollNames");
			int startCollectorNameCol = range.Column;
			int CollectorNameRow = range.Row;
			
			for (int col = startCollectorNameCol; col <= CollectorCount; col ++)
			{
				// get name of collector
				range = (Excel.Range)CTRLsheet.Cells[ CollectorNameRow, ( col + startCollectorNameCol ) ];
				string CollectorName = Convert.ToString(range.Cells.Value2);

				// get TYPE of collector
				range = (Excel.Range)CTRLsheet.Cells[ CollectorNameRow + 1, ( col + startCollectorNameCol ) ];
				string CollectorType = Convert.ToString(range.Cells.Value2);

                // April 2011 for new style results
                // get collector number
                range = (Excel.Range)CTRLsheet.Cells[CollectorNameRow + 2, (col + startCollectorNameCol)];
                int CollectorNum = 0;
                try
                {
                    CollectorNum = Convert.ToInt16(range.Cells.Value2);
                }
                catch (Exception)
                {                 
                    //throw;
                }
				// continue if faraday and not Axial
                //if ((CollectorType.Trim().StartsWith("Faraday")) 
                //    &&
                //    (! CollectorName.Trim().StartsWith("Axial")))
                // modified April 2011 to handle new MassSpec software useage and just look for High and Low
                if (((CollectorName.Trim().StartsWith("High"))
                    ||
                    (CollectorName.Trim().StartsWith("Low")))//
                    &&//
                    (CollectorType.Trim().StartsWith("Faraday")//
                    ||//
                    (CollectorType.Trim().StartsWith("-") && (CollectorNum > 0))//
                    ))
				{
					// define range of ratios in the spreadsheet
					range = Gainssheet.get_Range(
						Gainssheet.Cells[startRow, col + startCollectorNameCol], 
						Gainssheet.Cells[startRow + CyclesCount - 1, col + startCollectorNameCol]);

					// extract the ratios for this Collector
					Array CollectorValues = (Array)range.Cells.Value2;

					// populate a double [] with these ratios
					double[] tempRatios = new double[CollectorValues.GetLength(0)];
					for (int row = 1; row <= CollectorValues.GetLength(0); row++)
					{
						// shift indexes to standard 0-based array
						tempRatios[row - 1] = Convert.ToDouble(CollectorValues.GetValue(row, 1));
						// June 2005 then check for negative values due to string '#NUM!' in excel
						if (tempRatios[row - 1] < 0)
							tempRatios[row - 1] = 0;
					}

					// create a new RawRatio object to return
					RawRatio myRR = new RawRatio(CollectorName, tempRatios);
					
					myRR.CyclesPerBlock = CyclesCount;
				
					Collectors.Add(myRR);
				}
			}
			System.Runtime.InteropServices.Marshal.ReleaseComObject (sheets);
			sheets = null;
			System.Runtime.InteropServices.Marshal.ReleaseComObject (range);
			range = null;
		}

		private void DisposeDataFileInfo()
		{
			//http://www.eggheadcafe.com/articles/20021012.asp
			// Need all following code to clean up and extingush all references!!!
			theWorkbook.Close(null,null,null);
			ExcelObj.Workbooks.Close();
			ExcelObj.Quit();
			System.Runtime.InteropServices.Marshal.ReleaseComObject (ExcelObj);
			System.Runtime.InteropServices.Marshal.ReleaseComObject (theWorkbook);
			theWorkbook = null;
			ExcelObj = null;
			GC.Collect(); // force final cleanup!
			GC.WaitForPendingFinalizers();
		}

		/// <summary>
		/// 
		/// </summary>
		public void close()
		{
			if (ExcelObj != null)
			{
				// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dv_wrcore/html/wrtskhowtocloseworkbooks.asp
                try
                {
                    DisposeDataFileInfo();
                }
                catch (Exception)
                {
                    
                  //  throw;
                }
			}
		}

		#endregion Methods
	}
}
