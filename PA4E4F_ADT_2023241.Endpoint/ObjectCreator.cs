using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using PA4E4F_ADT_2023241.Models;

namespace PA4E4F_ADT_2023241.Endpoint
{
    public static class ObjectCreator
    {
        public static string CreateObject<T>(HttpContext Context, int ObjectId, ILogicFactory Factory) where T : IDbModel
        {
            try
            {
                string input;
                
                using (StreamReader sr = new(Context.Request.Body))
                {
                    input = sr.ReadToEndAsync().Result;
                }

                T? _newObject = JsonConvert.DeserializeObject<T>(input);

                if(_newObject == null)
                {
                    Context.Response.StatusCode = 500;
                    return "Object cannot be converted.";
                }

                if (_newObject is Student)
                {
                    Factory.CreateStudentLogic().Create(_newObject as Student);
                }
                else if (_newObject is Teacher)
                {
                    Factory.CreateTeacherLogic().Create(_newObject as Teacher);
                }
                else if (_newObject is Subject)
                {
                    Factory.CreateSubjectLogic().Create(_newObject as Subject);
                }
                else
                {
                    throw new InvalidCastException("Received Json can't be deserialized into an object");
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
