using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegistrationKiosk;
using System.Threading.Tasks;

namespace ERKTest
{
    [TestClass]
    public class WebAPITests
    {
        const int TEST_CODE = 123456;
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
            testRegistrant.RegistrantType = "Student";
            testRegistrant.ClassStanding = "Sophmore";
            testRegistrant.Email = TEST_EMAIL;
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
            testRegistrant.FirstName = "Jane";
            Assert.IsTrue(await webAPI.UpdateStudent(testRegistrant));
            //testRegistrant.Fname = "John";
            //Assert.IsTrue(await webAPI.UpdateStudent(testRegistrant));
        }
        [TestMethod]
        public async Task GetRegistrantByCodeTest()
        {
            Registrant testRegistrant = new Registrant();
            testRegistrant = await webAPI.GetRegistrantByCode(TEST_CODE);
            Assert.AreEqual("Test", testRegistrant.FirstName);
        }
        [TestMethod]
        public async Task GetRegistrantByEmailTest()
        {
            Registrant testRegistrant = new Registrant();
            testRegistrant = await webAPI.GetRegistrantByEmail(TEST_EMAIL);
            Assert.AreEqual("Test", testRegistrant.FirstName);
        }

        [Ignore]    // Ignored until it works at all.
        [TestMethod]
        public async Task GetQuestionCountTest()
        {
            Assert.AreEqual(1, await webAPI.GetQuestionCount());
        }
    }
}
