using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Moq;
using PA4E4F_ADT_2023241.Logic;
using PA4E4F_ADT_2023241.Models;
using PA4E4F_ADT_2023241.Repository;

namespace PA4E4F_ADT_2023241.Tests
{
    public class LogicTests
    {
        IStudentLogic MockedStudentLogic;
        ITeacherLogic MockedTeacherLogic;

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
        }

        // Create test 3x
        [Test]
        public void Positive_NamelessStudentThrowsArgumentException()
        {
            MockedStudentRepository.Setup(repo => repo.Create(It.IsAny<Student>()));
            Student su = new Student
            {
                Name = "",
                Id = 999,
            };

            Assert.Throws<ArgumentException>(() => MockedStudentLogic.Create(su));

            MockedStudentRepository.Verify();
        }

        [Test]
        public void Positive_StudentWithDuplicateIdThrowsArgumentException()
        {
            MockedStudentRepository.Setup(repo => repo.Create(It.IsAny<Student>()));

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
        public void Positive_NamelessTeacherThrowsArgumentException()
        {
            MockedTeacherRepository.Setup(repo => repo.Create(It.IsAny<Teacher>()));
            Teacher t = new Teacher
            {
                Name = "",
                Id = 999
            };

            Assert.Throws<ArgumentException>(() => MockedTeacherLogic.Create(t));

            MockedStudentRepository.Verify();
        }

        // Non-crud
    }
}