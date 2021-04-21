using Store.BusinessLogicLayer.Models.Authors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Store.DataAccessLayer.Enums.Enums.PrintingEditionEnums;
using Type = Store.DataAccessLayer.Enums.Enums.PrintingEditionEnums.Type;

namespace Store.BusinessLogicLayer.Models.EditionModel
{
    public class PrintingEditionFiltrationModel
    {
        public long? Id { get; set; }
        public string Description { get; set; }
        public double? Prise { get; set; }
        public bool? IsRemoved { get; set; }
        public string Status { get; set; }
        public Currency? Currency { get; set; }
        public Type? Type { get; set; }
        public List<AuthorModel> AuthorModels { get; set; } = new();
    }
}
