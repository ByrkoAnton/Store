using Store.BusinessLogicLayer.Models.EditionModel;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Serviсes.Interfaces
{
    public interface IPrintingEditionService
    {
        public Task<PrintingEditionModel> GetByIdAsync(long id);
        public Task<EditionNavigationModel> GetAsync(EditionFiltrationModel model);
        public Task CreateAsync(PrintingEditionModel model);
        public Task RemoveAsync(long id);
        public Task UpdateAsync(PrintingEditionModel model);
    }
}
