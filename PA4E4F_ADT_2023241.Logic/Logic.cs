using PA4E4F_ADT_2023241.Models;
using PA4E4F_ADT_2023241.Repository;

namespace PA4E4F_ADT_2023241.Logic
{
    public interface ILogic<T> where T : class, IModelWithID
    {
        public bool Create(T Entity);
        public T Read(Func<T, bool> QueryExpression);
        public bool Write(Func<T, bool> QueryExpression);
        public bool Delete(Func<T, bool> QueryExpression);
        public IEnumerable<T> GetAll();
        public bool Update(T Entity, Func<T, bool> QueryExpression);
    }

    public interface IStudentLogic : ILogic<Student>
    {
        public IEnumerable<Grade> GetGradesOfStudent(GradeRepository Grades, Student Student);
        public IEnumerable<Subject> GetSubjectsOfStudent(SubjectRepository Subjects, Student Student);
        public bool EnrollStudentInSubject(SubjectRepository Subjects, Student Student, int SubjectId);
        public bool DropStudentsSubject(SubjectRepository Subjects, Student Student, int SubjectId);
    }

    public interface ITeacherLogic : ILogic<Teacher>
    {
        public IEnumerable<Grade> GetGradesOfTeacher(GradeRepository Grades, Teacher Teacher);
        public IEnumerable<Subject> GetTaughtSubjects(SubjectRepository Subjects, Teacher Teacher);
        public bool GradeStudentInSubject(GradeRepository Grades, Teacher Teacher, Student Studet, int Grade);
    }
}