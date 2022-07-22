using AutoMapper;
using clearwaterstream.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.BusinessLogicLayer.Models.PaginationsModels;
using Store.BusinessLogicLayer.Models.RequestModel;
using Store.BusinessLogicLayer.Models.Users;
using Store.BusinessLogicLayer.Providers.Interfaces;
using Store.BusinessLogicLayer.Serviсes.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Extensions;
using Store.DataAccessLayer.Repositories.Interfaces;
using Store.Sharing.Constants;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Serviсes// TODO spelling+++
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailProvider _emailService;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IEmailProvider emailService, IMapper mapper, IUserRepository userRepository)
        {
            _userManager = userManager;
            _emailService = emailService;
            _mapper = mapper;
            _userRepository = userRepository;
            _signInManager = signInManager;
        }

        public async Task AddUserToRoleAsync(UserUpdateModel updateModel)
        {
            if (updateModel is null || updateModel.Id is default(long))
            {
                throw new CustomException(Constants.Error.WRONG_MODEL,
                    HttpStatusCode.BadRequest);
            }

            var user = await _userManager.FindByIdAsync(updateModel.Id.ToString());
            if (user is null)
            {
                throw new CustomException(Constants.Error.ADD_TO_ROLE_FAILD_NO_USER,
                    HttpStatusCode.BadRequest);
            }

            var roleAddingResult = await _userManager.AddToRoleAsync(user, updateModel.Role);

            if (!roleAddingResult.Succeeded)
            {
                throw new CustomException(Constants.Error.ROLE_IS_NOT_PROVIDED,
                    HttpStatusCode.BadRequest);
            }
        }
        public async Task UserBlockStatusChangingAsync(UserUpdateModel updateModel)
        {
            if (updateModel is null || updateModel.Id is default(long))
            {
                throw new CustomException(Constants.Error.WRONG_MODEL,
                    HttpStatusCode.BadRequest);
            }

            var user = await _userManager.FindByIdAsync(updateModel.Id.ToString());
            if (user is null)
            {
                throw new CustomException(Constants.Error.BLOCKING_FAILD_NO_USER,
                     HttpStatusCode.BadRequest);
            }

            user.IsBlocked = !user.IsBlocked;
            var blockResult = await _userManager.UpdateAsync(user);//TODO wrong spelling+++

            if (!blockResult.Succeeded)
            {
                throw new Exception($"{Constants.Error.BLOCKING_FAILD_NO_USER}" +
                    $" {HttpStatusCode.InternalServerError}");
            }
        }
        public async Task DeleteAllBlockedUserAsync()
        {
            await _userRepository.RemoveRangeAsync(_userManager.Users.Where(i => i.IsBlocked).ToList());
        }
        public async Task UserDeleteAsync(long id)
        {
            if (id is default(long))
            {
                throw new CustomException(Constants.Error.WRONG_MODEL,
                     HttpStatusCode.BadRequest);
            }

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                throw new CustomException(Constants.Error.DELETE_FAILD_NO_USER,
                     HttpStatusCode.BadRequest);
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception($"{Constants.Error.DELETE_USER_FAILD}" +
                    $" {StatusCodes.Status500InternalServerError}");
            }
        }
        public async Task<string> UserUpdateAsync(UserUpdateModel updateModel, string authentication)
        {
            if (!long.TryParse(authentication, out long id))
            {
                var jwtTrimmed = authentication.Replace(Constants.JwtProvider.BEARER, string.Empty).Trim();//TODO spelling+++
                var handler = new JwtSecurityTokenHandler().ReadJwtToken(jwtTrimmed);

                id = long.Parse(handler.Claims.Where(a => a.Type == Constants.JwtProvider.ID).FirstOrDefault().Value);
            }

            if (updateModel is null || id is default(long))
            {
                throw new CustomException(Constants.Error.WRONG_MODEL,
                     HttpStatusCode.BadRequest);
            }

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                throw new CustomException(Constants.Error.USER_NOT_FOUND,
                     HttpStatusCode.BadRequest);
            }

            bool isEmailChanging = user.Email != updateModel.Email;

            if (isEmailChanging && await _userManager.FindByEmailAsync(updateModel.Email) is not null)
            {
                throw new CustomException(Constants.Error.EMAIL_EXIST_DB,
                    HttpStatusCode.BadRequest);
            }

            if (!string.IsNullOrWhiteSpace(updateModel.Email))
            {
                user.Email = updateModel.Email;
                user.UserName = updateModel.Email;
            }

            if (!string.IsNullOrWhiteSpace(updateModel.FirstName))
            {
                user.FirstName = updateModel.FirstName;
            }

            if (!string.IsNullOrWhiteSpace(updateModel.LastName))
            {
                user.LastName = updateModel.LastName;
            }

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                throw new Exception($"{Constants.Error.CONTACT_ADMIN} {HttpStatusCode.InternalServerError}");
            }

            return Constants.User.UPDATE_SUCCES;
        }
        public async Task<string> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);

            //if (user is null || !await _userManager.IsEmailConfirmedAsync(user))//TODO legacy code
            //{
            //    throw new CustomException(Constants.Error.PASSWORD_RESET_FAILD_NO_USER,
            //         HttpStatusCode.BadRequest);
            //}

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var newPassword = PasswordGenerator.GeneratePassword(Constants.User.PASSWORD_LENGHT,
                Constants.User.PASSWORD_DIGETS, Constants.User.PASSWORD_SPECIAL_CHARS);
            var result = await _userManager.ResetPasswordAsync(user, code, newPassword);

            if (!result.Succeeded)
            {
                throw new Exception(Constants.Error.PASSWORD_RESET_FAILD);
            }

            await _emailService.SendEmailAsync(user.Email, Constants.User.RESET_PASSWORD_SUBJ,
            String.Format(Constants.User.RESET_PASSWORD_MASSAGE, newPassword));

            return Constants.User.CHECK_MASSAGE;
        }
        public async Task ResetPasswordByAdminAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                throw new CustomException(Constants.Error.PASSWORD_RESET_FAILD_NO_USER,
                     HttpStatusCode.BadRequest);
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var newPassword = PasswordGenerator.GeneratePassword(Constants.User.PASSWORD_LENGHT,
                Constants.User.PASSWORD_DIGETS, Constants.User.PASSWORD_SPECIAL_CHARS);
            var result = await _userManager.ResetPasswordAsync(user, code, newPassword);

            if (!result.Succeeded)
            {
                throw new Exception(Constants.Error.PASSWORD_RESET_FAILD);
            }

            await _emailService.SendEmailAsync(user.Email, Constants.User.RESET_PASSWORD_SUBJ,
            String.Format(Constants.User.RESET_PASSWORD_MASSAGE, newPassword));  //TODO please use just string
        }
        public async Task ChangePasswordAsync(ChangePasswordModel model, string authentication)
        {
            if (!long.TryParse(authentication, out long id))
            {
                var jwtTrimmed = authentication.Replace(Constants.JwtProvider.BEARER, string.Empty).Trim();//TODO spelling+++
                var handler = new JwtSecurityTokenHandler().ReadJwtToken(jwtTrimmed);

                id = long.Parse(handler.Claims.Where(a => a.Type == Constants.JwtProvider.ID).FirstOrDefault().Value);
            }

            if (id is default(long))
            {
                throw new CustomException(Constants.Error.WRONG_MODEL,
                     HttpStatusCode.BadRequest);
            }

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                throw new CustomException(Constants.Error.NO_USER_ID_IN_DB,
                     HttpStatusCode.BadRequest);
            }

            var isCurrentPasswordValid = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);//TODO spelling+++
            if (!isCurrentPasswordValid)
            {
                throw new CustomException(Constants.Error.CURRENT_PASSWORD_WRONG, HttpStatusCode.BadRequest);
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                throw new Exception(Constants.Error.PASSWORD_RESET_FAILD);
            }

            await _signInManager.SignOutAsync();
        }
        public async Task<UserModel> GetUserByIdAsync(string authentication)
        {
            if (!long.TryParse(authentication, out long id))
            {
                var jwtTrimmed = authentication.Replace(Constants.JwtProvider.BEARER, string.Empty).Trim();//TODO spelling+++
                var handler = new JwtSecurityTokenHandler().ReadJwtToken(jwtTrimmed);

                id = long.Parse(handler.Claims.Where(a => a.Type == Constants.JwtProvider.ID).FirstOrDefault().Value);
            }

            if (id is default(long))
            {
                throw new CustomException(Constants.Error.WRONG_MODEL,
                     HttpStatusCode.BadRequest);
            }

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                throw new CustomException(Constants.Error.NO_USER_ID_IN_DB, HttpStatusCode.BadRequest);
            }

            return _mapper.Map<UserModel>(user);
        }
        public async Task<NavigationModelBase<UserModel>> GetUsersAsync(UserFiltrationModel model)
        {
            var propertyForSort = typeof(User).GetProperty(model.PropertyForSort);

            var users = await _userManager.Users
                .Where(n => model.Id == null || n.Id == model.Id)
                .Where(n => EF.Functions.Like(n.LastName, $"%{model.LastName}%"))
                .Where(n => EF.Functions.Like(n.FirstName, $"%{model.FirstName}%"))
                .Where(n => EF.Functions.Like(n.Email, $"%{model.Email}%"))
                .Where(n => model.IsBlocked == null || n.IsBlocked == model.IsBlocked)
                .OrderBy(propertyForSort, model.IsAscending)
                .Skip((model.CurrentPage - Constants.PaginationParams.DEFAULT_OFFSET) * model.PageSize)
                .Take(model.PageSize).ToListAsync();

            int usersCount = default;
            if (users.Any())
            {
                usersCount = await _userManager.Users
                .Where(n => model.Id == null || n.Id == model.Id)
                .Where(n => EF.Functions.Like(n.LastName, $"%{model.LastName}%"))
                .Where(n => EF.Functions.Like(n.FirstName, $"%{model.FirstName}%"))
                .Where(n => EF.Functions.Like(n.Email, $"%{model.Email}%"))
                .Where(n => n.IsBlocked == model.IsBlocked || model.IsBlocked == null).CountAsync();
            }

            var userModels = _mapper.Map<IEnumerable<UserModel>>(users).ToList();

            PaginatedPageModel paginatedPage = new PaginatedPageModel(usersCount, model.CurrentPage, model.PageSize);
            NavigationModelBase<UserModel> navigation = new NavigationModelBase<UserModel>
            {
                PageModel = paginatedPage,
                Models = userModels
            };
            return navigation;
        }
    }
}

