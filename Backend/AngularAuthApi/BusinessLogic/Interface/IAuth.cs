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
        public User Login(LoginDto loginDTO);
        public bool AddUser(UserDto user);
        public bool UpdatePassword(string email, ResetPasswordDto resetPasswordDTO);
    }
}
