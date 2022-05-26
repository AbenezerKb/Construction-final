namespace ERP.DTOs
{
    public class CreateBorrowDTO
    {

        public int RequestedById { get; set; }

        public int SiteId { get; set; }

        public ICollection<CreateBorrowItem> BorrowItems { get; set; }
    }

    public class CreateBorrowItem
    {
        public int ItemId { get; set; }

        public int EquipmentModelId { get; set; }

        public int QtyRequested { get; set; }

        public string RequestRemark { get; set; } = string.Empty;


    }
}
