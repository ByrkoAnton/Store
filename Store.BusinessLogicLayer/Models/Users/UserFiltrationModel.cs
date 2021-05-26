using Store.BusinessLogicLayer.Models.Base;
using Store.Sharing.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Models.Users
{
    public class UserFiltrationModel : BaseFiltrationModel
    {
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public bool? IsBlocked { get; set; }
        
        public UserFiltrationModel()
        {
            PropertyForSort = Constants.SortingParams.USER_DEF_SORT_PROP;
        }
    }
}
