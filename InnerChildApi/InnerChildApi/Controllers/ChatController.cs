using Contract.Dtos.Requests.Ai;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace InnerChildApi.Controllers
{
    [Route("innerchild/aichat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IAiService _aiService;
        public ChatController(IAiService aiService)
        {
            _aiService = aiService;
        }
        [HttpPost("sendchat")]
        public async Task<IActionResult> SendChat([FromBody] ChatRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _aiService.SendChatAsync(request.Message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
