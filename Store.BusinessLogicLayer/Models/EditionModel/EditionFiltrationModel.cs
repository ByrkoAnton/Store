using Store.BusinessLogicLayer.Models.Base;
using Store.Sharing.Constants;
using System.Collections.Generic;
using static Store.DataAccessLayer.Enums.Enums.EditionEnums;

namespace Store.BusinessLogicLayer.Models.EditionModel
{
    public class EditionFiltrationModel : BaseFiltrationModel
    {
        public string Title { get; set; }
        public double? Prise { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public bool? IsRemoved { get; set; }
        public string Status { get; set; }
        public CurrencyType? Currency { get; set; }
        public int currentSliderFlor { get; set; }
        public int currentSliderCeil { get; set; }

        public List<PrintingEditionType> EditionType { get; set; }
        public string AuthorName { get; set; }

        public EditionFiltrationModel()
        {
            PropertyForSort = Constants.SortingParams.EDITION_DEF_SORT_PROP;
        }
    }

}
