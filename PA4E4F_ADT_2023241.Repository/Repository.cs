using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PA4E4F_ADT_2023241.Models;

namespace PA4E4F_ADT_2023241.Repository
{
    public interface IRepository<T> where T : class
    {
        void Create(T entity);
        T Read(int id);
        void Update(int id, T entity);
        void Delete(int id);
        IQueryable<T> ReadAll();
    }

    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext _dbContext;

        public Repository(DbContext dbContext) { _dbContext = dbContext; }
        public void Create(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public IQueryable<T> ReadAll()
        {
            return _dbContext.Set<T>();
        }

        public abstract void Delete(int id);
        public abstract T Read(int id);
        public abstract void Update(int id, T entity);
    }
}
