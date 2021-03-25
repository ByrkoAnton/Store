using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("UserBlock")]
        public async Task<IActionResult> UserBlock(UserUpdateModel updateModel)
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
    }
}
