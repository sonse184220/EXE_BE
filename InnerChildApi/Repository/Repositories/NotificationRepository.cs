using Repository.Base;
using Repository.Models;

namespace Repository.Repositories
{
    public interface INotificationRepository
    {
        Task<int> CreateNotificationAsync(Notification notification);
        Task<List<Notification>> GetAllNotificationsAsync();
        Task<bool> DeleteNotificationAsync(Notification notification);
        Task<Notification> GetNotificationByIdAsync(string id);
    }
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {

        public async Task<int> CreateNotificationAsync(Notification notification)
        {
            return await CreateAsync(notification);
        }

        public async Task<bool> DeleteNotificationAsync(Notification notification)
        {
            return await RemoveAsync(notification);
        }

        public async Task<List<Notification>> GetAllNotificationsAsync()
        {
            return await GetAllAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(string id)
        {
            return await GetByIdAsync(id);
        }
    }
}
