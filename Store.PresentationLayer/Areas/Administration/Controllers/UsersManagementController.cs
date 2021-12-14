using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models.Users;
using Store.BusinessLogicLayer.Servises.Interfaces;
using System.Threading.Tasks;

namespace Store.PresentationLayer.Areas.Administration.Controllers
{
    [Area("Administration")]

    public class UsersManagementController : Controller
    {
        private readonly IUserService _userService;

        public UsersManagementController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUsers(UserFiltrationModel model)
        {
            var result = await _userService.GetUsersAsync(model);
            return View("Users", result);
        }
    }
}
