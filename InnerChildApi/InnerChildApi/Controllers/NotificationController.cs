using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Service.Services;

namespace InnerChildApi.Controllers
{
    [Route("innerchild/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateNotification([FromForm] Notification request)
        {
            try
            {
                await _notificationService.CreateNotificationAsync(request);
                return Created("", new { message = "Notification created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong " + ex.Message);
            }

        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllNotification()
        {
            var notifications = await _notificationService.GetAllNotificationsAsync();
            return Ok(notifications);

        }
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetNotificationById(string id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null)
                return NotFound("Notification not found");
            return Ok(notification);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteNotification(string id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null)
                return NotFound("Notification not found");
            try
            {
                var deleteResult = await _notificationService.DeleteNotificationAsync(notification);
                return Ok("Deleted notification");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


    }
}
