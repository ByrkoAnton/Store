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
        public Task CreateAsync(PrintingEditionModel model);
    }
}
