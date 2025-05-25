using Repository.Models;
using Repository.Repositories;

namespace Service.Services
{
    public interface ISessionService
    {
        Task InvalidateOtherSessionsAsync(string userId, string profileId, string token);
        Task<bool> IsSessionValidAsync(string userId, string profileId, string sessionId);
        Task<int> CreateSessionAsync(Session session);

        Task DeleteAllInactiveSessionsAsync();
        
    }
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

        public async Task DeleteAllInactiveSessionsAsync()
        {
             await _sessionRepo.DeleteAllInactiveSessionsAsync();
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
