using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Store.BusinessLogicLayer.Models.Orders;
using Store.BusinessLogicLayer.Models.PaginationsModels;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Models.FiltrationModels;
using Store.DataAccessLayer.Repositories.Interfaces;
using Store.Sharing.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper maper, UserManager<User> userManager,//TODO AD:
            IPaymentRepository paymentRepository)
        {
            _orderRepository = orderRepository;
            _mapper = maper;
        }

        public async Task<List<OrderModel>> GetAll()
        {
            var orders = await _orderRepository.GetAllAsync();
            if (!orders.Any())
            {
                throw new CustomExeption(Constants.Error.NO_ORDERS_IN_DB,
                   HttpStatusCode.BadRequest);
            }
            var ordersModels = _mapper.Map<IEnumerable<OrderModel>>(orders);

            return ordersModels.ToList();
        }

        public async Task<NavigationModelBase<OrderModel>> GetAsync(OrderFiltrationModel model)
        {
            var orderFiltrPagingSortModelDAL = _mapper.Map<OrderFiltrationModelDAL>(model);

            (IEnumerable<Order> orders, int count) ordersCount = await _orderRepository.GetAsync(orderFiltrPagingSortModelDAL);

            if (ordersCount.count is default(int))
            {
                throw new CustomExeption(Constants.Error.WRONG_CONDITIONS_ORDER,
                   HttpStatusCode.BadRequest);
            }

            var orderModels = _mapper.Map<IEnumerable<OrderModel>>(ordersCount.orders);

            PaginatedPageModel paginatedPage = new PaginatedPageModel(ordersCount.count, model.CurrentPage, model.PageSize);
            NavigationModelBase<OrderModel> result = new NavigationModelBase<OrderModel>
            {
                PageModel = paginatedPage,
                Models = orderModels
            };
            return result;
        }

        public async Task<OrderModel> GetById(long id)
        {
            if (id is default(long))
            {
                throw new CustomExeption(Constants.Error.WRONG_MODEL, HttpStatusCode.BadRequest);
            }

            var order = await _orderRepository.GetByIdAsync(id);
            if (order is null)
            {
                throw new CustomExeption(Constants.Error.NO_ORDERS_THIS_ID, HttpStatusCode.BadRequest);
            }

            var orderModel = _mapper.Map<OrderModel>(order);
            return orderModel;
        }

        public async Task RemoveAsync(OrderModel model)
        {
            if (model is null)
            {
                throw new CustomExeption(Constants.Error.WRONG_MODEL, HttpStatusCode.BadRequest);
            }

            if (model.Id is default(long))
            {
                throw new CustomExeption(Constants.Error.WRONG_MODEL, HttpStatusCode.BadRequest);
            }

            var order = await _orderRepository.GetByIdAsync(model.Id);
            if (order is null)
            {
                throw new CustomExeption(Constants.Error.NO_ORDERS_THIS_ID, HttpStatusCode.BadRequest);
            }
            await _orderRepository.RemoveAsync(order);
        }

        public async Task UpdateAsync(OrderModel model)
        {
            if (model is null)
            {
                throw new CustomExeption(Constants.Error.WRONG_MODEL, HttpStatusCode.BadRequest);
            }

            if (model.Id is default(long))
            {
                throw new CustomExeption(Constants.Error.WRONG_MODEL, HttpStatusCode.BadRequest);
            }

            var order = await _orderRepository.GetByIdAsync(model.Id);
            if (order is null)
            {
                throw new CustomExeption(Constants.Error.NO_ORDERS_THIS_ID, HttpStatusCode.BadRequest);
            }

            order = _mapper.Map<Order>(model);
            await _orderRepository.UpdateAsync(order);
        }
    }
}
