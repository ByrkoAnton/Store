using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.AppContext;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Base;
using Store.DataAccessLayer.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories
{
    public class OrderItemRepository : BaseRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(ApplicationContext context) : base(context)
        {
        }
        public async Task<OrderItem> GetByIdAsync(long id)
        {
            var orderItem = await _dbSet.AsNoTracking().FirstOrDefaultAsync(Order => Order.Id == id);//TODO wrong naming+++
            return orderItem;
        }
    }
}
