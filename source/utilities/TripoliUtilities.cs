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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using Microsoft.Vbe.Interop;
using System.Windows.Forms;
using System.Security.Authentication;

namespace Tripoli.utilities
{
    public static class TripoliUtilities
    {

        public static string getTextFromURI(string fileURI)
        {
            Stream dataStream = null;

            try
            {
                dataStream = getWebStream(fileURI);
            }
            catch (Exception eFile)
            {
                Console.WriteLine(eFile.Message);
                throw new System.ArgumentException(
                    "File: "
                    + fileURI
                    + " does not exist at remote location.");
            }

            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();

            // Cleanup the streams and the response.
            reader.Close();
            dataStream.Close();

            return responseFromServer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public static Stream getWebStream(string requestUri)
        {
            HttpWebRequest request;
            HttpWebResponse response;
            Stream dataStream;

            try
            {
                // nov 2021 updated security using info from next line
                // https://community.developer.authorize.net/t5/Integration-and-Testing/Received-an-unexpected-EOF-or-0-bytes-from-the-transport-stream/td-p/59510
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(SslProtocols)0x00000C00;

                request = (HttpWebRequest)WebRequest.Create(requestUri);
                // Set some reasonable limits on resources used by this request
                request.MaximumAutomaticRedirections = 4;
                request.MaximumResponseHeadersLength = 4;
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                response = (HttpWebResponse)request.GetResponse();

                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
            }
            catch (Exception)
            {
                throw;
            }

            return dataStream;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="verbose"></param>
        /// <returns></returns>
        public static bool checkForTripoliUpdates(bool verbose)
        {
            bool amUpdating = false;
            try
            {
                // split on \n
                // nov 2021
                string[] currentInstaller = getTextFromURI(
                    @"https://raw.githubusercontent.com/bowring/Tripoli/master/.currentVersion").Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                Console.WriteLine("version  " + System.Windows.Forms.Application.ProductVersion.CompareTo(currentInstaller[0])
                    + "\n currentInstaller Info:\n"
                    + currentInstaller[1]);

                // here the ">" handles the beta versions since the last release
                if (System.Windows.Forms.Application.ProductVersion.CompareTo(currentInstaller[0]) >= 0)
                {
                    if (verbose)
                    {
                        MessageBox.Show("You are running the latest version of Tripoli !",
                            "Tripoli Information",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
                else
                {
                    DialogResult retval = MessageBox.Show(
                        "A newer version of Tripoli is available: " + currentInstaller[0] + "."
                        + "\n\nDo you want to install it ?"
                        + "\n\nPlease be sure to read the Release Notes and Help "
                        + "\navailable through the Help menu."
                        + "\n\nWhen you select Yes, Tripoli will quit and launch "
                        + "a browser and download the installer: \n\n" + currentInstaller[1]
                        + "\n\nRun the new installer.",
                        "Tripoli Information",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information);

                    if (retval.Equals(DialogResult.Yes))
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(currentInstaller[1]);
                            amUpdating = true;
                        }
                        catch (System.ArgumentException eFile)
                        {
                            MessageBox.Show("Failed to launch installer because \n\n"
                                + eFile.Message);
                        }
                    }
                    //return amUpdating;
                }
            }
            catch (System.ArgumentException e)
            {
                if (verbose)
                {
                    MessageBox.Show(
                       "Unable to confirm if this is the latest Tripoli version - \n\n "
                                          + " check your Internet connection !",
                                          "Tripoli Information",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Information);
                }
            }
            return amUpdating;
        }
    }
}
