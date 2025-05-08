using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ISessionService
    {
        Task InvalidateOtherSessionsAsync(string userId, string profileId, string token);
        Task<bool> IsSessionValidAsync(string userId, string profileId, string sessionId);
        Task<int> CreateSessionAsync(Session session);
    }
}
