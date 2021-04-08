using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.AppContext;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Base;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        
        public AuthorRepository(ApplicationContext context) : base(context)
        {
        }
        public override  async Task<Author> GetByIdAsync(Expression<Func<Author, bool>> predicate)
        {
            var result = await _dbSet.Include(author => author.PrintingEditions).AsNoTracking().FirstOrDefaultAsync(predicate);
            return result;
        }

        public override async Task<IEnumerable<Author>> GetAllAsync()
        {
            var result = await _dbSet.Include(author => author.PrintingEditions).ToListAsync();
            return result;
        }
        public override async Task<IEnumerable<Author>> GetAsync(Expression<Func<Author, bool>> predicate)
        {
            var result = await _dbSet.Where(predicate).Include(author => author.PrintingEditions).ToListAsync();
            return result;
        }

        public override async Task UpdateAsync(Author author)
        {
            var authorForUpdate = _dbSet.Include(author => author.PrintingEditions).
                FirstOrDefault(Authors => Authors.Id == author.Id);

            authorForUpdate.PrintingEditions.RemoveAll(p => !author.PrintingEditions.Exists(p2 => p2.Id == p.Id));
            var result = author.PrintingEditions.Where(p => !authorForUpdate.PrintingEditions.Exists(p2 => p2.Id == p.Id)).ToList();
            authorForUpdate.PrintingEditions.AddRange(result);
            _dbSet.Update(authorForUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync(); 
        }
    }
}
