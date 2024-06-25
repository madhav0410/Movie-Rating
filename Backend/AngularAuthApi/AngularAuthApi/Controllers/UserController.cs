using BusinessLogic.Interface;
using DataAceess.Data;
using DataAceess.Dto;
using DataAceess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularAuthApi.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMovie _movie;
        private readonly IUser _user;

        public UserController(ApplicationDbContext context, IMovie movie,IUser user)
        {
            _context = context;
            _movie = movie; 
            _user = user;
        }

        /// <summary>
        /// Retrieves the average rating for a movie based on its title.
        /// </summary>
        /// <param name="title">The title of the movie.</param>
        /// <returns>A response containing the average rating if successful, or an error message if not.</returns>
        [HttpGet, Route("/api/user/get-avg-rating", Name = "GetAvgRating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseDto<double>>> GetAvgRating(string title)
        {
            ResponseDto<double> response = new ResponseDto<double>();
            try
            {
                if (title == null)
                {
                    throw new Exception("Title cannot be null");
                }

                var isRating = await _context.Ratings.AnyAsync(i => i.Movie == title);
                if (!isRating)
                {
                    response.Success = false;
                    response.Message = "No ratings found for this movie";
                    return Ok(response);
                }

                var avgRating = await _movie.GetAvgRating(title); 
                response.Success = true;
                response.Data = avgRating;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        /// <summary>
        /// Retrieves the rating given by a user for a specific movie asynchronously.
        /// </summary>
        /// <param name="email">The email of the user whose rating is to be retrieved.</param>
        /// <param name="title">The title of the movie for which the rating is to be retrieved.</param>
        /// <returns>Returns an ActionResult with a ResponseDto<int> containing the user's rating for the movie.</returns>
        /// <remarks>
        /// Checks if the provided email is null; if so, throws an Exception.
        /// Retrieves the user's rating asynchronously for the specified movie title using _user.GetUserRating method.
        /// If no rating is found (null), returns a NotFound response with message "No Rating found".
        /// Returns an Ok response with success status and the user's rating upon successful retrieval.
        /// Handles exceptions by returning a BadRequest response with the exception message.
        /// </remarks>
        [Authorize]
        [HttpGet, Route("/api/user/get-user-rating", Name = "GetUserRating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDto<int>>> GetUserRating(string email, string title)
        {
            ResponseDto<int> response = new ResponseDto<int>();
            try
            {
                if (email == null)
                {
                    throw new Exception("Email cannot be null");
                }

                var userRating = await _user.GetUserRating(email, title);
                if (userRating == null)
                {
                    response.Success = false;
                    response.Message = "No Rating found";
                    return NotFound(response);
                }

                response.Success = true;
                response.Data = userRating.MovieRating;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }


        /// <summary>
        /// Updates the rating for a user associated with a specific movie.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="title">The title of the movie.</param>
        /// <param name="rating">The new rating to be updated.</param>
        /// <returns>A response indicating success or failure of the operation.</returns>
        [HttpPost, Route("/api/user/update-rating", Name = "UpdateRating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseDto<NoContentResult>>> UpdateRating(string email, string title, int rating)
        {
            ResponseDto<NoContentResult> response = new ResponseDto<NoContentResult>();
            try
            {
                if (email == null || title == null || rating == 0)
                {
                    throw new Exception("Email, title, or rating cannot be null or zero");
                }

                await _user.UpdateUserRating(email, title, rating);

                response.Success = true;
                response.Message = "Rating updated";
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

