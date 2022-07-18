using AutoMapper;
using Store.BusinessLogicLayer.Models.Orders;
using Store.DataAccessLayer.Entities;
//TODO extra line

namespace Store.BusinessLogicLayer.Mappings
{
    class EditionInOrderDetailsMappingProfile : Profile//TODO modifier?
    {
        public EditionInOrderDetailsMappingProfile()
        {
            CreateMap<PrintingEdition, EditionInOrderDatails>()
                .ForMember(x => x.EditionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.EditionTitle, opt => opt.MapFrom(src => src.Title))
                .ForMember(x => x.EditionPrice, opt => opt.MapFrom(src => src.Price));
        }
    }
}
