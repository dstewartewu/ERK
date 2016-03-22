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
        private string KioskRegistration;
        public EventInfo Event=  null;

        // Construct using target URI as string, and event number.
        public WebAPI(string target, string kioskRegistration)
        {
            TargetURI = new Uri(target);
            KioskRegistration = kioskRegistration;
            // Get's the Event information as an initial request. Throws exception on failure.
            Task<EventInfo>.Run( async () => { Event = await GetEventInfo(); }).Wait();
            if (Event == null)
                throw new ArgumentException("Could not get event information", "Event");
        }

        // Asynchronously create a WebAPI instance.
        public static async Task<WebAPI> CreateWebAPI(string target, string kioskRegistration)
        {
            WebAPI NewAPI = await Task<WebAPI>.Run(() => { return new WebAPI(target, kioskRegistration); });
            return NewAPI;
        }

        // Returns the test function string of the API.
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

                // Add the kiosk registration and event number to JSON before POSTing.
                JObject RegJSON = JObject.FromObject(registrant);
                RegJSON.Add(new JProperty("kioskReg", KioskRegistration));
                RegJSON["eventNum"] = Event.EventNumber;
                HttpResponseMessage response = await client.PostAsJsonAsync("api/register.php/addRegistrant", RegJSON);
                return response.IsSuccessStatusCode;
            }
        }

        // Update existing student registrant entry
        public async Task<Boolean> UpdateRegistrant(Registrant registrant)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = TargetURI;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Add the kiosk registration and event number to JSON before POSTing.
                JObject RegJSON = JObject.FromObject(registrant);
                RegJSON.Add(new JProperty("kioskReg", KioskRegistration));
                RegJSON["eventNum"] = Event.EventNumber;
                HttpResponseMessage response = await client.PostAsJsonAsync("api/register.php/updateRegistrant", RegJSON);
                return response.IsSuccessStatusCode;
            }
        }

        // Gets the event information linked with the kiosk registration
        public async Task<EventInfo> GetEventInfo()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = TargetURI;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Add the kiosk registration and event number to JSON before POSTing.
                string request = "api/register.php/checkKioskRegistration";
                JObject RegJSON = new JObject();
                RegJSON.Add("kioskReg", KioskRegistration);
                HttpResponseMessage response = await client.PostAsJsonAsync(request, RegJSON);
                if (response.IsSuccessStatusCode)
                {
                    EventInfo[] decodedResponse = JsonConvert.DeserializeObject<EventInfo[]>(await response.Content.ReadAsStringAsync());
                    if (decodedResponse.Length >= 1)    // Check if Event entry was returned, else null
                        return decodedResponse[0];
                }
                return null;
            }
        }
        // Get Registrant object from Code.
        public async Task<Registrant> GetRegistrantByCode(string code)
        {
            string request = ("api/register.php/getRegistrantByCode/" + code + "/" + Event.EventNumber);
            return await GetAsync(request);
        }

        // Get Registrant object from Email.
        public async Task<Registrant> GetRegistrantByEmail(string email)
        {
            string request = ("api/register.php/getRegistrantByEmail/" + email + "/" + Event.EventNumber);
            return await GetAsync(request);
        }

        // Used by other functions to make REST GET requests
        public async Task<Registrant> GetAsync(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = TargetURI;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Append Kiosk registration key to request URI
                request = request + "/" + KioskRegistration;
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
