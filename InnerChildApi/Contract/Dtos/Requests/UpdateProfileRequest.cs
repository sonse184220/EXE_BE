using Contract.Common.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Dtos.Requests
{
    public class UpdateProfileRequest
    {
        public string? FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public UserGenderEnum? Gender { get; set; }

        public string? PhoneNumber { get; set; }

        public IFormFile? ProfilePicture { get; set; }

    }
}
