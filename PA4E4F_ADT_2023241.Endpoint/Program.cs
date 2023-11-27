using PA4E4F_ADT_2023241.Logic;
using PA4E4F_ADT_2023241.Models;
using PA4E4F_ADT_2023241.Repository;
using Newtonsoft.Json;
using PA4E4F_ADT_2023241.Endpoint;
using System.Text.Json.Serialization;
using System.IO.Pipelines;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.AllowSynchronousIO = true;
});
var app = builder.Build();

DatabaseContext dbCtx = new DatabaseContext();

LogicFactory factory = new LogicFactory(
    new StudentRepository(dbCtx),
    new TeacherRepository(dbCtx),
    new SubjectRepository(dbCtx),
    new GradeRepository(dbCtx)
);

app.MapGet("/", () => "Working as intended!");

// Student Endpoints
app.MapGet("/Students", () => JsonConvert.SerializeObject(factory.CreateStudentLogic().ReadAll(), Formatting.Indented));
app.MapGet("/Students/{id}", (int id) => JsonConvert.SerializeObject(factory.CreateStudentLogic().Read(s => s.Id == id), Formatting.Indented));
app.MapGet("/Students/{id}/Grades", (int id) => JsonConvert.SerializeObject(factory.CreateStudentLogic().GetGradesOfStudent(id), Formatting.Indented));

app.MapPost("/Students/{id}/Create", (HttpContext context, int id) => { 
    try
    {
        string input;
        
        using(StreamReader sr = new (context.Request.Body))
        {
            input = sr.ReadToEnd();
        }

        Student? s = JsonConvert.DeserializeObject<Student>(input);
        s.Id = id;

        factory.CreateStudentLogic().Create(s);

        using (StreamWriter sw = new StreamWriter(context.Response.Body))
        {
            sw.WriteLine("Student successfully created with id {0}", id);
        }
    }
    catch(InvalidCastException)
    {
        context.Response.StatusCode = 500;
        
        using(StreamWriter sw = new StreamWriter(context.Response.Body))
        {
            sw.WriteLine("Object sent cannot be converted to type 'Student'.");
        }
    }
    catch(ArgumentException ex)
    {
        context.Response.StatusCode = 500;

        using (StreamWriter sw = new StreamWriter(context.Response.Body))
        {
            sw.WriteLine(ex.Message);
        }
    }

    return Task.CompletedTask;
});

// Teacher Endpoints
app.MapGet("/Teachers", () => JsonConvert.SerializeObject(factory.CreateTeacherLogic().ReadAll(), Formatting.Indented));
app.MapGet("/Teachers/{id}", (int id) => JsonConvert.SerializeObject(factory.CreateTeacherLogic().Read(t => t.Id == id), Formatting.Indented));
app.MapGet("/Teachers/{id}/Grades", (int id) => JsonConvert.SerializeObject(factory.CreateTeacherLogic().Read(t => t.Id == id).GivenGrades, Formatting.Indented));

app.MapPost("/Teachers/{id}/Create", (HttpContext context, int id) => {
    try
    {
        string input;

        using (StreamReader sr = new(context.Request.Body))
        {
            input = sr.ReadToEnd();
        }

        Teacher? t = JsonConvert.DeserializeObject<Teacher>(input);
        t.Id = id;

        factory.CreateTeacherLogic().Create(t);

        using (StreamWriter sw = new StreamWriter(context.Response.Body))
        {
            sw.WriteLine("Teacher successfully created with id {0}", id);
        }
    }
    catch (InvalidCastException)
    {
        context.Response.StatusCode = 500;

        using (StreamWriter sw = new StreamWriter(context.Response.Body))
        {
            sw.WriteLine("Object sent cannot be converted to type 'Teacher'.");
        }
    }
    catch (ArgumentException ex)
    {
        context.Response.StatusCode = 500;

        using (StreamWriter sw = new StreamWriter(context.Response.Body))
        {
            sw.WriteLine(ex.Message);
        }
    }

    return Task.CompletedTask;
});

// Subject Endpoints
app.MapGet("/Subjects", () => JsonConvert.SerializeObject(factory.CreateSubjectLogic().ReadAll(), Formatting.Indented));
app.MapGet("/Subjects/{id}", (int id) => JsonConvert.SerializeObject(factory.CreateSubjectLogic().Read(su => su.Id == id), Formatting.Indented));
app.MapGet("/Subjects/{id}/Grades", (int id) => JsonConvert.SerializeObject(factory.CreateSubjectLogic().Read(su => su.Id == id).Grades, Formatting.Indented));

app.MapPost("/Subjects/{id}/Create", (HttpContext context, int id) => {
    try
    {
        string input;

        using (StreamReader sr = new(context.Request.Body))
        {
            input = sr.ReadToEnd();
        }

        Subject? su = JsonConvert.DeserializeObject<Subject>(input);
        su.Id = id;

        factory.CreateSubjectLogic().Create(su);

        using (StreamWriter sw = new StreamWriter(context.Response.Body))
        {
            sw.WriteLine("Subject successfully created with id {0}", id);
        }
    }
    catch (InvalidCastException)
    {
        context.Response.StatusCode = 500;

        using (StreamWriter sw = new StreamWriter(context.Response.Body))
        {
            sw.WriteLine("Object sent cannot be converted to type 'Subject'.");
        }
    }
    catch (ArgumentException ex)
    {
        context.Response.StatusCode = 500;

        using (StreamWriter sw = new StreamWriter(context.Response.Body))
        {
            sw.WriteLine(ex.Message);
        }
    }

    return Task.CompletedTask;
});

app.Run();