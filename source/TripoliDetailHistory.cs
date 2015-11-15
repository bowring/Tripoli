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
	/// Stores name or ratio or collector, component rawRatios, current history RawRatio
	/// </summary>
	[Serializable]
	public class TripoliDetailHistory : ISerializable
	{
		// Fields
		private string _Name;
		private ArrayList _MemberRatios;
		private DetailHistory _DetailHistory;

		/// <summary>
		/// 
		/// </summary>
		public TripoliDetailHistory(string myName)
		{
			_Name = myName;
			_MemberRatios = new ArrayList();
			_DetailHistory = new DetailHistory( "History: " + myName);
		}

		#region Properties
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				_Name = value;
			}
		}
		public ArrayList MemberRatios
		{
			get
			{
				return _MemberRatios;
			}
			set
			{
				_MemberRatios = value;
			}
		}
		public DetailHistory DetailHistory
		{
			get
			{
				return _DetailHistory;
			}
			set
			{
				_DetailHistory = value;
			}
		}
		#endregion Properties

		#region Serialization and deserialization
		/// <summary>
		/// 
		/// </summary>
		/// <param name="si"></param>
		/// <param name="context"></param>
		protected TripoliDetailHistory(SerializationInfo si, StreamingContext context)
		{
			_Name = ( string )si.GetValue("Name",  typeof( string ));
			_MemberRatios = ( ArrayList )si.GetValue("MemberRatios", typeof( ArrayList ));
			_DetailHistory = ( DetailHistory )si.GetValue("DetailHistory",  typeof( DetailHistory ));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="si"></param>
		/// <param name="context"></param>
		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
		public virtual void GetObjectData(SerializationInfo si, StreamingContext context)
		{
			si.AddValue("Name", _Name, typeof( string ));
			si.AddValue("MemberRatios", _MemberRatios, typeof( ArrayList ));
			si.AddValue("DetailHistory", _DetailHistory, typeof( RawRatio ));
		}
		#endregion Serialization and deserialization

		#region Methods

		public void setMaxRatioCount()
		{
			DetailHistory.CyclesPerBlock = 0;

			for (int fileNo = 0; fileNo < MemberRatios.Count; fileNo ++)
			{
				DetailHistory.CyclesPerBlock = 
					Math.Max(((RawRatio)MemberRatios[fileNo]).CyclesPerBlock, DetailHistory.CyclesPerBlock);
			}

			// add space for mean to live
			DetailHistory.CyclesPerBlock ++;
		}


		#endregion Methods

	}
}
