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
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace Tripoli.earth_time_org
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [XmlRootAttribute("SampleMetaData",
        Namespace = "http://www.earth-time.org",
        IsNullable = true)]
    public class SampleMetaData
    {
        // NOTE: underscored names are used so that lower-case properties can be used 
        // for xml serialization

        private String _sampleName;
        private String _sampleAnalysisFolderPath;

        private FractionMetaData[] _fractionsMetaData;

 
        // Constructors

        public SampleMetaData() 
        {
            _sampleName = "";
            _sampleAnalysisFolderPath = "";
            _fractionsMetaData = new FractionMetaData[0];
        }

        public string getFractionFilePathForXML_U(string fractionID)
        {
            string retval = "";

            // get fraction entry
            for (int i = 0; i < _fractionsMetaData.Length; i++)
            {
                // toLower makes it case-insensitive
                if ((retval.Length == 0) && (fractionsMetaData[i].fractionID.ToLower().Equals(fractionID.ToLower())))
                {
                    retval = _sampleAnalysisFolderPath //
                        + @"\" + fractionsMetaData[i].aliquotName //
                        + @"\" + fractionsMetaData[i].fractionXMLUPbReduxFileName__U;
                }
            }

            return retval;           
        }

        public string getFractionFilePathForXML_Pb(string fractionID)
        {
            string retval = "";

            // get fraction entry
            for (int i = 0; i < _fractionsMetaData.Length; i++)
            {
                // toLower makes it case-insensitive
                if ((retval.Length == 0) && (fractionsMetaData[i].fractionID.ToLower().Equals(fractionID.ToLower())))
                {
                    retval = _sampleAnalysisFolderPath //
                        + @"\" + fractionsMetaData[i].aliquotName //
                        + @"\" + fractionsMetaData[i].fractionXMLUPbReduxFileName__Pb;
                }
            }

            return retval;
        }

        public Tripoli.earth_time_org.SampleMetaData ReadSampleMetaData(string filename)
        {
            // Create an instance of the XmlSerializer class;
            // specify the type of object to be deserialized.
            XmlSerializer serializer = new XmlSerializer(typeof(SampleMetaData));

            /* If the XML document has been altered with unknown 
            nodes or attributes, handle them with the 
            UnknownNode and UnknownAttribute events.*/
            serializer.UnknownNode += new
                    XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new
                    XmlAttributeEventHandler(serializer_UnknownAttribute);
            serializer.UnknownElement += new
                XmlElementEventHandler(serializer_UnknownElement);

            //// Create the XmlSchemaSet class.
            //XmlSchemaSet sc = new XmlSchemaSet();

            //// Add the schema to the collection.
            //sc.Add(null,//ConfigurationManager.AppSettings["EarthTimeOrgNamespace"],
            //    getTracerSchemaURI());

            //// Set the validation settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Document;
            //settings.ValidationType = ValidationType.Schema;
            //settings.Schemas = sc;
            //settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

            XmlReader reader;

            FileStream fs = null;
            try
            {
                // A FileStream is needed to read the XML document.
                fs = new FileStream(filename, FileMode.Open);

                // Create the XmlReader object.
                reader = XmlReader.Create(fs, settings);
            }
            catch (Exception eFile)
            {
                Console.WriteLine(eFile.Message);
                throw new System.ArgumentException(
                    "File: "
                    + filename
                    + " does not exist.");
            }

            SampleMetaData retVal = (SampleMetaData)serializer.Deserialize(reader);
            fs.Close();
            return retVal;

        }


        private void serializer_UnknownNode
        (object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        private void serializer_UnknownAttribute
        (object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " +
            attr.Name + "='" + attr.Value + "'");
        }

        private void serializer_UnknownElement(object sender, XmlElementEventArgs e)
        { }


        public String sampleName
        {
            get { return _sampleName; }
            set { _sampleName = value; }
        }

        public String sampleAnalysisFolderPath
        {
            get { return _sampleAnalysisFolderPath; }
            set { _sampleAnalysisFolderPath = value; }
        }

        public FractionMetaData[] fractionsMetaData
        {
            get { return _fractionsMetaData; }
            set { _fractionsMetaData = value; }
        }

    }
}
