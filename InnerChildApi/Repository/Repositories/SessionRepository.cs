using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Models;

namespace Repository.Repositories
{
    public interface ISessionRepository
    {
        Task<int> CreateSessionAsync(Session session);
        Task InvalidateOtherSessionsAsync(string userId, string profileId, string token);
        Task<bool> IsSessionValidAsync(string userId, string profileId, string token);

        Task DeleteAllInactiveSessionsAsync();
    }
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {

        public async Task<int> CreateSessionAsync(Session session)
        {
            return await CreateAsync(session);
        }

        public async Task DeleteAllInactiveSessionsAsync()
        {
            var allInactiveSessions = await _context.Sessions.Where(x => x.SessionIsActive == false).ToListAsync();
            _context.RemoveRange(allInactiveSessions);
            await _context.SaveChangesAsync();
        }

        public async Task InvalidateOtherSessionsAsync(string userId, string profileId, string token)
        {
            var otherSession = await _context.Sessions.Where(x => x.UserId == userId && x.ProfileId == profileId && x.Token != token).ToListAsync();
            if (otherSession != null && otherSession.Count > 0)
            {
                foreach (var session in otherSession)
                {
                    session.SessionIsActive = false;
                    _context.Sessions.Update(session);
                }
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> IsSessionValidAsync(string userId, string profileId, string token)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(x => x.UserId == userId && x.ProfileId == profileId && x.Token == token && x.SessionIsActive == true);
            if (session != null)
            {
                return true;
            }
            return false;
        }
    }

}
