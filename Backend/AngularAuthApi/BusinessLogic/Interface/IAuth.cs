using DataAceess.Dto;
using DataAceess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public interface IAuth
    {
        public Task<bool> AddUser(UserDto user);
        public Task<User> Login(LoginDto loginDTO);
        public Task<bool> UpdatePassword(string email, ResetPasswordDto resetPasswordDTO);
    }
}
