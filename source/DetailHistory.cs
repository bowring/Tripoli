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
using System.Security; 
using System.Security.Permissions;
using System.Runtime.Serialization;

namespace Tripoli
{
	/// <summary>
	/// Specializes RawRatio to allow for histories of rawRatios
	/// </summary>
	[Serializable]
	public class DetailHistory : Tripoli.RawRatio
	{
		// Fields
		ArrayList _GainsFilesInfo = new ArrayList();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Name"></param>
		public DetailHistory(string Name):base(Name)
		{
			// 
			// TODO: Add constructor logic here
			//
		}

		public DetailHistory(string Name, double[] Ratios) :base(Name, Ratios)
		{
		}

		#region Serialization and deserialization
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="si"></param>
		/// <param name="context"></param>
		protected DetailHistory(SerializationInfo si, StreamingContext context): base(si,context)
		{
			//_FolderInfo = ( DirectoryInfo )si.GetValue("FolderInfo",  typeof( DirectoryInfo ));
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
			//si.AddValue("FolderInfo", _FolderInfo, typeof( DirectoryInfo ));
		}

		#endregion Serialization and deserialization

		#region Properties

		public ArrayList GainsFilesInfo
		{
			get
			{
				return _GainsFilesInfo;
			}
			set
			{
				_GainsFilesInfo = new ArrayList();
				for (int i = 0; i < value.Count; i ++)
				{
					ArrayList temp = new ArrayList();
					temp.Add(((ArrayList)value[i])[0]);
					temp.Add(((ArrayList)value[i])[1]);
					temp.Add(((ArrayList)value[i])[2]);

					_GainsFilesInfo.Add(temp);	
				}
			}
		}
		
		#endregion Properties

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="masterDetail"></param>
		public void SynchDetailHistory(DetailHistory masterDetail)
		{
			//base.SynchRatios(masterRatio);

			// dec 2005 moved to here instead of using base and use masterRatios[0] as flag
			// set the same items negative in this ratio set
			base.RestoreRatios();
			//dec 2005 check if masterDetail is active
			if (masterDetail.Ratios.Length > 0)
			{
				for (int ratio = 0; ratio < Ratios.Length; ratio ++)
					if (masterDetail.Ratios[0] < 0)
						this.Ratios[ratio] *= -1.0;
			}
			base.SynchRatioHistory(masterDetail);
		
			GainsFilesInfo = masterDetail.GainsFilesInfo;
		}

		#endregion Methods

	}


}
