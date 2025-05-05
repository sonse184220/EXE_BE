using Repository.Base;
using Repository.Interfaces;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>,INotificationRepository
    {
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
