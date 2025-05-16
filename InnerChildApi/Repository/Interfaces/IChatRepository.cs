using Repository.MongoDbModels;

namespace Repository.Interfaces
{
    public interface IChatRepository
    {
        Task CreateSession(AiChatSessionMongo session);

        Task<List<AiChatMessageMongo>> GetMessagesBySessionIdAndProfileIdAsync(string sessionId, string profileId);

        Task<List<AiChatSessionMongo>?> GetSessionsByProfileId(string profileId);

        Task AddMessageToSessionAsync(string sessionId, AiChatMessageMongo message);

        Task<bool> DeleteSessionAsync(string sessionId, string profileId);

        Task<AiChatSessionMongo?> GetSessionBySessionIdAndProfileId(string sessionId, string profileId);

    }
}
