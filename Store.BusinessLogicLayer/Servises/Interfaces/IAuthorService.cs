using Store.BusinessLogicLayer.Models.Authors;
using Store.BusinessLogicLayer.Models.PaginationsModels;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.Interfaces
{
    public interface IAuthorService
    {
        public Task CreateAsync(AuthorModel model);
        public Task<AuthorModel> GetByIdAsync(long id);
        public Task<NavigationModel<AuthorModel>> GetAsync(AuthorFiltrPagingSortModel model);
        public Task<AuthorModel> GetByNameAsync(AuthorModel model);
        public Task RemoveAsync(AuthorModel model);
        public Task UpdateAsync(AuthorModel model);
    }
}
