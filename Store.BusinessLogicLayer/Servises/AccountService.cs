using Store.BusinessLogicLayer.Models;
using Store.BusinessLogicLayer.Models.Users;
using Store.BusinessLogicLayer.Servises.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Store.BusinessLogicLayer.Servises
{
    public class AccountService : IAccountService
    {
        

        public async Task<TokenResponseModel> SignInAsync(UserModel userModel)
        {
            var result = new TokenResponseModel();
            result.AccessToken = "ok";
            return result;
        }
    }
}
