namespace ERP.DTOs.Buy
{
    public class CreateBuyDTO
    {
        public ICollection<CreateBuyItem> BuyItems { get; set; }

    }

    public class CreateBuyItem
    {
        public int ItemId { get; set; }

        public int QtyRequested { get; set; }

        public string RequestRemark { get; set; } = string.Empty;
    }
}
