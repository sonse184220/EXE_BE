using Contract.Common.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Contract.Dtos.Requests.Auth
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is not valid")]

        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Password must be 5-100 characters")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Password and confirm password do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(25, MinimumLength = 4, ErrorMessage = "Fullname must be 4-25 characters")]
        public string FullName { get; set; }



        [Range(typeof(DateTime), "1/1/1800", "12/31/2100", ErrorMessage = "Date of birth must be between 01/01/1800 and 12/31/2100.")]
        public DateTime? DateOfBirth { get; set; }


        [EnumDataType(typeof(UserGenderEnum), ErrorMessage = "Invalid gender value.")]
        public UserGenderEnum? gender { get; set; }


        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? PhoneNumber { get; set; }

        public IFormFile? ProfilePicture { get; set; }
    }
}
