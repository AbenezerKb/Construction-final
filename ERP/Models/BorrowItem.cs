using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP.Models
{
    public class BorrowItem
    {
        [Key]
        public int BorrowId { get; set; }

        [Key]
        public int ItemId { get; set; }

        [Key]
        public int EquipmentModelId { get; set; } = 0;

        public int QtyRequested { get; set; }

        public int? QtyApproved { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Cost { get; set; }

        public string RequestRemark { get; set; } = string.Empty;

        public string? ApproveRemark { get; set; } = string.Empty;

        public string? HandRemark { get; set; } = string.Empty;

        [JsonIgnore]
        [ForeignKey("BorrowId")]
        public Borrow Borrow { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        public ICollection<BorrowItemEquipmentAsset> BorrowEquipmentAssets { get; set; }
    }
}
