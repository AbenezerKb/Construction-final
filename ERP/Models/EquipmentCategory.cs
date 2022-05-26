using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class EquipmentCategory
    {
        [Key]
        public int EquipmentCategoryId { get; set; }

        public string Name { get; set; }

        public string? FileName { get; set; }

        public ICollection<Equipment> Equipments { get; set; }

    }
}
