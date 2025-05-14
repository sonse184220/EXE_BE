using Contract.Dtos.Requests.Auth;
using Contract.Dtos.Responses.Auth;

namespace Service.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAccountAsync(RegisterRequest request);
        Task<List<PreLoginResponse>> CheckLoginAccountAsync(LoginRequest request);
        Task<FinalLoginResponse> LoginAccountAsync(string userId, string profileId);
        Task ChangePassword(string userId, string currentPassword, string newPassword);
        Task<List<PreLoginResponse>> AuthenticateWithFirebaseAsync(FirebaseTokenRequest request);
    }
}
