using Store.DataAccessLayer.Entities;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Dapper.Interfaces
{
   public interface IPaymentRepositoryDapper //TODO base interface?
    {
        public Task CreateAsync(Payment payment);
    }
}
