using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Base;

using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.Interfaces
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        public Task<Payment> GetByIdAsync(long id);
        public Task<Payment> GetByTransactionIdAsync(string transactionId);
        public Task<long> GetLastId();

    }
}
