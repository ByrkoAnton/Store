using Microsoft.AspNetCore.Mvc;
using Store.BusinessLogicLayer.Models.Users;
using Store.BusinessLogicLayer.Serviсes.Interfaces;
using System.Threading.Tasks;

namespace Store.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("AddNewRoleToTable")]
        public async Task<IActionResult> AddNewRoleToDb([FromBody] UserRoleModel role)
        {
            await _roleService.AddNewRoleAsync(role);
            return Ok();
        }
    }
}
