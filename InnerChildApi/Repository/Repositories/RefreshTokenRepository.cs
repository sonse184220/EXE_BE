using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Models;

namespace Repository.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<int> CreateRefreshTokenAsync(string userId, string profileId, string token, DateTime createAt, DateTime expireAt);
        Task<RefreshToken> GetByRefreshTokenAsync(string refreshToken);
        Task<int> RevokeTokenAsync(RefreshToken refreshToken);

        Task DeleteRevokedTokenAsync();
    }
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {

        public async Task<int> CreateRefreshTokenAsync(string userId, string profileId, string token, DateTime createAt, DateTime expireAt)
        {
            var refreshToken = new RefreshToken
            {
                RefreshTokenId = Guid.NewGuid().ToString(),
                UserId = userId,
                ProfileId = profileId,
                Token = token,
                CreatedAt = createAt,
                ExpiresAt = expireAt,
                IsRevoked = false,

            };
            await _context.RefreshTokens.AddAsync(refreshToken);
            return await _context.SaveChangesAsync();
        }

        public async Task DeleteRevokedTokenAsync()
        {
            var allRevokedTokens = await _context.RefreshTokens.Where(x => x.IsRevoked == true).ToListAsync();
            _context.RemoveRange(allRevokedTokens);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);
        }
        public async Task<RefreshToken> GetByRefreshTokenWithIncludeAsync(string refreshToken)
        {
            return await _context.RefreshTokens.Include(x => x.User).Include(x => x.Profile).FirstOrDefaultAsync(x => x.Token == refreshToken);
        }

        public async Task<int> RevokeTokenAsync(RefreshToken refreshToken)
        {
            refreshToken.IsRevoked = true;
            return await UpdateAsync(refreshToken);
        }
    }

}
