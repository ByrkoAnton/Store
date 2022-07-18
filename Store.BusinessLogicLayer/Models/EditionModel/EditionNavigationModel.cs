//TODO extra line
using Store.BusinessLogicLayer.Models.PaginationsModels;

namespace Store.BusinessLogicLayer.Models.EditionModel
{
   public class EditionNavigationModel : NavigationModelBase<PrintingEditionModel>
    {
        public double SliderFloor { get; set; }
        public double SliderCeil { get; set; }
    }
}
