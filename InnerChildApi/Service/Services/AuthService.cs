using Contract.Common.Enums;
using Contract.Dtos.Requests.Auth;
using Contract.Dtos.Responses.Auth;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using Repository.Interfaces;
using Repository.Models;
using Service.Interfaces;
using System.Security.Authentication;

namespace Service.Services
{
    public class AuthService : IAuthService
    {
        //repo
        private readonly IAccountRepository _accountRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly IRefreshTokenRepository _refreshTokenRepo;

        private readonly IEmailService _emailService;
        private readonly IProfileRepository _profileRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ITokenService _tokenService;
        private readonly ISessionService _sessionService;
        public AuthService(IAccountRepository accountRepo, IEmailService emailService, IProfileRepository profileRepo, IRoleRepository roleRepo, IHttpContextAccessor httpContextAccessor, ICloudinaryService cloudinaryService, ISessionService sessionService, ITokenService tokenService, IRefreshTokenRepository refreshTokenRepo)
        {
            _accountRepo = accountRepo;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _profileRepo = profileRepo;
            _roleRepo = roleRepo;
            _cloudinaryService = cloudinaryService;
            _tokenService = tokenService;
            _sessionService = sessionService;
            _refreshTokenRepo = refreshTokenRepo;
        }
        #region register acccount async
        public async Task RegisterAccountAsync(RegisterRequest request)
        {
            var userByName = await _accountRepo.GetByUserNameAsync(request.FullName);
            if (userByName != null)
            {
                throw new InvalidCredentialException("Name already existed");
            }
            var userByEmail = await _accountRepo.GetByEmailAsync(request.Email);
            if (userByEmail != null)
            {
                throw new InvalidCredentialException("Email already existed");
            }
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var role = await _roleRepo.GetByRoleNameAsync(RoleEnum.User.ToString());
            string imageUrl = null;
            if (request.ProfilePicture != null)
            {
                var uploadParams = _cloudinaryService.CreateUploadParams(request.ProfilePicture, CloudinaryFolderEnum.UserPicture.ToString());
                imageUrl = await _cloudinaryService.UploadAsync(uploadParams, request.ProfilePicture);
            }
            var user = new User
            {
                UserId = Guid.NewGuid().ToString(),
                Email = request.Email,
                PasswordHash = hashedPassword,
                FullName = request.FullName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.gender.ToString(),
                PhoneNumber = request.PhoneNumber,
                ProfilePicture = imageUrl,
                CreatedAt = DateTime.UtcNow,
                RoleId = role.RoleId,
                Verified = false
            };
            var userCreated = await _accountRepo.CreateUserAsync(user);
            if (userCreated > 0)
            {
                var profile = new Profile()
                {
                    ProfileId = Guid.NewGuid().ToString(),
                    UserId = user.UserId,
                    ProfileStatus = UserAccountEnum.Active.ToString(),
                };
                await _profileRepo.CreateProfileAsync(profile);
            }
            var emailConfirmationLink = _tokenService.GenerateEmailConfirmationLink(user.UserId);
            await _emailService.SendConfirmationEmailAsync(user.Email, user.FullName, emailConfirmationLink);
        }
        #endregion



        #region check login account async
        public async Task<List<PreLoginResponse>> CheckLoginAccountAsync(LoginRequest request)
        {
            var existingUser = await _accountRepo.GetByEmailAsync(request.Email);
            if (existingUser != null && existingUser.PasswordHash == null)
            {
                throw new InvalidCredentialException("User already has this account link with another sign in method.");
            }
            if (existingUser == null)
            {
                throw new InvalidCredentialException("User not found.");
            }
            if (!BCrypt.Net.BCrypt.Verify(request.Password, existingUser.PasswordHash))
            {
                throw new InvalidCredentialException("Invalid password.");
            }
            if (existingUser.Verified == false)
            {
                throw new InvalidCredentialException("Email not verified.");
            }
            var userProfiles = await _accountRepo.GetUserProfilesAsync(existingUser.UserId);

            var result = _tokenService.GeneratePreLoginJwtTokens(userProfiles);
            return result;
        }
        #endregion
        #region Final  Login Account Async
        public async Task<FinalLoginResponse> LoginAccountAsync(string userId, string profileId)
        {
            var user = await _accountRepo.GetByUserIdAsync(userId);
            if (user == null)
            {
                throw new InvalidCredentialException("User not found.");
            }
            var profile = await _profileRepo.GetByProfileIdAsync(profileId);
            if (profile == null)
            {
                throw new InvalidCredentialException("Profile not found.");
            }
            var sessionId = Guid.NewGuid().ToString();
            var userSession = new Session()
            {
                SessionId = sessionId,
                UserId = user.UserId,
                ProfileId = profileId,
                Token = sessionId,
                SessionIsActive = true,
            };
            var sessionCreated = await _sessionService.CreateSessionAsync(userSession);
            if (sessionCreated > 0)
            {
                await _sessionService.InvalidateOtherSessionsAsync(user.UserId, profileId, sessionId);
            }
            var accessToken = _tokenService.GenerateFinalLoginJwtToken(user.UserId, user.Email, profile.ProfileId, sessionId);
            var refreshToken = await _tokenService.GenerateRefreshToken(user.UserId, profile.ProfileId);
            return new FinalLoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }
        #endregion




















        #region change password function
        public async Task ChangePassword(string userId, string currentPassword, string newPassword)
        {
            var user = await _accountRepo.GetByUserIdAsync(userId);
            if (user == null)
            {
                throw new InvalidCredentialException("User not found.");
            }
            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
            {
                throw new InvalidCredentialException("Wrong current password please try again.");
            }
            var newPasswordHashed = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.PasswordHash = newPasswordHashed;
            await _accountRepo.UpdateUserAsync(user);
        }

        #endregion

        #region firebase authen
        public async Task<List<PreLoginResponse>> AuthenticateWithFirebaseAsync(FirebaseTokenRequest request)
        {
            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(request.FirebaseToken);
            var email = decodedToken.Claims.ContainsKey("email") ? decodedToken.Claims["email"].ToString() : null;
            var name = decodedToken.Claims.ContainsKey("name") ? decodedToken.Claims["name"].ToString() : null;
            var user = await _accountRepo.GetByEmailAsync(email);
            if (user == null)
            {
                var userRole = await _roleRepo.GetByRoleNameAsync(RoleEnum.User.ToString());
                if (userRole == null)
                {
                    throw new InvalidCredentialException("User role not found in database");
                }
                var userId = Guid.NewGuid();
                user = new User
                {
                    UserId = Guid.NewGuid().ToString(),
                    Email = email,
                    //PasswordHash = hashedPassword,
                    FullName = name,
                    CreatedAt = DateTime.UtcNow,
                    RoleId = userRole.RoleId,
                    Verified = true,
                };
                var userCreated = await _accountRepo.CreateUserAsync(user);
                if (userCreated > 0)
                {
                    var profile = new Profile()
                    {
                        ProfileId = Guid.NewGuid().ToString(),
                        UserId = user.UserId,
                        ProfileStatus = UserAccountEnum.Active.ToString(),
                    };
                    await _profileRepo.CreateProfileAsync(profile);
                }
                user = await _accountRepo.GetByEmailAsync(email);
            }
            var userProfiles = await _accountRepo.GetUserProfilesAsync(user.UserId);
            var result = _tokenService.GeneratePreLoginJwtTokens(userProfiles);

            return result;
        }



        #endregion




    }



}
