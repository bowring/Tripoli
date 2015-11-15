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
using Microsoft.Win32;

namespace Tripoli.source
{
    public static class TripoliRegistry
    {
        private static int MRU_LIST_COUNT = 5;

        // jan 2010 refactor by moving all here
        #region Registry Methods

        public static void SetRecentTripoliWorkFile(string FileName)
        {
            // TODO wrap in try catch and deal with missing
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
            RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
            RegistryKey mruFiles = Product.OpenSubKey("Recent TripoliWorkFile List", true);

            int markerIndex = MRU_LIST_COUNT;
            try
            {
                // first see if any of the existing mru are this filename
                for (int i = 1; i <= MRU_LIST_COUNT; i++)
                {
                    if (FileName == (string)mruFiles.GetValue("mruFile" + i.ToString()))
                    {
                        markerIndex = i;
                    }
                }
                // markerIndex now is MRU_LIST_COUNT for no coincidence or the mru slot that is equal
                // note that the last mru coincidence is the same as no coincidence => it drops off
                for (int i = markerIndex; i > 1; i--)
                {
                    if ((string)mruFiles.GetValue("mruFile" + ((int)(i - 1)).ToString()) != null)
                        mruFiles.SetValue("mruFile" + i.ToString(),
                            (string)mruFiles.GetValue("mruFile" + ((int)(i - 1)).ToString()));
                }

                mruFiles.SetValue("mruFile1", FileName);
            }
            catch { }
        }

        public static string GetRecentTripoliWorkFile(int no)
        {
            string retval = "";
            try
            {
                RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
                RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
                RegistryKey Product = Manuf.OpenSubKey("Tripoli");
                RegistryKey mruFiles = Product.OpenSubKey("Recent TripoliWorkFile List");

                retval = (string)mruFiles.GetValue("mruFile" + no.ToString());
            }
            catch { }
            return ((retval == "") || (retval == null)) ? Environment.CurrentDirectory : retval;
        }

        public static void SetRecentTripoliHistoryFile(string FileName)
        {
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
            RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
            RegistryKey mruFiles = Product.OpenSubKey("Recent TripoliHistoryFile List", true);

            int markerIndex = MRU_LIST_COUNT;
            try
            {
                // first see if any of the existing mru are this filename
                for (int i = 1; i <= MRU_LIST_COUNT; i++)
                {
                    if (FileName == (string)mruFiles.GetValue("mruFile" + i.ToString()))
                    {
                        markerIndex = i;
                    }
                }
                // markerIndex now is MRU_LIST_COUNT for no coincidence or the mru slot that is equal
                // note that the last mru coincidence is the same as no coincidence => it drops off
                for (int i = markerIndex; i > 1; i--)
                {
                    if ((string)mruFiles.GetValue("mruFile" + ((int)(i - 1)).ToString()) != null)
                        mruFiles.SetValue("mruFile" + i.ToString(),
                            (string)mruFiles.GetValue("mruFile" + ((int)(i - 1)).ToString()));
                }

                mruFiles.SetValue("mruFile1", FileName);
            }
            catch { }
        }

        public static string GetRecentTripoliHistoryFile(int no)
        {
            string retval = "";
            try
            {
                RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
                RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
                RegistryKey Product = Manuf.OpenSubKey("Tripoli");
                RegistryKey mruFiles = Product.OpenSubKey("Recent TripoliHistoryFile List");

                retval = (string)mruFiles.GetValue("mruFile" + no.ToString());
            }
            catch { }
            return ((retval == "") || (retval == null)) ? Environment.CurrentDirectory : retval;
        }






        public static void SetRecentIsoprobXExcelDataFile(string FileName)
        {
            try
            {
                RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
                RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
                RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
                RegistryKey mruFiles = Product.OpenSubKey("Recent MassLynxExcelDataFile List", true);

                mruFiles.SetValue("mruFile1", FileName);
            }
            catch { }
        }
        public static string GetRecentIsoprobXExcelDataFile()
        {
            string retval = "";
            try
            {
                RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
                RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
                RegistryKey Product = Manuf.OpenSubKey("Tripoli");
                RegistryKey mruFiles = Product.OpenSubKey("Recent MassLynxExcelDataFile List");

                retval = (string)mruFiles.GetValue("mruFile1");
            }
            catch { }
            return ((retval == "") || (retval == null)) ? Environment.CurrentDirectory : retval;
        }

        public static void SetRecentThermoFinniganTritonExpFile(string FileName)
        {
            try
            {
                RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
                RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
                RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
                RegistryKey mruFiles = Product.OpenSubKey("Recent ThermoFinniganTritonExpFile List", true);

                mruFiles.SetValue("mruFile1", FileName);
            }
            catch { }
        }
        public static string GetRecentThermoFinniganTritonExpFile()
        {
            // default
            string retval = "";
            try
            {
                RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
                RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
                RegistryKey Product = Manuf.OpenSubKey("Tripoli");
                RegistryKey mruFiles = Product.OpenSubKey("Recent ThermoFinniganTritonExpFile List");

                retval = (string)mruFiles.GetValue("mruFile1");
            }
            catch { }
            return ((retval == "") || (retval == null)) ? Environment.CurrentDirectory : retval;
        }

        public static void SetRecentTracersFolder(string Folder)
        {
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
            RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
            RegistryKey mruFolders = Product.OpenSubKey("Recent Tracers Folder", true);
            try
            {
                mruFolders.SetValue("mruFolder1", Folder);
            }
            catch { }
        }
        public static string GetRecentTracersFolder()
        {
            string retval = "";
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
            RegistryKey Product = Manuf.OpenSubKey("Tripoli");
            RegistryKey mruFolders = Product.OpenSubKey("Recent Tracers Folder");
            try
            {
                retval = (string)mruFolders.GetValue("mruFolder1");
            }
            catch { }
            return ((retval == "") || (retval == null)) ? Environment.CurrentDirectory : retval;
        }


        public static void SetRecentGainsFolder(string Folder)
        {
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
            RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
            RegistryKey mruFolders = Product.OpenSubKey("Recent Gains Folder", true);
            try
            {
                mruFolders.SetValue("mruFolder1", Folder);
            }
            catch { }
        }
        public static string GetRecentGainsFolder()
        {
            string retval = "";
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
            RegistryKey Product = Manuf.OpenSubKey("Tripoli");
            RegistryKey mruFolders = Product.OpenSubKey("Recent Gains Folder");
            try
            {
                retval = (string)mruFolders.GetValue("mruFolder1");
            }
            catch { }
            return ((retval == "") || (retval == null)) ? Environment.CurrentDirectory : retval;
        }

        public static void SetRecentStandardsFolder(string Folder)
        {
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
            RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
            RegistryKey mruFolders = Product.OpenSubKey("Recent Standards Folder", true);
            try
            {
                mruFolders.SetValue("mruFolder1", Folder);
            }
            catch { }
        }
        public static string GetRecentStandardsFolder()
        {
            string retval = "";
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
            RegistryKey Product = Manuf.OpenSubKey("Tripoli");
            RegistryKey mruFolders = Product.OpenSubKey("Recent Standards Folder");
            try
            {
                retval = (string)mruFolders.GetValue("mruFolder1");
            }
            catch { }
            return ((retval == "") || (retval == null)) ? Environment.CurrentDirectory : retval;
        }



        #endregion Registry Methods

        // oct 2009 for live update
        public static void SetRecentSector54LiveFolder(string Folder)
        {
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
            RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
            RegistryKey mruFolder = Product.OpenSubKey("Recent Sector54 LiveFolder", true);
            try
            {
                mruFolder.SetValue("mruFolder1", Folder);
            }
            catch { }
        }
        public static string GetRecentSector54LiveFolder()
        {
            string retval = "";
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
            RegistryKey Product = Manuf.OpenSubKey("Tripoli");
            RegistryKey mruFolder = Product.OpenSubKey("Recent Sector54 LiveFolder");
            try
            {
                retval = (string)mruFolder.GetValue("mruFolder1");
            }
            catch { }
            return ((retval == "") || (retval == null)) ? Environment.CurrentDirectory : retval;
        }

        // July 2011 for new LIveData facility from IonVantage software package
        // RecentIonVantageLiveDataFolder handles the management of LiveData Folders independently of LIve WorkFLow
        // for use in opening already-produced data
        public static void SetRecentIonVantageLiveDataFolder(string FolderName)
        {
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
            RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
            RegistryKey mruFolders = Product.OpenSubKey("Recent IonVantage LiveDataFolder List", true);

            try
            {
                mruFolders.SetValue("mruFolder1", FolderName);
            }
            catch { }
        }
        public static string GetRecentIonVantageLiveDataFolder()
        {
            string retval = "";
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
            RegistryKey Product = Manuf.OpenSubKey("Tripoli");
            RegistryKey mruFolders = Product.OpenSubKey("Recent IonVantage LiveDataFolder List");
            try
            {
                retval = (string)mruFolders.GetValue("mruFolder1");
            }
            catch { }
            return ((retval == "") || (retval == null)) ? Environment.CurrentDirectory : retval;
        }

        // handles IonVantage LiveDataStatus.txt containing directory
        public static void SetRecentIonVantageLiveDataStatusFileFolder(string FolderName)
        {
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
            RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
            RegistryKey mruFolders = Product.OpenSubKey("Recent IonVantage LiveDataStatusFileFolder List", true);

            try
            {
                mruFolders.SetValue("mruFolder1", FolderName);
            }
            catch { }
        }
        public static string GetRecentIonVantageLiveDataStatusFileFolder()
        {
            string retval = "";
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
            RegistryKey Product = Manuf.OpenSubKey("Tripoli");
            RegistryKey mruFolders = Product.OpenSubKey("Recent IonVantage LiveDataStatusFileFolder List");
            try
            {
                retval = (string)mruFolders.GetValue("mruFolder1");
            }
            catch { }
            return ((retval == "") || (retval == null)) ? Environment.CurrentDirectory : retval;
        }
        // end July 2011


        public static void SetRecentIsotopXLiveFolder(string Folder)
        {
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
            RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
            RegistryKey mruFolder = Product.OpenSubKey("Recent IsotopX LiveFolder", true);
            try
            {
                mruFolder.SetValue("mruFolder1", Folder);
            }
            catch { }
        }
        public static string GetRecentIsotopXLiveFolder()
        {
            string retval = "";
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
            RegistryKey Product = Manuf.OpenSubKey("Tripoli");
            RegistryKey mruFolder = Product.OpenSubKey("Recent IsotopX LiveFolder");
            try
            {
                retval = (string)mruFolder.GetValue("mruFolder1");
            }
            catch { }
            return ((retval == "") || (retval == null)) ? Environment.CurrentDirectory : retval;
        }

        public static void SetRecentThermoFinniganTritonLiveFolder(string Folder)
        {
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
            RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
            RegistryKey mruFolder = Product.OpenSubKey("Recent ThermoFinniganTriton LiveFolder", true);
            try
            {
                mruFolder.SetValue("mruFolder1", Folder);
            }
            catch { }
        }
        public static string GetRecentThermoFinniganTritonLiveFolder()
        {
            string retval = "";
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
            RegistryKey Product = Manuf.OpenSubKey("Tripoli");
            RegistryKey mruFolder = Product.OpenSubKey("Recent ThermoFinniganTriton LiveFolder");
            try
            {
                retval = (string)mruFolder.GetValue("mruFolder1");
            }
            catch { }
            return ((retval == "") || (retval == null)) ? Environment.CurrentDirectory : retval;
        }

        public static void SetRecentThermoFinniganMat261LiveFolder(string Folder)
        {
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
            RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
            RegistryKey mruFolder = Product.OpenSubKey("Recent ThermoFinniganMat261 LiveFolder", true);
            try
            {
                mruFolder.SetValue("mruFolder1", Folder);
            }
            catch { }
        }
        public static string GetRecentThermoFinniganMat261LiveFolder()
        {
            string retval = "";
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
            RegistryKey Product = Manuf.OpenSubKey("Tripoli");
            RegistryKey mruFolder = Product.OpenSubKey("Recent ThermoFinniganMat261 LiveFolder");
            try
            {
                retval = (string)mruFolder.GetValue("mruFolder1");
            }
            catch { }
            return ((retval == "") || (retval == null)) ? Environment.CurrentDirectory : retval;
        }


        public static void SetRecentSector54DatFile(string FileName)
        {
            try
            {
                RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
                RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
                RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
                RegistryKey mruFiles = Product.OpenSubKey("Recent Sector54DatFile List", true);

                mruFiles.SetValue("mruFile1", FileName);
            }
            catch { }
        }
        public static string GetRecentSector54DatFile()
        {
            // default
            string retval = "";

            try
            {
                RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
                RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
                RegistryKey Product = Manuf.OpenSubKey("Tripoli");
                RegistryKey mruFiles = Product.OpenSubKey("Recent Sector54DatFile List");

                retval = (string)mruFiles.GetValue("mruFile1");
            }
            catch { }
            return ((retval == "") || (retval == null)) ? Environment.CurrentDirectory : retval;
        }


        public static void SetRecentThermoFinniganMat261TxtFile(string FileName)
        {
            try
            {
                RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
                RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
                RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
                RegistryKey mruFiles = Product.OpenSubKey("Recent ThermoFinniganMat261TxtFile List", true);

                mruFiles.SetValue("mruFile1", FileName);
            }
            catch { }
        }
        public static string GetRecentThermoFinniganMat261TxtFile()
        {
            // default
            string retval = "";

            try
            {
                RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
                RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
                RegistryKey Product = Manuf.OpenSubKey("Tripoli");
                RegistryKey mruFiles = Product.OpenSubKey("Recent ThermoFinniganMat261TxtFile List");

                retval = (string)mruFiles.GetValue("mruFile1");
            }
            catch { }
            return ((retval == "") || (retval == null)) ? Environment.CurrentDirectory : retval;
        }



        public static void SetRecentIDTIMS_ImportCSVFile(string FileName)
        {
            try
            {
                RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
                RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
                RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
                RegistryKey mruFiles = Product.OpenSubKey("Recent IDTIMS_ImportCSVFile List", true);

                mruFiles.SetValue("mruFile1", FileName);
            }
            catch { }
        }
        public static string GetRecentIDTIMS_ImportCSVFile()
        {
            // default
            string retval = "";

            try
            {
                RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
                RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
                RegistryKey Product = Manuf.OpenSubKey("Tripoli");
                RegistryKey mruFiles = Product.OpenSubKey("Recent IDTIMS_ImportCSVFile List");

                retval = (string)mruFiles.GetValue("mruFile1");
            }
            catch { }
            return ((retval == "") || (retval == null)) ? Environment.CurrentDirectory : retval;
        }



        public static void SetRecentLiveWorkflowFileAccessTime(long fileAccessTime)
        {
            try
            {
                RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
                RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
                RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
                RegistryKey mruTime = Product.OpenSubKey("Recent LiveWorkflow FileAccessTime", true);

                mruTime.SetValue("mruTime", fileAccessTime, RegistryValueKind.QWord);
            }
            catch { }
        }
        public static long GetRecentLiveWorkflowFileAccessTime()
        {
            // default
            long retval = 0L;

            try
            {
                RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
                RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
                RegistryKey Product = Manuf.OpenSubKey("Tripoli");
                RegistryKey mruFiles = Product.OpenSubKey("Recent LiveWorkflow FileAccessTime");

                retval = (long)   Convert.ToDouble( mruFiles.GetValue("mruTime"));
            }
            catch { }
            return retval;
        }

        public static void SetRecentUPbReduxExportFolder(string Folder)
        {
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
            RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
            RegistryKey mruFolder = Product.OpenSubKey("Recent UPbRedux ExportFolder", true);
            try
            {
                mruFolder.SetValue("mruFolder1", Folder);
            }
            catch { }
        }
        public static string GetRecentUPbReduxExportFolder()
        {
            string retval = "";
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
            RegistryKey Product = Manuf.OpenSubKey("Tripoli");
            RegistryKey mruFolder = Product.OpenSubKey("Recent UPbRedux ExportFolder");
            try
            {
                retval = (string)mruFolder.GetValue("mruFolder1");
            }
            catch { }
            return ((retval == "") || (retval == null)) ? Environment.CurrentDirectory : retval;
        }

        public static void SetCurrentDataTypeChoice(string myDataTypeChoice)
        {
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
            RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
            RegistryKey dataTypeChoice = Product.OpenSubKey("Current DataType Choice", true);
            try
            {
                dataTypeChoice.SetValue("dataTypeChoice", myDataTypeChoice);
            }
            catch { }
        }
        public static string GetCurrentDataTypeChoice()
        {
            string retval = ".dat";
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
            RegistryKey Product = Manuf.OpenSubKey("Tripoli");
            RegistryKey dataTypeChoice = Product.OpenSubKey("Current DataType Choice");
            try
            {
                retval = (string)dataTypeChoice.GetValue("dataTypeChoice");
                if (!retval.StartsWith("."))
                    retval = ".dat";
            }
            catch { }
            return retval;
        }

        // mar 2010 made Sample Meta Data free of location constraints
        public static void SetSampleMetaDataFolder(string Folder)
        {
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
            RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
            RegistryKey mruFolder = Product.OpenSubKey("SampleMetaData Folder", true);
            try
            {
                mruFolder.SetValue("mruFolder1", Folder);
            }
            catch { }
        }
        public static string GetSampleMetaDataFolder()
        {
            string retval = "";
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
            RegistryKey Product = Manuf.OpenSubKey("Tripoli");
            RegistryKey mruFolder = Product.OpenSubKey("SampleMetaData Folder");
            try
            {
                retval = (string)mruFolder.GetValue("mruFolder1");
            }
            catch { }
            return ((retval == "") || (retval == null) || (!retval.ToUpper().Contains("SAMPLEMETADATA"))) ? null : retval;
        }

        public static void SetIgnoreSampleMetaData(string truth)
        {
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
            RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
            RegistryKey myTruth = Product.OpenSubKey("IgnoreSampleMetaData", true);
            try
            {
                myTruth.SetValue("myTruth", truth);
            }
            catch { }
        }
        public static string GetIgnoreSampleMetaData()
        {
            string retval = "";
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
            RegistryKey Product = Manuf.OpenSubKey("Tripoli");
            RegistryKey myTruth = Product.OpenSubKey("IgnoreSampleMetaData");
            try
            {
                retval = (string)myTruth.GetValue("myTruth");
            }
            catch { }
            return (!(retval.ToUpper().Trim().Contains("TRUE"))) ? "FALSE" : "TRUE";
        }

        //  2010 LA-ICP MS Section *************************************************************************************************

        public static void SetRecentNuPlasmaLiveFolder(string Folder)
        {
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
            RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
            RegistryKey mruFolder = Product.OpenSubKey("Recent NuPlasma LiveFolder", true);
            try
            {
                mruFolder.SetValue("mruFolder1", Folder);
            }
            catch { }
        }
        public static string GetRecentNuPlasmaLiveFolder()
        {
            string retval = "";
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
            RegistryKey Product = Manuf.OpenSubKey("Tripoli");
            RegistryKey mruFolder = Product.OpenSubKey("Recent NuPlasma LiveFolder");
            try
            {
                retval = (string)mruFolder.GetValue("mruFolder1");
            }
            catch { }
            return ((retval == "") || (retval == null)) ? Environment.CurrentDirectory : retval;
        }

        public static void SetRecentNuPlasmaTxtFile(string FileName)
        {
            try
            {
                RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
                RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
                RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
                RegistryKey mruFiles = Product.OpenSubKey("Recent NuPlasmaTxtFile List", true);

                mruFiles.SetValue("mruFile1", FileName);
            }
            catch { }
        }
        public static string GetRecentNuPlasmaTxtFile()
        {
            // default
            string retval = "";

            try
            {
                RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
                RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
                RegistryKey Product = Manuf.OpenSubKey("Tripoli");
                RegistryKey mruFiles = Product.OpenSubKey("Recent NuPlasmaTxtFile List");

                retval = (string)mruFiles.GetValue("mruFile1");
            }
            catch { }
            return ((retval == "") || (retval == null)) ? Environment.CurrentDirectory : retval;
        }

        public static void SetRecentElement2DataFolder(string Folder)
        {
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
            RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
            RegistryKey mruFolders = Product.OpenSubKey("Recent Element2Data Folder", true);
            try
            {
                mruFolders.SetValue("mruFolder1", Folder);
            }
            catch { }
        }
        public static string GetRecentElement2DataFolder()
        {
            string retval = "";
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
            RegistryKey Product = Manuf.OpenSubKey("Tripoli");
            RegistryKey mruFolders = Product.OpenSubKey("Recent Element2Data Folder");
            try
            {
                retval = (string)mruFolders.GetValue("mruFolder1");
            }
            catch { }
            return ((retval == "") || (retval == null)) ? Environment.CurrentDirectory : retval;
        }
    }
}
