using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Models.FiltrationModels;
using System.Collections.Generic;
using static Store.DataAccessLayer.Enums.Enums;
using static Store.DataAccessLayer.Enums.Enums.EditionEnums;

namespace Store.DataAccessLayer.FiltrationModels
{
    public class EditionFiltrationModelDAL : BaseFiltrationModelDAL
    {
        public string Title { get; set; }
        public double? Price { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public bool? IsRemoved { get; set; }
        public string Status { get; set; }
        public Currency? Currency { get; set; }
        public List<PrintingEditionType> EditionType { get; set; }
        public string AuthorName { get; set; }
    }
}
