namespace ERP.DTOs
{
    public class CreateEquipmentPurchaseDTO
    {
        public int ReceivingSiteId { get; set; }

        public ICollection<CreateEquipmentPurchaseItem> PurchaseItems { get; set; }

    }

    public class CreateEquipmentPurchaseItem
    {
        public int ItemId { get; set; }

        public int EquipmentModelId { get; set; }

        public int QtyRequested { get; set; }

        public string RequestRemark { get; set; } = string.Empty;

    }
}
