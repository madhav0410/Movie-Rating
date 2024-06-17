using BusinessLogic.Interface;
using DataAceess.Data;
using DataAceess.Dto;
using DataAceess.Models;
using Microsoft.EntityFrameworkCore;
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
        public async Task<bool> AddUser(UserDto userDTO)
        {
            var isUserExist = await _context.Users.FirstOrDefaultAsync(i => i.Email == userDTO.Email);
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

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<User> Login(LoginDto loginDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(i => i.Email == loginDTO.Email);
            if (user != null && BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password))
            {
                return user;
            }
            return null; 
        }


        public async Task<bool> UpdatePassword(string email, ResetPasswordDto resetPasswordDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(i => i.Email == email);
            if (user == null)
            {
                return false;
            }
            if (BCrypt.Net.BCrypt.Verify(resetPasswordDTO.Password, user.Password))
            {
                return false; 
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordDTO.Password, 12);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
