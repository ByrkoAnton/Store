using Store.BusinessLogicLayer.Models.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises.Interfaces
{
    public interface IPaymentServise
    {
        public Task CreateAsync(PaymentModel model);
        public Task<PaymentModel> GetByTransactionId(PaymentModel model);
        public Task<List<PaymentModel>> GetAll();
        public Task UpdateAsync(PaymentModel model);
        public Task RemoveAsync(PaymentModel model);
    }
}
