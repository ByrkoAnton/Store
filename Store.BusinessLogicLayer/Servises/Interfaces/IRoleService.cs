using System.Threading.Tasks;
using Store.BusinessLogicLayer.Models.Users;

namespace Store.BusinessLogicLayer.Servises.Interfaces//TODO wrong spelling
{
    public interface IRoleService
    {
        public Task AddNewRoleAsync(UserRoleModel roleModel);
    }
}
