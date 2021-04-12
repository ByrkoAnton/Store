using AutoMapper;
using Store.BusinessLogicLayer.Models.Authors;
using Store.BusinessLogicLayer.Models.EditionModel;
using Store.DataAccessLayer.Entities;

namespace Store.BusinessLogicLayer.Mappings
{
    public class AuthorMappingProfile : Profile
    {
        public AuthorMappingProfile()
        {
            //CreateMap<PrintingEditionModel, PrintingEdition>().ForMember(x => x.Authors, opt => opt.MapFrom(src => src.AuthorModels));
            //CreateMap<PrintingEdition, PrintingEditionModel>();

            CreateMap<Author, AuthorModel>().ForMember(x => x.PrintingEditionModels, opt => opt.MapFrom(src => src.PrintingEditions));
            CreateMap<AuthorModel, Author>().ForMember(x => x.PrintingEditions, opt => opt.MapFrom(src => src.PrintingEditionModels));

        }
    }
}
