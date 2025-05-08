using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<int> CreateRefreshTokenAsync(string userId ,string profileId, string token, DateTime createAt, DateTime expireAt);
        Task<RefreshToken> GetByRefreshTokenAsync(string refreshToken);
        Task<int> RevokeTokenAsync(RefreshToken refreshToken);
    }
}
