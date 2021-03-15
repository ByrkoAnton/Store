using Store.BusinessLogicLayer.Models;
using Store.BusinessLogicLayer.Servises.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Store.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Store.BusinessLogicLayer.Providers;
using Microsoft.Extensions.DependencyInjection;
using Store.BusinessLogicLayer.Providers.Interfaces;
using System.Linq;

namespace Store.BusinessLogicLayer.Servises
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IJwtProvider _jwtProvider;
        public AccountService(SignInManager<User> signInManager, UserManager<User> userManager, IJwtProvider jwtProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtProvider = jwtProvider;
        }
        public async Task<TokenResponseModel> SignInAsync(SignInModel signInModel)
        {
            if (signInModel is null)
            {
                throw new CustomExeption(Constants.Constants.Error.LOGIN_FAILD_MODELI_IS_NULL,
                    StatusCodes.Status400BadRequest);
            }

            List<string> extentionsList = new List<string>();

            if (string.IsNullOrWhiteSpace(signInModel.Email))
            {
                extentionsList.Add(Constants.Constants.Error.LOGIN_FAILD_MODEL_IS_NOT_CORECT);
            }

            if (string.IsNullOrWhiteSpace(signInModel.Password))
            {
                extentionsList.Add(Constants.Constants.Error.PASSWORD_FAILD_MODEL_IS_NOT_CORECT);
            }

            if (extentionsList.Any<string>())
            {
                throw new CustomExeption(extentionsList, StatusCodes.Status400BadRequest);
            }

            var user = await _userManager.FindByNameAsync(signInModel.Email);
            if (user is null)
            {
                throw new CustomExeption(Constants.Constants.Error.LOGIN_FAILD_NO_USER_WITH_THIS_EMAIL,
                    StatusCodes.Status400BadRequest);
            }

            var signIn = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false);

            if (!signIn.Succeeded)
            {
                throw new CustomExeption(Constants.Constants.Error.LOGIN_FAILD_WRONG_PASSWORD, StatusCodes.Status400BadRequest);
            }

            
            var roleList = await _userManager.GetRolesAsync(user);
            //List<string> roleList = new List<string>();

            if (roleList is null)
            { 
                throw new System.Exception ($"{Constants.Constants.Error.ERROR_NO_USERROLE} {StatusCodes.Status500InternalServerError}");
            }

            var role = roleList.FirstOrDefault(); 



            var result = new TokenResponseModel();
            result.AccessToken = _jwtProvider.GenerateJwt(signInModel.Email, role);
            return result;
        }

    }
}
