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
using System.Text;
using System.Threading.Tasks;

namespace DayMemory.Web.Components.Auth
{
    public interface IJwTokenHelper
    {
        Task<string> GenerateJwtToken(User user);
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

        public async Task<string> GenerateJwtToken(User user)
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

            var secret = _configuration.GetValue<string>("Secret");
            if (secret == null)
            {
                throw new ConfigurationException("Secret");
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddYears(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var res = tokenHandler.WriteToken(token);

            return res;
        }
    }
}
