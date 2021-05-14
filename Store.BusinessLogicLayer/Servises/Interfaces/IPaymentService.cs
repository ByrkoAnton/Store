using Store.BusinessLogicLayer.Models.Payments;
using Store.BusinessLogicLayer.Models.Stipe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises.Interfaces
{
    public interface IPaymentService
    {
        public Task<PaymentModel> GetByTransactionId(PaymentModel model);
        public Task<List<PaymentModel>> GetAll();
        public Task UpdateAsync(PaymentModel model);
        public Task RemoveAsync(PaymentModel model);
        public Task<string> PayAsync(StripePayModel model, string jwt);
    }
}
