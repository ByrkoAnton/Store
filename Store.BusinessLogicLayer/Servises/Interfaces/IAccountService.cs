
using System.Threading.Tasks;
using Store.BusinessLogicLayer.Models;
using Store.BusinessLogicLayer.Models.RequestModel;
using Store.BusinessLogicLayer.Models.Users;

namespace Store.BusinessLogicLayer.Servises.Interfaces
{
    public interface IAccountService
    {
        public Task<TokenResponseModel> SignInAsync(UserSignInModel loginModel);
        public Task<string> SignUpAsync(UserModel signUpModel);  
        public Task SignOutAsync();
        public Task<string> ConfirmEmailAsync(EmailConfirmationModel emailConfirmationModel);
    }
}
