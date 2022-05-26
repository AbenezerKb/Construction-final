namespace ERP.DTOs.BulkPurchase
{
    public class ConfirmBulkPurchaseDTO
    {
        public int BulkPurchaseId { get; set; }

        public ICollection<ConfirmBulkPurchaseItem> BulkPurchaseItems { get; set; }

    }

    public class ConfirmBulkPurchaseItem
    {
        public int ItemId { get; set; }

        public int EquipmentModelId { get; set; } = 0;

        public int QtyPurchased { get; set; }

        public string PurchaseRemark { get; set; } = string.Empty;

    }
}
