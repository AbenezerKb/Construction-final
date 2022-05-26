using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP.Models
{
    public class BuyItem
    {
        [Key]
        public int BuyId { get; set; }

        [Key]
        public int ItemId { get; set; }

        public int QtyRequested { get; set; } = 0;

        public int QtyApproved { get; set; } = 0;

        public int QtyBought { get; set; } = 0;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Cost { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalCost { get; set; }

        public string? RequestRemark { get; set; }

        public string? ApproveRemark { get; set; }

        public string? BuyRemark { get; set; }

        [JsonIgnore]
        [ForeignKey("BuyId")]
        public Buy Buy { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }
    }
}
