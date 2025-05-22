using Contract.Common.Constant;
using Contract.Common.Enums;
using Contract.Dtos.Requests.Ai;
using Contract.Dtos.Responses.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Repository.MongoDbModels;
using Service.Services;

namespace InnerChildApi.Controllers
{
    [Route("innerchild/aichat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IAiService _aiService;
        private readonly IChatService _chatService;
        public ChatController(IAiService aiService, IChatService chatService)
        {
            _aiService = aiService;
            _chatService = chatService;
        }
        [Authorize]
        [HttpPost("create-session")]
        public async Task<IActionResult> CreateSession([FromBody] SessionCreateRequest request)
        {
            try
            {
                var profileId = User.FindFirst(JwtClaimTypeConstant.ProfileId)?.Value;
                if (profileId == null)
                {
                    return NotFound("User not found");
                }
                var session = new AiChatSessionMongo()
                {
                    AiChatSessionId = ObjectId.GenerateNewId().ToString(),
                    ProfileId = profileId,
                    AiChatSessionTitle = request.SessionTitle,
                    AiChatSessionCreatedAt = DateTime.UtcNow,
                    AiChatMessages = new List<AiChatMessageMongo>(),

                };
                await _chatService.CreateSession(session);
                return Created("", new { message = $"Session with id {session.AiChatSessionId} created" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [Authorize]
        [HttpPost("send-chat")]
        public async Task<IActionResult> SendChat([FromBody] ChatRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var profileId = User.FindFirst(JwtClaimTypeConstant.ProfileId)?.Value;
                if (profileId == null)
                {
                    return NotFound("User not found");
                }
                var session = await _chatService.GetMessagesBySessionIdAndProfileIdAsync(request.AiChatSessionId, profileId);
                if (session == null)
                {
                    return NotFound("Session not found");
                }

                var chatHistory = session.AiChatMessages.Select(x => new AllChatHistoryResponse()
                {
                    Role = x.AiChatMessageSenderType.Trim(),
                    Content = x.AiChatMessageContent.Trim(),
                }).ToList();

                int totalChars = chatHistory.Sum(m =>
                    (m.Content?.Length ?? 0) + (m.Role?.Length ?? 0));
                if (totalChars > 130_000)
                {
                    return BadRequest($"Chat history is too long . Please create new session.");
                }
                var userChatMessage = new AiChatMessageMongo()
                {
                    AiChatMessageId = ObjectId.GenerateNewId().ToString(),
                    AiChatMessageSenderType = ChatMessageEnum.User.ToString(),
                    AiChatMessageSentAt = DateTime.UtcNow,
                    AiChatMessageContent = request.Message,
                };
                await _chatService.AddMessageToSessionAsync(request.AiChatSessionId, userChatMessage);

                var result = await _aiService.SendChatAsync(request.Message, chatHistory);
                var systemResponseMessage = new AiChatMessageMongo()
                {
                    AiChatMessageId = ObjectId.GenerateNewId().ToString(),
                    AiChatMessageSenderType = ChatMessageEnum.System.ToString(),
                    AiChatMessageSentAt = DateTime.UtcNow,
                    AiChatMessageContent = result,
                };
                await _chatService.AddMessageToSessionAsync(request.AiChatSessionId, systemResponseMessage);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("all-sessions")]
        public async Task<IActionResult> GetAllSession()
        {
            try
            {
                var profileId = User.FindFirst(JwtClaimTypeConstant.ProfileId)?.Value;
                if (profileId == null)
                {
                    return NotFound("User not found");
                }
                var sessions = await _chatService.GetSessionsByProfileId(profileId);
                var response = sessions.Select(session => new AllSessionResponse()
                {
                    AichatSessionId = session.AiChatSessionId,
                    AichatSessionTitle = session.AiChatSessionTitle,
                    AichatSessionCreatedAt = session.AiChatSessionCreatedAt,
                    ProfileId = session.ProfileId,
                }).ToList();


                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }
        [Authorize]
        [HttpGet("load-all-messages/{sessionId}")]
        public async Task<IActionResult> LoadAllMessage(string sessionId)
        {
            try
            {
                var profileId = User.FindFirst(JwtClaimTypeConstant.ProfileId)?.Value;
                if (profileId == null)
                {
                    return NotFound("User not found");
                }
                var session = await _chatService.GetSessionBySessionIdAndProfileId(sessionId, profileId);
                if (session == null)
                {
                    return NotFound("Session not found");
                }
                var message = await _chatService.GetMessagesBySessionIdAndProfileIdAsync(sessionId, profileId);
                return Ok(message);

            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }


        }
        [Authorize]
        [HttpDelete("delete-session/{sessionId}")]
        public async Task<IActionResult> DeleteSession(string sessionId)
        {
            try
            {
                var profileId = User.FindFirst(JwtClaimTypeConstant.ProfileId)?.Value;
                if (profileId == null)
                {
                    return NotFound("User not found");
                }
                var existingSession = await _chatService.GetSessionBySessionIdAndProfileId(sessionId, profileId);
                if (existingSession == null)
                {
                    return NotFound("Session not found");
                }
                await _chatService.DeleteSessionAsync(sessionId, profileId);
                return Ok($"{sessionId} was deleted!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
