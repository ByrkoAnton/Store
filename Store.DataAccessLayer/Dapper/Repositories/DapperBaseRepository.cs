using Store.DataAccessLayer.Dapper.Interfaces;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Store.Sharing.Configuration;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace Store.DataAccessLayer.Dapper.Repositories
{
    public class DapperBaseRepository<TEntity> : IDapperBaseRepository<TEntity> where TEntity : class
    {

        protected readonly ConnectionStringConfig _options;
        public DapperBaseRepository(IOptions<ConnectionStringConfig> options)
        {
            _options = options.Value;
        }
        public virtual async Task CreateAsync(TEntity item)
        {
            using IDbConnection db = new SqlConnection(_options.DefaultConnection);
            await db.InsertAsync(item);
        }

        public async virtual Task CreateAsync(IEnumerable<TEntity> items)
        {
            using IDbConnection db = new SqlConnection(_options.DefaultConnection);
            await db.InsertAsync(items);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            using IDbConnection db = new SqlConnection(_options.DefaultConnection);
            return (await db.GetAllAsync<TEntity>()).ToList();
        }

        public virtual async Task RemoveAsync(TEntity item)
        {
            using IDbConnection db = new SqlConnection(_options.DefaultConnection);
            await db.DeleteAsync(item);
        }

        public virtual async Task UpdateAsync(TEntity item)
        {
            using IDbConnection db = new SqlConnection(_options.DefaultConnection);
            await db.UpdateAsync(item);
        }
    }
}
