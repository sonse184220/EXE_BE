using Contract.Dtos.Requests.Payment;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Repository.Models;
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
        public PaymentController(IPaymentService paymentService,IUserService userService, IMoodJournalService moodJournalService)
        {
            _paymentService = paymentService;
            _userService = userService;
            _moodJournalService = moodJournalService;
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
                var result = await _paymentService.ConfirmWebhook(webhookBody);
              
                Console.WriteLine("webhook confirmed");
                Console.WriteLine("result" + result);
                Console.WriteLine("webhook body:" + webhookBody);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("payment-create")]
        public async Task<IActionResult> CreatePayment()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userService.GetByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            try
            {
                var domain = $"{Request.Scheme}://{Request.Host.Value}";
                PaymentRequest request = new PaymentRequest
                {
                    BuyerEmail = email,
                    BuyerName = user.FullName,
                    OrderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    Amount = 2000,
                    Description = "Test payment",
                    PaymentItems = new List<PaymentItem>
                    {
                        new PaymentItem { Name = "Item1", Quantity = 1, Price = 500 },
                        new PaymentItem { Name = "Item2", Quantity = 2, Price = 250 }
                    },
                    ReturnUrl = domain + "/innerchild/payment/success",
                    CancelUrl = domain + "/innerchild/payment/cancel",
                    ExpiredAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 1800
                };
                var result = await _paymentService.CreatePayment(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
