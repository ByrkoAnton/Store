using Store.DataAccessLayer.Entities;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Dapper.Interfaces
{
   public interface IPaymentRepositoryDapper
    {
        public Task CreateAsync(Payment payment);
    }
}
