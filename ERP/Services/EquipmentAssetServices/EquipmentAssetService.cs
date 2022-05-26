using ERP.Context;
using ERP.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.EquipmentAssetServices
{
    public class EquipmentAssetService : IEquipmentAssetService
    {
        private readonly DataContext _context;

        public EquipmentAssetService(DataContext context)
        {
            _context = context;
        }

        public async Task<EquipmentAsset> ReturnToSite(int equipmentAssetId, int siteId, int? assetDamageId)
        {
            var equipmentAsset = await SetSite(equipmentAssetId, siteId);

            equipmentAsset.AssetDamageId = assetDamageId;
            
            return equipmentAsset;
        }

        public async Task<EquipmentAsset> FixAsset(int equipmentAssetId)
        {
            var equipmentAsset = await _context.EquipmentAssets
                .Where(ea => ea.EquipmentAssetId == equipmentAssetId)
                .Include(ea => ea.CurrentSite)
                .Include(ea => ea.CurrentEmployee)
                .FirstOrDefaultAsync();


            if (equipmentAsset == null)
                throw new KeyNotFoundException("Cannot Find Equipment Asset");

            equipmentAsset.AssetDamageId = null;


            return equipmentAsset;
        }

        public async Task<List<EquipmentAsset>> GetBySite(int siteId)
        {
            var equipmentAssets = await _context.EquipmentAssets
                .Where(ea => ea.CurrentSiteId == siteId)
                .Include(ea => ea.CurrentSite)
                .Include(ea => ea.CurrentEmployee)
                .ToListAsync();

            return equipmentAssets;
        }

        public async Task<List<EquipmentAsset>> GetByEmployee(int employeeId)
        {
            var equipmentAssets = await _context.EquipmentAssets
                .Where(ea => ea.CurrentEmployeeId == employeeId)
                .Include(ea => ea.CurrentSite)
                .Include(ea => ea.CurrentEmployee)
                .ToListAsync();

            return equipmentAssets;
        }

        public async Task<EquipmentAsset> SetSite(int equipmentAssetId, int siteId)
        {
            var equipmentAsset = await _context.EquipmentAssets
                .Where(ea => ea.EquipmentAssetId == equipmentAssetId)
                .FirstOrDefaultAsync();

            if (equipmentAsset == null)
                throw new KeyNotFoundException("Cannot Find Equipment Asset");

            equipmentAsset.CurrentSiteId = siteId;
            equipmentAsset.CurrentEmployeeId = null;

            return equipmentAsset;
        }

        public async Task<EquipmentAsset> SetEmployee(int equipmentAssetId, int employeeId)
        {
            var equipmentAsset = await _context.EquipmentAssets
                .Where(ea => ea.EquipmentAssetId == equipmentAssetId)
                .FirstOrDefaultAsync();

            if (equipmentAsset == null)
                throw new KeyNotFoundException("Cannot Find Equipment Asset");

            equipmentAsset.CurrentSiteId = null;
            equipmentAsset.CurrentEmployeeId = employeeId;

            return equipmentAsset;
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
