using ERP.DTOs.BulkPurchase;
using ERP.DTOs.Purchase;
using ERP.Models;

namespace ERP.Services.BulkPurchaseServices
{
    public interface IBulkPurchaseService
    {
        public Task<BulkPurchase> GetById(int id);
        public Task<BulkPurchase> RequestBulkPurchase(RequestBulkPurchaseDTO requestDTO);
        public Task<BulkPurchase> ApproveBulkPurchase(ApproveBulkPurchaseDTO approveDTO);
        public Task<BulkPurchase> DeclineBulkPurchase(ApproveBulkPurchaseDTO declineDTO);
        public Task<BulkPurchase> ConfirmBulkPurchase(ConfirmBulkPurchaseDTO confirmDTO);

    }
}
