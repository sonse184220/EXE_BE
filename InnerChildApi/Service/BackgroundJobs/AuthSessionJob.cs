using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
