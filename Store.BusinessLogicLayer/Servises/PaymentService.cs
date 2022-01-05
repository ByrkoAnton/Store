using Store.BusinessLogicLayer.Models.Payments;
using Store.BusinessLogicLayer.Models.Stripe;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Interfaces;
using Store.Sharing.Constants;
using Stripe;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static Store.DataAccessLayer.Enums.Enums;
using Order = Store.DataAccessLayer.Entities.Order;
using OrderItem = Store.DataAccessLayer.Entities.OrderItem;

namespace Store.BusinessLogicLayer.Servises
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPrintingEditionRepository _printingEditionRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository,
         IPrintingEditionRepository printingEditionRepository, IOrderItemRepository orderItemRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _printingEditionRepository = printingEditionRepository;
            _orderItemRepository = orderItemRepository;
        }
        public async Task<ResultPayModel> PayAsync(StripePayModel model, string jwt)
        {
            var jwtTrimed = jwt.Replace(Constants.JwtProvider.BEARER, string.Empty).Trim();
            var handler = new JwtSecurityTokenHandler().ReadJwtToken(jwtTrimed);

            var id = long.Parse(handler.Claims.Where(a => a.Type == Constants.JwtProvider.ID).FirstOrDefault().Value);

            Order order = new Order
            {
                Discription = model.OrderDescription,
                UserId = id,
            };

            await _orderRepository.CreateAsync(order);

            List<long> editionIds = model.Editions.Select(id => id.EditionId).ToList();

            var editions = await _printingEditionRepository.GetEditionsListByIdListAsync(editionIds);

            List<OrderItem> orderItems = new List<OrderItem>();
            //List<OrderItem> test = editions.Select(edition => new OrderItem
            //{
            //    EditionPrice = edition.Price,
            //    Currency = edition.Currency,
            //    PrintingEditionId = edition.Id,
            //    OrderId = order.Id,
            //    Count = (int)model.Editions.FirstOrDefault(c => c.EditionId == edition.Id).Count
            //}).ToList(); 
            foreach (var i in editions)
            {
                OrderItem orderItem = new OrderItem
                {
                    EditionPrice = i.Price,
                    Currency = i.Currency,
                    PrintingEditionId = i.Id,
                    OrderId = order.Id,
                    Count = (int)model.Editions.FirstOrDefault(c => c.EditionId == i.Id).Count
                };
                orderItems.Add(orderItem);
            }

            await _orderItemRepository.CreateAsync(orderItems);

            var options = new ChargeCreateOptions
            {
                Amount = (int)order.OrderItems.Sum(s => s.EditionPrice * s.Count) * Constants.Charge.GET_CENTS,
                Currency = Constants.Charge.USD,
                Description = $"{Constants.Charge.FROM_USER}{order.UserId}",
                Source = model.Token
            };

            var service = new ChargeService();
            Charge charge = await service.CreateAsync(options);

            var status = charge.Paid ? OrderStatusState.Payed : OrderStatusState.Unpayed;

            Payment payment = new Payment
            {
                TransactionId = charge.Id,
            };
            await _paymentRepository.CreateAsync(payment);
            order.PaymentId = payment.Id;
            order.Status = status;
            await _orderRepository.UpdateAsync(order);

            var resultPayModel = new ResultPayModel()
            {
                Message = Constants.Charge.SUCCESS_MSG,
                OrderId = order.Id.ToString()
            };
            return resultPayModel;
        }

    }
}
