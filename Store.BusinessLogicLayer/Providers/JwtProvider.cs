using Store.BusinessLogicLayer.Providers.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Store.Sharing.Constants;

namespace Store.BusinessLogicLayer.Providers
{
    public class JwtProvider : IJwtProvider
    {
        private IConfiguration _config;
        public JwtProvider(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateJwt(string name, List<string> roles, string id)
        {
            string role = $"{(roles.Any(s => s.Contains(Constants.UserConstants.ROLE_ADMIN)) ? Constants.UserConstants.ROLE_ADMIN : Constants.UserConstants.ROLE_USER)}";
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role),
                new Claim(Constants.JwtProviderConst.ID, id)
            };

            var identity = new ClaimsIdentity(claims, Constants.JwtProviderConst.METHOD_NAME, ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config[Constants.JwtProviderConst.KEY]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: _config[Constants.JwtProviderConst.ISSUER],
                    audience: _config[Constants.JwtProviderConst.AUDIENCE],
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(1)),
                    signingCredentials: credentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            
            return encodedJwt;
        }
    }
}
