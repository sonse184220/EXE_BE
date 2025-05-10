using Contract.Common.Constant;
using Contract.Common.Enums;
using Contract.Dtos.Responses.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repository.Interfaces;
using Repository.Models;
using Repository.Repositories;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Contract.Common.Config.AppSettingConfig;
using static Org.BouncyCastle.Math.EC.ECCurve;
namespace Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly JwtTokenSetting _jwtTokenSetting;
        private readonly SymmetricSecurityKey _key;
        private readonly SigningCredentials _credentials;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenService(IOptions<JwtTokenSetting> jwtTokenSetting,IRefreshTokenRepository refreshTokenRepo, IHttpContextAccessor httpContextAccessor)
        {
            _jwtTokenSetting = jwtTokenSetting.Value;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenSetting.SecretKey));
            _credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512);
            _refreshTokenRepo = refreshTokenRepo;
            _httpContextAccessor = httpContextAccessor;
        }
        #region generate email confirmation token
        public string GenerateEmailConfirmationToken(string userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(JwtClaimTypeConstant.TokenType,JwtTypeEnum.EmailConfirm.ToString())
            };
           
            return GenerateToken(claims,TimeSpan.FromDays(1));
        }
        #endregion
        #region generate final jwt token
        public string GenerateFinalLoginJwtToken(string userId, string email, string profileId, string sessionId)
        {

            var claims = new List<Claim>
        {
        new Claim(ClaimTypes.NameIdentifier, userId),
        new Claim(ClaimTypes.Email, email),
        new Claim(JwtClaimTypeConstant.ProfileId, profileId),
        new Claim(JwtClaimTypeConstant.SessionId, sessionId),
        new Claim(JwtClaimTypeConstant.TokenType,JwtTypeEnum.FinalLogin.ToString())
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


        private string GenerateToken(IEnumerable<Claim> claims,TimeSpan expiration)
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
            var result = await _refreshTokenRepo.CreateRefreshTokenAsync(userId,profileId, token, createAt, expireAt);
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
    }
}
