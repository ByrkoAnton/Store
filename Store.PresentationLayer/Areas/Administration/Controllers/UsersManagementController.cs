using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models.Users;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.Sharing.Constants;
using System.Threading.Tasks;

namespace Store.PresentationLayer.Areas.Administration.Controllers
{
    [Area("Administration")]

    public class UsersManagementController : Controller
    {
        private readonly IUserService _userService;

        public UsersManagementController(IUserService userService)
        {
            _userService = userService; 
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUsers(string sortBy=Constants.AreaConstants.USER_DEF_SORT_PARAMS, bool isAsc = true, int page = Constants.AreaConstants.FIRST_PAGE)
        {
            var sortModel = new UserFiltrationModel {
                PropertyForSort = sortBy,
                IsAscending = isAsc,
                LastName = HttpContext.Request.Cookies[Constants.AreaConstants.USER_L_NAME_COOKIES],
                FirstName = HttpContext.Request.Cookies[Constants.AreaConstants.USER_F_NAME_COOKIES],
                Email = HttpContext.Request.Cookies[Constants.AreaConstants.USER_EMAIL_COOKIES],
                CurrentPage = page,
            };
            var result = await _userService.GetUsersAsync(sortModel);
           
            return View(Constants.AreaConstants.VIEW_USERS, result);
            
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUsers(UserFiltrationModel model)
        {

            var result = await _userService.GetUsersAsync(model);
            return View(Constants.AreaConstants.VIEW_USERS, result);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeStatus(long id, string url)
        {

            await _userService.UserBlockStatusChangingAsync(new UserUpdateModel() {Id = id });
            return Redirect(url);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProfile(string id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            return View(Constants.AreaConstants.VIEW_PROFILE_UPDATE, result);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProfile(UserUpdateModel updateModel)
        {
            var qwery = HttpContext.Request.Headers[Constants.AreaConstants.PATH].ToString();
           
            await _userService.UserUpdateAsync(updateModel, updateModel.Id.ToString());
            return Redirect(qwery);    
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ResetPassword(string id)
        {
            await _userService.ResetPasswordByAdminAsync(id);
            return Ok(Constants.AreaConstants.RESET_PESSWORD_MSG);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(long id, string url)
        {
            await _userService.UserDeleteAsync( id );
            return Redirect(url);
        }


    }
}
