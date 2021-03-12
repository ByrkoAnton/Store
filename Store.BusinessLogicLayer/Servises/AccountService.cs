using Store.BusinessLogicLayer.Models;
using Store.BusinessLogicLayer.Servises.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Store.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Store.BusinessLogicLayer.Providers;

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
                throw new CustomExeption("login faild - model is NULL", StatusCodes.Status400BadRequest);
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
                throw new CustomExeption(extentionsList, StatusCodes.Status400BadRequest);
            }

            var user = await _userManager.FindByNameAsync(signInModel.Email);
            if (user is null)
            {
                throw new CustomExeption("login faild - no user with this email", StatusCodes.Status400BadRequest);
            }

            var signIn = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false);

            if (!signIn.Succeeded)
            {
                throw new CustomExeption("login faild - wrong password", StatusCodes.Status400BadRequest);
            }

            var result = new TokenResponseModel();
            result.AccessToken = new JwtProvider(signInModel.Email, "Admin").jwt;
            return result;

            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimsIdentity.DefaultNameClaimType, signInModel.Email),
            //    new Claim(ClaimsIdentity.DefaultRoleClaimType, "Admin")
            //};

            //var identity = new ClaimsIdentity(claims, "SignInAsync", ClaimsIdentity.DefaultNameClaimType,
            //        ClaimsIdentity.DefaultRoleClaimType);

            //var now = DateTime.UtcNow;
            //var jwt = new JwtSecurityToken(
            //        issuer: AuthOptions.ISSUER,
            //        audience: AuthOptions.AUDIENCE,
            //        notBefore: now,
            //        claims: identity.Claims,
            //        expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
            //        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            //var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);                 
        }

    }
}
