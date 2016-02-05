using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationKiosk
{
    class RegAdapter
    {
        public static Registrant GetRegistrant(RegistrantEntry regEntry)
        {
            Registrant newReg = new Registrant();

            newReg.RegistrationCode = regEntry.CodeNum.ToString();
            newReg.FirstName = regEntry.Fname;
            newReg.LastName = regEntry.Lname;
            newReg.Email = regEntry.Email;
            newReg.RegistrantType = regEntry.RegTypeString;
            newReg.Major = regEntry.Major;
            newReg.College = regEntry.College;
            newReg.ClassStanding = regEntry.ClassStandingString;
            newReg.Company = regEntry.Business;
            newReg.JobTitle = regEntry.Job;

            return newReg;
        }
    }
}
