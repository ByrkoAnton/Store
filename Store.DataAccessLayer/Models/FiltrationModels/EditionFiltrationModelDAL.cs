using Store.DataAccessLayer.Models.FiltrationModels;
using System.Collections.Generic;
using static Store.DataAccessLayer.Enums.Enums;

namespace Store.DataAccessLayer.FiltrationModels
{
    public class EditionFiltrationModelDAL : BaseFiltrationModelDAL
    {
        public string Title { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public bool? IsRemoved { get; set; }
        public string Status { get; set; }
        public CurrencyType? Currency { get; set; }
        public int CurrentSliderFlor { get; set; }
        public int CurrentSliderCeil { get; set; }
        public List<PrintingEditionType> EditionType { get; set; }
        public string AuthorName { get; set; }
    }
}
