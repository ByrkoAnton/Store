using Store.BusinessLogicLayer.Models.Orders;
using Store.BusinessLogicLayer.Models.PaginationsModels;
using System.Collections.Generic;//TODO unused directives
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises.Interfaces//TODO wrong spelling
{
    public interface IOrderService
    {
       // public Task<List<OrderModel>> GetAllAsync();//TODO legacy code should be removed
        public Task<OrderModel> GetByIdAsync(long id);
        //public Task UpdateAsync(OrderModel model);//TODO legacy code should be removed
        public Task RemoveAsync(long id);
        public Task<NavigationModelBase<OrderModel>> GetAsync(OrderFiltrationModel model);
        public Task<OrderDetailsModel> GetOrderDetails(long id);
    }
}
