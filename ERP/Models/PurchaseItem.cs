using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP.Models
{
    public class PurchaseItem
    {
        [Key]
        public int PurchaseId { get; set; }

        [Key]
        public int ItemId { get; set; }

        [Key]
        public int EquipmentModelId { get; set; } = 0;

        public int QtyRequested { get; set; } = 0;

        public int QtyApproved { get; set; } = 0;

        public int QtyPurchased { get; set; } = 0;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Cost { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalCost { get; set; }

        public string? RequestRemark { get; set; }

        public string? ApproveRemark { get; set; }

        public string? PurchaseRemark { get; set; }

        [JsonIgnore]
        [ForeignKey("PurchaseId")]
        public Purchase Purchase { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

    }
}
