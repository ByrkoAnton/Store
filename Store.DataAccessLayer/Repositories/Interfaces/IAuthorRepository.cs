using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Models.FiltrationModels;
using Store.DataAccessLayer.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.Interfaces
{
    public interface IAuthorRepository : IBaseRepository<Author>
    {
        public Task<(IEnumerable<Author>, int)> GetAsync(AuthorFiltrPagingSortModelDAL model);
        public Task<Author> GetByIdAsync(long id);
        public Task<IEnumerable<Author>> GetByNameAsync(string name);
    }
}
