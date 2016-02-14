using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        Registrant testRegistrant = new Registrant();
        WebAPI webAPI = new WebAPI(TEST_URI, TEST_EVENTNUM);

        // Run before tests. Initilizes a test registrant.
        [TestInitialize()]
        public void Initialize()
        {
            testRegistrant.FirstName = "AUTO-JOHN";
            testRegistrant.LastName = "TEST-HUMAN";
            testRegistrant.Major = "Unit Testing";
            testRegistrant.College = "Eastern Washington University";
            testRegistrant.RegistrantType = "Student";
            testRegistrant.ClassStanding = "Sophomore";
            testRegistrant.Email = TEST_EMAIL;
            testRegistrant.EventNumber = TEST_EVENTNUM.ToString();
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
        public async Task UpdateRegistrantTest()
        {
            testRegistrant.FirstName = "Jane";
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
            Assert.IsNotNull(testRegistrant.FirstName);
        }
        // Asserts a registrant object has at least the first name returned.
        [TestMethod]
        public async Task GetRegistrantByEmailTest()
        {
            Registrant testRegistrant = new Registrant();
            testRegistrant = await webAPI.GetRegistrantByEmail(TEST_EMAIL);
            Assert.IsNotNull(testRegistrant.FirstName);
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

        [Ignore]    // Ignored until it works at all.
        [TestMethod]
        public async Task GetQuestionCountTest()
        {
            Assert.AreEqual(1, await webAPI.GetQuestionCount());
        }
    }
}
