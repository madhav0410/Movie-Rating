using BusinessLogic.Interface;
using DataAceess.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class JwtServiceRepository : IJwtService
    {
        public IConfiguration _configuration { get; set; }

        public JwtServiceRepository(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public string GetJwtToken(string email, int role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role,role.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(Double.Parse(_configuration["Jwt:ExpiryDays"])),
                SigningCredentials = credentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool VerifyToken(string token, out JwtSecurityToken jwttoken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:key"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                }, out SecurityToken validatedToken);
                jwttoken = (JwtSecurityToken)validatedToken;

                if (jwttoken != null)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                jwttoken = null;
                return false;
            }
        }
    }
}
