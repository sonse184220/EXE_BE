using Repository.Models;
using Repository.Repositories;

namespace Service.Services
{
    public interface INotificationService
    {
        Task<int> CreateNotificationAsync(Notification notification);
        Task<List<Notification>> GetAllNotificationsAsync();
        Task<bool> DeleteNotificationAsync(Notification notification);
        Task<Notification> GetNotificationByIdAsync(string id);
    }
    public class NotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly INotificationRepository _notiRepo;
        public NotificationService(INotificationRepository notiRepo)
        {
            _httpClient = new HttpClient();
            _notiRepo = notiRepo;
        }
        public async Task<int> CreateNotificationAsync(Notification notification)
        {
            return await _notiRepo.CreateNotificationAsync(notification);
        }

        public async Task<bool> DeleteNotificationAsync(Notification notification)
        {
            return await _notiRepo.DeleteNotificationAsync(notification);
        }

        public async Task<List<Notification>> GetAllNotificationsAsync()
        {
            return await _notiRepo.GetAllNotificationsAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(string id)
        {
            return await _notiRepo.GetNotificationByIdAsync(id);
        }


    }
}
