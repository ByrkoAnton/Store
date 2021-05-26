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
using Store.BusinessLogicLayer.Models.Stripe;
using static Store.DataAccessLayer.Enums.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Dynamic.Core;


namespace Store.BusinessLogicLayer.Servises
{
    public class PaymentService : IPaymentService
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
        public async Task<ResultPayModel> PayAsync(StripePayModel model, string jwt)
        {
            var handler = new JwtSecurityTokenHandler().ReadJwtToken(jwt.Remove(jwt.IndexOf(Constants.JwtProviderConst.BEARER),
                Constants.JwtProviderConst.BEARER.Length).Trim());
            var id = long.Parse(handler.Claims.Where(a => a.Type == Constants.JwtProviderConst.ID).FirstOrDefault().Value);

            DataAccessLayer.Entities.Order order = new DataAccessLayer.Entities.Order
            {
                Discription = model.OrderDescription,
                UserId = id,
            };

            await _orderRepository.CreateAsync(order);

            List<long> EditionsId = model.EditionsIdAndQuant.Select(id => id.EditionId).ToList();

            var editions = await _printingEditionRepository.GetEditionsListByIdListAsync(EditionsId);

            List<DataAccessLayer.Entities.OrderItem> orderItems = new List<DataAccessLayer.Entities.OrderItem>();

            foreach (var i in model.EditionsIdAndQuant)
            {
                var edition = editions.FirstOrDefault(e => e.Id == i.EditionId);
                DataAccessLayer.Entities.OrderItem orderItem = new DataAccessLayer.Entities.OrderItem
                {
                    Amount = edition.Price * i.Count,
                    Currency = edition.Currency,
                    PrintingEditionId = edition.Id,
                    OrderId = order.Id,
                    Count = (int)i.Count
                };
                orderItems.Add(orderItem);
            }

            await _orderItemRepository.CreateAsync(orderItems);

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
                Description = $"{Constants.ChargeConstants.FROM_USER}{order.UserId}",
                Source = stripeToken.Id
            };

            var service = new ChargeService();
            Charge charge = await service.CreateAsync(options);

            var stratus = charge.Paid ? OrderStatusState.Payed : OrderStatusState.Unpayed;

            Payment payment = new Payment
            {
                TransactionId = charge.PaymentMethod,
            };
            await _paymentRepository.CreateAsync(payment);
            order.PaymentId = payment.Id;
            order.Status = stratus;
            await _orderRepository.UpdateAsync(order);

            var resultPayModel = new ResultPayModel()
            {
                Message = Constants.ChargeConstants.SUCCESS_MSG,
                OrderID = order.Id.ToString()
            };
            return resultPayModel;
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
