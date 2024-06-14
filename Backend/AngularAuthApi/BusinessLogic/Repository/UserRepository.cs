using BusinessLogic.Interface;
using DataAceess.Data;
using DataAceess.Models;
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

        public Rating GetUserRating(string email, string title)
        {
           var rating = _context.Ratings.FirstOrDefault(i => i.UserEmail == email && i.Movie == title);
           return rating;
        }

        public void UpdateUserRating(string email, string title, int rating)
        {
            var userRating = _context.Ratings.FirstOrDefault(i => i.UserEmail == email && i.Movie == title);
            if(userRating != null)
            {
                userRating.MovieRating = rating;
                _context.Ratings.Update(userRating);
                _context.SaveChanges();
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
                _context.SaveChanges();
            }
        }
    }
}
