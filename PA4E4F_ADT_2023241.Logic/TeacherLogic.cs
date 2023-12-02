using PA4E4F_ADT_2023241.Models;
using PA4E4F_ADT_2023241.Repository;

namespace PA4E4F_ADT_2023241.Logic
{
    public class TeacherLogic : Logic<Teacher>, ITeacherLogic
    {
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

            if (_ownRepository.Read(Teacher.Id) != null) throw new ArgumentException("Teacher Id was not unique!");

            _ownRepository.Create(Teacher);
        }

        public IEnumerable<Grade> GetGradesOfTeacher(int TeacherId)
        {
            return _gradeRepository.ReadAll().Where(g => g.TeacherId == TeacherId);
        }
        public IEnumerable<Subject> GetTaughtSubjects(int TeacherId)
        {
            return _subjectRepository.ReadAll().Where(su => su.TeacherId == TeacherId);
        }
        public void GradeStudentInSubject(int TeacherId, int StudentId, int SubjectId, int Grade, string Desc)
        {
            Teacher? t = _ownRepository.Read(TeacherId);
            Student? s = _studentRepository.Read(StudentId);
            Subject? su = _subjectRepository.Read(SubjectId);

            if (t == null) throw new NullReferenceException("Teacher with specified ID does not exist");
            if (s == null) throw new NullReferenceException("Student with specified ID does not exist");
            if (su == null) throw new NullReferenceException("Subject with specified ID does not exist");

            if (Grade < 0 || Grade > 5) throw new ArgumentException("Grade was not in range of 0 - 5!");
            if (su.TeacherId != TeacherId) throw new ArgumentException("This teacher cannot grade, as they are not the subject teacher!");
            if (!su.EnrolledStudents.Contains(s)) throw new ArgumentException("Student is not enrolled in this subject!");

            Grade _g1;

            Grade? _g2 = _gradeRepository.ReadAll().First(g => g.SubjectId == SubjectId && g.TeacherId == TeacherId && g.StudentId == StudentId);

            if (_g2 != null)
            {
                _g1 = new Grade
                {
                    StudentId = _g2.StudentId,
                    SubjectId = _g2.SubjectId,
                    TeacherId = _g2.TeacherId,
                    FinalGrade = Grade,
                    Name = Desc,
                };

                if (Desc.Length == 0) _g1.Name = _g2.Name;

                _gradeRepository.Update(_g2.Id, _g1);
            }
            else
            {
                _g1 = new Grade {
                    FinalGrade = Grade,
                    StudentId = StudentId,
                    TeacherId = TeacherId,
                    SubjectId = SubjectId,
                    Name = Desc
                };

                _gradeRepository.Create(_g1);
            }

            _studentRepository.Update(StudentId, s);
        }
    }
}
