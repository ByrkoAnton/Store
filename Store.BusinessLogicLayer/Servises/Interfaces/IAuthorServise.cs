using Store.BusinessLogicLayer.Models.Authors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.Interfaces
{
    public interface IAuthorServise
    {
        public Task CreateAsync(AuthorModel model);
        public Task<AuthorModel> GetByIdAsync(long id);
        public Task<List<AuthorModel>> GetAllAsync();
        public Task<AuthorModel> GetByNameAsync(AuthorModel model);
        public Task RemoveAsync(AuthorModel model);
        public Task UpdateAsync(AuthorModel model);
    }
}
