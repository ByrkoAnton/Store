using Store.BusinessLogicLayer.Models;
using Store.BusinessLogicLayer.Servises.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Store.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using Store.BusinessLogicLayer.Providers.Interfaces;
using System.Linq;
using Store.BusinessLogicLayer.Models.Users;
using System;
using System.Web;
using Store.BusinessLogicLayer.Models.RequestModel;
using Store.Sharing.Constants;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Net;

namespace Store.BusinessLogicLayer.Servises
{
    public class UserAccountService : IUserAccountService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ITokenProvider _tokenProvider;
        private readonly IEmailProvider _emailService;
        private readonly IMapper _mapper;

        public UserAccountService(SignInManager<User> signInManager, UserManager<User> userManager, ITokenProvider jwtProvider,
            IEmailProvider emailService, IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenProvider = jwtProvider;
            _emailService = emailService;
            _mapper = mapper;
        }
        public async Task<string> SignUpAsync(UserModel signUpModel)
        {
            var EmailCheck = await _userManager.FindByEmailAsync(signUpModel.Email);
            if (EmailCheck is not null)
            {
                throw new CustomExeption(Constants.Error.REGISRATION_FAILD_THIS_EMAIL_IS_ALREADY_IN_USE,
                    HttpStatusCode.BadRequest);
            }

            User user = _mapper.Map<User>(signUpModel);

            IdentityResult result = await _userManager.CreateAsync(user, signUpModel.Password);

            if (!result.Succeeded)
            {
                throw new CustomExeption(Constants.Error.REGISRATION_USER_NOT_CREATED,
                   HttpStatusCode.BadRequest);
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user, Constants.User.ROLE_USER);

            if (!addToRoleResult.Succeeded)
            {
                throw new CustomExeption(Constants.Error.USER_ROLE_DID_NOT_ADDED,
                   HttpStatusCode.BadRequest);
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var callbackUrl = new UriBuilder(Constants.URLs.URL_CONFIRMEMAIL);
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters.Add(Constants.User.EMAIL, user.Email);
            parameters.Add(Constants.User.CODE, code);
            callbackUrl.Query = parameters.ToString();
            Uri finalUrl = callbackUrl.Uri;

            await _emailService.SendEmailAsync(user.Email, Constants.User.CONFIRM_EMAIL,
            string.Format(Constants.User.CONFIRM_LINK, finalUrl));

            return Constants.User.REGISTR_SUCCESS;
        }
        public async Task<TokenResponseModel> SignInAsync(UserSignInModel signInModel)
        {
            var user = await _userManager.FindByNameAsync(signInModel.Email);
            if (user is null)
            {
                throw new CustomExeption(Constants.Error.LOGIN_FAILD_EMAIL,
                    HttpStatusCode.BadRequest);
            }

            var signIn = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false);

            if (!signIn.Succeeded)
            {
                throw new CustomExeption(Constants.Error.LOGIN_FAILD_WRONG_PASSWORD, HttpStatusCode.BadRequest);
            }

            var roleList = await _userManager.GetRolesAsync(user);

            if (!roleList.Any())
            {
                throw new Exception($"{Constants.Error.NO_USERROLE} {StatusCodes.Status500InternalServerError}");
            }

            var result = new TokenResponseModel();

            result.AccessToken = _tokenProvider.GenerateJwt(signInModel.Email, roleList.ToList(), user.Id.ToString());
            result.RefreshToken = _tokenProvider.GenerateRefreshToken();
            user.RefreshToken = result.RefreshToken;
            await _userManager.UpdateAsync(user);

            return result;
        }
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<string> ConfirmEmailAsync(EmailConfirmationModel emailConfirmationModel)
        {
            var user = await _userManager.FindByEmailAsync(emailConfirmationModel.Email);
            if (user is null)
            {
                throw new Exception(Constants.Error.EMAILCONFIRM_NO_USER);
            }

            var result = await _userManager.ConfirmEmailAsync(user, emailConfirmationModel.Code);

            if (!result.Succeeded)
            {
                throw new Exception(Constants.Error.EMAIL_DID_NOT_CONFIRMED);
            }

            return Constants.User.EMAIL_CONFIRMED;
        }

        public async Task<TokenResponseModel> UpdateTokens(string jwtToken, string refreshToken)
        {
            var handler = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken.Remove(jwtToken.IndexOf(Constants.JwtProvider.BEARER),
                Constants.JwtProvider.BEARER.Length).Trim());
            var id = long.Parse(handler.Claims.Where(a => a.Type == Constants.JwtProvider.ID).FirstOrDefault().Value);

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                throw new CustomExeption(Constants.Error.NO_USER_ID_IN_DB,
                    HttpStatusCode.BadRequest);
            }

            if (refreshToken != user.RefreshToken)
            {
                throw new CustomExeption(Constants.RefreshToken.TOKENS_NOT_EQUALS,
                    HttpStatusCode.BadRequest);
            }
            TokenResponseModel tokenResponseModel = new TokenResponseModel();

            tokenResponseModel.AccessToken = _tokenProvider.GenerateJwt(user.Email, new List<string>(await _userManager.GetRolesAsync(user)),
                user.Id.ToString());
            tokenResponseModel.RefreshToken = _tokenProvider.GenerateRefreshToken();

            user.RefreshToken = tokenResponseModel.RefreshToken;
            await _userManager.UpdateAsync(user);

            return tokenResponseModel;
        }
    }
}
