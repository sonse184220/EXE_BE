using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Repository.MongoDbModels
{
    public class AiChatMessageMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AiChatMessageId { get; set; }

        public string AiChatMessageSenderType { get; set; }

        public DateTime? AiChatMessageSentAt { get; set; }

        public string AiChatMessageContent { get; set; }




    }
}
