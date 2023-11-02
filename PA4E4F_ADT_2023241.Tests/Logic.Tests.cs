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
        Mock<IStudentRepository> MockedStudentRepository;
        Mock<IGradeRepository> MockedGradeRepository;
        Mock<ISubjectRepository> MockedSubjectRepository;
        [SetUp]
        public void Setup()
        {
            MockedStudentRepository = new Mock<IStudentRepository>();
            MockedGradeRepository = new Mock<IGradeRepository>();
            MockedSubjectRepository = new Mock<ISubjectRepository>();
            MockedStudentLogic = new StudentLogic(MockedStudentRepository.Object, MockedGradeRepository.Object, MockedSubjectRepository.Object);
        }

        [Test]
        public void Positive_NamelessStudentThrowsArgumentException()
        {
            MockedStudentRepository.Setup(repo => repo.Create(It.IsAny<Student>()));
            Student su = new Student
            {
                Name = "",
                Id = 999,
            };

            try
            {
                MockedStudentLogic.Create(su);
            }
            catch(Exception ex)
            {
                Assert.That(ex is ArgumentException);
            }

            MockedStudentRepository.Verify();
        }
    }
}