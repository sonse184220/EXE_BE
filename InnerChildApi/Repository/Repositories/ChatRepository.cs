using Contract.Common.Enums;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Repository.MongoDbModels;
using static Contract.Common.Config.AppSettingConfig;

namespace Repository.Repositories
{
    public interface IChatRepository
    {
        Task CreateSession(AiChatSessionMongo session);

        Task<AiChatSessionMongo> GetMessagesBySessionIdAndProfileIdAsync(string sessionId, string profileId);

        Task<List<AiChatSessionMongo>?> GetSessionsByProfileId(string profileId);

        Task AddMessageToSessionAsync(string sessionId, AiChatMessageMongo message);

        Task<bool> DeleteSessionAsync(string sessionId, string profileId);

        Task<AiChatSessionMongo?> GetSessionBySessionIdAndProfileId(string sessionId, string profileId);

    }
    public class ChatRepository : IChatRepository
    {

        private readonly IMongoCollection<AiChatMessageMongo> _chatMessages;
        private readonly IMongoCollection<AiChatSessionMongo> _chatSessions;
        public ChatRepository(IOptions<ChatDbSettingConfig> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _chatSessions = database.GetCollection<AiChatSessionMongo>(MongoDBEnum.AiChatSessions.ToString());
        }
        public async Task CreateSession(AiChatSessionMongo session)
        {
            await _chatSessions.InsertOneAsync(session);
        }
        public async Task<AiChatSessionMongo> GetMessagesBySessionIdAndProfileIdAsync(string sessionId, string profileId)
        {
            var session = await _chatSessions.Find(s => s.AiChatSessionId == sessionId && s.ProfileId == profileId).FirstOrDefaultAsync();
            return session;
        }
        public async Task<List<AiChatSessionMongo>?> GetSessionsByProfileId(string profileId)
        {
            return await _chatSessions.Find(s => s.ProfileId == profileId).ToListAsync();
        }
        public async Task<AiChatSessionMongo?> GetSessionBySessionIdAndProfileId(string sessionId, string profileId)
        {
            return await _chatSessions.Find(x => x.AiChatSessionId == sessionId && x.ProfileId == profileId).FirstOrDefaultAsync();
        }
        public async Task AddMessageToSessionAsync(string sessionId, AiChatMessageMongo message)
        {
            var update = Builders<AiChatSessionMongo>.Update.Push(x => x.AiChatMessages, message);
            await _chatSessions.UpdateOneAsync(x => x.AiChatSessionId == sessionId, update);
        }
        public async Task<bool> DeleteSessionAsync(string sessionId, string profileId)
        {
            var result = await _chatSessions.DeleteOneAsync(s => s.AiChatSessionId == sessionId && s.ProfileId == profileId);
            return result.DeletedCount > 0;
        }

    }
}
