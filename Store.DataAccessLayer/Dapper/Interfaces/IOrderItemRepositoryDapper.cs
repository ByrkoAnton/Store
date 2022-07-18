using Store.DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Dapper.Interfaces
{
    public interface IOrderItemRepositoryDapper //TODO base interface?
    {
        public Task CreateAsync(List<OrderItem> orderItems);
    }
}
