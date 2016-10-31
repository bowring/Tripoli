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

namespace Tripoli.earth_time_org
{
	/// <summary>
	/// 
	/// </summary>
	public class MeasuredRatioModel : ValueModel
	{
        // NOTE: underscored names are used so that lower-case properties can be used 
        // for xml serialization

        private bool _fracCorr; // fractionation corrected by Tripoli
        public bool fracCorr
        {
            get { return _fracCorr; }
            set { _fracCorr = value; }
        }

        private bool _oxideCorr; // oxide corrected by Tripoli
        public bool oxideCorr
        {
            get { return _oxideCorr; }
            set { _oxideCorr = value; }
        }
        
        public MeasuredRatioModel()
		{
			// 
			// TODO: Add constructor logic here
			//
		}
		public MeasuredRatioModel(
            string name, decimal value, decimal oneSigma, bool fracCorr, bool oxideCorr)
		{
			this.name = name;
            this.value = value;
            this.uncertaintyType = "PCT";
            this.oneSigma = oneSigma;
			this.fracCorr = fracCorr;
            this.oxideCorr = oxideCorr;
        }


    }
}

