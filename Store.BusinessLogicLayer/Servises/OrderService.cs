using AutoMapper;
using Store.BusinessLogicLayer.Models.Orders;
using Store.BusinessLogicLayer.Models.PaginationsModels;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.DataAccessLayer.Dapper.Interfaces;
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
        private readonly IOrderRepositoryDapper _orderRepositoryDapper;
        private readonly IPrintingEditionRepositiryDapper _printingEditionRepositiryDapper;
        private readonly IPrintingEditionRepository _printingEditionRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IOrderRepositoryDapper orderRepositoryDapper, IUserService userService, IPrintingEditionRepository printingEditionRepository, IPrintingEditionRepositiryDapper printingEditionRepositiryDapper, IMapper maper)

        {
            _orderRepositoryDapper = orderRepositoryDapper;
            _orderRepository = orderRepository;
            _mapper = maper;
            _userService = userService;
            _printingEditionRepository = printingEditionRepository;
            _printingEditionRepositiryDapper = printingEditionRepositiryDapper;
        }

        public async Task<List<OrderModel>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            if (!orders.Any())
            {
                throw new CustomException(Constants.Error.NO_ORDERS_IN_DB,
                   HttpStatusCode.BadRequest);
            }
            var ordersModels = _mapper.Map<IEnumerable<OrderModel>>(orders);

            return ordersModels.ToList();
        }

        public async Task<NavigationModelBase<OrderModel>> GetAsync(OrderFiltrationModel model)
        {
            var orderFiltrPagingSortModelDAL = _mapper.Map<OrderFiltrationModelDAL>(model);

            (IEnumerable<Order> orders, int count) ordersCount = await _orderRepositoryDapper.GetAsync(orderFiltrPagingSortModelDAL);
           
            var orderModels = _mapper.Map<IEnumerable<OrderModel>>(ordersCount.orders);

            PaginatedPageModel paginatedPage = new PaginatedPageModel(ordersCount.count, model.CurrentPage, model.PageSize);
            NavigationModelBase<OrderModel> result = new NavigationModelBase<OrderModel>
            {
                PageModel = paginatedPage,
                Models = orderModels
            };
            return result;
        }

        public async Task<OrderModel> GetByIdAsync(long id)
        {
            if (id is default(long))
            {
                throw new CustomException(Constants.Error.WRONG_MODEL, HttpStatusCode.BadRequest);
            }

            var order = await _orderRepositoryDapper.GetByIdAsync(id);
            if (order is null)
            {
                throw new CustomException(Constants.Error.NO_ORDERS_THIS_ID, HttpStatusCode.BadRequest);
            }

            var orderModel = _mapper.Map<OrderModel>(order);
            return orderModel;
        }

        public async Task<OrderDetailsModel> GetOrderDetails(long id)
        {
            var order = await GetByIdAsync(id);
            var editionsId = order.OrderItems.Select(x => x.PrintingEditionId).ToList();
            var editions = await _printingEditionRepositiryDapper.GetEditionsListByIdListAsync(editionsId);
            var user = await _userService.GetUserByIdAsync(order.UserId.ToString());
            var editionsOrderDetails = _mapper.Map<IEnumerable<EditionInOrderDatails>>(editions).ToList();

            for (int i = 0; i < editionsOrderDetails.Count; i++)
            {
                editionsOrderDetails[i].EditionPrice = order.OrderItems[i].EditionPrice;
                editionsOrderDetails[i].Count = order.OrderItems[i].Count;
                editionsOrderDetails[i].PriceForAllEditions = editionsOrderDetails[i].EditionPrice * editionsOrderDetails[i].Count;
            }

            var orderDetails = new OrderDetailsModel()
            {
                OrderId = order.Id,
                DateOfCreation = order.DateOfCreation,
                Description = order.Discription,
                OrderStatus = order.Status.ToString(),
                PaymentId = order.PaymentId,
                UserId = order.UserId,
                Editions = editionsOrderDetails,
                TotalPrice = editionsOrderDetails.Sum(x => x.PriceForAllEditions),
                UserName = user.LastName
            };

            return orderDetails;
        }

        public async Task RemoveAsync(long id)
        {
            if (id is default(long))
            {
                throw new CustomException(Constants.Error.WRONG_MODEL, HttpStatusCode.BadRequest);
            }

            var order = await _orderRepository.GetByIdAsync(id);
            if (order is null)
            {
                throw new CustomException(Constants.Error.NO_ORDERS_THIS_ID, HttpStatusCode.BadRequest);
            }
            await _orderRepository.RemoveAsync(order);
        }

        public async Task UpdateAsync(OrderModel model)
        {
            if (model is null || model.Id is default(long))
            {
                throw new CustomException(Constants.Error.WRONG_MODEL, HttpStatusCode.BadRequest);
            }

            var order = await _orderRepository.GetByIdAsync(model.Id);
            if (order is null)
            {
                throw new CustomException(Constants.Error.NO_ORDERS_THIS_ID, HttpStatusCode.BadRequest);
            }

            order = _mapper.Map<Order>(model);
            await _orderRepository.UpdateAsync(order);
        }
    }
}
