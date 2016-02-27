using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace RegistrationKiosk
{
    [DataContract]
    public class EventInfo
    {
        public bool PreRegistration;
        public bool CustomQuestions;

        [DataMember(Name = "eventNum")]
        public int EventNumber { get; set; }

        [DataMember(Name = "eventName")]
        public string EventName { get; set; }

        [DataMember(Name = "startTime")]
        public string StartTime { get; set; }

        [DataMember(Name = "endTime")]
        public string EndTime { get; set; }

        [DataMember(Name = "siteHeader")]
        public string SiteHeader { get; set; }

        [DataMember(Name = "preReg")]   // Converts PreReg into/from bool since database uses enum 'true'/'false'
        public string PreRegistrationString
        {
            get { return Convert.ToString(PreRegistration); }
            set { PreRegistration = Convert.ToBoolean(value); }
        }

        [DataMember(Name = "cusQuest")] // Converts CusQuest into/from bool since database uses enum 'true'/'false'
        public string CustomQuestionsString
        { 
            get { return Convert.ToString(CustomQuestions); }
            set { CustomQuestions = Convert.ToBoolean(value); }
        }
    }
}
