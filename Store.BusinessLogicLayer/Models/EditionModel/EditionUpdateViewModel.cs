using Store.BusinessLogicLayer.Models.Authors;
using System.Collections.Generic;


namespace Store.BusinessLogicLayer.Models.EditionModel
{
    public class EditionUpdateViewModel
    {
        public PrintingEditionModel PrintingEdition { get; set; }
        public List<AuthorModel> AllAuthorModels { get; set; }
        public string NewAuthorsNames { get; set; }
        public string DeletedAuthorsNames { get; set; }
    }
}
