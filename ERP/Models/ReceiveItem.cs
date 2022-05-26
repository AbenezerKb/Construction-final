using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP.Models
{
    public class ReceiveItem
    {
        [Key]
        public int ReceiveId { get; set; }

        [Key]
        public int ItemId { get; set; }

        [Key]
        public int EquipmentModelId { get; set; } = 0;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Cost { get; set; }

        public int QtyReceived { get; set; } = 0;

        public string? ReceiveRemark { get; set; }

        [JsonIgnore]
        [ForeignKey("ReceiveId")]
        public Receive Receive { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }
    }
}
