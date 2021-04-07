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
            var result=await _dbSet.Include(author => author.PrintingEditions).AsNoTracking().FirstOrDefaultAsync(predicate);
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

        public override async Task UpdateAsync(Author autor)
        {
            /////////////// это последний относительно рабочий вариант
            //var printingEditions = new List<PrintingEdition>(autor.PrintingEditions);
            //autor.PrintingEditions.Clear();
            //_dbSet.Update(autor).State = EntityState.Modified;
            //var author = _dbSet.Include(autor => autor.PrintingEditions).FirstOrDefault(Authors => Authors.Id == autor.Id);
            //author.PrintingEditions.AddRange(printingEditions);
            //await _context.SaveChangesAsync();
            //////////////////////////////////////////////// 
            var printingEditions = new List<PrintingEdition>(autor.PrintingEditions);
            autor.PrintingEditions.Clear();
            _dbSet.Update(autor);
            var result = await _dbSet.Include(autor => autor.PrintingEditions).FirstOrDefaultAsync(x => x.Id == autor.Id);
            result.PrintingEditions = printingEditions;
            await _context.SaveChangesAsync();

            //var authors = new List<Author>(item.Authors);
            //item.Authors.Clear();
            //_dbSet.Update(item);
            //var result = await _dbSet.Include(edition => edition.Authors).FirstOrDefaultAsync(x => x.Id == item.Id);
            //result.Authors = authors;
            //await SaveChangesAsync();

            //var printingEditions = new List<PrintingEdition>(autor.PrintingEditions);
            ////_dbSet.Update(autor);
            //var author = _dbSet.Include(autor => autor.PrintingEditions).FirstOrDefault(Authors => Authors.Id == autor.Id);
            //author.PrintingEditions.Clear();
            //_dbSet.Update(author);
            //author.PrintingEditions.AddRange(printingEditions);
            //await _context.SaveChangesAsync();
        }
    }
}
