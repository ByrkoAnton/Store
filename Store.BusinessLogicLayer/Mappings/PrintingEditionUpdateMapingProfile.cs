using AutoMapper;
using Store.DataAccessLayer.Entities;

namespace Store.BusinessLogicLayer.Mappings
{
    public class PrintingEditionUpdateMappingProfile : Profile
    {
        public PrintingEditionUpdateMappingProfile()
        {
            CreateMap<PrintingEdition, PrintingEdition>().ReverseMap();
        }
    }
}
