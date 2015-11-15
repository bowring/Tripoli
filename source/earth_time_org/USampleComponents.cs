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
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;



[assembly: RegistryPermissionAttribute(SecurityAction.RequestMinimum,
ViewAndModify = "HKEY_CURRENT_USER")]


namespace Tripoli.earth_time_org
{
    [Serializable]
    public class USampleComponents
    {
        private decimal _labUBlankMass;

        public decimal labUBlankMass
        {
            get { return _labUBlankMass; }
            set { _labUBlankMass = value; }
        }
        private decimal _r238_235b;

        public decimal r238_235b
        {
            get { return _r238_235b; }
            set { _r238_235b = value; }
        }
        private decimal _r238_235s;

        public decimal r238_235s
        {
            get { return _r238_235s; }
            set { _r238_235s = value; }
        }
        private decimal _tracerMass;

        public decimal tracerMass
        {
            get { return _tracerMass; }
            set { _tracerMass = value; }
        }
        private decimal gmol235;

        public decimal Gmol235
        {
            get { return gmol235; }
            set { gmol235 = value; }
        }
        private decimal gmol238;

        public decimal Gmol238
        {
            get { return gmol238; }
            set { gmol238 = value; }
        }

        private decimal defaultLabUBlankMass;

        public decimal DefaultLabUBlankMass
        {
            get { return defaultLabUBlankMass; }
            set { defaultLabUBlankMass = value; }
        }
        private decimal defaultR238_235b;

        public decimal DefaultR238_235b
        {
            get { return defaultR238_235b; }
            set { defaultR238_235b = value; }
        }
        private decimal defaultR238_235s;

        public decimal DefaultR238_235s
        {
            get { return defaultR238_235s; }
            set { defaultR238_235s = value; }
        }
        private decimal defaultTracerMass;

        public decimal DefaultTracerMass
        {
            get { return defaultTracerMass; }
            set { defaultTracerMass = value; }
        }

        private decimal defaultGmol235;

        public decimal DefaultGmol235
        {
            get { return defaultGmol235; }
            set { defaultGmol235 = value; }
        }
        private decimal defaultGmol238;

        public decimal DefaultGmol238
        {
            get { return defaultGmol238; }
            set { defaultGmol238 = value; }
        }

        private decimal defaultR18O_16O;

        public decimal DefaultR18O_16O
        {
            get { return defaultR18O_16O; }
            set { defaultR18O_16O = value; }
        }

        public USampleComponents()
        {
            GetDefaultUsampleComponents();
        }

        public void SetDefaultUsampleComponents()
        {
            // TODO wrap in try catch and deal with missing
            RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
            RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems", true);
            RegistryKey Product = Manuf.OpenSubKey("Tripoli", true);
            RegistryKey entries = Product.OpenSubKey("UsampleComponents", true);

            try
            {
                entries.SetValue("defaultLabUBlankMass", Convert.ToString(DefaultLabUBlankMass));
                entries.SetValue("defaultR238_235b", Convert.ToString(DefaultR238_235b));
                entries.SetValue("defaultR238_235s", Convert.ToString(DefaultR238_235s));
                entries.SetValue("defaultTracerMass", Convert.ToString(DefaultTracerMass));
                entries.SetValue("defaultGmol235", Convert.ToString(DefaultGmol235));
                entries.SetValue("defaultGmol238", Convert.ToString(DefaultGmol238));
                entries.SetValue("defaultR18O_16O", Convert.ToString(DefaultR18O_16O));
            }
            catch { }
        }

        public void GetDefaultUsampleComponents()
        {
            try
            {
                RegistryKey Software = Registry.CurrentUser.OpenSubKey("Software");
                RegistryKey Manuf = Software.OpenSubKey("Sunchex Systems");
                RegistryKey Product = Manuf.OpenSubKey("Tripoli");
                RegistryKey entries = Product.OpenSubKey("UsampleComponents");

                DefaultLabUBlankMass = Convert.ToDecimal(entries.GetValue("defaultLabUBlankMass"));
                if (DefaultLabUBlankMass == 0.0m) DefaultLabUBlankMass = 0.10m;//picograms
                
                DefaultR238_235b = Convert.ToDecimal(entries.GetValue("defaultR238_235b"));
                if (DefaultR238_235b == 0.0m) DefaultR238_235b = 137.88m;

                DefaultR238_235s = Convert.ToDecimal(entries.GetValue("defaultR238_235s"));
                if (DefaultR238_235s == 0.0m) DefaultR238_235s = 137.88m;
                
                DefaultTracerMass = Convert.ToDecimal(entries.GetValue("defaultTracerMass"));
                if (DefaultTracerMass == 0.0m) DefaultTracerMass = 0.0100m;

                DefaultGmol235 = Convert.ToDecimal(entries.GetValue("defaultGmol235"));
                if (DefaultGmol235 == 0.0m) DefaultGmol235 = 235.043922m;

                DefaultGmol238 = Convert.ToDecimal(entries.GetValue("defaultGmol238"));
                if (DefaultGmol238 == 0.0m) DefaultGmol238 = 238.050785m;

                DefaultR18O_16O = Convert.ToDecimal(entries.GetValue("defaultR18O_16O"));
                if (DefaultR18O_16O == 0.0m) DefaultR18O_16O = 0.0020m;
            }
            catch { }
        }

    }



}
