using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using PA4E4F_ADT_2023241.Models;

namespace PA4E4F_ADT_2023241.Endpoint
{
    public static class ObjectCreator
    {
        public static string CreateObject<T>(HttpContext Context, int ObjectId, LogicFactory Factory) where T : IDbModel
        {
            try
            {
                string input;

                using (StreamReader sr = new(Context.Request.Body))
                {
                    input = sr.ReadToEndAsync().Result;
                }

                T? _newObject = JsonConvert.DeserializeObject<T>(input);

                if (_newObject is Student && _newObject != null) Factory.CreateStudentLogic().Create(_newObject as Student);
                if (_newObject is Teacher && _newObject != null) Factory.CreateTeacherLogic().Create(_newObject as Teacher);
                if (_newObject is Subject && _newObject != null) Factory.CreateSubjectLogic().Create(_newObject as Subject);

                if(_newObject == null)
                {
                    Context.Response.StatusCode = 200;
                    return "Object cannot be converted.";
                }

                Context.Response.StatusCode = 200;

                return $"{_newObject.GetType().Name} successfully created with id {ObjectId}";
                
            }
            catch (Exception ex)
            {
                Context.Response.StatusCode = 500;

                return ex.Message;
            }
        }
    }
}
