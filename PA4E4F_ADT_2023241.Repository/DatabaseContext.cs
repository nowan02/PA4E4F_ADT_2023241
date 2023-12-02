using Microsoft.EntityFrameworkCore;
using PA4E4F_ADT_2023241.Models;

namespace PA4E4F_ADT_2023241.Repository
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Grade> Grades { get; set; }

        public DatabaseContext() 
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("SchoolDb").UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(mb =>
            {
                mb.HasKey(s => s.Id);
                mb.Property(s => s.Id).ValueGeneratedOnAdd();

                mb.HasMany(s => s.Subjects).WithMany(sb => sb.EnrolledStudents);
                mb.HasMany(s => s.Grades).WithOne(g => g.Student).HasForeignKey(g => g.StudentId);

                mb.HasData(
                    new Student
                    {
                        Name = "Amal Mcguire",
                        Id = 100
                    },
                    new Student
                    {
                        Name = "Shellie Benson",
                        Id = 101
                    },
                    new Student
                    {
                        Name = "Ulric Yates",
                        Id = 102
                    },
                    new Student
                    {
                        Name = "Montana Fletcher",
                        Id = 103
                    },
                    new Student
                    {
                        Name = "Tanner Beach",
                        Id = 104
                    },
                    new Student
                    {
                        Name = "Nina Albert",
                        Id = 105
                    },
                    new Student
                    {
                        Name = "Macon Gilliam",
                        Id = 106
                    },
                    new Student
                    {
                        Name = "May Fields",
                        Id = 107
                    },
                    new Student
                    {
                        Name = "Tyler Conrad",
                        Id = 108
                    },
                    new Student
                    {
                        Name = "Pandora Knight",
                        Id = 109
                    }
                );
            });

            modelBuilder.Entity<Teacher>(mb =>
            {
                mb.HasKey(t => t.Id);
                mb.Property(t => t.Id).ValueGeneratedOnAdd();

                mb.HasData(
                    new Teacher
                    {
                        Name = "Tamekah Finch",
                        Id = 1000
                    },
                    new Teacher
                    {
                        Name = "Clio Allison",
                        Id = 1010
                    },
                    new Teacher
                    {
                        Name = "Conan Campbell",
                        Id = 1020
                    },
                    new Teacher
                    {
                        Name = "Laurel Pearson",
                        Id = 1030
                    },
                    new Teacher
                    {
                        Name = "Medge Mcmillan",
                        Id = 1040
                    },
                    new Teacher
                    {
                        Name = "Laurel Merrill",
                        Id = 1050
                    },
                    new Teacher
                    {
                        Name = "Jerome Norman",
                        Id = 1060
                    },
                    new Teacher
                    {
                        Name = "Amal Delacruz",
                        Id = 1070
                    },
                    new Teacher
                    {
                        Name = "Keelie Rodgers",
                        Id = 1080
                    },
                    new Teacher 
                    {
                        Name = "Azalia Daugherty",
                        Id = 1090
                    }
                );
            });

            modelBuilder.Entity<Subject>(mb =>
            {
                mb.HasKey(g => g.Id);
                mb.Property(g => g.Id).ValueGeneratedOnAdd();
                mb.HasOne(su => su.SubjectTeacher).WithMany(t => t.TaughtSubjects).HasForeignKey(t => t.TeacherId);
                mb.HasData(
                    new Subject
                    {
                        Name = "Calculus",
                        Id = 11,
                        TeacherId = 1000
                    },
                    new Subject
                    {
                        Name = "Discrete Mathematics",
                        Id = 12,
                        TeacherId = 1010
                    },
                    new Subject
                    {
                        Name = "Basics of Information Systems",
                        Id = 13,
                        TeacherId = 1020
                    },
                    new Subject
                    {
                        Name = "Cloud Technology and Infrastructure",
                        Id = 14,
                        TeacherId = 1030
                    },
                    new Subject
                    {
                        Name = "IT Security",
                        Id = 15,
                        TeacherId = 1040
                    },
                    new Subject
                    {
                        Name = "Databases",
                        Id = 16,
                        TeacherId = 1050
                    },
                    new Subject
                    {
                        Name = "Business Management",
                        Id = 17,
                        TeacherId = 1060
                    },
                    new Subject
                    {
                        Name = "PLC Programming",
                        Id = 18,
                        TeacherId = 1070
                    },
                    new Subject
                    {
                        Name = "Computer Architectures",
                        Id = 19,
                        TeacherId = 1080
                    },
                    new Subject
                    {
                        Name = "GNU/Linux",
                        Id = 20,
                        TeacherId = 1090
                    }
                ); ;
            });

            modelBuilder.Entity<Grade>(mb =>
            {
                mb.HasKey(g => g.Id);
                mb.Property(g => g.Id).ValueGeneratedOnAdd();
                mb.HasOne(g => g.Teacher).WithMany(t => t.GivenGrades).HasForeignKey(t => t.TeacherId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}