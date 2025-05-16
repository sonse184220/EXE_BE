using MongoDB.Driver;
using Repository.Interfaces;
using Repository.MongoDbModels;
using Service.Interfaces;

namespace Service.Services
{
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
        public async Task<List<AiChatMessageMongo>> GetMessagesBySessionIdAndProfileIdAsync(string sessionId, string profileId)
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
