using AutoMapper;
using Store.BusinessLogicLayer.Models.Users;

namespace Store.BusinessLogicLayer.Mappings
{
    class UserFiltrationMappingProfile: Profile//TODO modifier?
    {
        public UserFiltrationMappingProfile()
        {
            CreateMap<UserModel, UserFiltrationModel>().ReverseMap();
        }
    }
}
