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

using Wintellect.PowerCollections;

namespace Tripoli.earth_time_org
{
    /// <summary>
    /// ThalliumIC models the data structure for a generic ThalliumIC.
    /// The goal is to introduce into the Tripoli and EARTHTIME data
    /// repositories the use of XML as the universal data transport mechanism.
    /// BariumPhosphateIC can be serialized as either XML or a binary stream.
    /// </summary>
    [Serializable]
    [XmlRootAttribute("ThalliumIC",
        Namespace = "http://www.earth-time.org",
        IsNullable = true)]
    public class ThalliumIC : ISerializable
    {
        // Instance variables
        private string _thalliumICName;
        private int _versionNumber;
        private string _labName;
        private DateTime _dateCertified;

        private ValueModel[] _isotopeAbundances;

        // Constructors
        /// <summary>
        /// 
        /// </summary>
        public ThalliumIC()
        {
            thalliumICName = "Empty ThalliumIC";
            versionNumber = 0;
            labName = "Your Lab";
            _dateCertified = DateTime.Now;

            isotopeAbundances = new ValueModel[DataDictionary.EarthTimeThalliumICIsotopeNames.Length];
            for (int i = 0; i < isotopeAbundances.Length; i++)
            {
                isotopeAbundances[i] =
                    new ValueModel(DataDictionary.getEarthTimeThalliumICIsotopeNames(i));
                isotopeAbundances[i].uncertaintyType = "NONE";
            }
        }

        #region Properties

        // order matters here for XML serialization
        /// <summary>
        /// 
        /// </summary>
        public string thalliumICName
        {
            get { return _thalliumICName; }
            set { _thalliumICName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int versionNumber
        {
            get { return _versionNumber; }
            set { _versionNumber = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string labName
        {
            get { return _labName; }
            set { _labName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        [XmlArrayAttribute]
        public ValueModel[] isotopeAbundances
        {
            // no deep copy so can be bound to form data
            get { return _isotopeAbundances; }
            set { _isotopeAbundances = value; }
        }

        #endregion Properties

        #region Serialization and Deserialization

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public void SerializeXMLThalliumIC(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ThalliumIC));
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
            contents.Insert(5, "                            " + getThalliumICSchemaURI() + "\">"); 

            // write it back out
            StreamWriter sw = File.CreateText(filename);
            for (int s = 0; s < contents.Count; s++)
            {
                sw.WriteLine((string)contents[s]);
            }
            sw.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string getThalliumICSchemaURI()
        {
            return
                ConfigurationManager.AppSettings["EarthTimeOrgCurrentURL"]
                + ConfigurationManager.AppSettings["EarthTimeOrgCurrentDirectoryForXSD"]
                + "ThalliumICXMLSchema.xsd";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public Tripoli.earth_time_org.ThalliumIC ReadThalliumIC(string filename)
        {
            // Create an instance of the XmlSerializer class;
            // specify the type of object to be deserialized.
            XmlSerializer serializer = new XmlSerializer(typeof(ThalliumIC));

            /* If the XML document has been altered with unknown 
            nodes or attributes, handle them with the 
            UnknownNode and UnknownAttribute events.*/
            serializer.UnknownNode += new
                    XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new
                    XmlAttributeEventHandler(serializer_UnknownAttribute);
            serializer.UnknownElement += new
                XmlElementEventHandler(serializer_UnknownElement);

            // Create the XmlSchemaSet class.
            XmlSchemaSet sc = new XmlSchemaSet();

            // Add the schema to the collection.
            sc.Add(ConfigurationManager.AppSettings["EarthTimeOrgNamespace"],
                getThalliumICSchemaURI());

            // Set the validation settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = sc;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

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

            return (ThalliumIC)serializer.Deserialize(reader);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="si"></param>
        /// <param name="context"></param>
        protected ThalliumIC(SerializationInfo si, StreamingContext context)
        {
            thalliumICName = (string)si.GetValue("thalliumICName", typeof(string));
            versionNumber = (int)si.GetValue("versionNumber", typeof(int));
            labName = (string)si.GetValue("labName", typeof(string));
            dateCertified = (string)si.GetValue("dateCertified", typeof(string));

            isotopeAbundances = (ValueModel[])si.GetValue("isotopeAbundances", typeof(ValueModel[]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="si"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo si, StreamingContext context)
        {
            si.AddValue("thalliumICName", thalliumICName, typeof(string));
            si.AddValue("versionNumber", versionNumber, typeof(int));
            si.AddValue("labName", labName, typeof(string));
            si.AddValue("dateCertified", dateCertified, typeof(string));

            si.AddValue("isotopeAbundances", isotopeAbundances, typeof(ValueModel[]));
        }
        #endregion Serialization and Deserialization

        #region Methods

        /// <summary>
        /// Hard-coded EARTHTIME model as of 1 Oct 2009 to be used as the general default.
        /// </summary>
        /// <returns></returns>
        public static ThalliumIC EARTHTIME_TlIC()
        {
            ThalliumIC retVal = new ThalliumIC();
            retVal.thalliumICName = "EARTHTIME";
            retVal.versionNumber = 1;
            retVal.dateCertified = "10/01/2009";
            retVal.labName = "EARTHTIME.ORG";

            retVal.getIsotopeAbundanceByName("pct203Tl_Tl").value = 29.52m;
            retVal.getIsotopeAbundanceByName("pct205Tl_Tl").value = 70.48m;

            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string getNameAndVersion()
        {
            return thalliumICName.Trim() + " v:" + versionNumber;
        }

        internal ValueModel getIsotopeAbundanceByName(string isotopeAbundance)
        {
            for (int i = 0; i < isotopeAbundances.Length; i++)
            {
                if (isotopeAbundances[i].name.Equals(isotopeAbundance))
                    return isotopeAbundances[i];
            }
            return new ValueModel(isotopeAbundance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string[] getListOfEarthTimeThalliumICs()
        {
            string temp =
                TripoliUtilities.getTextFromURI(ConfigurationManager.AppSettings["EarthTimeOrgThalliumICNameListUri"]);
            // split on \n
            string[] retVal = temp.Split(new char[] { '\n' });

            return retVal;
        }

        /// <summary>
        /// Writes contents of XSLT translator to temporary file and uses it to
        /// render this BariumPhosphateIC to a specific file name.
        /// </summary>
        /// <param name="contentsOfXSLT"></param>
        /// <param name="targetFileName"></param>
        public void RenderThalliumICWithXSL(string contentsOfXSLT, string targetFileName)
        {
            FileInfo XSLtempFI = new FileInfo("XSLtemp.xsl");
            XSLtempFI.Delete();
            StreamWriter s = XSLtempFI.AppendText();
            s.Write(contentsOfXSLT);
            s.Close();

            // Load the style sheet.
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(XSLtempFI.FullName);

            // save the ThalliumIC as xml
            FileInfo tempThalliumICXML = new FileInfo("tempThalliumIC.xml");
            SerializeXMLThalliumIC(tempThalliumICXML.FullName);

            // Execute the transform and output the results to a file.
            xslt.Transform("tempThalliumIC.xml", targetFileName);

            tempThalliumICXML.Delete();
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


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public OrderedDictionary<string, decimal> calculateIsotopicComposition()
        {
            // http://www.codeplex.com/PowerCollections
            OrderedDictionary<string, decimal> retVal = new OrderedDictionary<string, decimal>();

            OrderedDictionary<int, decimal> isotopeTl = new OrderedDictionary<int, decimal>();

            for (int i = 0; i < isotopeAbundances.Length; i++)
            {
                ValueModel vm = (ValueModel)isotopeAbundances.GetValue(i);
                string atomicMassText = vm.name.Substring(3, 3);
                    isotopeTl.Add(Convert.ToInt32(atomicMassText), vm.value);
            }

            // calculate isotopic composition of Tl
            decimal v205 = 0m;
            isotopeTl.TryGetValue(205, out v205);

            if (v205 > 0m)
            {
                decimal v203 = 0m;
                isotopeTl.TryGetValue(203, out v203);
                retVal.Add("r203_205Tl", v203 / v205);
            }
            return retVal;
        }

        #endregion Methods

        static void Main(string[] args)
        {
            Console.WriteLine("Thallium");
            ThalliumIC t = new ThalliumIC();
            t._thalliumICName = "Hell Thallium";
            t.SerializeXMLThalliumIC("Thallium.xml");

            ThalliumIC t2 = new ThalliumIC();
            t2 = t2.ReadThalliumIC("Thallium.xml");

            Console.WriteLine("Thallium read: " + t2.getNameAndVersion());
        }

    }


}


