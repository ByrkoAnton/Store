using AutoMapper;
using Store.BusinessLogicLayer.Models.Users;
namespace Store.BusinessLogicLayer.Mappings
{
    class UserFiltrationMappingProfile: Profile
    {
        public UserFiltrationMappingProfile()
        {
            CreateMap<UserModel, UserFiltrationModel>().ReverseMap();
        }
    }
}
