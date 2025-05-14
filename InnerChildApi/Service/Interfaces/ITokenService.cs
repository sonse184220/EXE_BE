using Contract.Dtos.Responses.Auth;
using Repository.Models;

namespace Service.Interfaces
{
    public interface ITokenService
    {
        string GenerateEmailConfirmationToken(string userId);
        List<PreLoginResponse> GeneratePreLoginJwtTokens(List<Profile> profiles);
        string GenerateFinalLoginJwtToken(string userId, string email, string profileId, string sessionId);
        string ValidateEmailConfirmationToken(string token);
        PreFinalLoginResponse ValidatePreLoginJwtToken(string token);
        Task<string> GenerateRefreshToken(string userId, string profileId);
        Task<RefreshToken> GetByRefreshTokenAsync(string refreshToken);
        Task<int> RevokeTokenAsync(RefreshToken refreshToken);

        string GenerateEmailConfirmationLink(string userId);



    }
}
