using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Dapper.Interfaces
{
    public interface IDapperBaseRepository<TEntity> where TEntity : class
    {
        public Task CreateAsync(TEntity item);
        public Task CreateAsync(List<TEntity> item);
        public Task<IEnumerable<TEntity>> GetAllAsync();
        public Task RemoveAsync(TEntity item);
        public Task UpdateAsync(TEntity item);
        public Task SaveChangesAsync();
    }
}
