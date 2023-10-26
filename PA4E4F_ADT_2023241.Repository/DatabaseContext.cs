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

            });

            modelBuilder.Entity<Teacher>(mb =>
            {
                mb.HasKey(t => t.Id);
                mb.Property(t => t.Id).ValueGeneratedOnAdd();

                mb.HasMany(t => t.GivenGrades).WithOne(g => g.Teacher).HasForeignKey(g => g.TeacherId);
                mb.HasMany(t => t.TaughtSubjects).WithOne(sb => sb.SubjectTeacher).HasForeignKey(sb => sb.TeacherId);
            });

            modelBuilder.Entity<Subject>(mb =>
            {
                mb.HasKey(g => g.Id);
                mb.Property(g => g.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Grade>(mb =>
            {
                mb.HasKey(g => g.Id);
                mb.Property(g => g.Id).ValueGeneratedOnAdd();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}