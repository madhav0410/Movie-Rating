using BusinessLogic.Interface;
using DataAceess.Data;
using DataAceess.Dto;
using DataAceess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AngularAuthApi.Controllers
{
    public class AdminController : Controller
    {
       
        private readonly IAdmin _admin;
        public AdminController( IAdmin admin)
        {
            _admin = admin;
        }

        [Authorize]
        [HttpPost, Route("/api/admin/addmovie", Name = "AddMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ResponseDto<NoContentResult>> AddMovie([FromBody] Movie model)
        {
            ResponseDto<NoContentResult> response = new ResponseDto<NoContentResult>();
            try
            {
                if (model == null)
                {
                    throw new Exception("Something went wrong");
                }
                var movie = _admin.AddMovie(model);
                if (!movie)
                {
                    response.Success = false;
                    response.Message = "Movie already added in db";
                    return BadRequest(response);
                }
                response.Success = true;
                response.Message = "Movie added successfully";
                return Ok(response);

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }

        }

        [Authorize]
        [HttpPost, Route("/api/admin/updatemovie", Name = "UpdateMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ResponseDto<NoContentResult>> UpdateMovie([FromBody] Movie model)
        {
            ResponseDto<NoContentResult> response = new ResponseDto<NoContentResult>();
            try
            {
                if (model == null)
                {
                    throw new Exception("Something went wrong");
                }
                var movie = _admin.UpdateMovie(model);
                if (!movie)
                {
                    response.Success = false;
                    response.Message = "Movie not found";
                    return NotFound(response);
                }
                response.Success = true;
                response.Message = "Movie updated successfully";
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
        [HttpDelete, Route("/api/admin/deletemovie", Name = "DeleteMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ResponseDto<NoContentResult>> DeleteMovie(int id)
        {
            ResponseDto<NoContentResult> response = new ResponseDto<NoContentResult>();
            try
            {
                if (id == 0)
                {
                    throw new Exception("Something went wrong");
                }
                var movie = _admin.DeleteMovie(id);
                if (!movie)
                {
                    response.Success = false;
                    response.Message = "Movie Not Found";
                    return NotFound(response);
                } 
                response.Success = true;
                response.Message = "Movie Deleted Successfully";
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
       
