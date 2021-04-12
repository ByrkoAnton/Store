using AutoMapper;
using Store.BusinessLogicLayer.Models.Authors;
using Store.BusinessLogicLayer.Models.EditionModel;
using Store.DataAccessLayer.Entities;

namespace Store.BusinessLogicLayer.Mappings
{
    public class PrintingEditionMappingProfile : Profile
    {
        public PrintingEditionMappingProfile()
        {
            //CreateMap<AuthorModel, Author>().ForMember(x => x.PrintingEditions, opt => opt.MapFrom(src => src.PrintingEditionModels));
            //CreateMap<Author, AuthorModel>();

            
            CreateMap<PrintingEdition, PrintingEditionModel>().ForMember(x => x.AuthorModels, opt => opt.MapFrom(src => src.Authors));
            CreateMap<PrintingEditionModel, PrintingEdition>().ForMember(x => x.Authors, opt => opt.MapFrom(src => src.AuthorModels));

        }
    }
}
