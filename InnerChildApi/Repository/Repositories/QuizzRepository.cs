using Contract.Common.Enums;
using Contract.Dtos.Responses.Quizz;
using MongoDB.Driver;
using Repository.MongoDbModels;

namespace Repository.Repositories
{
    public interface IQuizzRepository
    {
        Task CreateQuizzAsync(QuizzMongo quizz, IClientSessionHandle session);

        Task<List<QuizzMongo>> GetAllQuizzAsync();

        Task<QuizzMongo> GetQuizzByIdAsync(string quizzId);

        Task<QuizzDetailResponse> GetFullQuizDetailByIdAsync(string quizzId);

        Task UpdateQuizzAsync(string quizzId, QuizzMongo updatedQuizz);

        Task DeleteQuizzAsync(string quizzId);
    }
    public class QuizzRepository : IQuizzRepository
    {
        private readonly IMongoCollection<QuizzMongo> _quizzCollection;
        private readonly IMongoCollection<QuizzQuestionMongo> _questionCollection;
        private readonly IMongoCollection<QuizzOptionMongo> _optionCollection;

        public QuizzRepository(IMongoUnitOfWork unitOfWork)
        {
            _quizzCollection = unitOfWork.GetCollection<QuizzMongo>(MongoDBEnum.Quizzes.ToString());
            _questionCollection = unitOfWork.GetCollection<QuizzQuestionMongo>(MongoDBEnum.QuizzQuestions.ToString());
            _optionCollection = unitOfWork.GetCollection<QuizzOptionMongo>(MongoDBEnum.QuizzOptions.ToString());
        }

        public Task CreateQuizzAsync(QuizzMongo quizz, IClientSessionHandle session)
        {
            return _quizzCollection.InsertOneAsync(session, quizz);
        }

        public Task<List<QuizzMongo>> GetAllQuizzAsync()
        {
            return _quizzCollection.Find(_ => true).ToListAsync();
        }

        public Task<QuizzMongo> GetQuizzByIdAsync(string quizzId)
        {
            return _quizzCollection.Find(q => q.QuizId == quizzId).FirstOrDefaultAsync();
        }

        

        public async Task UpdateQuizzAsync(string quizzId, QuizzMongo updatedQuizz)
        {
            var existingQuizz = await _quizzCollection.Find(q => q.QuizId == quizzId).FirstOrDefaultAsync();
            if (existingQuizz == null)
                throw new KeyNotFoundException("Quiz not found");

            existingQuizz.QuizTitle = updatedQuizz.QuizTitle ?? existingQuizz.QuizTitle;
            existingQuizz.QuizDescription = updatedQuizz.QuizDescription ?? existingQuizz.QuizDescription;
            existingQuizz.QuizUpdatedAt = DateTime.UtcNow;
            existingQuizz.QuizStatus = updatedQuizz.QuizStatus ?? existingQuizz.QuizStatus;
            existingQuizz.QuizMaxScore = updatedQuizz.QuizMaxScore ?? existingQuizz.QuizMaxScore;
            existingQuizz.QuizCategoryId = updatedQuizz.QuizCategoryId ?? existingQuizz.QuizCategoryId;

            await _quizzCollection.ReplaceOneAsync(q => q.QuizId == quizzId, existingQuizz);
        }

        public Task DeleteQuizzAsync(string quizzId)
        {
            return _quizzCollection.DeleteOneAsync(q => q.QuizId == quizzId);
        }

        public async Task<QuizzDetailResponse> GetFullQuizDetailByIdAsync(string quizzId)
        {
            var quizz = await _quizzCollection.Find(q => q.QuizId == quizzId).FirstOrDefaultAsync();
            if (quizz == null)
            {
                return null;
            }
            var question = await _questionCollection.Find(q => q.QuizId == quizzId).ToListAsync();
            var questionIds = question.Select(q => q.QuizQuestionId).ToList();

            var options = await _optionCollection.Find(o => questionIds.Contains(o.QuestionId)).ToListAsync();

            var questionDtos = question.Select(q => new QuizzQuestionDetailResponse
            {
                QuizQuestionId = q.QuizQuestionId,
                QuizQuestionContent = q.QuizQuestionContent,
                QuizQuestionOrder = q.QuizQuestionOrder,
                QuizId = q.QuizId,
                Options = options.Where(o => o.QuestionId == q.QuizQuestionId).Select(o => new QuizzOptionDetailResponse
                {
                    QuizAnswerOptionId = o.QuizAnswerOptionId,
                    QuizContent = o.QuizContent,
                    QuizScoreValue = o.QuizScoreValue,
                    QuestionId = o.QuestionId
                }).ToList()
            }).ToList();
            var result = new QuizzDetailResponse()
            {
                QuizId = quizz.QuizId,
                QuizTitle = quizz.QuizTitle,
                QuizDescription = quizz.QuizDescription,
                QuizStatus = quizz.QuizStatus,
                QuizMaxScore = quizz.QuizMaxScore,
                QuizCategoryId = quizz.QuizCategoryId,
                Questions = questionDtos

            };
            return result;
        }
    }
}
