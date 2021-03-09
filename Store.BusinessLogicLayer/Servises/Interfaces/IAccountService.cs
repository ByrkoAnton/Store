
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Store.BusinessLogicLayer.Models;

namespace Store.BusinessLogicLayer.Servises.Interfaces
{
    public interface IAccountService
    {
        public Task<TokenResponseModel> SignInAsync(SignInModel loginModel);
    }
}
