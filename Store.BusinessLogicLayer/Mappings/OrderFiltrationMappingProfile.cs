using AutoMapper;
using Store.BusinessLogicLayer.Models.Orders;
using Store.DataAccessLayer.Models.FiltrationModels;

namespace Store.BusinessLogicLayer.Mappings
{
    public class OrderFiltrationMappingProfile : Profile
    {
        public OrderFiltrationMappingProfile()
        {
            CreateMap<OrderFiltrPagingSortModel, OrderFiltrPagingSortModelDAL>().ReverseMap();
        }
    }
}
