using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;

namespace RegistrationKiosk {
    [DataContract]
    public class RegistrantEntry {

        //===========================================================================
        #region Enums
        //===========================================================================
        public enum RegistrantType { General, Student, Employee }
        public enum ClassStandingType { None, Freshman, Junior, Sophomore, Senior, PostBac, Graduate, Alumnus }
        public enum SexType { Male, Female }

        #endregion
        //===========================================================================
        #region Properties
        //===========================================================================

        // -------------------------
        #region General Properties
        // -------------------------

        // General
        public string Code {
            get;
            set;
        }
        [DataMember(Name = "codeNumber")]
        public int CodeNum
        {
            get;
            set;
        }

        // Event Number
        [DataMember(Name = "eventNum")]
        public int EventNum
        {
            get;
            set;
        }
        //Registrant Type
        //[DataMember(Name = "regType")] // Enums are handled purely as Ints during serilization.
        public RegistrantType RegType {
            get;
            set;
        }

        [DataMember(Name = "regType")]
        public string RegTypeString
        {
            get;
            set;
        }

        // Name
        [DataMember(Name = "fName")]
        public string Fname {
            get;
            set;
        }

        // last name
        [DataMember(Name = "lName")]
        public string Lname {
            get;
            set;
        }

        // Sex
        public SexType Sex {
            get;
            set;
        }

        //Contact Info
        private string phoneNormal;
        public string Email {
            get;
            set;
        }
        //Phone
        public string Phone {
            get { return FormatPhone(phoneNormal); }
            set { phoneNormal = NormalizePhone(value); }
        }

        #endregion
        // -------------------------
        #region Student Properties
        // -------------------------

        //Class standing
        
        public ClassStandingType ClassStanding {
            get;
            set;
        }
        [DataMember(Name = "classStanding")]
        public string ClassStandingString   // DataContract
        {
            get;
            set;
        }
        //College
        [DataMember(Name = "college")]
        public string College {
            get;
            set;
        }
        //Major
        [DataMember(Name = "major")]
        public string Major {
            get;
            set;
        }
        //Student ID
        public string StudentID {
            get;
            set;
        }
        //graduation year
        public int GradYear {
            get;
            set;
        }
        
        #endregion
        // -------------------------
        #region Employee Properties
        // -------------------------

        //business
        public string Business {
            get;
            set;
        }
        //job
        public string Job {
            get;
            set;
        }
        
        #endregion
        // -------------------------

        #endregion
        //===========================================================================
        #region Constructor
        //===========================================================================

        /// <summary>
        /// A constructor for a blank registrant.
        /// </summary>
        public RegistrantEntry() { }

        /// <summary>
        /// A constructor for a general registrant
        /// </summary>
        /// <param name="lname">Last Name</param>
        /// <param name="fname">First Name</param>
        /// <param name="sex">Sex (Male, Female)</param>
        /// <param name="email">Email Address</param>
        /// <param name="phone">Phone Number</param>
        public RegistrantEntry(string lname, string fname, SexType sex, string email, string phone) {
            this.Lname = lname;
            this.Fname = fname;
            this.Sex = sex;
            this.Email = email;
            this.Phone = phone;
            SetTypeGeneral();
            GenerateCode();
        }

        #endregion
        //===========================================================================
        #region Methods
        //===========================================================================
        
        /// <summary>
        /// Set Registrant data and type to student
        /// </summary>
        /// <param name="classStanding"></param>
        /// <param name="college"></param>
        /// <param name="major"></param>
        /// <param name="studentID"></param>
        /// <param name="gradYear"></param>
        public void SetTypeStudent(ClassStandingType classStanding, string college, string major, string studentID, int gradYear) {
            this.ClassStanding = classStanding;
            this.College = college;
            this.Major = major;
            this.StudentID = studentID;
            this.GradYear = gradYear;
            this.RegType = RegistrantType.Student;
        }

        /// <summary>
        /// Sets registrant type and data to employee
        /// </summary>
        /// <param name="business"></param>
        /// <param name="job"></param>
        public void SetTypeEmployee(string business, string job) {
            this.Business = business;
            this.Job = job;
            this.RegType = RegistrantType.Employee;
        }

        /// <summary>
        /// Sets registrant type to general
        /// </summary>
        public void SetTypeGeneral() {
            this.RegType = RegistrantType.General;
        }

        /// <summary>
        /// Returns a six-digit integer for database lookup of the registrant.
        /// </summary>
        /// <returns>Six-digit hash code</returns>
        public void GenerateCode() {
            Random r = new Random(this.GetHashCode());
            Code = r.Next(1000000).ToString("000000");
        }

        /// <summary>
        /// Normalizes the phone number string to 10 digit number
        /// </summary>
        /// <param name="phone">Phone number</param>
        /// <returns>Normalized phone number</returns>
        public string NormalizePhone(string phone) {
            string result = phone;
            result = Regex.Replace(result, "[^0-9]+", "");
            if (result.Length == 11)
                result = result.Substring(1);
            return result;
        }

        /// <summary>
        /// Changes normalized phone number into (xxx) xxx-xxxx format.
        /// </summary>
        /// <param name="phone">Normalized phone number</param>
        /// <returns>Formatted phone (or original if error)</returns>
        public static string FormatPhone(string phone) {
            try {
                double num = Convert.ToDouble(phone);
                string result = String.Format("{0:(###) ###-####}", num);
                return result;
            } catch (Exception) {
                return phone;
            }
            
        }

        #endregion
        //===========================================================================
    }
}
