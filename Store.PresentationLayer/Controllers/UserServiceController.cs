using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models.RequestModel;
using Store.BusinessLogicLayer.Models.Users;
using Store.BusinessLogicLayer.Servises.Interfaces;
using System.Threading.Tasks;

namespace Store.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserServiceController : Controller
    {
        private readonly IUserService _userService;
        public UserServiceController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("UserUpdate")]
        public async Task<IActionResult> UserUpdate([FromBody] UserUpdateModel updateModel)
        {
            await _userService.UserUpdateAsync(updateModel);
            return Ok();
        }

        [HttpPost("UserDelete")]
        public async Task<IActionResult> UserDelete(UserUpdateModel updateModel)
        {
            await _userService.UserDeleteAsync(updateModel);
            return Ok();
        }

        [HttpPost("UserBlockUnblok")]
        public async Task<IActionResult> UserBlockUnblok(UserUpdateModel updateModel)
        {
            await _userService.UserBlockStatusChangingAsync(updateModel);
            return Ok();
        }

        [HttpPost("GetUserRole")]
        public async Task<IActionResult> GetUserRole([FromBody] UserUpdateModel updateModel)
        {
            var result = await _userService.GetUserRoleAsync(updateModel);
            return Ok(result);
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
            return Ok(result);
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers(UserFiltrationModel model, string prop = "LastName",
            int page = 1, int pageSize = 5, bool asc = true)
        {  
            var result = await _userService.GetUsersAsync(model, prop, page, pageSize, asc);
            return Ok(result);
        }

        [HttpGet("GetFiltratedUsers")]
        public async Task<IActionResult> GetFiltratedUsers(UserFiltrationModel model)
        {
            var result = await _userService.GetFiltratedUsers(model);
            return Ok(result);
        }
    }
}
