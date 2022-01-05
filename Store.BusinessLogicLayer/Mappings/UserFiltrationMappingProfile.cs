using AutoMapper;
using Store.BusinessLogicLayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
