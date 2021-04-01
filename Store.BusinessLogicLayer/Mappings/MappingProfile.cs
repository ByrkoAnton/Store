using AutoMapper;
using Store.BusinessLogicLayer.Models.Authors;
using Store.BusinessLogicLayer.Models.Users;
using Store.DataAccessLayer.Entities;

namespace Store.BusinessLogicLayer.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserUpdateModel>().ReverseMap();
            CreateMap<Author, AuthorModel>().ReverseMap();
        }
    }
}
