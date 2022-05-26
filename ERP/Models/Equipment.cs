using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models
{
    public class Equipment
    {
        [Key]
        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        public string Unit { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int EquipmentCategoryId { get; set; }

        public EquipmentCategory EquipmentCategory { get; set; }

        public ICollection<EquipmentModel> EquipmentModels { get; set; }
    }
}
