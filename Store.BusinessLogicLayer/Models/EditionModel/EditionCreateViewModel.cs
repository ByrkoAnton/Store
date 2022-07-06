using Store.BusinessLogicLayer.Models.Authors;
using System.Collections.Generic;
using static Store.DataAccessLayer.Enums.Enums;

namespace Store.BusinessLogicLayer.Models.EditionModel
{
   public class EditionCreateViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool IsRemoved { get; set; }
        public string Status { get; set; }
        public string AuthorsNames { get; set; }
        public CurrencyType Currency { get; set; }
        public PrintingEditionType EditionType { get; set; }
        public List<AuthorModel> AllAuthorModels { get; set; }
    }
}

