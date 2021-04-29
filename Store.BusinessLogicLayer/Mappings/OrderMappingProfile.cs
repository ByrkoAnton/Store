using AutoMapper;
using Store.BusinessLogicLayer.Models.Orders;
using Store.DataAccessLayer.Entities;

namespace Store.BusinessLogicLayer.Mappings
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<OrderModel, Order>().ForMember(x => x.DateOfCreation, opt => opt.Ignore());
            CreateMap<Order, OrderModel>();
        }
    }
}
