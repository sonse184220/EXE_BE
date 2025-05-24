using Contract.Common.Enums;
using Contract.Dtos.Requests.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;
using Repository;
using Repository.Models;
using Repository.Repositories;
using static Contract.Common.Config.AppSettingConfig;

namespace Service.Services
{
    public interface IPaymentService
    {
        Task<string> CreatePayment(PaymentRequest request);
        Task ConfirmWebhook(WebhookType webhookBody);


        Task<Subscription> GetSubscriptionByIdAsync(string subscriptionId);
        Task<int> ConfirmTransaction(string transactionId);
    }
    public class PaymentService : IPaymentService
    {
        private readonly PayOsSettingConfig _payOsSettingConfig;
        private string apiKey;
        private string clientId;
        private string checksumKey;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        public PaymentService(IOptions<PayOsSettingConfig> payOsSettingConfig, IHttpContextAccessor httpContextAccessor, IPurchaseRepository purchaseRepository, ISubscriptionRepository subscriptionRepository, ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
        {
            _payOsSettingConfig = payOsSettingConfig.Value;
            _httpContextAccessor = httpContextAccessor;
            #region set value for os
            apiKey = _payOsSettingConfig.ApiKey;
            clientId = _payOsSettingConfig.ClientId;
            checksumKey = _payOsSettingConfig.ChecksumKey;
            #endregion
            _purchaseRepository = purchaseRepository;
            _subscriptionRepository = subscriptionRepository;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }
        private PayOS PayOSChecker()
        {
            var payOs = new PayOS(clientId, apiKey, checksumKey);
            return payOs;

        }
        public async Task<string> CreatePayment(PaymentRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var payOs = PayOSChecker();
                List<ItemData> items = new List<ItemData>();
                foreach (var paymentItem in request.PaymentItems)
                {
                    ItemData item = new ItemData(paymentItem.Name, paymentItem.Quantity, paymentItem.Price);
                    items.Add(item);
                }
                var paymentLinkRequest = new PaymentData(
                    orderCode: request.OrderCode,
                    amount: request.Amount,
                    description: request.Description,
                    items: items,
                    cancelUrl: request.CancelUrl,
                    returnUrl: request.ReturnUrl,
                    buyerName: request.BuyerName,
                    buyerEmail: request.BuyerEmail,
                    expiredAt: request.ExpiredAt
                );
                var response = await payOs.createPaymentLink(paymentLinkRequest);
                var newTransaction = new Repository.Models.Transaction()
                {
                    TransactionId = request.OrderCode.ToString(),
                    TransactionPaymentGateway = "PayOS",
                    TransactionAmount = request.Amount,
                    TransactionStatus = PaymentStatusEnum.Pending.ToString(),
                    TransactionCode = request.OrderCode.ToString(),
                    TransactionCreatedAt = DateTime.UtcNow,
                    UserId = request.UserId,
                    SubscriptionId = request.SubscriptionId,
                };
                await _unitOfWork.TransactionRepository.CreateTransactionAsync(newTransaction);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return response.checkoutUrl;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception(ex.Message);
            }

        }
        public async Task<PaymentLinkInformation> GetPaymentInfo(int orderCode)
        {
            var payOs = PayOSChecker();
            var response = await payOs.getPaymentLinkInformation(orderCode);
            return response;
        }
        public async Task ConfirmWebhook(WebhookType webhookBody)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var payOs = PayOSChecker();
                var verifiedData = payOs.verifyPaymentWebhookData(webhookBody);
                Console.WriteLine(verifiedData);
                if (webhookBody.success)
                {
                    var existingTransaction = await _unitOfWork.TransactionRepository.GetTransactionByIdAsync(verifiedData.orderCode.ToString());
                    existingTransaction.TransactionCurrency = verifiedData.currency;
                    existingTransaction.TransactionStatus = PaymentStatusEnum.Success.ToString();
                    existingTransaction.TransactionPaymentStatus = verifiedData.desc;
                    await _unitOfWork.TransactionRepository.UpdateTransactionAsync(existingTransaction);
                    var purchasedTime = DateTime.UtcNow;
                    var newPurchase = new Purchase()
                    {
                        PurchaseId = Guid.NewGuid().ToString(),
                        SubscriptionId = existingTransaction.SubscriptionId,
                        UserId = existingTransaction.UserId,
                        PurchasedAt = purchasedTime,
                        ExpireAt = purchasedTime.AddDays(60),
                        IsActive = true
                    };
                    var subscriptionChecked = existingTransaction.Subscription?.SubscriptionType.Equals(SubscriptionEnum.FamilyPlan);
                    if (subscriptionChecked == true && existingTransaction.User?.Profiles.Count == 1)
                    {
                        List<Profile> profiles = new List<Profile>();
                        for (int i = 0; i < 3; i++)
                        {
                            var newProfile = new Profile()
                            {
                                ProfileId = Guid.NewGuid().ToString(),
                                UserId = existingTransaction.UserId,
                                ProfileCreatedAt = DateTime.UtcNow,
                                ProfileStatus = UserAccountEnum.Active.ToString(),
                            };
                            profiles.Add(newProfile);
                        }
                        _unitOfWork.ProfileRepository.CreateMutipleProfiles(profiles);
                    }
                    await _unitOfWork.PurchaseRepository.CreatePurchaseAsync(newPurchase);
                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception(ex.Message);
            }

        }

        public Task<Subscription> GetSubscriptionByIdAsync(string subscriptionId)
        {
            return _subscriptionRepository.GetSubscriptionByIdAsync(subscriptionId);
        }

        public Task<int> ConfirmTransaction(string transactionId)
        {
            throw new NotImplementedException();
        }
    }
}
