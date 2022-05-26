namespace ERP.DTOs
{
    public class ApproveTransferDTO
    {
        public int TransferId { get; set; }

        public int ApprovedById { get; set; }

        public ICollection<ApproveTransferItems> TransferItems { get; set; }
    }

    public class ApproveTransferItems
    {
        public int ItemId { get; set; }

        public int EquipmentModelId { get; set; } = 0;

        public int QtyApproved { get; set; }

        public string ApproveRemark { get; set; }
    }
}
