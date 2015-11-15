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
    /// ThermoFinniganMat261 machine produces text files
    /// </summary>
    public class ThermoFinniganMat261 : MassSpecDataFile
    {
        // Fields
        FileInfo _DatFile = null;
        StreamReader _DatStream = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fi"></param>
        public ThermoFinniganMat261(FileInfo fi)
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
            string line = DatStream.ReadLine().Trim();
           // if (line.IndexOf("SIMULTANEOUS DATA ACQUISITION") > -1)
            // note Mat 262 dynamic files have the word acquisition misspeclled at top 
            if (line.IndexOf("DATA A") > -1)
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

            //bool FirstPass = true;
           // bool FirstPassGrand = true;

            retval.isPartialResult = DataFileInfo.FullName.ToLower().Contains("temp");

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

                    bool firstPass = true;

                    while (firstPass &&  ((line = DatStream.ReadLine()) != null))
                    {
                        // jan 2015
                        line = line.Trim();

                        if (line.StartsWith("SAMPLE I"))
                        {
                            string[] sampleLine = line.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            string[] sampleID = sampleLine[1].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            if (sampleID[0].StartsWith("FILE"))
                            {
                                retval.SampleName = "NONE";
                            }
                            else if (sampleID.Length > 2)
                            {
                                if (sampleID[2].StartsWith("FILE"))
                                {
                                    retval.SampleName = sampleID[0].Trim() + "_" + sampleID[1].Trim();
                                }
                            }
                            else
                            {
                                retval.SampleName = sampleID[0].Trim();
                            }
                        }

                        if (line.StartsWith("COMMENT"))
                        {
                            string[] fractionLine = line.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            string[] fractionID = fractionLine[1].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            if (!fractionID[0].StartsWith("USER"))
                            {
                                retval.FractionName = "_" + fractionID[0].Trim();
                            }
                        }

                        if (line.StartsWith("DATE"))
                        {
                            string[] dateTimeInfo = line.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            string[] dateInfo = dateTimeInfo[1].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            myDayOfMonth = dateInfo[0].Trim();
                            myMonth = dateInfo[1].Trim();
                            myYear = dateInfo[2].Trim();

                            myHours = dateTimeInfo[2].Trim();
                            myMinutes = dateTimeInfo[3].Trim();
                            mySeconds = dateTimeInfo[4].Trim();

                            // build a TimeStamp       
                            try
                            {
                                retval.TimeStamp = new DateTime(//
                                        Convert.ToInt32(myYear), //
                                        Convert.ToInt32(Enum.Parse(typeof(myMonthsShort), myMonth)), //
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

                        // for this type of file, we need two passes 
                        // pass 1 we get the meta data, pass2 we get the data
                        // still in pass 1

                        // detect ratio names
                        if (line.StartsWith("RUN SUMMARY OF INDIVIDUAL RATIOS"))
                        {
                            // read two lines and then get ratio names from next set of lines
                            string ratioLine = DatStream.ReadLine().Trim();
                            ratioLine = DatStream.ReadLine().Trim();
                            bool keepReading = (ratioLine.Trim().Length > 0);
                            while (keepReading)
                            {
                                // lines should be of form:  204/205    0.091584436 0.003919269    4.279    0.0022627907    2.471       12
                                ratioLine = DatStream.ReadLine().Trim();
                                if ((ratioLine.Length > 0) && !ratioLine.StartsWith("R"))
                                {
                                    string[] ratioLineData = ratioLine.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                    FunctionNames.Add(ratioLineData[0].Trim());
                                }
                                else
                                {
                                    keepReading = false;
                                }

                            }

                            if (((string)FunctionNames[0]).Contains("20"))
                                retval.RatioType = "Pb";
                            else
                                retval.RatioType = "U";

                            // PASS 2 for data ********************************************************
                            firstPass = false;
                            StreamReader DatStream2 = new StreamReader(_DatFile.FullName);

                            try
                            {
                                using (DatStream2)
                                {
                                    while ((line = DatStream2.ReadLine()) != null)
                                    {
                                        // jan 2015
                                        line = line.Trim();

                                        int totalRatiosPerLineInParagraph1 = 0;
                                        int totalRatiosPerLineInParagraph2 = 0;

                                        if (line.StartsWith("RED"))
                                        {
                                            // we have a block to read
                                            Console.WriteLine(line);
                                            // create new Block array
                                            int currentBlock = Blocks.Add(new ArrayList());

                                            // read until we get the first data line of form:  1      .097017105      1.657461614      1.372436105      3.324556343      1.207678527 
                                            // then read until we get end of first paragraph of data: M      .091584436      1.662938121      1.369846246      3.336047608      1.213968244 
                                            // then repeat once for the second paragraph and we are done with the block
                                            string dataLine = DatStream2.ReadLine().Trim();
                                           
                                            // mar 2015
                                            while (!dataLine.StartsWith("1"))
                                            {
                                                dataLine = DatStream2.ReadLine().Trim();
                                                Console.WriteLine(dataLine);
                                            }
                                            Boolean readNextLine = false;
                      /*                      
                                            // read until the blank line before the data 
                                            while (dataLine.Trim() != "")
                                            {
                                                dataLine = DatStream2.ReadLine().Trim(); 
                                                Console.WriteLine(dataLine);
                                            }
                       */ 
                                            // read the data lines until we get one starting with M
                                            bool endOfParagraph1 = false;
                                            bool endOfParagraph2 = false;

                       /*                     // second blank line
                                            dataLine = DatStream2.ReadLine().Trim();
                                            // in case there is a missing carriage return
                                            Boolean readNextLine = !dataLine.StartsWith("1");
                       */                     
                                            while (!endOfParagraph1)
                                            {
                                                if (readNextLine)
                                                {
                                                    dataLine = DatStream2.ReadLine().Trim();
                                                }
                                                readNextLine = true;

                                                if (dataLine.Trim().StartsWith("M"))
                                                {
                                                    endOfParagraph1 = true;
                                                }
                                                else
                                                {
                                                    string[] myRatios = dataLine.Trim().Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                                    totalRatiosPerLineInParagraph1 = myRatios.Length - 1;
                                                    for (int ratio = 1; ratio < myRatios.Length; ratio++)
                                                    {
                                                        try
                                                        {
                                                            string candidateRatio = myRatios[ratio].Trim();
                                                            candidateRatio = candidateRatio.Replace("*", ""); //myRatios[ratio].Trim().Replace("*", "");
                                                            // std file from mat 262 sometimes has trailing "O" in number
                                                            candidateRatio = candidateRatio.Replace("O", ""); //myRatios[ratio].Trim().Replace("O", "");
                                                            int last = ((ArrayList)Blocks[currentBlock]).Add(Convert.ToDouble(candidateRatio));
                                                        }
                                                        catch (Exception e)
                                                        {// this gets rid of bogus empties and spaces
                                                            Console.WriteLine("not a ratio: " + myRatios[ratio].Trim());
                                                        }
                                                    }
                                                }
                                            }

                                            // read three intervening lines
                                            dataLine = DatStream2.ReadLine();
                                            dataLine = DatStream2.ReadLine();
                                            dataLine = DatStream2.ReadLine();

                                            // detects if there is a second paragraph
                                            if (dataLine.Contains("-----"))
                                            {
                                                endOfParagraph2 = true;
                                            }
                                            
                                           
                                            int lineCount = 0;
                                            while (!endOfParagraph2)
                                            {
                                                dataLine = DatStream2.ReadLine().Trim();
                                                if (dataLine.Trim().StartsWith("M"))
                                                {
                                                    endOfParagraph2 = true;
                                                }
                                                else
                                                {
                                                    string[] myRatios = dataLine.Trim().Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                                    totalRatiosPerLineInParagraph2 = myRatios.Length - 1;
                                                    for (int ratio = 1; ratio < myRatios.Length; ratio++)
                                                    {
                                                        try
                                                        {
                                                            string candidateRatio = myRatios[ratio].Trim();
                                                            candidateRatio = candidateRatio.Replace("*", ""); //myRatios[ratio].Trim().Replace("*", "");
                                                            // std file from mat 262 sometimes has trailing "O" in number
                                                            candidateRatio = candidateRatio.Replace("O", "");
                                                            // insert on this pass as we are doubling back
                                                            int next = lineCount * (totalRatiosPerLineInParagraph1 + totalRatiosPerLineInParagraph2) + totalRatiosPerLineInParagraph1 + (ratio - 1);
                                                            ((ArrayList)Blocks[currentBlock]).Insert(next, Convert.ToDouble(candidateRatio));
                                                        }
                                                        catch (Exception e)
                                                        {// this gets rid of bogus empties and spaces
                                                            Console.WriteLine("not a ratio: " + myRatios[ratio].Trim());
                                                        }
                                                    }
                                                    lineCount++;
                                                }
                                            }


                                        }
                                    }
                                    DatStream2.Close();
                                }
                            }
                            catch (Exception ee)
                            {
                                // Let the user know what went wrong.
                                Console.WriteLine("There was a problem reading the file:");
                                Console.WriteLine(ee.Message);
                                return null;
                            }
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
                    //for (int num = (ratio * ratioCount); num < ((ratio + 1) * ratioCount); num ++)
                    {
                        // see note below re Mat at MIT
                        //int next = (block * ((ArrayList)Blocks[0]).Count / ratioCount) + (num / ratioCount);
                        int next = (block * myBlockSize / ratioCount) + (num / ratioCount);
                        ((double[])myDoubles[currentDouble])[next]
                            = Convert.ToDouble(((Double)((ArrayList)Blocks[block])[num]));
                        // reset bad data to 0  - these are shown as 'x' and ignored
                        if (((double[])myDoubles[currentDouble])[next] <= 0.0)
                            ((double[])myDoubles[currentDouble])[next] = 0.0;

                        Console.WriteLine(ratio + "  " + block + "  " + num + "  " + next + "  " + ((double[])myDoubles[currentDouble])[next]);

                    }

                }

                RawRatio myRR = new RawRatio((string)FunctionNames[ratio], (double[])myDoubles[currentDouble]);

                // repaired April 2007 in response to Matt in Sam's lab who noticed that
                // if the first block was short, all the other blocks were forced short
                // myRR.CyclesPerBlock = ((ArrayList)Blocks[0]).Count / ratioCount;
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
