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
        Task<List<PreLoginResponse>> CheckLoginAccountAsync(LoginRequest request);
        Task<FinalLoginResponse> LoginAccountAsync(string userId, string profileId);
        Task ChangePassword(string userId, string currentPassword, string newPassword);
        Task<List<PreLoginResponse>> AuthenticateWithFirebaseAsync(FirebaseTokenRequest request);
    }
}
