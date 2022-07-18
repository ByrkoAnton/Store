using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Models.FiltrationModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Dapper.Interfaces
{
   public interface IOrderRepositoryDapper //TODO base interface?
    {
        public Task CreateAsync(Order order);
        public Task UpdateAsync(Order order);
        public Task<Order> GetByIdAsync(long id);
        public Task<(IEnumerable<Order>, int)> GetAsync(OrderFiltrationModelDAL model);
        public Task DeleteAsync(long id);
    }
}
