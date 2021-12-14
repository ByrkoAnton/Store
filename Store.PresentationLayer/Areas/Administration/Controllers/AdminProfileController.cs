using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models.Users;
using Store.BusinessLogicLayer.Servises.Interfaces;
using System.Threading.Tasks;

namespace Store.PresentationLayer.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class AdminProfileController : Controller
    {
        private readonly IUserService _userService;

        public AdminProfileController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetProfile()
        {
            var accessToken = HttpContext.Request.Cookies["jwt"];
            var result = await _userService.GetUserByIdAsync(accessToken);
            return View("Profile", result);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProfile()
        {
            var accessToken = HttpContext.Request.Cookies["jwt"];
            var result = await _userService.GetUserByIdAsync(accessToken);
            return View("ProfileUpdate", result);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProfile(UserUpdateModel updateModel)
        {
            var accessToken = HttpContext.Request.Cookies["jwt"];
            await _userService.UserUpdateAsync(updateModel, accessToken);
            return RedirectToAction("GetProfile");
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
                var accessToken = HttpContext.Request.Cookies["jwt"];
                await _userService.ChangePasswordAsync(changePasswordModel, accessToken);
                return RedirectToAction("SignIn", "AdminAccount");
            }
            return View();
        }
    }
}
