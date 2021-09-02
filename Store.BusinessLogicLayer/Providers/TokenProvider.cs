using Store.BusinessLogicLayer.Providers.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Store.Sharing.Constants;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Store.BusinessLogicLayer.Configuration;

namespace Store.BusinessLogicLayer.Providers
{
    public class TokenProvider : ITokenProvider
    {
        private readonly TokenConfig _options;
        public TokenProvider(IOptions<TokenConfig> options)
        {
            _options = options.Value;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[Constants.RefreshToken.BYTES];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public string GenerateJwt(string name, List<string> roles, string id)
        {
            string role = $"{(roles.Any(s => s.Contains(Constants.User.ROLE_ADMIN)) ? Constants.User.ROLE_ADMIN : Constants.User.ROLE_USER)}";

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role),
                new Claim(Constants.JwtProvider.ID, id)
            };

            var identity = new ClaimsIdentity(claims, Constants.JwtProvider.METHOD_NAME, ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

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
