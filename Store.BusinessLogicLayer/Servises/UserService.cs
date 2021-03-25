using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Store.BusinessLogicLayer.Models.Users;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task AddUserToRoleAsync(UserUpdateModel updateModel)
        {
            var user = await _userManager.FindByIdAsync(updateModel.id.ToString());
            if (user is null)
            {
                throw new CustomExeption(Constants.Constants.Error.ADD_USER_TO_ROLE_FAILD_NO_USER_ID_IN_DB,
                    StatusCodes.Status400BadRequest);
            }

            var roleAddingResult = await _userManager.AddToRoleAsync(user, updateModel.Role);

            if (!roleAddingResult.Succeeded)
            {
                throw new CustomExeption(Constants.Constants.Error.ADD_USER_TO_ROLE_FAILD_ROLE_IS_NOT_PROVIDED,
                    StatusCodes.Status400BadRequest);
            }
        }

        public async Task<IList<string>> GetUserRoleAsync(UserUpdateModel updateModel)
        {
            var user = await _userManager.FindByIdAsync(updateModel.id.ToString());
            if (user is null)
            {
                throw new CustomExeption(Constants.Constants.Error.GET_USER_ROLE_FAILD_NO_USER_ID_IN_DB,
                    StatusCodes.Status400BadRequest);
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            if (!userRoles.Any())
            {
                throw new CustomExeption(Constants.Constants.Error.ERROR_NO_USERROLE,
                    StatusCodes.Status400BadRequest);

            }

            return userRoles;

        }
        public async Task<bool> IsUserInRoleAsync(UserUpdateModel updateModel)
        {
            var user = await _userManager.FindByIdAsync(updateModel.id.ToString());
            if (user is null)
            {
                throw new CustomExeption(Constants.Constants.Error.IS_USER_IN_ROLE_FAILD_NO_USER_ID_IN_DB,
                    StatusCodes.Status400BadRequest);
            }

            var isUserInRole = _userManager.IsInRoleAsync(user, updateModel.Role);

            return true;
        }

        public async Task UserBlockStatusChangingAsync(UserUpdateModel updateModel)
        {
            var user = await _userManager.FindByIdAsync(updateModel.id.ToString());
            if (user is null)
            {
                throw new CustomExeption(Constants.Constants.Error.BLOCKING_USER_FAILD_NO_USER_ID_IN_DB,
                    StatusCodes.Status400BadRequest);
            }

            user.IsBlocked = !user.IsBlocked;
            var blockRersult = await _userManager.UpdateAsync(user);

            if (!blockRersult.Succeeded)
            {
                throw new Exception($"{Constants.Constants.Error.BLOCKING_USER_FAILD_NO_USER_ID_IN_DB}" +
                    $" {StatusCodes.Status500InternalServerError}");
            }
        }

        public async Task UserDeleteAsync(UserUpdateModel updateModel)
        {
            var user = await _userManager.FindByIdAsync(updateModel.id.ToString());
            if (user is null)
            {
                throw new CustomExeption(Constants.Constants.Error.DELETE_USER_FAILD_NO_USER_ID_IN_DB,
                    StatusCodes.Status400BadRequest);
            }

            var delResult = await _userManager.DeleteAsync(user);

            if (!delResult.Succeeded)
            {
                throw new Exception($"{Constants.Constants.Error.DELETE_USER_FAILD_CONTACT_ADMIN}" +
                    $" {StatusCodes.Status500InternalServerError}");
            }
        }

        public async Task UserUpdateAsync(UserUpdateModel updateModel)
        {
            var user = await _userManager.FindByIdAsync(updateModel.id.ToString());
            if (user is null)
            {
                throw new CustomExeption(Constants.Constants.Error.UPDATE_USER_FAILD_USER_NOT_FOUND,
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
                throw new Exception($"{Constants.Constants.Error.UPDATE_USER_FAILD_CONTACT_ADMIN}" +
                    $" {StatusCodes.Status500InternalServerError}");
            }
        }
    }
}
