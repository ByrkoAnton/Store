using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.Interfaces
{
    public class AuthorService : IAuhorServise
    {
        private readonly IAuthorRepository _authorRepository;
        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task CreateAsync(Author author)
        {
            await _authorRepository.CreateAsync(author);
        }

        public async Task<Author> FindByIdAsync(long id)
        {
            return await _authorRepository.FindByIdAsync(id);
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _authorRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Author>> GetAsync(Expression<Func<Author, bool>> predicate)////////////////
        {
            return await _authorRepository.GetAsync(predicate);
        }

        public async Task RemoveAsync(Author author)
        {
           await _authorRepository.RemoveAsync(author);
            
        }

        public async Task UpdateAsync(Author author)
        {
            await _authorRepository.UpdateAsync(author);
        }
    }
}
