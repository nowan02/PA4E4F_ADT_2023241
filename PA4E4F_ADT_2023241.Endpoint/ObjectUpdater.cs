using Newtonsoft.Json;
using PA4E4F_ADT_2023241.Models;

namespace PA4E4F_ADT_2023241.Endpoint
{
    public static class ObjectUpdater
    {
        public static string UpdateObject<T>(HttpContext Context, int ObjectId, LogicFactory Factory) where T : IDbModel
        {
            try
            {
                string input;

                using (StreamReader sr = new(Context.Request.Body))
                {
                    input = sr.ReadToEndAsync().Result;
                }

                T? _newObject = JsonConvert.DeserializeObject<T>(input);
                _newObject.Id = ObjectId;

                if (_newObject is Student) Factory.CreateStudentLogic().Update(_newObject as Student, x => x.Id == ObjectId);
                if (_newObject is Teacher) Factory.CreateTeacherLogic().Update(_newObject as Teacher, x => x.Id == ObjectId);
                if (_newObject is Subject) Factory.CreateSubjectLogic().Update(_newObject as Subject, x => x.Id == ObjectId);

                if (_newObject == null)
                {
                    Context.Response.StatusCode = 500;
                    return "Object cannot be converted.";
                }

                return $"{_newObject.GetType().Name} successfully created with id {ObjectId}";
            }
            catch (InvalidOperationException ex)
            {
                Context.Response.StatusCode = 500;

                return ex.Message;
            }
        }
    }
}
