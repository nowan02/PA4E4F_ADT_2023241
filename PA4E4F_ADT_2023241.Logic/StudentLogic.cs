using PA4E4F_ADT_2023241.Models;
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
        public IEnumerable<Grade> GetGradesOfStudent(int StudentId)
        {
            return _gradeRepository.ReadAll().Where(g => g.StudentId == StudentId).AsEnumerable();
        }

        public IEnumerable<Subject> GetSubjectsOfStudent(int id)
        {
            return _subjectRepository.ReadAll().Where(su => su.EnrolledStudents.Contains(Read(s => s.Id == id))).AsEnumerable();
        }

        public void EnrollStudentInSubject(int StudentId, int SubjectId) 
        {
            Student? s = _ownRepository.Read(StudentId);
            Subject? su = _subjectRepository.Read(SubjectId);

            if (s == null) throw new NullReferenceException("Student with specified ID doesn't exist");
            if (su == null) throw new NullReferenceException("Subject with specified ID doesn't exist");

            su.EnrolledStudents.Add(s);
            _subjectRepository.Update(SubjectId, su);
            s.Subjects.Add(su);
            _ownRepository.Update(StudentId, s);
        }

        public void DropStudentsSubject(int StudentId, int Subjectid)
        {
            Student? s = _ownRepository.Read(StudentId);
            Subject? su = _subjectRepository.Read(Subjectid);
            su.EnrolledStudents.Remove(s);
            _subjectRepository.Update(su.Id, su);
            s.Subjects.Remove(su);
            _ownRepository.Update(StudentId, s);
        }

        public double GetStudentAverage(int StudentId)
        {
            return _gradeRepository.ReadAll().Where(g => g.StudentId == StudentId).Average(g => g.FinalGrade);
        }
    }
}
