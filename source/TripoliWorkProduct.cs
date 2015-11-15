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

using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

using Tripoli.earth_time_org;

using Wintellect.PowerCollections;


namespace Tripoli
{
    /// <summary>
    /// Wrapper class for Tripoli serialized workfile that is an arraylist of rawRatios
    /// </summary>
    [Serializable]
    public class TripoliWorkProduct : ArrayList, IComparable
    {
        // Fields
        private FileInfo _SourceFileInfo = null;
        private string _Notations = "Notations";

        // fields needed for fractionation correction march 2007
        private Tracer _CurrentTracer = null;
        private ArrayList _UraniumPairs = new ArrayList(); // elements = int[2] 0=238 index 1 = 233 index
        private ArrayList _UraniumPairsNames = new ArrayList();
        private ArrayList _UraniumPairsAlphaU = new ArrayList();

        private bool _ContainMeasuredUraniumForFractionationCorrection = false;
        private bool _ContainMeasuredPb202_205 = false;
        // oct 2009
        private bool _ContainMeasuredPb201_205andPb203_205 = false;

        private bool _AmFractionationCorrectable = false;
        private bool _AmFractionationCorrected = false;
        // oct 2009
        private bool _AmBaPO2andTIInterferenceCorrectable = false;
        private bool _AmBaPO2andTIInterferenceCorrected = false;

        // april 2007
        private double _ChauvenetsThreshold = Convert.ToDouble(Properties.Settings.Default.ChauvenetCriterion);
        private double _r238_235g = Convert.ToDouble(Properties.Settings.Default.r238_235g);

        // may 2007 oxide correction
        private ArrayList _UraniumOxidePairs = new ArrayList(); // elements = int[2] 0=270 index 1 = 267 index
        private ArrayList _UraniumOxidePairsNames = new ArrayList();
        private bool _AmOxideCorrected = false;

        private decimal _r18O_16O;// = Convert.ToDecimal(Properties.Settings.Default.r18O_16O);

        // April 2008 uSampleComponents for full U fractionation correction
        private USampleComponents _UsampleComponents;

        // October 2009 BariumPhospahate and Thalium interference corrections
        private BariumPhosphateIC _CurrentBariumPhosphateIC = null;
        private ThalliumIC _CurrentThalliumIC = null;

        // checkbox state holders for choices to use corrections
        private Boolean _UseTracerCorrection;// = true;
        private Boolean _UseInterferenceCorrection;// = true;

        // lead correction variables
        private decimal r202_205t;
        private decimal r201_205BaPO2;
        private decimal r202_205BaPO2;
        private decimal r203_205BaPO2;
        private decimal r204_205BaPO2;
        private decimal r203_205Tl;

        // Nov 2009 now detect these from text files
        private string sampleName;
        private string fractionName;
        private string ratioType;
        private bool partialResult;
        private DateTime timeStamp;



        // used to store default value needed for BaPO2Tl IC when no 202/205
        private decimal alphaPb;

        /// <summary>
        /// 
        /// </summary>
        public TripoliWorkProduct()
        {
            _SourceFileInfo = null;
            _Notations = "Notations";

            _CurrentTracer = null;
            _UraniumPairs = new ArrayList();
            _UraniumPairsNames = new ArrayList();
            _UraniumPairsAlphaU = new ArrayList();

            _ContainMeasuredUraniumForFractionationCorrection = false;
            _ContainMeasuredPb202_205 = false;
            _ContainMeasuredPb201_205andPb203_205 = false;
            _AmFractionationCorrectable = false;
            _AmFractionationCorrected = false;
            _AmBaPO2andTIInterferenceCorrectable = false;
            _AmBaPO2andTIInterferenceCorrected = false;

            _ChauvenetsThreshold = Convert.ToDouble(Properties.Settings.Default.ChauvenetCriterion);
            _r238_235g = Convert.ToDouble(Properties.Settings.Default.r238_235g);

            _UraniumOxidePairs = new ArrayList();
            _UraniumOxidePairsNames = new ArrayList();

            _UsampleComponents = new USampleComponents();
            _r18O_16O = Convert.ToDecimal(_UsampleComponents.DefaultR18O_16O);

            _CurrentBariumPhosphateIC = null;
            _CurrentThalliumIC = null;
            _UseTracerCorrection = false;
            _UseInterferenceCorrection = false;

            sampleName = "";
            fractionName = "";
            RatioType = "";
            isPartialResult = false;

            alphaPb = 0.0m;

        }

        /// <summary>
        /// Compares based on timestamp
        /// </summary>
        /// <param name="another"></param>
        /// <returns></returns>
        public Int32 CompareTo(object another)
        {
            return (this.timeStamp.CompareTo(((TripoliWorkProduct)another).timeStamp));
        }


        #region Properties

        public FileInfo SourceFileInfo
        {
            get { return _SourceFileInfo; }
            set { _SourceFileInfo = value; }
        }

        public string Notations
        {
            get { return _Notations; }
            set { _Notations = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Tripoli.earth_time_org.Tracer CurrentTracer
        {
            get { return _CurrentTracer; }
            set { _CurrentTracer = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ArrayList UraniumPairs
        {
            get { return _UraniumPairs; }
            set { _UraniumPairs = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public ArrayList UraniumPairsNames
        {
            get { return _UraniumPairsNames; }
            set { _UraniumPairsNames = value; }
        }

        public ArrayList UraniumPairsAlphaU
        {
            get { return _UraniumPairsAlphaU; }
            set { _UraniumPairsAlphaU = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Tripoli.earth_time_org.BariumPhosphateIC CurrentBariumPhosphateIC
        {
            get
            {
                return _CurrentBariumPhosphateIC;
            }

            set { _CurrentBariumPhosphateIC = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Tripoli.earth_time_org.ThalliumIC CurrentThalliumIC
        {
            get
            {
                return _CurrentThalliumIC;
            }

            set { _CurrentThalliumIC = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean UseTracerCorrection
        {
            get { return _UseTracerCorrection; }
            set { _UseTracerCorrection = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean UseInterferenceCorrection
        {
            get { return _UseInterferenceCorrection; }
            set { _UseInterferenceCorrection = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool ContainMeasuredPb202_205
        {
            get
            {
                if (!_ContainMeasuredPb202_205)
                {
                    for (int index = 0; index < Count; index++)
                    {
                        // assumption is 202/205
                        _ContainMeasuredPb202_205 = //
                            (_ContainMeasuredPb202_205 ||
                            ((RawRatio)this[index]).Name.Contains("202"));
                    }
                }
                return _ContainMeasuredPb202_205;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ContainMeasuredPb201_205andPb203_205
        {
            get
            {
                if (!_ContainMeasuredPb201_205andPb203_205)
                {
                    bool contains201 = false;
                    bool contains203 = false;
                    for (int index = 0; index < Count; index++)
                    {
                        // assumption is 201/205 and 203/205
                        contains201 = //
                            (contains201 || ((RawRatio)this[index]).Name.Contains("201"));
                        contains203 = //
                            (contains203 || ((RawRatio)this[index]).Name.Contains("203"));
                    }
                    _ContainMeasuredPb201_205andPb203_205 = (contains201 && contains203);
                } return _ContainMeasuredPb201_205andPb203_205;
            }
            set { _ContainMeasuredPb201_205andPb203_205 = value; }
        }

        /// <summary>
        /// This flag is set the first time Triploi opens this fraction and
        /// states whether fractionation correction can be performed
        /// </summary>
        public bool ContainMeasuredUraniumForFractionationCorrection
        {
            get
            {
                if (!AmFractionationCorrected)
                {
                    // THIS IS THE HARD WAY = bigo n*n
                    // walk the ratios and detect likely required uranium
                    UraniumPairs = new ArrayList();
                    UraniumPairsNames = new ArrayList();
                    int indexCount = -1;
                    for (int index = 0; index < Count; index++)
                    {
                        // Jan 2008
                        // modified to skip newly created 238/233 (see below)
                        // dec 2009
                        string tempName = ((RawRatio)this[index]).Name;
                        if (((RawRatio)this[index]).getSimpleName(tempName).Contains("238") &&
                            !((RawRatio)this[index]).getSimpleName(tempName).Contains("238/233"))//   /235"))

                        //if (((RawRatio)this[index]).Name.Contains("238") &&
                        //    !((RawRatio)this[index]).Name.Contains("238/233"))//   /235"))
                        {
                            // jan 2008 modified to three elements for the case of 238/233
                            UraniumPairs.Add(new int[] { index, -1, -1 });// -1 is placeholder
                            UraniumPairsNames.Add(new string[] { ((RawRatio)this[index]).Name, "", "" });

                            indexCount += 1;
                            ((RawRatio)this[index]).amUranium = true;

                            // now search all for the missing half of the pair
                            // first determine its name

                            // to avoid possible function names etc that may precede the
                            // ratio name, we strip out the string before the ratio name

                            string NameOf238 =
                                ((RawRatio)this[index]).Name
                                .Substring(((RawRatio)this[index]).Name.IndexOf("238"));

                            // jan 2008 now check for oxidecor:
                            int indexOC = NameOf238.IndexOf("OxideCor:");
                            if (indexOC == -1)
                                indexOC = NameOf238.Length;
                            else
                                indexOC += 9; // length of string "oxidecor:"
                            NameOf238 = NameOf238.Substring(0, indexOC);

                            string NameOf233 = NameOf238.Replace("238", "233");

                            // need to check for identical oxidecorrection status
                            // as duplicate names may exist
                            for (int index2 = 0; index2 < Count; index2++)
                            {
                                if (((RawRatio)this[index2]).Name.Contains(NameOf233)
                                    &&
                                    ((RawRatio)this[index]).OxidationCorrected
                                    .Equals(((RawRatio)this[index2]).OxidationCorrected))
                                {
                                    ((int[])UraniumPairs[indexCount])[1] = index2;
                                    ((string[])UraniumPairsNames[indexCount])[1] =
                                        ((RawRatio)this[index2]).Name;
                                    ((RawRatio)this[index2]).amUranium = true;

                                    // jan 2008
                                    // assume that if these are oxidecorrected, that the calculated
                                    // ratio 238/233 will have been inserted immediately after
                                    // the ratio265_267oxidecorrected at index265 (233)
                                    // see PerformOxideCorrection()
                                    if ((((RawRatio)this[index]).OxidationCorrected) &&
                                        (((RawRatio)this[index2]).OxidationCorrected))
                                    {
                                        ((int[])UraniumPairs[indexCount])[2] = index2 + 1;
                                        ((string[])UraniumPairsNames[indexCount])[2] =
                                            ((RawRatio)this[index2 + 1]).Name;
                                        ((RawRatio)this[index2 + 1]).amUranium = true;
                                    }

                                    break;
                                }
                            }
                        }
                    }
                    // march 2008                  CurrentTracer = null;
                    _ContainMeasuredUraniumForFractionationCorrection = indexCount > -1;// true;
                }

                return _ContainMeasuredUraniumForFractionationCorrection;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool AmFractionationCorrectable
        {
            // revised March 2008 for both Pb and U fractionation correction 
            // under certain tracers
            // revised October 2009 to handle BaPO2 and TI ==> see note below = no change

            get
            {
                // case lead
                bool leadResult =
                    (ContainMeasuredPb202_205 || ContainMeasuredPb201_205andPb203_205);

                bool uraniumResult =
                    ContainMeasuredUraniumForFractionationCorrection;

                _AmFractionationCorrectable = leadResult || uraniumResult;

                return _AmFractionationCorrectable;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AmFractionationCorrected
        {
            get { return _AmFractionationCorrected; }
            set { _AmFractionationCorrected = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AmBaPO2andTIInterferenceCorrectable
        {
            get
            {
                _AmBaPO2andTIInterferenceCorrectable = //
                    ContainMeasuredPb201_205andPb203_205;
                return _AmBaPO2andTIInterferenceCorrectable;
            }

            set { _AmBaPO2andTIInterferenceCorrectable = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AmBaPO2andTIInterferenceCorrected
        {
            get { return _AmBaPO2andTIInterferenceCorrected; }
            set { _AmBaPO2andTIInterferenceCorrected = value; }
        }



        /// <summary>
        /// 
        /// </summary>
        public double ChauvenetsThreshold
        {
            get { return _ChauvenetsThreshold; }
            set { _ChauvenetsThreshold = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double r238_235g
        {
            get { return _r238_235g; }
            set { _r238_235g = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ArrayList UraniumOxidePairs
        {
            get { return _UraniumOxidePairs; }
            set { _UraniumOxidePairs = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ArrayList UraniumOxidePairsNames
        {
            get { return _UraniumOxidePairsNames; }
            set { _UraniumOxidePairsNames = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool AmOxideCorrected
        {
            get { return _AmOxideCorrected; }
            set { _AmOxideCorrected = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal r18O_16O
        {
            get { return _r18O_16O; }
            set { _r18O_16O = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public USampleComponents UsampleComponents
        {
            get { return _UsampleComponents; }
            set { _UsampleComponents = value; }
        }


        /// <summary>
        /// This field will be moved to here once new dotnet allows modification of serialized data
        /// </summary>
        public virtual DirectoryInfo FolderInfo
        {
            get { return null; }
            set { }
        }

        public virtual SortedList AllRatioHistories
        {
            get { return null; }
            set { }
        }
        public virtual int RejectLevelSigma
        {
            get { return 0; }
            set { }
        }

        public string SampleName
        {
            get { return sampleName; }
            set { sampleName = value; }
        }

        public string FractionName
        {
            get { return fractionName; }
            set { fractionName = value; }
        }

        public string RatioType
        {
            get { return ratioType; }
            set { ratioType = value; }
        }

        public bool isPartialResult
        {
            get { return partialResult; }
            set { partialResult = value; }
        }

        public decimal AlphaPb
        {
            get { return alphaPb; }
            set { alphaPb = value; }
        }

        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }

        #endregion Properties

        #region Serialization and deserialization

        /// <summary>
        /// 
        /// </summary>
        /// <param name="si"></param>
        /// <param name="context"></param>
        protected TripoliWorkProduct(SerializationInfo si, StreamingContext context)
        {
            _SourceFileInfo = (FileInfo)si.GetValue("SourceFileInfo", typeof(FileInfo));
            _Notations = (string)si.GetValue("Notations", typeof(string));

            try
            {
                _CurrentTracer = (Tripoli.earth_time_org.Tracer)si.GetValue("CurrentTracer", typeof(Tripoli.earth_time_org.Tracer));
            }
            catch
            {
                /* Could alert user that a data file from an older version will be upgraded here*/
                _CurrentTracer = null;
            }

            try
            {
                _UraniumPairs = (ArrayList)si.GetValue("UraniumPairs", typeof(ArrayList));
            }
            catch {/* Could alert user that a data file from an older version will be upgraded here*/ }

            try
            {
                _UraniumPairsNames = (ArrayList)si.GetValue("UraniumPairsNames", typeof(ArrayList));
            }
            catch {/* Could alert user that a data file from an older version will be upgraded here*/ }

            try
            {
                _UraniumPairsAlphaU = (ArrayList)si.GetValue("UraniumPairsNames", typeof(ArrayList));
            }
            catch {/* Could alert user that a data file from an older version will be upgraded here*/ }

            try
            {
                _ContainMeasuredUraniumForFractionationCorrection = (bool)si.GetValue("AmPreProcessedForFractionationCorrection", typeof(bool));
            }
            catch {/* Could alert user that a data file from an older version will be upgraded here*/ }

            // added oct 2009 refactoring to handle bariumphosphate and thallium corrections
            try
            {
                _ContainMeasuredPb202_205 = (bool)si.GetValue("AmPreProcessedForPb202FractionationCorrection", typeof(bool));
            }
            catch {/* Could alert user that a data file from an older version will be upgraded here*/ }

            try
            {
                _AmFractionationCorrectable = (bool)si.GetValue("AmFractionationCorrectable", typeof(bool));
            }
            catch {/* Could alert user that a data file from an older version will be upgraded here*/ }

            try
            {
                _AmFractionationCorrected = (bool)si.GetValue("AmFractionationCorrected", typeof(bool));
            }
            catch {/* Could alert user that a data file from an older version will be upgraded here*/ }

            try
            {
                _ChauvenetsThreshold = (double)si.GetValue("ChauvenetsThreshold", typeof(double));
            }
            catch {/* Could alert user that a data file from an older version will be upgraded here*/ }

            try
            {
                _r238_235g = (double)si.GetValue("r238_235g", typeof(double));
            }
            catch {/* Could alert user that a data file from an older version will be upgraded here*/ }

            try
            {
                _UraniumOxidePairs = (ArrayList)si.GetValue("UraniumOxidePairs", typeof(ArrayList));
            }
            catch {/* Could alert user that a data file from an older version will be upgraded here*/ }

            try
            {
                _UraniumOxidePairsNames = (ArrayList)si.GetValue("UraniumOxidePairsNames", typeof(ArrayList));
            }
            catch {/* Could alert user that a data file from an older version will be upgraded here*/ }

            try
            {
                _AmOxideCorrected = (bool)si.GetValue("AmOxideCorrected", typeof(bool));
            }
            catch {/* Could alert user that a data file from an older version will be upgraded here*/ }


            try
            {
                _r18O_16O = (decimal)si.GetValue("r18O_16O", typeof(decimal));
            }
            catch {/* Could alert user that a data file from an older version will be upgraded here*/ }

            try
            {
                _UsampleComponents = (USampleComponents)si.GetValue("UsampleComponents", typeof(USampleComponents));
            }
            catch {/* Could alert user that a data file from an older version will be upgraded here*/ }

            try
            {
                _CurrentBariumPhosphateIC = (Tripoli.earth_time_org.BariumPhosphateIC)si.GetValue("CurrentBariumPhosphateIC", typeof(Tripoli.earth_time_org.BariumPhosphateIC));
            }
            catch
            {
                /* Could alert user that a data file from an older version will be upgraded here*/
                _CurrentBariumPhosphateIC = null;
            }

            try
            {
                _CurrentThalliumIC = (Tripoli.earth_time_org.ThalliumIC)si.GetValue("CurrentThalliumIC", typeof(Tripoli.earth_time_org.ThalliumIC));
            }
            catch
            {
                /* Could alert user that a data file from an older version will be upgraded here*/
                _CurrentThalliumIC = null;
            }
            try
            {
                _UseTracerCorrection = (bool)si.GetValue("UseTracerCorrection", typeof(bool));
            }
            catch
            {
                /* Could alert user that a data file from an older version will be upgraded here*/
                _UseTracerCorrection = false;
            }
            try
            {
                _UseInterferenceCorrection = (bool)si.GetValue("UseInterferenceCorrection", typeof(bool));
            }
            catch
            {
                /* Could alert user that a data file from an older version will be upgraded here*/
                _UseInterferenceCorrection = false;
            }

            try
            {
                sampleName = (string)si.GetValue("sampleName", typeof(string));
            }
            catch (Exception)
            {
                sampleName = "";
            }
            try
            {
                fractionName = (string)si.GetValue("fractionName", typeof(string));
            }
            catch (Exception)
            {
                fractionName = "";
            }
            try
            {
                ratioType = (string)si.GetValue("ratioType", typeof(string));
            }
            catch (Exception)
            {
                ratioType = "";
            }
            try
            {
                partialResult = (bool)si.GetValue("partialResult", typeof(bool));
            }
            catch (Exception)
            {
                partialResult = false;
            }

            try
            {
                alphaPb = (decimal)si.GetValue("alphaPb", typeof(decimal));
            }
            catch (Exception)
            {
                alphaPb = 0.0m;
            }

            try
            {
                timeStamp = (DateTime)si.GetValue("timeStamp", typeof(DateTime));
            }
            catch (Exception)
            {
                timeStamp = new DateTime();
            }
        }

        [OnDeserializing()]
        internal void OnDeserializedMethod(StreamingContext context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="si"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo si, StreamingContext context)
        {
            si.AddValue("SourceFileInfo", _SourceFileInfo, typeof(FileInfo));
            si.AddValue("Notations", _Notations, typeof(string));
            si.AddValue("CurrentTracer", _CurrentTracer, typeof(Tracer));
            si.AddValue("UraniumPairs", _UraniumPairs, typeof(ArrayList));
            si.AddValue("UraniumPairsNames", _UraniumPairsNames, typeof(ArrayList));
            si.AddValue("UraniumPairsAlphaU", _UraniumPairsAlphaU, typeof(ArrayList));
            si.AddValue("AmPreProcessedForFractionationCorrection", _ContainMeasuredUraniumForFractionationCorrection, typeof(bool));
            si.AddValue("AmPreProcessedForPbFractionationCorrection", _ContainMeasuredPb202_205, typeof(bool));
            // added oct 2009 as part of refactoring and adding in bariumphospahate and thalium corrections
            si.AddValue("AmPreProcessedForPb202FractionationCorrection", _ContainMeasuredPb202_205, typeof(bool));
            si.AddValue("AmFractionationCorrectable", _AmFractionationCorrectable, typeof(bool));
            si.AddValue("AmFractionationCorrected", _AmFractionationCorrected, typeof(bool));
            si.AddValue("ChauvenetsThreshold", _ChauvenetsThreshold, typeof(double));
            si.AddValue("r238_235g", _r238_235g, typeof(double));
            si.AddValue("UraniumOxidePairs", _UraniumOxidePairs, typeof(ArrayList));
            si.AddValue("UraniumOxidePairsNames", _UraniumOxidePairsNames, typeof(ArrayList));
            si.AddValue("AmOxideCorrected", _AmOxideCorrected, typeof(bool));
            si.AddValue("r18O_16O", _r18O_16O, typeof(decimal));
            si.AddValue("UsampleComponents", _UsampleComponents, typeof(USampleComponents));
            si.AddValue("CurrentBariumPhosphateIC", _CurrentBariumPhosphateIC, typeof(BariumPhosphateIC));
            si.AddValue("CurrentThalliumIC", _CurrentThalliumIC, typeof(ThalliumIC));
            si.AddValue("UseTracerCorrection", _UseTracerCorrection, typeof(bool));
            si.AddValue("UseInterferenceCorrection", _UseInterferenceCorrection, typeof(bool));
            si.AddValue("sampleName", sampleName, typeof(string));
            si.AddValue("fractionName", fractionName, typeof(string));
            si.AddValue("ratioType", ratioType, typeof(string));
            si.AddValue("partialResult", partialResult, typeof(bool));
            si.AddValue("alphaPb", alphaPb, typeof(decimal));
            si.AddValue("timeStamp", timeStamp, typeof(DateTime));
        }

        #endregion Serialization and deserialization

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static TripoliWorkProduct GetTripoliHistoryFile(string fileName)
        {
            TripoliWorkProduct retval = null;
            Stream stream = null;

            try
            {
                stream = File.Open(fileName, FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();

                retval = (TripoliWorkProduct)bformatter.Deserialize(stream);
            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.Message);
                MessageBox.Show("Failed to open Tripoli History File !", "Tripoli Warning");
            }
            try
            {
                stream.Close();
            }
            catch { }

            return retval;
        }

        public int getCountActiveFiles()
        {
            int retval = 0;

            for (int file = 0; file < this.Count; file++)
            {
                if (((TripoliDetailFile)this[file]).IsActive)
                {
                    retval++;
                }
            }
            return retval;
        }

        public virtual void setFileIsActive(int index, bool isActive)
        { }

        /// <summary>
        /// 
        /// </summary>
        public void PrepareGainsCollectors()
        {
            // sort ourself by init date extracted from excel file
            Sort(new TripoliDetailFile.DateOrderClass());

            // oct 2005 rewrite this to handle the problem that both gains folders
            // and later standards folders may contain files whose set  of
            // collectors or ratios is not identical across each file (as originally assumed

            // AllRatioHistories is a sorted list of TripoliRatioHistories
            // clear member ratios
            for (int index = 0; index < AllRatioHistories.Count; index++)
            {
                ((TripoliDetailHistory)AllRatioHistories.GetByIndex(index)).MemberRatios =
                    new ArrayList();
            }

            // set up dummy array = this will be fixed for hard coded count ***********
            double[] dumArray = new double[1];
            for (int i = 0; i < dumArray.Length; i++) dumArray[i] = (double)0;

            // step through the files in the folder and add new collectors when found
            int activeFilesCount = 0;
            for (int file = 0; file < this.Count; file++)
            {
                //if (((GainsFile)this[file]).IsActive)
                //{
                activeFilesCount++;

                // run through collectors and add rawratios to AllRatioHistories
                for (int collector = 0;
                    collector < ((TripoliDetailFile)this[file]).Collectors.Count;
                    collector++)
                {
                    //RawRatio tempRR = (RatioHistory)((GainsFile)this[file]).Collectors[collector];
                    RawRatio tempRR = (RawRatio)((TripoliDetailFile)this[file]).Collectors[collector];

                    // note serialization does not work if TripoliDetailHistory is Icomparable = BUG
                    // or inside the try block
                    TripoliDetailHistory tempTRH = new TripoliDetailHistory(tempRR.Name);

                    try
                    {
                        AllRatioHistories.Add(tempTRH.Name, tempTRH);
                    }
                    catch { }

                    //int index = AllRatioHistories.IndexOfKey(tempRR.Name);
                    int index = AllRatioHistories.IndexOfKey(tempTRH.Name);
                    // insert the dummy enough times to make up for missing file data 
                    int saveCount = ((TripoliDetailHistory)AllRatioHistories.GetByIndex(index)).MemberRatios.Count;
                    //if (saveCount > 11)
                    //	Console.WriteLine("HELP");
                    for (int dummy = 0; dummy < (activeFilesCount - 1 - saveCount); dummy++)
                    {
                        // dummy has to be unique object
                        RawRatio dummyRR = new RawRatio(tempRR.Name, dumArray);
                        ((TripoliDetailHistory)AllRatioHistories.GetByIndex(index)).MemberRatios.Add(dummyRR);
                    }

                    // now add the current ratio
                    ((TripoliDetailHistory)AllRatioHistories.GetByIndex(index)).MemberRatios.Add(tempRR);
                }
                //}
            }

            // now run through the collectors and fill them out to the end if they do not have 
            // a ratio for every file
            for (int collector = 0; collector < AllRatioHistories.Count; collector++)
            {
                int saveCount
                    = ((TripoliDetailHistory)AllRatioHistories.GetByIndex(collector)).MemberRatios.Count;

                for (int dummy = 0; dummy < (activeFilesCount - saveCount); dummy++)
                {
                    RawRatio dummyRR =
                        new RawRatio(((TripoliDetailHistory)AllRatioHistories.GetByIndex(collector)).Name, dumArray);

                    ((TripoliDetailHistory)AllRatioHistories.GetByIndex(collector)).MemberRatios.Add(dummyRR);
                }
            }

            // now go through and get maximum ratiocount for the collector
            for (int collector = 0; collector < AllRatioHistories.Count; collector++)
            {
                ((TripoliDetailHistory)AllRatioHistories.GetByIndex(collector)).setMaxRatioCount();
            }

            // initialize self for all not chosen = inactive 
            for (int item = 0; item < this.Count; item++)
            {
                ((TripoliDetailFile)this[item]).IsActive = false;

                setFileIsActive(item, false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphNumber"></param>
        /// <returns></returns>
        public RawRatio GainsHistory(int graphNumber)
        {
            // for this graph walk through active memberratios of AllRatioHistories and update the
            // DetailHistory of the specific DetailHistory


            // now get info about GainsFiles for display on every graph
            // GaisFileInfo will contain an Arraylist about each file at
            // each element = { bool = true(I am showing normally; false=I am selected by mouse)
            // , who I am name and date, 
            //	and now dec 2005 the index into the list of files}
            ArrayList GainsFilesInfo = new ArrayList();


            int dataCount =
                ((RawRatio)((TripoliDetailHistory)AllRatioHistories.GetByIndex(graphNumber))
                .DetailHistory).CyclesPerBlock;

            //int count = getCountActiveFiles();
            //double [] allData = new double[count * dataCount];//allow for extra slot for mean of file
            // dec 2005 use arraylist until done
            ArrayList allDataVector = new ArrayList();

            //int iCounter = 0;
            for (int iFile = 0;
                iFile < ((TripoliDetailHistory)AllRatioHistories.GetByIndex(graphNumber))
                .MemberRatios.Count; iFile++)
            {
                RawRatio tempRR =
                    ((RawRatio)((TripoliDetailHistory)AllRatioHistories
                    .GetByIndex(graphNumber)).MemberRatios[iFile]);

                if (tempRR.IsActive)
                {
                    // build tempWorkProduct array to get mean
                    double[] tempData = new double[dataCount - 1];

                    tempRR.CalcStats();

                    for (int gain = 0; gain < (dataCount - 1); gain++)
                    {
                        // test for less than dataCount ratios and force them to 0
                        if (tempRR.Ratios.Length <= (gain))
                            tempData[gain] = 0;
                        else
                        {
                            // test if data point is within tolerance
                            if (tempRR.IsDataPointInTolerance(tempRR.Ratios[gain], RejectLevelSigma))
                            {
                                tempData[gain] = tempRR.Ratios[gain];
                            }
                            else
                            {
                                tempData[gain] = 0;
                            }

                        }

                        //allData[iCounter * dataCount + gain] = tempData[gain];
                        allDataVector.Add(tempData[gain]);
                    }
                    // build tempWorkProduct rawratio to get mean
                    RawRatio tempRR2 = new RawRatio("tempWorkProduct", tempData);
                    tempRR2.CyclesPerBlock = dataCount - 1;
                    tempRR2.CalcStats();

                    // store the mean of the individual file as a negative number whose location we know
                    //allData[iCounter * dataCount + tempRR2.CyclesPerBlock] = -1 * tempRR2.Mean;
                    allDataVector.Add(-1 * tempRR2.Mean);

                    //iCounter ++;


                    ArrayList tempNode = new ArrayList();
                    tempNode.Add(true);
                    tempNode.Add(
                        ((TripoliDetailFile)this[iFile]).FileName
                        + "\r\n"
                        + ((TripoliDetailFile)this[iFile])
                        .InitDate.ToString("g"));
                    // dec 2005 add in index to file list
                    tempNode.Add(iFile);
                    GainsFilesInfo.Add(tempNode);
                }
            }

            // DetailHistory is a rawratio that contains the data from each member ratio
            //((TripoliDetailHistory)AllRatioHistories.GetByIndex(graphNumber))
            //	.DetailHistory.Ratios = allData;

            // dec 2005 refactoring by using arryalist and then converting
            ((TripoliDetailHistory)AllRatioHistories.GetByIndex(graphNumber))
                .DetailHistory.Ratios = (double[])allDataVector.ToArray(typeof(double));

            // store GainsFileInfo with DetailHistory
            ((DetailHistory)((TripoliDetailHistory)AllRatioHistories.GetByIndex(graphNumber))
                .DetailHistory).GainsFilesInfo = GainsFilesInfo;

            return ((TripoliDetailHistory)AllRatioHistories.GetByIndex(graphNumber)).DetailHistory;
        }

        /// <summary>
        /// March 2008: includes both Pb and U.
        /// Uses the first occurring of each of 233/235 and 238/235.
        /// </summary>
        public void PerformFractionationCorrection(//
            Tracer tracer, //
            BariumPhosphateIC bariumPhosphateIC, //
            ThalliumIC thalliumIC)
        {
            CurrentTracer = tracer;
            if (UseInterferenceCorrection)
            {
                CurrentBariumPhosphateIC = bariumPhosphateIC;
                CurrentThalliumIC = thalliumIC;
            }
            else
            {
                CurrentBariumPhosphateIC = null;
                CurrentThalliumIC = null;
            }

            AmFractionationCorrected = false;
            ArrayList correctedRatios = null;
            ArrayList alphaUPlusOneRatios = null;

            if ((CurrentTracer != null))// && !AmFractionationCorrected) nov 2009 see 3 lines above
            {
                correctedRatios = new ArrayList();
                alphaUPlusOneRatios = new ArrayList();

                if (ContainMeasuredUraniumForFractionationCorrection)
                {
                    decimal molU238b = 0.0m;
                    decimal molU235b = 0.0m;
                    decimal molU235t = 0.0m;
                    decimal molU233t = 0.0m;
                    decimal molU238t = 0.0m;

                    // modified Nov 2009 to limit automatic appearance of usample form to cases where it is the first time
                    bool showUsampleComponentsForm = //
                       ((UsampleComponents.labUBlankMass //
                       * UsampleComponents.Gmol235 //
                       * UsampleComponents.Gmol238  //
                       * UsampleComponents.tracerMass //
                       * UsampleComponents.r238_235b //
                       * UsampleComponents.r238_235s) == 0.0m);
                    bool proceedWithCorrection = true;

                    // URANIUM   Tracer type test
                    string tracerType = CurrentTracer.tracerType;

                    if ((tracerType.CompareTo("mixed 202-205-233-235") == 0) ||
                        (tracerType.CompareTo("mixed 205-233-235") == 0))
                    {
                        if (showUsampleComponentsForm)
                        {
                            frmUsampleComponents myUsampleComponents = null;
                            myUsampleComponents = new frmUsampleComponents(UsampleComponents);
                            myUsampleComponents.ShowDialog();

                            if ((UsampleComponents.tracerMass * UsampleComponents.labUBlankMass) == 0.0m)
                            {
                                MessageBox.Show(
                                    "Please replace values of zero in the current U sample components \n\n"
                                    + "with positive numbers. \n\n",
                                    "Tripoli Warning");
                                proceedWithCorrection = false;
                            }
                        }

                        if (proceedWithCorrection)
                        {
                            try
                            {
                                molU238b =
                                    UsampleComponents.labUBlankMass * (decimal)Math.Pow(10.0, -12.0) * //
                                    UsampleComponents.r238_235b / //
                                    (UsampleComponents.r238_235b + 1.0m) / //
                                    UsampleComponents.Gmol238;

                                molU235b =
                                    UsampleComponents.labUBlankMass * (decimal)Math.Pow(10.0, -12.0) * //
                                    1.0m / //
                                    (UsampleComponents.r238_235b + 1.0m) / //
                                    UsampleComponents.Gmol235;

                                molU235t =
                                    CurrentTracer.getIsotopeConcByName("concU235t").value * //
                                    UsampleComponents.tracerMass;

                                molU233t =
                                     CurrentTracer.getRatioByName("r233_235t").value * //
                                     molU235t;

                                molU238t =
                                     CurrentTracer.getRatioByName("r238_235t").value * //
                                     molU235t;
                            }
                            catch (Exception e)
                            {
                                proceedWithCorrection = false;
                            }
                        }
                    }
                    else if ((tracerType.CompareTo("mixed 202-205-233-236") == 0) ||
                                (tracerType.CompareTo("mixed 205-233-236") == 0))
                    {
                        // Notify user of unsupported tracer type
                        MessageBox.Show(
                            "Tripoli does not yet support "
                            + "Tracer type \n\n"
                            + CurrentTracer.tracerType
                            + "\n\n for U fractionation correction.",
                            "Tripoli Warning");

                        CurrentTracer = null;
                        CurrentBariumPhosphateIC = null;
                        CurrentThalliumIC = null;
                        AmBaPO2andTIInterferenceCorrected = false;
                        AmFractionationCorrected = false;
                        proceedWithCorrection = false;
                    }

                    else
                    {
                        // Notify user of wrong tracer type
                        MessageBox.Show(
                            "Tracer type \n\n"
                            + CurrentTracer.tracerType
                            + "\n\n does not support U fractionation correction.",
                            "Tripoli Warning");

                        CurrentTracer = null;
                        CurrentBariumPhosphateIC = null;
                        CurrentThalliumIC = null;
                        AmBaPO2andTIInterferenceCorrected = false;
                        AmFractionationCorrected = false;
                        proceedWithCorrection = false;

                    }

                    if (proceedWithCorrection)
                    {
                        // walk the uranium pairs and build and save corrected ratios
                        UraniumPairsAlphaU = new ArrayList();

                        foreach (object Upair in UraniumPairs)
                        {
                            int Index238 = ((int[])Upair)[0];
                            int Index233 = ((int[])Upair)[1];

                            // jan 2008
                            int Index238_233 = -1;
                            try
                            {
                                Index238_233 = ((int[])Upair)[2];
                            }
                            catch
                            {
                                // backwards compatible for length 3
                                Index238_233 = -1;
                            }

                            // check for missing 233
                            if (Index233 == -1) break;

                            // restore the data from which we copy
                            ((RawRatio)this[Index233]).RestoreData();
                            ((RawRatio)this[Index238]).RestoreData();
                            if (Index238_233 > -1)
                                ((RawRatio)this[Index238_233]).RestoreData();

                            // copy these restored ratios in preparation for the fractionation correction
                            double[] r233_235fc_Ratios = new double[((RawRatio)this[Index233]).Ratios.Length];
                            double[] r238_235fc_Ratios = new double[((RawRatio)this[Index238]).Ratios.Length];
                            double[] r238_233fc_Ratios = null;
                            if (Index238_233 > -1)
                                r238_233fc_Ratios = new double[((RawRatio)this[Index238_233]).Ratios.Length];

                            Array.Copy(((RawRatio)this[Index233]).Ratios,
                                r233_235fc_Ratios,
                                ((RawRatio)this[Index233]).Ratios.Length);

                            Array.Copy(((RawRatio)this[Index238]).Ratios,
                                r238_235fc_Ratios,
                                ((RawRatio)this[Index238]).Ratios.Length);

                            if (Index238_233 > -1)
                                Array.Copy(((RawRatio)this[Index238_233]).Ratios,
                                    r238_233fc_Ratios,
                                    ((RawRatio)this[Index238_233]).Ratios.Length);

                            // create the alpha arrays for the new ratios May 2007
                            decimal[] r233_235fc_Alpha = new decimal[((RawRatio)this[Index233]).Ratios.Length];
                            decimal[] r238_235fc_Alpha = new decimal[((RawRatio)this[Index238]).Ratios.Length];
                            decimal[] r238_233fc_Alpha;
                            if (Index238_233 > -1)
                                r238_233fc_Alpha = new decimal[((RawRatio)this[Index238_233]).Ratios.Length];
                            double[] onePlusAlpha = new double[((RawRatio)this[Index238]).Ratios.Length];

                            // fractionation correction
                            if (r238_235fc_Ratios.Length != r233_235fc_Ratios.Length)
                                MessageBox.Show("Data counts do not match for \n\n"
                                                + ((RawRatio)this[Index238]).Name
                                                + "\n\n and\n\n"
                                                + ((RawRatio)this[Index233]).Name
                                                + "\n\n PROCEEDING ANYWAY !!",
                                                "Tripoli Warning");

                            decimal sumAlphaU = 0.0m;
                            for (int index = 0; index < Math.Min(r233_235fc_Ratios.Length, r238_235fc_Ratios.Length); index++)
                            {
                                decimal r233_235t = CurrentTracer.getRatioByName("r233_235t").value;
                                decimal r238_235t = CurrentTracer.getRatioByName("r238_235t").value;

                                decimal alphaU = 0.0m;

                                if (((decimal)r233_235fc_Ratios[index] * //
                                    (decimal)r238_235fc_Ratios[index]) > 0.0m)
                                {

                                    if ((tracerType.CompareTo("mixed 202-205-233-235") == 0) ||
                                        (tracerType.CompareTo("mixed 205-233-235") == 0))
                                    {
                                        // Calculate AlphaU for Case 1 - april 2008
                                        decimal term1 =
                                            UsampleComponents.r238_235s * //
                                            (molU233t - (decimal)r233_235fc_Ratios[index] * //
                                            (molU235b + molU235t));

                                        decimal term2 =
                                            (decimal)r238_235fc_Ratios[index] * //
                                            molU233t;

                                        decimal term3 =
                                            (decimal)r233_235fc_Ratios[index] * //
                                            (molU238b + molU238t);

                                        decimal term4 =
                                            3.0m * //
                                            (decimal)r238_235fc_Ratios[index] * //
                                            molU233t;

                                        decimal term5 =
                                            2.0m * //
                                            (decimal)r233_235fc_Ratios[index] * //
                                            (molU238b + //
                                            molU238t - //
                                            UsampleComponents.r238_235s * //
                                            (molU235b + molU235t));

                                        try
                                        {
                                            alphaU =
                                            (term1 - term2 + term3) / //
                                            (term4 + term5);
                                        }
                                        catch (Exception e)
                                        {
                                        }


                                        AmFractionationCorrected = true;

                                    }
                                    else if ((tracerType.CompareTo("mixed 202-205-233-236") == 0) ||
                                        (tracerType.CompareTo("mixed 205-233-236") == 0))
                                    {
                                        // Calculate AlphaU for Case 2 - april 2008
                                    }

                                    if (AmFractionationCorrected)
                                    {
                                        // store alpha
                                        r233_235fc_Alpha[index] = alphaU;
                                        r238_235fc_Alpha[index] = alphaU;
                                        if (Index238_233 > -1)
                                            r233_235fc_Alpha[index] = alphaU;

                                        onePlusAlpha[index] = 1.0 + (double)alphaU;

                                        sumAlphaU += alphaU;

                                        // produce correction
                                        r233_235fc_Ratios[index] *= (1.0 - 2.0 * (double)alphaU);
                                        r238_235fc_Ratios[index] *= (1.0 + 3.0 * (double)alphaU);
                                        if (Index238_233 > -1)
                                            r238_233fc_Ratios[index] *= (1.0 + 5.0 * (double)alphaU);

                                    }
                                }
                            }

                            // save off mean alphau for each pair
                            // Note there are two meanAlphaU - one for all the data that is 
                            // stored with the TripoliWorkProduct in UraniumPairsAlphaU
                            // and another that is calculated on the fly for each ratio depending
                            // on which data are selected...this latter is output to the fraction for Redux
                            decimal meanAlphaU =
                                sumAlphaU / Math.Min(r233_235fc_Ratios.Length, r238_235fc_Ratios.Length);
                            UraniumPairsAlphaU.Add(meanAlphaU);

                            RawRatio r233_235fc = new RawRatio(
                                ((RawRatio)this[Index233]).Name, r233_235fc_Ratios);
                            r233_235fc.Alpha = r233_235fc_Alpha;
                            r233_235fc.CyclesPerBlock = ((RawRatio)this[Index233]).CyclesPerBlock;
                            r233_235fc.fractionationCorrected = true;
                            // July 2007 set oxide corrected for new
                            r233_235fc.OxidationCorrected = ((RawRatio)this[Index233]).OxidationCorrected;

                            // not set only get                    r233_235fc.MeanAlpha = meanAlphaU;

                            RawRatio r238_235fc = new RawRatio(
                                ((RawRatio)this[Index238]).Name, r238_235fc_Ratios);
                            r238_235fc.Alpha = r238_235fc_Alpha;
                            r238_235fc.CyclesPerBlock = ((RawRatio)this[Index238]).CyclesPerBlock;
                            r238_235fc.fractionationCorrected = true;
                            // July 2007 set oxide corrected for new
                            r238_235fc.OxidationCorrected = ((RawRatio)this[Index238]).OxidationCorrected;

                            // not set only get                    r238_235fc.MeanAlpha = meanAlphaU;

                            // Jan 2008
                            RawRatio r238_233fc = null;
                            if (Index238_233 > -1)
                            {
                                r238_233fc = new RawRatio(
                                ((RawRatio)this[Index238_233]).Name, r238_233fc_Ratios);
                                r238_233fc.Alpha = r238_235fc_Alpha; // a copy of the other
                                r238_233fc.CyclesPerBlock = ((RawRatio)this[Index238_233]).CyclesPerBlock;
                                r238_233fc.fractionationCorrected = true;
                                r238_233fc.OxidationCorrected = ((RawRatio)this[Index238_233]).OxidationCorrected;
                            }

                            // save these new guys for brute force insertion after they are created
                            correctedRatios.Add(r233_235fc);
                            correctedRatios.Add(r238_235fc);
                            if (Index238_233 > -1)
                                correctedRatios.Add(r238_233fc);

                            // create special ratio to return 1 + alphaU
                            RawRatio onePlusAlphaURatio =
                                new RawRatio(
                                    "1 + alphaU <" + ((RawRatio)this[Index238]).Name + ">",
                                    onePlusAlpha);
                            onePlusAlphaURatio.CyclesPerBlock = ((RawRatio)this[Index238]).CyclesPerBlock;
                            onePlusAlphaURatio.fractionationCorrected = false;

                            alphaUPlusOneRatios.Add(onePlusAlphaURatio);
                        }

                        // brute force insert
                        int frontIndex = 0;
                        foreach (object fc in alphaUPlusOneRatios)
                        {
                            this.Insert(frontIndex, fc);
                            frontIndex++;
                        }

                        foreach (object fc in correctedRatios)
                        {
                            string fcName = ((RawRatio)fc).Name;
                            for (int r = 0; r < Count; r++)
                            {
                                if (((RawRatio)this[r]).Name.Equals(fcName))
                                {
                                    ((RawRatio)fc).Name +=
                                        Environment.NewLine
                                        + " [FC: " + CurrentTracer.getNameAndVersion()
                                        + "]";
                                    this.Insert(r + 1, fc);
                                    // nov 2009
                                    // deselect the uncorrected ratio
                                    ((RawRatio)this[r]).IsActive = false;
                                    break;
                                }
                            }
                        }
                    }
                }

                // modified october 2009 for BariumPhosphate and Thallium Interference Corrections               
                else if (ContainMeasuredPb202_205 || ContainMeasuredPb201_205andPb203_205)
                {
                    PerformCorrectionsOfLeadRatios();
                }

                //if (correctedRatios.Count == 0)
                //    AmFractionationCorrected = false;
            }
        }

        private void PerformCorrectionsOfLeadRatios()
        {
            ArrayList correctedRatios = new ArrayList();

            // LEAD Tracer type test
            string tracerType = CurrentTracer.tracerType;
            if ((tracerType.CompareTo("mixed 202-205-233-235") == 0) ||
                (tracerType.CompareTo("mixed 202-205-233-236") == 0))
            {
                // get the index for each ratio
                int r202_205mIndex = -1;

                // added oct 2009
                int r201_205mIndex = -1;
                int r203_205mIndex = -1;

                int r206_204mIndex = -1;
                int r207_204mIndex = -1;
                int r208_204mIndex = -1;
                int r206_207mIndex = -1;
                int r206_208mIndex = -1;
                int r204_205mIndex = -1;
                int r206_205mIndex = -1;
                int r207_205mIndex = -1;
                int r208_205mIndex = -1;

                string ratioName = null;
                int ratioIndex = -1;

                double[][] leadRatiosFC = new double[this.Count + 5][];// nov 2009 to absorb 5 ratios due to IC corrections
                decimal[] leadRatiosAlpha = null;

                for (int i = 0; i < Count; i++)
                {
                    //  nov 2010 to make more robust ratio name recognizer
                    string myName = ((RawRatio)this[i]).Name;
                    string[] myNames = myName.Split(new char[] { '/' });

                    ratioName = null;
                    ratioIndex = -1;

                    // edited August 2012 to handle the presence of many ratios including multiple instance of flavors of 202/205
                    if (myNames.Length >= 2)
                    {
                        try
                        {
                            int leadLeftI = myNames[0].IndexOf("20");
                            String leadLeftS = myNames[0].Substring(leadLeftI, 3);

                            int leadRightI = myNames[1].IndexOf("20");
                            String leadRightS = myNames[1].Substring(leadRightI, 3);

                            ratioName = leadLeftS + "/" + leadRightS;
                            ratioIndex = i;
                        }
                        catch (Exception)
                        {
                        }
                    }

                    switch (ratioName)
                    {
                        case "201/205":
                            r201_205mIndex = ratioIndex;
                            Console.WriteLine("201/205" + "=" + ratioIndex);
                            break;
                        case "202/205":
                            if (r202_205mIndex == -1)
                            {
                                r202_205mIndex = ratioIndex;
                                Console.WriteLine("202/205" + "=" + ratioIndex);
                                // TODO fix this
                                // using the slot for 202/205 to store alpha results
                                // will work only as long as 202/205 is present
                                leadRatiosAlpha = new decimal[((RawRatio)this[ratioIndex]).Ratios.Length];
                            }
                            break;
                        case "203/205":
                            r203_205mIndex = ratioIndex;
                            Console.WriteLine("203/205" + "=" + ratioIndex);
                            break;
                        case "206/204":
                            r206_204mIndex = ratioIndex;
                            Console.WriteLine("206/204" + "=" + ratioIndex);
                            break;
                        case "207/204":
                            r207_204mIndex = ratioIndex;
                            Console.WriteLine("207/204" + "=" + ratioIndex);
                            break;
                        case "208/204":
                            r208_204mIndex = ratioIndex;
                            Console.WriteLine("208/204" + "=" + ratioIndex);
                            break;
                        case "206/207":
                            r206_207mIndex = ratioIndex;
                            Console.WriteLine("206/207" + "=" + ratioIndex);
                            break;
                        case "206/208":
                            r206_208mIndex = ratioIndex;
                            Console.WriteLine("206/208" + "=" + ratioIndex);
                            break;
                        case "204/205":
                            r204_205mIndex = ratioIndex;
                            Console.WriteLine("204/205" + "=" + ratioIndex);
                            break;
                        case "206/205":
                            r206_205mIndex = ratioIndex;
                            Console.WriteLine("206/205" + "=" + ratioIndex);
                            break;
                        case "207/205":
                            r207_205mIndex = ratioIndex;
                            Console.WriteLine("207/205" + "=" + ratioIndex);
                            break;
                        case "208/205":
                            r208_205mIndex = ratioIndex;
                            Console.WriteLine("208/205" + "=" + ratioIndex);
                            break;
                        default:
                            break;
                    }
                    if (ratioIndex > -1)
                    {
                        // restore data and copy in FC array for processing
                        ((RawRatio)this[ratioIndex]).RestoreData();
                        leadRatiosFC[ratioIndex] = new double[((RawRatio)this[ratioIndex]).Ratios.Length];
                        Array.Copy(((RawRatio)this[ratioIndex]).Ratios,
                            leadRatiosFC[ratioIndex], ((RawRatio)this[ratioIndex]).Ratios.Length);
                    }
                }

                // nov 2009 fix up with 5 additional fractions for IC
                // note also using 202_205 as a control
                if (r206_204mIndex < 0)
                {
                    r206_204mIndex = Count;
                    leadRatiosFC[r206_204mIndex] = new double[((RawRatio)this[r202_205mIndex]).Ratios.Length];
                }
                if (r206_207mIndex < 0)
                {
                    r206_207mIndex = Count + 1;
                    leadRatiosFC[r206_207mIndex] = new double[((RawRatio)this[r202_205mIndex]).Ratios.Length];
                }
                if (r206_208mIndex < 0)
                {
                    r206_208mIndex = Count + 2;
                    leadRatiosFC[r206_208mIndex] = new double[((RawRatio)this[r202_205mIndex]).Ratios.Length];
                }
                if (r207_204mIndex < 0)
                {
                    r207_204mIndex = Count + 3;
                    leadRatiosFC[r207_204mIndex] = new double[((RawRatio)this[r202_205mIndex]).Ratios.Length];
                }
                if (r208_204mIndex < 0)
                {
                    r208_204mIndex = Count + 4;
                    leadRatiosFC[r208_204mIndex] = new double[((RawRatio)this[r202_205mIndex]).Ratios.Length];
                }


                // calculate the correction
                r202_205t = CurrentTracer.getRatioByName("r202_205t").value;

                OrderedDictionary<string, decimal> BaPO2IC = null;
                r201_205BaPO2 = 0.0m;
                r202_205BaPO2 = 0.0m;
                r203_205BaPO2 = 0.0m;
                r204_205BaPO2 = 0.0m;
                if (CurrentBariumPhosphateIC != null)
                {
                    BaPO2IC = CurrentBariumPhosphateIC.calculateIsotopicComposition();
                    BaPO2IC.TryGetValue("r201_205BaPO2", out r201_205BaPO2);
                    BaPO2IC.TryGetValue("r202_205BaPO2", out r202_205BaPO2);
                    BaPO2IC.TryGetValue("r203_205BaPO2", out r203_205BaPO2);
                    BaPO2IC.TryGetValue("r204_205BaPO2", out r204_205BaPO2);
                }

                OrderedDictionary<string, decimal> TlIC = null;
                r203_205Tl = 0.0m;
                if (CurrentThalliumIC != null)
                {
                    TlIC = CurrentThalliumIC.calculateIsotopicComposition();
                    TlIC.TryGetValue("r203_205Tl", out r203_205Tl);
                }

                // nov 2009 for debugging of interference corrections, set up output file
                // write out a tempWorkProduct file and then show it in notepad
                FileInfo clist = new FileInfo("DetailedCorrectionsReport.csv");
                StreamWriter s = new StreamWriter(clist.FullName);
                //s.Write(RawRatios.getFractionationCorrectionReport());
                s.WriteLine("r201_205BaPO2 = " + r201_205BaPO2);
                s.WriteLine("r202_205BaPO2 = " + r202_205BaPO2);
                s.WriteLine("r203_205BaPO2 = " + r203_205BaPO2);
                s.WriteLine("r204_205BaPO2 = " + r204_205BaPO2);
                s.WriteLine("r203_205Tl = " + r203_205Tl);
                s.WriteLine();
                s.WriteLine("R#, alphaPb, r205BaPO2_205Pb, r205Tl_205Pb");
                //*********************************************************************

                // detect missing 202/205 based on first entry
                if ((((RawRatio)this[r202_205mIndex]).Ratios[0] == 0.0))
                {
                    try
                    {
                        Form myBaPO2Composition = //
                            new frmBaPO2_Tl_IsotopicComposition(//
                                CurrentBariumPhosphateIC,
                                CurrentThalliumIC,
                                this);

                        myBaPO2Composition.ShowDialog();
                    }
                    catch (Exception)
                    {

                    }
                }
                for (int i = 0; i < ((RawRatio)this[r202_205mIndex]).Ratios.Length; i++)
                {
                    decimal r205BaPO2_205Pb = 0.0m;
                    decimal r205Tl_205Pb = 0.0m;

                    if (UseTracerCorrection && !UseInterferenceCorrection)
                    {
                        alphaPb = //
                            CalculateAlphaPbForFractionationCorrectionOnly(i, r202_205mIndex);
                        s.WriteLine(i + 1 + "   , " + alphaPb + ",   " + r205BaPO2_205Pb + ",  " + r205Tl_205Pb);

                        // corrections
                        if (r206_204mIndex > -1)
                            leadRatiosFC[r206_204mIndex][i] *= (1.0 + 2.0 * (double)alphaPb);
                        if (r207_204mIndex > -1)
                            leadRatiosFC[r207_204mIndex][i] *= (1.0 + 3.0 * (double)alphaPb);
                        if (r208_204mIndex > -1)
                            leadRatiosFC[r208_204mIndex][i] *= (1.0 + 4.0 * (double)alphaPb);
                        if (r206_207mIndex > -1)
                            leadRatiosFC[r206_207mIndex][i] *= (1.0 - 1.0 * (double)alphaPb);
                        if (r206_208mIndex > -1)
                            leadRatiosFC[r206_208mIndex][i] *= (1.0 - 2.0 * (double)alphaPb);
                        if (r204_205mIndex > -1)
                            leadRatiosFC[r204_205mIndex][i] *= (1.0 - 1.0 * (double)alphaPb);
                        if (r206_205mIndex > -1)
                            leadRatiosFC[r206_205mIndex][i] *= (1.0 + 1.0 * (double)alphaPb);
                        if (r207_205mIndex > -1)
                            leadRatiosFC[r207_205mIndex][i] *= (1.0 + 2.0 * (double)alphaPb);
                        if (r208_205mIndex > -1)
                            leadRatiosFC[r208_205mIndex][i] *= (1.0 + 3.0 * (double)alphaPb);
                    }
                    else if (UseTracerCorrection && UseInterferenceCorrection)
                    {
                        // detect missing 202/205m - if zero, alphaPb has been entered by user
                        if ((((RawRatio)this[r202_205mIndex]).Ratios[i] != 0.0))
                        {
                            // JAN 2014 discovered that need to confirm user got all three ratios
                            alphaPb = 0.0m;

                            if (r201_205mIndex * r202_205mIndex * r203_205mIndex < 0)
                            {
                                MessageBox.Show(
                                    "One of the following ratios is missing and needed for Interference Correction: \n\n"
                                    + "\n 201/205, 202/205, 203/205.",
                                    "Tripoli Warning");

                                UseInterferenceCorrection = false;
                            }
                            else
                            {
                                alphaPb = //
                                    CalculateAlphaPbForInterferenceAndFractionationCorrectionBoth( //
                                    i, r201_205mIndex, r202_205mIndex, r203_205mIndex);
                            }
                        }

                        // jan 2014 check if alpha was calculated
                        if (alphaPb != 0.0m)
                        {
                            r205BaPO2_205Pb = CalculateR205BaPO2_205PbForInterferenceAndFractionationCorrectionBoth( //
                                i, r201_205mIndex, r202_205mIndex, r203_205mIndex, alphaPb);

                            r205Tl_205Pb = CalculateR205Tl_205PbForInterferenceAndFractionationCorrectionBoth( //
                                i, r201_205mIndex, r202_205mIndex, r203_205mIndex, alphaPb);

                            s.WriteLine(i + 1 + "   , " + alphaPb + ",   " + r205BaPO2_205Pb + ",  " + r205Tl_205Pb);

                            // corrections

                            if (r204_205mIndex > -1)
                            {
                                leadRatiosFC[r204_205mIndex][i] *= //
                                    (1.0 - 1.0 * (double)alphaPb) //
                                    * (1.0 + (double)r205Tl_205Pb + (double)r205BaPO2_205Pb)  //
                                    - (double)r204_205BaPO2 * (double)r205BaPO2_205Pb;

                            }
                            if (r206_205mIndex > -1)
                            {
                                leadRatiosFC[r206_205mIndex][i] *= //
                                    (1.0 + 1.0 * (double)alphaPb) //
                                    * (1.0 + (double)r205Tl_205Pb + (double)r205BaPO2_205Pb);
                            }
                            if (r207_205mIndex > -1)
                            {
                                leadRatiosFC[r207_205mIndex][i] *= //
                                    (1.0 + 2.0 * (double)alphaPb) //
                                    * (1.0 + (double)r205Tl_205Pb + (double)r205BaPO2_205Pb);
                            }
                            if (r208_205mIndex > -1)
                            {
                                leadRatiosFC[r208_205mIndex][i] *= //
                                    (1.0 + 3.0 * (double)alphaPb) //
                                    * (1.0 + (double)r205Tl_205Pb + (double)r205BaPO2_205Pb);
                            }

                            // the extra five fc ratios due to IC Nov 2009 **************************
                            // here if they did not exist, we create them
                            // division by zero does not throw an exception, rather produces NaN

                            if (r206_204mIndex > -1)
                                if (leadRatiosFC[r204_205mIndex][i].CompareTo(0.0) != 0)
                                {
                                    leadRatiosFC[r206_204mIndex][i] = //
                                        leadRatiosFC[r206_205mIndex][i] //
                                        / leadRatiosFC[r204_205mIndex][i];
                                }
                                else
                                    leadRatiosFC[r206_204mIndex][i] = 0.0;

                            if (r206_207mIndex > -1)
                                if (leadRatiosFC[r207_205mIndex][i].CompareTo(0.0) != 0)
                                {
                                    leadRatiosFC[r206_207mIndex][i] = //
                                        leadRatiosFC[r206_205mIndex][i] //
                                        / leadRatiosFC[r207_205mIndex][i];
                                }
                                else
                                    leadRatiosFC[r206_207mIndex][i] = 0.0;

                            if (r206_208mIndex > -1)
                                if (leadRatiosFC[r208_205mIndex][i].CompareTo(0.0) != 0)
                                {
                                    leadRatiosFC[r206_208mIndex][i] = //
                                           leadRatiosFC[r206_205mIndex][i] //
                                           / leadRatiosFC[r208_205mIndex][i];
                                }
                                else
                                    leadRatiosFC[r206_208mIndex][i] = 0.0;

                            if (r207_204mIndex > -1)
                                if (leadRatiosFC[r204_205mIndex][i].CompareTo(0.0) != 0)
                                {
                                    leadRatiosFC[r207_204mIndex][i] = //
                                        leadRatiosFC[r207_205mIndex][i] //
                                        / leadRatiosFC[r204_205mIndex][i];
                                }
                                else
                                    leadRatiosFC[r207_204mIndex][i] = 0.0;

                            if (r208_204mIndex > -1)
                                if (leadRatiosFC[r204_205mIndex][i].CompareTo(0.0) != 0)
                                {
                                    leadRatiosFC[r208_204mIndex][i] = //
                                         leadRatiosFC[r208_205mIndex][i] //
                                         / leadRatiosFC[r204_205mIndex][i];
                                }
                                else
                                    leadRatiosFC[r208_204mIndex][i] = 0.0;
                        }
                    }

                    else if (UseInterferenceCorrection)// not implemented as of Oct 2009
                    {
                        alphaPb = 0.0m;
                    }

                    leadRatiosAlpha[i] = alphaPb;
                    // save 1 plus AlphaPb for new fraction that reports just AlphaPb
                    // TODO: Fix this use of 202/205 slot to dedicated slot (see TODO above)
                    leadRatiosFC[r202_205mIndex][i] = (1.0 + (double)alphaPb);
                }

                // wrap up special report ********************************************
                s.Flush();
                s.Close();
                //try
                //{
                //    System.Diagnostics.Process.Start(clist.FullName);
                //}
                //catch { }


                if (r206_204mIndex > -1)
                {
                    RawRatio ratio = new RawRatio(
                        "206/204", leadRatiosFC[r206_204mIndex]);
                    Array.Copy(leadRatiosAlpha,
                                ratio.Alpha, leadRatiosAlpha.Length);
                    ratio.CyclesPerBlock = ((RawRatio)this[r202_205mIndex]).CyclesPerBlock;
                    ratio.fractionationCorrected = true;

                    correctedRatios.Add(ratio);
                }

                if (r207_204mIndex > -1)
                {
                    RawRatio ratio = new RawRatio(
                        "207/204", leadRatiosFC[r207_204mIndex]);
                    Array.Copy(leadRatiosAlpha,
                                ratio.Alpha, leadRatiosAlpha.Length);
                    ratio.CyclesPerBlock = ((RawRatio)this[r202_205mIndex]).CyclesPerBlock;
                    ratio.fractionationCorrected = true;

                    correctedRatios.Add(ratio);
                }

                if (r208_204mIndex > -1)
                {
                    RawRatio ratio = new RawRatio(
                        "208/204", leadRatiosFC[r208_204mIndex]);
                    Array.Copy(leadRatiosAlpha,
                                ratio.Alpha, leadRatiosAlpha.Length);
                    ratio.CyclesPerBlock = ((RawRatio)this[r202_205mIndex]).CyclesPerBlock;
                    ratio.fractionationCorrected = true;

                    correctedRatios.Add(ratio);
                }

                if (r206_207mIndex > -1)
                {
                    RawRatio ratio = new RawRatio(
                        "206/207", leadRatiosFC[r206_207mIndex]);
                    Array.Copy(leadRatiosAlpha,
                                ratio.Alpha, leadRatiosAlpha.Length);
                    ratio.CyclesPerBlock = ((RawRatio)this[r202_205mIndex]).CyclesPerBlock;
                    ratio.fractionationCorrected = true;

                    correctedRatios.Add(ratio);
                }

                if (r206_208mIndex > -1)
                {
                    RawRatio ratio = new RawRatio(
                        "206/208", leadRatiosFC[r206_208mIndex]);
                    Array.Copy(leadRatiosAlpha,
                                ratio.Alpha, leadRatiosAlpha.Length);
                    ratio.CyclesPerBlock = ((RawRatio)this[r202_205mIndex]).CyclesPerBlock;
                    ratio.fractionationCorrected = true;

                    correctedRatios.Add(ratio);
                }

                if (r204_205mIndex > -1)
                {
                    correctedRatios.Add(
                        createFractionationCorrectedPbRatio(
                        r204_205mIndex,
                        leadRatiosFC[r204_205mIndex],
                        leadRatiosAlpha));
                }

                if (r206_205mIndex > -1)
                {
                    correctedRatios.Add(
                        createFractionationCorrectedPbRatio(
                        r206_205mIndex,
                        leadRatiosFC[r206_205mIndex],
                        leadRatiosAlpha));
                }

                if (r207_205mIndex > -1)
                {
                    correctedRatios.Add(
                        createFractionationCorrectedPbRatio(
                        r207_205mIndex,
                        leadRatiosFC[r207_205mIndex],
                        leadRatiosAlpha));
                }

                if (r208_205mIndex > -1)
                {
                    correctedRatios.Add(
                        createFractionationCorrectedPbRatio(
                        r208_205mIndex,
                        leadRatiosFC[r208_205mIndex],
                        leadRatiosAlpha));
                }


                // create special ratio to return 1 + AlphaPb
                RawRatio onePlusAlphaPbRatio = new RawRatio(
                    "1 + alphaPb", leadRatiosFC[r202_205mIndex]);
                onePlusAlphaPbRatio.CyclesPerBlock = ((RawRatio)this[r202_205mIndex]).CyclesPerBlock;
                onePlusAlphaPbRatio.fractionationCorrected = false;

                this.Insert(0, onePlusAlphaPbRatio);

                // brute force insert
                foreach (object fc in correctedRatios)
                {
                    Boolean inserted = false;
                    string fcName = ((RawRatio)fc).Name;
                    for (int r = 0; r < Count; r++)
                    {
                        if (((RawRatio)this[r]).Name.Equals(fcName))
                        {
                            String fcFact = "FC: " + CurrentTracer.getNameAndVersion();
                            String icFact = "";
                            if (UseInterferenceCorrection)
                            {
                                icFact = "BaPO2IC: " + CurrentBariumPhosphateIC.getNameAndVersion() //
                                        + "; TlIC: " + CurrentThalliumIC.getNameAndVersion() + ";";
                            }
                            ((RawRatio)fc).Name +=
                                Environment.NewLine
                                + " [" + icFact + " " + fcFact //
                                + "]";
                            this.Insert(r + 1, fc);
                            inserted = true;
                            // nov 2009
                            // deselect the uncorrected ratio
                            ((RawRatio)this[r]).IsActive = false;
                            break;
                        }
                    }

                    if (!inserted)// one of the new 5 from IC Nov 2009
                    {
                        String fcFact = "FC: " + CurrentTracer.getNameAndVersion();
                        String icFact = "";
                        if (UseInterferenceCorrection)
                        {
                            icFact = "BaPO2IC: " + CurrentBariumPhosphateIC.getNameAndVersion() //
                                    + "; TlIC: " + CurrentThalliumIC.getNameAndVersion() + ";";
                        }
                        ((RawRatio)fc).Name +=
                            Environment.NewLine
                            + " [" + icFact + " " + fcFact //
                            + "]";
                        this.Insert(this.Count, fc);
                    }

                    // case where correctedRatio is all new = the 5 from IC

                }

                AmFractionationCorrected = true;
            }
            else
            {
                // Notify user of wrong tracer type
                MessageBox.Show(
                    "Tracer type \n\n"
                    + CurrentTracer.tracerType
                    + "\n\n does not support Pb fractionation correction.",
                    "Tripoli Warning");

                CurrentTracer = null;
                CurrentBariumPhosphateIC = null;
                CurrentThalliumIC = null;
            }

        }

        //private void 

        private decimal CalculateAlphaPbForFractionationCorrectionOnly(//
            int i, int r202_205mIndex)
        {
            decimal alphaPb = -1.0m; // since all ratios are restored this will force missing data to stay missing
            if (((RawRatio)this[r202_205mIndex]).Ratios[i] > 0.0)
            {
                alphaPb = (1.0m / 3.0m) * //
                    (1.0m - (r202_205t / (decimal)((RawRatio)this[r202_205mIndex]).Ratios[i]));
            }

            return alphaPb;
        }

        private decimal CalculateAlphaPbForInterferenceAndFractionationCorrectionBoth(//
            int i, int r201_205mIndex, int r202_205mIndex, int r203_205mIndex)
        {
            decimal alphaPb = -1.0m; // since all ratios are restored this will force missing data to stay missing
            if (((RawRatio)this[r202_205mIndex]).Ratios[i] > 0.0)
            {
                alphaPb =  //
                    ( //
                    ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                    * r202_205t //
                    * r203_205BaPO2 //

                    - r201_205BaPO2 //
                    * r202_205t //
                    * ((decimal)((RawRatio)this[r203_205mIndex]).Ratios[i]) //

                    + ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                    * r202_205BaPO2 //
                    * r203_205Tl //

                    - r201_205BaPO2 //
                    * ((decimal)((RawRatio)this[r202_205mIndex]).Ratios[i]) //
                    * r203_205Tl //

                    + r201_205BaPO2 //
                    * r202_205t //
                    * r203_205Tl //

                    - ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                    * r202_205t //
                    * r203_205Tl //

                    ) / ( //

                    4 * ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                    * r202_205t //
                    * r203_205BaPO2 //

                    - 2 * r201_205BaPO2 //
                    * r202_205t //
                    * ((decimal)((RawRatio)this[r203_205mIndex]).Ratios[i]) //

                    + 4 * ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                    * r202_205BaPO2 //
                    * r203_205Tl //

                    - 3 * r201_205BaPO2 //
                    * ((decimal)((RawRatio)this[r202_205mIndex]).Ratios[i]) //
                    * r203_205Tl //

                    - 4 * ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                    * r202_205t //
                    * r203_205Tl //

                    );
            }

            return alphaPb;
        }

        private decimal CalculateR205BaPO2_205PbForInterferenceAndFractionationCorrectionBoth(//
            int i, int r201_205mIndex, int r202_205mIndex, int r203_205mIndex, decimal alphaPb)
        {
            decimal r205BaPO2_205Pb = 0.0m;
            if (((RawRatio)this[r202_205mIndex]).Ratios[i] > 0.0)
            {
                r205BaPO2_205Pb =  //
                    ( //
                    ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                    * ( //
                    4 * r202_205t //
                    * r203_205Tl //
                    - 2 * r202_205t //
                    * ((decimal)((RawRatio)this[r203_205mIndex]).Ratios[i]) //
                    - ((decimal)((RawRatio)this[r202_205mIndex]).Ratios[i]) //
                    * r203_205Tl //
                    ) //
                    ) //
                    / //
                    ( //
                    2 * ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                    * r202_205BaPO2 //
                    * ((decimal)((RawRatio)this[r203_205mIndex]).Ratios[i]) //

                    - ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                    * ((decimal)((RawRatio)this[r202_205mIndex]).Ratios[i]) //
                    * r203_205BaPO2 //

                    - r201_205BaPO2 //
                    * ((decimal)((RawRatio)this[r202_205mIndex]).Ratios[i]) //
                    * ((decimal)((RawRatio)this[r203_205mIndex]).Ratios[i]) //

                    - 4 * ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                    * r202_205BaPO2 //
                    * r203_205Tl //

                    + 3 * r201_205BaPO2 //
                    * ((decimal)((RawRatio)this[r202_205mIndex]).Ratios[i]) //
                    * r203_205Tl //

                    + ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                    * ((decimal)((RawRatio)this[r202_205mIndex]).Ratios[i]) //
                    * r203_205Tl //
                    )
                    ;

            }
            else
            {
                r205BaPO2_205Pb =  //
                        ( //
                        ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                        * r203_205Tl //
                        * ( //

                        -1.0m //

                        + 4.0m * alphaPb //
                        )) //
                        / //
                        ( //
                        ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                        * r203_205BaPO2 //

                        - r201_205BaPO2 //
                        * ((decimal)((RawRatio)this[r203_205mIndex]).Ratios[i]) //

                        + r201_205BaPO2 //
                        * r203_205Tl //

                        - ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                         * r203_205Tl //

                         - 4.0m * ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                         * r203_205BaPO2
                         * alphaPb);

            }

            return r205BaPO2_205Pb;
        }

        private decimal CalculateR205Tl_205PbForInterferenceAndFractionationCorrectionBoth(//
            int i, int r201_205mIndex, int r202_205mIndex, int r203_205mIndex, decimal alphaPb)
        {
            decimal r205Tl_205Pb = 0.0m;
            if (((RawRatio)this[r202_205mIndex]).Ratios[i] > 0.0)
            {
                r205Tl_205Pb = //
                    -( //
                    ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                    * ((decimal)((RawRatio)this[r202_205mIndex]).Ratios[i]) //
                    * r203_205BaPO2 //

                    - 4 * ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                    * r202_205t //
                    * r203_205BaPO2 //

                    - 2 * ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                    * r202_205BaPO2 //
                    * ((decimal)((RawRatio)this[r203_205mIndex]).Ratios[i]) //

                    + r201_205BaPO2 //
                    * ((decimal)((RawRatio)this[r202_205mIndex]).Ratios[i]) //
                    * ((decimal)((RawRatio)this[r203_205mIndex]).Ratios[i]) //

                    + 2 * r201_205BaPO2 //
                    * r202_205t //
                    * ((decimal)((RawRatio)this[r203_205mIndex]).Ratios[i]) //

                    + 2 * ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                    * r202_205t //
                    * ((decimal)((RawRatio)this[r203_205mIndex]).Ratios[i]) //
                    ) //
                    / //
                    ( //
                    ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                    * ((decimal)((RawRatio)this[r202_205mIndex]).Ratios[i]) //
                    * r203_205BaPO2 //

                    - 2 * ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                    * r202_205BaPO2 //
                    * ((decimal)((RawRatio)this[r203_205mIndex]).Ratios[i]) //

                    + r201_205BaPO2 //
                    * ((decimal)((RawRatio)this[r202_205mIndex]).Ratios[i]) //
                    * ((decimal)((RawRatio)this[r203_205mIndex]).Ratios[i]) //

                    + 4 * ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                    * r202_205BaPO2 //
                    * r203_205Tl //

                    - 3 * r201_205BaPO2 //
                    * ((decimal)((RawRatio)this[r202_205mIndex]).Ratios[i]) //
                    * r203_205Tl //

                    - ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                    * ((decimal)((RawRatio)this[r202_205mIndex]).Ratios[i]) //
                    * r203_205Tl
                    );

            }
            else
            {
                r205Tl_205Pb = //
                   -( //
                   ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                   * r203_205BaPO2 //

                   - r201_205BaPO2 //
                   * ((decimal)((RawRatio)this[r203_205mIndex]).Ratios[i]) //

                   - 4.0m * ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                   * r203_205BaPO2 //
                   * alphaPb //

                   + 2.0m * r201_205BaPO2 //
                   * ((decimal)((RawRatio)this[r203_205mIndex]).Ratios[i]) //
                   * alphaPb //
                   ) //
                   / //
                   ( //
                   ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                   * r203_205BaPO2 //

                   - r201_205BaPO2 //
                   * ((decimal)((RawRatio)this[r203_205mIndex]).Ratios[i]) //

                   + r201_205BaPO2 //
                   * r203_205Tl //

                   - ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                   * r203_205Tl //

                   - 4.0m * ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                   * r203_205BaPO2 //
                   * alphaPb //

                   + 2.0m * r201_205BaPO2 //
                   * ((decimal)((RawRatio)this[r203_205mIndex]).Ratios[i]) //
                   * alphaPb //

                   + 4.0m * ((decimal)((RawRatio)this[r201_205mIndex]).Ratios[i]) //
                   * r203_205Tl //
                   * alphaPb //
                   );
            }

            return r205Tl_205Pb;
        }

        private RawRatio createFractionationCorrectedPbRatio(//
            int index, double[] leadRatiosFC, decimal[] leadRatiosAlpha)
        {
            RawRatio ratio = new RawRatio(
                ((RawRatio)this[index]).Name, leadRatiosFC);
            Array.Copy(leadRatiosAlpha,
                        ratio.Alpha, leadRatiosAlpha.Length);
            ratio.CyclesPerBlock = ((RawRatio)this[index]).CyclesPerBlock;
            ratio.fractionationCorrected = true;

            return ratio;
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveFractionationCorrection()
        {
            if (AmFractionationCorrected)
            {
                CurrentTracer = null;
                CurrentBariumPhosphateIC = null;
                CurrentThalliumIC = null;

                // remove existing 1 plus alpha U and Pb
                while (((RawRatio)this[0]).Name.StartsWith("1"))
                    this.RemoveAt(0);

                for (int r = Count - 1; r >= 0; r--)
                {
                    if (((RawRatio)this[r]).fractionationCorrected)
                        this.RemoveAt(r);
                }
                AmFractionationCorrected = false;

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string getFractionationCorrectionReport()
        {
            // March 2008 check for Pb or Uranium

            string retval = "Data has not been fractionation-corrected ... no results.";

            if (AmFractionationCorrected)
            {
                if (ContainMeasuredUraniumForFractionationCorrection)
                {
                    retval = "Fractionation correction report of paired 238/235 and 233/235 ratios:";
                    retval += Environment.NewLine;
                    retval += Environment.NewLine;
                    retval += "Tracer: " + CurrentTracer.getNameAndVersion();
                    retval += "   using r238_235g = " + this.r238_235g;
                    retval += Environment.NewLine;
                    retval += Environment.NewLine;

                    foreach (object pair in UraniumPairsNames)
                    {
                        retval += ((string[])pair)[0];
                        retval += Environment.NewLine;

                        retval += ((string[])pair)[1];
                        retval += Environment.NewLine;

                        try
                        {
                            if (AmFractionationCorrected)
                            {
                                retval += "   mean alphaU for all points = " + UraniumPairsAlphaU[UraniumPairsNames.IndexOf(pair)];
                            }

                            else
                                retval += "   mean alphaU not calculated ";
                        }
                        catch { }

                        retval += Environment.NewLine;
                        retval += Environment.NewLine;
                        retval += Environment.NewLine;

                    }

                }
                else if (ContainMeasuredPb202_205)
                {
                    retval = "Fractionation correction report for Pb ratios:";
                    retval += Environment.NewLine;
                    retval += Environment.NewLine;
                    try
                    {
                        retval += "Tracer: " + CurrentTracer.getNameAndVersion();
                    }
                    catch (Exception)
                    {

                    }
                    try
                    {
                        retval += "BaPO2 IC: " + CurrentBariumPhosphateIC.getNameAndVersion();
                        retval += "Tl IC: " + CurrentThalliumIC.getNameAndVersion();
                    }
                    catch (Exception)
                    {

                    }


                    retval += Environment.NewLine;
                    retval += Environment.NewLine;
                }
            }


            return retval;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string getCurrentTripoliTracerNameAndVersion()
        {
            if (CurrentTracer == null)
                return "NONE";
            else
                return CurrentTracer.getNameAndVersion();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string getCurrentTripoliBariumPhosphateICNameAndVersion()
        {
            if (CurrentBariumPhosphateIC == null)
                return "NONE";
            else
                return CurrentBariumPhosphateIC.getNameAndVersion();
        }


        /// <summary>
        /// 
        /// </summary>
        public void PerformOxideCorrection()
        {
            // May 2007 
            // if ratios contain 270/267 or  265/267, they will be corrected to
            //                   238/235 and 233/235

            // Jan 2008
            // if both 238/235 and 233/235 exist, then we produce 238/233 for export to Redux

            // jan 2011 first detect whether we are dealing with  nnn/268  or the new nnn/267
            // then call on the right correction method 

            String typeOfOxideCorrection = "";

            // modified Mar 2011 to force the detection of 270/268 for Drew Coleman's lab (Noah says won't affect anyone else)
            for (int index = 0; index < Count; index++)
            {
                string tempName = ((RawRatio)this[index]).Name;

                if (!((RawRatio)this[index]).OxidationCorrected)
                {
                    if (((RawRatio)this[index]).getSimpleName(tempName).Contains("270/268"))
                    {
                        typeOfOxideCorrection = "270/268";
                        break;
                    }
                }
            }

            if (typeOfOxideCorrection.Equals(""))
            {
                for (int index = 0; index < Count; index++)
                {
                    string tempName = ((RawRatio)this[index]).Name;

                    if (!((RawRatio)this[index]).OxidationCorrected)
                    {
                        if (((RawRatio)this[index]).getSimpleName(tempName).Contains("270/267"))
                        {
                            typeOfOxideCorrection = "270/267";
                            break;
                        }
                    }
                }
            }

            if (typeOfOxideCorrection.Equals("270/267"))
            {
                PerformOxideCorrectionFor233_235Tracer();
            }
            else if (typeOfOxideCorrection.Equals("270/268"))
            {
                PerformOxideCorrectionFor233_236Tracer();
            }
        }


        private void PerformOxideCorrectionFor233_235Tracer()
        {
            // follow the technique mapped out in ContainMeasuredUraniumForFractionationCorrection
            UraniumOxidePairs = new ArrayList();
            UraniumOxidePairsNames = new ArrayList();

            int indexCount = -1;
            for (int index = 0; index < Count; index++)
            {
                string tempName = ((RawRatio)this[index]).Name;

                // find first instance of 270/267 and then search for first instance of 265/267
                if (((RawRatio)this[index]).getSimpleName(tempName).Contains("270/267")
                    &&
                    !((RawRatio)this[index]).OxidationCorrected)
                {
                    UraniumOxidePairs.Add(new int[] { index, -1 });
                    UraniumOxidePairsNames.Add(new string[] { ((RawRatio)this[index]).Name, "" });

                    indexCount += 1;
                    ((RawRatio)this[index]).amUranium = true;

                    // now search all for the missing half of the pair
                    // first determine its name

                    // to avoid possible function names etc that may precede the
                    // ratio name, we strip out the string before the ratio name



                    // July 2012 further refinements from Noah at BGS data
                    string NameOf270 = "";
                    string NameOf265 = "";

                    // detect the presence of 238U02 or 238U.02 or 238U.1602 names for 270
                    if (((RawRatio)this[index]).Name.Contains("238UO2"))
                    {
                        NameOf270 =
                       ((RawRatio)this[index]).Name
                       .Substring(((RawRatio)this[index]).Name.IndexOf("238UO2"));

                        // strip of any trailing (#)
                        int indexFirstParen = NameOf270.LastIndexOf("(");
                        if (indexFirstParen > 0)
                        {
                            NameOf270 = NameOf270.Substring(0, indexFirstParen - 1);
                        }

                        NameOf265 = NameOf270.Replace("238UO2", "233UO2");
                    }

                    if (((RawRatio)this[index]).Name.Contains("238U.O2"))
                    {
                        NameOf270 =
                       ((RawRatio)this[index]).Name
                       .Substring(((RawRatio)this[index]).Name.IndexOf("238U.O2"));

                        // strip of any trailing (#)
                        int indexFirstParen = NameOf270.LastIndexOf("(");
                        if (indexFirstParen > 0)
                        {
                            NameOf270 = NameOf270.Substring(0, indexFirstParen - 1);
                        }

                        NameOf265 = NameOf270.Replace("238U.O2", "233U.O2");
                    }

                    if (((RawRatio)this[index]).Name.Contains("238U.16O2"))
                    {
                        NameOf270 =
                       ((RawRatio)this[index]).Name
                       .Substring(((RawRatio)this[index]).Name.IndexOf("238U.16O2"));

                        // strip of any trailing (#)
                        int indexFirstParen = NameOf270.LastIndexOf("(");
                        if (indexFirstParen > 0)
                        {
                            NameOf270 = NameOf270.Substring(0, indexFirstParen - 1);
                        }

                        NameOf265 = NameOf270.Replace("238U.16O2", "233U.16O2");
                    }

                    if (((RawRatio)this[index]).Name.Contains("270"))
                    {
                        NameOf270 =
                       ((RawRatio)this[index]).Name
                       .Substring(((RawRatio)this[index]).Name.IndexOf("270"));

                        // strip of any trailing (#)
                        int indexFirstParen = NameOf270.LastIndexOf("(");
                        if (indexFirstParen > 0)
                        {
                            NameOf270 = NameOf270.Substring(0, indexFirstParen - 1);
                        }

                        NameOf265 = NameOf270.Replace("270", "265");
                    }






                    for (int index2 = 0; index2 < Count; index2++)
                    {
                        if (((RawRatio)this[index2]).Name.Contains(NameOf265))
                        {
                            ((int[])UraniumOxidePairs[indexCount])[1] = index2;
                            ((string[])UraniumOxidePairsNames[indexCount])[1] = ((RawRatio)this[index2]).Name;
                            ((RawRatio)this[index2]).amUranium = true;

                            break;
                        }
                    }

                }
            }

            // Here the strategy will be to oxide-correct and replace rather than add ratios
            // the user will be given a notification
            ArrayList r238_233AddedRatios = new ArrayList();

            foreach (object Upair in UraniumOxidePairs)
            {
                int Index270 = ((int[])Upair)[0];
                int Index265 = ((int[])Upair)[1];

                // check for missing 233
                if (Index265 == -1) break;

                // name the ratios
                RawRatio ratio270_267 = (RawRatio)this[Index270];
                RawRatio ratio265_267 = (RawRatio)this[Index265];

                // restore the ratio data
                ratio270_267.RestoreData();
                ratio265_267.RestoreData();

                // oxide correction
                if (ratio270_267.Ratios.Length != ratio265_267.Ratios.Length)
                    MessageBox.Show("Data counts during oxide correction do not match for \n\n"
                                    + ratio270_267.Name
                                    + "\n\n and\n\n"
                                    + ratio265_267.Name
                                    + "\n\n PROCEEDING ANYWAY !!",
                                    "Tripoli Warning");

                // create the new ratio 238/233
                RawRatio r238_233m =
                    new RawRatio("238/233",
                    new Double[Math.Min(ratio270_267.Ratios.Length, ratio265_267.Ratios.Length)]);

                for (int index = 0; index < Math.Min(ratio270_267.Ratios.Length, ratio265_267.Ratios.Length); index++)
                {
                    // only perform for values > 0
                    if ((ratio265_267.Ratios[index] > 0.0)
                        &&
                        (ratio270_267.Ratios[index] > 0.0))
                    {
                        decimal denominator =
                            (1.0m / (decimal)(ratio265_267.Ratios[index]))
                                 - (2.0m * Convert.ToDecimal(_r18O_16O));

                        // calculate the correct 265/267
                        decimal r265_267 = 1.0m / denominator;

                        // calculate the correct 270/267
                        decimal r270_267 =
                            ((decimal)ratio270_267.Ratios[index] / (decimal)ratio265_267.Ratios[index])
                            / denominator;

                        // replace ratio values
                        ratio265_267.Ratios[index] = (double)r265_267; // ==> 233/235
                        ratio270_267.Ratios[index] = (double)r270_267; // ==> 238/235

                        // calculate 238/233
                        r238_233m.Ratios[index] = (double)r270_267 / (double)r265_267;
                    }
                }

                // reset the ratios ==> recalculates basic statistics even though = identity
                ratio270_267.Ratios = ratio270_267.Ratios;//   .CalcStats();
                ratio265_267.Ratios = ratio265_267.Ratios;//   .CalcStats();
                r238_233m.Ratios = r238_233m.Ratios;//   .CalcStats();

                // rename the corrected ratios

                // update July 2012 to handle the use of 238.02 etc as 270
                // the following code remains robust for the cases of 238.O2 etc, as the 270/267/265 names don't appear inside them
                // dec 2009 to handle Triton naming
                String ratio233_235Name = ratio265_267.Name.Replace("265/", @"233/");
                ratio233_235Name = ratio233_235Name.Replace("/267", @"/235");
                // jan 2008 change naming strategy
                string saveName = ratio265_267.Name;
                ratio265_267.Name =
                    ratio233_235Name
                    + " OxideCor:"
                    + "("
                    + saveName
                    + ")";

                // dec 2009 to handle Triton naming
                String ratio238_235Name = ratio270_267.Name.Replace("270/", @"238/");
                ratio238_235Name = ratio238_235Name.Replace("/267", @"/235");
                // jan 2008 change naming strategy
                saveName = ratio270_267.Name;
                ratio270_267.Name =
                    ratio238_235Name
                    + " OxideCor:"
                    + "("
                    + saveName
                    + ")";

                // flag the oxide correction
                ratio265_267.OxidationCorrected = true;
                ratio270_267.OxidationCorrected = true;

                // add in the new ratio
                r238_233m.OxidationCorrected = true;
                r238_233m.CyclesPerBlock = ratio265_267.CyclesPerBlock;
                r238_233m.Name
                    += " OxideCor:"
                    + "(" + ratio238_235Name + @"\" + ratio233_235Name + ")";

                r238_233AddedRatios.Add(r238_233m);

            }

            // insert new ratios after each pair and advance counter to accommodate new entry
            int pairCount = 0;
            foreach (Object r in r238_233AddedRatios)
            {
                Insert(((int[])UraniumOxidePairs[pairCount])[1] + pairCount + 1, r);
                pairCount++;
            }

            // or at least we tried
            AmOxideCorrected = (r238_233AddedRatios.Count > 0) && RatioType.Contains("U");
        }

        private void PerformOxideCorrectionFor233_236Tracer()
        {
            // follow the technique mapped out in ContainMeasuredUraniumForFractionationCorrection
            UraniumOxidePairs = new ArrayList();
            UraniumOxidePairsNames = new ArrayList();

            int indexCount = -1;
            for (int index = 0; index < Count; index++)
            {
                string tempName = ((RawRatio)this[index]).Name;

                // find first instance of 270/268 and then search for first instance of 265/268
                if (((RawRatio)this[index]).getSimpleName(tempName).Contains("270/268")
                    &&
                    !((RawRatio)this[index]).OxidationCorrected)
                {
                    UraniumOxidePairs.Add(new int[] { index, -1 });
                    UraniumOxidePairsNames.Add(new string[] { ((RawRatio)this[index]).Name, "" });

                    indexCount += 1;
                    ((RawRatio)this[index]).amUranium = true;

                    // now search all for the missing half of the pair
                    // first determine its name

                    // to avoid possible function names etc that may precede the
                    // ratio name, we strip out the string before the ratio name

                    string NameOf270 =
                        ((RawRatio)this[index]).Name
                        .Substring(((RawRatio)this[index]).Name.IndexOf("270"));

                    string NameOf265 = NameOf270.Replace("270", "265");

                    for (int index2 = 0; index2 < Count; index2++)
                    {
                        if (((RawRatio)this[index2]).Name.Contains(NameOf265))
                        {
                            ((int[])UraniumOxidePairs[indexCount])[1] = index2;
                            ((string[])UraniumOxidePairsNames[indexCount])[1] = ((RawRatio)this[index2]).Name;
                            ((RawRatio)this[index2]).amUranium = true;

                            break;
                        }
                    }

                }
            }

            // Here the strategy will be to oxide-correct and replace rather than add ratios
            // the user will be given a notification
            ArrayList r238_233AddedRatios = new ArrayList();

            foreach (object Upair in UraniumOxidePairs)
            {
                int Index270 = ((int[])Upair)[0];
                int Index265 = ((int[])Upair)[1];

                // check for missing 233
                if (Index265 == -1) break;

                // name the ratios
                RawRatio ratio270_268 = (RawRatio)this[Index270];
                RawRatio ratio265_268 = (RawRatio)this[Index265];

                // restore the ratio data
                ratio270_268.RestoreData();
                ratio265_268.RestoreData();

                // oxide correction
                if (ratio270_268.Ratios.Length != ratio265_268.Ratios.Length)
                    MessageBox.Show("Data counts during oxide correction do not match for \n\n"
                                    + ratio270_268.Name
                                    + "\n\n and\n\n"
                                    + ratio265_268.Name
                                    + "\n\n PROCEEDING ANYWAY !!",
                                    "Tripoli Warning");

                // create the new ratio 238/233
                RawRatio r238_233m =
                    new RawRatio("238/233",
                    new Double[Math.Min(ratio270_268.Ratios.Length, ratio265_268.Ratios.Length)]);

                for (int index = 0; index < Math.Min(ratio270_268.Ratios.Length, ratio265_268.Ratios.Length); index++)
                {
                    // only perform for values > 0
                    if ((ratio265_268.Ratios[index] > 0.0)
                        &&
                        (ratio270_268.Ratios[index] > 0.0))
                    {
                        // calculate the correct 265/268  ==> 233/236
                        decimal r265_268 = Convert.ToDecimal(ratio265_268.Ratios[index]); // identity

                        // calculate the corrected 270/268  ==> 238/236
                        decimal r270_268 =
                            (decimal)ratio270_268.Ratios[index] - (2.0m * Convert.ToDecimal(_r18O_16O));

                        // replace ratio values
                        //ratio265_268.Ratios[index] = (double)r265_268; // ==> 233/236 // identity so no math
                        ratio270_268.Ratios[index] = (double)r270_268; // ==> 238/236

                        // calculate 238/233
                        r238_233m.Ratios[index] = (double)r270_268 / (double)r265_268;
                    }
                }

                // reset the ratios ==> recalculates basic statistics even though = identity
                ratio270_268.Ratios = ratio270_268.Ratios;//   .CalcStats();
                ratio265_268.Ratios = ratio265_268.Ratios;//   .CalcStats();
                r238_233m.Ratios = r238_233m.Ratios;//   .CalcStats();

                // rename the corrected ratios
                // dec 2009 to handle Triton naming
                String ratio233_236Name = ratio265_268.Name.Replace("265/", @"233/");
                ratio233_236Name = ratio233_236Name.Replace("/268", @"/236");
                // jan 2008 change naming strategy
                string saveName = ratio265_268.Name;
                ratio265_268.Name =
                    ratio233_236Name
                    + " OxideCor:"
                    + "("
                    + saveName
                    + ")";

                // dec 2009 to handle Triton naming
                String ratio238_236Name = ratio270_268.Name.Replace("270/", @"238/");
                ratio238_236Name = ratio238_236Name.Replace("/268", @"/236");
                // jan 2008 change naming strategy
                saveName = ratio270_268.Name;
                ratio270_268.Name =
                    ratio238_236Name
                    + " OxideCor:"
                    + "("
                    + saveName
                    + ")";

                // flag the oxide correction
                ratio265_268.OxidationCorrected = true;
                ratio270_268.OxidationCorrected = true;

                // add in the new ratio
                r238_233m.OxidationCorrected = true;
                r238_233m.CyclesPerBlock = ratio265_268.CyclesPerBlock;
                r238_233m.Name
                    += " OxideCor:"
                    + "(" + ratio238_236Name + @"\" + ratio233_236Name + ")";

                r238_233AddedRatios.Add(r238_233m);

            }

            // insert new ratios after each pair and advance counter to accommodate new entry
            int pairCount = 0;
            foreach (Object r in r238_233AddedRatios)
            {
                Insert(((int[])UraniumOxidePairs[pairCount])[1] + pairCount + 1, r);
                pairCount++;
            }

            // or at least we tried
            AmOxideCorrected = (r238_233AddedRatios.Count > 0) && RatioType.Contains("U");
        }


        // exports any set of ratios not just redux subset
        public string ExportRatioData(string sourceTripoliFileName)
        {
            string retval = "";
            retval =
                "Tripoli tab-delimited output of processed data for:" + Environment.NewLine
                + sourceTripoliFileName + Environment.NewLine // was TripoliFileInfo in frmMain
                + "produced on: " + System.DateTime.Now.ToShortDateString() + Environment.NewLine + Environment.NewLine
                + "Each column header shows ratio name, Tripolized mean, Tripolized stdErr, Fractionation Corrected flag." + Environment.NewLine
                + "Data is presented by blocks that are separated by blank lines. " + Environment.NewLine
                + "Any discarded value is shown as a negative number. " + Environment.NewLine
                + "Any missing value is represented by a blank entry. " + Environment.NewLine + Environment.NewLine;

            // April 2007 - we need to show fractionation corrected names of 3 lines, but normal on one line
            // line 1
            for (int r = 0; r < this.Count; r++)
            {
                if (((RawRatio)this[r]).IsActive)
                {
                    // refresh values from stored points
                    ((RawRatio)this[r]).CalcStats();
                    // Environment.Newline = "\r\n" and we strip it out here
                    string[] myName = ((RawRatio)this[r]).Name.Split(new char[] { '\r', '\n' }, 2);
                    retval
                        += myName[0].PadRight(16)
                        + "\t";
                }
            }

            // line 2
            retval += Environment.NewLine;

            for (int r = 0; r < this.Count; r++)
            {
                if (((RawRatio)this[r]).IsActive)
                {
                    // refresh values from stored points
                    ((RawRatio)this[r]).CalcStats();
                    string[] myName = ((RawRatio)this[r]).Name.Split(new char[] { '\r', '\n' }, 2);
                    if (myName.Length > 1)
                    {
                        string[] secondPart = myName[1].Split(new char[] { 'w', '\\' }, 2);
                        retval += secondPart[0].Substring(2).PadRight(16);
                    }
                    else
                    {
                        retval += " ".PadRight(16);
                    }

                    retval += "\t";
                }
            }

            // line 3
            retval += Environment.NewLine;

            for (int r = 0; r < this.Count; r++)
            {
                if (((RawRatio)this[r]).IsActive)
                {
                    // refresh values from stored points
                    ((RawRatio)this[r]).CalcStats();
                    string[] myName = ((RawRatio)this[r]).Name.Split(new char[] { 'w', '\\' }, 2);
                    if (myName.Length > 1)
                    {
                        retval += "w" + myName[1].PadRight(16);
                    }
                    else
                    {
                        retval += " ".PadRight(16);
                    }

                    retval += "\t";
                }
            }

            // line 4
            retval += Environment.NewLine;

            for (int r = 0; r < this.Count; r++)
            {
                if (((RawRatio)this[r]).IsActive)
                {
                    retval
                        += ((RawRatio)this[r]).Mean.ToString("E9")
                        + "\t";
                }
            }

            retval += Environment.NewLine;

            for (int r = 0; r < this.Count; r++)
            {
                if (((RawRatio)this[r]).IsActive)
                {
                    retval
                        += ((RawRatio)this[r]).PctStdErr.ToString("E9")
                        + "\t";
                }
            }

            retval += Environment.NewLine;

            for (int r = 0; r < this.Count; r++)
            {
                if (((RawRatio)this[r]).IsActive)
                {
                    retval
                        += ((string)(((RawRatio)this[r]).fractionationCorrected ? "frac corr" : "not frac corr")).PadRight(16)
                        + "\t";
                }
            }

            retval += Environment.NewLine + Environment.NewLine;

            int rowCount = 0;
            for (int d = 0; d < ((RawRatio)this[0]).Ratios.Length; d++)
            {
                for (int r = 0; r < this.Count; r++)
                {
                    if (((RawRatio)this[r]).IsActive)
                    {
                        if (((RawRatio)this[r]).Ratios[d] != 0)
                        {
                            retval
                                += ((RawRatio)this[r]).Ratios[d].ToString("E9").PadLeft(16)
                                + "\t";
                        }
                        else
                        {
                            retval
                                += " ".PadLeft(16)
                                + "\t";
                        }
                    }
                }
                retval += Environment.NewLine;
                rowCount++;
                if (rowCount == ((RawRatio)this[0]).CyclesPerBlock)
                {
                    rowCount = 0;
                    retval += Environment.NewLine;	// blank line between blocks
                }

            }

            Console.WriteLine(retval);
            return retval;


        }

        public string getSampleNameFractionNameRatioTypeFileName()
        {
            return SampleName //
                           + "_" + FractionName//
                           + "_" + RatioType;
        }



        /// <summary>
        /// Refactored from DisplayCycleSelections (which was PrepareCycleSelections) in frmMainTripoli
        /// in order to rationalize oxide correction
        /// </summary>
        public void PrepareCycleSelections2011()
        {
            // April 2008
            if (UsampleComponents == null)
                UsampleComponents = new USampleComponents();

            // May 2007 now preprocess for oxide correction - this is automatic
            // but can be redone if the parameter needs to be reset f.e. _r18O_16O

            // Jan 2010 remove possibility of Pb and oxide correction for now by fixing legacy problem
            // also need to check for missing ratiotype which is true for standards and histories

            // feb 2010 for legacy tripoli files, need to discover ratiotype
            if ((RatioType == null) || (RatioType.CompareTo("") == 0))
            {
                if (((RawRatio)this[0]).Name.Contains("20"))
                {
                    RatioType = "Pb";
                }
                else//modified June 2011 if (((RawRatio)this[0]).Name.Contains("23"))
                {
                    RatioType = "U";
                }
            }

            if (AmOxideCorrected && RatioType.Contains("Pb"))
                AmOxideCorrected = false;

            if (!AmOxideCorrected && RatioType.Contains("U"))
            {
                PerformOxideCorrection();
            }
            else
            {
                // fix temporary bug from Jan 2010 where naming of oxide corrected ratios was broken
                // TODO: remove this after february 2010
                for (int index = 0; index < Count; index++)
                {
                    if (((RawRatio)this[index]).Name.Contains("270/235"))
                    {
                        ((RawRatio)(this[index])).Name = ((RawRatio)(this[index])).Name.Replace("270/235", "238/235");
                    }
                    if (((RawRatio)(this[index])).Name.Contains("265/235"))
                    {
                        ((RawRatio)(this[index])).Name = ((RawRatio)(this[index])).Name.Replace("265/235", "233/235");
                    }
                }
            }

        }

        #endregion Methods


    }
}
