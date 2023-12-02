using PA4E4F_ADT_2023241.Models;
using PA4E4F_ADT_2023241.Repository;

namespace PA4E4F_ADT_2023241.Logic
{
    public class SubjectLogic : Logic<Subject>, ISubjectLogic
    {
        private ITeacherRepository _teacherRepository;
        private IStudentRepository _studentRepository;
        private IGradeRepository _gradeRepository;

        public SubjectLogic(ISubjectRepository ownRepository, ITeacherRepository teacherRepositoy, IStudentRepository studentRepository, IGradeRepository gradeRepository) : base(ownRepository)
        {
            _teacherRepository = teacherRepositoy;
            _studentRepository = studentRepository;
            _gradeRepository = gradeRepository;
        }

        public override void Create(Subject Subject)
        {
            if (_ownRepository.Read(Subject.Id) != null) throw new ArgumentException("Subject Id was not unique!");

            if (Subject.Name == null || Subject.Name.Length == 0) throw new ArgumentException("Subject name is required!");

            _ownRepository.Create(Subject);
        }

        public IEnumerable<Student> GetStudentsOnSubject(int SubjectId)
        {
            Subject? su = _ownRepository.Read(SubjectId);
            return _studentRepository.ReadAll().Where(s => s.Subjects.Contains(su));
        }
        public double GetGradeAverage(int SubjectId)
        {
            return _gradeRepository.ReadAll().Where(g => g.SubjectId == SubjectId).Average(g => g.FinalGrade);
        }
        
        public Teacher? GetSubjectTeacher(int SubjectId)
        {
            try
            {
                return _teacherRepository.Read(_ownRepository.Read(SubjectId).TeacherId);
            }
            catch(NullReferenceException ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Subject> GetSubjectsWithNoTeacher()
        {
            return _ownRepository.ReadAll().Where(su => su.TeacherId == 0);
        }
    }
}
