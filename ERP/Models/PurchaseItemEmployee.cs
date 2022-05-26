using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP.Models
{
    public class PurchaseItemEmployee
    {
        [Key]
        public int PurchaseId { get; set; }

        [Key]
        public int ItemId { get; set; }

        [Key]
        public int EquipmentModelId { get; set; } = 0;

        [Key]
        public int RequestedById { get; set; }

        public int QtyRequested { get; set; } = 0;

        public string? RequestRemark { get; set; }

        [JsonIgnore]
        [ForeignKey("PurchaseId")]
        public Purchase Purchase { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        [ForeignKey("RequestedById")]
        public Employee Employee { get; set; }
    }
}
