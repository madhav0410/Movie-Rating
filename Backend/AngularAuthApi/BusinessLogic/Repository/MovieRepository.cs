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
    public class MovieRepository: IMovie
    {
        private readonly ApplicationDbContext _context;
        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Movie>> GetAllMovies(List<string> selectedGenres)
        {
            List<Movie> movieList;
            if (selectedGenres.Count == 0)
            {
                movieList = await _context.Movies.OrderBy(i => i.Id).ToListAsync();
            }
            else
            {
                movieList = await _context.Movies
                    .Where(movie => selectedGenres.All(genre => movie.Genre.Contains(genre)))
                    .OrderBy(i => i.Id)
                    .ToListAsync();
            }

            return movieList;
        }


        public async Task<double> GetAvgRating(string title)
        {
            var count = await _context.Ratings.Where(i => i.Movie == title).CountAsync();
            var list = await _context.Ratings.Where(i => i.Movie == title).ToListAsync();
            double avgRating = 0;
            foreach (var record in list) {
                avgRating += record.MovieRating;
            }
            avgRating = avgRating / count;
            return avgRating;
        }

        public async Task<Movie> GetMovieByTitle(string title)
        {
            return await _context.Movies.FirstOrDefaultAsync(movie => movie.Title == title);
        }

    }
}
