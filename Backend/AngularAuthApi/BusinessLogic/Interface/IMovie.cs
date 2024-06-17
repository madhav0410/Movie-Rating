using DataAceess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public interface IMovie
    {
        public Task<List<Movie>> GetAllMovies(List<string> selectedGenres);
        public Task<Movie> GetMovieByTitle(string title);
        public Task<double> GetAvgRating(string title);
    }
}
