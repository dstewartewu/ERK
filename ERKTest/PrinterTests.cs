using System;
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
            RegistrantEntry testRegistrant = new RegistrantEntry("Doe", "John", RegistrantEntry.SexType.Male, "name@email.web", "5551234567");
            testRegistrant.SetTypeStudent(RegistrantEntry.ClassStandingType.Senior, "Eastern Washington University", "Computer Science", "0", 2016);
            string testString = Printer.FormatRegistrant(testRegistrant);
            Assert.AreEqual("Senior\nComputer Science\nEastern Washington University", testString);

            //Minimal Student Info
            testRegistrant = new RegistrantEntry("Doe", "John", RegistrantEntry.SexType.Male, "name@email.web", "5551234567");
            testRegistrant.SetTypeStudent(RegistrantEntry.ClassStandingType.None, "", "", "0", 0000);
            testString = Printer.FormatRegistrant(testRegistrant);
            Assert.AreEqual("", testString);

            //Mixed Student Info
            testRegistrant = new RegistrantEntry("Doe", "John", RegistrantEntry.SexType.Male, "name@email.web", "5551234567");
            testRegistrant.SetTypeStudent(RegistrantEntry.ClassStandingType.Junior, "Eastern Washington University", "", "0", 0000);
            testString = Printer.FormatRegistrant(testRegistrant);
            Assert.AreEqual("Junior\nEastern Washington University", testString);

            //Full Employee Info
            testRegistrant = new RegistrantEntry("Anderson", "Montgomery", RegistrantEntry.SexType.Male, "name@email.web", "5551234567");
            testRegistrant.SetTypeEmployee("Dynamic Synergy Co. Inc.", "Team Energistics Supervisor");
            testString = Printer.FormatRegistrant(testRegistrant);
            Assert.AreEqual("Team Energistics Supervisor\nDynamic Synergy Co. Inc.", testString);

            //Mixed Employee Info
            testRegistrant = new RegistrantEntry("Anderson", "Montgomery", RegistrantEntry.SexType.Male, "name@email.web", "5551234567");
            testRegistrant.SetTypeEmployee("Dynamic Synergy Co. Inc.", "");
            testString = Printer.FormatRegistrant(testRegistrant);
            Assert.AreEqual("Dynamic Synergy Co. Inc.", testString);

            //General
            testRegistrant = new RegistrantEntry("Everyman", "William", RegistrantEntry.SexType.Male, "name@email.web", "5551234567");
            testRegistrant.SetTypeGeneral();
            testString = Printer.FormatRegistrant(testRegistrant);
            Assert.AreEqual("", testString);

        }

        [TestMethod]
        [Ignore]    // Ignored due to hardware usage. Comment out in order to run test.
        public void PrintTest()
        {
            Printer printer = new Printer();
            RegistrantEntry testRegistrant = new RegistrantEntry("Anderson", "Montgomery", RegistrantEntry.SexType.Male, "name@email.web", "5551234567");
            //testRegistrant.SetTypeStudent(RegistrantEntry.ClassStandingType.Senior, "Eastern Washington University", "Mechanical Engineering / Computer Systems", "0", 2016);
            testRegistrant.SetTypeEmployee("Dynamic Synergy Co. Inc.", "Team Energistics Supervisor");
            string testString = Printer.FormatRegistrant(testRegistrant);
            printer.Print(testRegistrant);

        }
    }
}
