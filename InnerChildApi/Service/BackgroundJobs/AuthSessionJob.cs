using Service.Services;

namespace Service.BackgroundJobs
{
    public class AuthSessionJob
    {
        private readonly ISessionService _sessionService;
        public AuthSessionJob(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public async Task Run()
        {
            await _sessionService.DeleteAllInactiveSessionsAsync();
        }
    }
}
