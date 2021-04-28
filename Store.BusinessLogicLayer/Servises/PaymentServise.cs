using Store.BusinessLogicLayer.Models.Payments;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Store.BusinessLogicLayer.Models.Authors;
using Store.BusinessLogicLayer.Models.PaginationsModels;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Models.FiltrationModels;
using Store.DataAccessLayer.Repositories.Interfaces;
using Store.Sharing.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises
{
    public class PaymentServise :IPaymentServise
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        public PaymentServise(IPaymentRepository paymentRepository, IMapper maper)
        {
            _paymentRepository = paymentRepository;
            _mapper = maper;
        }

        public async Task CreateAsync(PaymentCreationModel model)
        {
            var payments = await _paymentRepository.GetAsync(Payment => Payment.TransactionId == model.TransactionId);

            if (payments.Any())
            {
                throw new CustomExeption(Constants.Error.PAYMENT_CREATION_FAILD_PATMENT_ID_EXISTS,
                    StatusCodes.Status400BadRequest);
            }

            var payment = _mapper.Map<Payment>(model);
            await _paymentRepository.CreateAsync(payment);
        }
    }
}
