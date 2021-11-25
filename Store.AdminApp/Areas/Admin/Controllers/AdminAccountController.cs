using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models;
using Store.BusinessLogicLayer.Servises.Interfaces;
using System.Threading.Tasks;

namespace AdminApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminAccountController : Controller
    {

        //private readonly IUserAccountService _accountService;

        //public AdminAccountController(IUserAccountService accountService)
        //{
        //    //_accountService = accountService;
        //}

        //[HttpPost("signIn")]
        //public async Task<IActionResult> SignIn(UserSignInModel signInModel)
        //{
        //    //var result = await _accountService.SignInAsync(signInModel);

        //    return Ok();

        //}
    }
}
