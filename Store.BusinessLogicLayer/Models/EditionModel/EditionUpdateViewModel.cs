using Store.BusinessLogicLayer.Models.Authors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
