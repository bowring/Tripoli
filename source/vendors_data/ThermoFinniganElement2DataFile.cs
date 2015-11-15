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
    /// Reads in a single data file (.txt) from ThermoFinnigan Element2 - based on Jef Vervoort's setup at Washington State U.
    /// </summary>
    public class ThermoFinniganElement2DataFile : MassSpecDataFile
    {
        // Fields
        FileInfo _DatFile = null;
        StreamReader _DatStream = null;

        public ThermoFinniganElement2DataFile(FileInfo fi)
        {
            // we will open a simple stream on a textfile

            DataFileInfo = fi;
            _DatFile = fi;

            // we open the stream 
            _DatStream = new StreamReader(fi.FullName);
        }

        #region Properties

        public StreamReader DatStream
        {
            get
            {
                return _DatStream;
            }
            set
            {
                _DatStream = value;
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
            string line = DatStream.ReadLine();
            if (line.ToUpper().IndexOf("TRACE FOR MASS:") > -1)
                return "TRUE";
            else
            {
                close();
                return "FALSE";
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override TripoliWorkProduct LoadRatios()
        {
            // set up array to return rawratios
            ArrayList FunctionNames = new ArrayList();
            ArrayList Blocks = new ArrayList();
            int cyclesPerBlock = 10000; // forces all data into one block
            TripoliWorkProduct retval = new TripoliWorkProduct();
            retval.isPartialResult = false;
            retval.RatioType = "UPb";


            try
            {
                // reset DatStream from test of first line
                DatStream.Close();
                DatStream = new StreamReader(DataFileInfo.FullName);

                using (DatStream)
                {
                    string line;

                    string myDayOfMonth = "1";
                    string myMonth = "1";
                    string myYear = "2000";
                    string myHours = "0";
                    string myMinutes = "0";
                    string mySeconds = "0";

                    // mar 2010 missing date stamp so do this for now
                    retval.TimeStamp = new DateTime(//
                                        Convert.ToInt32(myYear), //
                                        Convert.ToInt32(myMonth), //
                                        Convert.ToInt32(myDayOfMonth), //
                                        Convert.ToInt32(myHours), //
                                        Convert.ToInt32(myMinutes), //
                                        Convert.ToInt32(mySeconds));

                    while ((line = DatStream.ReadLine()) != null)
                    {

                        ////if (line.ToUpper().StartsWith("DATE"))
                        ////{
                        ////    string[] dateTimeInfo = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        ////    string[] dateInfo = dateTimeInfo[1].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                        ////    myDayOfMonth = dateInfo[1].Trim();
                        ////    myMonth = dateInfo[0].Trim();
                        ////    myYear = dateInfo[2].Trim();

                        ////    // build a TimeStamp       
                        ////    try
                        ////    {
                        ////        retval.TimeStamp = new DateTime(//
                        ////                Convert.ToInt32(myYear), //
                        ////                Convert.ToInt32(Enum.Parse(typeof(myMonthsShort), myMonth)), //
                        ////                Convert.ToInt32(myDayOfMonth), //
                        ////                Convert.ToInt32(myHours), //
                        ////                Convert.ToInt32(myMinutes), //
                        ////                Convert.ToInt32(mySeconds));
                        ////    }
                        ////    catch (ArgumentException argExc)
                        ////    {
                        ////        retval.TimeStamp = new DateTime(//
                        ////                Convert.ToInt32(myYear), //
                        ////                Convert.ToInt32(myMonth), //
                        ////                Convert.ToInt32(myDayOfMonth), //
                        ////                Convert.ToInt32(myHours), //
                        ////                Convert.ToInt32(myMinutes), //
                        ////                Convert.ToInt32(mySeconds));
                        ////    }

                        ////}

                        // detect ratio names
                        if (line.ToUpper().StartsWith("TRACE FOR MASS:"))
                        {
                            // intensity and ratio names populate this line, followed  6 lines down by lines of data
                            string ratioLine = line;

                            // line should be of form:  Trace for Mass:	Hg202(LR)	Pb204(LR)	Pb206(LR)	Pb207(LR)	Pb208(LR)	Th232(LR)	U235(LR)	U238(LR)	
                            //                                                                              Pb206/Pb207(LR)	Pb206/U238(LR)	206/Pb204(LR)	Pb207/U235(LR)	Pb208/Th232(LR)
                            string[] ratioNames = ratioLine.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                            // load in names, ignoring first cell
                            for (int i = 1; i < ratioNames.Length; i++)
                            {
                                FunctionNames.Add(ratioNames[i].Trim());
                            }

                            // Skip 5 lines and each succeeding line contains cycle data *********************************
                            String dataLine = null;
                            for (int i = 0; i < 5; i++)
                            {
                                dataLine = DatStream.ReadLine().Trim();
                            }
                            
                            dataLine = DatStream.ReadLine().Trim();
                            Boolean keepReadingBlocks = (dataLine.Length > 0);

                            while (keepReadingBlocks)
                            {
                                // create new Block array
                                int currentBlock = Blocks.Add(new ArrayList());
                                for (int b = 0; b < cyclesPerBlock; b++)
                                {
                                    if (dataLine != null)
                                    {
                                        string[] myRatios = dataLine.Trim().Split(new String[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                                        // ignore first entry = cycle number
                                        for (int ratio = 1; ratio < myRatios.Length; ratio++)
                                        {
                                            try
                                            {
                                                string candidateRatio = myRatios[ratio].Trim();
                                                int last = ((ArrayList)Blocks[currentBlock]).Add(Convert.ToDouble(candidateRatio));
                                            }
                                            catch (Exception e)
                                            {// this gets rid of bogus empties and spaces
                                                Console.WriteLine("not a ratio: " + myRatios[ratio].Trim());
                                            }
                                        }
                                        dataLine = DatStream.ReadLine();
                                    }
                                }
                                // done with block, see if more data
                                keepReadingBlocks = (dataLine != null);
                            }
                        }
                    }
               //     Console.WriteLine("line   =  " + line);
                }
            }
            catch (Exception ee)
            {
                // Let the user know what went wrong.
                Console.WriteLine("There was a problem reading the file:");
                Console.WriteLine(ee.Message);
                return null;
            }

            //old code

            if (Blocks.Count == 0) return null;

            // now we need to create double[] for each raw data value
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
                int currentDouble = myDoubles.Add(new double[myDoubleSize]);
                for (int block = 0; block < Blocks.Count; block++)
                {
                    for (int num = ratio; num < ((ArrayList)Blocks[block]).Count; num += ratioCount)
                    {
                        int next = (block * myBlockSize / ratioCount) + (num / ratioCount);
                        ((double[])myDoubles[currentDouble])[next]
                            = Convert.ToDouble(((Double)((ArrayList)Blocks[block])[num]));
                        // reset bad data to 0  - these are shown as 'x' and ignored
                        if (((double[])myDoubles[currentDouble])[next] <= 0.0)
                            ((double[])myDoubles[currentDouble])[next] = 0.0;

                       // Console.WriteLine(ratio + "  " + block + "  " + num + "  " + next + "  " + ((double[])myDoubles[currentDouble])[next]);

                    }

                }

                RawRatio myRR = new RawRatio((string)FunctionNames[ratio], (double[])myDoubles[currentDouble]);
                myRR.CyclesPerBlock = myBlockSize / ratioCount;

                retval.Add(myRR);
            }
            return retval;
        }


        /// <summary>
        /// 
        /// </summary>
        public override void close()
        {
            DatStream.Close();
        }

        #endregion Methods

    }
}
