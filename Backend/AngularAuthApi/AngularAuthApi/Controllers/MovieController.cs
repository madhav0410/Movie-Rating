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

        /// <summary>
        /// Gets a movies by selected genres
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">No movies listed on per page</param>
        /// <param name="genres">The Genres of the movies to retrive</param>
        /// <returns>The list of movies with specified genres</returns>
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


        /// <summary>
        /// Gets a movie by title
        /// </summary>
        /// <param name="title">The title of the movie to retrive</param>
        /// <returns>The movie with the specified title</returns>
        /// <response code="200">Returns the movie.</response>
        /// <response code="400">If title is null</response>
        /// <response code="404">If the movie is not found.</response>
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
