using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Models.FiltrationModels;
using Store.DataAccessLayer.Repositories.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.Interfaces
{
    public interface IAuthorRepository : IBaseRepository<Author>
    {
        public Task<(IEnumerable<Author>, int)> GetAsync(AuthorFiltrationModelDAL model);
        public Task<Author> GetByIdAsync(long id);
        public Task<IEnumerable<Author>> GetByNameAsync(string name);
        public Task<List<Author>> GetAuthorsListByIdListAsync(List<long> id);
        public bool IsAuthorsInDb(List<long> id);
    }
}
