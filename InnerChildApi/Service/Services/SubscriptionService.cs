using Repository.Models;
using Repository.Repositories;

namespace Service.Services
{
    public interface ISubscriptionService
    {
        Task<int> CreateSubscriptionAsync(Subscription subscription);
        Task<int> UpdateSubscriptionAsync(Subscription subscription);
        Task<Subscription> GetSubscriptionByIdAsync(string subscriptionId);
        Task<List<Subscription>> GetAllSubscriptionsAsync();
    }
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        public SubscriptionService(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<int> CreateSubscriptionAsync(Subscription subscription)
        {
            return await _subscriptionRepository.CreateSubscriptionAsync(subscription);
        }

        public async Task<List<Subscription>> GetAllSubscriptionsAsync()
        {
            return await _subscriptionRepository.GetAllSubscriptionsAsync();
        }

        public async Task<Subscription> GetSubscriptionByIdAsync(string subscriptionId)
        {
            return await _subscriptionRepository.GetSubscriptionByIdAsync(subscriptionId);
        }

        public async Task<int> UpdateSubscriptionAsync(Subscription subscription)
        {
            return await _subscriptionRepository.UpdateSubscriptionAsync(subscription);
        }
    }
}
