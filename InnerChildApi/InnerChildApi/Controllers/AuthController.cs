using Contract.Common.Enums;
using Contract.Dtos.Requests.Auth;
using Contract.Dtos.Responses.Auth;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using System.Security.Authentication;
using System.Security.Claims;

namespace InnerChildApi.Controllers
{
    [Route("innerchild/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        public AuthController(IAuthService authService, IUserService userService, ICloudinaryService cloudinaryService, ITokenService tokenService, IEmailService emailService)
        {
            _authService = authService;
            _userService = userService;
            _cloudinaryService = cloudinaryService;
            _tokenService = tokenService;
            _emailService = emailService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (request.ProfilePicture != null)
            {
                if (!request.ProfilePicture.ContentType.StartsWith("image/"))
                {
                    return BadRequest($"{request.ProfilePicture.FileName} is not image file");
                }
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
                return StatusCode(500, new RegisterResponse()
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

                var userId = _tokenService.ValidateEmailConfirmationToken(token);
                if (userId == null)
                {
                    return BadRequest("Invalid or expired token!");
                }
                var user = await _userService.GetUserByIdAsync(userId);
                if (user.Verified == true)
                {
                    return BadRequest("Email already confirmed");
                }
                var result = await _emailService.VerifyAccount(userId);
                if (!result)
                {
                    return NotFound("User not found.");
                }
                return Ok("Email confirmed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }
        [HttpGet("resend-email-confirmation")]
        public async Task<IActionResult> ResendEmailConfirmation(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest("Missing email");
                }
                var user = await _userService.GetByEmailAsync(email);
                if (user == null)
                {
                    return NotFound("User not found.");
                }
                if (user.Verified == true)
                {
                    return BadRequest("Email already confirmed");
                }
                var confirmLink = _tokenService.GenerateEmailConfirmationLink(user.UserId);
                await _emailService.SendConfirmationEmailAsync(user.Email, user.FullName, confirmLink);
                return Ok("Email confirmation resent");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize]
        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateProfile([FromForm] ProfileUpdateRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid token or missing token");
            if (request.ProfilePicture != null)
            {
                if (!request.ProfilePicture.ContentType.StartsWith("image/"))
                {
                    return BadRequest($"{request.ProfilePicture.FileName} is not image file");
                }
            }
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
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
                        await _cloudinaryService.DeleteAsync(user.ProfilePicture);
                    }
                    var imageUploadParams = _cloudinaryService.CreateUploadParams(request.ProfilePicture, CloudinaryFolderEnum.UserPicture.ToString());
                    var avatarUrl = await _cloudinaryService.UploadAsync(imageUploadParams, request.ProfilePicture);
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
                    return NoContent();
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
                return StatusCode(500, ex.Message);
            }


        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] FinalLoginRequest request)
        {

            try
            {
                var user = _tokenService.ValidatePreLoginJwtToken(request.Token);
                if (user == null)
                {
                    throw new Exception("Validation failed");
                }
                var result = await _authService.LoginAccountAsync(user.UserId, user.ProfileId);
                return Ok(result);
            }
            catch (InvalidCredentialException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid token or missing token");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _authService.ChangePassword(userId, request.CurrentPassword, request.ConfirmPassword);
                return Ok("Password changed");
            }
            catch (InvalidCredentialException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpPost("check-login-firebase")]
        public async Task<IActionResult> CheckLoginFirebase([FromBody] FirebaseTokenRequest request)
        {
            try
            {
                var result = await _authService.AuthenticateWithFirebaseAsync(request);
                return Ok(result);
            }
            catch (InvalidCredentialException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (FirebaseAuthException ex)
            {
                throw new UnauthorizedAccessException("Firebase authentication failed: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error authorization:" + ex.Message);
            }
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var storedToken = await _tokenService.GetByRefreshTokenAsync(request.RefreshToken);
                if (storedToken == null || storedToken.IsRevoked == true || storedToken.ExpiresAt < DateTime.UtcNow)
                {
                    return Unauthorized("Invalid or expired refresh token");
                }
                await _tokenService.RevokeTokenAsync(storedToken);
                var result = await _authService.LoginAccountAsync(storedToken.UserId, storedToken.ProfileId);
                return Ok(result);
            }
            catch (InvalidCredentialException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }










        //forget password section


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = await _userService.GetByEmailAsync(request.Email);
                if (user == null)
                {
                    return NotFound("User not found");
                }
                if (user.Verified == false)
                {
                    return BadRequest("Email not confirmed");
                }
                var token = _tokenService.GenerateForgotPasswordToken(user.UserId);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userId = _tokenService.ValidateForgotPasswordToken(request.Token);
                if (userId == null)
                {
                    return NotFound("User not found");
                }
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("User not found");
                }
                if (user.Verified == false)
                {
                    return BadRequest("Email not confirmed");
                }

                var token = _tokenService.GenerateResetPasswordToken(userId, request.NewPassword);
                var resetLink = _tokenService.GenerateEmailConfirmationResetPasswordLink(token);
                await _emailService.SendResetPasswordEmailAsync(user.Email, user.FullName, resetLink);
                return Ok("Reset password link has been sent, please check gmail");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("verify-reset-password")]
        public async Task<IActionResult> VerifyResetPassword(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    return BadRequest("Missing token");
                }
                var result = _tokenService.ValidateResetPasswordToken(token);
                var user = await _userService.GetUserByIdAsync(result.userId);
                if (user == null)
                {
                    return NotFound("User not found");
                }
                user.PasswordHash = result.newPasswordHash;
                var updateResult = await _userService.UpdateUserAsync(user);
                return Ok($"Password changed for {user.Email}. You can now close this tab!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize]
        [HttpGet("personal-detail")]
        public async Task<IActionResult> GetPersonalDetail()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                NotFound("User not found");
            }
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var result = new UserDetailResponse()
            {
                UserId = user.UserId,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                FullName = user.FullName,
                Gender = user.Gender,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture
            };
            return Ok(result);
        }
        [HttpGet("get-user-detail/{userId}")]
        public async Task<IActionResult> GetUserDetail(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return NotFound("User not found");
            }
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var result = new UserDetailResponse()
            {
                UserId = user.UserId,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                FullName = user.FullName,
                Gender = user.Gender,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture
            };
            return Ok(result);
        }
        [Authorize]
        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var result = users.Select(x => new AllUsersResponse()
            {
                UserId = x.UserId,
                Email = x.Email,
                FullName = x.FullName,
                DateOfBirth = x.DateOfBirth,
                Gender = x.Gender,
                PhoneNumber = x.PhoneNumber,
                ProfilePicture = x.ProfilePicture,
                CreatedAt = x.CreatedAt,
                LastLoginDate = x.CreatedAt,
                Status = x.Status,
                Verified = x.Verified,
                RoleName = x.Role?.RoleName,
            });
            return Ok(result);
        }

    }
}
