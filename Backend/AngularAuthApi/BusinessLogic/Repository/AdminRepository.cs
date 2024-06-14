using BusinessLogic.Interface;
using DataAceess.Data;
using DataAceess.Dto;
using DataAceess.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class AdminRepository : IAdmin
    {
        private readonly ApplicationDbContext _context;
        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool AddMovie(Movie model)
        {
            var isMovieAdded = _context.Movies.FirstOrDefault(i => i.Title == model.Title);
            if(isMovieAdded != null)
            {
                return false;
            }
            else
            {
                Movie movie = new Movie()
                {
                    Title = model.Title,
                    Cast = model.Cast,
                    Director = model.Director,
                    Plot = model.Plot,
                    Genre = model.Genre,
                    Poster = model.Poster,
                    ReleaseDate = model.ReleaseDate.Split('T')[0],
                    Trailer = model.Trailer,
                };
                _context.Movies.Add(movie);
                _context.SaveChanges();
                return true;
            }

        }

        public bool DeleteMovie(int id)
        {
            var isMovieFound = _context.Movies.FirstOrDefault(i => i.Id == id);
            if (isMovieFound == null)
            {
                return false;
            }
            Movie movie = _context.Movies.FirstOrDefault(x => x.Id == id);
            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return true;
        }

        public bool UpdateMovie(Movie model)
        {
            var isMovieFound = _context.Movies.FirstOrDefault(i => i.Id == model.Id);
            if (isMovieFound == null)
            {
                return false;
            }
            Movie movie = _context.Movies.FirstOrDefault(x => x.Id == model.Id);
            movie.Title = model.Title;
            movie.Cast = model.Cast;
            movie.Director = model.Director;
            movie.Plot = model.Plot;
            movie.Genre = model.Genre;
            movie.Poster = model.Poster;
            movie.ReleaseDate = model.ReleaseDate.Split('T')[0];
            movie.Trailer = model.Trailer;
            _context.Movies.Update(movie);
            _context.SaveChanges();
            return true;
        }
    }
}
