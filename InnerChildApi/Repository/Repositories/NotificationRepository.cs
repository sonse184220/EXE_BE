using Repository.Base;
using Repository.DBContext;
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
        public NotificationRepository() : base()
        {

        }
        public NotificationRepository(InnerChildExeContext context) : base(context)
        {

        }
        public async Task<int> CreateNotificationAsync(Notification notification)
        {
            return await base.CreateAsync(notification);
        }

        public async Task<bool> DeleteNotificationAsync(Notification notification)
        {
            return await base.RemoveAsync(notification);
        }

        public async Task<List<Notification>> GetAllNotificationsAsync()
        {
            return await base.GetAllAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(string id)
        {
            return await base.GetByIdAsync(id);
        }
    }
}
