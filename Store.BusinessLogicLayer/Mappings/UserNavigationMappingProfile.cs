using AutoMapper;
using Store.BusinessLogicLayer.Models.PaginationsModels;
using Store.BusinessLogicLayer.Models.Users;

namespace Store.BusinessLogicLayer.Mappings
{
    class UserNavigationMappingProfile: Profile//TODO modifier?
    {
        public UserNavigationMappingProfile()
        {
            CreateMap<NavigationModelBase<UserModel>, NavigationModelBase<UserFiltrationModel>>().ForMember(x => x.Models, opt => opt.Ignore()).ReverseMap();
        }
    }
}
