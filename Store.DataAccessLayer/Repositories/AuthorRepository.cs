using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.AppContext;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Models.FiltrationModels;
using Store.DataAccessLayer.Repositories.Base;
using Store.DataAccessLayer.Repositories.Interfaces;
using Store.Sharing.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Store.DataAccessLayer.Repositories
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(ApplicationContext context) : base(context)
        {
        }
        public async Task<Author> GetByIdAsync(long id)
        {
            var result = await _dbSet.Include(author => author.PrintingEditions).AsNoTracking().FirstOrDefaultAsync(Authors => Authors.Id == id);
            return result;
        }

        public async Task<(IEnumerable<Author>, int)> GetAsync(AuthorFiltrPagingSortModelDAL model)
        {
            var authors = await _dbSet.Include(n => n.PrintingEditions).AsNoTracking()
            .Where(n => EF.Functions.Like(n.Id.ToString(), $"%{model.Id}%"))
            .Where(n => EF.Functions.Like(n.Name, $"%{model.Name}%"))
            .Where(n => string.IsNullOrEmpty(model.EditionDescription)
            || n.PrintingEditions.Any(t => EF.Functions.Like(t.Description, $"%{model.EditionDescription}%")))
            .OrderBy($"{model.PropertyForSort} " +
            $"{(model.IsAscending ? Constants.SortingParams.SORT_ASC_DIRECTION : Constants.SortingParams.SORT_DESC_DIRECTION)}")
            .Skip((model.CurrentPage - 1) * model.PageSize)
            .Take(model.PageSize).ToListAsync();

            int count = await _dbSet.Where(n => EF.Functions.Like(n.Id.ToString(), $"%{model.Id}%"))
            .Where(n => EF.Functions.Like(n.Name, $"%{model.Name}%"))
            .Where(n => string.IsNullOrEmpty(model.EditionDescription)
            || n.PrintingEditions.Any(t => EF.Functions.Like(t.Description, $"%{model.EditionDescription}%")))
            .CountAsync();

            var AuthorsWithCount = (authors: authors, count: count);

            return AuthorsWithCount;
        }
        public async Task<IEnumerable<Author>> GetByNameAsync(string name)
        {
            var result = await _dbSet.Where(Authors => Authors.Name == name)
                .Include(author => author.PrintingEditions).AsNoTracking().ToListAsync();
            return result;
        }
        public override async Task UpdateAsync(Author author)
        {
            var editions = new List<PrintingEdition>(author.PrintingEditions);
            author.PrintingEditions.Clear();
            _dbSet.Update(author);
            var authorForUpdate = _dbSet.Include(a => a.PrintingEditions).FirstOrDefault(b => b.Id == author.Id);
            authorForUpdate.PrintingEditions.RemoveAll(p => !editions.Exists(p2 => p2.Id == p.Id));
            var result = editions.Where(p => !authorForUpdate.PrintingEditions.Exists(p2 => p2.Id == p.Id)).ToList();
            authorForUpdate.PrintingEditions.AddRange(result);
            //authors.RemoveAll(p => !editionForUpdate.Authors.Exists(p2 => p2.Id == p.Id));
            //editionForUpdate.Authors.AddRange(authors);
            _dbSet.Update(authorForUpdate);
            await SaveChangesAsync();
        }
    }
}
