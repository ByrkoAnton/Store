using System.Collections.Generic;
using static Store.DataAccessLayer.Enums.Enums;

namespace Store.DataAccessLayer.Models.FiltrationModels //TODO wrong namespace+++
{
    public class EditionFiltrationModelDAL : BaseFiltrationModelDAL
    {
        public string Title { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public bool? IsRemoved { get; set; }
        public string Status { get; set; }
        public CurrencyType? Currency { get; set; }
        public int CurrentSliderFloor { get; set; }//TODO wrong spelling 'Floor'+++
        public int CurrentSliderCeil { get; set; }
        public List<PrintingEditionType> EditionType { get; set; }
        public string AuthorName { get; set; }
    }
}
