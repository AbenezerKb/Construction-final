using ERP.Models;

namespace ERP.Services.EquipmentAssetServices
{
    public interface IEquipmentAssetService
    {
        Task<EquipmentAsset> FixAsset(int equipmentAssetId);
        Task<EquipmentAsset> ReturnToSite(int equipmentAssetId, int siteId, int? assetDamageId);
        Task SaveChangesAsync();
        Task<EquipmentAsset> SetEmployee(int equipmentAssetId, int employeeId);
        Task<EquipmentAsset> SetSite(int equipmentAssetId, int siteId);
    }
}
