using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models.Users;
using Store.BusinessLogicLayer.Serviсes.Interfaces;
using System.Threading.Tasks;
using Store.Sharing.Constants;

namespace Store.PresentationLayer.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class AdminProfileController : Controller
    {
        private readonly IUserService _userService;

        public AdminProfileController(IUserService userService)
        {
            _userService = userService;
        }//TODO add new line
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetProfile()
        {
            var accessToken = HttpContext.Request.Cookies[Constants.AreaConstants.JWT];
            var result = await _userService.GetUserByIdAsync(accessToken);
            return View(Constants.AreaConstants.VIEW_PROFILE, result);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProfile()
        {
            var accessToken = HttpContext.Request.Cookies[Constants.AreaConstants.JWT];
            var result = await _userService.GetUserByIdAsync(accessToken);
            return View(Constants.AreaConstants.VIEW_PROFILE_UPDATE, result);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProfile(UserUpdateModel updateModel)
        {
            var accessToken = HttpContext.Request.Cookies[Constants.AreaConstants.JWT];
            await _userService.UserUpdateAsync(updateModel, accessToken);
            return RedirectToAction(Constants.AreaConstants.ACTION_GETPROFILE);
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            if (ModelState.IsValid)
            {
                var accessToken = HttpContext.Request.Cookies[Constants.AreaConstants.JWT];
                await _userService.ChangePasswordAsync(changePasswordModel, accessToken);
                return RedirectToAction(Constants.AreaConstants.ACTION_SIGN_IN, Constants.AreaConstants.CONTROLLER_ADMIN_ACCount);
            }
            return View();
        }
    }
}
