using PA4E4F_ADT_2023241.Logic;
using PA4E4F_ADT_2023241.Models;
using PA4E4F_ADT_2023241.Repository;
using Newtonsoft.Json;
using PA4E4F_ADT_2023241.Endpoint;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

DatabaseContext dbCtx = new DatabaseContext();

LogicFactory factory = new LogicFactory(
    new StudentRepository(dbCtx),
    new TeacherRepository(dbCtx),
    new SubjectRepository(dbCtx),
    new GradeRepository(dbCtx)
);

app.MapGet("/", () => "It lives");

// Student Endpoints
app.MapGet("/Students", () => JsonConvert.SerializeObject(factory.CreateStudentLogic().ReadAll(), Formatting.Indented));
app.MapGet("/Students/{id}", (int id) => JsonConvert.SerializeObject(factory.CreateStudentLogic().Read(s => s.Id == id), Formatting.Indented));
app.MapGet("/Students/{id}/Grades", (int id) => JsonConvert.SerializeObject(factory.CreateStudentLogic().GetGradesOfStudent(id), Formatting.Indented));

// Teacher Endpoints
app.MapGet("/Teachers", () => JsonConvert.SerializeObject(factory.CreateTeacherLogic().ReadAll(), Formatting.Indented));
app.MapGet("/Teachers/{id}", (int id) => JsonConvert.SerializeObject(factory.CreateTeacherLogic().Read(t => t.Id == id), Formatting.Indented));
app.MapGet("/Teachers/{id}/Grades", (int id) => JsonConvert.SerializeObject(factory.CreateTeacherLogic().Read(t => t.Id == id).GivenGrades, Formatting.Indented));

// Subject Endpoints
app.MapGet("/Subjects", () => JsonConvert.SerializeObject(factory.CreateSubjectLogic().ReadAll(), Formatting.Indented));
app.MapGet("/Subjects/{id}", (int id) => JsonConvert.SerializeObject(factory.CreateSubjectLogic().Read(su => su.Id == id), Formatting.Indented));
app.MapGet("/Subjects/{id}/Grades", (int id) => JsonConvert.SerializeObject(factory.CreateSubjectLogic().Read(su => su.Id == id).Grades, Formatting.Indented));

app.Run();