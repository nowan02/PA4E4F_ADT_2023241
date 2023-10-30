using PA4E4F_ADT_2023241.Models;
using PA4E4F_ADT_2023241.Repository;

namespace PA4E4F_ADT_2023241.Logic
{
    public class SubjectLogic : Logic<Subject>, ISubjectLogic
    {
        private Repository<Subject> _ownRepository;
        private Repository<Teacher> _teacherRepository;
        private Repository<Student> _studentRepository;
        private Repository<Grade> _gradeRepository;

        public SubjectLogic(Repository<Subject> ownRepository, Repository<Teacher> teacherRepositoy, Repository<Student> studentRepository, Repository<Grade> gradeRepository) : base(ownRepository)
        {
            _teacherRepository = teacherRepositoy;
            _studentRepository = studentRepository;
            _gradeRepository = gradeRepository;
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
    }
}
