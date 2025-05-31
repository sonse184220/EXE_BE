using System.ComponentModel.DataAnnotations;

namespace Contract.Dtos.Requests.Notification
{
    public class NotificationCreateRequest
    {
        [Required(ErrorMessage = "Device token is required")]
        public string DeviceToken { get; set; }
        public string NotificationUrl { get; set; }
        [Required(ErrorMessage = "Name is required!")]
        public string NotificationName { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string NotificationDescription { get; set; }


    }
}
