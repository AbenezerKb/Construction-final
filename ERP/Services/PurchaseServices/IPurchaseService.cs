using ERP.DTOs;
using ERP.DTOs.Purchase;
using ERP.Models;

namespace ERP.Services.PurchaseServices
{
    public interface IPurchaseService
    {
        public Task<Purchase> GetById(int id);
        public Task<List<Purchase>> GetByCondition(GetPurchasesDTO getPurchasesDTO);
        public Task<Purchase> RequestEquipment(CreateEquipmentPurchaseDTO purchaseDTO);
        Task<Purchase> RequestMaterial(CreateMaterialPurchaseDTO purchaseDTO);
        Task<Purchase> ApprovePurchase(ApprovePurchaseDTO approveDTO);
        Task<Purchase> DeclinePurchase(ApprovePurchaseDTO declineDTO);
        Task<Purchase> CheckPurchase(CheckPurchaseDTO checkDTO);
        public Task<Purchase> QueuePurchase(CheckPurchaseDTO checkDTO);
        Task<Purchase> RequestQueuedPurchase(CheckPurchaseDTO checkDTO);
        Task<Purchase> ConfirmPurchase(ConfirmPurchaseDTO confirmDTO);
    }
}
