using Contract.Dtos.Enums;
using Contract.Dtos.Requests;
using Contract.Dtos.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System.Security.Authentication;
using System.Text;

namespace InnerChildApi.Controllers
{
    [Route("innerchild/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ICloudinaryImageService _cloudinaryImageService;
        public AuthController(IAuthService authService, IUserService userService, ICloudinaryImageService cloudinaryImageService)
        {
            _authService = authService;
            _userService = userService;
            _cloudinaryImageService = cloudinaryImageService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _authService.RegisterAccountAsync(request);
                return Ok(new RegisterResponse()
                {
                    IsSuccess = true,
                    Message = "User created, please check your email to confirm account",
                });
            }
            catch (InvalidCredentialException ex)
            {
                return BadRequest(new RegisterResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                }); ;
            }
            catch (Exception ex)
            {
                return StatusCode(500,new RegisterResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                });
            }
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    return BadRequest("Missing token");
                }
                var userId = _authService.ValidateEmailConfirmationToken(token);
                if (userId == null)
                {
                    return BadRequest("Invalid or expired token!");
                }
                var result = await _authService.VerifyAccount(userId);
                if (!result)
                {
                    return NotFound("User not found.");
                }
                return Ok("Email confirmed");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
         

        }
        [HttpPut("update-profile/{id}")]
        public async Task<IActionResult> UpdateProfile(string id, [FromForm] UpdateProfileRequest request)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound("User not found");
                }
                if (request.FullName != null)
                {
                    var existingUserName = await _userService.GetByUserNameAsync(request.FullName);
                    if (existingUserName != null && existingUserName.UserId != user.UserId)
                    {
                        return BadRequest("User name already existed");
                    }
                    user.FullName = request.FullName;
                }
                if (request.ProfilePicture != null)
                {
                    if (!string.IsNullOrEmpty(user.ProfilePicture))
                    {
                        await _cloudinaryImageService.DeleteImageAsync(user.ProfilePicture);
                    }
                var avatarUrl = await _cloudinaryImageService.UploadImageAsync(request.ProfilePicture, CloudinaryFolderEnum.UserPicture.ToString());
                user.ProfilePicture = avatarUrl;
                }
                
                user.DateOfBirth = request.DateOfBirth ?? user.DateOfBirth;
                if (request.Gender != null)
                {
                    user.Gender = request.Gender.ToString();
                }
                user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
                var result = await _userService.UpdateUserAsync(user);
                if (result > 0)
                {
                    return Ok("Profile updated successfully");
                }
                return StatusCode(500, "Failed to update profile");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Failed to update profile, " + ex.Message);
            }
            
            
        }
        [HttpPost("check-login")]
        public async Task<IActionResult> CheckLogin([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _authService.CheckLoginAccountAsync(request);
                return Ok(result);

            }
            catch (InvalidCredentialException ex) 
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
           
         
        }
        [HttpPost("login")]
        public async  Task<IActionResult> Login([FromBody] FinalLoginRequest request)
        {
         
            try
            {
                var result = await _authService.LoginAccountAsync(request.UserId,request.ProfileId);
                return Ok(result);
            }
            catch (InvalidCredentialException ex) 
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }





    }
}
