using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.AppContext;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Base;
using Store.DataAccessLayer.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationContext context) : base(context)
        {
        }
        public async Task<Payment> GetByIdAsync(long id)
        {
            var result = await _dbSet.AsNoTracking().FirstOrDefaultAsync(Payment => Payment.Id == id);//TODO wrong naming
            return result;
        }
        public async Task<Payment> GetByTransactionIdAsync(string transactionId)
        {
            var result = await _dbSet.AsNoTracking().FirstOrDefaultAsync(t => t.TransactionId == transactionId);
            return result;
        }

    }
}
