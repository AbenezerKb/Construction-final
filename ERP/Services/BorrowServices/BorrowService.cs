using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using ERP.Services.EquipmentAssetServices;
using ERP.Services.ItemSiteQtyServices;
using ERP.Services.NotificationServices;
using ERP.Services.User;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.BorrowServices
{
    public class BorrowService: IBorrowService
    {
        private readonly DataContext _context;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IItemSiteQtyService _itemSiteQtyService;
        private readonly IEquipmentAssetService _equipmentAssetService;

        public BorrowService(DataContext context, INotificationService notificationService, IUserService userService, IItemSiteQtyService itemSiteQtyService, IEquipmentAssetService equipmentAssetService)
        {
            _context = context;
            _notificationService = notificationService;
            _userService = userService;
            _itemSiteQtyService = itemSiteQtyService;
            _equipmentAssetService = equipmentAssetService;
        }


        public async Task<Borrow> GetById(int id)
        {
            var borrow = await _context.Borrows.Where(borrow => borrow.BorrowId == id)
                .Include(borrow => borrow.Site)
                .Include(borrow => borrow.RequestedBy)
                .Include(borrow => borrow.ApprovedBy)
                .Include(borrow => borrow.HandedBy)
               .Include(borrow => borrow.BorrowItems)
               .ThenInclude(borrowItem => borrowItem.Item)
               .ThenInclude(item => item.Equipment)
               .Include(borrow => borrow.BorrowItems)
               .ThenInclude(borrowItem => borrowItem.BorrowEquipmentAssets)
               .ThenInclude(borrowEquipmentAsset => borrowEquipmentAsset.EquipmentAsset)
               .FirstOrDefaultAsync();

            if (borrow == null) throw new KeyNotFoundException("Borrow Not Found.");

            return borrow;
        }

        public async Task<List<Borrow>> GetByCondition(GetBorrowsDTO getBorrowsDTO)
        {
            checkEmployeeSiteIsAvailable();
            UserRole userRole = _userService.UserRole;
            int employeeId = _userService.Employee.EmployeeId;
            int siteId = (int)_userService.Employee.EmployeeSiteId;

            var borrows = await _context.Borrows
                .Where(borrow => (
                    (/*getBorrowsDTO.SiteId == -1 ||*/ borrow.SiteId == siteId) &&
                    ((userRole.CanRequestBorrow == true && borrow.RequestedById == employeeId) ||
                    (userRole.CanApproveBorrow == true && borrow.Status == BORROWSTATUS.REQUESTED) ||
                    (userRole.CanHandBorrow == true && borrow.Status == BORROWSTATUS.APPROVED))))
                .Include(borrow => borrow.Site)
                .OrderByDescending(borrow => borrow.BorrowId)
                .ToListAsync();

            return borrows;
        }

        private void checkEmployeeSiteIsAvailable()
        {
            if (_userService.Employee.EmployeeSite == null)
                throw new InvalidOperationException("Borrowing Employee Does Not Have A Site");
        }

        public async Task<Borrow> RequestBorrow(CreateBorrowDTO borrowDTO)
        {
            checkEmployeeSiteIsAvailable();

            Borrow borrow = new();
            borrow.RequestedById = _userService.Employee.EmployeeId;
            borrow.SiteId = (int)_userService.Employee.EmployeeSiteId;

            ICollection<BorrowItem> borrowItems = new List<BorrowItem>();

            foreach (var requestItem in borrowDTO.BorrowItems)
            {
                BorrowItem borrowItem = new();
                borrowItem.ItemId = requestItem.ItemId;
                borrowItem.QtyRequested = requestItem.QtyRequested;
                borrowItem.RequestRemark = requestItem.RequestRemark;

                var itemTemp = _context.Items.Where(item => item.ItemId == requestItem.ItemId).
                   Include(i => i.Equipment).
                   FirstOrDefault();

                if (itemTemp == null) throw new KeyNotFoundException($"Borrow Item with Id {requestItem.ItemId} Not Found");

                if (itemTemp.Type != ITEMTYPE.EQUIPMENT) throw new InvalidOperationException($"Borrow Item with Id {requestItem.ItemId} Is Not Type of Equipment");

                borrowItem.EquipmentModelId = requestItem.EquipmentModelId;

                var borrowEquipmentModel = await _context.EquipmentModels
                    .Where(equipModel => equipModel.EquipmentModelId == requestItem.EquipmentModelId)
                    .FirstOrDefaultAsync();

                if (borrowEquipmentModel == null) throw new KeyNotFoundException($"Borrow Item Model with Id {requestItem.EquipmentModelId} Not Found");

                borrowItem.Cost = borrowEquipmentModel.Cost;

                borrowItems.Add(borrowItem);
            }

            borrow.BorrowItems = borrowItems;

            _context.Borrows.Add(borrow);
            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.BORROW,
                status: borrow.Status,
                actionId: borrow.BorrowId,
                siteId: borrow.SiteId,
                employeeId: borrow.RequestedById);

            return borrow; 
        }

        public async Task<Borrow> ApproveBorrow(ApproveBorrowDTO approveDTO)
        {
            var borrow = await _context.Borrows
               .Where(b => b.BorrowId == approveDTO.BorrowId)
               .Include(b => b.BorrowItems)
               .FirstOrDefaultAsync();
            if (borrow == null) throw new KeyNotFoundException("Borrow Not Found.");

            borrow.ApproveDate = DateTime.Now;
            borrow.ApprovedById = _userService.Employee.EmployeeId;

            foreach (var requestItem in approveDTO.BorrowItems)
            {
                var borrowItem = borrow.BorrowItems
                    .Where(bi => bi.ItemId == requestItem.ItemId && bi.EquipmentModelId == requestItem.EquipmentModelId)
                    .FirstOrDefault();

                if (borrowItem == null) throw new KeyNotFoundException($"Borrow Item with Id {requestItem.ItemId} and Model Id {requestItem.EquipmentModelId} Not Found");

                borrowItem.QtyApproved = requestItem.QtyApproved;
                borrowItem.ApproveRemark = requestItem.ApproveRemark;
            }

            borrow.Status = BORROWSTATUS.APPROVED;

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.BORROW,
                status: borrow.Status,
                actionId: borrow.BorrowId,
                siteId: borrow.SiteId,
                employeeId: borrow.RequestedById);

            return borrow;

        }


        public async Task<Borrow> DeclineBorrow(DeclineBorrowDTO declineDTO)
        {
            var borrow = await _context.Borrows
                 .Where(b => b.BorrowId == declineDTO.BorrowId)
                 .Include(b => b.BorrowItems)
                 .FirstOrDefaultAsync();
            if (borrow == null) throw new KeyNotFoundException("Borrow Not Found.");

            borrow.ApproveDate = DateTime.Now;
            borrow.ApprovedById = _userService.Employee.EmployeeId;

            foreach (var requestItem in declineDTO.BorrowItems)
            {
                var borrowItem = borrow.BorrowItems
                    .Where(bi => bi.ItemId == requestItem.ItemId && bi.EquipmentModelId == requestItem.EquipmentModelId)
                    .FirstOrDefault();

                if (borrowItem == null) throw new KeyNotFoundException($"Borrow Item with Id {requestItem.ItemId} and Model Id {requestItem.EquipmentModelId} Not Found");

                borrowItem.QtyApproved = requestItem.QtyApproved;
                borrowItem.ApproveRemark = requestItem.ApproveRemark;
            }

            borrow.Status = BORROWSTATUS.DECLINED;

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.BORROW,
                status: borrow.Status,
                actionId: borrow.BorrowId,
                siteId: borrow.SiteId,
                employeeId: borrow.RequestedById);

            return borrow;
        }

        public async Task<Borrow> HandBorrow(HandBorrowDTO handDTO)
        {

            var borrow = await _context.Borrows
                 .Where(b => b.BorrowId == handDTO.BorrowId)
                 .Include(b => b.BorrowItems)
                 .FirstOrDefaultAsync();
            if (borrow == null) throw new KeyNotFoundException("Borrow Not Found.");

            borrow.HandDate = DateTime.Now;
            borrow.HandedById = _userService.Employee.EmployeeId;

            foreach (var requestItem in handDTO.BorrowItems)
            {
                var borrowItem = borrow.BorrowItems
                     .Where(bi => bi.ItemId == requestItem.ItemId && bi.EquipmentModelId == requestItem.EquipmentModelId)
                     .FirstOrDefault();

                if (borrowItem == null)
                    throw new KeyNotFoundException($"Borrow Item with Id {requestItem.ItemId} and Model Id {requestItem.EquipmentModelId} Not Found");

                borrowItem.HandRemark = requestItem.HandRemark;

                if (requestItem.EquipmentAssetIds != null)
                {
                    borrowItem.BorrowEquipmentAssets = new List<BorrowItemEquipmentAsset>();

                    foreach (var requestAssetId in requestItem.EquipmentAssetIds)
                    {
                        BorrowItemEquipmentAsset equipmentAsset = new();
                        equipmentAsset.EquipmentAssetId = requestAssetId;

                        borrowItem.BorrowEquipmentAssets.Add(equipmentAsset);

                        await _equipmentAssetService.SetEmployee(requestAssetId, _userService.Employee.EmployeeId);
                    }
                }


                await _itemSiteQtyService.SubtractEquipmentModel(borrowItem.EquipmentModelId, borrow.SiteId, (int)borrowItem.QtyApproved);
            }

            borrow.Status = BORROWSTATUS.HANDED;

            await _context.SaveChangesAsync();

            await _equipmentAssetService.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.BORROW,
                status: borrow.Status,
                actionId: borrow.BorrowId,
                siteId: borrow.SiteId,
                employeeId: borrow.RequestedById);

            return borrow;
        }
    }
}
