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
//using TRXTD200;
using System.Drawing;

namespace Tripoli
{
	/// <summary>
	/// Wrapper for celexis license wizard
	/// </summary>
	public class TripoliLicense
	{
    //    // Fields
        
    //    private static TRXTD200.mdnTrial mdnTrial1; //= new TRXTD200.mdnTrial();
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public static string TripoliLeaseNumber;
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public static int TripoliLeaseDaysRemaining;

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// 
    //    public TripoliLicense()
    //    {
            
    //        mdnTrial1 = new TRXTD200.mdnTrial();
    //        string tmpKey = null;
    //        // Tripoli key - September 2004
    //        //tmpKey = "I69e35I0Srx4r5evN0Y02z6L7662p97n1Xn37J54P20095R7tn7302TKKT00M1QWK22498oyAAvc78Pap6xB98VTcxiBgQMG4H2k343U5S797t9g70i7671RHcCo67T8638OnUa1a429";

    //        // Tripoli3 key - JUNE 2005 - July reverted to above
    //        //tmpKey = "26e3Y2zxwE278y2625L0441kl4jPNE5MLS33nC00T7m0Q3D5ev6986l66c3faP5r62R2e6b7w2RD803LM78r1w5I7SbOnW38l24U0oWx60P6JHJ2jTR695957D135Zbk17609w77R792";

    //        // Aug 16 2007: note: discovered that machine at CofC had broken license for some reason, but it 
    //        // worked to set the key to the #3 key above and make a build

    //        // Aug 21 2007 : new key for moving forward
    //        tmpKey = "s66OJ5FLryHuQ27H24U06t2o5cbwIfJLLq7A0Z19i1T1TQ06Y7P5j067JG39LN10L2Wqa27ctW6wy1vT00iM0ciaQ9Y962m2Mz3y7B9b2vM01XH65VL6E32I0i70i1Zg9I0QY89Bf19w";

    //        mdnTrial1.Key = tmpKey;
			
    //        // uncomment next try block during development to reset license **********************
    //        try
    //        {
             
    //            mdnTrial1.Remove();
    //        }
    //        catch(Exception ee) {
    //            Console.WriteLine(ee.Message);
    //        }
    //        //*******************************************************************************
			
    //        mdnTrial1.AllowExtend = true;
    //        try
    //        {
    //            TripoliLeaseNumber = mdnTrial1.get_Signature();
    //            TripoliLeaseDaysRemaining = mdnTrial1.TimeLeft();
    //        }
    //        catch{}

    //        mdnTrial1.Popup = TRXTD200.CheckStyles.ccStart;
    //        mdnTrial1.Style = TRXTD200.TrialStyles.ccDays;
    //        mdnTrial1.Duration = 45;
    //        mdnTrial1.ShowWelcome = false;//true;
    //        mdnTrial1.Title = "Tripoli";
    //        mdnTrial1.Copyright = "© 2004-2008 James F. Bowring";
    //        mdnTrial1.Version = "version " + System.Windows.Forms.Application.ProductVersion;
    //        mdnTrial1.Caption = "Tripoli Leasing Wizard";
    //        mdnTrial1.ShowMenu = false;
			
    //        mdnTrial1.LabelBack = "Back";
    //        mdnTrial1.LabelBuyNow = "LABEL BUY NOW";
    //        mdnTrial1.LabelCancel = "Cancel";
			
    //        mdnTrial1.LabelNext = "New Key";
    //        mdnTrial1.LabelOK = "OK"; //"Submit Lease Key";
    //        mdnTrial1.MsgAllow = "Thank you for using Tripoli!";
			
    //        mdnTrial1.MsgDeny = "Thank you for using Tripoli!";
    //        mdnTrial1.MsgEnd = "MSG END";
    //        mdnTrial1.MsgRegister = "This copy of Tripoli now has a permanent lease.";
    //        mdnTrial1.MsgStart = 
    //            @"Please press the 'New Lease Key' button below for instructions "
    //            + @"on extending your Tripoli lease.";
    //        mdnTrial1.MsgTimeLeft = "Your lease has # left. ";
    //        mdnTrial1.MsgWelcome = "MSG WELCOME";
    //        mdnTrial1.UseMessages = true;
    //        mdnTrial1.MsgFirst = "MSG FIRST";
    //        mdnTrial1.ShowWizardImage = false;


    //        // Extension Message
    //        mdnTrial1.LabelReg = "Tripoli Lease #: ";
    //        mdnTrial1.LabelUnlock = "Tripoli Lease Extension Key";
    //        mdnTrial1.MsgExtend = 
    //            @"Thank you for leasing Tripoli!\n"
    //            + @"Please follow these instructions "
    //            + @"for extending this lease.\n\n"
    //            + @"     1) Communicate the Tripoli Lease # in the RED box to us at "
    //            + @"\n\n             bowring@cc.gatech.edu.\n\n"
    //            + @"     2) Enter our Lease Extension Key in the GREEN box.\n"
    //            + @"     3) Press the 'Ok' button.\n\n"
    //            + @"If you have any questions please do not hesitate to contact us.";

    //        mdnTrial1.MsgBadUnlock = 
    //            @"We are sorry, but you have not entered a valid Tripoli "
    //            + @"Lease Extension Key !!";


    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <returns></returns>
    //    public bool CheckLicense()
    //    {
    //        return 	mdnTrial1.Check();
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <returns></returns>
    //    public bool IsRetail()
    //    {
    //        return mdnTrial1.Registered();
    //    }
    }
}
