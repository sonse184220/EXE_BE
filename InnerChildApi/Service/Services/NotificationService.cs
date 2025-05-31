using FirebaseAdmin;
using Microsoft.Extensions.Options;
using Repository.Models;
using Repository.Repositories;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static Contract.Common.Config.AppSettingConfig;

namespace Service.Services
{
    public interface INotificationService
    {
        Task<int> CreateNotificationAsync(Notification notification);
        Task<List<Notification>> GetAllOwnNotificationsAsync(string userId);
        Task<bool> DeleteNotificationAsync(Notification notification);
        Task<Notification> GetNotificationByIdAsync(string id, string userId);
        Task<bool> SendPushAsync(string deviceToken, string title, string body);
    }
    public class NotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly INotificationRepository _notiRepo;
        private readonly string projectId;
        private readonly FirebaseSettings _firebaseSettingConfig;
        public NotificationService(INotificationRepository notiRepo, IOptions<FirebaseSettings> firebaseSettingConfig, HttpClient httpClient)
        {
            _firebaseSettingConfig = firebaseSettingConfig.Value;
            projectId = _firebaseSettingConfig.ProjectId;
            _httpClient = httpClient;
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

        public async Task<List<Notification>> GetAllOwnNotificationsAsync(string userId)
        {
            return await _notiRepo.GetAllOwnNotificationsAsync(userId);
        }

        public async Task<Notification> GetNotificationByIdAsync(string id, string userId)
        {
            return await _notiRepo.GetNotificationByIdAsync(id, userId);
        }
        private async Task<string> GetAccessToken()
        {
            var credential = FirebaseApp.DefaultInstance.Options.Credential;
            if (credential == null)
            {
                throw new Exception("Credential firebase not initialized.");

            }
            ;
            var scope = credential.CreateScoped("https://www.googleapis.com/auth/firebase.messaging");
            return await scope.UnderlyingCredential.GetAccessTokenForRequestAsync();
        }

        public async Task<bool> SendPushAsync(string deviceToken, string title, string body)
        {

            var accessToken = await GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var message = new
            {
                message = new
                {
                    token = deviceToken,
                    notification = new { title, body },
                    android = new { priority = "high" }

                }
            };
            var json = JsonSerializer.Serialize(message);
            var url = $"https://fcm.googleapis.com/v1/projects/{projectId}/messages:send";
            var response = await _httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine(error);
            }
            return response.IsSuccessStatusCode;



        }



    }
}
