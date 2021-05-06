using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Providers.Interfaces
{
    public interface IJwtProvider
    {
        public string GenerateJwt(string name, List<string> roles);
    }
}
