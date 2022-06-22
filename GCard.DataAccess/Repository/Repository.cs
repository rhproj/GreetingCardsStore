using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GCard.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private DbSet<T> dbSet; 

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            dbSet = _dbContext.Set<T>();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProp = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProp != null)
            {
                foreach (var p in includeProp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(p);
                }
            }
            return query.ToList();
        }

        public T GetWithCondition(Expression<Func<T, bool>>? filter = null, string? includeProp = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProp != null)
            {
                foreach (var prop in includeProp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);
                }
            }
            return query.FirstOrDefault();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }

        public void DeleteRange(IEnumerable<T> rangeVals)
        {
            dbSet.RemoveRange(rangeVals);
            _dbContext.SaveChanges();
        }

        //public void Save() => _dbContext.SaveChanges();

        public void Update(T entity)
        {
            dbSet.Update(entity);
            _dbContext.SaveChanges();
        }
    }
}
