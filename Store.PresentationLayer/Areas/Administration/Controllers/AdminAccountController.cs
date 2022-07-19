using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models;
using Store.BusinessLogicLayer.Serviсes.Interfaces;
using System.Threading.Tasks;
using Store.Sharing.Constants;
using Store.BusinessLogicLayer.Models.Users;

namespace Store.PresentationLayer.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class AdminAccountController : Controller
    {

        private readonly IUserAccountService _accountService;

        public AdminAccountController(IUserAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInModel signInModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.SignInAsync(signInModel);
                Response.Cookies.Append(Constants.AreaConstants.JWT, result.AccessToken);
                return RedirectToAction(Constants.AreaConstants.ACTION_NAVIGATION);
            }
            return View();
        }

        [Authorize (Roles = "admin")]
        [HttpGet]
        public IActionResult Navigation()
        {
            return View();
        }
    }
}
