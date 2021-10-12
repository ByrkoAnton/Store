using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.BusinessLogicLayer.Models;
using Store.BusinessLogicLayer.Models.Users;
using Store.BusinessLogicLayer.Models.RequestModel;

namespace Store.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IUserAccountService _accountService;

        public AccountController(IUserAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp(UserModel signUpModel)
        {
            var result = await _accountService.SignUpAsync(signUpModel);
            return Json(result);
            
        }

        [HttpPost("signIn")]
        public async Task<IActionResult> SignIn([FromBody] UserSignInModel signInModel)
        {
            var result = await _accountService.SignInAsync(signInModel);

            return Ok(result);

        }

        [HttpGet("signOut")]
        public async Task<IActionResult> SignOut()
        {
            await _accountService.SignOutAsync();
            return Ok();
        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] EmailConfirmationModel emailConfirmationModel)
        {
            var result = await _accountService.ConfirmEmailAsync(emailConfirmationModel);
            return Ok(result);
        }

        
        [HttpPost("updateTokens")]
        public async Task<IActionResult> UpdateTokens([FromBody] TokenResponseModel model)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            var result = await _accountService.UpdateTokensAsync(accessToken, model.RefreshToken);
            return Ok(result);
        }
    }
}