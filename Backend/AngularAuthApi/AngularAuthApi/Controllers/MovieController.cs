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
        private readonly IMovie _movie;
        public MovieController(ApplicationDbContext context, IMovie movie)
        {
            _movie = movie;
        }

        /// <summary>
        /// Retrieves a paginated list of movies based on specified page, page size, and genres.
        /// </summary>
        /// <param name="page">The page number of the movie list to retrieve.</param>
        /// <param name="pageSize">The number of movies per page.</param>
        /// <param name="genres">A comma-separated string of genres to filter the movies by.</param>
        /// <returns>Returns an ActionResult with a ResponseDto<PaginatedMoviesDto> containing the paginated list of movies.</returns>
        /// <remarks>
        /// Splits the genres string into a list of selected genres. If genres is null, an empty list is used.
        /// Retrieves all movies asynchronously based on selected genres using _movie.GetAllMovies method.
        /// Calculates total items and total pages for pagination based on the retrieved movie list and page size.
        /// Retrieves a paginated subset of movies based on page and page size from the retrieved movie list.
        /// Constructs a PaginatedMoviesDto object containing total pages and paginated movie list.
        /// Returns an Ok response with success status and the PaginatedMoviesDto upon successful retrieval.
        /// </remarks>
        [HttpGet, Route("/api/movie/getallmovies", Name = "GetMovies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResponseDto<PaginatedMoviesDto>>> GetMovies(int page, int pageSize, string genres)
        {
            ResponseDto<PaginatedMoviesDto> response = new ResponseDto<PaginatedMoviesDto>();
            var selectedGenres = genres?.Split(',').ToList() ?? new List<string>();

            List<Movie> movieList = await _movie.GetAllMovies(selectedGenres);

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
        /// Retrieves a movie by its title.
        /// </summary>
        /// <param name="title">The title of the movie to retrieve.</param>
        /// <returns>Returns an ActionResult with a ResponseDto<Movie> containing the retrieved movie.</returns>
        /// <remarks>
        /// Checks if the provided title is null or empty; if so, throws an exception with message "Something went wrong".
        /// Retrieves the movie asynchronously by its title using _movie.GetMovieByTitle method.
        /// If the movie is not found (null), returns a NotFound response with message "Movie not found".
        /// Returns an Ok response with success status and the retrieved movie upon successful retrieval.
        /// Handles exceptions by returning a BadRequest response with the exception message.
        /// </remarks>
        [HttpGet, Route("/api/movie/getmovie", Name = "GetMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDto<Movie>>> GetMovie([FromQuery] string title)
        {
            ResponseDto<Movie> response = new ResponseDto<Movie>();
            try
            {
                if (string.IsNullOrEmpty(title))
                {
                    throw new Exception("Something went wrong");
                }

                var movie = await _movie.GetMovieByTitle(title);
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
