using BusinessLogic.Interface;
using DataAceess.Data;
using DataAceess.Dto;
using DataAceess.Models;
using Microsoft.EntityFrameworkCore;
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
        public async Task<bool> AddMovie(Movie model)
        {
            var isMovieAdded = await _context.Movies.FirstOrDefaultAsync(i => i.Title == model.Title);
            if (isMovieAdded != null)
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
                await _context.Movies.AddAsync(movie);
                await _context.SaveChangesAsync();
                return true;
            }
        }


        public async Task<bool> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(i => i.Id == id);
            if (movie == null)
            {
                return false;
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> UpdateMovie(Movie model)
        {
            var existingMovie = await _context.Movies.FirstOrDefaultAsync(i => i.Id == model.Id);
            if (existingMovie == null)
            {
                return false;
            }

            existingMovie.Title = model.Title;
            existingMovie.Cast = model.Cast;
            existingMovie.Director = model.Director;
            existingMovie.Plot = model.Plot;
            existingMovie.Genre = model.Genre;
            existingMovie.Poster = model.Poster;
            existingMovie.ReleaseDate = model.ReleaseDate.Split('T')[0];
            existingMovie.Trailer = model.Trailer;

            _context.Movies.Update(existingMovie);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
