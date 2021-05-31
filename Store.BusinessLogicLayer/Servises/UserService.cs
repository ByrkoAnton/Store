using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.BusinessLogicLayer.Models.RequestModel;
using Store.BusinessLogicLayer.Models.Users;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Store.BusinessLogicLayer.Models.PaginationsModels;
using Store.Sharing.Constants;
using clearwaterstream.Security;
using Store.DataAccessLayer.Extentions;
using Store.DataAccessLayer.Repositories.Interfaces;

namespace Store.BusinessLogicLayer.Servises
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailProvider _emailService;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(UserManager<User> userManager, IEmailProvider emailService, IMapper mapper, IUserRepository userRepository)
        {
            _userManager = userManager;
            _emailService = emailService;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task AddUserToRoleAsync(UserUpdateModel updateModel)
        {
            if (updateModel is null)
            {
                throw new CustomExeption(Constants.Error.WRONG_MODEL,
                    StatusCodes.Status400BadRequest);
            }

            if (updateModel.Id == Constants.Variables.WRONG_ID)
            {
                throw new CustomExeption(Constants.Error.WRONG_MODEL,
                    StatusCodes.Status400BadRequest);
            }

            var user = await _userManager.FindByIdAsync(updateModel.Id.ToString());
            if (user is null)
            {
                throw new CustomExeption(Constants.Error.ADD_TO_ROLE_FAILD_NO_USER,
                    StatusCodes.Status400BadRequest);
            }

            var roleAddingResult = await _userManager.AddToRoleAsync(user, updateModel.Role);

            if (!roleAddingResult.Succeeded)
            {
                throw new CustomExeption(Constants.Error.ROLE_IS_NOT_PROVIDED,
                    StatusCodes.Status400BadRequest);
            }
        }
         
        public async Task UserBlockStatusChangingAsync(UserUpdateModel updateModel)
        {
            if(updateModel is null)
            {
                throw new CustomExeption(Constants.Error.WRONG_MODEL,
                    StatusCodes.Status400BadRequest);
            }

            if (updateModel.Id == Constants.Variables.WRONG_ID)
            {
                throw new CustomExeption(Constants.Error.WRONG_MODEL,
                    StatusCodes.Status400BadRequest);
            }

            var user = await _userManager.FindByIdAsync(updateModel.Id.ToString());
            if (user is null)
            {
                throw new CustomExeption(Constants.Error.BLOCKING_FAILD_NO_USER,
                    StatusCodes.Status400BadRequest);
            }

            user.IsBlocked = !user.IsBlocked;
            var blockRersult = await _userManager.UpdateAsync(user);

            if (!blockRersult.Succeeded)
            {
                throw new Exception($"{Constants.Error.BLOCKING_FAILD_NO_USER}" +
                    $" {StatusCodes.Status500InternalServerError}");
            }
        }
        public async Task DeleteAllBlockedUserAsync()
        {
            await _userRepository.RemoveRangeAsync(_userManager.Users.Where(i => i.IsBlocked).ToList());
        }
        public async Task UserDeleteAsync(UserUpdateModel updateModel)
        {
            if (updateModel is null)
            {
                throw new CustomExeption(Constants.Error.WRONG_MODEL,
                    StatusCodes.Status400BadRequest);
            }

            if (updateModel.Id == Constants.Variables.WRONG_ID)
            {
                throw new CustomExeption(Constants.Error.WRONG_MODEL,
                    StatusCodes.Status400BadRequest);
            }

            var user = await _userManager.FindByIdAsync(updateModel.Id.ToString());
            if (user is null)
            {
                throw new CustomExeption(Constants.Error.DELETE_FAILD_NO_USER,
                    StatusCodes.Status400BadRequest);
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception($"{Constants.Error.DELETE_USER_FAILD}" +
                    $" {StatusCodes.Status500InternalServerError}");
            }
        }
        public async Task UserUpdateAsync(UserUpdateModel updateModel)
        {
            if (updateModel is null)
            {
                throw new CustomExeption(Constants.Error.WRONG_MODEL,
                    StatusCodes.Status400BadRequest);
            }

            if (updateModel.Id == Constants.Variables.WRONG_ID)
            {
                throw new CustomExeption(Constants.Error.WRONG_MODEL,
                    StatusCodes.Status400BadRequest);
            }

            var user = await _userManager.FindByIdAsync(updateModel.Id.ToString());
            if (user is null)
            {
                throw new CustomExeption(Constants.Error.USER_NOT_FOUND,
                    StatusCodes.Status400BadRequest);
            }

            if (!string.IsNullOrWhiteSpace(updateModel.Email))
            {
                user.Email = updateModel.Email;
            }

            if (!string.IsNullOrWhiteSpace(updateModel.FirstName))
            {
                user.FirstName = updateModel.FirstName;
            }

            if (!string.IsNullOrWhiteSpace(updateModel.LastName))
            {
                user.LastName = updateModel.FirstName;
            }

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                throw new Exception($"{Constants.Error.CONTACT_ADMIN} {StatusCodes.Status500InternalServerError}");
            }
        }
        public async Task<string> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
            if (user is null)
            {
                throw new CustomExeption(Constants.Error.PASSWORD_RESET_FAILD_NO_USER,
                    StatusCodes.Status400BadRequest);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                throw new CustomExeption(Constants.Error.PASSWORD_RESET_FAILD_NO_USER,
                    StatusCodes.Status400BadRequest);
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
            String.Format(Constants.User.RESET_PASSWORD_MSG, newPassword));

            return Constants.User.CHECK_MSG;
        }
        public async Task<NavigationModel<UserModel>> GetUsersAsync(UserFiltrationModel model)
        {
            var propertyForSort = typeof(User).GetProperty(model.PropertyForSort);

            if (propertyForSort is null)
            {
                throw new CustomExeption(Constants.Error.NO_ANY_PROP_NAME, StatusCodes.Status400BadRequest);
            }

            var users = await _userManager.Users
                .Where(n => model.Id == null || n.Id == model.Id)
                .Where(n => EF.Functions.Like(n.LastName, $"%{model.LastName}%"))
                .Where(n => EF.Functions.Like(n.FirstName, $"%{model.FirstName}%"))
                .Where(n => model.IsBlocked == null || n.IsBlocked == model.IsBlocked)
                .OrderBy(propertyForSort, model.IsAscending)
                .Skip((model.CurrentPage - Constants.PaginationParams.FIX_PAGINATION) * model.PageSize)
                .Take(model.PageSize).ToListAsync();

            if (!users.Any())
            {
                throw new CustomExeption(Constants.Error.NO_USER_THIS_CONDITIONS,
                   StatusCodes.Status400BadRequest);
            }

            int usersCount = await _userManager.Users
                .Where(n => model.Id == null || n.Id == model.Id)
                .Where(n => EF.Functions.Like(n.LastName, $"%{model.LastName}%"))
                .Where(n => EF.Functions.Like(n.FirstName, $"%{model.FirstName}%"))
                .Where(n => n.IsBlocked == model.IsBlocked || model.IsBlocked == null).CountAsync();

            var userModels = _mapper.Map<IEnumerable<UserModel>>(users).ToList();

            PaginatedPageModel paginatedPage = new PaginatedPageModel(usersCount, model.CurrentPage, model.PageSize);
            NavigationModel<UserModel> navigation = new NavigationModel<UserModel>
            {
                PageModel = paginatedPage,
                Models = userModels
            };
            return navigation;
        }
    }
}

