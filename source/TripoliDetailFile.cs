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
using System.Collections;
using System.Security; 
using System.Security.Permissions;
using System.Runtime.Serialization;
using System.Data;
using System.Data.OleDb;
using System.Reflection;

namespace Tripoli
{
	/// <summary>
	/// Parent of GainsFile and StandardsFile
	/// </summary>
	[Serializable]
	public class TripoliDetailFile : ISerializable
	{

		// Fields
		private string _FileName = "";
		private DateTime _InitDate = new DateTime(1);
		private bool _IsActive;
		private bool _IsActiveSaved;

		private ArrayList _Collectors;


		public TripoliDetailFile()
		{
			// 
			// TODO: Add constructor logic here
			//
		}

		public TripoliDetailFile(string FileName)
		{
			_FileName = FileName;
			_IsActive = false;
			_IsActiveSaved = false;
			_Collectors = new ArrayList();
		}

		#region Serialization and deserialization
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="si"></param>
		/// <param name="context"></param>
		protected TripoliDetailFile(SerializationInfo si, StreamingContext context)
		{
			_FileName = ( string )si.GetValue("FileName",  typeof( string ));
			_InitDate = ( DateTime )si.GetValue("InitDate",  typeof( DateTime ));
			_IsActive = ( bool )si.GetValue("IsActive",  typeof( bool ));
			_IsActiveSaved = ( bool )si.GetValue("IsActiveSaved",  typeof( bool ));
			_Collectors = ( ArrayList )si.GetValue("Collectors", typeof( ArrayList ));

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="si"></param>
		/// <param name="context"></param>
		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
		public virtual void GetObjectData(SerializationInfo si, StreamingContext context)
		{
			si.AddValue("FileName", _FileName, typeof( string ));
			si.AddValue("InitDate", _InitDate, typeof( DateTime ));
			si.AddValue("IsActive", _IsActive, typeof( bool ));
			si.AddValue("IsActiveSaved", _IsActiveSaved, typeof( bool ));
			si.AddValue("Collectors", _Collectors, typeof( ArrayList ));
		}

		#endregion Serialization and deserialization

		#region Properties

		public string FileName
		{
			get
			{
				return _FileName;
			}
			set
			{
				_FileName = value;
			}
		}
		public DateTime InitDate
		{
			get
			{
				return _InitDate;
			}
			set
			{
				_InitDate = value;
			}
		}
		public bool IsActive
		{
			get
			{
				return _IsActive;
			}
			set
			{
				_IsActive = value;
			}
		}
		public bool IsActiveSaved
		{
			get
			{
				return _IsActiveSaved;
			}
			set
			{
				_IsActiveSaved = value;
			}
		}
		public ArrayList Collectors
		{
			get
			{
				return _Collectors;
			}
			set
			{
				_Collectors = value;
			}
		}

		#endregion Properties

		#region IComparers
		/// <summary>
		/// 
		/// </summary>
		public class NameOrderClass : IComparer  
		{
			int IComparer.Compare( object x, object y )  
			{
				return( (new CaseInsensitiveComparer())
					.Compare( ((TripoliDetailFile)y).FileName , ((TripoliDetailFile)x).FileName ) );
			}

		}
		/// <summary>
		/// Ascending date compare and use file name if dates are equal
		/// </summary>
		public class DateOrderClass : IComparer  
		{
			int IComparer.Compare( object x, object y )  
			{
				if (( (new CaseInsensitiveComparer())
					.Compare( ((TripoliDetailFile)x).InitDate , ((TripoliDetailFile)y).InitDate ) ) == 0)
				{
					return( (new CaseInsensitiveComparer())
						.Compare( ((TripoliDetailFile)y).FileName , ((TripoliDetailFile)x).FileName ) );
				}
				else
				{
					return( (new CaseInsensitiveComparer())
						.Compare( ((TripoliDetailFile)x).InitDate , ((TripoliDetailFile)y).InitDate ) );
				}
			}

		}

		#endregion IComparers


	}


}
