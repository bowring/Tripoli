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
using System.IO;
using System.Collections;

using System.Security; 
using System.Security.Permissions;
using System.Runtime.Serialization;

using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;



namespace Tripoli
{
	/// <summary>
	/// Handles GAINS history files 
	/// inherits from TripoliWorkProduct which inherits from ArrayList
	/// </summary>
	[Serializable]
	public class TripoliHistoryFolderGains : Tripoli.TripoliWorkProduct
	{
		private DirectoryInfo _FolderInfo = null;
		private SortedList _AllRatioHistories = null;

		// Settings
		private int _RejectLevelSigma = 10; //default setting
		
		/// <summary>
		/// 
		/// </summary>
		public TripoliHistoryFolderGains(): base()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="FolderName"></param>
		/// <param name="Pattern"></param>
		public TripoliHistoryFolderGains(string FolderName, string Pattern)
			: base()
		{
			_FolderInfo = new DirectoryInfo(FolderName);
			if (! _FolderInfo.Exists)
				_FolderInfo = null;
			else
				RefreshFolder(Pattern);

			AllRatioHistories = new SortedList();
		}

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public override DirectoryInfo FolderInfo
		{
			get
			{
				return _FolderInfo;
			}
			set
			{
				_FolderInfo = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override SortedList AllRatioHistories
		{
			get
			{
				return _AllRatioHistories;
			}
			set
			{
				// need deep copy
				_AllRatioHistories = value;
			}
		}
		/// <summary>
		/// Used to reject gains when outside this many sigmas
		/// </summary>
		public override int RejectLevelSigma
		{
			get
			{
				return _RejectLevelSigma;
			}
			set
			{
				_RejectLevelSigma = value;
			}
		}

		#endregion Properties

		#region Serialization and deserialization
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="si"></param>
		/// <param name="context"></param>
		protected TripoliHistoryFolderGains(SerializationInfo si, StreamingContext context): base(si,context)
		{
			_FolderInfo = ( DirectoryInfo )si.GetValue("FolderInfo",  typeof( DirectoryInfo ));
			_AllRatioHistories = ( SortedList )si.GetValue("AllRatioHistories", typeof( SortedList ));
			_RejectLevelSigma = ( int )si.GetValue("RejectLevelSigma", typeof( int ));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="si"></param>
		/// <param name="context"></param>
		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
		public override void GetObjectData(SerializationInfo si, StreamingContext context)
		{
			base.GetObjectData(si,context);
			si.AddValue("FolderInfo", _FolderInfo, typeof( DirectoryInfo ));
			si.AddValue("AllRatioHistories", _AllRatioHistories, typeof( SortedList ));
			si.AddValue("RejectLevelSigma", _RejectLevelSigma, typeof( int ));
		}

		#endregion Serialization and deserialization

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="Pattern"></param>
		public void RefreshFolder(string Pattern)
		{
			FileInfo [] tempFileInfo = FolderInfo.GetFiles(Pattern);

			// sort our ArrayList
			this.Sort(new TripoliDetailFile.NameOrderClass());

			// iterate this list, seeing if it is in our ArrayList and add with data if not
			for (int item = 0; item < tempFileInfo.Length; item ++)
			{
				TripoliDetailFile tempFile = new GainsFile(tempFileInfo[item].Name);
				if (this.BinarySearch(tempFile, new TripoliDetailFile.NameOrderClass()) < 0)
				{
					// not found, so let's build it and get excel data etc
					tempFile.IsActive = false;

					// get data from excel
					((GainsFile)tempFile).OpenCCGainsExcelFile(FolderInfo);

					// add it to ArrayList
					if (((GainsFile)tempFile).TestFileValidity()) 
					{
						//tempFile.InitDate = System.DateTime.Now;
						((GainsFile)tempFile).LoadGains();
						this.Add(tempFile);
						// sort our ArrayList
						this.Sort(new TripoliDetailFile.NameOrderClass());
					}
					((GainsFile)tempFile).close();
				}
			}
		}



		public override void setFileIsActive(int index, bool isActive)
		{
			for (int collector = 0; collector < AllRatioHistories.Count; collector ++)
			{
				((RawRatio)((TripoliDetailHistory) AllRatioHistories.GetByIndex(collector))
					.MemberRatios[index]).IsActive = isActive;
			}
		}

		#endregion Methods

	}
}
