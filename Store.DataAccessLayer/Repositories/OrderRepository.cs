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
            var order = await _dbSet.FirstOrDefaultAsync(o => o.Id == id);//TODO wrong naming+++
            return order;
        }

        public async Task<(IEnumerable<Order>, int)> GetAsync(OrderFiltrationModelDAL model)
        {
            var orders = await _dbSet
               .Where(o => model.UserId == null || o.UserId == model.UserId)  
               .Where(o => string.IsNullOrEmpty(model.Description)
               || EF.Functions.Like(o.Description, $"%{model.Description}%"))
               .Where(o => model.Status == null || o.Status == model.Status)
                .OrderBy($"{model.PropertyForSort} {(model.IsAscending ? Constants.SortingParams.SORT_ASC : Constants.SortingParams.SORT_DESC)}")
                .Skip((model.CurrentPage - Constants.PaginationParams.DEFAULT_OFFSET) * model.PageSize).Take(model.PageSize).ToListAsync();// TODO Why single if u get a list+++

            int count = await _dbSet
               .Where(o => o.UserId == model.UserId || model.UserId == null)
               .Where(o => string.IsNullOrEmpty(model.Description)
               || EF.Functions.Like(o.Description, $"%{model.Description}%"))
               .Where(o => model.Status == null || o.Status == model.Status).CountAsync();

            var ordersWithCount = (orders, count); //TODO redundant explicit naming+++

            return ordersWithCount;
        }
    }
}
