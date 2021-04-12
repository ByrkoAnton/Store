using Store.BusinessLogicLayer.Models.EditionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises.Interfaces
{
    public interface IPrintingEditionService
    {
        public Task<PrintingEditionModel> GetByIdAsync(long id);
        public Task<List<PrintingEditionModel>> GetAllAsync();
        public Task<PrintingEditionModel> GetByDescriptionAsync(PrintingEditionModel model);
        public Task CreateAsync(PrintingEditionModel model);
        public Task RemoveAsync(PrintingEditionModel model);
        public Task UpdateAsync(PrintingEditionModel model);
    }
}
