using Store.BusinessLogicLayer.Models.Orders;
using Store.BusinessLogicLayer.Models.PaginationsModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises.Interfaces
{
    public interface IOrderService
    {
        public Task<List<OrderModel>> GetAllAsync();
        public Task<OrderModel> GetByIdAsync(long id);
        public Task UpdateAsync(OrderModel model);
        public Task RemoveAsync(OrderModel model);
        public Task<NavigationModelBase<OrderModel>> GetAsync(OrderFiltrationModel model);
    }
}
