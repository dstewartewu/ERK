﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegistrationKiosk;
using System.Threading.Tasks;

namespace ERKTest
{
    [TestClass]
    public class WebAPITests
    {
        const string TEST_CODE = "123456";
        const int TEST_EVENTNUM = 1;
        const string TEST_EMAIL = "fakemail@notreal.biz";
        const string TEST_URI = "http://www.timjunger.com/";
        const string TEST_REGISTRATION = "";
        Registrant testRegistrant = new Registrant();
        WebAPI webAPI = new WebAPI(TEST_URI, TEST_REGISTRATION);

        // Run before tests. Initilizes a test registrant.
        [TestInitialize()]
        public void Initialize()
        {
            testRegistrant.FirstName = "Unit";
            testRegistrant.LastName = "Test";
            testRegistrant.Major = "Unit Testing";
            testRegistrant.College = "Eastern Washington University";
            testRegistrant.RegistrantType = "Student";
            testRegistrant.ClassStanding = "Sophomore";
            testRegistrant.RegistrationCode = TEST_CODE;
            testRegistrant.EventNumber = TEST_EVENTNUM;
            testRegistrant.Email = TEST_EMAIL;
            testRegistrant.EventNumber = TEST_EVENTNUM;
        }
        [TestMethod]
        public async Task CreateWebAPITest()
        {
            WebAPI TestAPI = await WebAPI.CreateWebAPI(TEST_URI, TEST_REGISTRATION);
            Assert.IsNotNull(TestAPI.Event);
        }
        // Checks against the basic test return of the API
        [TestMethod]
        public async Task TestFunctionTest()
        {
            string response = await webAPI.RunAsync();
            Assert.IsTrue(response.Contains("Welcome to EWU Career Services Pre-Registration API. This is a test function."));
        }

        [TestMethod]
        public async Task EventInfoTest()
        {
            EventInfo Info = await webAPI.GetEventInfo();
            Assert.IsNotNull(Info);
        }
        // Adds a student, not a very comprehensive check that it worked.
        [TestMethod]
        public async Task AddStudentTest()
        {    
            Assert.IsTrue(await webAPI.AddStudent(testRegistrant));
        }

        // Update test registrant entry
        //[Ignore]
        [TestMethod]
        public async Task UpdateRegistrantTest()
        {
            testRegistrant.Major = "Unit Testing - UPDATE";
            Assert.IsTrue(await webAPI.UpdateRegistrant(testRegistrant));
            //testRegistrant.Fname = "John";
            //Assert.IsTrue(await webAPI.UpdateStudent(testRegistrant));
        }
        // Asserts a registrant object has at least the first name returned.
        [TestMethod]
        public async Task GetRegistrantByCodeTest()
        {
            Registrant testRegistrant = new Registrant();
            testRegistrant = await webAPI.GetRegistrantByCode(TEST_CODE);
            Assert.IsNotNull(testRegistrant);
        }
        // Asserts a registrant object has at least the first name returned.
        [TestMethod]
        public async Task GetRegistrantByEmailTest()
        {
            Registrant testRegistrant = new Registrant();
            testRegistrant = await webAPI.GetRegistrantByEmail(TEST_EMAIL);
            Assert.IsNotNull(testRegistrant);
        }
        // Assert that nonexistant codes return null
        [TestMethod]
        public async Task GetRegistrantByCodeNullTest()
        {
            Registrant testRegistrant = new Registrant();
            testRegistrant = await webAPI.GetRegistrantByCode("000000");
            Assert.IsNull(testRegistrant);
        }
        // Assert that nonexistant emails return null
        [TestMethod]
        public async Task GetRegistrantByEmailNullTest()
        {
            Registrant testRegistrant = new Registrant();
            testRegistrant = await webAPI.GetRegistrantByEmail("NotAnEmail");
            Assert.IsNull(testRegistrant);
        }
    }
}
