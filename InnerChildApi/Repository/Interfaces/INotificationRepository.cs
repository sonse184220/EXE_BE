using Repository.Models;

namespace Repository.Interfaces
{
    public interface INotificationRepository
    {
        Task<int> CreateNotificationAsync(Notification notification);
        Task<List<Notification>> GetAllNotificationsAsync();
        Task<bool> DeleteNotificationAsync(Notification notification);
        Task<Notification> GetNotificationByIdAsync(string id);
    }
}
