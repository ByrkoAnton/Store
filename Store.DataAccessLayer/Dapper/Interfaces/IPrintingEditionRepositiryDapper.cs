using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.FiltrationModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Dapper.Interfaces
{
    public interface IPrintingEditionRepositiryDapper
    {
        public Task<(IEnumerable<PrintingEdition>, int, double, double)> GetAsync(EditionFiltrationModelDAL model);
        public Task<PrintingEdition> GetByIdAsync(long id);
        public Task<List<PrintingEdition>> GetEditionsListByIdListAsync(List<long> id);
        public Task<PrintingEdition> GetByTitle(string description);
    }
}
