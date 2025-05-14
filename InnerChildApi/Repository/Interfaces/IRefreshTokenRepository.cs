using Repository.Models;

namespace Repository.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<int> CreateRefreshTokenAsync(string userId, string profileId, string token, DateTime createAt, DateTime expireAt);
        Task<RefreshToken> GetByRefreshTokenAsync(string refreshToken);
        Task<int> RevokeTokenAsync(RefreshToken refreshToken);
    }
}
