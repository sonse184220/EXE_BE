using Contract.Dtos.Requests;
using Contract.Dtos.Responses;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAccountAsync(RegisterRequest request);
        string? ValidateEmailConfirmationToken(string token);
        Task<bool> VerifyAccount(string userId);
        Task<List<PreLoginResponse>> CheckLoginAccountAsync(LoginRequest request);
        Task<FinalLoginResponse> LoginAccountAsync(string userId, string profileId);
        Task InvalidateOtherSessionsAsync(string userId, string profileId, string token);
        Task<bool> IsSessionValidAsync(string userId, string profileId, string sessionId);
    }
}
