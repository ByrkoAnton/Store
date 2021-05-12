using AutoMapper;
using Microsoft.AspNetCore.Http;
using Store.BusinessLogicLayer.Models.OrderItem;
using Store.BusinessLogicLayer.Models.Orders;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Interfaces;
using Store.Sharing.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;

        public OrderItemService(IOrderItemRepository orderItemRepository, IMapper maper)
        {
            _orderItemRepository = orderItemRepository;
            _mapper = maper;
        }
        public async Task CreateAsync(OrderItemModel model)
        {
            var orderItem = _mapper.Map<OrderItem>(model);
            await _orderItemRepository.CreateAsync(orderItem);
        }

        public async Task<List<OrderItemModel>> GetAll()
        {
            var orderItems = await _orderItemRepository.GetAllAsync();
            if (!orderItems.Any())
            {
                throw new CustomExeption(Constants.Error.NO_ANY_ORDERITEMS_IN_DB,
                   StatusCodes.Status400BadRequest);
            }
            var orderItemModels = _mapper.Map<IEnumerable<OrderItemModel>>(orderItems);

            return orderItemModels.ToList();
        }

        public async Task<OrderItemModel> GetById(long id)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(id);
            if (orderItem is null)
            {
                throw new CustomExeption(Constants.Error.NO_ANY_ORDERITEMS_IN_DB,
                   StatusCodes.Status400BadRequest);
            }
            var orderItemModel = _mapper.Map<OrderItemModel>(orderItem);

            return orderItemModel;
        }

        public async Task RemoveAsync(OrderItemModel model)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(model.Id);
            if (orderItem is null)
            {
                throw new CustomExeption(Constants.Error.NO_ANY_ORDERITEMS_IN_DB_WITH_THIS_ID,
                    StatusCodes.Status400BadRequest);
            }
            await _orderItemRepository.RemoveAsync(orderItem);
        }

        public async Task UpdateAsync(OrderItemModel model)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(model.Id);
            if (orderItem is null)
            {
                throw new CustomExeption(Constants.Error.NO_ANY_ORDERITEMS_IN_DB_WITH_THIS_ID,
                    StatusCodes.Status400BadRequest);
            }

            orderItem = _mapper.Map<OrderItem>(model);
            await _orderItemRepository.UpdateAsync(orderItem);
        }
    }
}
