namespace ERP.DTOs
{
    public class ReceiveTransferDTO
    {
        public int TransferId { get; set; }

        public int ReceivedById { get; set; }

        public string DeliveredBy { get; set; }

        public string VehiclePlateNo { get; set; } = string.Empty;

        public ICollection<ReceiveTransferItems> TransferItems { get; set; }
    }

    public class ReceiveTransferItems
    {
        public int ItemId { get; set; }

        public int EquipmentModelId { get; set; } = 0;

        public string ReceiveRemark { get; set; }
    }
}
