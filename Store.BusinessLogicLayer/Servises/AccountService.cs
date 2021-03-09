using Store.BusinessLogicLayer.Models;
using Store.BusinessLogicLayer.Servises.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Store.BusinessLogicLayer.Models.Users;
using Store.DataAccessLayer.Entities;

namespace Store.BusinessLogicLayer.Servises
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<User> _signInManager;
        public AccountService(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }
        public async Task<TokenResponseModel> SignInAsync(SignInModel signInModel)
        {
            var result = new TokenResponseModel();
            if (signInModel != null)
            {
                if (signInModel.Email != null && signInModel.Password != null)
                {
                    var res = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false);

                    if (res.Succeeded)
                    {
                        result.AccessToken = "login";
                        return result;
                    }
                }
            }
            result.AccessToken = "login faild";
            return result;
        }
    }
}
