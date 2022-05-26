using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models
{
    public class AssetDamage
    {
        [Key]
        public int AssetDamageId { get; set; }

        public string Name { get; set; }

        [Column(TypeName = "decimal(2, 2)")]
        public decimal PenalityPercentage { get; set; }

    }
}
