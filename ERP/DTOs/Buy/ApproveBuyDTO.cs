namespace ERP.DTOs.Buy
{
    public class ApproveBuyDTO
    {
        public int BuyId { get; set; }

        public ICollection<ApproveBuyItems> BuyItems { get; set; }

    }

    public class ApproveBuyItems
    {
        public int ItemId { get; set; }

        public int QtyApproved { get; set; }

        public string ApproveRemark { get; set; }
    }
}
