using ERP.Context;
using ERP.Models;
using ERP.Services.NotificationServices;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.ItemSiteQtyServices
{
    public class ItemSiteQtyService : IItemSiteQtyService
    {
        private readonly DataContext _context;
        private readonly INotificationService _notificationService;

        public ItemSiteQtyService(DataContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<EquipmentSiteQty> AddEquipmentModel(int modelId, int siteId, int qty)
        {
            var equipmentSiteQty = _context.EquipmentSiteQties
                .Where(esq => esq.EquipmentModelId == modelId && esq.SiteId == siteId)
                .FirstOrDefault();

            if (equipmentSiteQty == null) throw new KeyNotFoundException("Equipment Site Qty Entry Not Found");

            equipmentSiteQty.Qty += qty;

            _context.SaveChanges();

            if(equipmentSiteQty.Qty > equipmentSiteQty.MinimumQty)
                await _notificationService.Clear(NOTIFICATIONTYPE.MINEQUIPMENT, modelId);

            return equipmentSiteQty;
                
        }

        public async Task<MaterialSiteQty> AddMaterial(int itemId, int siteId, int qty)
        {
            var materialSiteQty = _context.MaterialSiteQties
                .Where(msq => msq.ItemId == itemId && msq.SiteId == siteId)
                .FirstOrDefault();

            if (materialSiteQty == null) throw new KeyNotFoundException("Material Site Qty Entry Not Found");

            materialSiteQty.Qty += qty;

            _context.SaveChanges();

            if (materialSiteQty.Qty > materialSiteQty.MinimumQty)
                await _notificationService.Clear(NOTIFICATIONTYPE.MINMATERIAL, itemId);

            return materialSiteQty;

        }

        public async Task<EquipmentSiteQty> SubtractEquipmentModel(int modelId, int siteId, int qty)
        {
            var equipmentSiteQty = _context.EquipmentSiteQties
                .Where(esq => esq.EquipmentModelId == modelId && esq.SiteId == siteId)
                .FirstOrDefault();

            if (equipmentSiteQty == null) throw new KeyNotFoundException("Equipment Site Qty Entry Not Found");

            
            equipmentSiteQty.Qty -= equipmentSiteQty.Qty >= qty ? qty : equipmentSiteQty.Qty;

            _context.SaveChanges();

            if (equipmentSiteQty.Qty < equipmentSiteQty.MinimumQty)
                await _notificationService.Add(NOTIFICATIONTYPE.MINEQUIPMENT, 0, modelId, siteId, null);

            return equipmentSiteQty;

        }

        public async Task<MaterialSiteQty> SubtractMaterial(int itemId, int siteId, int qty)
        {
            var materialSiteQty = _context.MaterialSiteQties
                .Where(msq => msq.ItemId == itemId && msq.SiteId == siteId)
                .FirstOrDefault();

            if (materialSiteQty == null) throw new KeyNotFoundException("Material Site Qty Entry Not Found");

            materialSiteQty.Qty -= materialSiteQty.Qty >= qty ? qty : materialSiteQty.Qty;

            _context.SaveChanges();

            if (materialSiteQty.Qty < materialSiteQty.MinimumQty)
                await _notificationService.Add(NOTIFICATIONTYPE.MINMATERIAL, 0, itemId, siteId, null);

            return materialSiteQty;

        }

    }
}
