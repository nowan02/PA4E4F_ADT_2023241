using Newtonsoft.Json;
using PA4E4F_ADT_2023241.Models;

namespace PA4E4F_ADT_2023241.Endpoint
{
    public static class Grader
    {
        public static string Grade(HttpContext Context, int TeacherId, int StudentId, int SubjectId, LogicFactory Factory)
        {
            try
            {
                string input;

                using(StreamReader sr = new StreamReader(Context.Request.Body))
                {
                    input = sr.ReadToEndAsync().Result;
                }

                Grade _newGrade = JsonConvert.DeserializeObject<Grade>(input);

                if(_newGrade == null)
                {
                    Context.Response.StatusCode = 500;

                    return "Object cannot be converted";
                }

                Factory.CreateTeacherLogic().GradeStudentInSubject(TeacherId, StudentId, SubjectId, _newGrade.FinalGrade);

                return $"Student with id {StudentId} has received grade for subject {SubjectId}: {_newGrade.FinalGrade}";
            }
            catch(InvalidCastException)
            {
                Context.Response.StatusCode = 500;

                return "Object cannot be converted";
            }
            catch(Exception ex)
            {
                Context.Response.StatusCode = 500;

                return ex.Message;
            }
        }
    }
}
