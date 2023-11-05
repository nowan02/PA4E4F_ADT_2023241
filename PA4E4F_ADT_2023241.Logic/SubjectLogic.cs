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

        public IEnumerable<Student> GetStudentsOnSubject(Subject Subject)
        {
            return _studentRepository.ReadAll().Where(s => s.Id == Subject.Id);
        }
        public double GetGradeAverage(Subject Subject)
        {
            return _gradeRepository.ReadAll().Where(g => g.SubjectId == Subject.Id).Average(g => g.FinalGrade);
        }
        
        public Teacher GetSubjectTeacher(Subject Subject)
        {
            return _teacherRepository.Read(_ownRepository.Read(Subject.Id).TeacherId);
        }

        public IEnumerable<Subject> GetSubjectsWithNoTeacher()
        {
            return _ownRepository.ReadAll().Where(su => su.SubjectTeacher == null);
        }
    }
}
