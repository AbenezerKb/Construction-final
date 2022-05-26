using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class Transfer
    {
        [Key]
        public int TransferId { get; set; }

        public int Status { get; set; } = TRANSFERSTATUS.REQUESTED;

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public DateTime? SendDate { get; set; }

        public DateTime? ApproveDate { get; set; }

        public DateTime? ReceiveDate { get; set; }

        public Site SendSite { get; set; }

        public int SendSiteId { get; set; }

        public Site ReceiveSite { get; set; }

        public int ReceiveSiteId { get; set; }

        public Employee RequestedBy { get; set; }

        public int RequestedById { get; set; }

        public Employee ReceivedBy { get; set; }

        public int? ReceivedById { get; set; }

        public string? DeliveredBy { get; set; }

        public Employee SentBy { get; set; }

        public int? SentById { get; set; }

        public string? VehiclePlateNo { get; set; }

        public virtual Employee ApprovedBy { get; set; }

        public int? ApprovedById { get; set; }

        public ICollection<TransferItem> TransferItems { get; set; }
    }
}
