
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Options;
using Store.DataAccessLayer.Dapper.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Models.FiltrationModels;
using Store.Sharing.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Dapper.Repositiories
{
    public class OrderRepositoryDapper : IOrderRepositoryDapper
    {
        private readonly ConnectionStringConfig _options;
        public OrderRepositoryDapper(IOptions<ConnectionStringConfig> options)
        {
            _options = options.Value;
        }

        public async Task CreateAsync(Order order)
        {
            using (IDbConnection db = new SqlConnection(_options.DefaultConnection))
            {
                await db.InsertAsync<Order>(order);
            }
        }
        public Task<(IEnumerable<Order>, int)> GetAsync(OrderFiltrationModelDAL model)
        {
            throw new System.NotImplementedException();
        }

        public Task<Order> GetByIdAsync(long id)
        {
            throw new System.NotImplementedException();
        }

        public async Task UpdateAsync(Order order)
        {
            using (IDbConnection db = new SqlConnection(_options.DefaultConnection))
            {
                await db.UpdateAsync<Order>(order);
            }
        }
    }
}
