using Service.Services;

namespace Service.BackgroundJobs
{
    public class PurchaseCheckJob
    {
        private readonly IPurchaseService _purchaseService;

        public PurchaseCheckJob(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }
        public async Task Run()
        {
            await _purchaseService.CheckPurchaseExpiry();
        }

    }
}
