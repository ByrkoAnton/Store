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
using Stripe;
using Store.BusinessLogicLayer.Models.Stipe;
using static Store.DataAccessLayer.Enums.Enums;
using System.IdentityModel.Tokens.Jwt;

namespace Store.BusinessLogicLayer.Servises
{
    public class PaymentService :IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPrintingEditionRepository _printingEditionRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;
        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository, IMapper maper,
         IPrintingEditionRepository printingEditionRepository, IOrderItemRepository orderItemRepository)
        {
            _paymentRepository = paymentRepository;
            _mapper = maper;
            _orderRepository = orderRepository;
            _printingEditionRepository = printingEditionRepository;
            _orderItemRepository = orderItemRepository;

        }
        public async Task<string> PayAsync(StripePayModel model)
        {
            var handler = new JwtSecurityTokenHandler().ReadJwtToken(model.Jwt);
            var id = long.Parse(handler.Claims.Where(a=>a.Type == Constants.JwtProviderConst.ID).FirstOrDefault().Value);
            
            DataAccessLayer.Entities.Order order = new DataAccessLayer.Entities.Order
            {
                Discription = model.OrderDescription,
                UserId = id,   
            };

            await _orderRepository.CreateAsync(order);

            foreach (var i in model.EditionsIdAndQuant)
            {
                var edition = await _printingEditionRepository.GetByIdAsync(i.EditionId);
                DataAccessLayer.Entities.OrderItem orderItem = new DataAccessLayer.Entities.OrderItem
                {
                    Amount = edition.Prise * i.Count,
                    Currency = edition.Currency,
                    PrintingEditionId = edition.Id,
                    OrderId = order.Id,
                    Count = (int)i.Count
                };

                await _orderItemRepository.CreateAsync(orderItem);
            }

            try
            {
                var optionsToken = new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    {
                        Number = model.CardNumber,
                        ExpMonth = model.Month,
                        ExpYear = model.Year,
                        Cvc = model.Cvc
                    }
                };

                var serviceToken = new TokenService();
                Token stripeToken = await serviceToken.CreateAsync(optionsToken);

                var options = new ChargeCreateOptions
                {
                    Amount = (int)order.OrderItems.Sum(s => s.Amount) * Constants.ChargeConstants.GET_CENTS,
                    Currency = Constants.ChargeConstants.USD,
                    Description = $"{Constants.ChargeConstants.FROM_USER}{order.Id}",
                    Source = stripeToken.Id
                };

                var service = new ChargeService();
                Charge charge = await service.CreateAsync(options);
               
                if (charge.Paid)
                {
                    Payment payment = new Payment
                    {
                        TransactionId = charge.PaymentMethod,
                    };
                    await _paymentRepository.CreateAsync(payment);
                    order.PaymentId = payment.Id;
                    order.Status = OrderStatus.Payed;
                    await _orderRepository.UpdateAsync(order);
                    return Constants.ChargeConstants.SUCCESS_MSG;
                }

                return Constants.ChargeConstants.UN_SUCCESS_MSG;
            }
            catch (System.Exception e)
            {
                return e.Message.ToString();
            }
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
