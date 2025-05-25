using Org.BouncyCastle.Security;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
