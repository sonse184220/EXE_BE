using Repository.Interfaces;
using Repository.Models;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepo;
        public SessionService(ISessionRepository sessionRepo)
        {
            _sessionRepo = sessionRepo;
        }
        public async Task<int> CreateSessionAsync(Session session)
        {
           return await _sessionRepo.CreateSessionAsync(session);
        }

        public async Task InvalidateOtherSessionsAsync(string userId, string profileId, string token)
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
    }
}
