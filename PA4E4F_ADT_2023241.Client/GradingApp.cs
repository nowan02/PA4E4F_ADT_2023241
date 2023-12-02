using System.Net;
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

        public bool EnsureConnection()
        {
            _httpClient.BaseAddress = new Uri(Uri);
            Console.WriteLine("Client configured for server: {0}", Uri);
            Console.WriteLine("Checking if server is alive:");
            try
            {
                HttpResponseMessage response = _httpClient.Send(new HttpRequestMessage(HttpMethod.Get, Uri + "/"));
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                return true;
            }
            catch(HttpRequestException)
            {
                Console.WriteLine("Connection refused, please check your URI!");
                return false;
            }
        }

        public void Run()
        {
            bool run = true;
            while(run)
            {
                Console.WriteLine("\nWrite 'help' to list commands");
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
                        Console.WriteLine("unattended: List all classes that have no teachers.");
                        Console.WriteLine("check: List an entry of a specified type and id");
                        Console.WriteLine("delete: Delete an object");
                        Console.WriteLine("enroll: Enroll a student in a subject");
                        Console.WriteLine("drop: Drop a student from a subject");
                        Console.WriteLine("grade: Give a student a grade in a subject");
                        Console.WriteLine("clear: Clear console");
                        Console.WriteLine("exit: exits app");
                        break;
                    case "create": CreateObject();
                        break;
                    case "listall": CheckAll();
                        break;
                    case "check": CheckObject(); 
                        break;
                    case "clear": Console.Clear();
                        break;
                    case "enroll": Enroll();
                        break;
                    case "drop": Drop();
                        break;
                    case "grade": Grade();
                        break;
                    case "unattended": Unattended();
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
            if (path.EndsWith("Subjects") || path.EndsWith("Subjects/Unattended")) Models.AddRange(JsonConvert.DeserializeObject<List<Subject>>(content));
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
                    foreach (Teacher t in _deserializeSeveral("/Teachers"))
                    {
                        Console.WriteLine("\t{0} {1}", t.Id, t.Name);
                    }
                    break;
                case "subject":
                    foreach (Subject su in _deserializeSeveral("/Subjects"))
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
    
        public void DeleteObject()
        {
            Console.WriteLine("\nChoose object to delete: [Student, Teacher, Subject]");
            string? input = Console.ReadLine().ToLower();
            Console.WriteLine("The id of the object:");
            HttpResponseMessage response;
            try
            {
                int id = Int32.Parse(Console.ReadLine());
                Console.Clear();
                switch (input)
                {
                    case "student":
                        response = _httpClient.Send(new HttpRequestMessage(HttpMethod.Delete, Uri + $"/Students/{id}/Delete"));
                        Console.WriteLine("Response from server: {0}", response.Content.ReadAsStringAsync().Result);
                        break;
                    case "teacher":
                        response = _httpClient.Send(new HttpRequestMessage(HttpMethod.Delete, Uri + $"/Teachers/{id}/Delete"));
                        Console.WriteLine("Response from server: {0}", response.Content.ReadAsStringAsync().Result);
                        break;
                    case "subject":
                        response = _httpClient.Send(new HttpRequestMessage(HttpMethod.Delete, Uri + $"/Subjects/{id}/Delete"));
                        Console.WriteLine("Response from server: {0}", response.Content.ReadAsStringAsync().Result);
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }
            catch (InvalidCastException)
            {
                Console.WriteLine("Invalid id given, aborting command.");
                return;
            }
        }

        public void Enroll()
        {
            try
            {
                Console.WriteLine("\nChoose student id:");
                int StudentId = int.Parse(Console.ReadLine());

                Console.WriteLine("Choose a subject id:");
                int SubjectId = int.Parse(Console.ReadLine());

                HttpResponseMessage response = _httpClient.Send(new HttpRequestMessage(HttpMethod.Put, Uri + $"/Students/{StudentId}/Enroll/{SubjectId}"));
                Console.WriteLine("Response from server: {0}", response.Content.ReadAsStringAsync().Result);
            }
            catch (InvalidCastException)
            {
                Console.WriteLine("Invalid id given, aborting command.");
                return;
            }
        }

        public void Drop()
        {
            try
            {
                Console.WriteLine("\nChoose student id:");
                int StudentId = int.Parse(Console.ReadLine());

                Console.WriteLine("Choose a subject id:");
                int SubjectId = int.Parse(Console.ReadLine());

                HttpResponseMessage response = _httpClient.Send(new HttpRequestMessage(HttpMethod.Put, Uri + $"/Students/{StudentId}/Drop/{SubjectId}"));
                Console.WriteLine("Response from server: {0}", response.Content.ReadAsStringAsync().Result);
            }
            catch (InvalidCastException)
            {
                Console.WriteLine("Invalid id given, aborting command.");
                return;
            }
        }

        public void Grade()
        {
            try
            {
                Console.WriteLine("\nChoose student id:");
                int StudentId = int.Parse(Console.ReadLine());

                Console.WriteLine("Choose a teacher id:");
                int TeacherId = int.Parse(Console.ReadLine());

                Console.WriteLine("Choose a subject id:");
                int SubjectId = int.Parse(Console.ReadLine());

                Console.WriteLine("Grade value < 0 - 5 >");
                int FinalGrade = int.Parse(Console.ReadLine());

                Grade newgrade = new Grade
                {
                    StudentId = StudentId,
                    SubjectId = SubjectId,
                    TeacherId = TeacherId,
                    FinalGrade = FinalGrade
                };

                Console.WriteLine("What is the grade for? (Not mandatory):");
                newgrade.Name = Console.ReadLine();

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, Uri + $"/Teachers/{TeacherId}/Grade/{StudentId}");
                request.Content = new StringContent(JsonConvert.SerializeObject(newgrade, Formatting.Indented));
                HttpResponseMessage response = _httpClient.Send(request);
                Console.WriteLine("Response from server: {0}", response.Content.ReadAsStringAsync().Result);
            }
            catch (InvalidCastException)
            {
                Console.WriteLine("Invalid id given, aborting command.");
                return;
            }
        }

        public void Unattended()
        {
            Console.WriteLine("\n Subjects with no teachers:");
            foreach (Subject su in _deserializeSeveral("/Subjects/Unattended"))
            {
                Console.WriteLine("\t{0}, {1}", su.Id, su.Name);
            }
        }
    }
}
