using BusinessLogic.Interface;
using DataAceess.Data;
using DataAceess.Dto;
using DataAceess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost, Route("/api/auth/signup", Name = "SignUp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
       
        public ActionResult<ResponseDto<NoContentResult>> CreateAccount([FromBody] UserDto userDTO)
        {
            ResponseDto<NoContentResult> response = new ResponseDto<NoContentResult>();
            try
            {
                if(userDTO == null) 
                {
                    throw new Exception("Something went wrong");
                }
                var user = _auth.AddUser(userDTO);
                if (!user)
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


        [HttpPost, Route("/api/auth/login", Name = "Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ResponseDto<string>> Authenticate([FromBody] LoginDto loginDTO)
        {
            ResponseDto<string> response = new ResponseDto<string>();
            try
            {
                if(loginDTO == null)
                {
                    throw new Exception("Something went wrong");
                }
                var user = _auth.Login(loginDTO);
                if(user == null)
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
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return BadRequest(response);
            }
            
        }


        [HttpPost,Route("api/auth/forgotPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ResponseDto<NoContentResult>> ForgotPassword([FromQuery] string email)
        {
            ResponseDto<NoContentResult> response = new ResponseDto<NoContentResult>();
            try
            {
                bool isAccountExist = _context.Users.Any(i => i.Email == email);
                if (!isAccountExist)
                {
                    response.Success = false;
                    response.Message = "User not exits assciated to this email";
                    return NotFound(response);
                }
                else
                {
                    var encryptedEmail = _utilities.EncryptEmail(email, "madhavparmar0410");
                    var time = DateTime.Now;
                    string ResetPasswordLink = "http://localhost:4200/reset-password?email=" + encryptedEmail + "&time=" + time;
                    var subject = "Reset Password Link";
                    var emailBody = $"Please click <a href=\"{ResetPasswordLink}\">here</a> to change the password.";
                    _utilities.SendMail(email, subject, emailBody);
                    response.Success = true;
                    return Ok(response);

                }
            }
            catch(Exception ex) { 
                response.Success = false; 
                response.Message = ex.Message; 
                return BadRequest(response); 
            }
        }


        [HttpPost, Route("api/auth/resetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ResponseDto<NoContentResult>> ResetPassword([FromQuery] string email,[FromQuery] string time, ResetPasswordDto resetPasswordDto)
        {

            ResponseDto<NoContentResult> response = new ResponseDto<NoContentResult>();
            DateTime time1 = DateTime.Parse(time);
            TimeSpan diff = DateTime.Now - time1;
            if (diff.Minutes > 30)
            {
                response.Success = true;
                response.Message = "Reset Password Link is Expired";
                return BadRequest(response);
            }
            if (resetPasswordDto.Password != resetPasswordDto.ConfirmPassword)
            {
                response.Success = false;
                response.Message = "Password and Confirm Password is not same";
                return BadRequest(response);
                
            }
            var decryptedEmail = _utilities.DecryptEmail(email, "madhavparmar0410");
            bool status = _auth.UpdatePassword(decryptedEmail, resetPasswordDto);
            if(!status)
            {
                response.Success = false;
                response.Message = "Please set password other older one";
                return BadRequest(response);
            }
            response.Success = true;
            response.Message = "Password Changed Successfully";
            return Ok(response);
            
        }

    }
}
