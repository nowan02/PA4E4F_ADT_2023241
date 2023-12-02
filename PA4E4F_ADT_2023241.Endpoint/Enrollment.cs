namespace PA4E4F_ADT_2023241.Endpoint
{
    public static class Enrollment
    {
        public static string Enroll(HttpContext Context, int StudentId, int SubjectId, LogicFactory Factory)
        {
            try
            {
                Factory.CreateStudentLogic().EnrollStudentInSubject(StudentId, SubjectId);
                Context.Response.StatusCode = 200;
                return $"Student {StudentId} was successfully enrolled in subject {SubjectId}";
            }
            catch (Exception ex)
            {
                Context.Response.StatusCode = 500;
                return ex.Message;
            }
        }

        public static string Drop(HttpContext Context, int StudentId, int SubjectId, LogicFactory Factory)
        {
            try
            {
                Factory.CreateStudentLogic().DropStudentsSubject(StudentId, SubjectId);
                Context.Response.StatusCode = 200;
                return $"Student {StudentId} was successfully removed from subject {SubjectId}";
            }
            catch (Exception ex)
            {
                Context.Response.StatusCode = 500;
                return ex.Message;
            }
        }
    }
}
