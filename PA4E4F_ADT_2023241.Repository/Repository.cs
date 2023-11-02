using Microsoft.EntityFrameworkCore;
using PA4E4F_ADT_2023241.Models;

namespace PA4E4F_ADT_2023241.Repository
{
    public interface IRepository<T> where T : class
    {
        void Create(T entity);
        T? Read(int id);
        void Update(int id, T entity);
        void Delete(int id);
        IQueryable<T> ReadAll();
    }

    public class Repository<T> : IRepository<T> where T : class, IModelWithID
    {
        protected DbContext _dbContext;
        protected DbSet<T> _entities;

        public Repository(DbContext dbContext) 
        { 
            _dbContext = dbContext;
            _entities = _dbContext.Set<T>();
        }
        public void Create(T entity)
        {
            _entities.Add(entity);
            _dbContext.SaveChanges();
        }

        public IQueryable<T> ReadAll()
        {
            return _entities;
        }

        public void Delete(int id)
        {
            _entities.Remove(Read(id));
            _dbContext.SaveChanges();
        }
        public T? Read(int id)
        {
            return _entities.FirstOrDefault(x => x.Id == id);
        }
        public void Update(int id, T entity)
        {
            Delete(id);
            _entities.Add(entity);
        }
    }

    public interface IStudentRepository : IRepository<Student>
    {
    }

    public interface ITeacherRepository : IRepository<Teacher>
    {
    }

    public interface ISubjectRepository : IRepository<Subject>
    { 
    }

    public interface IGradeRepository : IRepository<Grade>
    {
    }

    public class StudentRepository : Repository<Student>, IStudentRepository
    { 
        public StudentRepository(DbContext dbContext) : base(dbContext) { }
    }

    public class TeacherRepository : Repository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(DbContext dbContext) : base(dbContext) { }
    }

    public class SubjectRepository : Repository<Subject>, ISubjectRepository
    {
        public SubjectRepository(DbContext dbContext) : base(dbContext) { }
    }

    public class GradeRepository : Repository<Grade>, IGradeRepository
    {
        public GradeRepository(DbContext dbContext) : base(dbContext) { }
    }
}
