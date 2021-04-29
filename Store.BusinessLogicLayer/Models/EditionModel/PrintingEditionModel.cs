using Store.BusinessLogicLayer.Models.Authors;
using Store.BusinessLogicLayer.Models.Base;
using System.Collections.Generic;
using static Store.DataAccessLayer.Enums.Enums;
namespace Store.BusinessLogicLayer.Models.EditionModel
{
    public class PrintingEditionModel : BaseModel
    {
        public string Description { get; set; }
        public double Prise { get; set; }
        public bool IsRemoved { get; set; }
        public string Status { get; set; }
        public Currency Currency { get; set; }
        public PrintingEditionType Type { get; set; }

        public List<AuthorModel> AuthorModels { get; set; } = new();
    }
}
