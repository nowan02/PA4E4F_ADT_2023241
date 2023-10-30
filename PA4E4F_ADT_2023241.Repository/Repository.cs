using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using PA4E4F_ADT_2023241.Models;

namespace PA4E4F_ADT_2023241.Repository
{
    public interface IRepository<T> where T : class
    {
        void Create(T entity);
        T Read(int id);
        void Update(int id, T entity);
        void Delete(int id);
        IEnumerable<T> ReadAll();
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

        public IEnumerable<T> ReadAll()
        {
            return _entities.AsEnumerable();
        }

        public void Delete(int id)
        {
            _entities.Remove(Read(id));
            _dbContext.SaveChanges();
        }
        public T Read(int id)
        {
            return _entities.FirstOrDefault(x => x.Id == id);
        }
        public void Update(int id, T entity)
        {
            Delete(id);
            _entities.Add(entity);
        }
    }

    public class StudentRepository : Repository<Student>
    {
        public StudentRepository(DbContext dbContext) : base(dbContext) { }
    }

    public class TeacherRepository : Repository<Teacher>
    {
        public TeacherRepository(DbContext dbContext) : base(dbContext) { }
    }

    public class SubjectRepository : Repository<Subject>
    {
        public SubjectRepository(DbContext dbContext) : base(dbContext) { }
    }

    public class GradeRepository : Repository<Grade>
    {
        public GradeRepository(DbContext dbContext) : base(dbContext) { }
    }
}
