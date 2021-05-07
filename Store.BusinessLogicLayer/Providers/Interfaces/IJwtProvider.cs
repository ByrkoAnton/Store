using System.Collections.Generic;

namespace Store.BusinessLogicLayer.Providers.Interfaces
{
    public interface IJwtProvider
    {
        public string GenerateJwt(string name, List<string> roles);
    }
}
