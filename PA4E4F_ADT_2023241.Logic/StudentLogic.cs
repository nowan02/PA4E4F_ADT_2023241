﻿using PA4E4F_ADT_2023241.Models;
using PA4E4F_ADT_2023241.Repository;

namespace PA4E4F_ADT_2023241.Logic
{
    public class StudentLogic : Logic<Student>, IStudentLogic
    {
        private IGradeRepository _gradeRepository;
        private ISubjectRepository _subjectRepository;

        public StudentLogic(IStudentRepository OwnRepository, IGradeRepository GradeRepository, ISubjectRepository SubjectRepository) : base(OwnRepository) 
        {
            _gradeRepository = GradeRepository;
            _subjectRepository = SubjectRepository;
        }

        public override void Create(Student Student)
        {

            if (Student.Name == null || Student.Name.Length == 0)
            {
                throw new ArgumentException("Student name was empty and is required!");
            }

            if (_ownRepository.Read(Student.Id) != null) throw new ArgumentException("Student Id was not unique!");

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
            Subject? su = _subjectRepository.Read(Subjectid);
            su.EnrolledStudents.Add(Student);
            _subjectRepository.Update(su.Id, su);
            Student.Subjects.Add(su);
            _ownRepository.Update(Student.Id, Student);
        }

        public void DropStudentsSubject(Student Student, int Subjectid)
        {
            Subject? su = _subjectRepository.Read(Subjectid);
            su.EnrolledStudents.Remove(Student);
            _subjectRepository.Update(su.Id, su);
            Student.Subjects.Remove(su);
            _ownRepository.Update(Student.Id, Student);
        }

        public double GetStudentAverage(Student Student)
        {
            return _gradeRepository.ReadAll().Where(g => g.StudentId == Student.Id).Average(g => g.FinalGrade);
        }
    }
}
