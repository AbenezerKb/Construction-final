using ERP.Context;
using ERP.DTOs;
using ERP.DTOs.Purchase;
using ERP.Models;
using ERP.Services.ItemSiteQtyServices;
using ERP.Services.NotificationServices;
using ERP.Services.User;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.PurchaseServices
{
    public class PurchaseService : IPurchaseService
    {
        private readonly DataContext _context;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IItemSiteQtyService _itemSiteQtyService;

        public PurchaseService(DataContext context, INotificationService notificationService, IUserService userService, IItemSiteQtyService itemSiteQtyService)
        {
            _context = context;
            _notificationService = notificationService;
            _userService = userService;
            _itemSiteQtyService = itemSiteQtyService;
        }

        public async Task<Purchase> GetById(int id)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanViewPurchase != 1) return Forbid();

            var purchase = _context.Purchases.Where(purchase => purchase.PurchaseId == id)
               .Include(purchase => purchase.RequestedBy)
               .Include(purchase => purchase.CheckedBy)
               .Include(purchase => purchase.ApprovedBy)
               .Include(purchase => purchase.ReceivingSite)
               .Include(purchase => purchase.PurchaseItems)
               .ThenInclude(purchaseItem => purchaseItem.Item)
               .ThenInclude(item => item.Material)
               .Include(purchase => purchase.PurchaseItems)
               .ThenInclude(purchaseItem => purchaseItem.Item.Equipment)
               .FirstOrDefault();

            if (purchase == null) throw new KeyNotFoundException("Purchase Not Found.");

            return purchase;
        }

        public async Task<List<Purchase>> GetByCondition(GetPurchasesDTO getPurchasesDTO)
        {
            UserRole userRole = _userService.UserRole;
            int employeeId = _userService.Employee.EmployeeId;

            var purchases = await _context.Purchases
                .Where(purchase => (
                    (getPurchasesDTO.ReceivingSiteId == -1 || purchase.ReceivingSiteId == getPurchasesDTO.ReceivingSiteId) &&
                    ((userRole.CanViewPurchase == true && (purchase.Status == PURCHASESTATUS.DECLINED || 
                    purchase.Status == PURCHASESTATUS.BULKQUEUED || purchase.Status == PURCHASESTATUS.PURCHASED)) ||
                    (userRole.CanRequestPurchase == true && purchase.Status == PURCHASESTATUS.QUEUED) ||
                    (userRole.CanCheckPurchase == true && purchase.Status == PURCHASESTATUS.REQUESTED) ||
                    (userRole.CanApprovePurchase == true && purchase.Status == PURCHASESTATUS.CHECKED) ||
                    (userRole.CanConfirmPurchase == true && purchase.Status == PURCHASESTATUS.APPROVED))))
                .Include(purchase => purchase.ReceivingSite)
                .OrderByDescending(purchase => purchase.PurchaseId)
                .ToListAsync();

            return purchases;
        }

        public async Task<Purchase> RequestMaterial(CreateMaterialPurchaseDTO purchaseDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanRequestPurchase != 1) return Forbid();

            Purchase purchase = new();
            purchase.RequestedById = _userService.Employee.EmployeeId;
            purchase.ReceivingSiteId = purchaseDTO.ReceivingSiteId;

            ICollection<PurchaseItem> purchaseItems = new List<PurchaseItem>();
            ICollection<PurchaseItemEmployee> purchaseItemEmployees = new List<PurchaseItemEmployee>();

            foreach (var requestItem in purchaseDTO.PurchaseItems)
            {
                PurchaseItem purchaseItem = new();
                purchaseItem.ItemId = requestItem.ItemId;
                purchaseItem.QtyRequested = requestItem.QtyRequested;
                purchaseItem.RequestRemark = requestItem.RequestRemark;

                var itemTemp = _context.Items.Where(item => item.ItemId == requestItem.ItemId).
                    Include(i => i.Equipment).
                    Include(i => i.Material).
                    FirstOrDefault();

                if (itemTemp == null) throw new KeyNotFoundException($"Purchase Item with Id {requestItem.ItemId} Not Found");

                if (itemTemp.Type != ITEMTYPE.MATERIAL) throw new InvalidOperationException($"Purchase Item with Id {requestItem.ItemId} Is Not Type of Material");

                purchaseItem.Cost = itemTemp.Material.Cost;
                purchaseItem.TotalCost = purchaseItem.QtyRequested * purchaseItem.Cost;
                purchase.TotalPurchaseCost += purchaseItem.TotalCost;
                
                purchaseItems.Add(purchaseItem);

                PurchaseItemEmployee purchaseItemEmployee = new();
                purchaseItemEmployee.ItemId = requestItem.ItemId;

                purchaseItemEmployee.RequestedById = _userService.Employee.EmployeeId; ;
                purchaseItemEmployee.QtyRequested = requestItem.QtyRequested;
                purchaseItemEmployee.RequestRemark = requestItem.RequestRemark;
                purchaseItemEmployees.Add(purchaseItemEmployee);

            }

            purchase.PurchaseItems = purchaseItems;
            purchase.PurchaseItemEmployees = purchaseItemEmployees;

            _context.Purchases.Add(purchase);

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.PURCHASE,
                status: purchase.Status,
                actionId: purchase.PurchaseId,
                siteId: purchase.ReceivingSiteId,
                employeeId: purchase.RequestedById);

            return purchase;
        }

        public async Task<Purchase> RequestEquipment(CreateEquipmentPurchaseDTO purchaseDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanRequestPurchase != 1) return Forbid();

            Purchase purchase = new();
            purchase.RequestedById = _userService.Employee.EmployeeId;
            purchase.ReceivingSiteId = purchaseDTO.ReceivingSiteId;

            ICollection<PurchaseItem> purchaseItems = new List<PurchaseItem>();
            ICollection<PurchaseItemEmployee> purchaseItemEmployees = new List<PurchaseItemEmployee>();

            foreach (var requestItem in purchaseDTO.PurchaseItems)
            {
                PurchaseItem purchaseItem = new();
                purchaseItem.ItemId = requestItem.ItemId;
                purchaseItem.QtyRequested = requestItem.QtyRequested;
                purchaseItem.RequestRemark = requestItem.RequestRemark;

                var itemTemp = _context.Items
                    .Where(item => item.ItemId == requestItem.ItemId)
                    .Include(i => i.Equipment)
                    .Include(i => i.Material)
                    .FirstOrDefault();

                if (itemTemp == null) throw new KeyNotFoundException($"Purchase Item with Id {requestItem.ItemId} Not Found");

                if (itemTemp.Type != ITEMTYPE.EQUIPMENT) throw new InvalidOperationException($"Purchase Item with Id {requestItem.ItemId} Is Not Type of Equipment");

                purchaseItem.EquipmentModelId = requestItem.EquipmentModelId;

                var purchaseEquipmentModel = await _context.EquipmentModels
                    .Where(equipModel => equipModel.EquipmentModelId == requestItem.EquipmentModelId)
                    .FirstOrDefaultAsync();

                if (purchaseEquipmentModel == null) throw new KeyNotFoundException($"Purchase Item Model with Id {requestItem.EquipmentModelId} Not Found");

                purchaseItem.Cost = purchaseEquipmentModel.Cost;
                purchaseItem.TotalCost = purchaseItem.QtyRequested * purchaseItem.Cost;
                purchase.TotalPurchaseCost += purchaseItem.TotalCost;

                purchaseItems.Add(purchaseItem);

                purchase.TotalPurchaseCost += purchaseItem.TotalCost;

                PurchaseItemEmployee purchaseItemEmployee = new();
                purchaseItemEmployee.ItemId = requestItem.ItemId;
                purchaseItemEmployee.EquipmentModelId = requestItem.EquipmentModelId;
                purchaseItemEmployee.RequestedById = _userService.Employee.EmployeeId; ;
                purchaseItemEmployee.QtyRequested = requestItem.QtyRequested;
                purchaseItemEmployee.RequestRemark = requestItem.RequestRemark;
                
                purchaseItemEmployees.Add(purchaseItemEmployee);

            }

            purchase.PurchaseItems = purchaseItems;
            purchase.PurchaseItemEmployees = purchaseItemEmployees;

            _context.Purchases.Add(purchase);

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.PURCHASE,
                status: purchase.Status,
                actionId: purchase.PurchaseId,
                siteId: purchase.ReceivingSiteId,
                employeeId: purchase.RequestedById);

            return purchase;
        }

        public async Task<Purchase> ApprovePurchase(ApprovePurchaseDTO approveDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanApprovePurchase != 1) return Forbid();

            var purchase = _context.Purchases
                 .Where(t => t.PurchaseId == approveDTO.PurchaseId)
                 .Include(t => t.PurchaseItems)
                 .FirstOrDefault();
            if (purchase == null) throw new KeyNotFoundException("Purchase Not Found.");

            purchase.ApproveDate = DateTime.Now;
            purchase.ApprovedById = _userService.Employee.EmployeeId;

            foreach (var requestItem in approveDTO.PurchaseItems)
            {
                var purchaseItem = purchase.PurchaseItems
                    .Where(purchaseItem => purchaseItem.ItemId == requestItem.ItemId && purchaseItem.EquipmentModelId == requestItem.EquipmentModelId)
                    .FirstOrDefault();

                if (purchaseItem == null) throw new KeyNotFoundException($"Purchase Item with Id {requestItem.ItemId} Not Found");

                purchaseItem.QtyApproved = requestItem.QtyApproved;
                purchaseItem.TotalCost = purchaseItem.Cost * purchaseItem.QtyApproved;
                purchaseItem.ApproveRemark = requestItem.ApproveRemark;

            }

            purchase.TotalPurchaseCost = calculateTotalCost(purchase.PurchaseItems); ;

            purchase.Status = PURCHASESTATUS.APPROVED;

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.PURCHASE,
                status: purchase.Status,
                actionId: purchase.PurchaseId,
                siteId: purchase.ReceivingSiteId,
                employeeId: purchase.RequestedById);

            return purchase;
        }

        public async Task<Purchase> DeclinePurchase(ApprovePurchaseDTO declineDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanApprovePurchase != 1) return Forbid();

            var purchase = _context.Purchases
                  .Where(t => t.PurchaseId == declineDTO.PurchaseId)
                  .Include(t => t.PurchaseItems)
                  .FirstOrDefault();

            if (purchase == null) throw new KeyNotFoundException("Purchase Not Found.");

            purchase.ApproveDate = DateTime.Now;
            purchase.ApprovedById = _userService.Employee.EmployeeId;
            purchase.TotalPurchaseCost = 0;

            foreach (var purchaseItem in purchase.PurchaseItems)
            {
                purchaseItem.QtyApproved = 0;

                purchaseItem.TotalCost = 0;
            }

            purchase.Status = PURCHASESTATUS.DECLINED;

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.PURCHASE,
                status: purchase.Status,
                actionId: purchase.PurchaseId,
                siteId: purchase.ReceivingSiteId,
                employeeId: purchase.RequestedById);

            return purchase;
        }
        
        public async Task<Purchase> CheckPurchase(CheckPurchaseDTO checkDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanCheckPurchase != 1) return Forbid();

            var purchase = await _context.Purchases.FindAsync(checkDTO.PurchaseId);

            if (purchase == null) throw new KeyNotFoundException("Purchase Not Found.");

            purchase.CheckDate = DateTime.Now;
            purchase.CheckedById = _userService.Employee.EmployeeId;
            purchase.Status = PURCHASESTATUS.CHECKED;

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.PURCHASE,
                status: purchase.Status,
                actionId: purchase.PurchaseId,
                siteId: purchase.ReceivingSiteId,
                employeeId: purchase.RequestedById);

            return purchase;
        }

        public async Task<Purchase> QueuePurchase(CheckPurchaseDTO checkDTO)
        {
            var purchase = _context.Purchases
               .Where(purchase => purchase.PurchaseId == checkDTO.PurchaseId)
               .Include(purchase => purchase.PurchaseItems)
               .Include(purchase => purchase.PurchaseItemEmployees)
               .FirstOrDefault();
            
            if (purchase == null) throw new KeyNotFoundException("Purchase Not Found");

            var bulkPurchase = _context.BulkPurchases
               .Where(bulk => bulk.Status == BULKPURCHASESTATUS.QUEUED)
               .Include(bulk => bulk.BulkPurchaseItems)
               .FirstOrDefault();

            ICollection<BulkPurchaseItem>? bulkPurchaseItems;

            if (bulkPurchase == null)
            {
                bulkPurchase = new BulkPurchase
                {
                    Status = BULKPURCHASESTATUS.QUEUED,
                    RequestedById = _userService.Employee.EmployeeId,
                };

                bulkPurchaseItems = new List<BulkPurchaseItem>();
                bulkPurchase.BulkPurchaseItems = bulkPurchaseItems;

            }
            else
            {
                bulkPurchaseItems = bulkPurchase.BulkPurchaseItems;
            }

            foreach (var purchaseItem in purchase.PurchaseItems)
            {
                var bulkPurchaseItem = bulkPurchase.BulkPurchaseItems
                    .Where(bItem => bItem.ItemId == purchaseItem.ItemId && bItem.EquipmentModelId == purchaseItem.EquipmentModelId)
                    .FirstOrDefault();

                if (bulkPurchaseItem != null)
                {
                    bulkPurchaseItem.QtyRequested += purchaseItem.QtyRequested;
                }
                else
                {
                    bulkPurchaseItem = new BulkPurchaseItem
                    {
                        ItemId = purchaseItem.ItemId,
                        QtyRequested = purchaseItem.QtyRequested,
                        Cost = purchaseItem.Cost,
                        EquipmentModelId = purchaseItem.EquipmentModelId
                    };
                }

                bulkPurchaseItem.TotalCost = bulkPurchaseItem.Cost * bulkPurchaseItem.QtyRequested;

                if (bulkPurchaseItem.QtyRequested == purchaseItem.QtyRequested)
                {
                    bulkPurchaseItems.Add(bulkPurchaseItem);
                }
            }

            bulkPurchase.BulkPurchaseItems = bulkPurchaseItems;

            purchase.CheckDate = DateTime.Now;
            purchase.CheckedById = _userService.Employee.EmployeeId;
            purchase.Status = PURCHASESTATUS.BULKQUEUED;

            if (bulkPurchase.BulkPurchaseId == 0)
            {
                _context.BulkPurchases.Add(bulkPurchase);
            }

            bulkPurchase.TotalPurchaseCost = calculateTotalCost(bulkPurchase.BulkPurchaseItems);

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.PURCHASE,
                status: purchase.Status,
                actionId: purchase.PurchaseId,
                siteId: purchase.ReceivingSiteId,
                employeeId: purchase.RequestedById);

            purchase.BulkPurchaseId = bulkPurchase.BulkPurchaseId;

            await _context.SaveChangesAsync();
            
            var centralSite = _context.Sites
                .FirstOrDefault();

            await _notificationService.Add(type: NOTIFICATIONTYPE.BULKPURCHASE,
                status: bulkPurchase.Status,
                actionId: bulkPurchase.BulkPurchaseId,
                siteId: centralSite.SiteId,
                employeeId: bulkPurchase.RequestedById);

            return purchase;
        }

        public async Task<Purchase> RequestQueuedPurchase(CheckPurchaseDTO checkDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanCheckPurchase != 1) return Forbid();

            var purchase = await _context.Purchases.FindAsync(checkDTO.PurchaseId);

            if (purchase == null) throw new KeyNotFoundException("Purchase Not Found.");

            purchase.Status = PURCHASESTATUS.REQUESTED;

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.PURCHASE,
                status: purchase.Status,
                actionId: purchase.PurchaseId,
                siteId: purchase.ReceivingSiteId,
                employeeId: purchase.RequestedById);

            return purchase;
        }

        public async Task<Purchase> ConfirmPurchase(ConfirmPurchaseDTO confirmDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanApprovePurchase != 1) return Forbid();

            var purchase = _context.Purchases
                 .Where(t => t.PurchaseId == confirmDTO.PurchaseId)
                 .Include(t => t.PurchaseItems)
                 .FirstOrDefault();

            if (purchase == null) throw new KeyNotFoundException("Purchase Not Found.");

            foreach (var requestItem in confirmDTO.PurchaseItems)
            {
                var purchaseItem = purchase.PurchaseItems
                    .Where(purchaseItem => purchaseItem.ItemId == requestItem.ItemId && purchaseItem.EquipmentModelId == requestItem.EquipmentModelId)
                    .FirstOrDefault();

                if (purchaseItem == null) throw new KeyNotFoundException($"Purchase Item with Id {requestItem.ItemId} Not Found");

                purchaseItem.QtyPurchased = requestItem.QtyPurchased;
                purchaseItem.PurchaseRemark = requestItem.PurchaseRemark;
                purchaseItem.TotalCost = purchaseItem.Cost * purchaseItem.QtyPurchased;

            }

            purchase.TotalPurchaseCost = calculateTotalCost(purchase.PurchaseItems); ;

            purchase.Status = PURCHASESTATUS.PURCHASED;
            purchase.PurchaseDate = DateTime.Now;
            
            await _context.SaveChangesAsync();

            //Add receive
            Receive receive = new();
            receive.PurchaseId = purchase.PurchaseId;
            receive.PurchaseDate = purchase.PurchaseDate;
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

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.PURCHASE,
                status: purchase.Status,
                actionId: purchase.PurchaseId,
                siteId: purchase.ReceivingSiteId,
                employeeId: purchase.RequestedById);

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.RECEIVE,
                status: receive.Status,
                actionId: receive.ReceiveId,
                siteId: receive.ReceivingSiteId,
                employeeId: receive.ReceivedById);

            return purchase;
        }

        private decimal calculateTotalCost (ICollection<PurchaseItem> Items)
        {
            var totalCost = (decimal)0;
            
            foreach (var purchaseItem in Items)
            {
                totalCost += purchaseItem.TotalCost;
            }

            return totalCost;
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
