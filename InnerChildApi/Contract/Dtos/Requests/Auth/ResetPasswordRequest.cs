using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Dtos.Requests.Auth
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage ="Token is required")]
        public string Token { get; set;}
        [Required(ErrorMessage = "New password is required")]

        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("NewPassword", ErrorMessage = "New Password and confirm new password do not match")]
        public string ConfirmNewPassword { get; set; }
    }
}
