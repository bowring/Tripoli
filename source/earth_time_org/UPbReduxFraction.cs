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
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Configuration;

namespace Tripoli.earth_time_org
{
    /// <summary>
    /// Defines and produces fractions for use with www.earth-time.org U-pb redux program.
    /// Provides for recording the actual Tracer used to process the fraction, if any.
    /// </summary>
    //[Serializable]
    [XmlRootAttribute("UPbReduxFraction",
        Namespace = "https://raw.githubusercontent.com/EARTHTIME/Schema",
        IsNullable = true)]
    public class UPbReduxFraction //: ISerializable
    {
        private static string schemaName = "UPbReduxInputXMLSchema.xsd";
        // Fields
        private string _sampleName;
        private string _fractionID;
        private string _ratioType;
        private string _pedigree;

        private MeasuredRatioModel[] _measuredRatios;

        private decimal _meanAlphaU;
        private decimal _meanAlphaPb;
        private decimal _r18O_16O;
        private decimal _labUBlankMass;
        // april 2008
        private decimal _r238_235b;
        private decimal _r238_235s;
        private decimal _tracerMass;



        private Tracer appliedTracer;

        public UPbReduxFraction()//string sampleName, string _fractionID)
        {
            this._sampleName = "";
            this._fractionID = "";
            this._ratioType = "";
            this._pedigree = "";

            _measuredRatios = new MeasuredRatioModel[DataDictionary.UPbReduxMeasuredRatioNames.Length];
            for (int i = 0; i < _measuredRatios.Length; i++)
            {
                _measuredRatios[i] = 
                    new MeasuredRatioModel(
                        DataDictionary.UPbReduxMeasuredRatioNames[i], 0.0m, 0.0m, false, false);
            }

            this._meanAlphaU = 0.0m;
            this._meanAlphaPb = 0.0m;
            this._r18O_16O = 0.0m;

            AppliedTracer = new Tracer();

        }

        #region Serialization and Deserialization

        public void SerializeXMLUPbReduxFraction(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UPbReduxFraction));

            try
            {
                TextWriter writer = new StreamWriter(filename);

                serializer.Serialize(writer, this);

                writer.Close();



                // add in the schema location and copyright
                // read it back in
                ArrayList contents = new ArrayList();
                StreamReader sr = File.OpenText(filename);
                String input;
                while ((input = sr.ReadLine()) != null)
                {
                    contents.Add(input);
                }
                sr.Close();

                // edit it
                string[] temp = new string[1];
                temp = ((string)contents[1]).Split(new string[] { "xmlns" }, StringSplitOptions.RemoveEmptyEntries);
                Console.WriteLine(temp);

                // fix up contents
                contents.RemoveAt(1);
                contents.Insert(1, temp[0] + "xmlns" + temp[1]);
                contents.Insert(2, "        xmlns" + temp[2]);
                contents.Insert(3, "        xmlns=\"https://raw.githubusercontent.com/EARTHTIME/Schema\"");
                contents.Insert(4, "        xsi:schemaLocation=\"https://raw.githubusercontent.com/EARTHTIME/Schema");
                contents.Insert(5, "                            " + getUPbReduxFractionSchemaURI() + "\">");

                // write it back out
                StreamWriter sw = File.CreateText(filename);
                for (int s = 0; s < contents.Count; s++)
                {
                    sw.WriteLine((string)contents[s]);
                }
                sw.Close();




            }
            catch (Exception e)
            {
                Console.WriteLine("Serializing " + filename + " with problem: " + e.Message);
            }

            // force a later time ??
        }

        public string getUPbReduxFractionSchemaURI()
        {
            return
                ConfigurationManager.AppSettings["EarthTimeOrgCurrentDirectoryForXSD"]
                + schemaName;
        }


/*
        protected UPbReduxFraction(SerializationInfo si, StreamingContext context)
        {
            sampleName = (string)si.GetValue("sampleName", typeof(string));
            _fractionID = (string)si.GetValue("_fractionID", typeof(string));
            _ratioType = (string)si.GetValue("_ratioType", typeof(string));
            _pedigree = (string)si.GetValue("_pedigree", typeof(string));
            ratios = (MeasuredRatioModel[])si.GetValue("ratios", typeof(MeasuredRatioModel[]));
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo si, StreamingContext context)
        {
            si.AddValue("sampleName", sampleName, typeof(string));
            si.AddValue("_fractionID", sampleName, typeof(string));
            si.AddValue("_ratioType", sampleName, typeof(string));
            si.AddValue("_pedigree", sampleName, typeof(string));
            si.AddValue("ratios", ratios, typeof(MeasuredRatioModel[]));
        }
        */
        #endregion Serialization and Deserialization

        #region Properties

        // order matters here for XML serialization
        public string sampleName
        {
            get { return _sampleName; }
            set { _sampleName = value; }
        }

        public string fractionID
        {
            get { return _fractionID; }
            set { _fractionID = value; }
        }

        public string ratioType
        {
            get { return _ratioType; }
            set { _ratioType = value; }
        }

        public string pedigree
        {
            get { return _pedigree; }
            set { _pedigree = value; }
        }

        public MeasuredRatioModel[] measuredRatios
        {
            // need deep copy
            get { return _measuredRatios; }
            set { _measuredRatios = value; }
        }

        public decimal meanAlphaU
        {
            get { return _meanAlphaU; }
            set { _meanAlphaU = value; }
        }

        public decimal meanAlphaPb
        {
            get { return _meanAlphaPb; }
            set { _meanAlphaPb = value; }
        }

        public decimal r18O16O
        {
            get { return _r18O_16O; }
            set { _r18O_16O = value; }
        }

        public decimal labUBlankMass
        {
            get { return _labUBlankMass; }
            set { _labUBlankMass = value; }
        }
        public decimal r238235b
        {
            get { return _r238_235b; }
            set { _r238_235b = value; }
        }
        public decimal r238235s
        {
            get { return _r238_235s; }
            set { _r238_235s = value; }
        }
        public decimal tracerMass
        {
            get { return _tracerMass; }
            set { _tracerMass = value; }
        }
        
        [XmlElement("tracer")]
        public Tracer AppliedTracer
        {
            get { return appliedTracer; }
            set { appliedTracer = value; }
        }

        #endregion Properties


        internal MeasuredRatioModel getRatioByName(string ratioName)
        {
            for (int i = 0; i < _measuredRatios.Length; i++)
            {
                if (_measuredRatios[i].name.Contains(ratioName))
                    return _measuredRatios[i]; // returning pointer so it can be bound to form
            }
            return null;
        }

        public void prepareForExportToUPbRedux(TripoliWorkProduct RawRatios)
        {
            bool typePb = false;
            bool typeU = false;

            // extract ratio names and data from selected ratios
            // todo re-organize so that meanalpha and type are only figured once
            bool detectedFractionationCorrection = false;
            for (int r = 0; r < RawRatios.Count; r++)
            {
                if (((RawRatio)RawRatios[r]).IsActive) // checked ratios
                {
                    // first determine if the checked ratio is a UPbRedux ratio
                    string ratioName = ((RawRatio)RawRatios[r]).checkForUPbReduxRatioName();
                    if (!ratioName.Equals(""))
                    {
                        if (!(typePb || typeU))
                        {
                            if (DataDictionary.getElementNameOfRatio(ratioName).Equals("Pb"))
                                typePb = true;
                            if (DataDictionary.getElementNameOfRatio(ratioName).Equals("U"))
                                typeU = true;
                        }
                        MeasuredRatioModel myRatio = getRatioByName(ratioName);
                        // check to see if already read in (non-zero value of mean) 
                        // ==> user error if not unique
                        if (myRatio.value.Equals(0.0m))
                        {
                            // haven't visited this one yet
                            // refresh values from stored points
                            ((RawRatio)RawRatios[r]).CalcStats();

                            // record ratio into fraction
                            myRatio.value = (Decimal)((RawRatio)RawRatios[r]).Mean;
                            myRatio.oneSigma = (Decimal)((RawRatio)RawRatios[r]).PctStdErr;
                            myRatio.fracCorr = ((RawRatio)RawRatios[r]).fractionationCorrected;
                            myRatio.oxideCorr = ((RawRatio)RawRatios[r]).OxidationCorrected;

                            try
                            {
                                if ((!detectedFractionationCorrection) && myRatio.fracCorr)
                                {
                                    if (typeU)
                                    {
                                        meanAlphaU = ((RawRatio)RawRatios[r]).MeanAlpha;
                                        _labUBlankMass = RawRatios.UsampleComponents.labUBlankMass;
                                        r238235b = RawRatios.UsampleComponents.r238_235b;
                                        r238235s = RawRatios.UsampleComponents.r238_235s;
                                        tracerMass = RawRatios.UsampleComponents.tracerMass;
                                    }
                                    if (typePb)
                                        meanAlphaPb = ((RawRatio)RawRatios[r]).MeanAlpha;
                                    detectedFractionationCorrection = true;
                                    Console.WriteLine(meanAlphaU + "  " + meanAlphaPb);
                                }
                            }
                            catch (Exception eOldRatio)
                            {
                                // older trip files have no MeanAlpha
                                meanAlphaU = 0.0m;
                                meanAlphaPb = 0.0m;
                                Console.WriteLine(eOldRatio.Message);
                            }

                        }
                        else
                        {
                            // need to throw error
                            return; // this causes error because fraction type never set
                        }
                    }

                }
            }

            // determine fraction type
            if (typeU) ratioType += "U";
            if (typePb) ratioType += "Pb";

            //MeanAlpha = 

        }

    }
}
