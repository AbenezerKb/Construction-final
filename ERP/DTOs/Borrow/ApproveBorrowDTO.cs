namespace ERP.DTOs
{
    public class ApproveBorrowDTO
    {
        public int BorrowId { get; set; }

        public int ApprovedById { get; set; }

        public ICollection<ApproveBorrowItems> BorrowItems { get; set; }
    }

    public class ApproveBorrowItems
    {
        public int ItemId { get; set; }

        public int EquipmentModelId { get; set; }

        public int QtyApproved { get; set; }

        public string ApproveRemark { get; set; }
    }
}
