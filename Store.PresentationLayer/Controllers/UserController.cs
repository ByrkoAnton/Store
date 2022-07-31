using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models.RequestModel;
using Store.BusinessLogicLayer.Models.Users;
using Store.BusinessLogicLayer.Serviсes.Interfaces;
using System.Threading.Tasks;

namespace Store.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("UserUpdate")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UserUpdate([FromBody] UserUpdateModel updateModel)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            await _userService.UserUpdateAsync(updateModel, accessToken);
            return Ok();
        }

        [HttpPost("UserBlockUnblok")]//TODO spelling
        public async Task<IActionResult> UserBlockUnblok(UserUpdateModel updateModel)//TODO spelling
        {
            await _userService.UserBlockStatusChangingAsync(updateModel);
            return Ok();
        }

        [HttpPost("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole([FromBody] UserUpdateModel updateModel)
        {
            await _userService.AddUserToRoleAsync(updateModel);
            return Ok();
        }
        
        [HttpGet("DeleteAllBlockedUser")]
        public async Task<IActionResult> DeleteAllBlockedUser()
        {
            await _userService.DeleteAllBlockedUserAsync();
            return Ok();
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            var result = await _userService.ForgotPasswordAsync(forgotPasswordModel);
            return Json(result);
        }

        [HttpPost("ChangePassword")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ChangePassword (ChangePasswordModel changePasswordModel)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            await _userService.ChangePasswordAsync(changePasswordModel, accessToken);
            return Ok();
        }
        //TODO extra line


        [HttpPost("GetUsers")]
        public async Task<IActionResult> GetUsers(UserFiltrationModel model)
        {  
            var result = await _userService.GetUsersAsync(model);
            return Ok(result);
        }

        [HttpGet("GetUserById")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserById()
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            var result = await _userService.GetUserByIdAsync(accessToken);
            return Json(result);
        }
    }
}
