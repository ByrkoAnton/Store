using AutoMapper;
using Store.BusinessLogicLayer.Models.Authors;
using Store.DataAccessLayer.Models.FiltrationModels;

namespace Store.BusinessLogicLayer.Mappings
{
   public class AuthorFiltrationMappingProfile : Profile
    {
        public AuthorFiltrationMappingProfile()
        {
            CreateMap<AuthorFiltrationModel, AuthorFiltrationModelDAL>().ReverseMap();
        }
    }
}
