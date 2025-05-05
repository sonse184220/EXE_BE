using Contract.Dtos.Requests;
using Contract.Dtos.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAccountAsync(RegisterRequest request);
        string? ValidateEmailConfirmationToken(string token);
        Task<bool> VerifyAccount(string userId);
    }
}
