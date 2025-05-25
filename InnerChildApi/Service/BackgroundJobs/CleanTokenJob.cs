using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.BackgroundJobs
{
    public class CleanTokenJob
    {
        private readonly ITokenService _tokenService;
        public CleanTokenJob(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        public async Task Run()
        {
            await _tokenService.DeleteRevokedTokenAsync();
        }
    }
}
