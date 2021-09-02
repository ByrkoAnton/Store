﻿using Store.BusinessLogicLayer.Models.EditionModel;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Servises.Interfaces
{
    public interface IPrintingEditionService
    {
        public Task<PrintingEditionModel> GetByIdAsync(long id);
        public Task<EditionNavigationModel> GetAsync(EditionFiltrationModel model);
        public Task<PrintingEditionModel> GetByTitleAsync(PrintingEditionModel model);
        public Task CreateAsync(PrintingEditionModel model);
        public Task RemoveAsync(PrintingEditionModel model);
        public Task UpdateAsync(PrintingEditionModel model);
    }
}
