using PA4E4F_ADT_2023241.Logic;
using PA4E4F_ADT_2023241.Repository;

namespace PA4E4F_ADT_2023241.Endpoint
{
    public class LogicFactory
    {
        protected IStudentRepository _studentRepository;
        protected ITeacherRepository _teacherRepository;
        protected ISubjectRepository _subjectRepository;
        protected IGradeRepository _gradeRepository;
        public LogicFactory(IStudentRepository StudentRepository, ITeacherRepository TeacherRepository, ISubjectRepository SubjectRepository, IGradeRepository GradeRepository)
        {
            _studentRepository = StudentRepository;
            _teacherRepository = TeacherRepository;
            _subjectRepository = SubjectRepository;
            _gradeRepository = GradeRepository;
        }

        public StudentLogic CreateStudentLogic()
        {
            return new StudentLogic(_studentRepository, _gradeRepository, _subjectRepository);
        }

        public TeacherLogic CreateTeacherLogic()
        {
            return new TeacherLogic(_teacherRepository, _subjectRepository, _gradeRepository, _studentRepository);
        }

        public SubjectLogic CreateSubjectLogic()
        {
            return new SubjectLogic(_subjectRepository, _teacherRepository, _studentRepository, _gradeRepository);
        }
    }
}
