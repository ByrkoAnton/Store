using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Models.FiltrationModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Dapper.Interfaces
{
    public interface IAuthorRepositoryDapper
    {
        public Task<(IEnumerable<Author>, int)> GetAsync(AuthorFiltrationModelDAL model);
        public Task<Author> GetByIdAsync(long id);
  
        public Task<Author> GetByNameAsync(string name);
        public Task<List<Author>> GetAuthorsListByNamesListAsync(List<string> names);
        public bool IsAuthorsInDb(List<long> id);
    }
}
