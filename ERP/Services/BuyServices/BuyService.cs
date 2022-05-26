using ERP.Context;
using ERP.DTOs;
using ERP.DTOs.Buy;
using ERP.DTOs.Purchase;
using ERP.Models;
using ERP.Services.ItemSiteQtyServices;
using ERP.Services.NotificationServices;
using ERP.Services.User;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.BuyServices
{
    public class BuyService : IBuyService
    {
        private readonly DataContext _context;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IItemSiteQtyService _itemSiteQtyService;

        public BuyService(DataContext context, INotificationService notificationService, IUserService userService, IItemSiteQtyService itemSiteQtyService)
        {
            _context = context;
            _notificationService = notificationService;
            _userService = userService;
            _itemSiteQtyService = itemSiteQtyService;
        }

        public async Task<Buy> GetById(int id)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanViewbuy != 1) return Forbid();

            var buy = _context.Buys.Where(buy => buy.BuyId == id)
               .Include(buy => buy.RequestedBy)
               .Include(buy => buy.CheckedBy)
               .Include(buy => buy.ApprovedBy)
               .Include(buy => buy.BuySite)
               .Include(buy => buy.BuyItems)
               .ThenInclude(buyItem => buyItem.Item)
               .ThenInclude(item => item.Material)
               .Include(buy => buy.BuyItems)
               .ThenInclude(buyItem => buyItem.Item.Equipment)
               .FirstOrDefault();

            if (buy == null) throw new KeyNotFoundException("Buy Not Found.");

            return buy;
        }

        public async Task<List<Buy>> GetByCondition()
        {
            UserRole userRole = _userService.UserRole;
            int employeeId = _userService.Employee.EmployeeId;
            int employeeSiteId = (int)_userService.Employee.EmployeeSiteId;

            var buys = await _context.Buys
                .Where(buy => (
                    (buy.BuySiteId == employeeSiteId) &&
                    ((userRole.CanViewBuy == true && (buy.Status == BUYSTATUS.DECLINED ||
                    buy.Status == BUYSTATUS.QUEUED || buy.Status == BUYSTATUS.BOUGHT)) ||
                    (userRole.CanCheckBuy == true && buy.Status == BUYSTATUS.REQUESTED) ||
                    (userRole.CanApproveBuy == true && buy.Status == BUYSTATUS.CHECKED) ||
                    (userRole.CanConfirmBuy == true && buy.Status == BUYSTATUS.APPROVED))))
                .Include(buy => buy.BuySite)
                .OrderByDescending(buy => buy.BuyId)
                .ToListAsync();

            return buys;
        }

        public async Task<Buy> RequestBuy(CreateBuyDTO buyDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanRequestBuy != 1) return Forbid();

            Buy buy = new();
            buy.RequestedById = _userService.Employee.EmployeeId;
            buy.BuySiteId = (int)_userService.Employee.EmployeeSiteId;

            ICollection<BuyItem> buyItems = new List<BuyItem>();

            foreach (var requestItem in buyDTO.BuyItems)
            {
                BuyItem buyItem = new();
                buyItem.ItemId = requestItem.ItemId;
                buyItem.QtyRequested = requestItem.QtyRequested;
                buyItem.RequestRemark = requestItem.RequestRemark;

                var itemTemp = _context.Items.Where(item => item.ItemId == requestItem.ItemId).
                    Include(i => i.Equipment).
                    Include(i => i.Material).
                    FirstOrDefault();

                if (itemTemp == null) throw new KeyNotFoundException($"Buy Item with Id {requestItem.ItemId} Not Found");

                if (itemTemp.Type != ITEMTYPE.MATERIAL) throw new InvalidOperationException($"Buy Item with Id {requestItem.ItemId} Is Not Type of Material");

                buyItem.Cost = itemTemp.Material.Cost;
                buyItem.TotalCost = itemTemp.Material.Cost * buyItem.QtyRequested;

                buyItems.Add(buyItem);

                buy.TotalBuyCost += buyItem.TotalCost;
            }

            buy.BuyItems = buyItems;

            _context.Buys.Add(buy);
            
            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.BUY,
                status: buy.Status,
                actionId: buy.BuyId,
                siteId: buy.BuySiteId,
                employeeId: buy.RequestedById);

            return buy;
        }

        public async Task<Buy> CheckBuy(CheckBuyDTO checkDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanCheckBuy != 1) return Forbid();

            var buy = await _context.Buys.FindAsync(checkDTO.BuyId);

            if (buy == null) throw new KeyNotFoundException("Buy Not Found.");

            buy.CheckDate = DateTime.Now;
            buy.CheckedById = _userService.Employee.EmployeeId;
            buy.Status = BUYSTATUS.CHECKED;

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.BUY,
                status: buy.Status,
                actionId: buy.BuyId,
                siteId: buy.BuySiteId,
                employeeId: buy.RequestedById);

            return buy;
        }

        public async Task<Buy> QueueBuy(QueueBuyDTO queueDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanCheckBuy != 1) return Forbid();

            var buy = _context.Buys.Where(buy => buy.BuyId == queueDTO.BuyId)
                .Include(buy => buy.BuyItems)
                .ThenInclude(buyItem => buyItem.Item)
                .FirstOrDefault();

            if (buy == null) throw new KeyNotFoundException("Buy Not Found.");

            var purchase = _context.Purchases
               .Where(purchase => purchase.Status == PURCHASESTATUS.QUEUED && purchase.ReceivingSiteId == (int)_userService.Employee.EmployeeSiteId)
               .Include(purchase => purchase.PurchaseItems)
               .Include(purchase => purchase.PurchaseItemEmployees)
               .FirstOrDefault();

            ICollection<PurchaseItem>? purchaseItems;
            ICollection<PurchaseItemEmployee>? purchaseItemEmployees;

            if (purchase == null)
            {
                purchase = new Purchase
                {
                    Status = PURCHASESTATUS.QUEUED,
                    RequestedById = _userService.Employee.EmployeeId,
                    ReceivingSiteId = (int)_userService.Employee.EmployeeSiteId
                };

                purchaseItems = new List<PurchaseItem>();
                purchase.PurchaseItems = purchaseItems;

                purchaseItemEmployees = new List<PurchaseItemEmployee>();
                purchase.PurchaseItemEmployees = purchaseItemEmployees;
            }
            else
            {
                purchaseItems = purchase.PurchaseItems;
                purchaseItemEmployees = purchase.PurchaseItemEmployees;
            }

            foreach (var buyItem in buy.BuyItems)
            {
                var purchaseItemEmployee = purchase.PurchaseItemEmployees
                    .Where(pItem => pItem.PurchaseId == purchase.PurchaseId && pItem.ItemId == buyItem.ItemId && pItem.RequestedById == buy.RequestedById)
                    .FirstOrDefault();

                if (purchaseItemEmployee != null)
                {
                    purchaseItemEmployee.QtyRequested += buyItem.QtyRequested;
                }
                else
                {
                    purchaseItemEmployee = new PurchaseItemEmployee
                    {
                        ItemId = buyItem.ItemId,
                        RequestedById = buy.RequestedById,
                        QtyRequested = buyItem.QtyRequested,
                        RequestRemark = buyItem.RequestRemark
                    };
                }

                if (purchaseItemEmployee.QtyRequested == buyItem.QtyRequested)
                {
                    purchaseItemEmployees.Add(purchaseItemEmployee);

                }

                var purchaseItem = purchase.PurchaseItems
                    .Where(pItem => pItem.PurchaseId == purchase.PurchaseId && pItem.ItemId == buyItem.ItemId)
                    .FirstOrDefault();

                if (purchaseItem != null)
                {
                    purchaseItem.QtyRequested += buyItem.QtyRequested;
                }
                else
                {
                    purchaseItem = new PurchaseItem
                    {
                        ItemId = buyItem.ItemId,
                        QtyRequested = buyItem.QtyRequested,
                        Cost = buyItem.Cost
                    };
                }

                purchaseItem.TotalCost = purchaseItem.Cost * purchaseItem.QtyRequested;

                if (purchaseItem.QtyRequested == buyItem.QtyRequested)
                {
                    purchaseItems.Add(purchaseItem);
                }
            }

            purchase.PurchaseItems = purchaseItems;
            purchase.PurchaseItemEmployees = purchaseItemEmployees;

            buy.CheckDate = DateTime.Now;
            buy.CheckedById = _userService.Employee.EmployeeId;
            buy.Status = BUYSTATUS.QUEUED;

            if (purchase.PurchaseId == 0)
            {
                _context.Purchases.Add(purchase);
            }

            purchase.TotalPurchaseCost = calculateTotalCost(purchase.PurchaseItems);

            await _context.SaveChangesAsync();

            buy.PurchaseId = purchase.PurchaseId;

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.BUY,
                status: buy.Status,
                actionId: buy.BuyId,
                siteId: buy.BuySiteId,
                employeeId: buy.RequestedById);

            await _notificationService.Add(type: NOTIFICATIONTYPE.PURCHASE,
                status: purchase.Status,
                actionId: purchase.PurchaseId,
                siteId: purchase.ReceivingSiteId,
                employeeId: purchase.RequestedById);

            return buy;
        }

        public async Task<Buy> ApproveBuy(ApproveBuyDTO approveDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanApproveBuy != 1) return Forbid();

            var buy = _context.Buys
                 .Where(b => b.BuyId == approveDTO.BuyId)
                 .Include(b => b.BuyItems)
                 .ThenInclude(bItem => bItem.Item)
                 .FirstOrDefault();
            if (buy == null) throw new KeyNotFoundException("Buy Not Found.");

            buy.ApproveDate = DateTime.Now;
            buy.ApprovedById = _userService.Employee.EmployeeId;

            foreach (var requestItem in approveDTO.BuyItems)
            {
                var buyItem = buy.BuyItems.Where(b => b.ItemId == requestItem.ItemId).FirstOrDefault();

                if (buyItem == null) throw new KeyNotFoundException($"Buy Item with Id {requestItem.ItemId} Not Found");

                buyItem.QtyApproved = requestItem.QtyApproved;
                buyItem.ApproveRemark = requestItem.ApproveRemark;

                buyItem.TotalCost = buyItem.Cost * buyItem.QtyApproved;
            }

            buy.TotalBuyCost = 0;

            foreach (var purchaseItem in buy.BuyItems)
            {
                buy.TotalBuyCost += purchaseItem.TotalCost;
            }

            buy.Status = BUYSTATUS.APPROVED;

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.BUY,
                status: buy.Status,
                actionId: buy.BuyId,
                siteId: buy.BuySiteId,
                employeeId: buy.RequestedById);

            return buy;
        }

        public async Task<Buy> DeclineBuy(ApproveBuyDTO declineDTO)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanApprovePurchase != 1) return Forbid();

            var buy = _context.Buys
                .Where(b => b.BuyId == declineDTO.BuyId)
                .Include(b => b.BuyItems)
                .ThenInclude(bItem => bItem.Item)
                .FirstOrDefault();

            if (buy == null) throw new KeyNotFoundException("Buy Not Found.");

            buy.ApproveDate = DateTime.Now;
            buy.ApprovedById = _userService.Employee.EmployeeId;

            foreach (var buyItem in buy.BuyItems)
            {
                buyItem.QtyApproved = 0;

                buyItem.TotalCost = 0;
            }

            buy.Status = BUYSTATUS.DECLINED;

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.BUY,
                status: buy.Status,
                actionId: buy.BuyId,
                siteId: buy.BuySiteId,
                employeeId: buy.RequestedById);

            return buy;
        }

        public async Task<Buy> ConfirmBuy(ConfirmBuyDTO confirmDTO)
        {
            var buy = _context.Buys
                 .Where(b => b.BuyId == confirmDTO.BuyId)
                 .Include(b => b.BuyItems)
                 .ThenInclude(bItem => bItem.Item)
                 .FirstOrDefault();
            if (buy == null) throw new KeyNotFoundException("Buy Not Found.");

            buy.BuyDate = DateTime.Now;

            foreach (var requestItem in confirmDTO.BuyItems)
            {
                var buyItem = buy.BuyItems.Where(b => b.ItemId == requestItem.ItemId).FirstOrDefault();

                if (buyItem == null) throw new KeyNotFoundException($"Buy Item with Id {requestItem.ItemId} Not Found");

                buyItem.QtyBought = requestItem.QtyBought;
                buyItem.BuyRemark = requestItem.BuyRemark;

                buyItem.TotalCost = buyItem.Cost * buyItem.QtyBought;
            }

            buy.TotalBuyCost = 0;

            foreach (var purchaseItem in buy.BuyItems)
            {
                buy.TotalBuyCost += purchaseItem.TotalCost;
            }

            buy.Status = BUYSTATUS.BOUGHT;

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.BUY,
                status: buy.Status,
                actionId: buy.BuyId,
                siteId: buy.BuySiteId,
                employeeId: buy.RequestedById);

            return buy;
        }


        private decimal calculateTotalCost(ICollection<PurchaseItem> Items)
        {
            var totalCost = (decimal)0;

            foreach (var purchaseItem in Items)
            {
                totalCost += purchaseItem.TotalCost;
            }

            return totalCost;
        }
    }
}
