using Contract.Dtos.Enums;
using Contract.Dtos.Requests;
using Contract.Dtos.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Interfaces;
using Repository.Models;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class AuthService:IAuthService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IRoleRepository _roleRepo;         
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        private readonly IProfileRepository _profileRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICloudinaryImageService _cloudinaryImageService;
        private readonly ISessionRepository _sessionRepo;
        public AuthService(IAccountRepository accountRepo, IConfiguration config, IEmailService emailService,IProfileRepository profileRepo,IRoleRepository roleRepo, IHttpContextAccessor httpContextAccessor,ICloudinaryImageService cloudinaryImageService,ISessionRepository sessionRepo)
        {
            _accountRepo = accountRepo;
            _config = config;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _profileRepo = profileRepo; 
            _roleRepo = roleRepo;
            _cloudinaryImageService = cloudinaryImageService;
            _sessionRepo = sessionRepo;
        }
        #region register
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
                    imageUrl =  await _cloudinaryImageService.UploadImageAsync(request.ProfilePicture, CloudinaryFolderEnum.UserPicture.ToString());
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
                var emailConfirmationToken = GenerateEmailConfirmation(user.UserId);
                var requestUrl = _httpContextAccessor.HttpContext.Request;
                var baseUrl = $"{requestUrl.Scheme}://{requestUrl.Host.Value}";
                var emailConfirmationTokenLink = $"{baseUrl}/innerchild/auth/confirm-email?token={emailConfirmationToken}";
                await _emailService.SendConfirmationEmailAsync(user.Email, user.FullName, emailConfirmationTokenLink);
        }
        #endregion



        #region normal login
        public async Task<List<PreLoginResponse>> CheckLoginAccountAsync(LoginRequest request)
        {
            var existingUser = await _accountRepo.GetByEmailAsync(request.Email);
            if (existingUser == null)
            {
                throw new InvalidCredentialException("User not found.");
            }
            if (!BCrypt.Net.BCrypt.Verify(request.Password, existingUser.PasswordHash))
            {
                throw new InvalidCredentialException("Invalid password.");
            }
            if (existingUser.Verified==false)
            {
                throw new InvalidCredentialException("Email not verified.");
            }
            var userProfiles = await _accountRepo.GetUserProfilesAsync(existingUser.UserId);

            var result = GeneratePreJwtToken(userProfiles);
            return result;
        }
        #endregion

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
            var sessionCreated = await _sessionRepo.CreateSessionAsync(userSession);
            if (sessionCreated > 0)
            {
                await InvalidateOtherSessionsAsync(user.UserId, profileId, sessionId);
            }
            var token = GenerateFinalJwtToken(user.UserId, user.Email,profile.ProfileId ,sessionId);
            return new FinalLoginResponse
            {
                Token = token,
            };
        }






        #region helper methods
        private string GenerateEmailConfirmation(string userId)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("TokenType",JwtTypeEnum.EmailConfirm.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));  
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );
            var emailConfirmationToken = new JwtSecurityTokenHandler().WriteToken(token);
            return emailConfirmationToken;
        }

        private List<PreLoginResponse>  GeneratePreJwtToken(List<Profile> profiles)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expires = int.Parse(jwtSettings["Expires"]);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokens = new List<string>();
            var finalProfiles = new List<PreLoginResponse>();
            foreach (var profile in profiles)
            {
                var claims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, profile.UserId),
            new Claim("ProfileId", profile.ProfileId),
            new Claim("TokenType", JwtTypeEnum.PreLogin.ToString())
            };

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(expires),
                    signingCredentials: creds
                );
                var tokenResult = new JwtSecurityTokenHandler().WriteToken(token);
                tokens.Add(tokenResult);
                var eachProfileToken = new PreLoginResponse()
                {
                    ProfileId = profile.ProfileId,
                    UserId = profile.UserId,
                    ProfileStatus = profile.ProfileStatus,
                    ProfileToken = tokenResult
                };
                finalProfiles.Add(eachProfileToken);
            }

            return finalProfiles ;
        }

        private string GenerateFinalJwtToken(string userId,string email,string profileId,string sessionId)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expires = int.Parse(jwtSettings["Expires"]);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, userId),
        new Claim(ClaimTypes.Email, email),
        new Claim("ProfileId", profileId),
        new Claim("SessionId", sessionId),
        new Claim("TokenType",JwtTypeEnum.FinalLogin.ToString())
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(expires),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public string? ValidateEmailConfirmationToken(string token)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principle = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,

                    ValidateAudience = true,
                    ValidAudience = audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                },out SecurityToken validatedToken);
                var typeClaim = principle.FindFirst("TokenType");
                if (typeClaim?.Value!= JwtTypeEnum.EmailConfirm.ToString())
                {
                    return null;
                }
                var userId = principle.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return userId;
            }catch(Exception ex)
            {
                return null;
            }

        }

        public async Task<bool> VerifyAccount(string userId)
        {
            var user = await _accountRepo.GetByUserIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            user.Verified = true;
            var result = await _accountRepo.UpdateUserAsync(user);
            if (result>0)
            {
                return true;
            }
            return false;
        }
        public async Task InvalidateOtherSessionsAsync(string userId,string profileId,string token)
        {
            await _sessionRepo.InvalidateOtherSessionsAsync(userId, profileId, token);  
        }
        public async Task<bool> IsSessionValidAsync(string userId, string profileId, string sessionId)
        {
            var sessionChecked = await _sessionRepo.IsSessionValidAsync(userId, profileId, sessionId);
            if (sessionChecked == true)
            {
                return true;
            }
            return false;
        }
    #endregion

    }



}
