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

       
        [HttpGet, Route("/api/user/get-avg-rating", Name = "GetAvgRating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ResponseDto<double>> GetAvgRating(string title)
        {
            ResponseDto<double> response = new ResponseDto<double>();
            try
            {
                if (title == null)
                {
                    throw new Exception();
                }
                var isRating = _context.Ratings.Any(i => i.Movie == title);
                if (!isRating)
                {
                    response.Success = false;
                    response.Message = "No Rating not found";
                    return NotFound(response);
                }
                var avgRating = _movie.GetAvgRating(title);
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


        [Authorize]
        [HttpGet, Route("/api/user/get-user-rating", Name = "GetUserRating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ResponseDto<int>> GetUserRating(string email, string title)
        {
            ResponseDto<int> response = new ResponseDto<int>();
            try
            {
                if (email == null)
                {
                    throw new Exception();
                }
                var userRating = _user.GetUserRating(email,title);
                if (userRating == null)
                {
                    response.Success = false;
                    response.Message = "No Rating not found";
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


        [HttpPost, Route("/api/user/update-rating", Name = "UpdateRating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ResponseDto<NoContentResult>> UpdateRating(string email,string title,int rating) 
        {
            ResponseDto<NoContentResult> response = new ResponseDto<NoContentResult>();
            try
            {
                if(email == null || title == null || rating == 0) 
                {
                    throw new Exception("Something went wrong");
                }
                _user.UpdateUserRating(email,title,rating);
                response.Success = true;
                response.Message = "Rating updates";
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

