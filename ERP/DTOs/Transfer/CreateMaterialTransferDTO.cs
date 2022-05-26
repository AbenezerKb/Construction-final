namespace ERP.DTOs
{
    public class CreateMaterialTransferDTO
    {

        public int SendSiteId { get; set; }

        public int ReceiveSiteId { get; set; }

        public int RequestedById { get; set; }

        public ICollection<CreateMaterialTransferItem> TransferItems { get; set; }

    }

    public class CreateMaterialTransferItem
    {
        public int ItemId { get; set; }

        public int QtyRequested { get; set; }

        public string RequestRemark { get; set; } = string.Empty;

    }

}
