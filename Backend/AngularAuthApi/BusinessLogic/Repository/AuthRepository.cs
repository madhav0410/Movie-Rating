using BusinessLogic.Interface;
using DataAceess.Data;
using DataAceess.Dto;
using DataAceess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class AuthRepository : IAuth
    {
        private readonly ApplicationDbContext _context;
        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool AddUser(UserDto userDTO)
        {
            User isUserExist = _context.Users.FirstOrDefault(i => i.Email == userDTO.Email);
            if (isUserExist != null)
            {
                return false;
            }

            User user = new User()
            {
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                DateOfBirth = userDTO.DateOfBirth,
                Gender = userDTO.Gender,
                Email = userDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password, 12),
                Role = 2,
                CreatedDate = DateTime.UtcNow
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return true;

        }

        public User Login(LoginDto loginDTO)
        {
            var user = _context.Users.FirstOrDefault(i => i.Email == loginDTO.Email);
            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password))
                {
                    return user;
                }
            }
            return null;
        }

        public bool UpdatePassword(string email, ResetPasswordDto resetPasswordDTO)
        {
            var user = _context.Users.FirstOrDefault(i => i.Email == email);
            if (BCrypt.Net.BCrypt.Verify(resetPasswordDTO.Password, user.Password))
            {
                return false;
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordDTO.Password, 12);
            _context.Users.Update(user);
            _context.SaveChanges();
            return true;

        }
    }
}
