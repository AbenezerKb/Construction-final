namespace ERP.DTOs
{
    public class DeclineTransferDTO
    {
        public int TransferId { get; set; }

        public int ApprovedById { get; set; }


        public ICollection<DeclineTransferItems> TransferItems { get; set; }
    }

    public class DeclineTransferItems
    {
        public int ItemId { get; set; }

        public int EquipmentModelId { get; set; } = 0;

        public int QtyApproved { get; set; }

        public string ApproveRemark { get; set; }

    }
}
