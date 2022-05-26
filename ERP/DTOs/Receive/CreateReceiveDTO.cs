namespace ERP.DTOs
{
    public class CreateReceiveDTO
    {
        public int ReceiveId { get; set; }

        //public int DeliveredById { get; set; }

        public ICollection<CreateReceiveItem> ReceiveItems { get; set; }

    }

    public class CreateReceiveItem
    {
        public int ItemId { get; set; }

        public int EquipmentModelId { get; set; } = 0;

        public int QtyReceived { get; set; }

        public string ReceiveRemark { get; set; } = string.Empty;

    }
}
