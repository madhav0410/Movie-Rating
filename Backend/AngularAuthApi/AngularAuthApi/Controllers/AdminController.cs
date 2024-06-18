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

        /// <summary>
        /// adds a new movie to the database.
        /// </summary>
        /// <param name="model">The movie data to be added, passed in the request body.</param>
        /// <returns>The response indicates whether the movie was successfully added or if an error occurred.</returns>
        /// <remarks>
        /// This method performs the following steps:
        /// 1. Initializes a response object to hold the result.
        /// 2. Validates the input model to ensure it is not null.
        /// 3. Calls the asynchronous method to add the movie to the database.
        /// 4. Checks the result of the add operation and updates the response accordingly:
        ///    - If the movie already exists, returns a <see cref="BadRequest"/> response with a failure message.
        ///    - If the movie is successfully added, returns an <see cref="Ok"/> response with a success message.
        /// 5. Catches and handles any exceptions, returning a <see cref="BadRequest"/> response with the exception message.
        /// </remarks>
        [Authorize]
        [HttpPost, Route("/api/admin/addmovie", Name = "AddMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseDto<NoContentResult>>> AddMovie([FromBody] Movie model)
        {
            ResponseDto<NoContentResult> response = new ResponseDto<NoContentResult>();
            try
            {
                if(!ModelState.IsValid) 
                { 
                    return BadRequest("Model State is Invaild");
                }
                if (model == null)
                {
                    throw new Exception("Something went wrong");
                }

                var movieAdded = await _admin.AddMovie(model);
                if (!movieAdded)
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
                return BadRequest(response);
            }
        }

        /// <summary>
        /// updates an existing movie in the database.
        /// </summary>
        /// <param name="model">The movie data to be updated, passed in the request body.</param>
        /// <returns>The response object indicates whether the movie was successfully updated or if an error occurred.</returns>
        /// <remarks>
        /// This method performs the following steps:
        /// 1. Initializes a response object to hold the result.
        /// 2. Validates the input model to ensure it is not null.
        /// 3. Calls the asynchronous method to update the movie in the database.
        /// 4. Checks the result of the update operation and updates the response accordingly:
        ///    - If the movie is not found (update operation returns false), returns a <see cref="NotFound"/> response with a failure message.
        ///    - If the movie is successfully updated, returns an <see cref="Ok"/> response with a success message.
        /// 5. Catches and handles any exceptions, returning a <see cref="BadRequest"/> response with the exception message.
        /// </remarks>
        [Authorize]
        [HttpPost, Route("/api/admin/updatemovie", Name = "UpdateMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDto<NoContentResult>>> UpdateMovie([FromBody] Movie model)
        {
            ResponseDto<NoContentResult> response = new ResponseDto<NoContentResult>();
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Model State is Invaild");
                }
                if (model == null)
                {
                    throw new Exception("Something went wrong");
                }

                var movieUpdated = await _admin.UpdateMovie(model);
                if (!movieUpdated)
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

        /// <summary>
        /// deletes a movie from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie to delete.</param>
        /// <returns>The response indicates whether the movie was successfully deleted or if an error occurred.</returns>
        /// <remarks>
        /// This method performs the following steps:
        /// 1. Initializes a response object to hold the result.
        /// 2. Validates the input ID to ensure it is not zero.
        /// 3. Calls the asynchronous method to delete the movie from the database.
        /// 4. Checks the result of the delete operation and updates the response accordingly:
        ///    - If the movie is not found (delete operation returns false), returns a <see cref="NotFound"/> response with a failure message.
        ///    - If the movie is successfully deleted, returns an <see cref="Ok"/> response with a success message.
        /// 5. Catches and handles any exceptions, returning a <see cref="BadRequest"/> response with the exception message.
        /// </remarks>
        [Authorize]
        [HttpDelete, Route("/api/admin/deletemovie", Name = "DeleteMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDto<NoContentResult>>> DeleteMovie(int id)
        {
            ResponseDto<NoContentResult> response = new ResponseDto<NoContentResult>();
            try
            {
                if (id == 0)
                {
                    throw new Exception("Something went wrong");
                }

                var movieDeleted = await _admin.DeleteMovie(id);
                if (!movieDeleted)
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
       
