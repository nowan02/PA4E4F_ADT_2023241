using PA4E4F_ADT_2023241.Models;
using PA4E4F_ADT_2023241.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA4E4F_ADT_2023241.Logic
{
    public class StudentLogic : Logic<Student>, IStudentLogic
    {
        private Repository<Student> _ownRepository;
        private Repository<Grade> _gradeRepository;
        private Repository<Subject> _subjectRepository;

        public StudentLogic(Repository<Student> OwnRepository, Repository<Grade> GradeRepository, Repository<Subject> SubjectRepository) : base(OwnRepository) 
        {
            _gradeRepository = GradeRepository;
            _subjectRepository = SubjectRepository;
        }

        public override void Create(Student Student)
        {
            try
            {
                Read(s => s.Id == Student.Id);

                throw new ArgumentException("Student Id was not unique!");
            }
            catch(InvalidOperationException)
            {
                if (Student.Name == null || Student.Name.Length == 0)
                {
                    throw new ArgumentException("Student name was empty and is required!");
                }
            }

            _ownRepository.Create(Student);
        }
        public IEnumerable<Grade> GetGradesOfStudent(Student Student)
        {
            return _gradeRepository.ReadAll().Where(g => g.StudentId == Student.Id).AsEnumerable();
        }

        public IEnumerable<Subject> GetSubjectsOfStudent(Student Student)
        {
            return _subjectRepository.ReadAll().Where(su => su.EnrolledStudents.Contains(Student)).AsEnumerable();
        }

        public void EnrollStudentInSubject(Student Student, int Subjectid) 
        {
            Subject su = _subjectRepository.Read(Subjectid);
            su.EnrolledStudents.Add(Student);
        }

        public void DropStudentsSubject(Student Student, int Subjectid)
        {
            Subject su = _subjectRepository.Read(Subjectid);
            su.EnrolledStudents.Remove(Student);
        }
    }
}
