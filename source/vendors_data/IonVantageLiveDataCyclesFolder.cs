using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Tripoli.vendors_data
{
    /// <summary>
    /// Created July 2011 to handle new feature of IonVantage software that writes each cycle to a text file in 
    /// a specific folder "LiveData" whose location is specified by LiveDataStatus.txt, which the 
    /// user needs to specify for actual live data flow
    /// </summary>
    class IonVantageLiveDataCyclesFolder : MassSpecDataFile
    {
        DirectoryInfo liveDataFolder;
        FileInfo[] cycleTextFiles;

        public IonVantageLiveDataCyclesFolder(DirectoryInfo liveDataFolder)
        {
            this.liveDataFolder = liveDataFolder;
            cycleTextFiles = liveDataFolder.GetFiles("*-B*-C*.txt", SearchOption.TopDirectoryOnly);

            if (cycleTextFiles.Length > 0)
            {
                // use first file for datafileinfo
                DataFileInfo = cycleTextFiles[0];

                // order files by timestamp
                Array.Sort(cycleTextFiles, delegate(FileInfo f1, FileInfo f2)
                {
                    return f1.CreationTime.CompareTo(f2.CreationTime);
                });

            }
        }
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string TestFileValidity()
        {
            string isValid = "FALSE";

            if (cycleTextFiles.Length > 0)
            {
                StreamReader firstFileStream = new StreamReader(cycleTextFiles[0].FullName);

                string line = firstFileStream.ReadLine();
                if (line.IndexOf("Version") > -1)
                {
                    isValid = "TRUE";
                }

                firstFileStream.Close();
            }

            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override TripoliWorkProduct LoadRatios()
        {
            TripoliWorkProduct tripoliWorkproduct = new TripoliWorkProduct();

            // we have a set of files where each file contains single cycle data
            // the first file has the header info we need for reading the subsequent files
            StreamReader firstFileStream = new StreamReader(cycleTextFiles[0].FullName);

            // we know the content of each line, so just walk the file
            // skip the first 5 lines 
            string line = null;
            for (int i = 0; i < 6; i++)
            {
                line = firstFileStream.ReadLine();
            }

            // now line is of form 
            // Method,"C:\IonVantage Projects\Turrets.PRO\Data\ETsynzirc500Ma z_07-17-11_3 Pb2.RAW\ETsynzirc500Ma z_07-17-11_3 Pb2-3756"
            // extract sampleName and fractionName
            
            line = line.Replace("\"", "");
            int startOfSampleName = line.IndexOf(".RAW") + 5;
            string[] sampleFractionStuff = line.Substring(startOfSampleName).Trim().Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            tripoliWorkproduct.SampleName = sampleFractionStuff[0];
            tripoliWorkproduct.FractionName = sampleFractionStuff[1];

            // get timestamp for start of aquisition of form 
            // Acquire Date,Monday 18 July 2011 15:1:16
            line = firstFileStream.ReadLine();
            string dateString = line.Substring(line.IndexOf("Date") + 5);
            string[] dateParts = dateString.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string dayOfMonth = dateParts[1];
            string month = dateParts[2];
            string year = dateParts[3];
            string timeInfo = dateParts[4];
            string[] timeInfoDet = timeInfo.Split(new Char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            string hours = timeInfoDet[0].Trim();
            string minutes = timeInfoDet[1].Trim();
            string seconds = timeInfoDet[2].Trim();

            // build a date       
            tripoliWorkproduct.TimeStamp = new DateTime(//
                Convert.ToInt32(year), //
                Convert.ToInt32(Enum.Parse(typeof(myMonths), month)), //
                Convert.ToInt32(dayOfMonth), //
                Convert.ToInt32(hours), //
                Convert.ToInt32(minutes), //
                Convert.ToInt32(seconds));

            // get number of functions and/or ratios of form
            // Functions,14
            line = firstFileStream.ReadLine();
            int numberOfFunctions = Convert.ToInt32(line.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[1]);

            // skip 3 lines
            for (int i = 0; i < 3; i++)
            {
                line = firstFileStream.ReadLine();
            }

            // retrieve the function (ratio) names
            string[] functionNames = new string[numberOfFunctions];
            for (int i = 0; i < numberOfFunctions; i++)
            {
                line = firstFileStream.ReadLine();
                string functionName = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
                // strip out quote marks
                functionNames[i] = functionName.Replace("\"", "");
            }

            // the files are named serialNumber-blockNumber-cycleNumber.txt
            // as in 3756-B1-C1 and continue in order

            // thus the number of file names containing B1 EQUALS the cycles per block
            int cyclesPerBlock = liveDataFolder.GetFiles("*-B1-*.txt", SearchOption.TopDirectoryOnly).Length;

            // the number of blocks is revealed in the name of the newest file
            string lastCycleFileName = cycleTextFiles.Last().Name;
            int blockNumberStart = lastCycleFileName.IndexOf("-B") + 2;
            int blockNumberEnd = lastCycleFileName.IndexOf("-C");
            string blockCount = lastCycleFileName.Substring(blockNumberStart, blockNumberEnd - blockNumberStart);
            int countOfBlocks = Convert.ToInt32(blockCount);

            firstFileStream.Close();

            // now we are ready to read data into an array
            double[][] blocksOfCyclesByFunction = new double[numberOfFunctions][];
            for(int i = 0; i < numberOfFunctions; i ++){
                blocksOfCyclesByFunction[i] = new double[countOfBlocks * cyclesPerBlock];
            }

            // this function takes in the array of files and the data array and populates the dataArray 
            for (int f = 0; f < cycleTextFiles.Length; f++)
            {
                // walk the file until pass the line containing "#START"
                StreamReader cycleFileStream = new StreamReader(cycleTextFiles[f].FullName);
                string aLine = "";
                while (! aLine.StartsWith("#START"))
                {
                    aLine = cycleFileStream.ReadLine();
                }

                // extract data
                for (int i = 0; i < numberOfFunctions; i++)
                {
                    aLine = cycleFileStream.ReadLine();
                    string[] aLineData = aLine.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    blocksOfCyclesByFunction[i][f] = Convert.ToDouble(aLineData[2]);
                }

                cycleFileStream.Close();
            }

            // build ratios
            for (int i = 0; i < numberOfFunctions; i++)
            {
                RawRatio rawRatio = new RawRatio(functionNames[i], blocksOfCyclesByFunction[i]);
                rawRatio.CyclesPerBlock = cyclesPerBlock;
                tripoliWorkproduct.Add(rawRatio);

                // detect type
                if (tripoliWorkproduct.RatioType.Equals(""))
                {
                    if (rawRatio.Name.Contains("20"))
                    {
                        tripoliWorkproduct.RatioType = "Pb";
                    }
                    if (rawRatio.Name.Contains("23") || rawRatio.Name.Contains("26") || rawRatio.Name.Contains("27"))
                    {
                        tripoliWorkproduct.RatioType = "U";
                    }
                }
            }

            // force it to partial as we don't have an ending flag 
            tripoliWorkproduct.isPartialResult = true;

            return tripoliWorkproduct;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void close()
        {
        }

        #endregion Methods

    }
}
