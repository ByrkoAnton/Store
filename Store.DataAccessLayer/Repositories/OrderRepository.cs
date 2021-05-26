using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.AppContext;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Models.FiltrationModels;
using Store.DataAccessLayer.Repositories.Base;
using Store.DataAccessLayer.Repositories.Interfaces;
using Store.Sharing.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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
            var result = await _dbSet.FirstOrDefaultAsync(Order => Order.Id == id);
            return result;
        }

        public async Task<(IEnumerable<Order>, int)> GetAsync(OrderFiltrationModelDAL model)
        {
            var order = await _dbSet.Where(o => o.UserId == model.UserId || model.UserId == null)
               .Where(o=> model.EditionId==null || o.OrderItems.Any(i => i.PrintingEditionId == model.EditionId))
               .Where(o => string.IsNullOrEmpty(model.Discription)
               || EF.Functions.Like(o.Discription, $"%{model.Discription}%"))
               .Where(o => model.Status == null || o.Status == model.Status)
                .OrderBy($"{model.PropertyForSort} " +
                $"{(model.IsAscending ? Constants.SortingParams.SORT_ASC_DIRECTION : Constants.SortingParams.SORT_DESC_DIRECTION)}")
                .Skip((model.CurrentPage - Constants.PaginationParams.STARTS_ONE) * model.PageSize).Take(model.PageSize).ToListAsync();

            int count = await _dbSet.Where(o => o.UserId == model.UserId || model.UserId == null)
               .Where(o => model.EditionId == null || o.OrderItems.Any(i => i.PrintingEditionId == model.EditionId))
               .Where(o => string.IsNullOrEmpty(model.Discription)
               || EF.Functions.Like(o.Discription, $"%{model.Discription}%"))
               .Where(o => model.Status == null || o.Status == model.Status).CountAsync();

            var ordersWithCount = (editions: order, count: count);

            return ordersWithCount;
        }
    }
}
