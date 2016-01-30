using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using RegistrationKiosk;

namespace ERKTest
{
    [TestClass]
    public class WebAPITests
    {
        const int TEST_CODE = 123456;
        const int TEST_EVENTNUM = 1;
        const string TEST_EMAIL = "fakemail@notreal.biz";
        const string TEST_URI = "http://www.timjunger.com/";
        RegistrantEntry testRegistrant = new RegistrantEntry("Doe", "John", RegistrantEntry.SexType.Male, "name@email.web", "5551234567");
        WebAPI webAPI = new WebAPI(TEST_URI, TEST_EVENTNUM);

        // Run before tests. Initilizes a test registrant.
        [TestInitialize()]
        public void Initialize()
        {
            testRegistrant.SetTypeStudent(RegistrantEntry.ClassStandingType.Senior, "ICELAND", "\'BILLIONAIRE ENTREPENUER\'", "0", 2016);
            testRegistrant.EventNum = 1;
            testRegistrant.RegTypeString = "Student";
            testRegistrant.ClassStandingString = "Freshman";
            testRegistrant.CodeNum = TEST_CODE;
        }

        // Checks against the basic test return of the API
        [TestMethod]
        public async Task TestFunctionTest()
        {
            string response = await webAPI.RunAsync();
            Assert.IsTrue(response.Contains("Welcome to EWU Career Services Pre-Registration API. This is a test function."));
        }

        // Adds a student, not a very comprehensive check that it worked.
        [TestMethod]
        public async Task AddStudentTest()
        {    
            Assert.IsTrue(await webAPI.AddStudent(testRegistrant));
        }

        // Update test registrant entry
        [Ignore] // Currently non-functional. Ignoring until implemented properly.
        [TestMethod]
        public async Task UpdateStudent()
        {
            testRegistrant.Fname = "Jane";
            Assert.IsTrue(await webAPI.UpdateStudent(testRegistrant));
            //testRegistrant.Fname = "John";
            //Assert.IsTrue(await webAPI.UpdateStudent(testRegistrant));
        }
        [TestMethod]
        public async Task GetRegistrantByCodeTest()
        {
            RegistrantEntry testRegistrant = new RegistrantEntry();
            testRegistrant = await webAPI.GetRegistrantByCode(TEST_CODE);
            Assert.AreEqual("Test", testRegistrant.Fname);
        }
        [TestMethod]
        public async Task GetRegistrantByEmailTest()
        {
            RegistrantEntry testRegistrant = new RegistrantEntry();
            testRegistrant = await webAPI.GetRegistrantByEmail(TEST_EMAIL);
            Assert.AreEqual("Test", testRegistrant.Fname);
        }

        [Ignore]    // Ignored until it works at all.
        [TestMethod]
        public async Task GetQuestionCountTest()
        {
            Assert.AreEqual(1, await webAPI.GetQuestionCount());
        }
    }
}
