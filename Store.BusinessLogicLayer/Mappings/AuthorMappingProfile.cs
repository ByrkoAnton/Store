using AutoMapper;
using Store.BusinessLogicLayer.Models.Authors;
//TODO extra line
using Store.DataAccessLayer.Entities;

namespace Store.BusinessLogicLayer.Mappings
{
    public class AuthorMappingProfile : Profile
    {
        public AuthorMappingProfile()
        {
            CreateMap<Author, AuthorModel>().ForMember(x => x.PrintingEditionModels, opt => opt.MapFrom(src => src.PrintingEditions));
            CreateMap<AuthorModel, Author>().ForMember(x => x.PrintingEditions, opt => opt.MapFrom(src => src.PrintingEditionModels));
        }
    }
}
