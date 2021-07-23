using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.FiltrationModels;
using Store.DataAccessLayer.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Store.DataAccessLayer.Enums.Enums.EditionEnums;

namespace Store.DataAccessLayer.Repositories.Interfaces
{
    public interface IPrintingEditionRepository : IBaseRepository<PrintingEdition>
    {
        public Task<(IEnumerable<PrintingEdition>, int, double, double)> GetAsync(EditionFiltrationModelDAL model);
        public Task<PrintingEdition> GetByIdAsync(long id);
        public Task<List<PrintingEdition>> GetEditionsListByIdListAsync(List<long> id);
        public Task<IEnumerable<PrintingEdition>> GetByTitle(string description);
        //public Task<bool> FindByTitle(string title);
        //public Task<bool> FindByCurrency(Currency? currency);
        //public Task<bool> FindByEditionType(List<PrintingEditionType> types);
    }
}
