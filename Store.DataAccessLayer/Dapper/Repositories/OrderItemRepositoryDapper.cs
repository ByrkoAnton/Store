using Microsoft.Extensions.Options;
using Store.DataAccessLayer.Dapper.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.Sharing.Configuration;

namespace Store.DataAccessLayer.Dapper.Repositories//TODO wrong selling+++
{
    public class OrderItemRepositoryDapper : DapperBaseRepository<OrderItem>, IOrderItemRepositoryDapper
    {
        public OrderItemRepositoryDapper(IOptions<ConnectionStringConfig> options) : base(options)
        {
        }
    }
}
