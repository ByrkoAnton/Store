using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public Task RemoveRangeAsync(List<User> users);
    }
}
