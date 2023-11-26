using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PA4E4F_ADT_2023241.Models;

namespace PA4E4F_ADT_2023241.Client
{
    internal partial class GradingApp
    {
        public string Uri;
        private HttpClient _httpClient;
        
        public GradingApp(string uri, HttpClient httpClient)
        {
            Uri = uri;
            _httpClient = httpClient;
        }

        public void EnsureConnection()
        {
            _httpClient.BaseAddress = new Uri(Uri);
            Console.WriteLine("Client configured for server: {0}", Uri);
            Console.WriteLine("Checking if server is alive:");
            try
            {
                HttpResponseMessage response = _httpClient.Send(new HttpRequestMessage(HttpMethod.Get, Uri + "/"));
                _ = response.Content.ReadAsStringAsync().ContinueWith(c => Console.WriteLine("Response from server: {0}", c.Result));
            }
            catch(HttpRequestException)
            {
                Console.WriteLine("Connection refused, please check your URI!");
            }
        }

        /// <summary>
        /// Serializes the model and sends it to the correct endpoint, returns the response from the server.
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        private string _serialize(IModelWithID Model)
        {
            string JString = JsonConvert.SerializeObject(Model, Formatting.Indented);
            string URI = Uri;
            int id = new Random().Next(1000, 9999);

            switch(Model.GetType().Name)
            {
                case "Student": URI += $"/Students/{id}/Create";
                    break;
                case "Teacher": URI += $"/Teachers/{id}/Create";
                    break;
                case "Subject": URI += $"/Subjects/{id}/Create";
                    break;
                case "Grade": URI += $"/Grades/{id}/Create";
                    break;
                default:
                    Console.WriteLine("Object type cannot be serialized.");
                    break;
            }

            HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Post, URI);
            Request.Content = new StringContent(JString);
            HttpResponseMessage response = _httpClient.Send(Request);

            return response.Content.ReadAsStringAsync().Result;
        }

        public void CreateStudent()
        {
            Student s = new Student
            {
                Name = "Testing Thomas"
            };

            Console.WriteLine(_serialize(s));
        }
    }
}
