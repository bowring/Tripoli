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
using System.Data;
using System.Data.OleDb;
using System.Reflection;

using System.Runtime.Serialization.Formatters.Binary;

using System.Windows.Forms;


namespace Tripoli
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class StandardsFile : Tripoli.TripoliDetailFile
	{
		// non-serialized Fields
		TripoliWorkProduct RawRatios = null;

		public StandardsFile(string FileName)
			:base(FileName)
		{}

		#region Serialization and deserialization
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="si"></param>
		/// <param name="context"></param>
		protected StandardsFile(SerializationInfo si, StreamingContext context)
			: base(si, context)
		{}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="si"></param>
		/// <param name="context"></param>
		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
		public override void GetObjectData(SerializationInfo si, StreamingContext context)
		{
			base.GetObjectData(si,context);
		}

		#endregion Serialization and deserialization

		#region Methods

		/// <summary>
		/// Opens a tripolized (.trip) file
		/// Should be refactored as this code also appears inside opentripoliworkproduct of frmmain
		/// </summary>
		/// <param name="FileInfo"></param>
		public FileInfo OpenStandardsDetailFile(DirectoryInfo FileInfo)
		{
			FileInfo TripoliFileInfo = new FileInfo(FileInfo.FullName + @"\" +  FileName);

			// recover the serialized form 
			Stream stream = null;
			try
			{
				stream  = File.Open(TripoliFileInfo.FullName, FileMode.Open);
				BinaryFormatter bformatter = new BinaryFormatter();
        
				RawRatios = null;
				RawRatios = (TripoliWorkProduct)bformatter.Deserialize(stream);
				// this traps for accidentally opening tripoli history file
				if (RawRatios.GetType().Name != "TripoliWorkProduct")
				{
					RawRatios = null;
					throw(new Exception());
				}
					
			}
			catch (Exception eee)
			{
				Console.WriteLine(eee.Message);
				MessageBox.Show("Failed to open Tripoli WorkFile !", "Tripoli Warning");
			}
			try
			{
				stream.Close();
			}
			catch{}	
	
			return RawRatios.SourceFileInfo;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool TestFileValidity()
		{
			return (RawRatios.GetType().Name == "TripoliWorkProduct");
		}

		/// <summary>
		/// 
		/// </summary>
		public void LoadGains()
		{
			// get initial date
			// this is temporarily done by historyfolder on a recall of excel file
			//InitDate = rawRatios.SourceFileInfo.CreationTime;
			

			// loop through Rawratios and build new summary Rawratio
			// that contains one entry per block (f.e. the mean for the block) 
			for (int rr = 0; rr < RawRatios.Count; rr ++)
			{
				RawRatio meansRR = new RawRatio(((RawRatio)RawRatios[rr]).Name + " (means)");
				meansRR.CyclesPerBlock = 
					(int)Math.Ceiling(((RawRatio)RawRatios[rr]).AllCount 
										/ ((RawRatio)RawRatios[rr]).CyclesPerBlock);
				double [] means = new double[meansRR.CyclesPerBlock];
				
				for (int block = 0; block < meansRR.CyclesPerBlock;	block ++)
				{
					double [] cycles = new double[((RawRatio)RawRatios[rr]).CyclesPerBlock];
					for (int count = 0; count < ((RawRatio)RawRatios[rr]).CyclesPerBlock; count ++)
					{
						cycles[count] = 
							((RawRatio)RawRatios[rr])
								.Ratios[block * ((RawRatio)RawRatios[rr]).CyclesPerBlock + count];
					}

					RawRatio tempRR = new RawRatio("tempWorkProduct", cycles);
					tempRR.CalcStats();
					means[block] = tempRR.Mean;			
				}
				
				meansRR.Ratios = means;
				Collectors.Add(meansRR);
			}

		}

		private void DisposeDataFileInfo()
		{

		}

		/// <summary>
		/// 
		/// </summary>
		public void close()
		{

		}

		#endregion Methods



	}
}
