using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models
{
    public class BulkPurchase
    {
        [Key]
        public int BulkPurchaseId { get; set; }

        public int Status { get; set; } = PURCHASESTATUS.REQUESTED;

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public Employee RequestedBy { get; set; }

        public int RequestedById { get; set; }

        public DateTime? ApproveDate { get; set; }

        public Employee ApprovedBy { get; set; }

        public int? ApprovedById { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalPurchaseCost { get; set; } = 0;

        public ICollection<BulkPurchaseItem> BulkPurchaseItems { get; set; }

    }
}
