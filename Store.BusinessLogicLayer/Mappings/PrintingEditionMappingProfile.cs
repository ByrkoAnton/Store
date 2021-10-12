using AutoMapper;
using Store.BusinessLogicLayer.Models.EditionModel;
using Store.DataAccessLayer.Entities;

namespace Store.BusinessLogicLayer.Mappings
{
    public class PrintingEditionMappingProfile : Profile
    {
        public PrintingEditionMappingProfile()
        {
            CreateMap<PrintingEdition, PrintingEditionModel>().ForMember(x => x.AuthorModels, opt => opt.MapFrom(src => src.Authors));

            CreateMap<PrintingEditionModel, PrintingEdition>().ForMember(x => x.Authors, opt => opt.MapFrom(src => src.AuthorModels));
        }
    }
}
