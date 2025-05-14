using Contract.Common.Enums;
using Microsoft.AspNetCore.Http;

namespace Contract.Dtos.Requests.Auth
{
    public class ProfileUpdateRequest
    {
        public string? FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public UserGenderEnum? Gender { get; set; }

        public string? PhoneNumber { get; set; }

        public IFormFile? ProfilePicture { get; set; }

    }
}
