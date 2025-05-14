using Repository.Models;

namespace Repository.Interfaces
{
    public interface ISessionRepository
    {
        Task<int> CreateSessionAsync(Session session);
        Task InvalidateOtherSessionsAsync(string userId, string profileId, string token);
        Task<bool> IsSessionValidAsync(string userId, string profileId, string token);
    }
}
