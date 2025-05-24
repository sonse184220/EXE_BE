using Repository.Base;
using Repository.Models;

namespace Repository.Repositories
{
    public interface ISubscriptionRepository
    {
        Task<int> CreateSubscriptionAsync(Subscription subscription);
        Task<int> UpdateSubscriptionAsync(Subscription subscription);
        Task<Subscription> GetSubscriptionByIdAsync(string subscriptionId);
        Task<List<Subscription>> GetAllSubscriptionsAsync();
    }
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        public async Task<int> CreateSubscriptionAsync(Subscription subscription)
        {
            return await CreateAsync(subscription);
        }
        public async Task<int> UpdateSubscriptionAsync(Subscription subscription)
        {
            return await UpdateAsync(subscription);
        }
        public async Task<Subscription> GetSubscriptionByIdAsync(string subscriptionId)
        {
            return await GetByIdAsync(subscriptionId);
        }
        public async Task<List<Subscription>> GetAllSubscriptionsAsync()
        {
            return await GetAllAsync();
        }
    }
}
