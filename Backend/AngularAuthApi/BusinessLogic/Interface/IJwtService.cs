using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public interface IJwtService
    {
        public string GetJwtToken(string email, int role);
        public bool VerifyToken(string token, out JwtSecurityToken jwttoken);
    }
}
