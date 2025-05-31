using Microsoft.Extensions.Options;
using MongoDB.Driver;
using static Contract.Common.Config.AppSettingConfig;

namespace Repository
{
    public interface IMongoUnitOfWork
    {

        IClientSessionHandle Session { get; }
        Task StartTransactionAsync();
        Task CommitAsync();
        Task AbortAsync();
        IMongoCollection<T> GetCollection<T>(string name);
    }
    public class MongoUnitOfWork : IMongoUnitOfWork
    {
        private readonly IMongoClient _client;
        private IMongoDatabase _database;
        public IClientSessionHandle Session { get; private set; }
        public MongoUnitOfWork(IOptions<QuizzSettingConfig> settings)
        {
            _client = new MongoClient(settings.Value.ConnectionString);
            _database = _client.GetDatabase(settings.Value.DatabaseName);
        }

        public Task AbortAsync()
        {
            return Session.AbortTransactionAsync();
        }

        public Task CommitAsync()
        {
            return Session.CommitTransactionAsync();
        }

        public async Task StartTransactionAsync()
        {
            Session = await _client.StartSessionAsync();
            Session.StartTransaction();
        }
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}
