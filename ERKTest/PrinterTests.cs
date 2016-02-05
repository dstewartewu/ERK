using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegistrationKiosk;

namespace ERKTest
{
    [TestClass]
    public class PrinterTests
    {
        [TestMethod]
        public void FormatTest()
        {
            //Full Student Info
            Registrant testRegistrant = CreateRegistrant("John", "Doe", "Student", "Eastern Washington University", "Computer Science", "Senior");
            string testString = Printer.FormatRegistrant(testRegistrant);
            Assert.AreEqual("Senior\nComputer Science\nEastern Washington University", testString);

            //Minimal Student Info
            testRegistrant = CreateRegistrant("John", "Doe", "Student", "", "", "");
            testString = Printer.FormatRegistrant(testRegistrant);
            Assert.AreEqual("", testString);

            //Mixed Student Info - University, Class Standing
            testRegistrant = CreateRegistrant("John", "Doe", "Student", "Eastern Washington University", "", "Junior");
            testString = Printer.FormatRegistrant(testRegistrant);
            Assert.AreEqual("Junior\nEastern Washington University", testString);

            //General
            testRegistrant = CreateRegistrant("Everyman", "William", "General", "", "", "");
            testString = Printer.FormatRegistrant(testRegistrant);
            Assert.AreEqual("", testString);

        }

        [TestMethod]
        [Ignore]    // Ignored due to hardware usage. Comment out in order to run test.
        public void PrintTest()
        {
            Printer printer = new Printer();
            Registrant testRegistrant = CreateRegistrant("John", "Doe", "Student", "Eastern Washington University", "Computer Science", "Senior");
            string testString = Printer.FormatRegistrant(testRegistrant);
            printer.Print(testRegistrant);

        }

        public static Registrant CreateRegistrant(string _FirstName, string _LastName, string _RegistrantType, string _College, string _Major, string _ClassStanding)
        {
            Registrant testRegistrant = new Registrant();
            testRegistrant.FirstName = _FirstName;
            testRegistrant.LastName = _LastName;
            testRegistrant.RegistrantType = _RegistrantType;
            testRegistrant.Major = _Major;
            testRegistrant.College = _College;
            testRegistrant.ClassStanding = _ClassStanding;

            return testRegistrant;
        }
    }
}
