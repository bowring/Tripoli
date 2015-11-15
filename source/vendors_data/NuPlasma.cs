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
    /// Nu Plasma machine produces text files - currently based on George Gehrels, Univ Arizona
    /// Jan 2010
    /// </summary>
    public class NuPlasma : MassSpecDataFile
    {
        // Fields
        FileInfo _DatFile = null;
        StreamReader _DatStream = null;

        public NuPlasma(FileInfo fi)
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
            if (line.IndexOf("Run File") > -1)
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
            TripoliWorkProduct retval = new TripoliWorkProduct();

            retval.isPartialResult = DataFileInfo.FullName.ToLower().Contains("temp");
            retval.SampleName = "UPbThFAR";
            retval.FractionName = "NONE";
            retval.RatioType = "UPb";

            // set ratio names
            FunctionNames.Add("206/238");
            FunctionNames.Add("206/207");
            FunctionNames.Add("206/204");
            FunctionNames.Add("208/232");
            FunctionNames.Add("208/204");

            // get to the first block and count and create the ratios
            try
            {
                using (DatStream)
                {
                    string line;

                    string myDayOfMonth = "1";
                    string myMonth = Convert.ToString(myMonthsShort.Jan);
                    string myYear = "2000";
                    string myHours = "0";
                    string myMinutes = "0";
                    string mySeconds = "0";

                    while ((line = DatStream.ReadLine()) != null)
                    {
                        if (line.IndexOf("Started analysis at") > -1)
                        {
                            line = line.Replace("\"", ""); // remove slashes for quotes
                            string[] dateTimeInfo = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            string[] timeInfo = dateTimeInfo[3].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            myDayOfMonth = dateTimeInfo[6].Trim();
                            myMonth = dateTimeInfo[7].Trim();
                            myYear = dateTimeInfo[8].Trim();

                            myHours = timeInfo[0].Trim();
                            myMinutes = timeInfo[1].Trim();
                            mySeconds = "0";

                            // build a TimeStamp       
                            try
                            {
                                retval.TimeStamp = new DateTime(//
                                        Convert.ToInt32(myYear), //
                                        Convert.ToInt32(Enum.Parse(typeof(myMonths), myMonth)), //
                                        Convert.ToInt32(myDayOfMonth), //
                                        Convert.ToInt32(myHours), //
                                        Convert.ToInt32(myMinutes), //
                                        Convert.ToInt32(mySeconds));
                            }
                            catch (ArgumentException argExc)
                            {
                                retval.TimeStamp = new DateTime(//
                                        Convert.ToInt32(myYear), //
                                        Convert.ToInt32(myMonth), //
                                        Convert.ToInt32(myDayOfMonth), //
                                        Convert.ToInt32(myHours), //
                                        Convert.ToInt32(myMinutes), //
                                        Convert.ToInt32(mySeconds));
                            }

                        }

                        // this code is specific to George Gehrels approach for now
                        // Faraday analysis and we know the ratio names, we have to calculate the ratios
                        // each sample will be mapped to a block to leverage the existing tripoli data structures


                        if ((line.IndexOf("Sample Name is") > -1) && (line.IndexOf("SL") > -1))
                        {
                            // we have a standard sample
                            // we read in sample data and calculate ratios until "end of analysis"

                            // create new Block array
                            int currentBlock = Blocks.Add(new ArrayList());
                            string dataLine = "";
                            bool endOfBlock = false;
                            //try
                            //{
                            //    using (DatStream)
                            //    {
                                    while (!endOfBlock)
                                    {
                                        dataLine = DatStream.ReadLine();
                                        if (dataLine.IndexOf("End of Analysis") > -1)
                                        {
                                            endOfBlock = true;
                                        }
                                        else
                                        {
                                            // each line contains 32 doubles and ends with an integer 1...15, comma delimited
                                            double[] myCollectorValues = new double[32];
                                            string[] myCollectors = dataLine.Trim().Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                            for (int collector = 0; collector < 32; collector++)
                                            {
                                                try
                                                {
                                                    string candidateValue = myCollectors[collector].Trim();
                                                    candidateValue = myCollectors[collector].Trim();
                                                    myCollectorValues[collector] = Convert.ToDouble(candidateValue);
                                                }
                                                catch (Exception e)
                                                {// this gets rid of bogus empties and spaces
                                                    Console.WriteLine("not a value: " + myCollectors[collector].Trim());
                                                }
                                            }
                                            // now calculate counts
                                            double count238 = Math.Abs(myCollectorValues[16] - myCollectorValues[0]);
                                            double count232 = Math.Abs(myCollectorValues[17] - myCollectorValues[1]);
                                            double count208 = Math.Abs(myCollectorValues[25] - myCollectorValues[9]);
                                            double count207 = Math.Abs(myCollectorValues[26] - myCollectorValues[10]);
                                            double count206 = Math.Abs(myCollectorValues[27] - myCollectorValues[11]);
                                            double count204 = Math.Abs(myCollectorValues[28] - myCollectorValues[12]);

                                            // calculate ratios
                                            ((ArrayList)Blocks[currentBlock]).Add(count206 / count238);
                                            ((ArrayList)Blocks[currentBlock]).Add(count206 / count207);
                                            ((ArrayList)Blocks[currentBlock]).Add(count206 / count204);
                                            ((ArrayList)Blocks[currentBlock]).Add(count208 / count232);
                                            ((ArrayList)Blocks[currentBlock]).Add(count208 / count204);
                                        }
                                    }
                            //    }
                            //}
                            //catch (Exception e)
                            //{
                            //}

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

            //old code

            if (Blocks.Count == 0) return null;

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
                int currentDouble = myDoubles.Add(new double[myDoubleSize]);
                for (int block = 0; block < Blocks.Count; block++)
                {
                    for (int num = ratio; num < ((ArrayList)Blocks[block]).Count; num += ratioCount)
                    {
                        // see note below re Mat at MIT
                        int next = (block * myBlockSize / ratioCount) + (num / ratioCount);
                        ((double[])myDoubles[currentDouble])[next]
                            = Convert.ToDouble(((Double)((ArrayList)Blocks[block])[num]));
                        // reset bad data to 0  - these are shown as 'x' and ignored
                        if (((double[])myDoubles[currentDouble])[next] <= 0.0)
                            ((double[])myDoubles[currentDouble])[next] = 0.0;

                        Console.WriteLine(ratio + "  " + block + "  " + num + "  " + next + "  " + ((double[])myDoubles[currentDouble])[next]);

                    }

                    // experiment to produce same results gehrels gets
                    // first put ratios in order by size asc within each block = standard
                    // sort sub array based on index and length
                    //int startBlockData = (block * myBlockSize / ratioCount) + (ratio / ratioCount);
                    //Array.Sort(((double[])myDoubles[currentDouble]), startBlockData, 15);
                }

                RawRatio myRR = new RawRatio((string)FunctionNames[ratio], (double[])myDoubles[currentDouble]);

                // repaired April 2007 in response to Matt in Sam's lab who noticed that
                // if the first block was short, all the other blocks were forced short
                // myRR.CyclesPerBlock = ((ArrayList)Blocks[0]).Count / ratioCount;
                myRR.CyclesPerBlock = myBlockSize / ratioCount;

                // the previous statement initializes statistics and maps, so now
                // we can play with George Gehrels techniques
      //          myRR.SortBlocksAscending();




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
