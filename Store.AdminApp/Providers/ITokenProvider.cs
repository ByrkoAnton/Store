using System.Collections.Generic;


namespace AdminApp.Providers
{
    public interface ITokenProvider
    {
        public string GenerateJwt(string name, List<string> roles, string id);
    }
}
