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
using Store.DataAccessLayer.SupportingClasses;

namespace Store.DataAccessLayer.Repositories
{
    public class PrintingEditionRepository : BaseRepository<PrintingEdition>, IPrintingEditionRepository
    {
        private readonly IAuthorRepository _authorRepository;
        public PrintingEditionRepository(ApplicationContext context, IAuthorRepository authorRepository) : base(context)
        {
            _authorRepository = authorRepository;
        }

        public override async Task CreateAsync(PrintingEdition edition)
        {
            List<Author> authors = new List<Author>(edition.Authors);

            edition.Authors.Clear();
            await _dbSet.AddAsync(edition);
            edition.Authors.AddRange(authors);
            await _context.SaveChangesAsync();
        }

        public override async Task<PrintingEdition> GetByIdAsync(Expression<Func<PrintingEdition, bool>> predicate)
        {
            var result = await _dbSet.Include(pe => pe.Authors).AsNoTracking().FirstOrDefaultAsync(predicate);
            return result;
        }

        public override async Task<IEnumerable<PrintingEdition>> GetAsync(Expression<Func<PrintingEdition, bool>> predicate)
        {
            var result = await _dbSet.Where(predicate).Include(pe => pe.Authors).AsNoTracking().ToListAsync();
            return result;
        }

        public async Task<(IEnumerable<PrintingEdition>, int)> GetAsync(EditionFiltrPagingSortModelDAL model)
        {
            string direction = "ASC";
            if (!model.IsAsc)
            {
                direction = "DESC";
            }

            var editions = await _dbSet.Include(pe => pe.Authors).AsNoTracking()
            .Where(n => EF.Functions.Like(n.Id.ToString(), $"%{model.Id}%")
            && EF.Functions.Like(n.Description, $"%{model.Description}%")
            && EF.Functions.Like(n.Prise.ToString(), $"%{model.Prise}%")
            && EF.Functions.Like(n.Status, $"%{model.Status}%")
            && (n.Currency == model.Currency || model.Currency == null)
            && (n.Type == model.Type || model.Type == null)
            && (n.Authors.Any(t => EF.Functions.Like(t.Name, $"%{model.AuthorName}%"))))
            .OrderBy($"{model.PropForSort} {direction}").Skip((model.CurrentPage - 1) * model.PageSize).Take(model.PageSize).ToListAsync();

            int count = await _dbSet
                .Where(n => EF.Functions.Like(n.Id.ToString(), $"%{model.Id}%")
                && EF.Functions.Like(n.Description, $"%{model.Description}%")
                && EF.Functions.Like(n.Prise.ToString(), $"%{model.Prise}%")
                && EF.Functions.Like(n.Status, $"%{model.Status}%")
                && (n.Currency == model.Currency || model.Currency == null)
                && (n.Type == model.Type || model.Type == null)
                && (n.Authors.Any(t => EF.Functions.Like(t.Name, $"%{model.AuthorName}%")))).CountAsync();

            
            var editionsCount = (editions:editions, count:count);
            
            return editionsCount;
        }

        public override async Task UpdateAsync(PrintingEdition edition)
        {
            _dbSet.Update(edition);
            List<Author> authors = new List<Author>(edition.Authors);

            var editionForUpdate = _dbSet.Include(a => a.Authors).
                FirstOrDefault(e => e.Id == edition.Id);

            editionForUpdate.Authors.RemoveAll(p => !authors.Exists(p2 => p2.Id == p.Id));
            var result = authors.Where(p => !editionForUpdate.Authors.Exists(p2 => p2.Id == p.Id)).ToList();
            editionForUpdate.Authors.AddRange(result);
            await _context.SaveChangesAsync();
        }
    }
}
