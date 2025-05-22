using Repository.MongoDbModels;
using Repository.Repositories;

namespace Service.Services
{
    public interface IChatService
    {
        Task CreateSession(AiChatSessionMongo session);

        Task<AiChatSessionMongo> GetMessagesBySessionIdAndProfileIdAsync(string sessionId, string profileId);

        Task<List<AiChatSessionMongo>?> GetSessionsByProfileId(string profileId);

        Task AddMessageToSessionAsync(string sessionId, AiChatMessageMongo message);

        Task<bool> DeleteSessionAsync(string sessionId, string profileId);

        Task<AiChatSessionMongo?> GetSessionBySessionIdAndProfileId(string sessionId, string profileId);
    }
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepo;
        public ChatService(IChatRepository chatRepo)
        {
            _chatRepo = chatRepo;
        }
        public async Task CreateSession(AiChatSessionMongo session)
        {
            await _chatRepo.CreateSession(session);
        }
        public async Task<AiChatSessionMongo> GetMessagesBySessionIdAndProfileIdAsync(string sessionId, string profileId)
        {
            return await _chatRepo.GetMessagesBySessionIdAndProfileIdAsync(sessionId, profileId);
        }
        public async Task<List<AiChatSessionMongo>?> GetSessionsByProfileId(string profileId)
        {
            return await _chatRepo.GetSessionsByProfileId(profileId);
        }
        public async Task AddMessageToSessionAsync(string sessionId, AiChatMessageMongo message)
        {
            await _chatRepo.AddMessageToSessionAsync(sessionId, message);
        }
        public async Task<bool> DeleteSessionAsync(string sessionId, string profileId)
        {
            return await _chatRepo.DeleteSessionAsync(sessionId, profileId);
        }

        public async Task<AiChatSessionMongo?> GetSessionBySessionIdAndProfileId(string sessionId, string profileId)
        {
            return await _chatRepo.GetSessionBySessionIdAndProfileId(sessionId, profileId);
        }
    }
}
