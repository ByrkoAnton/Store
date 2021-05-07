using Store.BusinessLogicLayer.Models.Payments;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.DataAccessLayer.Repositories.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Store.DataAccessLayer.Entities;
using Store.Sharing.Constants;
using System.Collections.Generic;

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

        public async Task CreateAsync(PaymentModel model)
        {
            var payment = await _paymentRepository.GetByTransactionIdAsync(model.TransactionId);

            if (payment is not null)
            {
                throw new CustomExeption(Constants.Error.PAYMENT_CREATION_FAILD_PATMENT_ID_EXISTS,
                    StatusCodes.Status400BadRequest);
            }

            payment = _mapper.Map<Payment>(model);
            await _paymentRepository.CreateAsync(payment);
        }

        public async Task<PaymentModel> GetByTransactionId(PaymentModel model)
        {
            var payment = await _paymentRepository.GetByTransactionIdAsync(model.TransactionId);
            if (payment is null)
            {
                throw new CustomExeption(Constants.Error.NO_ANY_PAYMENTS_IN_DB_WITH_THIS_TRANSACTION_ID,
                   StatusCodes.Status400BadRequest);
            }
            var paymentModel = _mapper.Map<PaymentModel>(payment);

            return paymentModel;
        }
        public async Task<List<PaymentModel>> GetAll()
        {
            var payments = await _paymentRepository.GetAllAsync();
            if (!payments.Any())
            {
                throw new CustomExeption(Constants.Error.NO_ANY_PAYMENTS_IN_DB,
                   StatusCodes.Status400BadRequest);
            }
            var paymentModel = _mapper.Map<IEnumerable<PaymentModel>>(payments);

            return paymentModel.ToList();
        }
        public async Task UpdateAsync(PaymentModel model)
        {
            var payment = await _paymentRepository.GetByIdAsync(model.Id);
            if (payment is null)
            {
                throw new CustomExeption(Constants.Error.NO_ANY_PAYMENTS_IN_DB_WITH_THIS_ID,
                    StatusCodes.Status400BadRequest);
            }

            payment = _mapper.Map<Payment>(model);

            await _paymentRepository.UpdateAsync(payment);
        }
        public async Task RemoveAsync(PaymentModel model)
        {
            var payment = await _paymentRepository.GetByTransactionIdAsync(model.TransactionId);
            if (payment is null)
            {
                throw new CustomExeption(Constants.Error.NO_ANY_PAYMENTS_IN_DB_WITH_THIS_TRANSACTION_ID,
                    StatusCodes.Status400BadRequest);
            }
            await _paymentRepository.RemoveAsync(payment);
        }
    }
}
