using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.FiltrationModels;
using Store.DataAccessLayer.Repositories.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.Interfaces
{
    public interface IPrintingEditionRepository : IBaseRepository<PrintingEdition>
    {
        public Task<(IEnumerable<PrintingEdition>, int)> GetAsync(EditionFiltrPagingSortModelDAL model);
    }
}
