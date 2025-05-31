using Contract.Dtos.Requests.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Service.Services;
using System.Security.Claims;

namespace InnerChildApi.Controllers
{
    [Route("innerchild/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationCreateRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var notification = new Notification()
            {
                NotificationId = Guid.NewGuid().ToString(),
                NotificationName = request.NotificationName,
                NotificationDescription = request.NotificationDescription,
                NotificationUrl = request.NotificationUrl,
                UserId = userId,
            };
            try
            {
                await _notificationService.SendPushAsync(request.DeviceToken, request.NotificationName, request.NotificationDescription);
                await _notificationService.CreateNotificationAsync(notification);
                return Created("", new { message = "Notification created successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, $"Something went wrong");
            }

        }
        [Authorize]
        [HttpGet("all-own-notification")]
        public async Task<IActionResult> GetAllNotification()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return NotFound();
            }
            var notifications = await _notificationService.GetAllOwnNotificationsAsync(userId);
            return Ok(notifications);

        }
        [Authorize]
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetNotificationById(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return NotFound();
            }
            var notification = await _notificationService.GetNotificationByIdAsync(id, userId);
            if (notification == null)
                return NotFound("Notification not found");
            return Ok(notification);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteNotification(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return NotFound();
            }
            var notification = await _notificationService.GetNotificationByIdAsync(id, userId);
            if (notification == null)
                return NotFound("Notification not found");
            try
            {
                var deleteResult = await _notificationService.DeleteNotificationAsync(notification);
                return Ok("Deleted notification");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "An error occurred");
            }
        }


    }
}
