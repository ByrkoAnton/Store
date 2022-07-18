using Store.BusinessLogicLayer.Models.EditionModel;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises.Interfaces//TODO wrong spelling
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
