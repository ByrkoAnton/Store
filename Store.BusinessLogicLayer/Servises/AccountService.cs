using Store.BusinessLogicLayer.Models;
using Store.BusinessLogicLayer.Servises.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Store.BusinessLogicLayer.Models.Users;
using Store.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Store.BusinessLogicLayer.Servises
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public AccountService(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<TokenResponseModel> SignInAsync(SignInModel signInModel)
        {
            if (signInModel is null)
            {
                throw new CustomExeption("login faild - model is NULL",StatusCodes.Status200OK);
            }

            List<string> extentionsList = new List<string>();

            if (string.IsNullOrWhiteSpace(signInModel.Email))
            {
                extentionsList.Add("login faild - model is not corect");
            }

            if (string.IsNullOrWhiteSpace(signInModel.Password))
            {
                extentionsList.Add("password faild - model is not corect");
            }

            if (extentionsList.Count > 0)
            {
                throw new CustomExeption(extentionsList, StatusCodes.Status200OK);
            }

            var user = await _userManager.FindByNameAsync(signInModel.Email);
            if (user == null)
            {
                throw new CustomExeption("login faild - no user with this email",StatusCodes.Status200OK);
            }

            var signIn = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false);

            if (!signIn.Succeeded)
            {
                throw new CustomExeption("login faild - wrong password", StatusCodes.Status200OK);
            }

            var result = new TokenResponseModel();
            result.AccessToken = "login";
            return result;
        }
    }
}
