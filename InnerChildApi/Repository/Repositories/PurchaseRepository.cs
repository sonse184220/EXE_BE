using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.DBContext;
using Repository.Models;

namespace Repository.Repositories
{
    public interface IPurchaseRepository
    {
        Task<int> CreatePurchaseAsync(Purchase purchase);
        Task<Purchase> GetOwnPuchase(string userId);
        Task<int> UpdatePurchaseAsync(Purchase purchase);
        void PrepareCreatePurchase(Purchase purchase);
        Task CheckPurchaseExpiry();
    }
    public class PurchaseRepository : GenericRepository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository()
        {

        }
        public PurchaseRepository(InnerChildExeContext context) : base(context)
        {

        }
        public async Task<int> CreatePurchaseAsync(Purchase purchase)
        {
            return await CreateAsync(purchase);
        }

        public async Task<Purchase> GetOwnPuchase(string userId)
        {
            return await _context.Purchases.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<int> UpdatePurchaseAsync(Purchase purchase)
        {
            return await UpdateAsync(purchase);
        }
        public void PrepareCreatePurchase(Purchase purchase)
        {
            PrepareCreate(purchase);
        }


        public async Task CheckPurchaseExpiry()
        {
            var timeNow = DateTime.UtcNow;
            var allPurchases = await _context.Purchases.Where(x=>x.ExpireAt < timeNow && x.IsActive==true).ToListAsync();
            foreach (var purchase in allPurchases)
            {
                purchase.IsActive = false;
                _context.Purchases.Update(purchase);
            }
            await _context.SaveChangesAsync();
        }
    }
}
