namespace ERP.DTOs
{
    public class ApprovePurchaseDTO
    {
        public int PurchaseId { get; set; }

        public ICollection<ApprovePurchaseItem> PurchaseItems { get; set; }

    }

    public class ApprovePurchaseItem
    {
        public int ItemId { get; set; }

        public int EquipmentModelId { get; set; } = 0;

        public int QtyApproved { get; set; }

        public string ApproveRemark { get; set; } = string.Empty;

    }
}