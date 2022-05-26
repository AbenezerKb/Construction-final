using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP.Models
{

    public class Materials
    {
        [Key]
        public int ItemId { get; set; }
        
        [JsonIgnore]
        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        public string Spec { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public bool IsTransferable { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Cost { get; set; }

    }
}
