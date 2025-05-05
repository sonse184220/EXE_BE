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
        public AuthService(IAccountRepository accountRepo, IConfiguration config, IEmailService emailService,IProfileRepository profileRepo,IRoleRepository roleRepo, IHttpContextAccessor httpContextAccessor)
        {
            _accountRepo = accountRepo;
            _config = config;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _profileRepo = profileRepo; 
            _roleRepo = roleRepo;
        }

        public async Task<RegisterResponse> RegisterAccountAsync(RegisterRequest request)
        {
            try
            {
                var userByName = await _accountRepo.GetByUserNameAsync(request.FullName);
                if (userByName != null)
                {
                    return new RegisterResponse
                    {
                        IsSuccess = false,
                        Message = "User already exists."
                    };
                }
                var userByEmail = await _accountRepo.GetByEmailAsync(request.Email);
                if (userByEmail != null)
                {
                    return new RegisterResponse
                    {
                        IsSuccess = false,
                        Message = "Email already exists."
                    };
                }
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
                var role = await _roleRepo.GetByRoleNameAsync(RoleEnum.User.ToString());
                var user = new User
                {
                    UserId = Guid.NewGuid().ToString(),
                    Email = request.Email,
                    PasswordHash = hashedPassword,
                    FullName = request.FullName,
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
                return new RegisterResponse
                {
                    IsSuccess = true,
                    Message = "User registered successfully, please check your email for confirmation."
                };
            }
            catch(Exception ex)
            {
                return new RegisterResponse
                {
                    IsSuccess = false,
                    Message = $"Something went wrong: {ex.Message}"
                };
            }
            
        }













        private string GenerateEmailConfirmation(string userId)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("type", "EmailConfirmation")
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
        public string? ValidateEmailConfirmationToken(string token)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            Console.WriteLine($"SecretKey: {secretKey ?? "null"}");
            Console.WriteLine($"Issuer: {issuer ?? "null"}");
            Console.WriteLine($"Audience: {audience ?? "null"}");
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
                var typeClaim = principle.FindFirst("type");
                if (typeClaim?.Value!= "EmailConfirmation")
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
    }
    
}
