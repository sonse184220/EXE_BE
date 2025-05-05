using Contract.Dtos.Enums;
using Contract.Dtos.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
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
        public AuthController(IAuthService authService, IUserService userService, ICloudinaryImageService cloudinaryImageService, IEmailService emailService)
        {
            _authService = authService;
            _userService = userService;
            _cloudinaryImageService = cloudinaryImageService;
            _emailService = emailService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {

            var result = await _authService.RegisterAccountAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string token)
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
                var existingUserName = await _userService.GetByUserNameAsync(request.FullName);
                if (existingUserName != null)
                {
                    return BadRequest("User name already existed");
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
                user.FullName = request.FullName;
                user.DateOfBirth = request.DateOfBirth;
                user.Gender = request.Gender.ToString();
                user.PhoneNumber = request.PhoneNumber;
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


    }
}
