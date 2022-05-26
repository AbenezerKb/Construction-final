namespace ERP.DTOs.BulkPurchase
{
    public class ApproveBulkPurchaseDTO
    {
        public int BulkPurchaseId { get; set; }

        public ICollection<ApproveBulkPurchaseItem> BulkPurchaseItems { get; set; }

    }

    public class ApproveBulkPurchaseItem
    {
        public int ItemId { get; set; }

        public int EquipmentModelId { get; set; } = 0;

        public int QtyApproved { get; set; }

        public string ApproveRemark { get; set; } = string.Empty;

    }
}
