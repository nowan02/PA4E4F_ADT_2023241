using PA4E4F_ADT_2023241.Models;
using PA4E4F_ADT_2023241.Repository;

namespace PA4E4F_ADT_2023241.Logic
{
    public class TeacherLogic : Logic<Teacher>, ILogic<Teacher>
    {
        private ITeacherRepository _ownRepository;
        private ISubjectRepository _subjectRepository;
        private IGradeRepository _gradeRepository;
        private IStudentRepository _studentRepository;

        public TeacherLogic(ITeacherRepository ownRepository, ISubjectRepository subjectRepository, IGradeRepository gradeRepository, IStudentRepository studentRepository) : base(ownRepository)
        {
            _subjectRepository = subjectRepository;
            _gradeRepository = gradeRepository;
            _studentRepository = studentRepository;
        }

        public override void Create(Teacher Teacher)
        {
            if (Teacher.Name == null || Teacher.Name.Length == 0)
            {
                throw new ArgumentException("Teacher name was empty!");
            }

            if (Read(t => t.Id == Teacher.Id) != null) throw new ArgumentException("Teacher Id was not unique!");

            _ownRepository.Create(Teacher);
        }

        public IEnumerable<Grade> GetGradesOfTeacher(Teacher Teacher)
        {
            return _gradeRepository.ReadAll().Where(g => g.TeacherId == Teacher.Id).AsEnumerable();
        }
        public IEnumerable<Subject> GetTaughtSubjects(Teacher Teacher)
        {
            return _subjectRepository.ReadAll().Where(su => su.TeacherId == Teacher.Id).AsEnumerable();
        }
        public void GradeStudentInSubject(Teacher Teacher, Student Student, Subject Subject, int Grade)
        {
            if (Grade < 0 || Grade > 5) throw new ArgumentException("Grade was not in range of 0 - 5!");

            Grade _g1;

            Grade? _g2 = _gradeRepository.ReadAll().FirstOrDefault(g => g.SubjectId == Subject.Id);

            if (_g2 != null)
            {
                _g1 = _g2;
            }
            else
            {
                _g1 = new Grade {
                    FinalGrade = Grade,
                    StudentId = Student.Id,
                    TeacherId = Teacher.Id,
                    SubjectId = Subject.Id
                };

                _gradeRepository.Create(_g1);
            }

            Student.Grades.Add(_g1);

            _studentRepository.Update(Student.Id, Student);
        }
    }
}
