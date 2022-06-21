using Store.DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Dapper.Interfaces
{
    public interface IOrderItemRepositoryDapper
    {
        public Task CreateAsync(List<OrderItem> orderItems);
    }
}
