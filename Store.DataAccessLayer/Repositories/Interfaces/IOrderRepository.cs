using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Base;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.Interfaces
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        public Task<Order> GetByIdAsync(long id);
    }
}
