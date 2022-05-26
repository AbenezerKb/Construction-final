using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using ERP.Services.EquipmentAssetServices;
using ERP.Services.FileServices;
using ERP.Services.ItemSiteQtyServices;
using ERP.Services.NotificationServices;
using ERP.Services.User;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.ReturnServices
{
    public class ReturnService : IReturnService
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IItemSiteQtyService _itemSiteQtyService;
        private readonly IEquipmentAssetService _equipmentAssetService;
        private readonly IFileService _fileService;

        public ReturnService(DataContext context, IUserService userService, INotificationService notificationService, IItemSiteQtyService itemSiteQtyService, IEquipmentAssetService equipmentAssetService, IFileService fileService)
        {
            _context = context;
            _userService = userService;
            _notificationService = notificationService;
            _itemSiteQtyService = itemSiteQtyService;
            _equipmentAssetService = equipmentAssetService;
            _fileService = fileService;
        }

        public async Task<List<Return>> GetByCondition()
        {
            checkEmployeeSiteIsAvailable();
            UserRole userRole = _userService.UserRole;
            int employeeId = _userService.Employee.EmployeeId;
            int siteId = (int)_userService.Employee.EmployeeSiteId;

            var returnResult = await _context.Returns
                .Where(ret => ret.SiteId == siteId && 
                ((userRole.CanReturnBorrow == true) ||
                (userRole.CanRequestBorrow == true && ret.ReturnEquipmentAssets.Any(rea => rea.Borrow.RequestedById == employeeId))))
                .Include(ret => ret.ReturnEquipmentAssets)
                .ThenInclude(rea => rea.Borrow)
                .Include(ret=> ret.Site)
                .OrderByDescending(ret => ret.ReturnId)
                .ToListAsync();
                       
            return returnResult;
        }

        public async Task<Return> GetById(int id){

            var returnResult = await _context.Returns.Where(ret => ret.ReturnId == id)
                .Include(ret => ret.ReturnedBy)
                .Include(ret => ret.ReturnEquipmentAssets)
                .ThenInclude(asset => asset.EquipmentAsset)
                .ThenInclude(ea => ea.EquipmentModel)
                .ThenInclude(em => em.Equipment)
                .ThenInclude(equipment => equipment.Item)
                .Include(ret => ret.ReturnEquipmentAssets)
                .ThenInclude(ea => ea.Borrow.RequestedBy)
                .Include(ret => ret.ReturnEquipmentAssets)
                .ThenInclude(ea => ea.AssetDamage)
                .Include(ret => ret.Site)
                .FirstOrDefaultAsync();

            if (returnResult == null) throw new KeyNotFoundException("Return Not Found.");

            return returnResult;
        }
        private void checkEmployeeSiteIsAvailable()
        {
            if (_userService.Employee.EmployeeSite == null)
                throw new InvalidOperationException("Borrowing Employee Does Not Have A Site");
        }

        public async Task<Return> ReturnEquipments(ReturnBorrowDTO returnDTO)
        {
            checkEmployeeSiteIsAvailable();

            List<BorrowItemEquipmentAsset> equipmentAssets = new();

            if (returnDTO.BorrowAssets == null || returnDTO.BorrowAssets.Count == 0)
                throw new InvalidOperationException("No Assets Provided to Return.");

            foreach (var requestAsset in returnDTO.BorrowAssets)
            {
                var asset = await _context.BorrowItemEquipmentAssets
                    .Where(ea => ea.ItemId == requestAsset.ItemId &&
                        ea.EquipmentModelId == requestAsset.EquipmentModelId &&
                        ea.EquipmentAssetId == requestAsset.EquipmentAssetId &&
                        ea.ReturnId == null)
                    .Include(ea => ea.Borrow)
                    .FirstOrDefaultAsync();

                if (asset == null)
                    throw new KeyNotFoundException($"Borrow Item with Id {requestAsset.ItemId},  Model Id {requestAsset.EquipmentModelId}, Asset Id {requestAsset.EquipmentAssetId} Not Found In Borrowed Equiments");

                asset.ReturnRemark = requestAsset.ReturnRemark;
                asset.AssetDamageId = requestAsset.AssetDamageId == -1 ? null : requestAsset.AssetDamageId;
                asset.FileName = requestAsset.FileName == "" ? null : requestAsset.FileName;
                                    
                equipmentAssets.Add(asset);

                await _itemSiteQtyService.AddEquipmentModel(asset.EquipmentModelId, (int)_userService.Employee.EmployeeSiteId, 1);
                await _equipmentAssetService.ReturnToSite(asset.EquipmentAssetId, (int)_userService.Employee.EmployeeSiteId, asset.AssetDamageId);
                
            }

            Return borrowReturn = new();
            borrowReturn.ReturnedById = _userService.Employee.EmployeeId;
            borrowReturn.SiteId = (int)_userService.Employee.EmployeeSiteId;

            borrowReturn.ReturnEquipmentAssets = equipmentAssets;

            _context.Returns.Add(borrowReturn);
            await _context.SaveChangesAsync();
            await _equipmentAssetService.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.RETURN,
                status: 0,
                actionId:borrowReturn.ReturnId,
                siteId: borrowReturn.SiteId,
                employeeId: equipmentAssets.First().Borrow.RequestedById);

            return borrowReturn;
        }


        public async Task<List<EquipmentAsset>> GetReturnableItems(GetReturnItemsDTO returnItemsDTO)
        {
            List<BorrowItemEquipmentAsset> borrowAssets = await _context.BorrowItemEquipmentAssets
                    .Where(ea => ea.Borrow.RequestedById == returnItemsDTO.RequestedById
                            && (returnItemsDTO.ItemId == null || ea.ItemId == returnItemsDTO.ItemId)
                            && ea.ReturnId == null)
                    .Include(ea => ea.EquipmentAsset.EquipmentModel.Equipment)
                    .ToListAsync();

            List<EquipmentAsset> assets = new();

            foreach (var borrowAsset in borrowAssets)
                assets.Add(borrowAsset.EquipmentAsset);

            return assets;
        }

        public async Task<List<AssetDamage>> GetDamageTypes()
        {
            var damages = await _context.AssetDamages.ToListAsync();

            return damages;
        }
    }
}
