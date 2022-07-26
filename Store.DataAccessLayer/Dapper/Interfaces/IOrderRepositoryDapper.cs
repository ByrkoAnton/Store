using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Models.FiltrationModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Dapper.Interfaces
{
   public interface IOrderRepositoryDapper: IDapperBaseRepository<Order> //TODO base interface?+++
    {
        public Task<Order> GetByIdAsync(long id);
        public Task<(IEnumerable<Order>, int)> GetAsync(OrderFiltrationModelDAL model);
        public Task DeleteAsync(long id);
    }
}
