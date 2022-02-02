using Microsoft.Extensions.Options;
using Store.DataAccessLayer.Dapper.Interfaces;
using Store.Sharing.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace Store.DataAccessLayer.Dapper.Repositiories
{
    public class DapperBaseRepository<TEntity> : IDapperBaseRepository<TEntity> where TEntity : class
    {

        private readonly ConnectionStringConfig _options;
        public DapperBaseRepository(IOptions<ConnectionStringConfig> options)
        {
            _options = options.Value;
        }
        
        public Task CreateAsync(TEntity item)
        {
            using (IDbConnection db = new SqlConnection(_options.DefaultConnection))
            {

            }
                throw new NotImplementedException();
        }

        public Task CreateAsync(List<TEntity> item)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(TEntity item)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TEntity item)
        {
            throw new NotImplementedException();
        }
    }
}
