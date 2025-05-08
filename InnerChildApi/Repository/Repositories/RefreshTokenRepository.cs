using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.DBContext;
using Repository.Interfaces;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class RefreshTokenRepository:GenericRepository<RefreshToken>,IRefreshTokenRepository
    {
        public RefreshTokenRepository() : base()
        {
        }
        public RefreshTokenRepository(InnerChildExeContext context) : base(context)
        {
        }
        public async Task<int> CreateRefreshTokenAsync(string userId,string profileId, string token, DateTime createAt, DateTime expireAt)
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
        public async Task<RefreshToken> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);
        }
        public async Task<RefreshToken> GetByRefreshTokenWithIncludeAsync(string refreshToken)
        {
            return await _context.RefreshTokens.Include(x=>x.User).Include(x=>x.Profile).FirstOrDefaultAsync(x => x.Token == refreshToken);
        }

        public async Task<int> RevokeTokenAsync(RefreshToken refreshToken)
        {
            refreshToken.IsRevoked = true;
            return await base.UpdateAsync(refreshToken);
        }
    }
   
}
