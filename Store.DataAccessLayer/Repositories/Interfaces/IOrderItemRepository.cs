using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Base;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.Interfaces
{
   public interface IOrderItemRepository : IBaseRepository<OrderItem>
    {
        public Task<OrderItem> GetByIdAsync(long id);
    }
}
