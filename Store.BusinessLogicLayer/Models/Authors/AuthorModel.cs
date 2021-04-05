using Store.BusinessLogicLayer.Models.EditionModel;
using Store.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Models.Authors
{
    public class AuthorModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<PrintingEditionModel> PrintingEditionModels { get; set; }
    }
}
