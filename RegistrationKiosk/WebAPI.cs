using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace RegistrationKiosk
{
    public class WebAPI
    {
        public Uri TargetURI;
        int EventNum;

        // Construct using target URI as string, and event number.
        public WebAPI(string _target, int _eventNum)
        {
            TargetURI = new Uri(_target);
            EventNum = _eventNum;
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

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Registrant));
                /*MemoryStream ms = new MemoryStream();
                serializer.WriteObject(ms, registrant);
                ms.Position = 0;
                StreamReader sr = new StreamReader(ms);
                string json = (sr.ReadToEnd());*/

                HttpResponseMessage response = await client.PostAsJsonAsync("api/index.php/addStudent", registrant);
                return response.IsSuccessStatusCode;
            }
        }

        // Update student
        public async Task<Boolean> UpdateStudent(Registrant registrant)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = TargetURI;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PutAsJsonAsync("api/index.php/updateStudent", registrant);
                return response.IsSuccessStatusCode;
            }
        }

        // Get Registrant object from Code.
        public async Task<Registrant> GetRegistrantByCode(int code)
        {
            string request = ("api/index.php/getRegistrantByCode/" + code + "/" + EventNum);
            return await GetAsync(request);
        }

        // Get Registrant object from Email.
        public async Task<Registrant> GetRegistrantByEmail(string email)
        {
            string request = ("api/index.php/getRegistrantByEmail/" + email + "/" + EventNum);
            return await GetAsync(request);
        }

        // Currently nonfunctional. Supposed to return 
        public async Task<int> GetQuestionCount()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = TargetURI;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string request = ("api/index.php/getQuestionCount/" + EventNum);

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

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Registrant));

                Stream response = await client.GetStreamAsync(request);

                Registrant registrant = ((Registrant[])serializer.ReadObject(response))[0];
                return registrant;
            }
        }
    }
}
