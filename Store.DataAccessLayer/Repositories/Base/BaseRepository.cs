using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private DbContext _context;
        private DbSet<TEntity> _dbSet;

        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task CreateAsync(TEntity item)
        {
         await _dbSet.AddAsync(item);
         await _context.SaveChangesAsync();
        }

        public async Task<TEntity> FindByIdAsync(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)////////////////
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task RemoveAsync(TEntity item)
        {
           _dbSet.Remove(item);
           await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity item)
        {
           _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
