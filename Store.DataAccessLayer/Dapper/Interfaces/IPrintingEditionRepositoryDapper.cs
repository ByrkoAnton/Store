using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Models.FiltrationModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Dapper.Interfaces
{
    public interface IPrintingEditionRepositoryDapper //TODO base interface?, wrong spelling ++++
    {
        public Task<(IEnumerable<PrintingEdition>, int, double, double)> GetAsync(EditionFiltrationModelDAL model);
        public Task<PrintingEdition> GetByIdAsync(long id);
        public Task<List<PrintingEdition>> GetEditionsListByIdListAsync(List<long> id);
        public Task<PrintingEdition> GetByTitleAsync(string description);
        public Task CreateAsync(PrintingEdition edition);
        public Task UpdateAsync(PrintingEdition edition);
        public Task DeleteAsync(long id);
    }
}
