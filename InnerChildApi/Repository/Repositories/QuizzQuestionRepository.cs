using Contract.Common.Enums;
using MongoDB.Driver;
using Repository.MongoDbModels;

namespace Repository.Repositories
{
    public interface IQuizzQuestionRepository
    {
        Task CreateQuizzQuestionAsync(List<QuizzQuestionMongo> model, IClientSessionHandle session);
        Task<List<QuizzQuestionMongo>> GetAllQuizzQuestionsAsync();
        Task<QuizzQuestionMongo> GetQuizzQuestionByIdAsync(string quizQuestionId);
        Task UpdateQuizzQuestionAsync(string quizQuestionId, QuizzQuestionMongo model);
    }
    public class QuizzQuestionRepository : IQuizzQuestionRepository
    {

        private IMongoCollection<QuizzQuestionMongo> _quizzQuestionMongoCollection;

        public QuizzQuestionRepository(IMongoUnitOfWork unitOfWork)
        {
            _quizzQuestionMongoCollection = unitOfWork.GetCollection<QuizzQuestionMongo>(MongoDBEnum.QuizzQuestions.ToString());
        }

        public Task CreateQuizzQuestionAsync(List<QuizzQuestionMongo> model, IClientSessionHandle session)
        {
            return _quizzQuestionMongoCollection.InsertManyAsync(session, model);
        }

        public Task<List<QuizzQuestionMongo>> GetAllQuizzQuestionsAsync()
        {
            return _quizzQuestionMongoCollection.Find(_ => true).ToListAsync();
        }

        public Task<QuizzQuestionMongo> GetQuizzQuestionByIdAsync(string quizQuestionId)
        {
            return _quizzQuestionMongoCollection.Find(x => x.QuizQuestionId == quizQuestionId).FirstOrDefaultAsync();
        }

        public async Task UpdateQuizzQuestionAsync(string quizQuestionId, QuizzQuestionMongo model)
        {
            var existing = await _quizzQuestionMongoCollection.Find(x => x.QuizQuestionId == quizQuestionId).FirstOrDefaultAsync();
            if (existing == null)
            {
                throw new KeyNotFoundException("Quiz question not found");
            }
            existing.QuizQuestionContent = model.QuizQuestionContent ?? existing.QuizQuestionContent;
            existing.QuizQuestionOrder = model.QuizQuestionOrder ?? existing.QuizQuestionOrder;
            existing.QuizId = model.QuizId ?? existing.QuizId;

            await _quizzQuestionMongoCollection.ReplaceOneAsync(x => x.QuizQuestionId == quizQuestionId, existing);
        }


    }
}
