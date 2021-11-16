using AdminApp.Config;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace AdminApp.Providers
{
    public class TokenProvider : ITokenProvider
    {
        private readonly TokenConfig _options;
        public TokenProvider(IOptions<TokenConfig> options)
        {
            _options = options.Value;
        }

        public string GenerateJwt(string name, List<string> roles, string id)
        {
            string role = $"{(roles.Any(s => s.Contains("admin")) ? "admin" : "user")}";

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role),
                new Claim("id", id)
            };

            var identity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            var tmp = _options.Issuer;

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: _options.Issuer,
                    audience: _options.Audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(double.Parse(_options.Lifetime))),
                    signingCredentials: credentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }


    }
}
