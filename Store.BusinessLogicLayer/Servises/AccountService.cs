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

namespace Store.BusinessLogicLayer.Servises
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IJwtProvider _jwtProvider;
        private readonly IEmailProvider _emailService;
        private readonly IMapper _mapper;


        public AccountService(SignInManager<User> signInManager, UserManager<User> userManager, IJwtProvider jwtProvider,
            IEmailProvider emailService, IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtProvider = jwtProvider;
            _emailService = emailService;
            _mapper = mapper;
        }
        public async Task<string> SignUpAsync(UserModel signUpModel)
        {
            var EmailCheck = await _userManager.FindByEmailAsync(signUpModel.Email);
            if (EmailCheck != null)
            {
                throw new CustomExeption(Constants.Error.REGISRATION_FAILD_THIS_EMAIL_IS_ALREADY_IN_USE,
                    StatusCodes.Status400BadRequest);
            }

            User user = _mapper.Map<User>(signUpModel);

            IdentityResult result = await _userManager.CreateAsync(user, signUpModel.Password);

            if (!result.Succeeded)
            {
                throw new CustomExeption(Constants.Error.REGISRATION_FAILD_USER_DID_NOT_CREATED,
                   StatusCodes.Status400BadRequest);
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user, Constants.UserConstants.ROLE_USER);

            if (!addToRoleResult.Succeeded)
            {
                throw new CustomExeption(Constants.Error.USER_ROLE_DID_NOT_ADDED,
                   StatusCodes.Status400BadRequest);
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var callbackUrl = new UriBuilder(Constants.URLs.URL_CONFIRMEMAIL);
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters.Add(Constants.UserConstants.EMAIL, user.Email);
            parameters.Add(Constants.UserConstants.CODE, code);
            callbackUrl.Query = parameters.ToString();
            Uri finalUrl = callbackUrl.Uri;

            await _emailService.SendEmailAsync(user.Email, Constants.UserConstants.CONFIRM_EMAIL,
            String.Format(Constants.UserConstants.CONFIRM_REGISRT_LINK, finalUrl));

            return Constants.UserConstants.REGISTR_SUCCESS;
        }
        public async Task<TokenResponseModel> SignInAsync(UserSignInModel signInModel)
        {
            var user = await _userManager.FindByNameAsync(signInModel.Email);
            if (user is null)
            {
                throw new CustomExeption(Constants.Error.LOGIN_FAILD_NO_USER_WITH_THIS_EMAIL,
                    StatusCodes.Status400BadRequest);
            }

            var signIn = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false);

            if (!signIn.Succeeded)
            {
                throw new CustomExeption(Constants.Error.LOGIN_FAILD_WRONG_PASSWORD, StatusCodes.Status400BadRequest);
            }

            var roleList = await _userManager.GetRolesAsync(user);

            if (!roleList.Any())
            {
                throw new Exception($"{Constants.Error.ERROR_NO_USERROLE} {StatusCodes.Status500InternalServerError}");
            }

            var result = new TokenResponseModel();
            
            result.AccessToken = _jwtProvider.GenerateJwt(signInModel.Email, roleList.ToList());
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
                throw new Exception(Constants.Error.EMAILCONFIRMATION_USER_NOT_FOUND);
            }

            var result = await _userManager.ConfirmEmailAsync(user, emailConfirmationModel.Code);

            if (!result.Succeeded)
            {
                throw new Exception(Constants.Error.EMAILCONFIRMATION_EMAIL_DID_NOT_CONFIRMED);
            }

            return Constants.UserConstants.EMAIL_CONFIRMED;
        }
    }
}
