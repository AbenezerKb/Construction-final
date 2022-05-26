using ERP.Context;
using ERP.DTOs;
using ERP.DTOs.Receive;
using ERP.Models;
using ERP.Services.AssetNumberServices;
using ERP.Services.ItemSiteQtyServices;
using ERP.Services.NotificationServices;
using ERP.Services.User;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.ReceiveServices
{
    public class ReceiveService : IReceiveService
    {
        private readonly DataContext _context;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IAssetNumberService _assetNumberService;
        private readonly IItemSiteQtyService _itemSiteQtyService;

        public ReceiveService(DataContext context, INotificationService notificationService, IUserService userService, IAssetNumberService assetNumberService, IItemSiteQtyService itemSiteQtyService)
        {
            _context = context;
            _notificationService = notificationService;
            _userService = userService;
            _assetNumberService = assetNumberService;
            _itemSiteQtyService = itemSiteQtyService;
        }

        public async Task<Receive> GetById(int id)
        {
            //if (UserAccount != null && UserAccount.UserRole.CanViewPurchase != 1) return Forbid();

            var receive = _context.Receives.Where(receive => receive.ReceiveId == id)
               .Include(receive => receive.ReceivedBy)
               .Include(receive => receive.ApprovedBy)
               .Include(receive => receive.DeliveredBy)
               .Include(receive => receive.ReceiveItems)
               .ThenInclude(receiveItem => receiveItem.Item)
               .ThenInclude(item => item.Material)
               .Include(receive => receive.ReceiveItems)
               .ThenInclude(receiveItem => receiveItem.Item.Equipment)
               .FirstOrDefault();

            if (receive == null) throw new KeyNotFoundException("Receive Not Found.");

            return receive;
        }

        public async Task<List<Receive>> GetMySite()
        {
            //if (UserAccount != null && UserAccount.UserRole.CanViewPurchase != 1) return Forbid();

            var receives = await _context.Receives
              .Where(receive => receive.ReceivingSiteId == (int)_userService.Employee.EmployeeSiteId && receive.Status == RECEIVESTATUS.PURCHASED)
              .Include(receive => receive.ReceiveItems)
              .ThenInclude(receiveItem => receiveItem.Item)
              .ThenInclude(item => item.Material)
              .Include(receive => receive.ReceiveItems)
              .ThenInclude(receiveItem => receiveItem.Item.Equipment)
              .OrderByDescending(receive => receive.ReceiveId)
              .ToListAsync();

            if (receives == null) throw new KeyNotFoundException("Receive Not Found.");

            return receives;
        }

        public async Task<List<Receive>> GetByCondition(GetReceivesDTO getReceivesDTO)
        {
            UserRole userRole = _userService.UserRole;
            int employeeId = _userService.Employee.EmployeeId;

            var receives = await _context.Receives
                .Where(receive => (
                    (getReceivesDTO.ReceivingSiteId == -1 || receive.ReceivingSiteId == getReceivesDTO.ReceivingSiteId) &&
                    ((userRole.CanViewReceive == true && receive.Status == RECEIVESTATUS.APPROVED) ||
                    (userRole.CanReceive == true && receive.Status == RECEIVESTATUS.PURCHASED) ||
                    (userRole.CanApproveReceive == true && receive.Status == RECEIVESTATUS.RECEIVED))))
                .Include(receive => receive.ReceivingSite)
                .OrderByDescending(receive => receive.ReceiveId)
                .ToListAsync();

            return receives;
        }

        public async Task<Receive> ReceiveItem(CreateReceiveDTO receiveDTO)
        {

            var receive = _context.Receives
                 .Where(r => r.ReceiveId == receiveDTO.ReceiveId)
                 .Include(r => r.ReceiveItems)
                 .FirstOrDefault();

            if (receive == null) throw new KeyNotFoundException("Receive Not Found.");

            receive.ReceiveDate = DateTime.Now;
            //receive.DeliveredById = receiveDTO.DeliveredById;
            receive.ReceivedById = _userService.Employee.EmployeeId;
            ICollection<ReceiveItem> receiveItems = new List<ReceiveItem>();

            foreach (var requestItem in receiveDTO.ReceiveItems)
            {
                var receiveItem = receive.ReceiveItems
                    .Where(rItem => rItem.ItemId == requestItem.ItemId && rItem.EquipmentModelId == requestItem.EquipmentModelId)
                    .FirstOrDefault();

                if (receiveItem == null) throw new KeyNotFoundException($"Receive Item with Id {requestItem.ItemId} Not Found");

                receiveItem.QtyReceived = requestItem.QtyReceived;
                receiveItem.ReceiveRemark = requestItem.ReceiveRemark;
                
                var itemTemp = _context.Items
                    .Where(item => item.ItemId == requestItem.ItemId)
                    .FirstOrDefault();

                if (itemTemp == null) throw new KeyNotFoundException($"Item with Id {requestItem.ItemId} Not Found");


                if (itemTemp.Type == ITEMTYPE.EQUIPMENT)
                {
                    await _itemSiteQtyService.AddEquipmentModel(requestItem.EquipmentModelId, (int)_userService.Employee.EmployeeSiteId, requestItem.QtyReceived);
                    
                    List<string> assetNumbers = await _assetNumberService.GenerateAssetNumbers(requestItem.ItemId, receiveItem.QtyReceived);
                    
                    foreach (var assetNumber in assetNumbers)
                    {
                        EquipmentAsset equipmentAsset = new();
                        equipmentAsset.EquipmentModelId = requestItem.EquipmentModelId;
                        equipmentAsset.AssetNo = assetNumber;
                        equipmentAsset.CurrentSiteId = (int)_userService.Employee.EmployeeSiteId;

                        _context.EquipmentAssets.Add(equipmentAsset);
                    }

                    await _context.SaveChangesAsync();

                }
                else
                {
                    await _itemSiteQtyService.AddMaterial(requestItem.ItemId, (int)_userService.Employee.EmployeeSiteId, requestItem.QtyReceived);
                }
                
                receiveItems.Add(receiveItem);

            }

            receive.Status = RECEIVESTATUS.RECEIVED;

            foreach (var buy in _context.Buys
                .Where(buy => buy.PurchaseId == receive.PurchaseId && buy.Status == BUYSTATUS.QUEUED))
            {
                buy.Status = BUYSTATUS.BOUGHT;

                await _notificationService.Add(type: NOTIFICATIONTYPE.BUY,
                    status: buy.Status,
                    actionId: buy.BuyId,
                    siteId: buy.BuySiteId,
                    employeeId: buy.RequestedById);
            }

            await _context.SaveChangesAsync();

            

            await _notificationService.Add(type: NOTIFICATIONTYPE.RECEIVE,
                status: receive.Status,
                actionId: receive.ReceiveId,
                siteId: receive.ReceivingSiteId,
                employeeId: receive.ReceivedById);

            return receive;
        }

        public async Task<Receive> ApproveReceive(ApproveReceiveDTO approveDTO)
        {
            var receive = _context.Receives
                .Where(receive => receive.ReceiveId == approveDTO.ReceiveId)
                .FirstOrDefault();

            if (receive == null) throw new KeyNotFoundException("Receive Not Found.");

            receive.ApproveDate = DateTime.Now;
            receive.ApprovedById = _userService.Employee.EmployeeId;
            receive.Status = RECEIVESTATUS.APPROVED;

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.RECEIVE,
                status: receive.Status,
                actionId: receive.ReceiveId,
                siteId: receive.ReceivingSiteId,
                employeeId: receive.ReceivedById);

            return receive;
        }
    }
}
