using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RegistrationKiosk
{
    public class WebAPI
    {
        private Uri TargetURI;
        private int EventNum;
        private string KioskRegistration;

        // Construct using target URI as string, and event number.
        public WebAPI(string target, string kioskRegistration, int eventNum)
        {
            TargetURI = new Uri(target);
            EventNum = eventNum;
            KioskRegistration = kioskRegistration;
        }
        public async Task<string> RunAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = TargetURI;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/");
                if (response.IsSuccessStatusCode)
                    return response.Content.ReadAsStringAsync().Result;
            }
            return "Something went wrong";
        }

        // Add registrant
        public async Task<Boolean> AddStudent(Registrant registrant)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = TargetURI;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                JObject RegJSON = JObject.FromObject(registrant);
                RegJSON.Add(new JProperty("kioskReg", KioskRegistration));
                HttpResponseMessage response = await client.PostAsJsonAsync("api/register.php/addRegistrant", RegJSON);
                return response.IsSuccessStatusCode;
            }
        }

        // Update student
        public async Task<Boolean> UpdateRegistrant(Registrant registrant)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = TargetURI;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                JObject RegJSON = JObject.FromObject(registrant);
                RegJSON.Add(new JProperty("kioskReg", KioskRegistration));
                HttpResponseMessage response = await client.PostAsJsonAsync("api/register.php/updateRegistrant", RegJSON);
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<EventInfo> GetEventInfo()
        {
            // checkKioskRegistration
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = TargetURI;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string request = "api/register.php/checkKioskRegistration";    // Append Kiosk registration key
                JObject RegJSON = new JObject();    // Create JSON for string "kioskReg"
                RegJSON.Add("kioskReg", KioskRegistration);
                HttpResponseMessage response = await client.PostAsJsonAsync(request, RegJSON);
                if (response.IsSuccessStatusCode)
                {
                    EventInfo[] decodedResponse = JsonConvert.DeserializeObject<EventInfo[]>(await response.Content.ReadAsStringAsync());
                    if (decodedResponse.Length >= 1)    // Check if registrant entry was returned, else null
                        return decodedResponse[0];
                }
                return null;
            }
        }
        // Get Registrant object from Code.
        public async Task<Registrant> GetRegistrantByCode(string code)
        {
            string request = ("api/register.php/getRegistrantByCode/" + code + "/" + EventNum);
            return await GetAsync(request);
        }

        // Get Registrant object from Email.
        public async Task<Registrant> GetRegistrantByEmail(string email)
        {
            string request = ("api/register.php/getRegistrantByEmail/" + email + "/" + EventNum);
            return await GetAsync(request);
        }

        // Currently nonfunctional. Supposed to return number of questions
        // TODO: Make functional?
        public async Task<int> GetQuestionCount()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = TargetURI;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string request = ("api/register.php/getQuestionCount/" + EventNum);

                //string TEST = await client.GetAsync(request).Result.Content.ReadAsStringAsync();

                HttpResponseMessage response = await client.GetAsync(request);
                int count = await response.Content.ReadAsAsync<int>();

                return count;
            }
        }

        // Used by other functions to make REST GET calls
        public async Task<Registrant> GetAsync(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = TargetURI;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                request = request + "/" + KioskRegistration;    // Append Kiosk registration key
                HttpResponseMessage response = await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    Registrant[] decodedResponse = JsonConvert.DeserializeObject<Registrant[]>(await response.Content.ReadAsStringAsync());
                    if (decodedResponse.Length >= 1)    // Check if registrant entry was returned, else null
                        return decodedResponse[0];
                }
                return null;
            }
        }
    }
}
