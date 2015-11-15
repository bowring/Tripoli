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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Tripoli
{
    public partial class frmSelectETOTracer : Form
    {
        // Fields
        private string[] myList;
        private int _selectedIndex;
        private bool _cancel = true;

        public frmSelectETOTracer(string[] entries)
        {
            myList = entries;

            InitializeComponent();
        }

        private void frmSelectETOTracer_Load(object sender, EventArgs e)
        {
            foreach (string s in myList)
            {
                listFiles.Items.Add(s);
            }

            listFiles.SelectedIndex = 0;
        }

        public int selectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; }
        }
        public bool cancel 
        {
            get { return _cancel; }
            set { _cancel = value; }
        }


        private void frmSelectETOTracer_FormClosed(object sender, FormClosedEventArgs e)
        {
            selectedIndex = listFiles.SelectedIndex;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            cancel = false;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cancel = true;
            this.Close();

        }
    }
}