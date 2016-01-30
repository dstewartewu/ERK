using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationKiosk
{
    class Registrant
    {
        private String firstName;
        private String lastName;
        private Controller.ClassStanding classStanding;
        private Controller.RegistrantMode registrantType; //PG: Make sure this matches the database registrant.regType enums!
        private String schoolOrCompany;
        private String jobTitle;
        private String eventCode;


    }
}
