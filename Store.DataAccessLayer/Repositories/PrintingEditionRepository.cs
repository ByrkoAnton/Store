using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.AppContext;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Models.FiltrationModels;
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
        public async Task<PrintingEdition> GetByTitle(string title)
        {
    
            var result = await _dbSet.Include(pe => pe.Authors).AsNoTracking().FirstOrDefaultAsync(edition =>
            edition.Title == title);
            return result;
        }

        public async Task<(IEnumerable<PrintingEdition>, int, double, double)> GetAsync(EditionFiltrationModelDAL model)
        {
            var editions = await _dbSet.Include(pe => pe.Authors).AsNoTracking()
            .Where(n => model.Id == null || n.Id == model.Id)
            .Where(n => EF.Functions.Like(n.Title, $"%{model.Title}%"))
            .Where(n => n.Currency == model.Currency || model.Currency == null)
            .Where(n => model.EditionType.Contains(n.EditionType))
            .Where(n => string.IsNullOrEmpty(model.AuthorName)
            || n.Authors.Any(t => EF.Functions.Like(t.Name, $"%{model.AuthorName}%")))
            .Where(n => n.Price <= model.MaxPrice || model.MaxPrice == null)
            .Where(n => n.Price >= model.MinPrice || model.MinPrice == null)
            .OrderBy($"{model.PropertyForSort} {(model.IsAscending ? Constants.SortingParams.SORT_ASC : Constants.SortingParams.SORT_DESC)}")
            .Skip((model.CurrentPage - Constants.PaginationParams.DEFAULT_OFFSET) * model.PageSize).Take(model.PageSize)
            .ToListAsync();

            int count = default;
            double minPrice = model.CurrentSliderFloor;
            double maxPrice = model.CurrentSliderCeil;
            var result = (editions: editions, count: count, minPrice: minPrice, maxPrice: maxPrice);//TODO redundant explicit name

            if (!editions.Any())
            {
                return result;
            }

            count = await _dbSet
           .Where(n => model.Id == null || n.Id == model.Id)
           .Where(n => EF.Functions.Like(n.Title, $"%{model.Title}%"))
           .Where(n => n.Currency == model.Currency || model.Currency == null)
           .Where(n => model.EditionType.Contains(n.EditionType))
           .Where(n => string.IsNullOrEmpty(model.AuthorName)
           || n.Authors.Any(t => EF.Functions.Like(t.Name, $"%{model.AuthorName}%")))
           .Where(n => model.MaxPrice == null || n.Price <= model.MaxPrice)
           .Where(n => model.MinPrice == null || n.Price >= model.MinPrice)
           .CountAsync();

            maxPrice = await _dbSet
           .Where(n => model.Id == null || n.Id == model.Id)
           .Where(n => EF.Functions.Like(n.Title, $"%{model.Title}%"))
           .Where(n => n.Currency == model.Currency || model.Currency == null)
           .Where(n => model.EditionType.Contains(n.EditionType))
           .Where(n => string.IsNullOrEmpty(model.AuthorName)
           || n.Authors.Any(t => EF.Functions.Like(t.Name, $"%{model.AuthorName}%"))).MaxAsync(n => n.Price);

            minPrice = await _dbSet
           .Where(n => model.Id == null || n.Id == model.Id)
           .Where(n => EF.Functions.Like(n.Title, $"%{model.Title}%"))
           .Where(n => n.Currency == model.Currency || model.Currency == null)
           .Where(n => model.EditionType.Contains(n.EditionType))
           .Where(n => string.IsNullOrEmpty(model.AuthorName)
           || n.Authors.Any(t => EF.Functions.Like(t.Name, $"%{model.AuthorName}%"))).MinAsync(n => n.Price);

            result = (editions: editions, count: count, minPrice: minPrice, maxPrice: maxPrice);

            return result;
        }
        public override async Task UpdateAsync(PrintingEdition edition)
        {
            var authors = new List<Author>(edition.Authors);
            edition.Authors.Clear();
            _dbSet.Update(edition);
            var editionForUpdate = _dbSet.Include(e => e.Authors).FirstOrDefault(e => e.Id == edition.Id);
            editionForUpdate.Authors.RemoveAll(a => !authors.Exists(incomingAuthors => incomingAuthors.Id == a.Id));//TODO possible null reference exception
            var result = authors.Where(a => !editionForUpdate.Authors.Exists(authorsFromDb => authorsFromDb.Id == a.Id)).ToList();
            editionForUpdate.Authors.AddRange(result);
            _dbSet.Update(editionForUpdate);
            await SaveChangesAsync();
        }
    }
}
