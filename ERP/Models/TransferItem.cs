using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP.Models
{
    public class TransferItem
    {
        [Key]
        public int TransferId { get; set; }

        [Key]
        public int ItemId { get; set; }

        [Key]
        public int EquipmentModelId { get; set; } = 0;

        public int QtyRequested { get; set; }

        public int? QtyApproved { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Cost { get; set; }

        public string RequestRemark { get; set; } = string.Empty;

        public string? ApproveRemark { get; set; }

        public string? SendRemark { get; set; }

        public string? ReceiveRemark { get; set; }

        [JsonIgnore]
        [ForeignKey("TransferId")]
        public Transfer Transfer { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        public ICollection<TransferItemEquipmentAsset> TransferEquipmentAssets { get; set; }


    }
}
