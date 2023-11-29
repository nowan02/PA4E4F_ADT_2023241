using PA4E4F_ADT_2023241.Models;
using PA4E4F_ADT_2023241.Repository;
using Newtonsoft.Json;
using PA4E4F_ADT_2023241.Endpoint;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<KestrelServerOptions>(options => {
    options.AllowSynchronousIO = false;
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

// Student GET
app.MapGet("/Students", async () => await Task<String>.Run(() => JsonConvert.SerializeObject(factory.CreateStudentLogic().ReadAll(), Formatting.Indented)));
app.MapGet("/Students/{id}", async (int id) => await Task<String>.Run(() => JsonConvert.SerializeObject(factory.CreateStudentLogic().Read(s => s.Id == id), Formatting.Indented)));
app.MapGet("/Students/{id}/Grades", async (int id) => await Task<String>.Run(() => JsonConvert.SerializeObject(factory.CreateStudentLogic().GetGradesOfStudent(id), Formatting.Indented)));
app.MapGet("/Students/{id}/Subjects", async (int id) => await Task<String>.Run(() => JsonConvert.SerializeObject(factory.CreateStudentLogic().GetSubjectsOfStudent(id), Formatting.Indented)));
app.MapGet("/Students/{id}/Average", async (int id) => await Task<String>.Run(() => JsonConvert.SerializeObject(factory.CreateStudentLogic().GetStudentAverage(id), Formatting.Indented)));

// Student POST
app.MapPost("/Students/{id}/Create", async (HttpContext context, int id) => await Task<String>.Run(() => ObjectCreator.CreateObject<Student>(context, id, factory)));

// Student PUT
app.MapPut("/Students/{id}/Update", async (HttpContext context, int id) => await Task<String>.Run(() => ObjectUpdater.UpdateObject<Student>(context, id, factory)));

// Teacher GET
app.MapGet("/Teachers", async () => await Task<String>.Run(() => JsonConvert.SerializeObject(factory.CreateTeacherLogic().ReadAll(), Formatting.Indented)));
app.MapGet("/Teachers/{id}", async (int id) => await Task<String>.Run(() => JsonConvert.SerializeObject(factory.CreateTeacherLogic().Read(t => t.Id == id), Formatting.Indented)));
app.MapGet("/Teachers/{id}/Grades", async (int id) => await Task<String>.Run(() => JsonConvert.SerializeObject(factory.CreateTeacherLogic().GetGradesOfTeacher(id), Formatting.Indented)));
app.MapGet("/Teachers/{id}/Subjects", async (int id) => await Task<String>.Run(() => JsonConvert.SerializeObject(factory.CreateTeacherLogic().GetTaughtSubjects(id), Formatting.Indented)));

// Teacher POST
app.MapPost("/Teachers/{id}/Create", async (HttpContext context, int id) => await Task<String>.Run(() => ObjectCreator.CreateObject<Teacher>(context, id, factory)));
app.MapPost("/Teachers/{id}/Grade/{StudentId}", async (HttpContext context, int id, int StudentId, int SubjectId) => await Task<String>.Run(() => Grader.Grade(context, id, StudentId, SubjectId, factory)));

// Teacher PUT
app.MapPut("/Teachers/{id}/Update", (HttpContext context, int id) => ObjectUpdater.UpdateObject<Teacher>(context, id, factory));

// Subject GET
app.MapGet("/Subjects", async () => await Task<String>.Run(() => JsonConvert.SerializeObject(factory.CreateSubjectLogic().ReadAll(), Formatting.Indented)));
app.MapGet("/Subjects/{id}", async (int id) => await Task<String>.Run(() => JsonConvert.SerializeObject(factory.CreateSubjectLogic().Read(su => su.Id == id), Formatting.Indented)));
app.MapGet("/Subjects/{id}/Students", async (int id) => await Task<String>.Run(() => JsonConvert.SerializeObject(factory.CreateSubjectLogic().GetStudentsOnSubject(id), Formatting.Indented)));

// Subject POST
app.MapPost("/Subjects/{id}/Create", async (HttpContext context, int id) => await Task<String>.Run(() => ObjectCreator.CreateObject<Subject>(context, id, factory)));

// Subject PUT
app.MapPut("/Subjects/{id}/Update", async (HttpContext context, int id) => await Task<String>.Run(() => ObjectUpdater.UpdateObject<Subject>(context, id, factory)));

app.Run();