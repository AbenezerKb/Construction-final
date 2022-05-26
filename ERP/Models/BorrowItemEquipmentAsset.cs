using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models
{
    public class BorrowItemEquipmentAsset
    {
        [Key]
        public int BorrowId { get; set; }

        [Key]
        public int ItemId { get; set; }

        [Key]
        public int EquipmentModelId { get; set; }

        [Key]
        public int EquipmentAssetId { get; set; }

        public int? AssetDamageId { get; set; }

        public AssetDamage AssetDamage { get; set; }

        public string? ReturnRemark { get; set; } 
               
        public int? ReturnId { get; set; }

        public string? FileName { get; set; }

        public Return Return { get; set; }

        [ForeignKey("BorrowId, ItemId, EquipmentModelId")]
        public BorrowItem BorrowItem { get; set; }

        [ForeignKey("BorrowId")]
        public Borrow Borrow { get; set; }

        public EquipmentAsset EquipmentAsset { get; set; }
    }
}
