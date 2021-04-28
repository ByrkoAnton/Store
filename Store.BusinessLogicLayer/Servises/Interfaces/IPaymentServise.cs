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
        public Task CreateAsync(PaymentCreationModel model);
    }
}
