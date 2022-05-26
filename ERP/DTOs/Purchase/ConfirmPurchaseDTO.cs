namespace ERP.DTOs.Purchase
{
    public class ConfirmPurchaseDTO
    {
        public int PurchaseId { get; set; }

        public ICollection<ConfirmPurchaseItem> PurchaseItems { get; set; }

    }

    public class ConfirmPurchaseItem
    {
        public int ItemId { get; set; }

        public int EquipmentModelId { get; set; } = 0;

        public int QtyPurchased { get; set; }

        public string PurchaseRemark { get; set; } = string.Empty;

    }
}