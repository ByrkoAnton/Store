using Store.BusinessLogicLayer.Models.EditionModel;
using Store.BusinessLogicLayer.Models.PaginationsModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises.Interfaces
{
    public interface IPrintingEditionService
    {
        public Task<PrintingEditionModel> GetByIdAsync(long id);
        public Task<NavigationModel<PrintingEditionModel>> GetAsync(EditionFiltrationModel model);
        public Task<PrintingEditionModel> GetByTitleAsync(PrintingEditionModel model);
        public Task CreateAsync(PrintingEditionModel model);
        public Task RemoveAsync(PrintingEditionModel model);
        public Task UpdateAsync(PrintingEditionModel model);
    }
}
