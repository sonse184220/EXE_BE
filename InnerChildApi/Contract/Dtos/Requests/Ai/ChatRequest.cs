using System.ComponentModel.DataAnnotations;

namespace Contract.Dtos.Requests.Ai
{
    public class ChatRequest
    {
        [Required]
        public string Message { get; set; }
    }
}
