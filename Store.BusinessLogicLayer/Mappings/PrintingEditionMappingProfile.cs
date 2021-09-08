using AutoMapper;
using Store.BusinessLogicLayer.Models.EditionModel;
using Store.DataAccessLayer.Entities;

namespace Store.BusinessLogicLayer.Mappings
{
    public class PrintingEditionMappingProfile : Profile
    {
        public PrintingEditionMappingProfile()
        {
            CreateMap<PrintingEdition, PrintingEditionModel>().ReverseMap();

        }
    }
}
