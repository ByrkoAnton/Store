using AutoMapper;
using Store.BusinessLogicLayer.Models.Users;
using Store.DataAccessLayer.Entities;

namespace Store.BusinessLogicLayer.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>().ForMember(x => x.UserName, opt => opt.MapFrom(src => src.Email));
        }

    }
}
