using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.Management;

namespace RegistrationKiosk{

    /// <summary>
    /// Class containing methods for using a DYMO label printer
    /// </summary>
    public class Printer{
        /*PG: THIS CLASS HAS BEEN COMMENTED OUT FOR COMPILING: THIS CLASS CONTAINS NUMEROUS REFERENCES TO OLD PROJECT CLASSES THAT NO LONGER EXIST.
        /// <summary>
        /// Prints a jobfair nametag
        /// </summary>
        /// <param name="registrant">The registrant to print</param>
        public void Print(RegistrantEntry registrant){
            

            var label = (DYMO.Label.Framework.ILabel)null;
            string labelName = registrant.Fname + ((registrant.Fname.Length + registrant.Lname.Length >= 16) ? "\n" : "") + registrant.Lname;
            string labelDetails = FormatRegistrant(registrant);

            try {
                label = DYMO.Label.Framework.Label.Open(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "/ERK.label");
            }
            catch {
                MessageBox.Show("File 'ERK.label' not found in " + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
                return;
            }
                        
            label.SetObjectText("Name", labelName);
            label.SetObjectText("Details", labelDetails);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
            string printerName = "";

            foreach (ManagementObject printer in searcher.Get()) {
                printerName = printer["Name"].ToString();
                if (printerName.Equals(@"DYMO LabelWriter 450 DUO Label"))
                    if (printer["WorkOffline"].ToString().ToLower().Equals("true"))
                    {
                        MessageBox.Show("'DYMO LabelWriter 450 DUO Label' - Printer not found.");
                    }
                    else
                    {
                        try
                        {
                            label.Print("DYMO LabelWriter 450 DUO Label");

                            MessageBox.Show("Thank you for registering!\n\n" +
                                            "Retrieve your name tag and enjoy the event!");
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("'DYMO LabelWriter 450 DUO Label' - Failed to print");
                        }
                    }
            }
            
        }
        public static string FormatRegistrant(RegistrantEntry registrant)
        {
            // NAME FORMATTING: 16 Characters Each name; Total longer than 15? Split into two lines
            //string regString = registrant.Fname + " " + registrant.Lname;
            string regString = "";
            if (registrant.RegType == RegistrantEntry.RegistrantType.Student)
            {
                if (registrant.ClassStanding != RegistrantEntry.ClassStandingType.None)
                    regString += registrant.ClassStanding.ToString();
                if (!String.IsNullOrWhiteSpace(registrant.Major))
                    regString += (String.IsNullOrWhiteSpace(regString) ? "" : "\n") + registrant.Major;
                if (!String.IsNullOrWhiteSpace(registrant.College))
                    regString += (String.IsNullOrWhiteSpace(regString) ? "" : "\n") + registrant.College;
            }
            else if (registrant.RegType == RegistrantEntry.RegistrantType.Employee)
            {
                if(!String.IsNullOrWhiteSpace(registrant.Job))
                    regString += registrant.Job;
                if (!String.IsNullOrWhiteSpace(registrant.Business))
                    regString += (String.IsNullOrWhiteSpace(regString) ? "" : "\n") + registrant.Business;
            }
            return regString;
        }
        */
    }
}
