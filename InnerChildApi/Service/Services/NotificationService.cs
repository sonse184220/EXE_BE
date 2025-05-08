using Repository.Interfaces;
using Repository.Models;
using Repository.Repositories;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notiRepo;
        public NotificationService(INotificationRepository notiRepo)
        {
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
