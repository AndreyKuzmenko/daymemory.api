using DayMemory.Core.Models.Common;
using DayMemory.Core.Models.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DayMemory.Web.Components.Auth
{
    public interface IJwTokenHelper
    {
        Task<string> GenerateAccessToken(User user);

        string GenerateRefreshToken();

        ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
    }

    public class JwTokenHelper : IJwTokenHelper
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwTokenHelper> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public JwTokenHelper(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration, ILogger<JwTokenHelper> logger, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._configuration = configuration;
            this._logger = logger;
            this._roleManager = roleManager;
        }

        public async Task<string> GenerateAccessToken(User user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }

            claims.Add(new Claim(ClaimTypes.Name, user.Id));
            claims.Add(new Claim(ClaimTypes.Email, user.Email!));

            var secret = _configuration.GetValue<string>("JWT:Secret");
            if (string.IsNullOrEmpty(secret))
            {
                throw new ConfigurationException("JWT:Secret");
            }

            var tokenValidityInMinutes = _configuration.GetValue<int>("JWT:TokenValidityInMinutes");
            if (tokenValidityInMinutes == 0)
            {
                throw new ConfigurationException("JWT:TokenValidityInMinutes");
            }

            var validAudience = _configuration.GetValue<string>("JWT:ValidAudience");
            if (string.IsNullOrEmpty(validAudience))
            {
                throw new ConfigurationException("JWT:ValidAudience");
            }

            var validIssuer = _configuration.GetValue<string>("JWT:ValidIssuer");
            if (string.IsNullOrEmpty(validIssuer))
            {
                throw new ConfigurationException("JWT:ValidIssuer");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = validIssuer,
                Audience = validAudience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(tokenValidityInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var res = tokenHandler.WriteToken(token);

            return res;
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var secret = _configuration["JWT:Secret"];
            if (secret == null)
            {
                throw new ConfigurationException("JWT:Secret");
            }
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
