using ERP.Context;
using ERP.DTOs;
using ERP.DTOs.BulkPurchase;
using ERP.Models;
using ERP.Services.NotificationServices;
using ERP.Services.User;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.BulkPurchaseServices
{
    public class BulkPurchaseService : IBulkPurchaseService
    {
        private readonly DataContext _context;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;

        public BulkPurchaseService(DataContext context, INotificationService notificationService, IUserService userService)
        {
            _context = context;
            _notificationService = notificationService;
            _userService = userService;
        }

        public async Task<BulkPurchase> GetById(int id)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanViewPurchase != 1) return Forbid();

            var bulkPurchase = _context.BulkPurchases.Where(bulk => bulk.BulkPurchaseId == id)
               .Include(bulk => bulk.RequestedBy)
               .Include(bulk => bulk.ApprovedBy)
               .Include(bulk => bulk.BulkPurchaseItems)
               .ThenInclude(bulkItem => bulkItem.Item)
               .ThenInclude(item => item.Material)
               .Include(bulk => bulk.BulkPurchaseItems)
               .ThenInclude(bulkItem => bulkItem.Item.Equipment)
               .FirstOrDefault();

            if (bulkPurchase == null) throw new KeyNotFoundException("BulkPurchase Not Found.");

            return bulkPurchase;
        }

        public async Task<BulkPurchase> RequestBulkPurchase(RequestBulkPurchaseDTO requestDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanCheckPurchase != 1) return Forbid();

            var bulkPurchase = await _context.BulkPurchases.FindAsync(requestDTO.BulkPurchaseId);

            if (bulkPurchase == null) throw new KeyNotFoundException("Bulk Purchase Not Found.");

            bulkPurchase.Status = BULKPURCHASESTATUS.REQUESTED;

            await _context.SaveChangesAsync();

            var centralSite = _context.Sites
                .FirstOrDefault();

            await _notificationService.Add(type: NOTIFICATIONTYPE.BULKPURCHASE,
                status: bulkPurchase.Status,
                actionId: bulkPurchase.BulkPurchaseId,
                siteId: centralSite.SiteId,
                employeeId: bulkPurchase.RequestedById);

            return bulkPurchase;
        }

        public async Task<BulkPurchase> ApproveBulkPurchase(ApproveBulkPurchaseDTO approveDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanApprovePurchase != 1) return Forbid();

            var bulkPurchase = _context.BulkPurchases
                 .Where(bulk => bulk.BulkPurchaseId == approveDTO.BulkPurchaseId)
                 .Include(bulk => bulk.BulkPurchaseItems)
                 .FirstOrDefault();

            if (bulkPurchase == null) throw new KeyNotFoundException("Bulk Purchase Not Found.");

            bulkPurchase.ApproveDate = DateTime.Now;
            bulkPurchase.ApprovedById = _userService.Employee.EmployeeId;

            foreach (var requestItem in approveDTO.BulkPurchaseItems)
            {
                var bulkPurchaseItem = bulkPurchase.BulkPurchaseItems
                    .Where(bulkPurchaseItem => bulkPurchaseItem.ItemId == requestItem.ItemId && bulkPurchaseItem.EquipmentModelId == requestItem.EquipmentModelId)
                    .FirstOrDefault();

                if (bulkPurchaseItem == null) throw new KeyNotFoundException($"Bulk Purchase Item with Id {requestItem.ItemId} Not Found");

                bulkPurchaseItem.QtyApproved = requestItem.QtyApproved;
                bulkPurchaseItem.TotalCost = bulkPurchaseItem.Cost * bulkPurchaseItem.QtyApproved;
                bulkPurchaseItem.ApproveRemark = requestItem.ApproveRemark;

            }

            bulkPurchase.TotalPurchaseCost = calculateTotalCost(bulkPurchase.BulkPurchaseItems); ;

            bulkPurchase.Status = BULKPURCHASESTATUS.APPROVED;

            await _context.SaveChangesAsync();
            
            var centralSite = _context.Sites.FirstOrDefault();

            await _notificationService.Add(type: NOTIFICATIONTYPE.BULKPURCHASE,
                status: bulkPurchase.Status,
                actionId: bulkPurchase.BulkPurchaseId,
                siteId: centralSite.SiteId,
                employeeId: bulkPurchase.RequestedById);

            return bulkPurchase;
        }

        public async Task<BulkPurchase> DeclineBulkPurchase(ApproveBulkPurchaseDTO declineDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanApprovePurchase != 1) return Forbid();

            var bulkPurchase = _context.BulkPurchases
                 .Where(bulk => bulk.BulkPurchaseId == declineDTO.BulkPurchaseId)
                 .Include(bulk => bulk.BulkPurchaseItems)
                 .FirstOrDefault();

            if (bulkPurchase == null) throw new KeyNotFoundException("Bulk Purchase Not Found.");

            bulkPurchase.ApproveDate = DateTime.Now;
            bulkPurchase.ApprovedById = _userService.Employee.EmployeeId;
            bulkPurchase.TotalPurchaseCost = 0;

            foreach (var bulkPurchaseItem in bulkPurchase.BulkPurchaseItems)
            {
                bulkPurchaseItem.QtyApproved = 0;

                bulkPurchaseItem.TotalCost = 0;
            }

            bulkPurchase.Status = BULKPURCHASESTATUS.DECLINED;

            await _context.SaveChangesAsync();

            var centralSite = _context.Sites.FirstOrDefault();

            await _notificationService.Add(type: NOTIFICATIONTYPE.BULKPURCHASE,
                status: bulkPurchase.Status,
                actionId: bulkPurchase.BulkPurchaseId,
                siteId: centralSite.SiteId,
                employeeId: bulkPurchase.RequestedById);

            return bulkPurchase;
        }

        public async Task<BulkPurchase> ConfirmBulkPurchase(ConfirmBulkPurchaseDTO confirmDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanApprovePurchase != 1) return Forbid();

            var bulkPurchase = _context.BulkPurchases
                 .Where(t => t.BulkPurchaseId == confirmDTO.BulkPurchaseId)
                 .Include(t => t.BulkPurchaseItems)
                 .FirstOrDefault();

            if (bulkPurchase == null) throw new KeyNotFoundException("BulkPurchase Not Found.");
            
            foreach (var requestItem in confirmDTO.BulkPurchaseItems)
            {
                var bulkPurchaseItem = bulkPurchase.BulkPurchaseItems
                    .Where(bulkPurchaseItem => bulkPurchaseItem.ItemId == requestItem.ItemId && bulkPurchaseItem.EquipmentModelId == requestItem.EquipmentModelId)
                    .FirstOrDefault();

                if (bulkPurchaseItem == null) throw new KeyNotFoundException($"Purchase Item with Id {requestItem.ItemId} Not Found");

                bulkPurchaseItem.QtyPurchased = requestItem.QtyPurchased;
                bulkPurchaseItem.PurchaseRemark = requestItem.PurchaseRemark;
                bulkPurchaseItem.TotalCost = bulkPurchaseItem.Cost * bulkPurchaseItem.QtyPurchased;

            }

            bulkPurchase.TotalPurchaseCost = calculateTotalCost(bulkPurchase.BulkPurchaseItems); ;
            
            var centralSite = _context.Sites.FirstOrDefault();

            bulkPurchase.Status = BULKPURCHASESTATUS.PURCHASED;

            foreach (var purchase in _context.Purchases
            .Where(purchase => purchase.BulkPurchaseId == bulkPurchase.BulkPurchaseId && purchase.Status == PURCHASESTATUS.BULKQUEUED)
            .Include(purchase => purchase.PurchaseItems))
            {
                purchase.Status = PURCHASESTATUS.PURCHASED;
                purchase.PurchaseDate = DateTime.Now;

                //Add receive
                Receive receive = new();
                receive.PurchaseId = purchase.PurchaseId;
                receive.PurchaseDate = DateTime.Now;
                receive.ReceivingSiteId = purchase.ReceivingSiteId;
                ICollection<ReceiveItem> receiveItems = new List<ReceiveItem>();
                
                foreach (var requestItem in purchase.PurchaseItems)
                {
                    ReceiveItem receiveItem = new();
                    receiveItem.ItemId = requestItem.ItemId;
                    receiveItem.Cost = requestItem.Cost;
                    receiveItem.EquipmentModelId = requestItem.EquipmentModelId;

                    receiveItems.Add(receiveItem);
                }
                
                receive.ReceiveItems = receiveItems;
                receive.Status = RECEIVESTATUS.PURCHASED;

                _context.Receives.Add(receive);

            }
            
            await _context.SaveChangesAsync();
            
            await _notificationService.Add(type: NOTIFICATIONTYPE.BULKPURCHASE,
                status: bulkPurchase.Status,
                actionId: bulkPurchase.BulkPurchaseId,
                siteId: centralSite.SiteId,
                employeeId: bulkPurchase.RequestedById);
            return bulkPurchase;
        }


        private decimal calculateTotalCost(ICollection<BulkPurchaseItem> Items)
        {
            var totalCost = (decimal)0;

            foreach (var bulkPurchaseItem in Items)
            {
                totalCost += bulkPurchaseItem.TotalCost;
            }

            return totalCost;
        }


    }
}
