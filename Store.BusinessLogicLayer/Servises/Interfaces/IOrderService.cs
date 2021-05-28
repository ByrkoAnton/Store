using Store.BusinessLogicLayer.Models.Orders;
using Store.BusinessLogicLayer.Models.PaginationsModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises.Interfaces
{
    public interface IOrderService
    {
        public Task<List<OrderModel>> GetAll();
        public Task<OrderModel> GetById(long id);
        public Task UpdateAsync(OrderModel model);
        public Task RemoveAsync(OrderModel model);
        public Task<NavigationModel<OrderModel>> GetAsync(OrderFiltrationModel model);
    }
}
