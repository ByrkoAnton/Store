using Store.BusinessLogicLayer.Models.EditionModel;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.AdminServises.Interfeces//TODO wrong spelling
{
  public interface IPrintingEditionServiceAdmin
    {
        public Task CreateEditionByAdminAsync(EditionCreateViewModel model);
        public Task UpdateEditionByAdminAsync(EditionUpdateViewModel model);
    }
}
