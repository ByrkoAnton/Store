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
        public async Task<IEnumerable<PrintingEdition>> GetByDescriptionAsync(string description)
        {
            var result = await _dbSet.Where(edition => edition.Description == description)
                .Include(pe => pe.Authors).AsNoTracking().ToListAsync();
            return result;
        }
        public async Task<(IEnumerable<PrintingEdition>, int)> GetAsync(EditionFiltrPagingSortModelDAL model)
        {
            var editions = await _dbSet.Include(pe => pe.Authors).AsNoTracking()
            .Where(n => EF.Functions.Like(n.Id.ToString(), $"%{model.Id}%")
            && EF.Functions.Like(n.Description, $"%{model.Description}%")
            && EF.Functions.Like(n.Prise.ToString(), $"%{model.Prise}%")
            && EF.Functions.Like(n.Status, $"%{model.Status}%")
            && (n.Currency == model.Currency || model.Currency == null)
            && (n.Type == model.Type || model.Type == null)
            && (n.Authors.Any(t => EF.Functions.Like(t.Name, $"%{model.AuthorName}%"))))
            .OrderBy($"{model.PropertyForSort} " +
            $"{(model.IsAscending ? Constants.SortingParams.SORT_ASC_DIRECTION : Constants.SortingParams.SORT_DESC_DIRECTION)}")
            .Skip((model.CurrentPage - 1) * model.PageSize).Take(model.PageSize).ToListAsync();

            int count = await _dbSet
                .Where(n => EF.Functions.Like(n.Id.ToString(), $"%{model.Id}%")
                && EF.Functions.Like(n.Description, $"%{model.Description}%")
                && EF.Functions.Like(n.Prise.ToString(), $"%{model.Prise}%")
                && EF.Functions.Like(n.Status, $"%{model.Status}%")
                && (n.Currency == model.Currency || model.Currency == null)
                && (n.Type == model.Type || model.Type == null)
                && (n.Authors.Any(t => EF.Functions.Like(t.Name, $"%{model.AuthorName}%")))).CountAsync();

            var editionsWithCount = (editions: editions, count: count);

            return editionsWithCount;
        }
        public override async Task UpdateAsync(PrintingEdition edition)
        {
            var authors = new List<Author>(edition.Authors);
            edition.Authors.Clear();
            _dbSet.Update(edition);
            var editionForUpdate = _dbSet.Include(a => a.Authors).FirstOrDefault(b => b.Id == edition.Id);
            editionForUpdate.Authors.RemoveAll(p => !authors.Exists(p2 => p2.Id == p.Id));
            var result = authors.Where(p => !editionForUpdate.Authors.Exists(p2 => p2.Id == p.Id)).ToList();
            editionForUpdate.Authors.AddRange(result);
            //authors.RemoveAll(p => !editionForUpdate.Authors.Exists(p2 => p2.Id == p.Id));
            //editionForUpdate.Authors.AddRange(authors);
            _dbSet.Update(editionForUpdate);
            await SaveChangesAsync();
        }
    }
}
