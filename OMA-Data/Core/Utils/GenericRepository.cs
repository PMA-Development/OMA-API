using Microsoft.EntityFrameworkCore;
using OMA_Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.Core.Utils
{
    public class GenericRepository<T>(OMAContext context) : IGenericRepository<T> where T : class
    {
        protected OMAContext _context = context;
        internal DbSet<T> _dbContext = context.Set<T>();

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return (await _dbContext.FindAsync(id))!;
        }
        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return (await _dbContext.FindAsync(id))!;
        }
        public virtual IQueryable<T> GetAll()
        {
            return _dbContext;
        }

        public virtual async Task<bool> Add(T entity)
        {
            await _dbContext.AddAsync(entity);
            return true;
        }

        public virtual bool Update(T entity)
        {
            _dbContext.Update(entity);
            return true;
        }

        public virtual bool Delete(T entity)
        {
            _dbContext.Remove(entity);
            return true;
        }

        public bool DeleteRange(IEnumerable<T> entites)
        {
            _dbContext.RemoveRange(entites);
            return true;
        }

    }
}
