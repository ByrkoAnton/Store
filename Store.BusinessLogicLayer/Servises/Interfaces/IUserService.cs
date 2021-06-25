using Store.BusinessLogicLayer.Models.PaginationsModels;
using Store.BusinessLogicLayer.Models.RequestModel;
using Store.BusinessLogicLayer.Models.Users;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises.Interfaces
{
    public interface IUserService
    {
        public Task<string> UserUpdateAsync(UserUpdateModel updateModel, string jwt);
        public Task UserDeleteAsync(UserUpdateModel updateModel);
        public Task DeleteAllBlockedUserAsync();
        public Task UserBlockStatusChangingAsync(UserUpdateModel updateModel);
        public Task AddUserToRoleAsync(UserUpdateModel updateModel);
        public Task ChangePasswordAsync(ChangePasswordModel model, string jwt);
        public Task<string> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel);
        public Task<NavigationModel<UserModel>> GetUsersAsync(UserFiltrationModel model);
        public Task<UserModel> GetUserByIdAsync(string id);
    }
}
