using Contract.Dtos.Requests.Subscription;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Service.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InnerChildApi.Controllers
{
    [Route("innerchild/subscription")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly ILogger<SubscriptionController> _logger;
        public SubscriptionController(ISubscriptionService subscriptionService, ILogger<SubscriptionController> logger)
        {
            _subscriptionService = subscriptionService;
            _logger = logger;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllSubcription()
        {
            var subscriptions = await _subscriptionService.GetAllSubscriptionsAsync();
            return Ok(subscriptions);
        }

        [Authorize]
        [HttpGet("detail/{subscriptionId}")]
        public async Task<IActionResult> GetOwnSubscription(string subscriptionId)
        {
            var subscription = await _subscriptionService.GetSubscriptionByIdAsync(subscriptionId);
            if (subscription == null)
            {
                return NotFound("Subscription not found");
            }
            return Ok(subscription);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSubscription([FromBody] SubscriptionCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var subscription = new Subscription()
                {
                    SubscriptionId = Guid.NewGuid().ToString(),
                    SubscriptionType = request.SubscriptionType.ToString(),
                    SubscriptionDescription = request.SubscriptionDescription,
                    SubscriptionPrice = request.SubscriptionPrice,
                };
                var result = await _subscriptionService.CreateSubscriptionAsync(subscription);
                if (result > 0)
                {
                    return Created("", new { message = "Subscription created successfully" });
                }
                return StatusCode(500, "Create subscription failed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPut("update/{subscriptionId}")]
        public async Task<IActionResult> UpdateSubscription(string subscriptionId, [FromBody] SubscriptionUpdateRequest request)
        {
            try
            {
                var existingSubscription = await _subscriptionService.GetSubscriptionByIdAsync(subscriptionId);
                if (existingSubscription == null)
                {
                    return NotFound("Subscription not found");
                }
                existingSubscription.SubscriptionDescription = request.SubscriptionDescription ?? existingSubscription.SubscriptionDescription;
                if (request.SubscriptionPrice.HasValue && request.SubscriptionPrice.Value > 0)
                {
                    existingSubscription.SubscriptionPrice = request.SubscriptionPrice.Value;
                }
                if (request.SubscriptionType.HasValue)
                {
                    existingSubscription.SubscriptionType = request.SubscriptionType?.ToString();
                }
                var updatedRows = await _subscriptionService.UpdateSubscriptionAsync(existingSubscription);
                if (updatedRows <= 0) return StatusCode(500);
                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }


    }
}
