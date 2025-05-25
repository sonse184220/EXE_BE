using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
