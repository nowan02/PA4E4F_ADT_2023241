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
        List<Student> students;

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
            students = new List<Student>();
            for(int i = 0; i < 5; i++)
            {
                subjects.Add(new Subject { Id = i, Name = "subject" + i, EnrolledStudents = new List<Student>()});
                students.Add(new Student { Id = i, Name = "student" + i, Subjects = new List<Subject>()});
            }

            subjects[1].SubjectTeacher = new Teacher { Name = "teacher3", Id = 3 };
            subjects[1].TeacherId = 3;

            subjects[1].EnrolledStudents = new List<Student> { students[1], students[2], students[3], students[4] };

            students[1].Subjects.Add(subjects[1]);
            students[2].Subjects.Add(subjects[1]);
            students[3].Subjects.Add(subjects[1]);
            students[4].Subjects.Add(subjects[1]);

            subjects[2].SubjectTeacher = new Teacher { Name = "teacher5", Id = 5 };
            subjects[2].TeacherId = 5;
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
            MockedStudentLogic.Create(students[4]);
            // Setup return
            MockedStudentRepository.Setup(repo => repo.Read(students[4].Id)).Returns(students[4]);

            Student su2 = new Student
            {
                Name = "OtherTest",
                Id = students[4].Id
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
            MockedSubjectRepository.Verify();
        }


        [Test]
        public void GradeStudentInSubjectThrowsArgumentExceptionWhenOutOfBounds([Values(-1, 6, 24, -9)] int grade)
        {
            MockedTeacherRepository.Setup(repo => repo.Read(subjects[1].TeacherId)).Returns(subjects[1].SubjectTeacher);
            MockedStudentRepository.Setup(repo => repo.Read(students[1].Id)).Returns(students[1]);
            MockedSubjectRepository.Setup(repo => repo.Read(subjects[1].Id)).Returns(subjects[1]);

            Assert.Throws<ArgumentException>(() =>
            {
                MockedTeacherLogic.GradeStudentInSubject(subjects[1].SubjectTeacher.Id, students[1].Id, subjects[1].Id, grade);
            });
        }

        [Test]
        public void GradeStudentInSubjectOtherTeacherCantGrade()
        {
            MockedTeacherRepository.Setup(repo => repo.Read(subjects[2].TeacherId)).Returns(subjects[2].SubjectTeacher);
            MockedStudentRepository.Setup(repo => repo.Read(students[1].Id)).Returns(students[1]);
            MockedSubjectRepository.Setup(repo => repo.Read(subjects[1].Id)).Returns(subjects[1]);

            Assert.Throws<ArgumentException>(() =>
            {
                MockedTeacherLogic.GradeStudentInSubject(subjects[2].TeacherId, students[1].Id, subjects[1].Id, 3);
            });
        }

        [Test]
        public void EnrollStudentInSubjectUpdates()
        {
            subjects[2].EnrolledStudents = new List<Student>();

            MockedStudentRepository.Setup(repo => repo.Update(students[3].Id, students[3]));
            MockedStudentRepository.Setup(repo => repo.Read(students[3].Id)).Returns(students[3]);

            MockedSubjectRepository.Setup(repo => repo.Update(subjects[2].Id, subjects[2]));
            MockedSubjectRepository.Setup(repo => repo.Read(subjects[2].Id)).Returns(subjects[2]);

            MockedStudentLogic.EnrollStudentInSubject(students[3].Id, subjects[2].Id);

            if (students[3].Subjects.Contains(subjects[2]) && subjects[2].EnrolledStudents.Contains(students[3])) Assert.Pass();

            Assert.Fail();
        }

        [Test]
        public void GetStudentsInSubjectReturnsOnlyEnrolled()
        {
            MockedStudentRepository.Setup(repo => repo.ReadAll()).Returns(students.AsQueryable());

            foreach(Student s in MockedSubjectLogic.GetStudentsOnSubject(subjects[1].Id))
            {
                if (s.Subjects.Contains(subjects[1]))
                {
                    continue;
                }
                else Assert.Fail();
            }

            Assert.Pass();
            MockedStudentRepository.Verify();
        }

        // 2x else


    }
}