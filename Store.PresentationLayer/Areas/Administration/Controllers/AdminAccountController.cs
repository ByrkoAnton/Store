using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models;
using Store.BusinessLogicLayer.Servises.Interfaces;
using System.Threading.Tasks;

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
            var result = await _accountService.SignInAsync(signInModel);

            return View("Navigation");

        }
    }
}
