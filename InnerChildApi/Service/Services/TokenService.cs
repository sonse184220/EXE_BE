using Contract.Common.Constant;
using Contract.Common.Enums;
using Contract.Dtos.Responses.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repository.Models;
using Repository.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static Contract.Common.Config.AppSettingConfig;
namespace Service.Services
{
    public interface ITokenService
    {
        string GenerateEmailConfirmationToken(string userId);
        List<PreLoginResponse> GeneratePreLoginJwtTokens(List<Profile> profiles);
        string GenerateFinalLoginJwtToken(string userId, string email, string profileId, string sessionId,string purchasePlan,string role);
        string ValidateEmailConfirmationToken(string token);
        PreFinalLoginResponse ValidatePreLoginJwtToken(string token);
        Task<string> GenerateRefreshToken(string userId, string profileId);
        Task<RefreshToken> GetByRefreshTokenAsync(string refreshToken);
        Task<int> RevokeTokenAsync(RefreshToken refreshToken);

        string GenerateEmailConfirmationLink(string userId);
        //forgot and reset
        string GenerateForgotPasswordToken(string userId);


        string ValidateForgotPasswordToken(string token);
        (string userId, string newPasswordHash) ValidateResetPasswordToken(string token);
        string GenerateResetPasswordToken(string userId, string password);
        string GenerateEmailConfirmationResetPasswordLink(string token);

        Task DeleteRevokedTokenAsync();
    }
    public class TokenService : ITokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly JwtTokenSetting _jwtTokenSetting;
        private readonly SymmetricSecurityKey _key;
        private readonly SigningCredentials _credentials;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenService(IOptions<JwtTokenSetting> jwtTokenSetting, IRefreshTokenRepository refreshTokenRepo, IHttpContextAccessor httpContextAccessor)
        {
            _jwtTokenSetting = jwtTokenSetting.Value;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenSetting.SecretKey));
            _credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512);
            _refreshTokenRepo = refreshTokenRepo;
            _httpContextAccessor = httpContextAccessor;
        }
        #region forget password section
        public string GenerateForgotPasswordToken(string userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(JwtClaimTypeConstant.TokenType,JwtTypeEnum.ForgotPassword.ToString())
            };
            return GenerateToken(claims, TimeSpan.FromMinutes(5));
        }
        public string ValidateForgotPasswordToken(string token)
        {
            var principle = ValidateToken(token);
            var typeClaim = principle.FindFirst(JwtClaimTypeConstant.TokenType)?.Value;
            if (typeClaim != JwtTypeEnum.ForgotPassword.ToString())
            {
                throw new InvalidCredentialException("Invalid token type");
            }
            var userId = principle.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidCredentialException("User Id not found in token");
            }
            return userId;
        }
        public string GenerateResetPasswordToken(string userId, string password)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(JwtClaimTypeConstant.PasswordHash, hashedPassword),
                new Claim(JwtClaimTypeConstant.TokenType,JwtTypeEnum.ResetPassword.ToString())
            };
            return GenerateToken(claims, TimeSpan.FromDays(1));
        }
        public (string userId, string newPasswordHash) ValidateResetPasswordToken(string token)
        {
            var principle = ValidateToken(token);
            var typeClaim = principle.FindFirst(JwtClaimTypeConstant.TokenType)?.Value;
            if (typeClaim != JwtTypeEnum.ResetPassword.ToString())
            {
                throw new InvalidCredentialException("Invalid token type");
            }
            var userId = principle.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var newPasswordHash = principle.FindFirst(JwtClaimTypeConstant.PasswordHash)?.Value;
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(newPasswordHash))
            {
                throw new InvalidCredentialException("User Id or password not found in token");
            }
            return (userId, newPasswordHash);
        }

        #endregion







        #region generate email confirmation token
        public string GenerateEmailConfirmationToken(string userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(JwtClaimTypeConstant.TokenType,JwtTypeEnum.EmailConfirm.ToString())
            };

            return GenerateToken(claims, TimeSpan.FromDays(1));
        }
        #endregion
        #region generate final jwt token
        public string GenerateFinalLoginJwtToken(string userId, string email, string profileId, string sessionId, string purchasePlan,string role)
        {

            var claims = new List<Claim>
        {
        new Claim(ClaimTypes.NameIdentifier, userId),
        new Claim(ClaimTypes.Email, email),
        new Claim(JwtClaimTypeConstant.ProfileId, profileId),
        new Claim(JwtClaimTypeConstant.SessionId, sessionId),
        new Claim(JwtClaimTypeConstant.TokenType,JwtTypeEnum.FinalLogin.ToString()),
        new Claim(JwtClaimTypeConstant.PurchasePlan, purchasePlan),
        new Claim(ClaimTypes.Role,role),
        };

            return GenerateToken(claims, TimeSpan.FromDays(_jwtTokenSetting.ExpiresAccessToken));
        }
        #endregion
        #region generate prelogin token
        public List<PreLoginResponse> GeneratePreLoginJwtTokens(List<Profile> profiles)
        {
            var finalProfiles = new List<PreLoginResponse>();
            foreach (var profile in profiles)
            {
                var claims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, profile.UserId),
            new Claim(JwtClaimTypeConstant.ProfileId, profile.ProfileId),
            new Claim(JwtClaimTypeConstant.TokenType, JwtTypeEnum.PreLogin.ToString())
            };

                var tokenResult = GenerateToken(claims, TimeSpan.FromDays(_jwtTokenSetting.ExpiresAccessToken));
                finalProfiles.Add(new PreLoginResponse
                {
                    ProfileId = profile.ProfileId,
                    UserId = profile.UserId,
                    ProfileStatus = profile.ProfileStatus,
                    ProfileToken = tokenResult
                });
            }

            return finalProfiles;
        }


        #endregion
        #region validate email confirmation token
        public string ValidateEmailConfirmationToken(string token)
        {
            var principle = ValidateToken(token);
            var typeClaim = principle.FindFirst(JwtClaimTypeConstant.TokenType)?.Value;
            if (typeClaim != JwtTypeEnum.EmailConfirm.ToString())
            {
                throw new InvalidCredentialException("Invalid token type");
            }
            var userId = principle.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidCredentialException("User Id not found in token");
            }
            return userId;
        }
        #endregion
        #region validate prelogin token
        public PreFinalLoginResponse ValidatePreLoginJwtToken(string token)
        {
            var principle = ValidateToken(token);

            var typeClaim = principle.FindFirst(JwtClaimTypeConstant.TokenType)?.Value;
            if (typeClaim != JwtTypeEnum.PreLogin.ToString())
            {
                throw new InvalidCredentialException("Token is invalid");
            }
            var userId = principle.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var profileId = principle.FindFirst(JwtClaimTypeConstant.ProfileId)?.Value;
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(profileId))
            {
                throw new InvalidCredentialException("Required claims not found in token");
            }
            return new PreFinalLoginResponse()
            {
                ProfileId = profileId,
                UserId = userId,
            };
        }
        #endregion


        private string GenerateToken(IEnumerable<Claim> claims, TimeSpan expiration)
        {
            var token = new JwtSecurityToken(
               issuer: _jwtTokenSetting.Issuer,
               audience: _jwtTokenSetting.Audience,
               claims: claims,
               expires: DateTime.UtcNow.Add(expiration),
               signingCredentials: _credentials
           );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtTokenSetting.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtTokenSetting.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);
        }
        public async Task<string> GenerateRefreshToken(string userId, string profileId)
        {
            var token = GenerateRandomToken();
            var createAt = DateTime.UtcNow;
            var expireAt = createAt.AddDays(_jwtTokenSetting.ExpiresRefreshToken);
            var result = await _refreshTokenRepo.CreateRefreshTokenAsync(userId, profileId, token, createAt, expireAt);
            return token;
        }
        private string GenerateRandomToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public async Task<RefreshToken> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _refreshTokenRepo.GetByRefreshTokenAsync(refreshToken);
        }

        public async Task<int> RevokeTokenAsync(RefreshToken refreshToken)
        {
            return await _refreshTokenRepo.RevokeTokenAsync(refreshToken);
        }

        public string GenerateEmailConfirmationLink(string userId)
        {
            var emailConfirmationToken = GenerateEmailConfirmationToken(userId);
            var requestUrl = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{requestUrl.Scheme}://{requestUrl.Host.Value}";
            var emailConfirmationTokenLink = $"{baseUrl}/innerchild/auth/confirm-email?token={emailConfirmationToken}";
            return emailConfirmationTokenLink;
        }

        public string GenerateEmailConfirmationResetPasswordLink(string token)
        {

            var requestUrl = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{requestUrl.Scheme}://{requestUrl.Host.Value}";
            var emailResetPasswordTokenLink = $"{baseUrl}/innerchild/auth/verify-reset-password?token={token}";
            return emailResetPasswordTokenLink;
        }

        public async Task DeleteRevokedTokenAsync()
        {
            await _refreshTokenRepo.DeleteRevokedTokenAsync();
        }
    }
}
