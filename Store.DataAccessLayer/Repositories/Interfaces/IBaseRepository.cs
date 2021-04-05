using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.Base
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        public Task CreateAsync(TEntity item);
        public Task<TEntity> GetByIdAsync(Expression<Func<TEntity, bool>> predicate);
        public Task<IEnumerable<TEntity>> GetAllAsync();
        public Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);
        public Task RemoveAsync(TEntity item);
        public Task UpdateAsync(TEntity item);
    }
}
