using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegistrationKiosk;
using System.Threading.Tasks;

namespace ERKTest
{
    [TestClass]
    public class PrinterTests
    {
        // Test that formatter has expected results.
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
            Registrant testRegistrant = CreateRegistrant("毛泽东", "Guðmundsdóttir", "Student", "Eastern Washington University", "Computer Science", "Senior");
            string testString = Printer.FormatRegistrant(testRegistrant);
            Printer.Print(testRegistrant, null);

        }

        // Test getting a registrant by code from the net, and printing it.
        [TestMethod]
        [Ignore]    // Ignored due to hardware usage. Comment out in order to run test.
        public async Task NetPrintTest()
        {
            WebAPI API = new WebAPI("http://www.timjunger.com/", "");
            Registrant testRegistrant = await API.GetRegistrantByCode("123456");
            string testString = Printer.FormatRegistrant(testRegistrant);
            Printer.Print(testRegistrant, null);
        }

        // Used as a shortcut to create Registrant objects.
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
