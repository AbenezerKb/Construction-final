using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP.Models
{
    public class Purchase
    {
        [Key]
        public int PurchaseId { get; set; }

        public int Status { get; set; } = PURCHASESTATUS.REQUESTED;

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public Employee RequestedBy { get; set; }

        public int RequestedById { get; set; }

        public DateTime? CheckDate { get; set; }

        public Employee CheckedBy { get; set; }

        public int? CheckedById { get; set; }

        public DateTime? ApproveDate { get; set; }

        public Employee ApprovedBy { get; set; }

        public int? ApprovedById { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public Site ReceivingSite { get; set; }

        public int ReceivingSiteId { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalPurchaseCost { get; set; } = 0;

        public BulkPurchase BulkPurchase { get; set; }

        public int? BulkPurchaseId { get; set; }

        public ICollection<PurchaseItem> PurchaseItems { get; set; }
        
        public ICollection<PurchaseItemEmployee> PurchaseItemEmployees { get; set; }
    
    }
}
