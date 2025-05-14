using Repository.Models;

namespace Service.Interfaces
{
    public interface INotificationService
    {
        Task<int> CreateNotificationAsync(Notification notification);
        Task<List<Notification>> GetAllNotificationsAsync();
        Task<bool> DeleteNotificationAsync(Notification notification);
        Task<Notification> GetNotificationByIdAsync(string id);
    }
}
