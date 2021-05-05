using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.AppContext;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.FiltrationModels;
using Store.DataAccessLayer.Repositories.Base;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Store.Sharing.Constants;
using AutoMapper;

namespace Store.DataAccessLayer.Repositories
{
    public class PrintingEditionRepository : BaseRepository<PrintingEdition>, IPrintingEditionRepository
    {
        private readonly IMapper _mapper;
        public PrintingEditionRepository(ApplicationContext context, IAuthorRepository authorRepository, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        public override async Task CreateAsync(PrintingEdition edition)
        {
            List<Author> authors = new List<Author>(edition.Authors);

            edition.Authors.Clear();
            await _dbSet.AddAsync(edition);
            edition.Authors.AddRange(authors);
            await SaveChangesAsync();
        }
        public override async Task<PrintingEdition> GetByIdAsync(Expression<Func<PrintingEdition, bool>> predicate)
        {
            var result = await _dbSet.Include(x=>x.Authors).AsNoTracking().FirstOrDefaultAsync(predicate);
            return result;
        }
        public override async Task<IEnumerable<PrintingEdition>> GetAsync(Expression<Func<PrintingEdition, bool>> predicate)
        {
            var result = await _dbSet.Where(predicate).Include(pe => pe.Authors).AsNoTracking().ToListAsync();
            return result;
        }
        public async Task<(IEnumerable<PrintingEdition>, int)> GetAsync(EditionFiltrPagingSortModelDAL model)
        {
            string direction = Constants.SortingParams.SORT_ASC_DIRECTION;
            if (!model.IsAscending)
            {
                direction = Constants.SortingParams.SORT_DESC_DIRECTION;
            }

            var editions = await _dbSet.Include(pe => pe.Authors).AsNoTracking()
            .Where(n => EF.Functions.Like(n.Id.ToString(), $"%{model.Id}%")
            && EF.Functions.Like(n.Description, $"%{model.Description}%")
            && EF.Functions.Like(n.Prise.ToString(), $"%{model.Prise}%")
            && EF.Functions.Like(n.Status, $"%{model.Status}%")
            && (n.Currency == model.Currency || model.Currency == null)
            && (n.Type == model.Type || model.Type == null)
            && (n.Authors.Any(t => EF.Functions.Like(t.Name, $"%{model.AuthorName}%"))))
            .OrderBy($"{model.PropertyForSort} {direction}").Skip((model.CurrentPage - 1) * model.PageSize).Take(model.PageSize).ToListAsync();

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

            //var editionForUpdate = _dbSet.Include(a => a.Authors).
            //   FirstOrDefault(b => b.Id == edition.Id);
            
            //editionForUpdate.Authors.RemoveAll(p => !edition.Authors.Exists(p2 => p2.Id == p.Id));
            //var result = edition.Authors.Where(p => !editionForUpdate.Authors.Exists(p2 => p2.Id == p.Id)).ToList();

            ////editionForUpdate.Authors.AddRange(result);
            ////editionForUpdate.Currency = edition.Currency;
            ////editionForUpdate.DateOfCreation = edition.DateOfCreation;
            ////editionForUpdate.Description = edition.Description;
            ////editionForUpdate.IsRemoved = edition.IsRemoved;
            ////editionForUpdate.Prise = edition.Prise;
            //_dbSet.Update(editionForUpdate);
            //await SaveChangesAsync();

            var authors = new List<Author>(edition.Authors);
            edition.Authors.Clear();
            _dbSet.Update(edition);
            var result = await _dbSet.Include(edition => edition.Authors).FirstOrDefaultAsync(x => x.Id == edition.Id);
            result.Authors.RemoveAll(x => authors.Any(y => y.Id == x.Id));
            result.Authors.AddRange(authors);
            _dbSet.Update(edition);
            await SaveChangesAsync();
        }
    }
}
