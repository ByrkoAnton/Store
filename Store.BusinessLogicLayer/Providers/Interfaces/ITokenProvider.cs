using System.Collections.Generic;

namespace Store.BusinessLogicLayer.Providers.Interfaces
{
    public interface ITokenProvider
    {
        public string GenerateJwt(string name, List<string> roles, string id);
        public string GenerateRefreshToken();
    }
}
