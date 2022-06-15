using Store.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
