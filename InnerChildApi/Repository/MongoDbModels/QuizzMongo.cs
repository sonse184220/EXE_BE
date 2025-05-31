using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Repository.MongoDbModels
{
    public class QuizzCategoryMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string QuizCategoryId { get; set; }

        public string QuizCategoryName { get; set; }

        public string QuizCategoryDescription { get; set; }

    }
    public class QuizzMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string QuizId { get; set; }

        public string QuizTitle { get; set; }

        public string QuizDescription { get; set; }

        public DateTime? QuizCreatedAt { get; set; }

        public DateTime? QuizUpdatedAt { get; set; }

        public string QuizStatus { get; set; }

        public double? QuizMaxScore { get; set; }

        public string QuizCategoryId { get; set; }

    }
    public class QuizzQuestionMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string QuizQuestionId { get; set; }

        public string QuizQuestionContent { get; set; }

        public int? QuizQuestionOrder { get; set; }

        public string QuizId { get; set; }

    }
    public class QuizzOptionMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string QuizAnswerOptionId { get; set; }

        public string QuizContent { get; set; }

        public double? QuizScoreValue { get; set; }

        public string QuestionId { get; set; }

    }

}
