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

namespace Tripoli.earth_time_org
{
    class DataDictionary
    {
        // Static
        internal static string[] EarthTimeBariumPhosphateICIsotopeNames = new String[]{
                                                            "pct130Ba_BaPO2",
                                                            "pct132Ba_BaPO2",
                                                            "pct134Ba_BaPO2",
                                                            "pct135Ba_BaPO2",
                                                            "pct136Ba_BaPO2",
                                                            "pct137Ba_BaPO2",
                                                            "pct138Ba_BaPO2",
                                                            "pct16O_BaPO2",
                                                            "pct17O_BaPO2",
                                                            "pct18O_BaPO2"
    };

        internal static string[] EarthTimeThalliumICIsotopeNames = new String[]{
                                                            "pct203Tl_Tl",
                                                            "pct205Tl_Tl"
    };

        internal static string[] EarthTimeTracerRatioNames = new string[] {   
                                                          "206_204",
                                                          "207_206",
                                                          "206_208",
                                                          "206_205",
                                                          "207_205",
                                                          "208_205",
                                                          "202_205",
                                                          "238_235",
                                                          "233_235",
                                                          "233_236",
                                                          "235_205"};

        internal static string[] UPbReduxMeasuredRatioNames = new string[] {   
                                                          "206_204",
                                                          "207_204",
                                                          "208_204",
                                                          "206_207",
                                                          "206_208",
                                                          "204_205",
                                                          "206_205",
                                                          "207_205",
                                                          "208_205",
                                                          "202_205",
                                                          "238_236",     //jan 2011
                                                          "233_236",
                                                          "238_235",
                                                          "233_235",
                                                          "238_233"};

        internal static string[] isotopeNames = new string[]
                                                  {     "Pb205",
                                                        "U235" };

        internal static string[] TracerTypes = new string[]
                                                  {     "mixed 205-235",
                                                        "mixed 205-233-235",
                                                        "mixed 208-235",
                                                        "mixed 205-233-236",
                                                        "mixed 202-205-233-235",
                                                        "mixed 202-205-233-236",
                                                        "mixed 205-233-235-230Th" };


        internal static string getEarthTimeBariumPhosphateICIsotopeNames(int index)
        {
            return EarthTimeBariumPhosphateICIsotopeNames[index];
        }

        internal static string getEarthTimeThalliumICIsotopeNames(int index)
        {
            return EarthTimeThalliumICIsotopeNames[index];
        }  
      
        internal static string getTracerRatioName(int index)
        {
            return "r" + EarthTimeTracerRatioNames[index] + "t";
        }

        internal static string getTracerIsotopeConcName(int index)
        {
            return "conc" + isotopeNames[index] + "t";
        }

        internal static string getMeasuredRatioName(int index)
        {
            return "r" + UPbReduxMeasuredRatioNames[index] + "m";
        }

        internal static string getElementNameOfRatio(string name)
        {
            if (name.Substring(1, 1).Equals("0"))
                return "Pb";
            else
                return "U";
        }

    }
}
