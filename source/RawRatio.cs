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
using System.Security;
using System.Security.Permissions;
using System.Runtime.Serialization;
using Tripoli.earth_time_org;


namespace Tripoli
{
    /// <summary>
    /// Tripoli representation of one collected ratio or function
    /// </summary>
    [Serializable]
    public class RawRatio : ISerializable
    {
        // jan 2010 for playing with Gehrel's data
        private double[] _BlockMeans = new double[0];


        // fields
        string _Name = "NONE";
        bool _IsActive = true;
        bool _IsUserFunction = false;
        bool _AmSynchronizer = false;
        bool _AmSynchronized = false;

        bool _amUranium = false;
        bool _amPbLead = false;
        bool _fractionationCorrected = false;

        // added may 2007 to store the mean alphaU from fractionation correction
        // refactored to be the mean alpha either U or Pb depending on ratio
        // also, now is recalcualted on demand by getter to be the mean alpha 
        // for the selected data in the ratio
        decimal _meanAlpha = 0.0m;
        bool _oxidationCorrected = false;

        float _DataPenSize = 1.5F;
        int _CyclesPerBlock = 0;
        ArrayList _UndoHistory = new ArrayList();// null;

        /// <summary>
        /// Initial ratios
        /// </summary>
        double[] _Ratios = new double[1];// null;

        // nov 2009 added to save state during live update
        double[] _SavedRatios = new double[1];

        public double[] SavedRatios
        {
            get { return _SavedRatios; }
            set { _SavedRatios = value; }
        }

        /// <summary>
        /// Stores either alphaU or AlphaPb so that mean of selected alphas can be computed.
        /// May 2007
        /// </summary>
        decimal[] _Alpha = new decimal[1];

        public decimal[] Alpha
        {
            get { return _Alpha; }
            set { _Alpha = value; }
        }

        /// <summary>
        /// Currently selected ratios
        /// </summary>
        ArrayList _ActiveRatios = new ArrayList();// null;

        /// <summary>
        /// Index map back to _Ratios
        /// </summary>
        ArrayList _ActiveRatiosMap = new ArrayList();// null;
        //		bool _DiscardOutliers;



        // statistics
        double _Min = 0.0;
        double _Max = 0.0;
        double _Mean = 0.0;
        double _StdDev = 0.0;
        double _StdErr = 0.0;
        double _PctStdErr = 0.0;

        double _AllMin = 0.0;
        double _AllMax = 0.0;
        double _AllMean = 0.0;
        double _AllStdDev = 0.0;
        double _AllStdErr = 0.0;
        double _AllPctStdErr = 0.0;
        double _AllCount = 0.0;
        int _AllMissing = 0;

        double _LiveDataCount = 0.0;

        protected bool _AutoCalculate = true;
        protected int _HistogramBinCount = 0;

        public enum StatsDisplayed : int
        {
            None,
            StdErr,
            SigmaStdErr,
            TwoSigmaSigmaStdErr
        }
        protected int _StatsDetailDisplay = (int)StatsDisplayed.TwoSigmaSigmaStdErr;

        public enum HandleDiscards : int
        {
            Include,
            Ignore,
            Hide
        }
        public int _HandleDiscardsFlag = (int)HandleDiscards.Include;

        public enum HandleOutliers : int
        {
            Manual,
            TwoSigma,
            Chauvenet
        }
        protected int _HandleOutliersFlag = (int)HandleOutliers.Manual;

        // stores the outliers selected by 2-sigma or Chauvenet algorithm
        // so that they can be displayed differently - with a box around them
        public ArrayList _CurrentOutliersSelected = new ArrayList();


        public RawRatio()
        {
        }

        public RawRatio(string Name)
        {
            _Name = Name;

            _IsActive = true;
            _IsUserFunction = false;
            _AmSynchronizer = false;
            _AmSynchronized = false;
            _DataPenSize = 1.5F;
            _UndoHistory = new ArrayList();
            _AutoCalculate = true;
            _HistogramBinCount = 0;
            _StatsDetailDisplay = (int)StatsDisplayed.TwoSigmaSigmaStdErr;
            _HandleDiscardsFlag = (int)HandleDiscards.Include;
            _HandleOutliersFlag = (int)HandleOutliers.Manual;
            _CurrentOutliersSelected = new ArrayList();
            _amPbLead = false;
            _amUranium = false;
            _fractionationCorrected = false;
            _meanAlpha = 0.0m;
            _oxidationCorrected = false;

        }

        public RawRatio(string Name, double[] Ratios)
            : this(Name)
        {
            this.Ratios = Ratios;
        }

        public override bool Equals(object other)
        {
            bool result = false;
            if (other is RawRatio)
            {
                result = this.Name.Equals(((RawRatio)other).Name);
            }

            return result;
        }

        #region Serialization and deserialization

        /// <summary>
        /// 
        /// </summary>
        /// <param name="si"></param>
        /// <param name="context"></param>
        protected RawRatio(SerializationInfo si, StreamingContext context)
        {
            _Name = (string)si.GetValue("Name", typeof(string));
            _IsActive = (bool)si.GetValue("IsActive", typeof(bool));
            _IsUserFunction = (bool)si.GetValue("IsUserFunction", typeof(bool));
            _AmSynchronizer = (bool)si.GetValue("AmSynchronizer", typeof(bool));

            try
            {
                _amUranium = (bool)si.GetValue("amUranium", typeof(bool));
            }
            catch { }

            try
            {
                _amPbLead = (bool)si.GetValue("amPbLead", typeof(bool));
            }
            catch { }

            try
            {
                _fractionationCorrected = (bool)si.GetValue("fractionationCorrected", typeof(bool));
            }
            catch { }

            try
            {
                _oxidationCorrected = (bool)si.GetValue("oxidationCorrected", typeof(bool));
            }
            catch { }

            try
            {
                _meanAlpha = (decimal)si.GetValue("meanAlphaU", typeof(decimal));
            }
            catch { }

            _DataPenSize = (float)si.GetValue("DataPenSize", typeof(float));
            _CyclesPerBlock = (int)si.GetValue("CyclesPerBlock", typeof(int));
            _UndoHistory = (ArrayList)si.GetValue("UndoHistory", typeof(ArrayList));
            _Ratios = (double[])si.GetValue("Ratios", typeof(double[]));
            try
            {
                _Alpha = (decimal[])si.GetValue("Alpha", typeof(decimal[]));
            }
            catch { }
            _ActiveRatios = (ArrayList)si.GetValue("ActiveRatios", typeof(ArrayList));
            _ActiveRatiosMap = (ArrayList)si.GetValue("ActiveRatiosMap", typeof(ArrayList));
            _AllMin = (double)si.GetValue("AllMin", typeof(double));
            _AllMax = (double)si.GetValue("AllMax", typeof(double));
            _AllMean = (double)si.GetValue("AllMean", typeof(double));
            _AllStdDev = (double)si.GetValue("AllStdDev", typeof(double));
            _AllStdErr = (double)si.GetValue("AllStdErr", typeof(double));
            _AllPctStdErr = (double)si.GetValue("AllPctStdErr", typeof(double));
            _AllCount = (double)si.GetValue("AllCount", typeof(double));
            _AllMissing = (int)si.GetValue("AllMissing", typeof(int));

            try
            {
                _AutoCalculate = (bool)si.GetValue("AutoCalculate", typeof(bool));
            }
            catch { }

            try
            {
                _HistogramBinCount = (int)si.GetValue("HistogramBinCount", typeof(int));
            }
            catch { }

            try
            {
                _StatsDetailDisplay = (int)si.GetValue("StatsDetailDisplay", typeof(int));
            }
            catch { }

            try
            {
                _HandleDiscardsFlag = (int)si.GetValue("HandleDiscardsFlag", typeof(int));
            }
            catch { }

            try
            {
                _HandleOutliersFlag = (int)si.GetValue("HandleOutliersFlag", typeof(int));
            }
            catch { }

            try
            {
                _CurrentOutliersSelected = (ArrayList)si.GetValue("CurrentOutliersSelected", typeof(ArrayList));
            }
            catch { }

            try
            {
                _AmSynchronized = (bool)si.GetValue("AmSynchronized", typeof(bool));
            }
            catch { }

        }

        [OnDeserializing()]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            _amUranium = false;
            _amPbLead = false;
            _fractionationCorrected = false;
            _meanAlpha = 0.0m;
            _oxidationCorrected = false;
            _AutoCalculate = true;
            _HistogramBinCount = 0;
            _StatsDetailDisplay = 3;
            _HandleDiscardsFlag = 0;
            _HandleOutliersFlag = 0;
            _CurrentOutliersSelected = new ArrayList();
            _AmSynchronized = false;
            _Alpha = new decimal[1];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="si"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo si, StreamingContext context)
        {
            si.AddValue("Name", _Name, typeof(string));
            si.AddValue("IsActive", _IsActive, typeof(bool));
            si.AddValue("IsUserFunction", _IsUserFunction, typeof(bool));
            si.AddValue("AmSynchronizer", _AmSynchronizer, typeof(bool));

            si.AddValue("amUranium", _amUranium, typeof(bool));
            si.AddValue("amPbLead", _amPbLead, typeof(bool));
            si.AddValue("fractionationCorrected", _fractionationCorrected, typeof(bool));
            si.AddValue("meanAlphaU", _meanAlpha, typeof(decimal));
            si.AddValue("oxidationCorrected", _oxidationCorrected, typeof(bool));

            si.AddValue("DataPenSize", _DataPenSize, typeof(float));
            si.AddValue("CyclesPerBlock", _CyclesPerBlock, typeof(int));
            si.AddValue("UndoHistory", _UndoHistory, typeof(ArrayList));
            si.AddValue("Ratios", _Ratios, typeof(double[]));
            si.AddValue("Alpha", _Alpha, typeof(decimal[]));
            si.AddValue("ActiveRatios", _ActiveRatios, typeof(ArrayList));
            si.AddValue("ActiveRatiosMap", _ActiveRatiosMap, typeof(ArrayList));
            //		si.AddValue("DiscardOutliers", _DiscardOutliers, typeof( bool ));

            si.AddValue("AllMin", _AllMin, typeof(double));
            si.AddValue("AllMax", _AllMax, typeof(double));
            si.AddValue("AllMean", _AllMean, typeof(double));
            si.AddValue("AllStdDev", _AllStdDev, typeof(double));
            si.AddValue("AllStdErr", _AllStdErr, typeof(double));
            si.AddValue("AllPctStdErr", _AllPctStdErr, typeof(double));
            si.AddValue("AllCount", _AllCount, typeof(double));
            si.AddValue("AllMissing", _AllMissing, typeof(int));

            si.AddValue("AutoCalculate", _AutoCalculate, typeof(bool));
            si.AddValue("HistogramBinCount", _HistogramBinCount, typeof(int));
            si.AddValue("StatsDetailDisplay", _StatsDetailDisplay, typeof(int));
            si.AddValue("HandleDiscardsFlag", _HandleDiscardsFlag, typeof(int));
            si.AddValue("HandleOutliersFlag", _HandleOutliersFlag, typeof(int));
            si.AddValue("CurrentOutliersSelected", _CurrentOutliersSelected, typeof(ArrayList));
            si.AddValue("AmSynchronized", _AmSynchronized, typeof(bool));

        }

        #endregion Serialization and deserialization

        #region Properties

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }
        public bool IsUserFunction
        {
            get { return _IsUserFunction; }
            set { _IsUserFunction = value; }
        }
        public bool AmSynchronizer
        {
            get { return _AmSynchronizer; }
            set { _AmSynchronizer = value; }
        }
        public bool AmSynchronized
        {
            get { return _AmSynchronized; }
            set { _AmSynchronized = value; }
        }


        public bool amUranium
        {
            get { return _amUranium; }
            set { _amUranium = value; }
        }
        public bool amPbLead
        {
            get { return _amPbLead; }
            set { _amPbLead = value; }
        }
        public bool fractionationCorrected
        {
            get { return _fractionationCorrected; }
            set { _fractionationCorrected = value; }
        }

        public bool OxidationCorrected
        {
            get { return _oxidationCorrected; }
            set { _oxidationCorrected = value; }
        }

        public decimal MeanAlpha
        {
            get
            {
                // calculate mean from active ratio values
                int dataCount = 0;
                decimal total = 0.0m;
                for (int index = 0; index < Ratios.Length; index++)
                {
                    if (Ratios[index] > 0)
                    {
                        total += Alpha[index];
                        dataCount++;
                    }
                }
                if (dataCount > 0)
                    _meanAlpha = total / (decimal)dataCount;
                else
                    _meanAlpha = 0.0m;

                return _meanAlpha;
            }
        }

        public float DataPenSize
        {
            get
            {
                return _DataPenSize;
            }
            set
            {
                _DataPenSize = value;
                //_AmSynchronized = false;
            }
        }
        public ArrayList UndoHistory
        {
            get
            {
                return _UndoHistory;
            }
            set
            {
                // deep copy
                _UndoHistory = new ArrayList();
                for (int row = 0; row < ((ArrayList)value).Count; row++)
                {
                    _UndoHistory.Add(new ArrayList());
                    for (int col = 0; col < ((ArrayList)((ArrayList)value)[row]).Count; col++)
                    {
                        ((ArrayList)_UndoHistory[row])
                            .Add(((ArrayList)((ArrayList)value)[row])[col]);
                    }
                }
                //_AmSynchronized = false;

            }
        }
        public int CyclesPerBlock
        {
            get
            {
                return _CyclesPerBlock;
            }
            set
            {
                _CyclesPerBlock = value;
            }
        }

        public double[] Ratios
        {
            get
            {
                return _Ratios;
            }
            set
            {
                //_Ratios = value;
                // March 2007 introduce a deep copy
                // since this array contains double the array.copy method works fine

                // _Ratios = value;
                _Ratios = new double[((double[])value).Length];
                Array.Copy(((double[])value), _Ratios, ((double[])value).Length);

                // may 2007 setup Alpha
                _Alpha = new decimal[_Ratios.Length];


                CalcStats();

                // on creation, we save the original min and max
                _AllMin = _Min;
                _AllMax = _Max;
                _AllMean = _Mean;
                _AllStdDev = _StdDev;
                _AllStdErr = _StdErr;
                _AllPctStdErr = _PctStdErr;
                _AllCount = (int)_LiveDataCount;//Ratios.Length;
                _AllMissing = (int)Ratios.Length - (int)_AllCount;
            }
        }
        public ArrayList ActiveRatios
        {
            get
            {
                return _ActiveRatios;
            }
            set
            {
                _ActiveRatios = value;
                //_AmSynchronized = false;
            }
        }
        public ArrayList ActiveRatiosMap
        {
            get
            {
                return _ActiveRatiosMap;
            }
            set
            {
                _ActiveRatiosMap = value;
                //_AmSynchronized = false;
            }
        }

        public double Min
        {
            get
            {
                return _Min;
            }
            set
            {
                _Min = value;
            }
        }
        public double Max
        {
            get
            {
                return _Max;
            }
            set
            {
                _Max = value;
            }
        }
        public double AllMin
        {
            get
            {
                return _AllMin;
            }
            set
            {
                _AllMin = value;
            }
        }
        public double AllMax
        {
            get
            {
                return _AllMax;
            }
            set
            {
                _AllMax = value;
            }
        }

        public double Mean
        {
            get
            {
                return _Mean;
            }
            set
            {
                _Mean = value;
            }
        }
        public double StdDev
        {
            get
            {
                return _StdDev;
            }
            set
            {
                _StdDev = value;
            }
        }
        public double StdErr
        {
            get
            {
                return _StdErr;
            }
            set
            {
                _StdErr = value;
            }
        }
        public double PctStdErr
        {
            get
            {
                return _PctStdErr;
            }
            set
            {
                _PctStdErr = value;
            }
        }
        public double AllMean
        {
            get
            {
                return _AllMean;
            }
            set
            {
                _AllMean = value;
            }
        }
        public double AllStdDev
        {
            get
            {
                return _AllStdDev;
            }
            set
            {
                _AllStdDev = value;
            }
        }
        public double AllStdErr
        {
            get
            {
                return _AllStdErr;
            }
            set
            {
                _AllStdErr = value;
            }
        }
        public double AllPctStdErr
        {
            get
            {
                return _AllPctStdErr;
            }
            set
            {
                _AllPctStdErr = value;
            }
        }
        public double LiveDataCount
        {
            get
            {
                return _LiveDataCount;
            }
            set
            {
                _LiveDataCount = value;
                //_AmSynchronized = false;
            }
        }
        public double AllCount
        {
            get
            {
                return _AllCount;
            }
            set
            {
                _AllCount = value;
            }
        }
        public int AllMissing
        {
            get { return _AllMissing; }
            set { _AllMissing = value; }
        }

        public bool AutoCalculate
        {
            get
            {
                return _AutoCalculate;
            }
            set
            {
                _AutoCalculate = value;
                //_AmSynchronized = false;
            }
        }

        public int HistogramBinCount
        {
            get
            {
                return _HistogramBinCount;
            }
            set
            {
                _HistogramBinCount = value;
                //_AmSynchronized = false;
            }
        }

        /// <summary>
        /// Sets flag for graphs where 1 shows stderr only, 2 adds sigma band, 3 adds 2-sigma band
        /// </summary>
        public int StatsDetailDisplay
        {
            get
            {
                return _StatsDetailDisplay;
            }
            set
            {
                _StatsDetailDisplay = value;
                //_AmSynchronized = false;
            }
        }

        /// <summary>
        /// Sets flag for viewing data: Include, Ignore, Hide Discards
        /// </summary>
        public int HandleDiscardsFlag
        {
            get
            {
                return _HandleDiscardsFlag;
            }
            set
            {
                _HandleDiscardsFlag = value;
                //_AmSynchronized = false;
            }
        }

        /// <summary>
        /// Sets flag for outlier algorithm: Manual, s-sigma, Chauvenet
        /// </summary>
        public int HandleOutliersFlag
        {
            get
            {
                return _HandleOutliersFlag;
            }
            set
            {
                _HandleOutliersFlag = value;
                //_AmSynchronized = false;
            }
        }

        public ArrayList CurrentOutliersSelected
        {
            get
            {
                return _CurrentOutliersSelected;
            }
            set
            {
                _CurrentOutliersSelected = value;
                //_AmSynchronized = false;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public void CalcStats()
        {
            // http://www.forestry.umt.edu/academics/courses/For505/F505%20Formulas.htm

            // First, build _ActiveRatios and Map from _Ratios, ignoring negatives (zapped)
            // calc stats at same time
            ActiveRatios = new ArrayList();
            ActiveRatiosMap = new ArrayList();

            Min = 10.0E6;
            Max = 0.0;
            double sumX = 0.0;
            double sumXX = 0.0;
            LiveDataCount = 0.0;

            for (int index = 0; index < Ratios.Length; index++)
            {
                double X = Ratios[index];
                if ((HandleDiscardsFlag != (int)HandleDiscards.Hide) && (Ratios[index] < 0))//now use zero to represent bad data
                {
                    // here the value is included as a negative number but
                    // not used in calculations
                    ActiveRatios.Add(X);
                }
                if (Ratios[index] == 0)//now use zero to represent bad data
                {
                    ActiveRatios.Add(X);
                    ActiveRatiosMap.Add(index); // this keeps the map correct when outliers hidden
                    // because these 0 values are not true outliers
                    // this line forces display Min = 0;
                }
                if (Ratios[index] > 0)//now use zero to represent bad data
                {
                    ActiveRatios.Add(X);
                    ActiveRatiosMap.Add(index);
                    LiveDataCount++;

                    sumX += X;
                    sumXX += (X * X);
                    if (Ratios[index] < Min) Min = Ratios[index];
                    if (Ratios[index] > Max) Max = Ratios[index];
                }
            }

            // dec 2006 technique to suppress recalculation until user wants it
            if (!_AutoCalculate) return;

            // march 2005 a little safety - check for division by 0
            if (LiveDataCount == 0)
            {
                Mean = 0;
                StdDev = 0;
                StdErr = 0;
                PctStdErr = 0;
            }
            else if (LiveDataCount == 1)
            {
                Mean = sumX;
                StdDev = 0;
                StdErr = StdDev / Math.Sqrt(LiveDataCount);
                PctStdErr = (100.0) * StdErr / Mean;
            }
            else
            {
                Mean = sumX / LiveDataCount;
                StdDev = Math.Sqrt((sumXX - ((sumX * sumX) / LiveDataCount)) / (LiveDataCount - 1.0));
                // standard error of mean = standard deviation/square root(n)
                StdErr = StdDev / Math.Sqrt(LiveDataCount);
                PctStdErr = (100.0) * StdErr / Mean;
            }

            // added jan 2010 to experiemnt with Gehrels data
            CalculateBlockMeans();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sigmaCount"></param>
        /// <returns></returns>
        public bool IsDataPointInTolerance(double data, int sigmaCount)
        {
            if (StdDev == 0)
                return true; // changed dec 2005 false;
            else
                return (Math.Abs(data - Mean) / StdDev) <= (double)sigmaCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int DiscardRatio(int index)
        {
            // set value to negative of self - then ignored in calcstats
            int retval = getRatioIndex(index);
            if (retval >= 0)
                Ratios[retval] *= -1.0;

            return retval;
        }

        public int getRatioIndex(int index)
        {
            // the index refers to the index of the ActiveRatioMap which
            // contains the index to the rawRatios
            // test for last value and preserve to prevent division by zero
            // also if already negative, we ignore
            int retval = -1; // default
            if ((ActiveRatios.Count > 1))//&& ((double)ActiveRatios[index] >= 0))
            {
                if (HandleDiscardsFlag == (int)HandleDiscards.Hide)
                    retval = (int)ActiveRatiosMap[index];
                else
                    retval = index;
            }
            return retval;
        }

        public void ToggleRatio(int index)
        {
            // toggles state
            Ratios[index] *= -1;
        }

        public void RestoreRatio(int index)
        {
            // set value positive
            Ratios[index] = Math.Abs(Ratios[index]);
        }

        public void RestoreRatios()
        {
            // sets all values positive
            for (int index = 0; index < Ratios.Length; index++)
                RestoreRatio(index);
        }
        public void SynchRatios(RawRatio masterRatio)
        {
            // set the same items negative in this ratio set
            RestoreRatios();
            for (int ratio = 0; ratio < Ratios.Length; ratio++)
                if (masterRatio.Ratios[ratio] < 0)
                    this.Ratios[ratio] *= -1.0;
            SynchRatioHistory(masterRatio);
            AmSynchronized = true;
        }

        public void SynchRatioHistory(RawRatio masterRatio)
        {
            UndoHistory = masterRatio.UndoHistory;
            //DiscardOutliers = masterRatio.DiscardOutliers;
            DataPenSize = masterRatio.DataPenSize;

            AutoCalculate = masterRatio.AutoCalculate;
            HistogramBinCount = masterRatio.HistogramBinCount;
            StatsDetailDisplay = masterRatio.StatsDetailDisplay;
            HandleDiscardsFlag = masterRatio.HandleDiscardsFlag;
            HandleOutliersFlag = masterRatio.HandleOutliersFlag;
            CurrentOutliersSelected = masterRatio.CurrentOutliersSelected;
            AmSynchronized = true;

        }

        public void SetOutliers(ArrayList dataSelected)
        {
            // get selected values REMEMBER last element is count of data still selected
            // extract count of data still selected, then remove element
            int DataCount = (int)dataSelected[dataSelected.Count - 1];
            //dataSelected.RemoveAt(dataSelected.Count - 1);

            // adding cluster of data
            int undoIndex = UndoHistory.Add(new ArrayList());
            // accommodate the outliers as well
            bool TickTest = (LiveDataCount > (2 + DataCount));
            if (TickTest)
            {
                for (int tick = 0; tick < dataSelected.Count - 1; tick++)
                {
                    int retval = DiscardRatio((int)dataSelected[tick]);
                    // default return value of DiscardRatio is -1 -> there is only one activeratio
                    if (retval >= 0)
                        ((ArrayList)UndoHistory[undoIndex]).Add(retval);
                }
                // check for empty undo
                if (((ArrayList)UndoHistory[undoIndex]).Count == 0)
                    UndoHistory.RemoveAt(undoIndex);

                CalcStats();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSelected"></param>
        public void SetBlockOutliers(ArrayList dataSelected)
        {
            // prepare dataSelected per new Rule for blocks:
            // the whole block will be colored the minority color (0 counts), tie = red
            int DataCount = (int)dataSelected[dataSelected.Count - 1];
            dataSelected.RemoveAt(dataSelected.Count - 1);


            bool TickTest = (LiveDataCount > (2 + DataCount));
            if (TickTest)
            {
                int voter = 0;
                for (int tick = 0; tick < dataSelected.Count; tick++)
                {
                    int retval = getRatioIndex((int)dataSelected[tick]);
                    if (retval >= 0)
                        voter += Math.Sign(Ratios[retval]);
                }
                if (voter == 0) voter = -1;
                for (int tick = 0; tick < dataSelected.Count; tick++)
                {
                    int retval = getRatioIndex((int)dataSelected[tick]);
                    if (retval >= 0)
                        Ratios[retval] = (double)Math.Abs(Ratios[retval]) * (double)(Math.Sign(voter));
                }
            }
            else
            {
                // all points go black as red would wipe out all points
                for (int tick = 0; tick < dataSelected.Count; tick++)
                {
                    int retval = getRatioIndex((int)dataSelected[tick]);
                    if (retval >= 0)
                        Ratios[retval] = (double)Math.Abs(Ratios[retval]);
                }
            }

            dataSelected.Add(DataCount); // restore it
            SetOutliers(dataSelected);
        }


        // march 2007 re-factor from ratiograph
        public void RestoreData()
        {
            RestoreRatios();
            UndoHistory = new ArrayList();
            CalcStats();

            HandleDiscardsFlag = (int)RawRatio.HandleDiscards.Include;

            HandleOutliersFlag = (int)RawRatio.HandleOutliers.Manual;

            CurrentOutliersSelected = new ArrayList();
        }


        // April 2007 to work with export to upbredux
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string checkForUPbReduxRatioName() //ETO = EARTHTIME.org
        {
            // modified Jan 2008 to handle oxide-corrected ratio names
            int index = this.Name.IndexOf("OxideCor:");
            if (index == -1) index = this.Name.Length;

            string tempName = this.Name.Substring(0, index);

            // dec 2009 modified to handle more types of naming specifically thermo finnigan triton
            tempName = getSimpleName(tempName);

            // July 2012
            // handle 238.O2/235.O2 = 270/267 , etc. when NOT OXIDECORRECTED by Tripoli
            if ( (!this.Name.Contains("OxideCor:")) ||(this.Name.Contains("O2")))
            {
                if (tempName.Contains("270/267"))
                    return "238_235";
                 if (tempName.Contains("265/267"))
                    return "233_235";
            }

            foreach (string r in DataDictionary.UPbReduxMeasuredRatioNames)
            {
                //int index = this.Name.IndexOf("OxideCor:");
                //if (index == -1) index = this.Name.Length;

                //string tempName = this.Name.Substring(0, index);

                //// dec 2009 modified to handle more types of naming specifically thermo finnigan triton
                //tempName = getSimpleName(tempName);

                if (tempName.Contains(r.Replace("_", "/")))
                    return r;

               
            }
            return "";
        }

        public string getSimpleName(string tempName)
        {
            tempName = tempName.Replace("Pb", "");
            tempName = tempName.Replace("U", "");
         // July 2012   
          //  tempName = tempName.Replace(".O2", "");
            tempName = tempName.Replace("238.O2", "270");
            tempName = tempName.Replace("235.O2", "267");
            tempName = tempName.Replace("233.O2", "265");

            tempName = tempName.Replace("238O2", "270");
            tempName = tempName.Replace("235O2", "267");
            tempName = tempName.Replace("233O2", "265");

            tempName = tempName.Replace("238.16O2", "270");
            tempName = tempName.Replace("235.16O2", "267");
            tempName = tempName.Replace("233.16O2", "265");

            tempName = tempName.Replace("1:", "");
            tempName = tempName.Replace("2:", "");
            tempName = tempName.Replace("3:", "");
            tempName = tempName.Replace("4:", "");
            tempName = tempName.Replace("5:", "");
            tempName = tempName.Replace("6:", "");
            tempName = tempName.Replace("7:", "");
            tempName = tempName.Replace("8:", "");
            tempName = tempName.Replace("9:", "");

            return tempName;
        }

        // jan 2010 play with Gehrels techniques
        private void SortBlocksAscending(int style)
        {
            // style 0 ==> do nothing
            // style 1 ==> toss high2, low 2
            // style 3 ==> toss lower 4, fit line somehow later
            RestoreData();

            SavedRatios = new double[Ratios.Length];
            Ratios.CopyTo(SavedRatios, 0);

            for (int block = 0; block < (Ratios.Length / CyclesPerBlock); block ++)
            {
                int startIndex = block * CyclesPerBlock;
                Array.Sort(Ratios, startIndex, CyclesPerBlock);
                if (style == 1)
                {
                    Ratios[startIndex] = Ratios[startIndex] * -1.0;
                    Ratios[startIndex + 1] = Ratios[startIndex + 1] * -1.0;
                    Ratios[startIndex + CyclesPerBlock - 2] = Ratios[startIndex + CyclesPerBlock - 2] * -1.0;
                    Ratios[startIndex + CyclesPerBlock - 1] = Ratios[startIndex + CyclesPerBlock - 1] * -1.0;
               }
            }
        }

        private void UnSortBlocks()
        {
            SavedRatios.CopyTo(Ratios, 0);
            RestoreData();
        }

        public void ToggleBlockSort(bool amSorted)
        {
            //if (amSorted)
            //    UnSortBlocks();
            //else
            //    SortBlocksAscending(1);
        }

        public void CalculateBlockMeans()
        {
            if (CyclesPerBlock > 0.0)
            {
                _BlockMeans = new double[Ratios.Length / CyclesPerBlock];

                for (int block = 0; block < (Ratios.Length / CyclesPerBlock); block++)
                {
                    double sumX = 0.0;
                    _BlockMeans[block] = 0.0;
                    int blockLiveDataCount = 0;

                    for (int index = block * CyclesPerBlock; index < ((block + 1) * CyclesPerBlock); index++)
                    {
                        double X = Ratios[index];
                        if (Ratios[index] > 0)//now use zero to represent bad data
                        {
                            blockLiveDataCount++;
                            sumX += X;
                        }
                    }

                    if (blockLiveDataCount == 0)
                    {
                        _BlockMeans[block] = 0.0;
                    }
                    else
                    {
                        _BlockMeans[block] = sumX / blockLiveDataCount;
                    }
                }
            }
        }

        public double[] GetBlockMeans()
        {
            return _BlockMeans;
        }

  
        #endregion Methods

    }
}
