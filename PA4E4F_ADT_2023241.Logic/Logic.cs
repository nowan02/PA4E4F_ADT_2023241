﻿using Castle.DynamicProxy.Generators.Emitters;
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

        public T Read(Func<T, bool> QueryExpression)
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
        public IEnumerable<Grade> GetGradesOfStudent(Student Student);
        public IEnumerable<Subject> GetSubjectsOfStudent(Student Student);
        public void EnrollStudentInSubject(Student Student, int SubjectId);
        public void DropStudentsSubject(Student Student, int SubjectId);
        public double GetStudentAverage(Student Student);
    }

    public interface ITeacherLogic : ILogic<Teacher>
    {
        public IEnumerable<Grade> GetGradesOfTeacher(Teacher Teacher);
        public IEnumerable<Subject> GetTaughtSubjects(Teacher Teacher);
        public void GradeStudentInSubject(Teacher Teacher, Student Student, Subject Subject, int Grade);
    }

    public interface ISubjectLogic : ILogic<Subject>
    {
        public IEnumerable<Student> GetStudentsOnSubject(Subject Subject);
        public double GetGradeAverage(Subject Subject);
        public Teacher GetSubjectTeacher(Subject Subject);
        public IEnumerable<Subject> GetSubjectsWithNoTeacher();
    }
}