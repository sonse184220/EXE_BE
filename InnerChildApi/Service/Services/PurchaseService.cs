using Repository.Repositories;

namespace Service.Services
{
    public interface IPurchaseService
    {
        Task CheckPurchaseExpiry();
    }
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        public PurchaseService(IPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }
        public async Task CheckPurchaseExpiry()
        {
            await _purchaseRepository.CheckPurchaseExpiry();
        }
    }
}
