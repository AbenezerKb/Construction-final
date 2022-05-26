using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models
{
    public class TransferItemEquipmentAsset
    {
        [Key]
        public int TransferId { get; set; }

        [Key]
        public int ItemId { get; set; }

        [Key]
        public int EquipmentModelId { get; set; }

        [Key]
        public int EquipmentAssetId { get; set; }

        [ForeignKey("TransferId, ItemId, EquipmentModelId")]
        public TransferItem TransferItem { get; set; }

        public EquipmentAsset EquipmentAsset { get; set; }
    }
}
