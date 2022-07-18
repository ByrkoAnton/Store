using Store.BusinessLogicLayer.Models.PaginationsModels;
using Store.BusinessLogicLayer.Models.RequestModel;
using Store.BusinessLogicLayer.Models.Users;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises.Interfaces//TODO wrong spelling
{
    public interface IUserService
    {
        public Task<string> UserUpdateAsync(UserUpdateModel updateModel, string authentication);
        public Task UserDeleteAsync(long id);
        public Task DeleteAllBlockedUserAsync();
        public Task UserBlockStatusChangingAsync(UserUpdateModel updateModel);
        public Task AddUserToRoleAsync(UserUpdateModel updateModel);
        public Task ChangePasswordAsync(ChangePasswordModel model, string authentication); 
        public Task<string> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel);
        public Task ResetPasswordByAdminAsync(string id);
        public Task<NavigationModelBase<UserModel>> GetUsersAsync(UserFiltrationModel model);
        public Task<UserModel> GetUserByIdAsync(string authentication);   
    }
}
