using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.AppContext;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Models.FiltrationModels;
using Store.DataAccessLayer.Repositories.Base;
using Store.DataAccessLayer.Repositories.Interfaces;
using Store.Sharing.Constants;
using System.Collections.Generic;
using System.Linq;
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

        public bool IsAuthorsInDb(List<long> id)
        {
            var res = id.All(x => _dbSet.Select(d => d.Id).Contains(x));
            return res;
        }

        public async Task<(IEnumerable<Author>, int)> GetAsync(AuthorFiltrationModelDAL model)
        {
            var authors = await _dbSet.Include(n => n.PrintingEditions).AsNoTracking()
            .Where(n => model.Id == null || n.Id == model.Id)
            .Where(n => EF.Functions.Like(n.Name, $"%{model.Name}%"))
            .Where(n => string.IsNullOrEmpty(model.EditionDescription)
            || n.PrintingEditions.Any(t => EF.Functions.Like(t.Description, $"%{model.EditionDescription}%")))
            .OrderBy($"{model.PropertyForSort} {(model.IsAscending ? Constants.SortingParams.SORT_ASC : Constants.SortingParams.SORT_DESC)}")
            .Skip((model.CurrentPage - Constants.PaginationParams.DEFAULT_OFFSET) * model.PageSize)
            .Take(model.PageSize).ToListAsync();

            int count = await _dbSet
            .Where(n => model.Id == null || n.Id == model.Id)
            .Where(n => EF.Functions.Like(n.Name, $"%{model.Name}%"))
            .Where(n => string.IsNullOrEmpty(model.EditionDescription)
            || n.PrintingEditions.Any(t => EF.Functions.Like(t.Description, $"%{model.EditionDescription}%")))
            .CountAsync();

            var AuthorsWithCount = (authors: authors, count: count);

            return AuthorsWithCount;
        }
        public async Task<Author> GetByNameAsync(string name)
        {
            //var result = await _dbSet.Where(Authors => Authors.Name == name)
            //   .Include(author => author.PrintingEditions).AsNoTracking().ToListAsync();

            var result = await _dbSet.Include(author => author.PrintingEditions).AsNoTracking().FirstOrDefaultAsync(Authors => Authors.Name == name);   

            return result;
        }
        public override async Task UpdateAsync(Author author)
        {
            var editions = new List<PrintingEdition>(author.PrintingEditions);
            author.PrintingEditions.Clear();
            _dbSet.Update(author);
            var authorForUpdate = _dbSet.Include(a => a.PrintingEditions).FirstOrDefault(a => a.Id == author.Id);
            authorForUpdate.PrintingEditions.RemoveAll(e => !editions.Exists(pe => pe.Id == e.Id));
            var result = editions.Where(e => !authorForUpdate.PrintingEditions.Exists(pe => pe.Id == e.Id)).ToList();
            authorForUpdate.PrintingEditions.AddRange(result);
            _dbSet.Update(authorForUpdate);
            await SaveChangesAsync();
        }
    }
}
