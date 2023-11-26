using Castle.DynamicProxy.Generators.Emitters;
using PA4E4F_ADT_2023241.Models;
using PA4E4F_ADT_2023241.Repository;
using System.Reflection.Metadata;

namespace PA4E4F_ADT_2023241.Logic
{
    public interface ILogic<T> where T : class, IModelWithID
    {
        public void Create(T Entity);
        public T Read(Func<T, bool> QueryExpression);
        public void Delete(Func<T, bool> QueryExpression);
        public IEnumerable<T> ReadAll();
        public void Update(T Entity, Func<T, bool> QueryExpression);
    }

    public abstract class Logic<T> : ILogic<T> where T : class, IModelWithID
    {
        protected IRepository<T> _ownRepository;

        public Logic(IRepository<T> OwnRepository)
        {
            _ownRepository = OwnRepository;
        }

        public abstract void Create(T Entity);

        public T? Read(Func<T, bool> QueryExpression)
        {
            try
            {
                T? Readable = _ownRepository.ReadAll().FirstOrDefault(QueryExpression);
                return _ownRepository.Read(Readable.Id);
            }
            catch(InvalidOperationException ex) 
            {
                throw ex;
            }
        }

        public IEnumerable<T> ReadAll()
        {
            return _ownRepository.ReadAll().AsEnumerable();
        }    

        public void Update(T Entity, Func<T, bool> QueryExpression)
        {
            try
            {
                T? ToMod = Read(QueryExpression);
                _ownRepository.Update(ToMod.Id, Entity);
            }
            catch(InvalidOperationException ex)
            {
                throw ex;
            }
        }

        public void Delete(Func<T, bool> QueryExpression)
        {
            try
            {
                _ownRepository.Delete(Read(QueryExpression).Id);
            }
            catch(NullReferenceException ex)
            {
                throw ex;
            }
        }
    }

    public interface IStudentLogic : ILogic<Student>
    {
        public IEnumerable<Grade> GetGradesOfStudent(int StudentId);
        public IEnumerable<Subject> GetSubjectsOfStudent(int StudentId);
        public void EnrollStudentInSubject(int StudentId, int SubjectId);
        public void DropStudentsSubject(int StudentId, int SubjectId);
        public double GetStudentAverage(int StudentId);
    }

    public interface ITeacherLogic : ILogic<Teacher>
    {
        public IEnumerable<Grade> GetGradesOfTeacher(int TeacherId);
        public IEnumerable<Subject> GetTaughtSubjects(int TeacherId);
        public void GradeStudentInSubject(int TeacherId, int StudentId, int SubjectId, int Grade);
    }

    public interface ISubjectLogic : ILogic<Subject>
    {
        public IEnumerable<Student> GetStudentsOnSubject(int SubjectId);
        public double GetGradeAverage(int SubjectId);
        public Teacher GetSubjectTeacher(int SubjectId);
        public IEnumerable<Subject> GetSubjectsWithNoTeacher();
    }
}