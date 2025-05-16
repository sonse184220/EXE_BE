using System.ComponentModel.DataAnnotations;

namespace Contract.Dtos.Requests.Ai
{
    public class ChatRequest
    {
        [Required(ErrorMessage ="Message is required")]
        [StringLength(400, MinimumLength = 1, ErrorMessage = "Message must be from 1-400 characters")]
        [MinLength(1, ErrorMessage = "Minimum length is 1 character")]
        [MaxLength(400, ErrorMessage = "Maximum length is 400 character")]
        public string Message { get; set; }
        [Required(ErrorMessage ="Ai Chat session is required")]
        public string AiChatSessionId { get; set; }
    }
}
