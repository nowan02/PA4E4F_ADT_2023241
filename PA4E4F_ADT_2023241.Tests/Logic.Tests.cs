using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Moq;
using PA4E4F_ADT_2023241.Logic;
using PA4E4F_ADT_2023241.Models;
using PA4E4F_ADT_2023241.Repository;
using System.Security.Cryptography.X509Certificates;

namespace PA4E4F_ADT_2023241.Tests
{
    public class LogicTests
    {
        List<Subject> subjects;

        IStudentLogic MockedStudentLogic;
        ITeacherLogic MockedTeacherLogic;
        ISubjectLogic MockedSubjectLogic;

        Mock<IStudentRepository> MockedStudentRepository;
        Mock<IGradeRepository> MockedGradeRepository;
        Mock<ISubjectRepository> MockedSubjectRepository;
        Mock<ITeacherRepository> MockedTeacherRepository;

        [SetUp]
        public void Setup()
        {
            MockedStudentRepository = new Mock<IStudentRepository>();
            MockedGradeRepository = new Mock<IGradeRepository>();
            MockedSubjectRepository = new Mock<ISubjectRepository>();
            MockedTeacherRepository = new Mock<ITeacherRepository>();

            MockedStudentLogic = new StudentLogic(MockedStudentRepository.Object, MockedGradeRepository.Object, MockedSubjectRepository.Object);
            MockedTeacherLogic = new TeacherLogic(MockedTeacherRepository.Object, MockedSubjectRepository.Object, MockedGradeRepository.Object, MockedStudentRepository.Object);
            MockedSubjectLogic = new SubjectLogic(MockedSubjectRepository.Object, MockedTeacherRepository.Object, MockedStudentRepository.Object, MockedGradeRepository.Object);

            MockedStudentRepository.Setup(repo => repo.Create(It.IsAny<Student>()));
            MockedTeacherRepository.Setup(repo => repo.Create(It.IsAny<Teacher>()));

            subjects = new List<Subject>();
            for(int i = 0; i < 10; i++)
            {
                subjects.Add(new Subject() { Id = i, Name = "subject" + i });
            }

            subjects[3].SubjectTeacher = new Teacher { Name = "teacher3", Id = 3 };
            subjects[5].SubjectTeacher = new Teacher { Name = "teacher5", Id = 5 };
            subjects[7].SubjectTeacher = new Teacher { Name = "teacher7", Id = 7 };
        }

        // Create test 3x
        [Test]
        public void NamelessStudentThrowsArgumentException()
        {
            Student su = new Student
            {
                Name = "",
                Id = 999,
            };

            Assert.Throws<ArgumentException>(() => MockedStudentLogic.Create(su));

            MockedStudentRepository.Verify();
        }

        [Test]
        public void StudentWithDuplicateIdThrowsArgumentException()
        {
            Student su = new Student
            {
                Name = "Test",
                Id = 999
            };

            MockedStudentLogic.Create(su);
            // Setup return
            MockedStudentRepository.Setup(repo => repo.Read(su.Id)).Returns(su);

            Student su2 = new Student
            {
                Name = "OtherTest",
                Id = 999
            };

            Assert.Throws<ArgumentException>(() => MockedStudentLogic.Create(su2));

            MockedGradeRepository.Verify();
        }

        [Test]
        public void NamelessTeacherThrowsArgumentException()
        {
            Teacher t = new Teacher
            {
                Name = "",
                Id = 999
            };

            Assert.Throws<ArgumentException>(() => MockedTeacherLogic.Create(t));

            MockedStudentRepository.Verify();
        }

        // 5x Non-crud

        [Test]
        public void SubjectsWithNoTeacherReturnOnlyNull()
        {
            MockedSubjectRepository.Setup(repo => repo.ReadAll()).Returns(subjects.AsQueryable());

            IEnumerable<Subject> returned = MockedSubjectLogic.GetSubjectsWithNoTeacher();

            foreach(Subject subject in returned)
            {
                if (subject.SubjectTeacher == null)
                {
                    continue;
                }
                else Assert.Fail();
            }

            Assert.Pass();
        }


        [Test]
        public void GradeStudentInSubjectThrowsArgumentExceptionWhenOutOfBounds([Values(-1, 6, 24, 9)] int grade)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                MockedTeacherLogic.GradeStudentInSubject(It.IsAny<Teacher>(), It.IsAny<Student>(), It.IsAny<Subject>(), grade);
            });
        }

        [Test]
        public void GradeStudentInSubjectOtherTeacherCantGrade()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                MockedTeacherLogic.GradeStudentInSubject(subjects[3].SubjectTeacher, It.IsAny<Student>(), subjects[5], 3);
            });
        }
    }
}