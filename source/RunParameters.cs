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

namespace Tripoli
{
	/// <summary>
	/// Contains the MassLynx Run Parameters from the Summary page
	/// </summary>
	public class RunParameters
	{
		// Fields
        //int _SampleNo;
        //string _SampleID;
        //string _CentreCh;
        //string _MethodName;
        //string _SampleType;
        //string _FileText;
        //bool _ApplyBaselines;
        //double _SigmaLevel;
        //string _UserName;
        //string _BlankSubtract;
        //double _RejectPct;
        //int _SerialNo;
        //DateTime _PrintDate;
        //string _PeriodicDB;
        //string _SummaryWB;
		DateTime _AcquireDate = new DateTime(1);
		string _ExcelFileName;
		
		public RunParameters()
		{
			// 
			// TODO: Add constructor logic here
			//
		}

		public string ExcelFileName
		{
			get
			{
				return _ExcelFileName;
			}
			set
			{
				_ExcelFileName = value;
			}
		}
		public DateTime AquireDate
		{
			get
			{
				return _AcquireDate;
			}
			set
			{
				_AcquireDate = value;
			}
		}

	}
}
