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

//using Tripoli.vendors_data;

namespace Tripoli.vendors_data
{
	/// <summary>
	/// Sector 54 is an older machine that produces text files
	/// </summary>
	public class Sector54DataFile : MassSpecDataFile
	{
		// Fields
		FileInfo _DatFile = null;
		StreamReader _DatStream = null;

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fi"></param>
		public Sector54DataFile(FileInfo fi)
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
				_DatStream = value ;
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
			if (line.IndexOf("Isotope Ratio Software Data Dump") > -1 )
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

			bool FirstPass = true;
			bool FirstPassGrand = true;
            bool TimeFieldRead = false;

			// feb 2005 refinement to differentiate between two kinds of data
			// files :  the original Single Collector Cycle Data
			//          and the 'new' Static Collector Block Data
			//          that has no data - just summaries - so we will
			//          read the summaries, using the AFTER numbers
			//          and generate fake data = all the same for each block

			// get to the first block and count and create the ratios
			try 
			{
				using (DatStream) 
				{
                    string line;

                    string myDayOfMonth = "1";
                    string myMonth = Convert.ToString(myMonths.January);
                    string myYear = "2000";
                    string myHours = "0";
                    string myMinutes = "0";
                    string mySeconds = "0";

					while ((line = DatStream.ReadLine()) != null)
					{
                        // Nov 2009 live mode need to get sample and file names
                        // there are two common possibilities
                        // 1. SampleName  FractionName UPb (Sam Bowring)
                        // 2. U or Pb SampleName  FractionName  (Drew Coleman)
                        if (line.StartsWith("\"Sample I")){// note software misspells identifier !!!
                            line = line.Replace("\"", ""); // remove slashes for quotes
                            string[] sampleFrac1 = line.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries);
                            if (sampleFrac1.Length < 4)
                            {
                                // non standard naming detected
                                retval.SampleName = "_" + sampleFrac1[2].Trim();
                                retval.FractionName = "";
                                retval.isPartialResult = DataFileInfo.FullName.ToLower().Contains("temp");
                            }
                            else if ((sampleFrac1[2].Trim().ToLower().CompareTo("pb") == 0) || //
                                (sampleFrac1[2].Trim().ToLower().CompareTo("u") == 0))
                            {
                                retval.SampleName = sampleFrac1[3].Trim();
                                retval.FractionName = sampleFrac1[4].Trim();
                                retval.isPartialResult = DataFileInfo.FullName.ToLower().Contains("temp");
                            }
                            else
                            {
                                retval.SampleName = sampleFrac1[2].Trim();
                                retval.FractionName = sampleFrac1[3].Trim();
                                retval.isPartialResult = DataFileInfo.FullName.ToLower().Contains("temp");
                            }
                        }

                        if (line.StartsWith("\"Date"))
                        {   
                            line = line.Replace("\"", ""); // remove slashes for quotes
                            string[] dateInfo = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            myDayOfMonth = dateInfo[1].Trim();
                            myMonth = dateInfo[2].Trim();
                            myYear = dateInfo[3].Trim();
                        }

                        if (!TimeFieldRead && (line.StartsWith("\"Time")))
                        {
                            line = line.Replace("\"", ""); // remove slashes for quotes
                            string[] timeInfo = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            string[] timeInfoDet = timeInfo[1].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            myHours = timeInfoDet[0].Trim();
                            myMinutes = timeInfoDet[1].Trim();
                            mySeconds = timeInfoDet[2].Trim();

                            // build a date       
                            retval.TimeStamp = new DateTime(//
                                Convert.ToInt32(myYear), //
                                Convert.ToInt32(Enum.Parse(typeof(myMonths), myMonth)), //
                                Convert.ToInt32(myDayOfMonth), //
                                Convert.ToInt32(myHours), //
                                Convert.ToInt32(myMinutes), //
                                Convert.ToInt32(mySeconds));

                            TimeFieldRead = true;
                        }

                        if (line.StartsWith("\"#END#\"")){
                            retval.isPartialResult = false;
                        }
						// if this never hits, we have no data so see below where
						// we check if (Blocks.Count == 0)
						if (line == "\"Individual Ratios for this Block:\"")
						{
							// create new Block array
							int currentBlock = 0;// see below Blocks.Add(new ArrayList());

							// now read in ratio names
							string ratioNames = DatStream.ReadLine();
							
							// July 2005 learned of an old function of the sector 54 softwarte that can produce
							// files in a no longer supported format...need to trap for them
							if (! ratioNames.StartsWith("\""))
							{
								// no ratio names here
								// the effect will be to treat this old style as block summaries only
								// because we ignore the individual ratios with this conditional
							}
							else
							{
								// test for two lines
								string testLine = DatStream.ReadLine();
								// test for aborted run
								if (testLine.Trim() == "") break;////is this kosher?
								if (testLine.Substring(0,1) == "\"")
								{
									ratioNames += testLine;
									testLine = "";
								}
								if (FirstPass)
								{
									//parse into method and node
									string[] myRatioNames = ratioNames.Split(new Char[]{'\"'});
									for (int name = 0; name < myRatioNames.Length; name++)
									{
										// create a new RawRatio object to return
										if (myRatioNames[name].Trim() != "")
										{
											FunctionNames.Add(myRatioNames[name].Trim());
										}
									}
									FirstPass = false;

                                    // nov 2009 detect ratio type Pb or U
                                    if (((string)FunctionNames[0]).Contains("20"))
                                    {
                                        retval.RatioType = "Pb";
                                    }
                                    if (((string)FunctionNames[0]).Contains("23"))
                                    {
                                        retval.RatioType = "U";
                                    }
								}
								// now read in ratios
								// we read in a line at a time, grouping lines until we
								// have one number for each ratio, then repeat until a blank line
                                //if (testLine == "")
                                //{
                                //    testLine = DatStream.ReadLine();
                                //}
                                while (testLine.Trim() == "")
                                {
                                    testLine = DatStream.ReadLine();
                                }
                                //if (testLine.Trim() != "")
                                //{
									// create new Block array
                                currentBlock = Blocks.Add(new ArrayList());
								//}
								while (testLine.Trim() != "")
								{
									string[] myRatios = testLine.Trim().Split(new Char[]{' '});

                                        // aug 2012 pre-qualify row of ratios
                                        double[] myRatioDoubles = new double[FunctionNames.Count];
                                        int myRatioCount = -1;
                                        bool goodRowDetected = true;
                                        for (int ratio = 0; ratio < myRatios.Length; ratio++)
                                        {
                                            try
                                            {
                                                if (!myRatios[ratio].Trim().Equals(""))
                                                {
                                                    myRatioCount++;
                                                    double myRatio = Convert.ToDouble(myRatios[ratio].Trim());
                                                    myRatioDoubles[myRatioCount] = myRatio;
                                                }
                                            }
                                            catch (Exception e)
                                            {// this gets rid of bogus empties and spaces
                                                Console.WriteLine("not a ratio: " + myRatios[ratio].Trim());
                                                goodRowDetected = false;
                                            }
                                        }

                                        if (goodRowDetected)
                                        {
                                            for (int ratio = 0; ratio < myRatioDoubles.Length; ratio++)
                                            {
                                                ((ArrayList)Blocks[currentBlock]).Add(myRatioDoubles[ratio]);

                                                //try
                                                //{
                                                //    if (!myRatios[ratio].Trim().Equals(""))
                                                //    {
                                                //        ((ArrayList)Blocks[currentBlock]).Add(Convert.ToDouble(myRatios[ratio].Trim()));
                                                //    }
                                                //}
                                                //catch (Exception e)
                                                //{// this gets rid of bogus empties and spaces
                                                //    Console.WriteLine("not a ratio: " + myRatios[ratio].Trim());
                                                //}
                                            }
                                        }
                         
									testLine = DatStream.ReadLine();
								}

                                // Jan 2010 thanks to Matt Rioux, it turns out that extra blank lines can appear before Grand summary
                                while (testLine.Trim() == "")
                                {
                                    testLine = DatStream.ReadLine();
                                }
								// check for "Grand" block of info to extract real names
								//testLine = DatStream.ReadLine();
								if ((FirstPassGrand) && (testLine.Substring(0, 7) == "\"Grand\""))
								{
									// we have labels, so may have to read a second line
									if (testLine.LastIndexOf("No Before") == -1)
										testLine = DatStream.ReadLine();
									for (int item = 0; item < FunctionNames.Count; item ++)
									{
										testLine = DatStream.ReadLine();
										string[] myNames = testLine.Split(new Char[]{'\"'});
										//string[] myNames = testLine.Split(new Char[]{' '});
										//FunctionNames[item] += ": " + myNames[1].Trim().Substring(1, myNames[1].Trim().Length - 2);
										FunctionNames[item] += ": " + myNames[3].Trim();

									}

									FirstPassGrand = false;
								}
								
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

			// new feb 2005 section to generate fake data from summaries when
			// no data exists
			// so we do not know from any row what all the functions are
			// we have to glean them as we go
			if (Blocks.Count == 0)
			{
				DatStream.Close();
				DatStream = new StreamReader(_DatFile.FullName);
				FirstPass = true;
				try
				{
					using (DatStream) 
					{
						String line;
						while ((line = DatStream.ReadLine()) != null)
						{
							if (line.StartsWith("\"Block\"\"Ratio\""))
							{
								int currentBlock = Blocks.Add(new ArrayList());

								if (line.LastIndexOf("No Before") == -1)
									line = DatStream.ReadLine(); // in case there is a text wrap
								
								// now we get the block  summary data and 
								// (on the firstpass)the function names
								while ((line = DatStream.ReadLine()) != "")
								{
									// nov 2005 another format of the static file appears
									// and we need to trap for it
									if (line.IndexOf("Exponential Correction used") > -1)
									{
										line = DatStream.ReadLine();
										line = DatStream.ReadLine();
										line = DatStream.ReadLine();// reads last before blank line
									}
									else
									{
									
										string[] myNames = line.Split(new Char[]{'\"'});

										string meanString = myNames[4].Substring(1, 16);
									
										if (!meanString.StartsWith("*****ERROR******"))
										{
											double mean = 0;
											try 
											{
												mean = Convert.ToDouble(meanString);
											}
											catch(Exception e){
                                                Console.WriteLine("meanString is bad: " + meanString);
                                            }
										
											// lets try just entering one reading per block
											((ArrayList)Blocks[currentBlock]).Add(mean);
										}
									
										if (FirstPass)
										{
											FunctionNames.Add(myNames[3].Trim());
										}
								
									}
								}
							
								FirstPass = false;

								
							}

						}
					}

				}
				catch(Exception enodata)
				{
					// Let the user know what went wrong.
					Console.WriteLine("There was a problem reading the file:");
					Console.WriteLine(enodata.Message);
					return null;
				}
			}
			
			if (Blocks.Count == 0) return null;

			// now we need to create double[] for each rawratio
			int ratioCount = FunctionNames.Count;
			// size of double[] is going to be number of blocks times size of first block / ratioCount
			ArrayList myDoubles = new ArrayList();
			// find largest Block size to use
			int myBlockSize = 0;
			for (int i = 0; i < Blocks.Count; i ++)
			{
				if (((ArrayList)Blocks[i]).Count > myBlockSize)
					myBlockSize = ((ArrayList)Blocks[i]).Count;
			}
			
			int myDoubleSize = (Blocks.Count) * myBlockSize / ratioCount;
			for (int ratio = 0; ratio < FunctionNames.Count; ratio ++)
			{
				int currentDouble = myDoubles.Add(new double[myDoubleSize]);
				for (int block = 0; block < Blocks.Count; block ++)
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

//						Console.WriteLine(ratio + "  " + block + "  " + num + "  " + next + "  " + ((double[])myDoubles[currentDouble])[next]);

					}

				}

				RawRatio myRR = new RawRatio((string)FunctionNames[ratio], (double[])myDoubles[currentDouble]);
				
                // repaired April 2007 in response to Matt in Sam's lab who noticed that
                // if the first block was short, all the other blocks were forced short
                // myRR.CyclesPerBlock = ((ArrayList)Blocks[0]).Count / ratioCount;
                myRR.CyclesPerBlock = myBlockSize / ratioCount;

                retval.Add(myRR);

                // July 2011 - a little more robust in case not determined from file header
                if (retval.RatioType.Equals(""))
                {
                    if (myRR.Name.Contains("20"))
                    {
                        retval.RatioType = "Pb";
                    }
                    if (myRR.Name.Contains("23"))
                    {
                        retval.RatioType = "U";
                    }
                }
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
