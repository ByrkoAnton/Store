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
        public PrintingEditionRepository(ApplicationContext context) : base(context)
        {
        }

        public override async Task<PrintingEdition> GetByIdAsync(Expression<Func<PrintingEdition, bool>> predicate)
        {
            var result = await _dbSet.Include(pe => pe.Authors).FirstOrDefaultAsync(predicate);
            return result;
        }

        public override async Task<IEnumerable<PrintingEdition>> GetAsync(Expression<Func<PrintingEdition, bool>> predicate)
        {
            var result = await _dbSet.Where(predicate).Include(pe => pe.Authors).ToListAsync();
            return result;
        }
    }
}
