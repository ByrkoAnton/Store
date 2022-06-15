using Store.DataAccessLayer.Entities;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Dapper.Interfaces
{
   public interface IPaymentRepositoryDapper
    {
        public Task<Payment> GetByIdAsync(long id);
        public Task<Payment> GetByTransactionIdAsync(string transactionId);
        public Task CreateAsync(Payment payment);
    }
}
