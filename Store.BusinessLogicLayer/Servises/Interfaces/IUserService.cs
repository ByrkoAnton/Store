using Store.BusinessLogicLayer.Models.PaginationsModels;
using Store.BusinessLogicLayer.Models.RequestModel;
using Store.BusinessLogicLayer.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises.Interfaces
{
    public interface IUserService
    {
        public Task UserUpdateAsync(UserUpdateModel updateModel);
        public Task UserDeleteAsync(UserUpdateModel updateModel);
        public Task DeleteAllBlockedUserAsync();
        public Task UserBlockStatusChangingAsync(UserUpdateModel updateModel);
        public Task<IList<string>> GetUserRoleAsync(UserUpdateModel updateModel);
        public Task AddUserToRoleAsync(UserUpdateModel updateModel);
        public Task<bool> IsUserInRoleAsync(UserUpdateModel updateModel);
        public Task<string> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel);
        public Task<IndexViewModel<UserModel>> GetUsersAsync(UserFiltrationModel model, string prop, int page, int pageSize , bool asc);
        public Task<List<UserModel>> GetFiltratedUsers(UserFiltrationModel model);
    }
}
