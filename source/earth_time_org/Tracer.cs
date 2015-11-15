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
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Configuration;
using System.Xml.Xsl;
using System.Windows.Forms;
using Tripoli.earth_time_org;
using Tripoli.utilities;

namespace Tripoli.earth_time_org
{
    /// <summary>
    /// Tracer encodes the data structure for a generic Tracer (aka spike.)
    /// The goal is to introduce into the Tripoli and EARTHTIME data
    /// repositories the use of XML as the universal data transport mechanism.
    /// Tracer can be serialized as either XML or a binary stream.
    /// </summary>
    [Serializable]
    [XmlRootAttribute("Tracer",
        Namespace = "http://www.earth-time.org",
        IsNullable = true)]
    public class Tracer : ISerializable
    {

        private static string schemaName = "TracerXMLSchema.xsd";
        // NOTE: underscored names are used so that lower-case properties can be used 
        // for xml serialization
        // Instance variables
        private string _tracerName;
        private int _versionNumber;
        private string _tracerType;
        private string _labName;
        private DateTime _dateCertified;

        private ValueModel[] _ratios;

        private ValueModel[] _isotopeConcentrations;

        // Constructors

        public Tracer()
        {
            tracerName = "Empty Tracer";
            versionNumber = 0;
            tracerType = DataDictionary.TracerTypes[0]; // defaults to first type
            labName = "Your Lab";
            _dateCertified = DateTime.Now;

            ratios = new ValueModel[DataDictionary.EarthTimeTracerRatioNames.Length];
            for (int i = 0; i < ratios.Length; i++)
            {
                // form name of tracer ratio
                ratios[i] = new ValueModel(DataDictionary.getTracerRatioName(i));
            }
            isotopeConcentrations = new ValueModel[DataDictionary.isotopeNames.Length];
            for (int i = 0; i < isotopeConcentrations.Length; i++)
            {
                isotopeConcentrations[i] =
                    new ValueModel(DataDictionary.getTracerIsotopeConcName(i));
            }
        }

        #region Properties

        // order matters here for XML serialization
        public string tracerName
        {
            get { return _tracerName; }
            set { _tracerName = value; }
        }
        public int versionNumber
        {
            get { return _versionNumber; }
            set { _versionNumber = value; }
        }

        public string tracerType
        {
            get { return _tracerType; }
            set { _tracerType = value; }
        }

        public string labName
        {
            get { return _labName; }
            set { _labName = value; }
        }

        public string dateCertified
        {
            get { return _dateCertified.ToString("yyyy-MM-dd"); }// 1-based month is MM and 0-based is mm
            set
            {
                // this hack is to handle a change in date format during development
                try
                {
                    _dateCertified = Convert.ToDateTime(value + " 00:00:00");
                }
                catch
                {
                    _dateCertified = Convert.ToDateTime(value);
                }
            }
        }

        /* The XmlArrayAttribute changes the XML element name
         from the default of "ratios" to "ratios". */
        [XmlArrayAttribute("ratios")]
        public ValueModel[] ratios
        {
            // no deep copy so can be bound to form data
            get { return _ratios; }
            set { _ratios = value; }
        }

        [XmlArrayAttribute]
        public ValueModel[] isotopeConcentrations
        {
            // no deep copy so can be bound to form data
            get { return _isotopeConcentrations; }
            set { _isotopeConcentrations = value; }
        }

        #endregion Properties

        #region Serialization and Deserialization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public void SerializeXMLTracer(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Tracer));
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
            contents.Insert(3, "        xmlns=\"http://www.earth-time.org\"");
            contents.Insert(4, "        xsi:schemaLocation=\"http://www.earth-time.org");
            contents.Insert(5, "                            " + getTracerSchemaURI() + "\">"); //http://www.earth-time.org/projects/upb/public_data/XSD/TracerXMLSchema.xsd\">");

            // write it back out
            StreamWriter sw = File.CreateText(filename);
            for (int s = 0; s < contents.Count; s++)
            {
                sw.WriteLine((string)contents[s]);
            }
            sw.Close();
        }

        public string getTracerSchemaURI()
        {
            return
                ConfigurationManager.AppSettings["EarthTimeOrgCurrentURL"]
                + ConfigurationManager.AppSettings["EarthTimeOrgCurrentDirectoryForXSD"]
                + schemaName;//"TracerXMLSchema.xsd";
        }

        public Tripoli.earth_time_org.Tracer ReadTracer(string filename)
        {
            // Create an instance of the XmlSerializer class;
            // specify the type of object to be deserialized.
            XmlSerializer serializer = new XmlSerializer(typeof(Tracer));

            /* If the XML document has been altered with unknown 
            nodes or attributes, handle them with the 
            UnknownNode and UnknownAttribute events.*/
            serializer.UnknownNode += new
                    XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new
                    XmlAttributeEventHandler(serializer_UnknownAttribute);
            serializer.UnknownElement += new
                XmlElementEventHandler(serializer_UnknownElement);


            // *** AUGUST 2012 removed validation because of lack of internet at British Geological Survey lab

            ////////////// Create the XmlSchemaSet class.
            ////////////XmlSchemaSet sc = new XmlSchemaSet();

            ////////////// Test for connectivity and then add the schema to the collection.
            ////////////try
            ////////////{
            ////////////    TripoliUtilities.getWebStream(getTracerSchemaURI());
            ////////////    sc.Add(null, getTracerSchemaURI());
            ////////////}
            ////////////catch (Exception eFile)
            ////////////{
            ////////////    Console.WriteLine(eFile.Message);
            ////////////    System.Windows.Forms.MessageBox.Show(
            ////////////         "File: "
            ////////////        + getTracerSchemaURI()
            ////////////        + " cannot be reached ... check your Internet connection.\n\nTripoli will attempt to load Tracer file without validation.",
            ////////////        "Tripoli Warning");
            ////////////    sc = null;
            ////////////}

            //////////////sc.Add(null, getTracerSchemaURI());

            ////////////// Set the validation settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Document;
            ////////if (sc == null)
            ////////{
            settings.ValidationType = ValidationType.None;
            ////////}
            ////////else
            ////////{
            ////////    settings.ValidationType = ValidationType.Schema;
            ////////    settings.Schemas = sc;
            ////////    settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
            ////////}

            XmlReader reader;


            try
            {
                if (filename.StartsWith("http://"))
                {
                    // Create the XmlReader object.
                    reader = XmlReader.Create(TripoliUtilities.getWebStream(filename), settings);
                }
                else
                {
                    // A FileStream is needed to read the XML document.
                    FileStream fs = new FileStream(filename, FileMode.Open);

                    // Create the XmlReader object.
                    reader = XmlReader.Create(fs, settings);
                }

            }
            catch (ArgumentNullException eFile)
            {
                Console.WriteLine(eFile.Message);
                throw new System.ArgumentException(
                    "File: "
                    + filename
                    + " does not exist.");
            }

            return (Tracer)serializer.Deserialize(reader);

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

        // Display any validation errors.
        private static void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show(
                "XML validation reports error: \n\n" + e.Message
                + "\n\n Tripoli will load the XML data, but you should fix the problem.",
                "Tripoli Warning");
        }

        protected Tracer(SerializationInfo si, StreamingContext context)
        {
            tracerName = (string)si.GetValue("tracerName", typeof(string));
            versionNumber = (int)si.GetValue("versionNumber", typeof(int));
            tracerType = (string)si.GetValue("tracerType", typeof(string));
            labName = (string)si.GetValue("labName", typeof(string));
            dateCertified = (string)si.GetValue("dateCertified", typeof(string));

            ratios = (ValueModel[])si.GetValue("ratios", typeof(ValueModel[]));
            isotopeConcentrations = (ValueModel[])si.GetValue("isotopeConcentrations", typeof(ValueModel[]));
        }


        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo si, StreamingContext context)
        {
            si.AddValue("tracerName", tracerName, typeof(string));
            si.AddValue("versionNumber", versionNumber, typeof(int));
            si.AddValue("tracerType", tracerType, typeof(string));
            si.AddValue("labName", labName, typeof(string));
            si.AddValue("dateCertified", dateCertified, typeof(string));

            si.AddValue("ratios", ratios, typeof(ValueModel[]));
            si.AddValue("isotopeConcentrations", isotopeConcentrations, typeof(ValueModel[]));
        }
        #endregion Serialization and Deserialization

        #region Methods

        public string getNameAndVersion()
        {
            return tracerName.Trim() + " v:" + versionNumber;
        }

        internal ValueModel getRatioByName(string ratioName)
        {
            for (int i = 0; i < ratios.Length; i++)
            {
                if (ratios[i].name.Equals(ratioName))
                    return ratios[i]; // returning pointer so it can be bound to form
            }
            return new ValueModel("Missing");
        }

        internal ValueModel getIsotopeConcByName(string isotopeConc)
        {
            for (int i = 0; i < isotopeConcentrations.Length; i++)
            {
                if (isotopeConcentrations[i].name.Equals(isotopeConc))
                    return isotopeConcentrations[i];
            }
            return new ValueModel("Missing");
        }

        public string[] getListOfEarthTimeTracers()
        {
            string temp =
                TripoliUtilities.getTextFromURI(ConfigurationManager.AppSettings["EarthTimeOrgTracerListUri"]);
            // split on \n
            string[] retVal = temp.Split(new char[] { '\n' });

            return retVal;
        }

        /// <summary>
        /// Writes contents of XSLT translator to temporary file and uses it to
        /// render this Tracer to a specific file name.
        /// </summary>
        /// <param name="contentsOfXSLT"></param>
        /// <param name="targetFileName"></param>
        public void RenderTracerWithXSL(string contentsOfXSLT, string targetFileName)
        {
            FileInfo XSLtempFI = new FileInfo("XSLtemp.xsl");
            XSLtempFI.Delete();
            StreamWriter s = XSLtempFI.AppendText();
            s.Write(contentsOfXSLT);
            s.Close();

            // Load the style sheet.
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(XSLtempFI.FullName);

            // save the Tracer as xml
            FileInfo tempTracerXML = new FileInfo("tempTracer.xml");
            SerializeXMLTracer(tempTracerXML.FullName);

            // Execute the transform and output the results to a file.
            xslt.Transform("tempTracer.xml", targetFileName);

            tempTracerXML.Delete();
            XSLtempFI.Delete();

            try
            {
                System.Diagnostics.Process.Start(targetFileName);
            }
            catch (System.ArgumentException eFile)
            {
                MessageBox.Show("Failed to create " + targetFileName + " because \n\n"
                    + eFile.Message);
            }
        }


        #endregion Methods

        static void Main(string[] args)
        {
            Console.WriteLine("BariumPhosphate");
            Tracer tr = new Tracer();
            tr.tracerName = "Hell Yes";
            tr.SerializeXMLTracer("Tracer.xml");

            Tracer tr2 = new Tracer();
            tr2 = tr2.ReadTracer("Tracer.xml");

            Console.WriteLine("Tracer read: " + tr2.getNameAndVersion());
        }
    }


}


