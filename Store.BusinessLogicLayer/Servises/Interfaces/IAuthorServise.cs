using Store.BusinessLogicLayer.Models.Authors;
using Store.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.Interfaces
{
    public interface IAuthorServise
    {
        public Task CreateAsync(AuthorModel model);
        public Task<AuthorModel> GetByIdAsync(long id);
  
        public Task<IEnumerable<AuthorModel>> GetAllAsync();
        public Task<AuthorModel> GetByNameAsync(AuthorModel model);
        public Task RemoveAsync(AuthorModel model);
        public Task UpdateAsync(AuthorModel model);
    }
}
