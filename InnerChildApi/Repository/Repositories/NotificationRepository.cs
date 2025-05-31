using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Models;

namespace Repository.Repositories
{
    public interface INotificationRepository
    {
        Task<int> CreateNotificationAsync(Notification notification);
        Task<List<Notification>> GetAllOwnNotificationsAsync(string userId);
        Task<bool> DeleteNotificationAsync(Notification notification);
        Task<Notification> GetNotificationByIdAsync(string notificationId, string userId);
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

        public async Task<List<Notification>> GetAllOwnNotificationsAsync(string userId)
        {
            return await _context.Notifications.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(string notificationId, string userId)
        {
            return await _context.Notifications.FirstOrDefaultAsync(x => x.NotificationId == notificationId && x.UserId == userId);
        }
    }
}
