using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Models
{
    public class TokenResponseModel
    {
        public string AccessToken { set; get; }
        public string RefreshToken { set; get; }
    }
}
