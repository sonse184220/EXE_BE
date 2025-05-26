using Service.Services;

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
