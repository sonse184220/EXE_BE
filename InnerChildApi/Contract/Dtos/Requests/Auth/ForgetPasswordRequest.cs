using System.ComponentModel.DataAnnotations;

namespace Contract.Dtos.Requests.Auth
{
    public class ForgetPasswordRequest
    {
        [Required]
        public string Email { get; set; }

    }
}
