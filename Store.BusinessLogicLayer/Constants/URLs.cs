using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Constants
{
    public partial class Constants
    {
       public class URLs
        {
            public const string URL_RESETPASSWORD = "https://localhost:5001/api/UserService/ResetPassword";
            public const string URL_CONFIRMEMAIL = "https://localhost:5001/api/Account/ConfirmEmail";
        }
    }
}
