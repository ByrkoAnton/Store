using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Options;
using Store.DataAccessLayer.Dapper.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.Sharing.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Dapper.Repositiories
{
    public class PaymentRepositoryDapper : IPaymentRepositoryDapper
    {
        private readonly ConnectionStringConfig _options;
        public PaymentRepositoryDapper(IOptions<ConnectionStringConfig> options)
        {
            _options = options.Value;
        }
        public async Task CreateAsync(Payment payment)
        {
            using (IDbConnection db = new SqlConnection(_options.DefaultConnection))
            {
                await db.InsertAsync<Payment>(payment);
            }
        }  
    }
}
