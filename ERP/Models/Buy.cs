using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP.Models
{
    public class Buy
    {
        [Key]
        public int BuyId { get; set; }

        public int Status { get; set; } = BUYSTATUS.REQUESTED;

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public Employee RequestedBy { get; set; }

        public int RequestedById { get; set; }

        public Site BuySite { get; set; }

        public int BuySiteId { get; set; }

        public DateTime? CheckDate { get; set; }

        public Employee CheckedBy { get; set; }

        public int? CheckedById { get; set; }

        public DateTime? ApproveDate { get; set; }

        public Employee ApprovedBy { get; set; }

        public int? ApprovedById { get; set; }

        public DateTime? BuyDate { get; set; }

        public Purchase Purchase { get; set; }

        public int? PurchaseId { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalBuyCost { get; set; } = 0;

        public ICollection<BuyItem> BuyItems { get; set; }
   
    }
}
