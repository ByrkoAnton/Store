using AutoMapper;
using Store.BusinessLogicLayer.Models.Users;

namespace Store.BusinessLogicLayer.Mappings
{
    public class UserFiltrationMappingProfile: Profile//TODO modifier?+++
    {
        public UserFiltrationMappingProfile()
        {
            CreateMap<UserModel, UserFiltrationModel>().ReverseMap();
        }
    }
}
