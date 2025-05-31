using Contract.Common.Enums;
using MongoDB.Driver;
using Repository.MongoDbModels;

namespace Repository.Repositories
{
    public interface IQuizCategoryRepository
    {
        Task CreateQuizzCategoryAsync(QuizzCategoryMongo model);
        Task<List<QuizzCategoryMongo>> GetAllQuizzCategoryAsync();
        Task<QuizzCategoryMongo> GetQuizzCategoryByIdAsync(string quizzCategoryId, IClientSessionHandle session);
        Task<QuizzCategoryMongo> GetQuizzCategoryByIdAsync(string quizzCategoryId);
        Task UpdateQuizzCategoryAsync(string quizzCategoryId, QuizzCategoryMongo model);


    }
    public class QuizCategoryRepository : IQuizCategoryRepository
    {
        private readonly IMongoCollection<QuizzCategoryMongo> _quizzCategoryMongoCollection;
        public QuizCategoryRepository(IMongoUnitOfWork mongoUnitOfWork)
        {

            _quizzCategoryMongoCollection = mongoUnitOfWork.GetCollection<QuizzCategoryMongo>(MongoDBEnum.QuizzCategories.ToString());

        }

        public Task CreateQuizzCategoryAsync(QuizzCategoryMongo model)
        {
            return _quizzCategoryMongoCollection.InsertOneAsync(model);
        }

        public Task<List<QuizzCategoryMongo>> GetAllQuizzCategoryAsync()
        {
            return _quizzCategoryMongoCollection.Find(_ => true).ToListAsync();
        }

        public Task<QuizzCategoryMongo> GetQuizzCategoryByIdAsync(string quizzCategoryId, IClientSessionHandle session)
        {
            return _quizzCategoryMongoCollection.Find(session, x => x.QuizCategoryId == quizzCategoryId).FirstOrDefaultAsync();
        }
        public Task<QuizzCategoryMongo> GetQuizzCategoryByIdAsync(string quizzCategoryId)
        {
            return _quizzCategoryMongoCollection.Find( x => x.QuizCategoryId == quizzCategoryId).FirstOrDefaultAsync();
        }
        public async Task UpdateQuizzCategoryAsync(string quizzCategoryId, QuizzCategoryMongo model)
        {
            var existingQuizzCategory = await _quizzCategoryMongoCollection.Find(x => x.QuizCategoryId == quizzCategoryId).FirstOrDefaultAsync();
            if (existingQuizzCategory == null)
            {
                throw new KeyNotFoundException("quizz category not found");
            }
            existingQuizzCategory.QuizCategoryName = model.QuizCategoryName ?? existingQuizzCategory.QuizCategoryName;
            existingQuizzCategory.QuizCategoryDescription = model.QuizCategoryDescription ?? existingQuizzCategory.QuizCategoryDescription;
            await _quizzCategoryMongoCollection.ReplaceOneAsync(x => x.QuizCategoryId == quizzCategoryId, existingQuizzCategory);
        }


    }
}
