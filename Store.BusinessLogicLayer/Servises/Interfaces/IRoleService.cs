using System.Threading.Tasks;
using Store.BusinessLogicLayer.Models.Users;

namespace Store.BusinessLogicLayer.Servises.Interfaces
{
    public interface IRoleService
    {
        public Task AddNewRoleAsync(UserRoleModel roleModel);
    }
}
