using Contract.Common.Enums;
using MongoDB.Driver;
using Repository.MongoDbModels;

namespace Repository.Repositories
{
    public interface IQuizzOptionRepository
    {
        Task CreateQuizzOptionAsync(List<QuizzOptionMongo> model, IClientSessionHandle session);

    }
    public class QuizzOptionRepository : IQuizzOptionRepository
    {

        private IMongoCollection<QuizzOptionMongo> _quizzOptionMongoCollection;

        public QuizzOptionRepository(IMongoUnitOfWork unitOfWork)
        {
            _quizzOptionMongoCollection = unitOfWork.GetCollection<QuizzOptionMongo>(MongoDBEnum.QuizzOptions.ToString());
        }
        public Task CreateQuizzOptionAsync(List<QuizzOptionMongo> model, IClientSessionHandle session)
        {
            return _quizzOptionMongoCollection.InsertManyAsync(session, model);

        }
    }
}
