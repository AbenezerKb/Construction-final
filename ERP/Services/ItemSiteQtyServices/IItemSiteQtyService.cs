using ERP.Models;

namespace ERP.Services.ItemSiteQtyServices
{
    public interface IItemSiteQtyService
    {
        Task<EquipmentSiteQty> AddEquipmentModel(int modelId, int siteId, int qty);
        Task<MaterialSiteQty> AddMaterial(int itemId, int siteId, int qty);
        Task<EquipmentSiteQty> SubtractEquipmentModel(int modelId, int siteId, int qty);
        Task<MaterialSiteQty> SubtractMaterial(int itemId, int siteId, int qty);
    }
}
