using Store.BusinessLogicLayer.Models.OrderItem;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises.Interfaces
{
    public interface IOrderItemService
    {
        public Task CreateAsync(OrderItemModel model);
        public Task<List<OrderItemModel>> GetAll();
        public Task UpdateAsync(OrderItemModel model);
        public Task RemoveAsync(OrderItemModel model);
    }
}
