﻿using Store.BusinessLogicLayer.Models.Payments;
using Store.BusinessLogicLayer.Models.Stripe;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Serviсes.Interfaces
{
    public interface IPaymentService
    {
        public Task<ResultPayModel> PayAsync(StripePayModel model, string jwt);
    }
}
