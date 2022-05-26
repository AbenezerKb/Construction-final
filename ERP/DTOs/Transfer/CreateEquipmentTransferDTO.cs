namespace ERP.DTOs
{
    public class CreateEquipmentTransferDTO
    {

        public int SendSiteId { get; set; }

        public int ReceiveSiteId { get; set; }

        public ICollection<CreateEquipmentTransferItem> TransferItems { get; set; }

    }

    public class CreateEquipmentTransferItem
    {
        public int ItemId { get; set; }

        public int QtyRequested { get; set; }

        public int EquipmentModelId { get; set; }

        public string RequestRemark { get; set; } = string.Empty;

    }

}
