using Repository.Models;

namespace Service.Interfaces
{
    public interface ISessionService
    {
        Task InvalidateOtherSessionsAsync(string userId, string profileId, string token);
        Task<bool> IsSessionValidAsync(string userId, string profileId, string sessionId);
        Task<int> CreateSessionAsync(Session session);
    }
}
