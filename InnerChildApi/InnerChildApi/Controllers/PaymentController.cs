using Contract.Dtos.Requests.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Service.Services;
using System.Security.Claims;

namespace InnerChildApi.Controllers
{
    [Route("innerchild/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMoodJournalService _moodJournalService;
        private readonly IPaymentService _paymentService;
        private readonly IUserService _userService;
        private readonly ILogger<PaymentController> _logger;
        public PaymentController(IPaymentService paymentService, IUserService userService, IMoodJournalService moodJournalService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _userService = userService;
            _moodJournalService = moodJournalService;
            _logger = logger;
        }
        [HttpGet("success")]
        public IActionResult Success()
        {
            return Ok("Payment was successful.");
        }
        [HttpGet("cancel")]
        public IActionResult Cancel()
        {
            return Ok("Payment was cancelled.");
        }
        [HttpPost("confirm-webhook")]
        public async Task<IActionResult> ConfirmWebhook([FromBody] WebhookType webhookBody)
        {
            try
            {
                await _paymentService.ConfirmWebhook(webhookBody);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }
        [Authorize]
        [HttpPost("payment-create")]
        public async Task<IActionResult> CreatePayment([FromBody] BuySubscriptionRequest buyRequest)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            try
            {
                if (userId == null)
                {
                    return NotFound();
                }
                var user = await _userService.GetByEmailAsync(email);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var existingPlan = await _paymentService.GetSubscriptionByIdAsync(buyRequest.SubscriptionId);
                if (existingPlan == null)
                {
                    return NotFound("Plan not found.");
                }
                if (existingPlan.SubscriptionPrice % 1 != 0)
                {
                    return BadRequest("Subscription price is invalid.");
                }
                int planAmount = (int)existingPlan.SubscriptionPrice;
                var domain = $"{Request.Scheme}://{Request.Host.Value}";
                PaymentRequest request = new PaymentRequest
                {
                    BuyerEmail = email,
                    BuyerName = user.FullName,
                    OrderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    Amount = planAmount,
                    Description = $"Purchase {existingPlan.SubscriptionDescription}",
                    PaymentItems = new List<PaymentItem>
                    {
                        new PaymentItem { Name = $"{existingPlan.SubscriptionType}", Quantity = 1, Price = planAmount },
                    },
                    ReturnUrl = domain + "/innerchild/payment/success",
                    CancelUrl = domain + "/innerchild/payment/cancel",
                    ExpiredAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 3600,
                    UserId = userId,
                    SubscriptionId = buyRequest.SubscriptionId,
                };
                var result = await _paymentService.CreatePayment(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }
    }
}
