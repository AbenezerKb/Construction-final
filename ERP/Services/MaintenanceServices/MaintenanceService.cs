using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using ERP.Services.NotificationServices;
using ERP.Services.User;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.MaintenanceServices
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        public MaintenanceService(DataContext context, IUserService userService, INotificationService notificationService)
        {
            _context = context;
            _userService = userService;
            _notificationService = notificationService;
        }

        public async Task<Maintenance> GetById(int id)
        {
            var maintenance = await _context.Maintenances.Where(maintenance => maintenance.MaintenanceId == id)
                .Include(maintenance => maintenance.Site)
                .Include(maintenance => maintenance.RequestedBy)
                .Include(maintenance => maintenance.ApprovedBy)
                .Include(maintenance => maintenance.FixedBy)
                .Include(maintenance => maintenance.Item)
                .ThenInclude(item => item.Equipment)
                .Include(maintenance => maintenance.EquipmentAsset)
                .FirstOrDefaultAsync();

            if (maintenance == null) throw new KeyNotFoundException("Maintenance Request Not Found.");

            return maintenance;
        }

        public async Task<List<EquipmentAsset>> GetItems(int modelId)
        {
            checkEmployeeSiteIsAvailable();
            UserRole userRole = _userService.UserRole;
            int siteId = (int)_userService.Employee.EmployeeSiteId;

            var items = await _context.EquipmentAssets
                .Where(ea => ea.EquipmentModelId == modelId && ea.AssetDamageId != null && ea.CurrentSiteId == siteId)
                .ToListAsync();

            return items;
        }

        public async Task<List<Maintenance>> GetByCondition()
        {
            checkEmployeeSiteIsAvailable();
            UserRole userRole = _userService.UserRole;
            int employeeId = _userService.Employee.EmployeeId;
            int siteId = (int)_userService.Employee.EmployeeSiteId;

            var borrows = await _context.Maintenances
                .Where(maintenance => (
                    ( maintenance.SiteId == siteId) &&
                    ((userRole.CanRequestMaintenance == true && maintenance.RequestedById == employeeId) ||
                    (userRole.CanApproveMaintenance == true && maintenance.Status == MAINTENANCESTATUS.REQUESTED) ||
                    (userRole.CanFixMaintenance == true && maintenance.Status == MAINTENANCESTATUS.APPROVED))))
                .Include(m => m.Site)
                .OrderByDescending(m => m.MaintenanceId)
                .ToListAsync();

            return borrows;
        }

        private void checkEmployeeSiteIsAvailable()
        {
            if (_userService.Employee.EmployeeSite == null)
                throw new InvalidOperationException("Borrowing Employee Does Not Have A Site");
        }

        public async Task<Maintenance> RequestMaintenance(CreateMaintenanceDTO maintenanceDTO)
        {
            checkEmployeeSiteIsAvailable();

            Maintenance maintenance = new();
            maintenance.RequestedById = _userService.Employee.EmployeeId;
            maintenance.SiteId = (int)_userService.Employee.EmployeeSiteId;
            maintenance.Reason = maintenanceDTO.Reason;

            var itemTemp = _context.Items.Where(item => item.ItemId == maintenanceDTO.ItemId).FirstOrDefault();

            if (itemTemp == null) throw new KeyNotFoundException($"Maintenance Item with Id {maintenanceDTO.ItemId} Not Found");

            if (itemTemp.Type != ITEMTYPE.EQUIPMENT)
                throw new InvalidOperationException($"Maintenance Item with Id {maintenanceDTO.ItemId} Is Not Type of Equipment");

            var maintenanceEquipmentModel = await _context.EquipmentModels
                .Where(em => em.EquipmentModelId == maintenanceDTO.EquipmentModelId && em.ItemId == maintenanceDTO.ItemId)
                .FirstOrDefaultAsync();

            if (maintenanceEquipmentModel == null)
                throw new KeyNotFoundException($"Maitenance Item with Id {maintenanceDTO.ItemId} and Model Id {maintenanceDTO.EquipmentModelId} Not Found");

            if (maintenanceDTO.EquipmentAssetId != null)
            {
                var maintenanceEquipmentAsset = await _context.EquipmentAssets
                    .Where(ea => ea.EquipmentAssetId == maintenanceDTO.EquipmentAssetId && ea.EquipmentModelId == maintenanceDTO.EquipmentModelId)
                    .FirstOrDefaultAsync();

                if (maintenanceEquipmentAsset == null)
                    throw new KeyNotFoundException($"Maitenance Item with Model Id {maintenanceDTO.EquipmentModelId} and Asset Id {maintenanceDTO.EquipmentAssetId} Not Found");

                maintenance.EquipmentAssetId = maintenanceDTO.EquipmentAssetId;
            }

            maintenance.Cost = maintenanceEquipmentModel.Cost;

            maintenance.ItemId = maintenanceDTO.ItemId;
            maintenance.EquipmentModelId = maintenanceDTO.EquipmentModelId;

            _context.Maintenances.Add(maintenance);
            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.MAINTENANCE,
                status: maintenance.Status,
                actionId: maintenance.MaintenanceId,
                siteId: maintenance.SiteId,
                employeeId: maintenance.RequestedById);

            return maintenance;
        }

        public async Task<Maintenance> ApproveMaintenance(ApproveMaintenanceDTO approveDTO)
        {

            var maintenance = await _context.Maintenances
                .Where(m => m.MaintenanceId == approveDTO.MaintenanceId)
                .FirstOrDefaultAsync();
            if (maintenance == null) throw new KeyNotFoundException("Maintenance Request Not Found.");

            maintenance.ApproveDate = DateTime.Now;
            maintenance.ApprovedById = _userService.Employee.EmployeeId;
            maintenance.ApproveRemark = approveDTO.ApproveRemark;

            maintenance.Status = MAINTENANCESTATUS.APPROVED;

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.MAINTENANCE,
                status: maintenance.Status,
                actionId: maintenance.MaintenanceId,
                siteId: maintenance.SiteId,
                employeeId: maintenance.RequestedById);

            return maintenance;
        }

        public async Task<Maintenance> DeclineMaintenance(ApproveMaintenanceDTO declineDTO)
        {
            var maintenance = await _context.Maintenances
                .Where(m => m.MaintenanceId == declineDTO.MaintenanceId)
                .FirstOrDefaultAsync();
            if (maintenance == null) throw new KeyNotFoundException("Maintenance Request Not Found.");

            maintenance.ApproveDate = DateTime.Now;
            maintenance.ApprovedById = _userService.Employee.EmployeeId;
            maintenance.ApproveRemark = declineDTO.ApproveRemark;

            maintenance.Status = MAINTENANCESTATUS.DECLINED;

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.MAINTENANCE,
                status: maintenance.Status,
                actionId: maintenance.MaintenanceId,
                siteId: maintenance.SiteId,
                employeeId: maintenance.RequestedById);

            return maintenance;
        }

        public async Task<Maintenance> FixMaintenance(FixMaintenanceDTO fixDTO)
        {
            var maintenance = await _context.Maintenances
                .Where(m => m.MaintenanceId == fixDTO.MaintenanceId)
                .Include(m=>m.EquipmentAsset)
                .FirstOrDefaultAsync();
            if (maintenance == null) throw new KeyNotFoundException("Maintenance Request Not Found.");


            maintenance.FixDate = DateTime.Now;
            maintenance.FixedById = _userService.Employee.EmployeeId;
            maintenance.FixRemark = fixDTO.FixRemark;
            maintenance.EquipmentAsset.AssetDamageId = null;

            maintenance.Status = MAINTENANCESTATUS.FIXED;

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.MAINTENANCE,
                status: maintenance.Status,
                actionId: maintenance.MaintenanceId,
                siteId: maintenance.SiteId,
                employeeId: maintenance.RequestedById);

            return maintenance;

        }
    }
}
