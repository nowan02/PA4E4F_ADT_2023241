using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
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
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
            catch(HttpRequestException)
            {
                Console.WriteLine("Connection refused, please check your URI!");
            }
        }

        public void Run()
        {
            bool run = true;
            while(run)
            {
                Console.WriteLine("Write 'help' to list commands");
                string input = Console.ReadLine().ToLower();
                switch(input)
                {
                    case "exit":
                        run = false; 
                        break;
                    case "help":
                        Console.WriteLine("help: List commands");
                        Console.WriteLine("create: Create new object");
                        Console.WriteLine("listall: List all entries of specified type.");
                        Console.WriteLine("check: List an entry of a specified type and id");
                        Console.WriteLine("exit: exits app");
                        break;
                    case "create": CreateObject();
                        break;
                    case "listall": CheckAll();
                        break;
                    case "check": CheckObject(); 
                        break;
                    default:
                        Console.WriteLine("Unknown command.");
                        break;
                }
            }
            Console.WriteLine("Bye");
        }

        /// <summary>
        /// Serializes the model and sends it to the correct endpoint, returns the response from the server.
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        private string _serialize(IDbModel Model)
        {
            Model.Id = new Random().Next(1000, 9999);
            string JString = JsonConvert.SerializeObject(Model, Formatting.Indented);
            string URI = Uri;

            switch(Model.GetType().Name)
            {
                case "Student": URI += $"/Students/{Model.Id}/Create";
                    break;
                case "Teacher": URI += $"/Teachers/{Model.Id}/Create";
                    break;
                case "Subject": URI += $"/Subjects/{Model.Id}/Create";
                    break;
                case "Grade": URI += $"/Grades/{Model.Id}/Create";
                    break;
                default:
                    Console.WriteLine("Object type cannot be serialized.");
                    break;
            }

            HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Post, URI);
            Request.Content = new StringContent(JString);
            HttpResponseMessage Response = _httpClient.Send(Request);

            return Response.Content.ReadAsStringAsync().Result;
        }
        /// <summary>
        /// Deserializes models based on path, if wrong path is given, returns null.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private IDbModel? _deserialize(string path)
        {
            string URI = Uri + path;

            HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Get, URI);
            HttpResponseMessage Response = _httpClient.Send(Request);
            string content = Response.Content.ReadAsStringAsync().Result;

            if(Response.StatusCode == HttpStatusCode.OK)
            {
                if (path.Contains("Student")) return JsonConvert.DeserializeObject<Student>(content);
                if (path.Contains("Teacher")) return JsonConvert.DeserializeObject<Teacher>(content);
                if (path.Contains("Subject")) return JsonConvert.DeserializeObject<Subject>(content);
                if (path.Contains("Grade")) return JsonConvert.DeserializeObject<Grade>(content);
            }

            return null;
        }

        private List<IDbModel>? _deserializeSeveral(string path)
        {
            string URI = Uri + path;

            HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Get, URI);
            HttpResponseMessage Response = _httpClient.Send(Request);
            string content = Response.Content.ReadAsStringAsync().Result;

            List<IDbModel> Models = new() { };

            if (path.EndsWith("Students")) Models.AddRange(JsonConvert.DeserializeObject<List<Student>>(content));
            if (path.EndsWith("Teachers")) Models.AddRange(JsonConvert.DeserializeObject<List<Teacher>>(content));
            if (path.EndsWith("Subjects")) Models.AddRange(JsonConvert.DeserializeObject<List<Subject>>(content));
            if (path.EndsWith("Grades")) Models.AddRange(JsonConvert.DeserializeObject<List<Grade>>(content));

            return Models;
        }

        public void CreateObject()
        {
            Console.WriteLine("\nChoose object to create: [Student, Teacher, Subject]");
            string? input = Console.ReadLine().ToLower();

            switch(input)
            {
                case "student":
                    Student student = new Student();

                    Console.Clear();
                    Console.WriteLine("Student Name:");

                    student.Name = Console.ReadLine();

                    Console.WriteLine("Student named {0} will be registered.", student.Name);

                    Console.WriteLine("Response from server: {0}", _serialize(student));
                    break;
                case "teacher":
                    Teacher teacher = new Teacher();

                    Console.Clear();
                    Console.WriteLine("Teacher Name:");

                    teacher.Name = Console.ReadLine();

                    Console.WriteLine("Teacher named {0} will be registered.", teacher.Name);

                    Console.WriteLine("Response from server: {0}", _serialize(teacher));
                    break;
                case "subject":
                    Subject subject = new Subject();

                    Console.Clear();
                    Console.WriteLine("Subject Name:");

                    subject.Name = Console.ReadLine();

                    Console.WriteLine("Subject named {0} will be registered.", subject.Name);

                    Console.WriteLine("Response from server: {0}", _serialize(subject));
                    break;
                default:
                    Console.WriteLine("Invalid input.");
                    break;

            }
        }

        public void CheckAll()
        {
            Console.WriteLine("\nChoose object type to list all of: [Student, Teacher, Subject]");
            string? input = Console.ReadLine().ToLower();
            switch (input)
            {
                case "student":
                    Console.WriteLine("All students:");
                    foreach(Student s in _deserializeSeveral("/Students"))
                    {
                        Console.WriteLine("\t{0} {1}", s.Id, s.Name);
                    }
                    break;
                case "teacher":
                    foreach (Teacher t in _deserializeSeveral("/Students"))
                    {
                        Console.WriteLine("\t{0} {1}", t.Id, t.Name);
                    }
                    break;
                case "subject":
                    foreach (Subject su in _deserializeSeveral("/Students"))
                    {
                        Console.WriteLine("\t{0} {1}", su.Id, su.Name);
                    }
                    break;
                default:
                    Console.WriteLine("Object type invalid.");
                    break;
            }
        }

        public void CheckObject()
        {
            Console.WriteLine("\nChoose object to check: [Student, Teacher, Subject]");
            string? input = Console.ReadLine().ToLower();
            try
            {
                int id = Int32.Parse(Console.ReadLine());
                Console.Clear();
                switch (input)
                {
                    case "student":
                        Student s = (Student)_deserialize($"/Students/{id}");
                        Console.WriteLine("Student: {0} {1}", s.Id, s.Name);
                        Console.WriteLine("Enrolled subjects:");
                        foreach(Subject su in _deserializeSeveral($"/Students/{id}/Subjects"))
                        {
                            Console.WriteLine("\t{0} {1}", su.Id, su.Name);
                        }
                        foreach(Grade g in _deserializeSeveral("/Student/{id}/Grades"))
                        {
                            Console.WriteLine("\tSubjectId {0}: {1}", g.SubjectId, g.FinalGrade);
                        }
                        break;
                    case "teacher":
                        Teacher t = (Teacher)_deserialize($"/Teachers/{id}");
                        Console.WriteLine("Teacher: {0} {1}", t.Id, t.Name);
                        Console.WriteLine("Taughts subjects:");
                        foreach (Subject sub in _deserializeSeveral($"/Teachers/{id}/Subjects"))
                        {
                            Console.WriteLine("\t{0} {1}", sub.Id, sub.Name);
                        }
                        break;
                    case "subject":
                        Subject subj = (Subject)_deserialize($"/Subjects/{id}");
                        Teacher sut = (Teacher)_deserialize($"/Subjects/{id}/Teacher");
                        Console.WriteLine("Subject: {0} {1}", subj.Id, subj.Name);
                        Console.WriteLine("Teacher:\n\t{0}", sut.Id, sut.Name);
                        Console.WriteLine("Students enrolled:");
                        foreach (Student sd in _deserializeSeveral($"/Subjects/{id}/Students"))
                        {
                            Console.WriteLine("\t{0} {1}", sd.Id, sd.Name);
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }
            catch(InvalidCastException)
            {
                Console.WriteLine("Invalid id given, aborting command.");
                return;
            }
        }
    }
}
