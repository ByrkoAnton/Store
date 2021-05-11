using Store.BusinessLogicLayer.Models.Orders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises.Interfaces
{
    public interface IOrderService
    {
        public Task CreateAsync(OrderModel model);
        public Task<List<OrderModel>> GetAll();
        public Task UpdateAsync(OrderModel model);
        public Task RemoveAsync(OrderModel model);
     
    }
}
