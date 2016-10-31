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
using System.IO;

namespace Tripoli.vendors_data
{
	/// <summary>
	/// Parent of MassLynxDataFile and Sector54DataFile and Thermo-FinniganTriton
	/// </summary>
	public abstract class MassSpecDataFile
	{
		// Fields
		FileInfo _DataFileInfo;

        public  enum myMonths : int
        {
            January = 1, February, March,
            April, May, June,
            July, August, September,
            October, November, December
        };

        public enum myMonthsShort : int
        {
            Jan = 1, Feb, Mar,
            Apr, May, Jun,
            Jul, Aug, Sep,
            Oct, Nov, Dec
        };
		/// <summary>
		/// 
		/// </summary>
        public MassSpecDataFile()
		{
			// 
			// TODO: Add constructor logic here
			//
		}

		#region Properties
		/// <summary>
		/// 
		/// </summary>
        public FileInfo DataFileInfo
		{
			get
			{
				return _DataFileInfo;
			}
			set
			{
				_DataFileInfo = value;
			}
		
		}

		#endregion Properties


		public abstract void close();
        public abstract string TestFileValidity();
        public abstract TripoliWorkProduct LoadRatios();
	}
}
