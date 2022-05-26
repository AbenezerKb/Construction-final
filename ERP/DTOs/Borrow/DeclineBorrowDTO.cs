namespace ERP.DTOs
{
    public class DeclineBorrowDTO
    {
        public int BorrowId { get; set; }

        public int ApprovedById { get; set; }

        public ICollection<DeclineBorrowItems> BorrowItems { get; set; }
    }

    public class DeclineBorrowItems
    {
        public int ItemId { get; set; }

        public int EquipmentModelId { get; set; }

        public int QtyApproved { get; set; }

        public string ApproveRemark { get; set; }
    }
}
