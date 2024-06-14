using BusinessLogic.Interface;
using DataAceess.Data;
using DataAceess.Dto;
using DataAceess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularAuthApi.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMovie _movie;
        public MovieController(ApplicationDbContext context, IMovie movie)
        {
            _context = context;
            _movie = movie;
        }


        [HttpGet, Route("/api/movie/getallmovies", Name = "GetMovies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<ResponseDto<PaginatedMoviesDto>> GetMovies(int page, int pageSize, string genres)
        {
            ResponseDto<PaginatedMoviesDto> response = new ResponseDto<PaginatedMoviesDto>();
            var selectedGenres = genres?.Split(',').ToList() ?? new List<string>();
            List<Movie> movieList = _movie.GetAllMovies(selectedGenres);
            int totalItems = movieList.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var paginatedList = movieList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PaginatedMoviesDto paginatedMoviesDto = new PaginatedMoviesDto
            {
                TotalPages = totalPages,
                PaginatedList = paginatedList
            };

            response.Success = true;
            response.Data = paginatedMoviesDto;
            return Ok(response);
        }


        [HttpGet, Route("/api/movie/getmovie", Name = "GetMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ResponseDto<Movie>> GetMovie([FromQuery] string title)
        {
            ResponseDto<Movie> response = new ResponseDto<Movie>();
            try
            {
                if (title == null)
                {
                    throw new Exception("Something went wrong");
                }
                var movie = _movie.GetMovieByTitle(title);
                if (movie == null)
                {
                    response.Success = false;
                    response.Message = "Movie not found";
                    return NotFound(response);
                }
                response.Success = true;
                response.Data = movie;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }
    }
}
