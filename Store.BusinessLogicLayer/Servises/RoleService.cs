using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Store.BusinessLogicLayer.Models.Users;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.Sharing.Constants;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole<long>> _roleManager;
        
        public RoleService(RoleManager<IdentityRole<long>> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task AddNewRoleToRolesTable(UserRoleModel roleModel)
        {
            var role = await _roleManager.FindByNameAsync(roleModel.RoleName);
            if (role != null)
            {
                throw new CustomExeption(Constants.Error.ROLE_ALREADY_EXISTS,
                   StatusCodes.Status400BadRequest);
            }

            await _roleManager.CreateAsync(new IdentityRole<long>(roleModel.RoleName));

        }
    }
}
