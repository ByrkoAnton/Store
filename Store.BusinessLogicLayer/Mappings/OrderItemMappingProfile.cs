using AutoMapper;
using Store.BusinessLogicLayer.Models.OrderItem;
using Store.DataAccessLayer.Entities;

namespace Store.BusinessLogicLayer.Mappings
{
    public class OrderItemMappingProfile : Profile
    {
        public OrderItemMappingProfile()
        {
            CreateMap<OrderItemModel, OrderItem>().ForMember(x => x.DateOfCreation, opt => opt.Ignore());
            CreateMap<OrderItem, OrderItemModel>();
        }
    }
}