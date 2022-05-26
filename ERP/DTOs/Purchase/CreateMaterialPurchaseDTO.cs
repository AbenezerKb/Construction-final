namespace ERP.DTOs
{
    public class CreateMaterialPurchaseDTO
    {
        public int ReceivingSiteId { get; set; }

        public ICollection<CreateMaterialPurchaseItem> PurchaseItems { get; set; }

    }

    public class CreateMaterialPurchaseItem
    {
        public int ItemId { get; set; }

        public int QtyRequested { get; set; }

        public string RequestRemark { get; set; } = string.Empty;

    }
}
