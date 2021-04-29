using Store.BusinessLogicLayer.Models.Base;
using Store.BusinessLogicLayer.Models.EditionModel;
using System.Collections.Generic;

namespace Store.BusinessLogicLayer.Models.Authors
{
    public class AuthorModel:BaseModel
    {
        public string Name { get; set; }
        public List<PrintingEditionModel> PrintingEditionModels { get; set; }
    }
}
