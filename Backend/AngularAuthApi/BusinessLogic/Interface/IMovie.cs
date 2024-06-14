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
        public List<Movie> GetAllMovies(List<string> selectedGenres);
        public Movie GetMovieByTitle(string title);
        public double GetAvgRating(string title);
    }
}
