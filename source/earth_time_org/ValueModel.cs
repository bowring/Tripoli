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

namespace Tripoli.earth_time_org
{
    [Serializable]
    public class ValueModel
    {
        // NOTE: underscored names are used so that lower-case properties can be used 
        // for xml serialization
        private string _name;
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }       
        
        private decimal _value;
        public decimal value
        {
            get { return _value; }
            set { _value = value; }
        }

        private string _uncertaintyType;
        public string uncertaintyType
        {
            get { return _uncertaintyType; }
            set { _uncertaintyType = value; }
        }
        
        private decimal _oneSigma;
        public decimal oneSigma
        {
            get { return _oneSigma; }
            set { _oneSigma = value; }
        }

        public ValueModel()
		{
			// 
			// TODO: Add constructor logic here
			//
		}

        public ValueModel(string name)
        {
            this.name = name;
            value = 0.000m;
            uncertaintyType = "PCT";
            oneSigma = 0.0m;
        }

        public ValueModel(
            string name, decimal value, decimal oneSigma)
		{
			this.name = name;
            this.value = value;
            this.uncertaintyType = "PCT";
            this.oneSigma = oneSigma;
        }


    }
}

