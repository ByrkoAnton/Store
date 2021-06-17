using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.AppContext;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.FiltrationModels;
using Store.DataAccessLayer.Repositories.Base;
using Store.DataAccessLayer.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Store.Sharing.Constants;

namespace Store.DataAccessLayer.Repositories
{
    public class PrintingEditionRepository : BaseRepository<PrintingEdition>, IPrintingEditionRepository
    {
        public PrintingEditionRepository(ApplicationContext context) : base(context)
        {
        }
        public override async Task CreateAsync(PrintingEdition edition)
        {
            List<Author> authors = new List<Author>(edition.Authors);

            edition.Authors.Clear();
            await _dbSet.AddAsync(edition);
            edition.Authors.AddRange(authors);
            await SaveChangesAsync();
        }
        public async Task<PrintingEdition> GetByIdAsync(long id)
        {
            var result = await _dbSet.Include(x => x.Authors).AsNoTracking().FirstOrDefaultAsync(edition => edition.Id == id);
            return result;
        }

        public async Task<List<PrintingEdition>> GetEditionsListByIdListAsync(List<long> id)
        {
            var result = await _dbSet.Where(x => id.Contains(x.Id)).ToListAsync();
            return result;
        }
        public async Task<IEnumerable<PrintingEdition>> GetByTitle(string description)
        {
            var result = await _dbSet.Where(edition => edition.Description == description)
                .Include(pe => pe.Authors).AsNoTracking().ToListAsync();
            return result;
        }
        public async Task<(IEnumerable<PrintingEdition>, int)> GetAsync(EditionFiltrationModelDAL model)
        {
            var editions = await _dbSet.Include(pe => pe.Authors).AsNoTracking()
            .Where(n => model.Id == null || n.Id == model.Id)
            .Where(n => EF.Functions.Like(n.Description, $"%{model.Description}%"))
            .Where(n => EF.Functions.Like(n.Price.ToString(), $"%{model.Price}%"))
            .Where(n => EF.Functions.Like(n.Status, $"%{model.Status}%"))
            .Where(n => n.Currency == model.Currency || model.Currency == null)
            .Where(n => n.Type == model.Type || model.Type == null)
            .Where(n => string.IsNullOrEmpty(model.AuthorName)
            || n.Authors.Any(t => EF.Functions.Like(t.Name, $"%{model.AuthorName}%")))
            .Where(n => n.Price <= model.MaxPrice || model.MaxPrice == null)
            .Where(n => n.Price >= model.MinPrice || model.MinPrice == null)
            .OrderBy($"{model.PropertyForSort} {(model.IsAscending ? Constants.SortingParams.SORT_ASC : Constants.SortingParams.SORT_DESC)}")
            .Skip((model.CurrentPage - Constants.PaginationParams.DEFAULT_OFFSET) * model.PageSize).Take(model.PageSize).ToListAsync();

            int count = await _dbSet
            .Where(n => model.Id == null || n.Id == model.Id)
            .Where(n => EF.Functions.Like(n.Description, $"%{model.Description}%"))
            .Where(n => EF.Functions.Like(n.Price.ToString(), $"%{model.Price}%"))
            .Where(n => EF.Functions.Like(n.Status, $"%{model.Status}%"))
            .Where(n => n.Currency == model.Currency || model.Currency == null)
            .Where(n => n.Type == model.Type || model.Type == null)
            .Where(n => string.IsNullOrEmpty(model.AuthorName)
            || n.Authors.Any(t => EF.Functions.Like(t.Name, $"%{model.AuthorName}%")))
            .Where(n => model.MaxPrice == null || n.Price <= model.MaxPrice)
            .Where(n => model.MinPrice == null || n.Price >= model.MinPrice).CountAsync();

            var editionsWithCount = (editions: editions, count: count);

            return editionsWithCount;
        }
        public override async Task UpdateAsync(PrintingEdition edition)
        {
            var authors = new List<Author>(edition.Authors);
            edition.Authors.Clear();
            _dbSet.Update(edition);
            var editionForUpdate = _dbSet.Include(e => e.Authors).FirstOrDefault(e => e.Id == edition.Id);
            editionForUpdate.Authors.RemoveAll(a => !authors.Exists(a2 => a2.Id == a.Id));
            var result = authors.Where(a => !editionForUpdate.Authors.Exists(a2 => a2.Id == a.Id)).ToList();
            editionForUpdate.Authors.AddRange(result);
            _dbSet.Update(editionForUpdate);
            await SaveChangesAsync();
        }
    }
}
