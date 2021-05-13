using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Store.BusinessLogicLayer.Models.Orders;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Interfaces;
using Store.Sharing.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public OrderService(IOrderRepository orderRepository, IMapper maper, UserManager<User> userManager,
            IPaymentRepository paymentRepository)
        {
            _orderRepository = orderRepository;
            _mapper = maper;
            _userManager = userManager;
            _paymentRepository = paymentRepository;
        }
        public async Task CreateAsync(OrderModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (user is null)
            {
                throw new CustomExeption(Constants.Error.NO_USER_ID_IN_DB,
                    StatusCodes.Status400BadRequest);
            }
            var order = _mapper.Map<Order>(model);

            await _paymentRepository.CreateAsync(new Payment());
            order.PaymentId = await _paymentRepository.GetLastId();

            await _orderRepository.CreateAsync(order);
        }

        public async Task<List<OrderModel>> GetAll()
        {
            var orders = await _orderRepository.GetAllAsync();
            if (!orders.Any())
            {
                throw new CustomExeption(Constants.Error.NO_ANY_ORDERS_IN_DB,
                   StatusCodes.Status400BadRequest);
            }
            var ordersModels = _mapper.Map<IEnumerable<OrderModel>>(orders);

            return ordersModels.ToList();
        }

        public async Task<OrderModel> GetById(long id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order is null)
            {
                throw new CustomExeption(Constants.Error.NO_ANY_ORDERS_IN_DB_WITH_THIS_ID,
                    StatusCodes.Status400BadRequest);
            }

            var orderModel = _mapper.Map<OrderModel>(order);
            return orderModel;
        }

        public async Task RemoveAsync(OrderModel model)
        {
            var order = await _orderRepository.GetByIdAsync(model.Id);
            if (order is null)
            {
                throw new CustomExeption(Constants.Error.NO_ANY_ORDERS_IN_DB_WITH_THIS_ID,
                    StatusCodes.Status400BadRequest);
            }
            await _orderRepository.RemoveAsync(order);
        }

        public async Task UpdateAsync(OrderModel model)
        {
            var order = await _orderRepository.GetByIdAsync(model.Id);
            if (order is null)
            {
                throw new CustomExeption(Constants.Error.NO_ANY_ORDERS_IN_DB_WITH_THIS_ID,
                    StatusCodes.Status400BadRequest);
            }

            order = _mapper.Map<Order>(model);

            await _orderRepository.UpdateAsync(order);
        }
    }
}
