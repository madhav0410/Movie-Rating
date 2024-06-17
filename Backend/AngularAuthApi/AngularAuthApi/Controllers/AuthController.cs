using BusinessLogic.Interface;
using DataAceess.Data;
using DataAceess.Dto;
using DataAceess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AngularAuthApi.Controllers
{
    
    [ApiController]
    public class AuthController : Controller
    {
        private ApplicationDbContext _context;
        private readonly IAuth _auth;
        private readonly IJwtService _jwtService;
        private readonly IUtilities _utilities;

        public AuthController(ApplicationDbContext context,IAuth auth, IJwtService jwtService,IUtilities utilities)
        {
            _context = context;
            _auth = auth;
            _jwtService = jwtService;
            _utilities = utilities;
        }

        /// <summary>
        /// Creates a new user account based on the provided UserDto.
        /// </summary>
        /// <param name="userDTO">The UserDto object containing user information.</param>
        /// <returns>Returns an ActionResult with a ResponseDto indicating the operation status.</returns>
        /// <remarks>
        /// If userDTO is null, throws an Exception with message "Something went wrong".
        /// Attempts to add the user asynchronously using the _auth service.
        /// If the user already exists (checked by email), returns a BadRequest response.
        /// Returns an Ok response with success message "User registered successfully" upon successful registration.
        /// Handles exceptions by returning a BadRequest response with the exception message.
        /// </remarks>
        [HttpPost, Route("/api/auth/signup", Name = "SignUp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseDto<NoContentResult>>> CreateAccount([FromBody] UserDto userDTO)
        {
            ResponseDto<NoContentResult> response = new ResponseDto<NoContentResult>();
            try
            {
                if (userDTO == null)
                {
                    throw new Exception("Something went wrong");
                }

                var userAdded = await _auth.AddUser(userDTO); 

                if (!userAdded)
                {
                    response.Success = false;
                    response.Message = "User already exists associated with this email";
                    return BadRequest(response);
                }

                response.Success = true;
                response.Message = "User registered successfully";
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
        /// Authenticates a user based on the provided LoginDto.
        /// </summary>
        /// <param name="loginDTO">The LoginDto object containing user login credentials.</param>
        /// <returns>Returns an ActionResult with a ResponseDto<string> indicating the authentication status and JWT token.</returns>
        /// <remarks>
        /// If loginDTO is null, throws an Exception with message "Something went wrong".
        /// Attempts to authenticate the user asynchronously using the _auth service.
        /// If the user is not found, returns a NotFound response with message "User Not Found".
        /// Returns an Ok response with success message "Login Successful" and a JWT token upon successful authentication.
        /// Handles exceptions by returning a BadRequest response with the exception message.
        /// </remarks>
        [HttpPost, Route("/api/auth/login", Name = "Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDto<string>>> Authenticate([FromBody] LoginDto loginDTO)
        {
            ResponseDto<string> response = new ResponseDto<string>();
            try
            {
                if (loginDTO == null)
                {
                    throw new Exception("Something went wrong");
                }

                var user = await _auth.Login(loginDTO); 

                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User Not Found";
                    return NotFound(response);
                }

                response.Success = true;
                response.Message = "Login Successfull";
                response.Data = _jwtService.GetJwtToken(user.Email, user.Role);
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
        /// Initiates the process for resetting the password by sending a reset password link to the user's email.
        /// </summary>
        /// <param name="email">The email address associated with the user account.</param>
        /// <returns>Returns an ActionResult with a ResponseDto<NoContentResult> indicating the status of the password reset initiation.</returns>
        /// <remarks>
        /// Checks if an account exists asynchronously based on the provided email address using _context.Users.
        /// If no account exists, returns a NotFound response with message "User not exists associated with this email".
        /// If the account exists, generates a reset password link with encrypted email and current timestamp.
        /// Sends an email asynchronously to the user's email address containing the reset password link.
        /// Returns an Ok response with success message upon successful initiation of the password reset process.
        /// Handles exceptions by returning a BadRequest response with the exception message.
        /// </remarks>
        [HttpPost,Route("api/auth/forgotPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseDto<NoContentResult>>> ForgotPassword([FromQuery] string email)
        {
            ResponseDto<NoContentResult> response = new ResponseDto<NoContentResult>();
            try
            {
                bool isAccountExist = await _context.Users.AnyAsync(i => i.Email == email);
                if (!isAccountExist)
                {
                    response.Success = false;
                    response.Message = "User not exists associated with this email";
                    return NotFound(response);
                }
                else
                {
                    var encryptedEmail = _utilities.EncryptEmail(email, "madhavparmar0410");
                    var time = DateTime.Now;
                    string resetPasswordLink = "http://localhost:4200/account/reset-password?email=" + encryptedEmail + "&time=" + time;
                    var subject = "Reset Password Link";
                    var emailBody = $"Please click <a href=\"{resetPasswordLink}\">here</a> to change the password.";

                    await  _utilities.SendMail(email, subject, emailBody);

                    response.Success = true;
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }


        /// <summary>
        /// Resets the password for a user based on the provided reset password link details and new password.
        /// </summary>
        /// <param name="email">The encrypted email associated with the user account.</param>
        /// <param name="time">The timestamp indicating when the reset password link was generated.</param>
        /// <param name="resetPasswordDto">The ResetPasswordDto object containing the new password and confirm password.</param>
        /// <returns>Returns an ActionResult with a ResponseDto<NoContentResult> indicating the status of the password reset.</returns>
        /// <remarks>
        /// Parses the provided time string to DateTime and calculates the time difference with the current time.
        /// If the time difference is more than 30 minutes, returns a BadRequest response with message "Reset Password Link has expired".
        /// Checks if the new password and confirm password in resetPasswordDto match; if not, returns a BadRequest response with message "Password and Confirm Password do not match".
        /// Decrypts the provided email to retrieve the original email address.
        /// Updates the password asynchronously using _auth.UpdatePassword method.
        /// If the password update fails, returns a BadRequest response with message "Failed to update password".
        /// Returns an Ok response with success message "Password changed successfully" upon successful password reset.
        /// Handles exceptions by returning a BadRequest response with the exception message.
        /// </remarks>
        [HttpPost, Route("api/auth/resetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseDto<NoContentResult>>> ResetPassword([FromQuery] string email, [FromQuery] string time, ResetPasswordDto resetPasswordDto)
        {
            ResponseDto<NoContentResult> response = new ResponseDto<NoContentResult>();

            try
            {
                DateTime time1 = DateTime.Parse(time);
                TimeSpan diff = DateTime.Now - time1;

                if (diff.Minutes > 30)
                {
                    response.Success = false;
                    response.Message = "Reset Password Link has expired";
                    return BadRequest(response);
                }

                if (resetPasswordDto.Password != resetPasswordDto.ConfirmPassword)
                {
                    response.Success = false;
                    response.Message = "Password and Confirm Password do not match";
                    return BadRequest(response);
                }

                var decryptedEmail = _utilities.DecryptEmail(email, "madhavparmar0410");
                var updateStatus = await _auth.UpdatePassword(decryptedEmail, resetPasswordDto);

                if (!updateStatus)
                {
                    response.Success = false;
                    response.Message = "Failed to updated password";
                    return BadRequest(response);
                }

                response.Success = true;
                response.Message = "Password changed successfully";
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
