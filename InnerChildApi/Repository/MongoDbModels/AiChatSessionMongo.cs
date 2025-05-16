using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Repository.MongoDbModels
{
    public class AiChatSessionMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AiChatSessionId { get; set; }

        public string AiChatSessionTitle { get; set; }

        public DateTime? AiChatSessionCreatedAt { get; set; }

        public string ProfileId { get; set; }

        public List<AiChatMessageMongo> AiChatMessages { get; set; } = new List<AiChatMessageMongo>();



    }
}
