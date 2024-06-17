using BusinessLogic.Interface;
using DataAceess.Data;
using DataAceess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class UserRepository : IUser
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Rating> GetUserRating(string email, string title)
        {
            var rating = await _context.Ratings.FirstOrDefaultAsync(i => i.UserEmail == email && i.Movie == title);
            return rating;
        }

        public async Task UpdateUserRating(string email, string title, int rating)
        {
            var userRating = await _context.Ratings.FirstOrDefaultAsync(i => i.UserEmail == email && i.Movie == title);

            if (userRating != null)
            {
                userRating.MovieRating = rating;
                _context.Ratings.Update(userRating);
            }
            else
            {
                Rating newRating = new Rating()
                {
                    UserEmail = email,
                    Movie = title,
                    MovieRating = rating
                };
                _context.Ratings.Add(newRating);
            }
            await _context.SaveChangesAsync();

            
        }
    }
}
