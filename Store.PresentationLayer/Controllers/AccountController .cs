using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.BusinessLogicLayer.Models;
using Store.BusinessLogicLayer;

namespace Store.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("register")]
        public async Task<IActionResult> Register()
        {
            return Ok("*ok*");
        }

        [HttpPost("signIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInModel signInModel)
        {
            var res = await _accountService.SignInAsync(signInModel);

            return Ok(res);
        }
    }
}