using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.AppContext;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Base;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

        public override async Task<IEnumerable<PrintingEdition>> GetAllAsync()
        {

            var result = await _dbSet.Include(pe => pe.Authors).AsNoTracking().ToListAsync();
            return result;
        }

        public override async Task UpdateAsync(PrintingEdition edition)
        {
            _dbSet.Update(edition);
            List<Author> authors = new List<Author>(edition.Authors);

            var editionForUpdate = _dbSet.Include(a => a.Authors).
                FirstOrDefault(e =>e.Id == edition.Id);

            editionForUpdate.Authors.RemoveAll(p => !authors.Exists(p2 => p2.Id == p.Id));
            var result = authors.Where(p => !editionForUpdate.Authors.Exists(p2 => p2.Id == p.Id)).ToList();
            editionForUpdate.Authors.AddRange(result);
            await _context.SaveChangesAsync();
        }
    }
}
