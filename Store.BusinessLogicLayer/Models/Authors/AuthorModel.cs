using Store.BusinessLogicLayer.Models.Base;
using Store.BusinessLogicLayer.Models.EditionModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Store.BusinessLogicLayer.Models.Authors
{
    public class AuthorModel:BaseModel
    {
        [Required]
        public string Name { get; set; }
        public List<PrintingEditionModel> PrintingEditionModels { get; set; }
    }
}
