using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Models.FiltrationModels
{
    public class AuthorFiltrPagingSortModelDAL : BaseFiltrPagingSortModelDAL
    {
        public string Name { get; set; }
        public string EditionDescription { get; set; }
    }
}
