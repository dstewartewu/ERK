using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace RegistrationKiosk
{
    [DataContract]
    public class Registrant
    {
        #region DATA MEMBERS

        [DataMember(Name = "codeNum")]
        public String RegistrationCode { get; set; }
        
        [DataMember(Name = "eventNum")]
        public String EventNumber { get; set; }

        [DataMember(Name = "fName")]
        public String FirstName { get; set; }
        
        [DataMember(Name = "lName")]
        public String LastName{ get; set; }
        
        [DataMember(Name = "email")]
        public String Email{ get; set; }
        
        [DataMember(Name = "regType")]
        public String RegistrantType{ get; set; }

        [DataMember(Name = "checkInTime")]
        public String CheckInTime{ get; set; }
        
        [DataMember(Name = "major")]
        public String Major{ get; set; }

        [DataMember(Name = "college")]
        public String College{ get; set; }
        
        [DataMember(Name = "classStanding")]
        public String ClassStanding{ get; set; }

        [DataMember(Name = "company")]
        public String Company{ get; set; }

        [DataMember(Name = "employeePosition")]
        public String Position{ get; set; }

        #endregion

        #region INITIALIZATION

        public Registrant() { }

        #endregion
    }
}
