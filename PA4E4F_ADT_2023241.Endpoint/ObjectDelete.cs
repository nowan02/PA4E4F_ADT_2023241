namespace PA4E4F_ADT_2023241.Endpoint
{
    public static class ObjectDelete
    {
        public static string Delete<T>(HttpContext context, int id, LogicFactory Factory)
        {
            try
            {
                switch(typeof(T).Name)
                {
                    case "Student": Factory.CreateStudentLogic().Delete(x => x.Id == id);
                        break;
                    case "Subject": Factory.CreateSubjectLogic().Delete(x => x.Id == id);
                        break;
                    case "Teacher": Factory.CreateTeacherLogic().Delete(x => x.Id == id);
                        break;
                    default:
                        return "Object type was unknown, cannot delete.";
                }

                context.Response.StatusCode = 200;
                return $"{typeof(T).Name} with id {id} was successfully deleted.";
            }
            catch(Exception ex)
            {
                context.Response.StatusCode = 500;
                return ex.Message;
            }
        }
    }
}
