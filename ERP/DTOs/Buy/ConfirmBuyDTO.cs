namespace ERP.DTOs.Buy
{
    public class ConfirmBuyDTO
    {
        public int BuyId { get; set; }

        public ICollection<ConfirmBuyItems> BuyItems { get; set; }

    }

    public class ConfirmBuyItems
    {
        public int ItemId { get; set; }

        public int QtyBought { get; set; }

        public string BuyRemark { get; set; }
    }
}
