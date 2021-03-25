using Store.BusinessLogicLayer.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises.Interfaces
{
    public interface IUserService
    {
        public Task UserUpdateAsync(UserUpdateModel updateModel);
        public Task UserDeleteAsync(UserUpdateModel updateModel);
        public Task UserBlockStatusChangingAsync(UserUpdateModel updateModel);
      
        public Task<IList<string>> GetUserRoleAsync(UserUpdateModel updateModel);
        public Task AddUserToRoleAsync(UserUpdateModel updateModel);
        public Task<bool> IsUserInRoleAsync(UserUpdateModel updateModel);
    }
}
