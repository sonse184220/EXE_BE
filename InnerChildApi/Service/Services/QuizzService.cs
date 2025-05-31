using Contract.Common.Enums;
using Contract.Dtos.Requests.Quizz;
using Contract.Dtos.Responses.Quizz;
using MongoDB.Bson;
using MongoDB.Driver;
using Repository;
using Repository.Models;
using Repository.MongoDbModels;
using Repository.Repositories;

namespace Service.Services
{
    public interface IQuizzService
    {
        #region quizz category
        Task CreateQuizzCategoryAsync(QuizzCategoryMongo model);
        Task<List<QuizzCategoryMongo>> GetAllQuizzCategoryAsync();
        Task<QuizzCategoryMongo> GetQuizzCategoryByIdAsync(string quizzCategoryId);

        Task<QuizzCategoryMongo> GetQuizzCategoryByIdAsync(string quizzCategoryId, IClientSessionHandle session);
        Task UpdateQuizzCategoryAsync(string quizzCategoryId, QuizzCategoryMongo model);
        #endregion
        #region quizz transaction
        Task CreateQuizzTransacAsync(QuizzCreateRequest.CreateQuizzDto request);
        #endregion

        #region quizz 
        Task<QuizzDetailResponse> GetFullQuizDetailByIdAsync(string quizzId);
        Task<List<QuizzMongo>> GetAllQuizzAsync();
        #endregion


    }
    public class QuizzService : IQuizzService
    {
        private readonly IQuizCategoryRepository _quizCategoryRepository;
        private readonly IQuizzRepository _quizzRepository;
        private readonly IQuizzQuestionRepository _quizzQuestionRepository;
        private readonly IQuizzOptionRepository _quizzOptionRepository;
        private readonly IMongoUnitOfWork _mongoUnitOfWork;
        public QuizzService(IQuizzOptionRepository quizzOptionRepository, IQuizCategoryRepository quizCategoryRepository, IMongoUnitOfWork mongoUnitOfWork, IQuizzRepository quizzRepository, IQuizzQuestionRepository quizzQuestionRepository)
        {
            _quizCategoryRepository = quizCategoryRepository;
            _quizzQuestionRepository = quizzQuestionRepository;
            _quizzRepository = quizzRepository;
            _quizzOptionRepository = quizzOptionRepository;
            _mongoUnitOfWork = mongoUnitOfWork;
        }

        public async Task CreateQuizzCategoryAsync(QuizzCategoryMongo model)
        {
            await _quizCategoryRepository.CreateQuizzCategoryAsync(model);
        }

        public async Task CreateQuizzTransacAsync(QuizzCreateRequest.CreateQuizzDto request)
        {
            await _mongoUnitOfWork.StartTransactionAsync();
            try
            {
                var existingQuizzCategory = await this.GetQuizzCategoryByIdAsync(request.QuizCategoryId, _mongoUnitOfWork.Session);
                if (existingQuizzCategory == null)
                {
                    throw new KeyNotFoundException($"Quiz category with id {request.QuizCategoryId} not found");

                }
                var quizzId = ObjectId.GenerateNewId().ToString();
                var timeNow = DateTime.UtcNow;
                var quizz = new QuizzMongo()
                {
                    QuizId = quizzId,
                    QuizCategoryId = request.QuizCategoryId,
                    QuizDescription = request.QuizDescription,
                    QuizCreatedAt = timeNow,
                    QuizMaxScore = request.QuizMaxScore,
                    QuizStatus = QuizzEnum.Active.ToString(),
                    QuizTitle = request.QuizTitle,
                    QuizUpdatedAt = timeNow,

                };
                var questions = new List<QuizzQuestionMongo>();
                var options = new List<QuizzOptionMongo>();
                foreach (var questionDto in request.QuizzQuestions)
                {
                    var questionId = ObjectId.GenerateNewId().ToString();

                    questions.Add(new QuizzQuestionMongo()
                    {
                        QuizId = quizzId,
                        QuizQuestionContent = questionDto.QuizQuestionContent,
                        QuizQuestionId = questionId,
                        QuizQuestionOrder = questionDto.QuizQuestionOrder
                    });
                    foreach (var optionDto in questionDto.QuizzOptions)
                    {
                        options.Add(new QuizzOptionMongo()
                        {
                            QuestionId = questionId,
                            QuizAnswerOptionId = ObjectId.GenerateNewId().ToString(),
                            QuizContent = optionDto.QuizContent,
                            QuizScoreValue = optionDto.QuizScoreValue,

                        });
                    }
                }
                await _quizzRepository.CreateQuizzAsync(quizz, _mongoUnitOfWork.Session);
                await _quizzQuestionRepository.CreateQuizzQuestionAsync(questions, _mongoUnitOfWork.Session);
                await _quizzOptionRepository.CreateQuizzOptionAsync(options, _mongoUnitOfWork.Session);


                await _mongoUnitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _mongoUnitOfWork.AbortAsync();
                throw new Exception(ex.Message);
            }
        }

        public Task<List<QuizzMongo>> GetAllQuizzAsync()
        {
            return _quizzRepository.GetAllQuizzAsync();
        }

        public async Task<List<QuizzCategoryMongo>> GetAllQuizzCategoryAsync()
        {
            return await _quizCategoryRepository.GetAllQuizzCategoryAsync();
        }

        public Task<QuizzDetailResponse> GetFullQuizDetailByIdAsync(string quizzId)
        {
           return _quizzRepository.GetFullQuizDetailByIdAsync(quizzId);
        }

        public async Task<QuizzCategoryMongo> GetQuizzCategoryByIdAsync(string quizzCategoryId, IClientSessionHandle session)
        {
            return await _quizCategoryRepository.GetQuizzCategoryByIdAsync(quizzCategoryId, session);
        }

        public async Task<QuizzCategoryMongo> GetQuizzCategoryByIdAsync(string quizzCategoryId)
        {
            return await _quizCategoryRepository.GetQuizzCategoryByIdAsync(quizzCategoryId);
        }

        public async Task UpdateQuizzCategoryAsync(string quizzCategoryId, QuizzCategoryMongo model)
        {
            await _quizCategoryRepository.UpdateQuizzCategoryAsync(quizzCategoryId, model);
        }
    }
}
