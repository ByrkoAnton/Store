using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Options;
using Store.DataAccessLayer.Dapper.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.Sharing.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Dapper.Repositiories
{
    public class OrderItemRepositoryDapper : IOrderItemRepositoryDapper
    {
        private readonly ConnectionStringConfig _options;
        public OrderItemRepositoryDapper(IOptions<ConnectionStringConfig> options)
        {
            _options = options.Value;
        }
        public async Task CreateAsync(List<OrderItem> orderItems)
        {
            using (IDbConnection db = new SqlConnection(_options.DefaultConnection))
            {
                await db.InsertAsync<List<OrderItem>>(orderItems);
            } 
        }

        public Task<OrderItem> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
