using System.Threading.Tasks;
using Store.BusinessLogicLayer.Models.Users;

namespace Store.BusinessLogicLayer.Serviсes.Interfaces
{
    public interface IRoleService
    {
        public Task AddNewRoleAsync(UserRoleModel roleModel);
    }
}
