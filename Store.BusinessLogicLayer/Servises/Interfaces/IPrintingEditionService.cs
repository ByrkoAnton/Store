using Store.BusinessLogicLayer.Models.EditionModel;
using Store.BusinessLogicLayer.Models.PaginationsModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises.Interfaces
{
    public interface IPrintingEditionService
    {
        public Task<PrintingEditionModel> GetByIdAsync(long id);
        public Task<NavigationModel<PrintingEditionModel>> GetAsync(EditionFiltrPaginSortModel model);
        public Task<PrintingEditionModel> GetByDescriptionAsync(PrintingEditionModel model);
        public Task CreateAsync(PrintingEditionModel model);
        public Task RemoveAsync(PrintingEditionModel model);
        public Task UpdateAsync(PrintingEditionModel model);
    }
}
