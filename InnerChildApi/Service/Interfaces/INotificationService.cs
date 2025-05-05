using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
