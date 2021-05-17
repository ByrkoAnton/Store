using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.FiltrationModels;
using Store.DataAccessLayer.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.Interfaces
{
    public interface IPrintingEditionRepository : IBaseRepository<PrintingEdition>
    {
        public Task<(IEnumerable<PrintingEdition>, int)> GetAsync(EditionFiltrPagingSortModelDAL model);
        public Task<PrintingEdition> GetByIdAsync(long id);
        public Task<List<PrintingEdition>> GetByIdAsync(List<long> id);
        public Task<IEnumerable<PrintingEdition>> GetByDescriptionAsync(string description);
    }
}
