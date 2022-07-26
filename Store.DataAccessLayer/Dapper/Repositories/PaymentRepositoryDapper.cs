using Microsoft.Extensions.Options;
using Store.DataAccessLayer.Dapper.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.Sharing.Configuration;

namespace Store.DataAccessLayer.Dapper.Repositories //TODO wrong spelling+++
{
    public class PaymentRepositoryDapper : DapperBaseRepository<Payment>,  IPaymentRepositoryDapper
    {
        public PaymentRepositoryDapper(IOptions<ConnectionStringConfig> options):base(options)
        {
        }  
    }
}
