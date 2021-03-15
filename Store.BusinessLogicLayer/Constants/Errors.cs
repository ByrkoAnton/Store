using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Constants
{
    public partial class Constants
    {
        public class Error
        {
            public const string LOGIN_FAILD_MODELI_IS_NULL = "login faild - model is NULL";
            public const string LOGIN_FAILD_MODEL_IS_NOT_CORECT = "login faild - model is not corect";
            public const string PASSWORD_FAILD_MODEL_IS_NOT_CORECT = "password faild - model is not corect";
            public const string LOGIN_FAILD_NO_USER_WITH_THIS_EMAIL = "login faild - no user with this email";
            public const string LOGIN_FAILD_WRONG_PASSWORD = "login faild - wrong password";
            public const string ERROR_NO_USERROLE = "login faild - no userRole";
        }
    }
}
