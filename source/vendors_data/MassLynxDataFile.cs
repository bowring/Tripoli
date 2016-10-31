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
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
//using System.Runtime.InteropServices; 
//using Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace Tripoli.vendors_data
{
    /// <summary>
    /// Wrapper for Excel data file supplied by ISOLynx
    /// </summary>
    public class MassLynxDataFile : MassSpecDataFile
    {
        // Fields
        private Excel.Application ExcelObj = null;
        private Excel.Workbook theWorkbook = null;

        public MassLynxDataFile(FileInfo fi)
        {
            // get rid of any open excel instances
            close();

            DataFileInfo = fi;
            ExcelObj = new Excel.Application();

            theWorkbook = ExcelObj.Workbooks.Open(
                fi.FullName, 0, true, 5,
                "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false,
                0, true, Missing.Value, Missing.Value);

        }

        public void DisposeDataFileInfo()
        {
            //http://www.eggheadcafe.com/articles/20021012.asp
            // Need all following code to clean up and extinguish all references!!!
            try
            {
                theWorkbook.Close(null, null, null);
                ExcelObj.Workbooks.Close();
                ExcelObj.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(ExcelObj);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(theWorkbook);
                theWorkbook = null;
                ExcelObj = null;
                GC.Collect(); // force final cleanup!
                GC.WaitForPendingFinalizers();
            }
            catch { }
        }



        public RunParameters LoadRunParameters()
        {
            // set up RunParameters return
            RunParameters retval = new RunParameters();

            // get the collection of sheets in the workbook
            Excel.Sheets sheets = theWorkbook.Worksheets;

            // get the SUMMARY worksheet from the collection
            Excel._Worksheet CTRLsheet = (Excel._Worksheet)sheets["CTRL"];

            // get excel file name
            Excel.Range range = CTRLsheet.get_Range("ILV_DocName", "ILV_DocName");
            retval.ExcelFileName = Convert.ToString(range.Cells.Value2);

            // get aquiredate
            range = CTRLsheet.get_Range("ILV_StartTime", "ILV_StartTime");
            retval.AquireDate = Convert.ToDateTime(range.Cells.Value2);

            sheets = null;

            return retval;

        }

        public override string TestFileValidity()
        {
            // check to see if this is a Masslynx file or a tripoli file or neither
            // get the collection of sheets in the workbook
            Excel.Sheets sheets = theWorkbook.Worksheets;

            try
            {
                // get the SUMMARY worksheet from the collection
                Excel._Worksheet CTRLsheet = (Excel._Worksheet)sheets["CTRL"];
            }
            catch
            {
                return "FALSE";
            }
            // now it is either a masslynx or tripoli

            try
            {
                // get the SUMMARY worksheet from the collection
                Excel._Worksheet Tsheet = (Excel._Worksheet)sheets["TRIPOLI SAVED DATA"];
            }
            catch
            {
                return "TRUE";
            }

            return "TRIPOLI";

        }

        public override TripoliWorkProduct LoadRatios()
		{
			// set up array to return rawratios
			TripoliWorkProduct retval = new TripoliWorkProduct();

			// get the collection of sheets in the workbook
			Excel.Sheets sheets = theWorkbook.Worksheets;

			// get the CTRL and CYCLES worksheet from the collection
			// if fails - this is bad file
			Excel._Worksheet CYCLEsheet;
			Excel._Worksheet CTRLsheet;

			try 
			{
				CYCLEsheet = (Excel._Worksheet)sheets["CYCLE"];
				CTRLsheet = (Excel._Worksheet)sheets["CTRL"];
			}
			catch(Exception eExcel)
			{
					return null;
			}
				
            // get the start time
            Excel.Range range = CTRLsheet.get_Range("ILV_StartTime", "ILV_StartTime");
            retval.TimeStamp = Convert.ToDateTime(range.Cells.Value2);

            // modified Jan 2010 to extract SampleName, FractionName and ratioType 
            // assume sample fraction separated by space

            // modified June 2011 to accomodate new labeled cell for information = ILV_SampleName
            string[] sampFract = null;

            range = CTRLsheet.get_Range("ILV_AnalysisName", "ILV_AnalysisName");
            string analysisName = Convert.ToString(range.Cells.Value2);

            range = CTRLsheet.get_Range("ILV_SampleName", "ILV_SampleName");
            string sampleName = Convert.ToString(range.Cells.Value2);

            if (sampleName.Trim().Length > 0)
            {
                sampFract = sampleName.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            }
            else if (analysisName.Trim().Length > 0)
            {
                sampFract = analysisName.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (sampFract != null)
            {
                retval.SampleName = sampFract[0];
                if (sampFract.Length > 1)
                {
                    retval.FractionName = sampFract[1];
                }
                else
                {
                    retval.FractionName = "FRAC1";
                }
                if (sampleName.Contains("Pb"))
                {
                    retval.RatioType = "Pb";
                }
                if (sampleName.Contains("U"))
                {
                    retval.RatioType = "U";
                }
            }
            else
            {
                retval.SampleName = "MISSING";
                retval.FractionName = "MISSING";
                retval.RatioType = "";

                System.Windows.Forms.MessageBox.Show("Please enter \"[SampleName] [FractionName] [U  or Pb]\" (without brackets)\n\n" //
                    + " into the Sample Name cell  D8 and then re-import the Excel file." );

            }


            // Jan 2010 detect aborted run
            range = CTRLsheet.get_Range("ILV_ErrorMessage", "ILV_ErrorMessage");
            string errorMessage = Convert.ToString(range.Cells.Value2);
            if (errorMessage.Contains("ABORTED"))
            {
                retval.isPartialResult = true;
            }
            else
            {
                retval.isPartialResult = false;
            }


			// get number of functions
			range = CTRLsheet.get_Range("ILV_NoFunctions", "ILV_NoFunctions");
			int FunctionCount = Convert.ToInt16(range.Cells.Value2) ;

			// get number of USER functions
			range = CTRLsheet.get_Range("ILV_NoUserFunctions", "ILV_NoUserFunctions");
			int UserFunctionCount = Convert.ToInt16(range.Cells.Value2) ;

			// get number of cycles per block
			range = CTRLsheet.get_Range("ILV_CyclesPerBlk", "ILV_CyclesPerBlk");
			int CyclesPerBlock = Convert.ToInt16(range.Cells.Value2) ;

            // July 2011 modification to change cycles/block by 1 if beam interpolation is true
            // discovered discrepancy in implementing the IonVantage livedata folder
            range = CTRLsheet.get_Range("ILV_ApplyBeamInterp", "ILV_ApplyBeamInterp");
            string beamInterpApplied = Convert.ToString(range.Cells.Value2);
            if (beamInterpApplied.ToUpper().Trim().Equals("TRUE")){
                CyclesPerBlock ++;
            }

			// get number of rows ie ratios
			range = CTRLsheet.get_Range("ILV_Ratios", "ILV_Ratios");
			int RatioCount = Convert.ToInt16(range.Cells.Value2) ;

			// get the starting point of the cycle data
			range = CYCLEsheet.get_Range("CD_Origin", "CD_Origin" );
			int startCol = range.Column;
			int startRow = range.Row;

			// get the location of the first function name
			range = CYCLEsheet.get_Range("CD_FunctionNamesDefault", "CD_FunctionNamesDefault" );
			int startFuncNameCol = range.Column;
			int FuncNameRow = range.Row;

            int numberOfBlocks = 0;

			for (int col = 0; col < FunctionCount ; col++)
			{
				range = (Excel.Range)CYCLEsheet.Cells[ FuncNameRow, ( col + startFuncNameCol ) ];
				// get name of function
				string FuncName = Convert.ToString(range.Cells.Value2);

                // feb 2010
                if (retval.RatioType.Equals(""))
                {
                    if (FuncName.Contains("20"))
                    {
                        retval.RatioType = "Pb";
                    }
                    if (FuncName.Contains("23"))
                    {
                        retval.RatioType = "U";
                    }
                }

				// define range of ratios in the spreadsheet
				range = CYCLEsheet.get_Range(
					CYCLEsheet.Cells[startRow, col + startFuncNameCol], 
					CYCLEsheet.Cells[startRow + RatioCount - 1, col + startFuncNameCol]);

				// extract the ratios for this function
				Array FunctionValues = (Array)range.Cells.Value2;

				// populate a double [] with these ratios
                // july 2011 calculate the number of blocks and make the data array size be numberOfBlocks X cyclesPerBlock
                // so Tripoli can show missing values at end if they exist

                if (col == 0)
                {
                    numberOfBlocks = (int)Math.Ceiling((double)FunctionValues.GetLength(0) / (double)CyclesPerBlock);
                }

				double[] tempRatios = new double[ numberOfBlocks * CyclesPerBlock];//                              FunctionValues.GetLength(0)];
				for (int row = 1; row <= FunctionValues.GetLength(0); row++)
				{
					// shift indexes to standard 0-based array
                    tempRatios[row - 1] = Convert.ToDouble(FunctionValues.GetValue(row, 1));
					// June 2005 then check for negative values due to string '#NUM!' in excel
					if (tempRatios[row - 1] < 0)
						tempRatios[row - 1] = 0;

				}

				// create a new RawRatio object to return
				RawRatio myRR = new RawRatio(FuncName, tempRatios);
				// check if user function - these are always last - and label
				if (col > (FunctionCount - UserFunctionCount - 1)) // -1 adjusts for 0-based
					myRR.IsUserFunction = true;
				else
					myRR.IsUserFunction = false;

				myRR.CyclesPerBlock = CyclesPerBlock;
				

				retval.Add(myRR);
					
			}
			System.Runtime.InteropServices.Marshal.ReleaseComObject (sheets);
			sheets = null;
			System.Runtime.InteropServices.Marshal.ReleaseComObject (range);
			range = null;

			return retval;
		}

        public override void close()
        {
            if (ExcelObj != null)
            {
                // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dv_wrcore/html/wrtskhowtocloseworkbooks.asp

                DisposeDataFileInfo();
            }


        }
    }
}
