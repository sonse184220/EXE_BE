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
        private readonly NotificationRepository _notiRepo;
        public NotificationService(NotificationRepository notiRepo)
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

        public Task<List<Notification>> GetAllNotificationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Notification> GetNotificationByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
