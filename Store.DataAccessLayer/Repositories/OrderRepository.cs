using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.AppContext;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Base;
using Store.DataAccessLayer.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationContext context) : base(context)
        {
        }
        public async Task<Order> GetByIdAsync(long id)
        {
            var result = await _dbSet.AsNoTracking().FirstOrDefaultAsync(Order => Order.Id == id);
            return result;
        }
    }
}
