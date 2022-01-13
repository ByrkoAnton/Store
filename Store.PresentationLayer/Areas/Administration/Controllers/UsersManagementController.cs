using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models.Users;
using Store.BusinessLogicLayer.Servises.Interfaces;
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
        public async Task<IActionResult> GetUsers(string sortBy="LastName", bool isAsc = true, int page = 1)
        {
            var sortModel = new UserFiltrationModel {
                PropertyForSort = sortBy,
                IsAscending = isAsc,
                LastName = HttpContext.Request.Cookies["lastNameForSearch"],
                FirstName = HttpContext.Request.Cookies["firstNameForSearch"],
                Email = HttpContext.Request.Cookies["emailForSearch"],
                CurrentPage = page,
            };
            var result = await _userService.GetUsersAsync(sortModel);
           
            return View("Users", result);
            
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUsers(UserFiltrationModel model)
        {

            var result = await _userService.GetUsersAsync(model);
            return View("Users", result);
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
            return View("ProfileUpdate", result);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProfile(UserUpdateModel updateModel)
        {
            var qwery = HttpContext.Request.Headers["Referer"].ToString();
           
            await _userService.UserUpdateAsync(updateModel, updateModel.Id.ToString());
            return Redirect(qwery);    
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ResetPassword(string id)
        {
            await _userService.ResetPasswordByAdminAsync(id);
            return Ok("Password reseted successfully");
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
