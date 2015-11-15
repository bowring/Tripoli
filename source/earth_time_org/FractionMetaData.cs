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
using System.Collections.Generic;
using System.Text;

namespace Tripoli.earth_time_org
{
    [Serializable]
    public class FractionMetaData
    {
        private string _fractionID;

        public string fractionID
        {
            get { return _fractionID; }
            set { _fractionID = value; }
        }

        private string _aliquotName;

        public string aliquotName
        {
            get { return _aliquotName; }
            set { _aliquotName = value; }
        }

        private string _fractionXMLUPbReduxFileName_U;

        public string fractionXMLUPbReduxFileName__U // double underscore handles quirk of xstream in U-Pb_Redux
        {
            get { return _fractionXMLUPbReduxFileName_U; }
            set { _fractionXMLUPbReduxFileName_U = value; }
        }

        private string _fractionXMLUPbReduxFileName_Pb;

        public string fractionXMLUPbReduxFileName__Pb // double underscore handles quirk of xstream in U-Pb_Redux
        {
            get { return _fractionXMLUPbReduxFileName_Pb; }
            set { _fractionXMLUPbReduxFileName_Pb = value; }
        }



        public FractionMetaData() { }



    }
}
