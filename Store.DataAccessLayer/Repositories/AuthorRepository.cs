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
        public override async Task<Author> GetByIdAsync(Expression<Func<Author, bool>> predicate)
        {
            var result = await _dbSet.Include(author => author.PrintingEditions).AsNoTracking().FirstOrDefaultAsync(predicate);
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
        public override async Task<IEnumerable<Author>> GetAsync(Expression<Func<Author, bool>> predicate)
        {
            var result = await _dbSet.Where(predicate).Include(author => author.PrintingEditions).AsNoTracking().ToListAsync();
            return result;
        }
        public override async Task UpdateAsync(Author author)
        {
            _dbSet.Update(author);
            List<PrintingEdition> printingEditions = new List<PrintingEdition>(author.PrintingEditions);

            var authorForUpdate = _dbSet.Include(a => a.PrintingEditions).
                FirstOrDefault(Authors => Authors.Id == author.Id);

            authorForUpdate.PrintingEditions.RemoveAll(p => !printingEditions.Exists(p2 => p2.Id == p.Id));
            var result = printingEditions.Where(p => !authorForUpdate.PrintingEditions.Exists(p2 => p2.Id == p.Id)).ToList();
            authorForUpdate.PrintingEditions.AddRange(result);
            await SaveChangesAsync();
        }
    }
}
