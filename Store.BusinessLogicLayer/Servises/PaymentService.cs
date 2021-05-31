using Store.BusinessLogicLayer.Models.Payments;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.DataAccessLayer.Repositories.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
            var handler = new JwtSecurityTokenHandler().ReadJwtToken(jwt.Remove(jwt.IndexOf(Constants.JwtProvider.BEARER),
                Constants.JwtProvider.BEARER.Length).Trim());
            var id = long.Parse(handler.Claims.Where(a => a.Type == Constants.JwtProvider.ID).FirstOrDefault().Value);

            DataAccessLayer.Entities.Order order = new DataAccessLayer.Entities.Order
            {
                Discription = model.OrderDescription,
                UserId = id,
            };

            await _orderRepository.CreateAsync(order);

            List<long> EditionsId = model.Editions.Select(id => id.EditionId).ToList();

            var editions = await _printingEditionRepository.GetEditionsListByIdListAsync(EditionsId);

            List<DataAccessLayer.Entities.OrderItem> orderItems = new List<DataAccessLayer.Entities.OrderItem>();

            foreach (var i in editions)
            {  
                DataAccessLayer.Entities.OrderItem orderItem = new DataAccessLayer.Entities.OrderItem
                {
                    EditionPrice = i.Price,
                    Currency = i.Currency,
                    PrintingEditionId = i.Id,
                    OrderId = order.Id,
                    Count =(int) model.Editions.FirstOrDefault(c => c.EditionId == i.Id).Count
                };
                orderItems.Add(orderItem);
            }

            await _orderItemRepository.CreateAsync(orderItems);

            var optionsToken = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Number = model.CardNumber,
                    ExpMonth = model.CardExpMonth,
                    ExpYear = model.CardExpYear,
                    Cvc = model.CardCvc
                }
            };

            var serviceToken = new TokenService();
            Token stripeToken = await serviceToken.CreateAsync(optionsToken);

            var options = new ChargeCreateOptions
            {
                Amount = (int)order.OrderItems.Sum(s => s.EditionPrice * s.Count) * Constants.Charge.GET_CENTS,
                Currency = Constants.Charge.USD,
                Description = $"{Constants.Charge.FROM_USER}{order.UserId}",
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
                Message = Constants.Charge.SUCCESS_MSG,
                OrderID = order.Id.ToString()
            };
            return resultPayModel;
        }
        
    }
}
