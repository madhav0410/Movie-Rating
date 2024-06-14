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
        public List<Movie> GetAllMovies(List<string> selectedGenres)
        {
            List<Movie> movieList = new List<Movie>();
            if(selectedGenres.Count == 0)
            {
                movieList = _context.Movies.OrderBy(i => i.Id).ToList();
                return movieList;
            }
            else
            {
                movieList = _context.Movies.Where(movie => selectedGenres.All(genre => movie.Genre.Contains(genre))).ToList();
                return movieList;
            }

        }

        public double GetAvgRating(string title)
        {
            var count = _context.Ratings.Where(i => i.Movie == title).Count();
            var list = _context.Ratings.Where(i => i.Movie == title).ToList();
            double avgRating = 0;
            foreach (var record in list) {
                avgRating += record.MovieRating;
            }
            avgRating = avgRating / count;
            return avgRating;
        }

        public Movie GetMovieByTitle(string title)
        {
            var movie = _context.Movies.First(i => i.Title == title);
            return movie;
        }
    }
}
