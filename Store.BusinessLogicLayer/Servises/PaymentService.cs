using Store.BusinessLogicLayer.Models.Payments;
using Store.BusinessLogicLayer.Models.Stripe;
using Store.BusinessLogicLayer.Serviсes.Interfaces;
using Store.DataAccessLayer.Dapper.Interfaces;
using Store.DataAccessLayer.Entities;
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

namespace Store.BusinessLogicLayer.Serviсes//TODO spelling
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepositoryDapper _paymentRepositoryDapper;
        private readonly IOrderRepositoryDapper _orderRepositoryDapper;
        private readonly IPrintingEditionRepositiryDapper _printingEditionRepositoryDapper;
        private readonly IOrderItemRepositoryDapper _orderItemRepositoryDapper;
        public PaymentService(IPaymentRepositoryDapper paymentRepositoryDapper, IOrderRepositoryDapper orderRepositoryDapper, IPrintingEditionRepositiryDapper printingEditionRepositiryDapper, IOrderItemRepositoryDapper orderItemRepositoryDapper)//TODO spelling
        {
            _paymentRepositoryDapper = paymentRepositoryDapper;
            _printingEditionRepositoryDapper = printingEditionRepositiryDapper;
            _orderItemRepositoryDapper = orderItemRepositoryDapper;
            _orderRepositoryDapper = orderRepositoryDapper;
        }
        public async Task<ResultPayModel> PayAsync(StripePayModel model, string jwt)
        {
            var jwtTrimmed = jwt.Replace(Constants.JwtProvider.BEARER, string.Empty).Trim();//TODO spelling+++
            var handler = new JwtSecurityTokenHandler().ReadJwtToken(jwtTrimmed);

            var id = long.Parse(handler.Claims.Where(a => a.Type == Constants.JwtProvider.ID).FirstOrDefault().Value);

            Order order = new Order
            {
                Description = model.OrderDescription,
                UserId = id,
            };

            await _orderRepositoryDapper.CreateAsync(order);

            List<long> editionIds = model.Editions.Select(id => id.EditionId).ToList();

            var editions = await _printingEditionRepositoryDapper.GetEditionsListByIdListAsync(editionIds);

            List<OrderItem> orderItems = new List<OrderItem>();
          
            foreach (var edition in editions)
            {
                OrderItem orderItem = new OrderItem
                {
                    EditionPrice = edition.Price,
                    Currency = edition.Currency,
                    PrintingEditionId = edition.Id,
                    OrderId = order.Id,
                    Count = (int)model.Editions.FirstOrDefault(c => c.EditionId == edition.Id).Count//TODO possible null reference
                };
                orderItems.Add(orderItem);
            }

            await _orderItemRepositoryDapper.CreateAsync(orderItems);

            var options = new ChargeCreateOptions
            {
                Amount = (int)orderItems.Sum(s => s.EditionPrice * s.Count) * Constants.Charge.GET_CENTS,
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
            await _paymentRepositoryDapper.CreateAsync(payment);
            order.PaymentId = payment.Id;
            order.Status = status;
            await _orderRepositoryDapper.UpdateAsync(order);

            var resultPayModel = new ResultPayModel()
            {
                Message = Constants.Charge.SUCCESS_MASSAGE,
                OrderId = order.Id.ToString()
            };
            return resultPayModel;
        }

    }
}
