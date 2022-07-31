using System.Collections.Generic;
//TODO extra line

namespace AdminApp.Providers
{
    public interface ITokenProvider
    {
        public string GenerateJwt(string name, List<string> roles, string id);
    }
}
