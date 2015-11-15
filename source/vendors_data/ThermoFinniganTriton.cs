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
using System.IO;

namespace Tripoli.vendors_data
{
    /// <summary>
    /// Another manufacturer different from the MicroMass-GV standard
    /// </summary>
    public class ThermoFinniganTriton : MassSpecDataFile
    {
        // Fields
        FileInfo _ExpFile = null;
        StreamReader _ExpStream = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fi"></param>
        public ThermoFinniganTriton(FileInfo fi)
        {
            // we will open a simple stream on a textfile
            DataFileInfo = fi;
            _ExpFile = fi;

            // we open the stream 
            _ExpStream = new StreamReader(fi.FullName);
        }

        #region Properties

        public StreamReader ExpStream
        {
            get
            {
                return _ExpStream;
            }
            set
            {
                _ExpStream = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string TestFileValidity()
        {
            string line = ExpStream.ReadLine();
            if (line.IndexOf("Triton Analysis Data Report") > -1)
                return "TRUE";
            else
            {
                close();
                return "FALSE";
            }
        }

        /// <summary>
        /// LoadRatios reads the ThermoFinniganThermo data and creates an array of
        /// rawRatios with block information.  This arry is returned as an object
        /// of type TripoliWorkProduct.
        /// </summary>
        /// <returns></returns>
        public override TripoliWorkProduct LoadRatios()
        {
            // set up array to return rawratios
            ArrayList FunctionNames = new ArrayList();
            int FirstRatioIndex = -1;
            ArrayList Blocks = new ArrayList();

            TripoliWorkProduct retval = new TripoliWorkProduct();

            // March 2006
            // using an implicit cycles per block of 20
            // int CyclesPerBlock = 20;

            // June 2012 per Noah
            // 1 block unless a cycles per block integer appears as the first entry after Comment in header
            int expectedCyclesPerBlock = 20000;

            try
            {
                using (ExpStream)
                {
                    String line;

                    string myDayOfMonth = "1";
                    string myMonth = Convert.ToString(myMonths.January);
                    string myYear = "2000";
                    string myHours = "0";
                    string myMinutes = "0";
                    string mySeconds = "0";
                    //string myAMPM = "AM";

                    while ((line = ExpStream.ReadLine()) != null)
                    {
                        // dec 2009 upgrading info collection for live workflow

                        //  sample and file names
                        // 1. SampleName  FractionName UPb 
                        if (line.StartsWith("Sample I"))
                        {// note software misspells identifier !!!
                            line = line.Replace("\"", ""); // remove slashes for quotes
                            line = line.Replace("\t", "");
                            string[] sampleFrac1 = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            if (sampleFrac1.Length > 3)
                            {
                                retval.SampleName = sampleFrac1[2].Trim();
                                retval.FractionName = sampleFrac1[3].Trim();
                                retval.isPartialResult = DataFileInfo.FullName.ToLower().Contains("temp");
                            }
                            else
                            {
                                retval.SampleName = "NO SAMPLE ID FOUND";
                                retval.FractionName = "NO FRACTION ID FOUND";
                                retval.isPartialResult = DataFileInfo.FullName.ToLower().Contains("temp");
                            }
                        }

                        if (line.ToUpper().Contains("Analysis date".ToUpper()))//  this seems to be a bad date in the files Dan Condon sent Dec 2009("Analysis date"))
                        {
                            // June 2010 Albrecht von Quadt sent files containing European style dates:  dd.mm.yyyy
                            // so we need to detect and handle

                            // June 2012 looks like it is impossible to detect european style always

                            line = line.Replace("\"", ""); // remove slashes for quotes
                            string[] dateInfo = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            string dateString = dateInfo[2].Replace("\t", ""); // remove tabs

                            Boolean probablyEuropean = false;
                            string[] dateStringInfo;

                            if (dateString.Contains("."))
                            {
                                probablyEuropean = true;
                                dateStringInfo = dateString.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                            }
                            else // english / american
                            {
                                dateStringInfo = dateString.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                            }

                            // test for numerical flags on date type
                            int firstDateField = Convert.ToInt32(dateStringInfo[0].Trim());
                            int secondDateField = Convert.ToInt32(dateStringInfo[1].Trim());
                            // europeans use days in first place
                            if (firstDateField > 12)
                            {
                                probablyEuropean = true;
                            }
                            // americans use days in second place
                            if (secondDateField > 12)
                            {
                                probablyEuropean = false;
                            }

                            if (probablyEuropean)
                            {
                                myDayOfMonth = dateStringInfo[0].Trim();
                                myMonth = dateStringInfo[1].Trim();
                                myYear = dateStringInfo[2].Trim();
                            }
                            else //  american
                            {
                                myDayOfMonth = dateStringInfo[1].Trim();
                                myMonth = dateStringInfo[0].Trim();
                                myYear = dateStringInfo[2].Trim();
                            }
                        }

                        if (line.StartsWith("Analysis time"))
                        {
                            line = line.Replace("\"", ""); // remove slashes for quotes
                            line = line.Replace("\t", ""); // remove tabs
                            string[] timeInfo = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            string[] timeInfoDet = timeInfo[2].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            myHours = timeInfoDet[0].Trim();
                            myMinutes = timeInfoDet[1].Trim();
                            mySeconds = timeInfoDet[2].Trim();

                            // build a date    
                            // handle AM/PM
                            int theHours = Convert.ToInt32(myHours);
                            if (timeInfo.Length > 3)
                                if (timeInfo[3].Contains("PM") && theHours < 12)
                                {
                                    theHours += 12;
                                }
                            try
                            {
                                retval.TimeStamp = new DateTime(//
                                Convert.ToInt32(myYear), //
                                Convert.ToInt32(Enum.Parse(typeof(myMonths), myMonth)), //
                                Convert.ToInt32(myDayOfMonth), //
                                theHours, //Convert.ToInt32(myHours), //
                                Convert.ToInt32(myMinutes), //
                                Convert.ToInt32(mySeconds));
                            }
                            catch (ArgumentException)
                            { }

                        }

                        // June 2012 Noah proposes the use of comment field to have as first entry the block size in cycles

                        if (line.StartsWith("Comment"))
                        {
                            line = line.Replace("\"", ""); // remove slashes for quotes
                            line = line.Replace("\t", ""); // remove tabs
                            string[] commentInfo = line.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            if (commentInfo.Length > 1)
                            {
                                string specifiedBlockSize = commentInfo[1].Trim();
                                int specifiedBlockSizeInt = 0;
                                try
                                {
                                    specifiedBlockSizeInt = Convert.ToInt32(specifiedBlockSize);
                                }
                                catch (Exception)
                                {
                                   
                                } if (specifiedBlockSizeInt > 0)
                                {
                                    expectedCyclesPerBlock = specifiedBlockSizeInt;
                                }
                            }
                        }


                        // if this never hits, we have no data so see below where
                        // we check if (Blocks.Count == 0)
                        if (line.IndexOf("Cycle	Time") > -1)
                        {
                            // now extract ratio names
                            string header = line;
                            // split it on "\t"
                            string[] temp = header.Split(new Char[] { '\t' });
                            //// now select those that end with ")"
                            //// also save index of first ratio
                            //for (int i = 0; i < temp.Length; i++)
                            //{
                            //    if (temp[i].EndsWith(")"))
                            //    {
                            //        FunctionNames.Add(temp[i]);
                            //        if (FirstRatioIndex == -1)
                            //            FirstRatioIndex = i;
                            //    }
                            //}

                            // june 2012 Noah is now at BGS so we get more robust replacing the filtering code above
                            // the first column is the cycle number
                            // the second column is the time
                            // starting with the third column, we take every column
                            // we trim off the last one if it is empty
                            for (int i = 2; i < temp.Length; i++)
                            {
                                if (temp[i].Trim().Length > 0)
                                {
                                    FunctionNames.Add(temp[i]);
                                }
                            }
                            FirstRatioIndex = 2;


                            // dec 2009
                            // add in more info gleaned from fraction
                            retval.RatioType = "U";
                            for (int i = 0; i < FunctionNames.Count; i++)
                            {
                                if (((string)FunctionNames[i]).Contains("Pb"))
                                {
                                    retval.RatioType = "Pb";
                                    break;
                                }
                            }


                            // now read in block data - here we assume block size of 20
                            // but this means there may be a 21st line containing "D" entries
                            // per Daniel Condon: The D denotes the extra cycle in a block of 
                            // cycles that is used for the beam interpolation, in the GV data 
                            // output these cycles are not outputted.  
                            // They are NOT to be included in graphing/calculation.

                            // we proceed line by line
                            // Special for Finnigan Thermo= ALL ONE BLOCK

                            // June 2012 modification: user can set block size at "Comment" line in header of exp file

                            int currentBlock = 0;
                            // Blocks.Add(new ArrayList());
                            //int cycleCount = 0;

                            string cycle;

                            Boolean keepReadingData = (!((string)(cycle = ExpStream.ReadLine())).StartsWith("***"));

                            while (keepReadingData)
                            {
                                //string[] cycleData = cycle.Split(new Char[] { '\t' });

                                //cycleCount++;

                                currentBlock = Blocks.Add(new ArrayList());
                                int cycleCounter = 0;

                                //Boolean deleteRowFlagDetected = false;

                                while (keepReadingData && (cycleCounter < expectedCyclesPerBlock))
                                {
                                    string[] cycleData = cycle.Split(new Char[] { '\t' });
                                    // inside cycle

                                    // aug 2012 pre-qualify row of ratios
                                    double[] myRatioDoubles = new double[FunctionNames.Count];
                                    int myRatioCount = -1;
                                    bool goodRowDetected = true;

                                    for (int ratio = FirstRatioIndex; ratio < (FirstRatioIndex + FunctionNames.Count); ratio++)
                                    {
                                        try
                                        {
                                            if (cycleData[ratio].StartsWith("X"))
                                            {
                                                // remove "X" prefix that denotes outliers
                                                string tempNum = cycleData[ratio].Substring(1, cycleData[ratio].Length - 1);
                                                myRatioCount++;
                                                double myRatio = Convert.ToDouble(tempNum);
                                                myRatioDoubles[myRatioCount] = myRatio;
                                            }
                                            else if (cycleData[ratio].StartsWith("D") || cycleData[ratio].Trim().Equals(""))
                                            {
                                                myRatioCount++;
                                                myRatioDoubles[myRatioCount] = 0.0;
                                                goodRowDetected = false;
                                            }
                                            else
                                            {
                                                myRatioCount++;
                                                double myRatio = Convert.ToDouble(cycleData[ratio].Trim());
                                                myRatioDoubles[myRatioCount] = myRatio;
                                            }

                                        }
                                        catch (Exception e)
                                        {// this gets rid of bogus empties and spaces
                                            Console.WriteLine("not a ratio: " + cycleData[ratio].Trim());
                                            goodRowDetected = false;
                                        }
                                    }

                                    if (goodRowDetected)
                                    {
                                        for (int ratio = 0; ratio < myRatioDoubles.Length; ratio++)
                                        {
                                            ((ArrayList)Blocks[currentBlock]).Add(myRatioDoubles[ratio]);

                                            //////try
                                            //////{
                                            //////    if (cycleData[ratio].StartsWith("X"))
                                            //////    {
                                            //////        // remove "X" prefix that denotes outliers
                                            //////        string tempNum = cycleData[ratio].Substring(1, cycleData[ratio].Length - 1);
                                            //////        // here for FinniganThermo, the minus 1 flags that it will be shown as red
                                            //////        // june 2012 noah says dont exclude but include so remove -1
                                            //////        ((ArrayList)Blocks[currentBlock]).Add(Convert.ToDouble(tempNum.Trim()));
                                            //////    }
                                            //////    else if (cycleData[ratio].StartsWith("D"))
                                            //////    {
                                            //////        ((ArrayList)Blocks[currentBlock]).Add(0.0);
                                            //////        deleteRowFlagDetected = true;
                                            //////    }
                                            //////    else
                                            //////    {
                                            //////        ((ArrayList)Blocks[currentBlock]).Add(Convert.ToDouble(cycleData[ratio].Trim()));
                                            //////    }
                                            //////}
                                            //////catch { } // this gets rid of bogus empties and spaces


                                            ////////// D represents a value to toss used for beam interpolation
                                            ////////try
                                            ////////{
                                            ////////    if (cycleData[ratio].StartsWith("D"))
                                            ////////    {
                                            ////////        ((ArrayList)Blocks[currentBlock]).Add(0.0);
                                            ////////    }

                                            ////////}
                                            ////////catch { } // this gets rid of bogus empties and spaces
                                        }

                                        cycleCounter++;
                                    }


                                    keepReadingData = false;
                                    try
                                    {
                                        cycle = ExpStream.ReadLine();
                                        //keepReadingData = (!cycle.Contains("\t\t\t")) && (!((string)cycle).StartsWith("***"));
                                        keepReadingData = (!((string)cycle).StartsWith("***"));
                                    }
                                    catch { }

                                    //// check for existence of deleteRowFlagDetected and if so remove first row in block
                                    //if (deleteRowFlagDetected)
                                    //{
                                    //    cycleCounter--;
                                    //    deleteRowFlagDetected = false;

                                    //    // remove last entry
                                    //    for (int i = 0; i < FunctionNames.Count; i++)
                                    //    {
                                    //        int lastIndex = ((ArrayList)Blocks[currentBlock]).Count - 1;
                                    //        ((ArrayList)Blocks[currentBlock]).RemoveAt(lastIndex);
                                    //    }
                                    //}

                                }// while for cycle line

                            }//while

                        }
                    }
                }
            }
            catch (Exception ee)
            {
                // Let the user know what went wrong.
                Console.WriteLine("There was a problem reading the file:");
                Console.WriteLine(ee.Message);
                return null;
            }

            if (Blocks.Count == 0) return null;

            // REFACTOR ALERT = copied from Sector54 
            // now we need to create double[] for each rawratio
            int ratioCount = FunctionNames.Count;
            // size of double[] is going to be number of blocks times size of first block / ratioCount
            ArrayList myDoubles = new ArrayList();
            // find largest Block size to use
            int myBlockSize = 0;
            for (int i = 0; i < Blocks.Count; i++)
            {
                if (((ArrayList)Blocks[i]).Count > myBlockSize)
                    myBlockSize = ((ArrayList)Blocks[i]).Count;
            }

            int myDoubleSize = (Blocks.Count) * myBlockSize / ratioCount;
            for (int ratio = 0; ratio < FunctionNames.Count; ratio++)
            {
                ////////////// march 2006 for FinniganThermo ... here we simulate the deselection of 
                ////////////// the X data supplied by the input file so the points appear red
                ////////////// and can be re-included

                ////////////// this is the same structure as used by dataselected() in the RatioGraph
                ////////////ArrayList selected = new ArrayList();
                ////////////// this saves the negative value needed to show red 
                ////////////ArrayList saved = new ArrayList();
                ////////////int count = 0;

                int currentDouble = myDoubles.Add(new double[myDoubleSize]);
                for (int block = 0; block < Blocks.Count; block++)
                {
                    // June 2007 per Blair Shoene sometimes there are partial lines, so we need to
                    // prevent their use by ignoring partial lines
                    int dataCount =
                        ((ArrayList)Blocks[block]).Count
                        - (((ArrayList)Blocks[block]).Count % ratioCount);

                    for (int num = ratio; num < dataCount; num += ratioCount)
                    //for (int num = ratio; num < ((ArrayList)Blocks[block]).Count; num += ratioCount)
                    //for (int num = (ratio * ratioCount); num < ((ratio + 1) * ratioCount); num ++)
                    {
                        int next = (block * myBlockSize / ratioCount) + (num / ratioCount);
                        //int next = (block * ((ArrayList)Blocks[0]).Count / ratioCount) + (num / ratioCount);

                        ((double[])myDoubles[currentDouble])[next]
                            = Convert.ToDouble(((Double)((ArrayList)Blocks[block])[num]));
                        //////////// reset bad data to 0  - these are shown as 'x' and ignored
                        //////////// for FinniganThermo this is changed so that negative values from X above
                        //////////// can be shown as red below initially
                        //////////if (((double[])myDoubles[currentDouble])[next] < 0.0)
                        //////////{
                        //////////    selected.Add(next);
                        //////////    saved.Add(((double[])myDoubles[currentDouble])[next]);
                        //////////    count++;
                        //////////    ((double[])myDoubles[currentDouble])[next] = Math.Abs(((double[])myDoubles[currentDouble])[next]);
                        //////////}

                        Console.WriteLine("RATIO  " + ratio + "  " + block + "  " + num + "  " + next + "  " + ((double[])myDoubles[currentDouble])[next]);

                    }

                }

                RawRatio myRR = new RawRatio((string)FunctionNames[ratio], (double[])myDoubles[currentDouble]);
                myRR.CyclesPerBlock = ((ArrayList)Blocks[0]).Count / ratioCount;
                retval.Add(myRR);


                //myRR.CalcStats();

                //////////// here the negative values are set to appear as outliers
                //////////for (int index = 0; index < count; index++)
                //////////{
                //////////    myRR.ActiveRatios[(int)selected[index]] = (double)saved[index];
                //////////}
                //////////selected.Add(count);
                //////////myRR.SetOutliers(selected);


            }
            return retval;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void close()
        {
            ExpStream.Close();
        }

        #endregion Methods

    }
}
