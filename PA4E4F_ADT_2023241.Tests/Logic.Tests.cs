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
            for(int i = 0; i < 10; i++)
            {
                subjects.Add(new Subject { Id = i, Name = "subject" + i, EnrolledStudents = new List<Student>()});
                students.Add(new Student { Id = i, Name = "student" + i, Subjects = new List<Subject>()});
            }

            subjects[3].SubjectTeacher = new Teacher { Name = "teacher3", Id = 3 };

            subjects[3].EnrolledStudents = new List<Student> { students[1], students[5], students[8], students[9] };

            students[1].Subjects.Add(subjects[3]);
            students[5].Subjects.Add(subjects[3]);
            students[8].Subjects.Add(subjects[3]);
            students[9].Subjects.Add(subjects[3]);

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

        [Test]
        public void EnrollStudentInSubjectUpdates()
        {
            subjects[3].EnrolledStudents = new List<Student>();

            MockedStudentRepository.Setup(repo => repo.Update(students[7].Id, students[7]));
            MockedSubjectRepository.Setup(repo => repo.Update(subjects[3].Id, subjects[3]));
            MockedSubjectRepository.Setup(repo => repo.Read(subjects[3].Id)).Returns(subjects[3]);

            MockedStudentLogic.EnrollStudentInSubject(students[7], subjects[3].Id);

            if (students[7].Subjects.Contains(subjects[3]) && subjects[3].EnrolledStudents.Contains(students[7])) Assert.Pass();

            Assert.Fail();
        }

        [Test]
        public void GetStudentsInSubjectReturnsOnlyEnrolled()
        {
            MockedStudentRepository.Setup(repo => repo.ReadAll()).Returns(students.AsQueryable());

            foreach(Student s in MockedSubjectLogic.GetStudentsOnSubject(subjects[3]))
            {
                if (s.Subjects.Contains(subjects[3]))
                {
                    continue;
                }
                else Assert.Fail();
            }

            Assert.Pass();
        }
    }
}