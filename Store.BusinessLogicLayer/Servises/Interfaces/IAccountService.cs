using Store.BusinessLogicLayer.Models;
using Store.BusinessLogicLayer.Models.Users;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace Store.BusinessLogicLayer.Servises.Interfaces
{
    public interface IAccountService
    {
        public Task<TokenResponseModel> SignInAsync(UserModel userModel);
        //public Task RegisterAsync(UserModel userModel);
 
    }
}
